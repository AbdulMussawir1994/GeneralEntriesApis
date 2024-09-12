namespace GeneralEntries.ViewModel
{
    public class TokenViewModel
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
