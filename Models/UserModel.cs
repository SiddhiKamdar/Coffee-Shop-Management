using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [Phone]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public bool IsActive { get; set; }
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
    }
}
