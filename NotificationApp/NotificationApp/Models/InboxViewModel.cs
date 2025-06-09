using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NotificationApp.Models
{
    public class InboxViewModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
        public string AccountOrganization { get; set; }
        public string AccountRole { get; set; }
         public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();
        [ValidateNever]
        public string? Search {  get; set; }
        public bool? FilterImportant { get; set; }
        public bool? FilterRead { get; set; }
        [ValidateNever]
        [BindNever]
        public int? OrganizationId { get; set; }
    }
}
