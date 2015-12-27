using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Piano
{
    public class Piece
    {
        [Required]
        public int GenreId { get; set; }
        [Required]
        public int DifficultyId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Composer { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string PdfPath { get; set; }
        public string MidiPath { get; set; }
        public string Mp3Path { get; set; }

        public void CopyFrom(Piece piece) {
            GenreId = piece.GenreId;
            DifficultyId = piece.DifficultyId;
            Composer = piece.Composer;
            Title = piece.Title;
            Id = piece.Id;
            UserId = piece.UserId;
            PdfPath = piece.PdfPath;
            MidiPath = piece.MidiPath;
            Mp3Path = piece.Mp3Path;
        }

    }
}