using Microsoft.AspNetCore.Mvc;
using SQA.Models.FacultyContext;
using SQA.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SQA.Models
{
    public class PUser
    {
        [Required(ErrorMessage = "Please add name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select the Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [RegularExpression(@"(01)(0|1|2|5)[0-9]{8}", ErrorMessage = "It is not a correct phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please select your Department")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Please select your faculty")]
        public string Faculty { get; set; }
    }
    public class CreateUser : PUser
    {
        [Required(ErrorMessage = "Please add user name")]
        [Remote(action: "VerifyUserName", controller: "Student")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please add the email")]
        [EmailAddress(ErrorMessage = "The Email is not correct")]
        [Remote(action: "VerifyEmail", controller: "Student")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please add the basic password")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).{8,15}$", ErrorMessage = "Password must be at least of 8 character and has number and one special chracter at least")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm the password")]
        [Compare("Password", ErrorMessage = "The password does not match")]
        [Display(Name = "Confirm Passowrd")]
        public string ConfirmPassword { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
    public class UserandRole
    {
        public User User { get; set; }
        public IList<string> Role { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
    }
    public class EditUser : PUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Can not remove id")]
        public string Id { get; set; }
        public string Search { get; set; }
        public List<Faculty> Faculties { get; set; }
        public List<Department> MyDepartment { get; set; }
    }
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please enter username")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter new password")]
        [Display(Name = "Password")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).{8,15}$", ErrorMessage = "Password must be at least of 8 character and has number and one special chracter at least")]
        [Remote(action: "CheckPassword", controller: "Student")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm the password")]
        [Compare("Password", ErrorMessage = "The password does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please add old password")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).{8,15}$", ErrorMessage = "Old Password is not correct")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        public string Search { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    public class UsersPaging
    {
        public List<UserandRole> UserandRoles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string Search { get; set; }
    }
}
