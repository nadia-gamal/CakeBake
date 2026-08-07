namespace CakeBake.ViewModels.Admin
{
    public class AdminProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName
            => $"{FirstName} {LastName}";

        public string Email { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}