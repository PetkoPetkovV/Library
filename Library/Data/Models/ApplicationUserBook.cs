using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class ApplicationUserBook
    {
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int BookId { get; set; }
        [Required]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
    }
}
//⦁	ApplicationUserId – a string, Primary Key, foreign key (required)
//⦁	ApplicationUser – ApplicationUser
//⦁	BookId – an integer, Primary Key, foreign key (required)
//⦁	Book – Book
