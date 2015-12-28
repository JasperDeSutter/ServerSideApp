using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Comment
{
    public class CommentListPM
    {
        public IEnumerable<CommentPM> Comments { get; set; }
        public bool CanCreate { get; set; }
        public bool CanHide { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public Topics Topic { get; set; }
        public int? PostId { get; set; }
    }
}