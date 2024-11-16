using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CMS.DataLayer.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Display(Name = "عنوان پست")]
        [Required(ErrorMessage = "عنوان پست اجباری است")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Display(Name = "محتوای پست")]
        [Required(ErrorMessage = "محتوای پست اجباری است")]
        [MaxLength(750)]
        public string Content { get; set; }
        [Display(Name = "آواتار پست")]
        [AllowNull]
        [MaxLength(100)]
        public string? ImagePath { get; set; }
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime? UpdateDate { get; set; }
        [Required]
        [MaxLength(500)]
        public string Tags { get; set; }
        [Required]
        public int CategoryId { get; set; }


        #region Navigations

        public User User { get; set; }
        public List<PostComment> PostComments { get; set; }
        public List<PostReaction> PostReactions { get; set; }
        public Category Category { get; set; }

        #endregion
    }
}
