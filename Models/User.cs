using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListUsers.Models
{
   public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string Profession { get; set; } = string.Empty;

       
        [DisplayName("Image")]
        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; } = string.Empty;

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? imageFile { get; set; } 

        [NotMapped]
        public string ImageSrc { get; set; } = string.Empty;
    }
}
