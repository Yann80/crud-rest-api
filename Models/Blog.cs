using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBlog.Models
{
    public class Blog
    {
        /// <summary>
        /// Id for the post
        /// </summary>
        [Key]
        public int BlogId { get; set; }

        /// <summary>
        /// The title of the post
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$",
            ErrorMessage = "Special Characters are not allowed.")]
        public string Title { get; set; }

        /// <summary>
        /// The content of the post
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$",
            ErrorMessage = "Special Characters are not allowed.")]
        public string Body { get; set; }

        /// <summary>
        /// Published date of the post
        /// </summary>
        public DateTime PublishedDate { get; set; }

        [Required]
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
    }
}
