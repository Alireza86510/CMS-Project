using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CMS.DataLayer.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام اجباری است")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "توضیحات")]
        [AllowNull]
        [MaxLength(300)]
        public string? Description { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل اجباری است")]
        [MaxLength(200)]
        public string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور اجباری است")]
        [MaxLength(100)]
        public string Password { get; set; }
        [Display(Name = "کد فعالسازی")]
        [Required]
        [MaxLength(75)]
        public string ActiveCode { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public DateTime RegisterDate { get; set; }
        [Display(Name = "عکس پروفایل")]
        [AllowNull]
        [MaxLength(100)]
        public string? ImagePath { get; set; } = "UserDefault.png";


        #region Navigations

        public List<Post> Posts { get; set; }
        public List<PostComment> PostComments { get; set; }
        public List<PostReaction> PostReactions { get; set; }

        #endregion
    }
}
