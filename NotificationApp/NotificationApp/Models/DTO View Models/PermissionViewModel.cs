using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NotificationApp.Models
{
    public class PermissionViewModel
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        public int? ParentId { get; set; }
    }
}
