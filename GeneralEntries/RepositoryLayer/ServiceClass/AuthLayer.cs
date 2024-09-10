using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using GeneralEntries.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GeneralEntries.RepositoryLayer.ServiceClass;

public class AuthLayer : IAuthLayer
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthLayer> _logger;
    private readonly IConfiguration _configuration;
    private readonly DbContextClass _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthLayer(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
        RoleManager<IdentityRole> roleManager, DbContextClass applicationDbContext,
        ILogger<AuthLayer> logger,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _context = applicationDbContext;

    }

    public async Task<TokenViewModel> Auth(LoginViewModel model)
    {
        TokenViewModel _TokenViewModel = new();

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                _TokenViewModel.Status = false;
                _TokenViewModel.Message = "Invalid Email";
                return _TokenViewModel;

            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _TokenViewModel.Status = false;
                _TokenViewModel.Message = "Invalid Password";
                return _TokenViewModel;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                  new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tempUserClaims = _userManager.GetClaimsAsync(user).Result.ToList();
            var Claims = new List<UserClaimDto>();
            foreach (var claim in tempUserClaims)
            {
                Claims.Add(new UserClaimDto() { ClaimType = claim.Type, ClaimValue = claim.Value });
            }

            _TokenViewModel.AccessToken = GenerateNewToken(authClaims, Claims);
            _TokenViewModel.RefreshToken = GenerateRefreshToken();
            _TokenViewModel.Status = true;
            _TokenViewModel.Message = "Success";

            var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWTKey:RefreshTokenValidityInDays"]);
            user.RefreshToken = _TokenViewModel.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _TokenViewModel.Status = false;
            _TokenViewModel.Message = ex.Message;
        }

        return _TokenViewModel;
    }

    public async Task<ServiceResponse<RegisterDto>> RegisterUser(RegisterViewModel model)
    {
        var response = new ServiceResponse<RegisterDto>();

        try
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                response.Status = false;
                response.Message = "Email already exists.";
                return response;
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    response.Message = error.Description;
                }
                response.Status = false;
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.Role) || model.Role == RolesViewModel.User || model.Role == "string")
            {
                if (await _roleManager.RoleExistsAsync("User"))
                    await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Role,
                        NormalizedName = model.Role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });

                await _userManager.AddToRoleAsync(user, model.Role);
            }

            response.Status = true;
            response.Message = $"New User {user.UserName} has been created and Role Assign {model.Role}";
            response.Value = model.Adapt<RegisterDto>();

        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<LoginDto>> LoginModel(LoginViewModel login)
    {
        var response = new ServiceResponse<LoginDto>();

        try
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                response.Status = false;
                response.Message = "Invalid username";
                return response;
            }

            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                response.Status = false;
                response.Message = "Invalid password";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // Generate token
            string token = GenerateToken(authClaims);

            // Create LoginDto object
            var loginDto = new LoginDto
            {
                Token = token,
            };

            response.Value = loginDto;
            response.Status = true;
            response.Message = "Successfully logged in";
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();

        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                response.Status = false;
                response.Message = "Invalid username";
                return response;

            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                response.Status = false;
                response.Message = "Invalid password";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                  new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            //string token = GenerateToken(authClaims);

            response.Value = GenerateToken(authClaims);
            response.Status = true;
            response.Message = "Successfully Login";

        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ServiceResponse<string>> ChangePasswordAsync(ChangePasswordViewModel model, string email)
    {
        var status = new ServiceResponse<string>();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            status.Message = "Email does not exist";
            status.Status = false;
            return status;
        }
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            status.Message = "Password has updated successfully";
            status.Status = true;
        }
        else
        {
            status.Message = "Some error occcured";
            status.Status = false;
        }
        return status;
    }
    private string GenerateNewToken(List<Claim> claims, List<UserClaimDto> userClaims)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWTKey:Secret"]));
        var _TokenExpiryinMinutes = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInMinutes"]);

        var roleClaims = userClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue));
        claims.AddRange(roleClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWTKey:ValidIssuer"],
            Audience = _configuration["JWTKey:ValidAudience"],
            Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryinMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims),
            //NotBefore = DateTime.UtcNow.AddMinutes(1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
        // var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
        var _TokenExpiryinMinutes = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInMinutes"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWTKey:ValidIssuer"],
            Audience = _configuration["JWTKey:ValidAudience"],
            //  Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
            Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryinMinutes),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim (ClaimTypes.Email, user.Email),
        };

        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingsToken is null)
            throw new Exception("AppSettings Token is null!");

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(appSettingsToken));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model)
    {
        TokenViewModel _TokenViewModel = new();
        var principal = GetPrincipalFromExpiredToken(model.AccessToken);
        string username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            _TokenViewModel.Status = false;
            _TokenViewModel.Message = "Invalid access token or refresh token";
            return _TokenViewModel;
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var tempUserClaims = _userManager.GetClaimsAsync(user).Result.ToList();
        var Claims = new List<UserClaimDto>();
        foreach (var claim in tempUserClaims)
        {
            Claims.Add(new UserClaimDto() { ClaimType = claim.Type, ClaimValue = claim.Value });
        }

        var newAccessToken = GenerateNewToken(authClaims, Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        _TokenViewModel.Status = true;
        _TokenViewModel.Message = "Success";
        _TokenViewModel.AccessToken = newAccessToken;
        _TokenViewModel.RefreshToken = newRefreshToken;
        return _TokenViewModel;
    }

    //private string GenerateTokenRefresh(IEnumerable<Claim> claims)
    //{
    //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
    //    var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Issuer = _configuration["JWTKey:ValidIssuer"],
    //        Audience = _configuration["JWTKey:ValidAudience"],
    //        //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
    //        Expires = DateTime.UtcNow.AddMinutes(30),
    //        SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
    //        Subject = new ClaimsIdentity(claims)
    //    };

    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {

        var key = Encoding.ASCII.GetBytes(_configuration["JWTKey:Secret"]);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateAudience = true,
            ValidIssuer = _configuration["JWTKey:ValidIssuer"],
            ValidAudience = _configuration["JWTKey:ValidAudience"],
            RequireExpirationTime = true,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
