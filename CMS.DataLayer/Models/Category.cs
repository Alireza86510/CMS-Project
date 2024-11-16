using System.ComponentModel.DataAnnotations;

namespace CMS.DataLayer.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name = "نام دسته بندی")]
        [Required(ErrorMessage = "نام دسته بندی اجباری است")]
        [MaxLength(75)]
        public string Name { get; set; }


        #region Navigations

        public List<Post> Posts { get; set; }

        #endregion
    }
}
