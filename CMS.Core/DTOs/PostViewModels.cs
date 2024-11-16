using CMS.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.DTOs
{
    public class AddPostViewModel
    {
        [Display(Name = "عنوان پست")]
        [Required(ErrorMessage = "عنوان پست اجباری است")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Display(Name = "محتوای پست")]
        [Required(ErrorMessage = "محتوای پست اجباری است")]
        [MaxLength(750)]
        public string Content { get; set; }
        [Required]
        [MaxLength(500)]
        public string Tags { get; set; }
    }
}