using System.ComponentModel.DataAnnotations;

namespace CMS.DataLayer.Models
{
    public class PostReaction
    {
        [Key]
        public int ReactionId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PostId { get; set; }


        #region Navigations

        public User User { get; set; }
        public Post Post { get; set; }

        #endregion
    }
}
