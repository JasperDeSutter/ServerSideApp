using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Piano
{
    public class Genre: INameId
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
    }
}