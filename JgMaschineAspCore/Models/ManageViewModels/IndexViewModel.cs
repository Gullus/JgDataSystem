using System.ComponentModel.DataAnnotations;

namespace JgMaschineAspCore.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Benutzername")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
