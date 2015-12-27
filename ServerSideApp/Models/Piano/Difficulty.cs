using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Piano
{
    public class Difficulty: INameId
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public override string ToString() {
            return $"{Id} - {Name}";
        }
    }
}