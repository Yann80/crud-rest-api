
using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Models
{
    public class Author
    {
        /// <summary>
        /// Id of the author
        /// </summary>
        [Key]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$",
            ErrorMessage = "Special Characters are not allowed.")]
        public int AuthorId { get; set; }

        /// <summary>
        /// First name of the author
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$",
            ErrorMessage = "Special Characters are not allowed.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the author
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$",
            ErrorMessage = "Special Characters are not allowed.")]
        public string LastName { get; set; }

        /// <summary>
        /// Author's email
        /// </summary>
        [Required]
        [RegularExpression("^(.+)@(.+)$")]
        public string Email { get; set; }

        /// <summary>
        /// Date at which the author is created
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
