using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.DataLayer.Models
{
    public class PostComment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required(ErrorMessage = "متن نظر اجباری است")]
        [MaxLength(300)]
        public string CommentText { get; set; }
        public DateTime CreateDate { get; set; }


        #region Navigations

        public User User { get; set; }
        public Post Post { get; set; }

        #endregion
    }
}
