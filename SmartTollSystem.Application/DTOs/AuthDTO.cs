using SmartTollSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public record UserRegisterDTO
    {
        [Required(ErrorMessage = " First name cannot be blank ")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = " Last name cannot be blank ")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = " Email cannot be blank ")]
        [EmailAddress(ErrorMessage = " Invalid email address format ")]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = " Password cannot be blank ")]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = " Confirm password cannot be blank ")]
        [StringLength(100, MinimumLength = 8)]
        [Compare("Password", ErrorMessage = " Password and Confirm password do not match ")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = " Address cannot be blank ")]
        [StringLength(200, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\s,.'-]{5,200}$", ErrorMessage = " Invalid address format ")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = " City cannot be blank ")]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s-]{2,100}$", ErrorMessage = " Invalid city format ")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = " State cannot be blank ")]
        [StringLength(100, MinimumLength = 2)]
        public string State { get; set; } = string.Empty;
        [Required(ErrorMessage = " Zip code cannot be blank ")]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = " Invalid zip code format ")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = string.Empty;
        [Required(ErrorMessage = " Country cannot be blank ")]
        [StringLength(100, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = " Phone number cannot be blank ")]
        [StringLength(15, MinimumLength = 10)]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = " Invalid phone number format ")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = " Invalid phone number format ")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = " Date of birth cannot be blank ")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = " Invalid date format ")]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = " Date of birth must be between 1900 and 2100 ")]
        public DateTime DateOfBirth { get; set; }
        public List<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
    }
    public record UserLoginDTO
    {
        [Required(ErrorMessage = " Email cannot be blank ")]
        [EmailAddress(ErrorMessage = " Invalid email address format ")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = " Password cannot be blank ")]
        public string Password { get; set; } = string.Empty;
    }

    public record AuthResultDto
    (
        string Token,
        string? RefreshToken ,
        DateTime ExpiresAt,
        UserDto User ,
        string? Message  = null
    );
}
