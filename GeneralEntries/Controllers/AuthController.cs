using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using GeneralEntries.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GeneralEntries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthLayer _authService;
        private readonly DbContextClass _context;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthController> logger,
            DbContextClass context,
            IAuthLayer authService,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _authService = authService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("Authentication")]
        public async Task<ActionResult> Authentication([FromBody] LoginViewModel model)
        {
            try
            {
                var response = await _authService.Auth(model);

                if (!response.Status)
                {
                    // return Ok(response);
                    return BadRequest(response);
                }
                //  return Ok(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("NewUserRegister")]
        public async Task<ActionResult<ServiceResponse<RegisterDto>>> NewUserRegister(RegisterViewModel model)
        {
            try
            {
                var response = await _authService.RegisterUser(model);

                if (!response.Status)
                {
                    return BadRequest(response);
                }
                //  return Ok(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginDTO")]
        public async Task<ActionResult<ServiceResponse<LoginDto>>> AuthenticationUser(LoginViewModel model)
        {
            try
            {
                var response = await _authService.LoginModel(model);

                if (!response.Status)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginResponse")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginUser(LoginViewModel model)
        {
            try
            {
                var response = await _authService.Login(model.Email, model.Password);

                if (!response.Status)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(GetRefreshTokenViewModel model)
        {
            try
            {
                if (model is null)
                {
                    return BadRequest("Invalid client request");
                }

                var result = await _authService.GetRefreshToken(model);
                if (result.Status == false)
                    return BadRequest(result.Message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return Ok("Success");
        }

        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
            return Ok("Success");
        }

        [HttpGet]
        [Route("LogOut")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(model, model.Email);
                if (!result.Status)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1
                var roleResult = await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return Ok(new { result = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return BadRequest(new { error = $"Issue adding the new {roleName} role" });
                }
            }

            return BadRequest(new { error = "Role already exist" });
        }

        // Get all users
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        // Add User to role
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return Ok(new { result = $"User {user.Email} added to the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to add user {user.Email} to the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        // Get specific user role
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            // Resolve the user via their email
            var user = await _userManager.FindByEmailAsync(email);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // Remove User to role
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} removed from the {roleName} role");
                    return Ok(new { result = $"User {user.Email} removed from the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to removed user {user.Email} from the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to removed user {user.Email} from the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        [HttpGet]
        [Route("GetAllClaims")]
        public async Task<IActionResult> GetAllClaims(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(user);

            return Ok(claims);
        }

        // Add Claim to user
        [HttpPost]
        [Route("AddClaimToUser")]
        public async Task<IActionResult> AddClaimToUser(string email, string claimName, string value)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var userClaim = new Claim(claimName, value);

            if (user != null)
            {
                var result = await _userManager.AddClaimAsync(user, userClaim);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"the claim {claimName} add to the  User {user.Email}");
                    return Ok(new { result = $"the claim {claimName} add to the  User {user.Email}" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add the claim {claimName} to the  User {user.Email}");
                    return BadRequest(new { error = $"Error: Unable to add the claim {claimName} to the  User {user.Email}" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        [HttpPost]
        [Route("RemoveClaims")]
        public async Task<IActionResult> RemoveClaims(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(user);

            if (user != null)
            {
                var result = await _userManager.RemoveClaimsAsync(user, claims);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} removed claims");
                    return Ok(new { result = $"User {user.Email} removed claims" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to removed user {user.Email}");
                    return BadRequest(new { error = $"Error: Unable to removed user {user.Email}" });
                }
            }
            return BadRequest(new { error = "Unable to find user" });
        }

        [HttpPost]
        [Route("RemoveClaim")]
        public async Task<IActionResult> RemoveClaim(string email, string claimType, string claimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "Unable to find user" });
            }

            var claims = await _userManager.GetClaimsAsync(user);

            var claimToRemove = claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);
            if (claimToRemove != null)
            {
                var result = await _userManager.RemoveClaimAsync(user, claimToRemove);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} removed claim");
                    return Ok(new { result = $"User {user.Email} removed claim" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to removed user {user.Email}");
                    return BadRequest(new { error = $"Error: Unable to removed user {user.Email}" });
                }
            }
            return BadRequest(new { error = "Claim not found" });
        }

        //

        //[HttpGet]
        //[Route("GetAuthorizedUserInfo")]
        //public async Task<Object> GetAuthorizedUserInfo(string id)
        //{
        //    string userId = User.Claims.First(c => c.Type == id).Value;
        //    var user = await _userManager.FindByIdAsync(userId);
        //    return new
        //    {
        //        user.Email
        //    };
        //}

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUserViewModel>>> Users()
        {
            List<ApplicationUserViewModel> applicationUserViewModels = new List<ApplicationUserViewModel>();
            List<ApplicationUser> applicationUsers = await _context.Users.ToListAsync();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                applicationUserViewModels.Add(new ApplicationUserViewModel()
                {
                    Id = applicationUser.Id,
                    Email = applicationUser.Email,
                    Roles = _userManager.GetRolesAsync(applicationUser).Result.ToArray()
                });
            }
            return applicationUserViewModels;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserViewModel>> GetUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel applicationUserViewModel = new ApplicationUserViewModel()
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Roles = _userManager.GetRolesAsync(applicationUser).Result.ToArray()
            };

            return applicationUserViewModel;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUserViewModel>> DeleteUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return new ApplicationUserViewModel()
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Roles = _userManager.GetRolesAsync(applicationUser).Result.ToArray()
            };
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(string id, ApplicationUserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                var userRoles = await _userManager.GetRolesAsync(applicationUser);
                await _userManager.RemoveFromRolesAsync(applicationUser, userRoles.ToArray());
                await _userManager.AddToRolesAsync(applicationUser, model.Roles);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (applicationUser == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/Users/GetRoles
        [HttpGet]
        [Route("GetRoles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
