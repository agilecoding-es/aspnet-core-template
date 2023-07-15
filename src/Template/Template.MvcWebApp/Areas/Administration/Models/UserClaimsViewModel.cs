namespace Template.MvcWebApp.Areas.Administration.Models
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaimViewModel> Claims { get; set; } = new List<UserClaimViewModel>();
    }
}
