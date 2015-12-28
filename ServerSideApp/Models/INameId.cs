using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models
{
    public interface INameId
    {
        [Required]
        string Name { get; set; }
        int Id { get; set; } 
    }
}