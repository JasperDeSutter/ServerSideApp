using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Comment
{
    public class Comment
    {
        public int TopicId { get; set; }
        public int? PostId { get; set; }
        public string UserId { get; set; }
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool Hidden { get; set; }
    }
}