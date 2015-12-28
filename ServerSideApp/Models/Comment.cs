using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models
{
    public class Comment
    {
        public int TopicId { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public int Id { get; set; }

        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool Hidden { get; set; }

        public int RatingCount { get; set; }
        public int RatingTotal { get; set; }
    }
}