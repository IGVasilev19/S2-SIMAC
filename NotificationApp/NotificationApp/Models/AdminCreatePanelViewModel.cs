using NotificationApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace NotificationApp.Models
{
    public class AdminCreatePanelViewModel
    {
        [Required(ErrorMessage = "Organization name is required!")]
        [MaxLength(50)]
        public string OrganizationName {  get; set; }
        [Required(ErrorMessage = "Manager name is required!")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Manager email is required!")]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Manager password is required!")]
        [MaxLength(255)]
        public string Password { get; set; }

    }
}
