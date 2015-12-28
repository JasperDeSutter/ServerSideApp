using System.Collections.Generic;
using System.Security.AccessControl;

namespace ServerSideApp.Models.Piano
{
    public class DetailsPM : Piece
    {
        
        public string UserName { get; set; }
        public int CommentCount { get; set; }
        public bool HasMp3 { get; set; }
        public bool HasPdf { get; set; }
        public bool HasMidi { get; set; }
        public bool CanEdit { get; set; }

        public string Genre { get; set; }
        public string Difficulty { get; set; }

        public IEnumerable<Comment.Comment> Comments { get; set; }

    }

}