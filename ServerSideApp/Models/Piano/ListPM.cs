using System.Collections.Generic;

namespace ServerSideApp.Models.Piano
{
    public class ListItemPM : Piece
    {
        public string UserName { get; set; }
        public int CommentCount { get; set; }
        public bool HasMp3 { get; set; }
        public bool HasPdf { get; set; }
        public bool HasMidi { get; set; }
        public bool CanEdit { get; set; }
    }
    public class ListPM
    {
        public IEnumerable<ListItemPM> PianoPieces { get; set; }
        public bool CanCreate { get; set; }
    }


}