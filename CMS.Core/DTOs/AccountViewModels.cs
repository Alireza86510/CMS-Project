using System.ComponentModel.DataAnnotations;
using CMS.DataLayer.Models;

namespace CMS.Core.DTOs;

public class LoginViewModel
{
    [Required(ErrorMessage = "ایمیل اجباری است")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "رمز عبور اجباری است")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "نام اجباری است")]
    public string Name { get; set; }
    [Required(ErrorMessage = "ایمیل اجباری است")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "رمز عبور اجباری است")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "تکرار رمز عبور اجباری است")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string RePassword { get; set; }
}

public class ShowPageViewModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsActive { get; set; }
}

public class ResetPasswordViewModel
{
    public string ActiveCode { get; set; }
    [Required(ErrorMessage = "رمز عبور اجباری است")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "تکرار رمز عبور اجباری است")]
    [Compare("Password")]
    public string? RePassword { get; set; }
}

public class ChangePasswordViewModel
{
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string? NewRePassword { get; set; }
}