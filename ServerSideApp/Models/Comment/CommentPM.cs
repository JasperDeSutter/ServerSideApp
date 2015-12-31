using ServerSideApp.Helpers;

namespace ServerSideApp.Models.Comment
{
    public class CommentPM : Comment
    {
        public string UserName { get; set; }
        public float Rating { get; set; }
        public string RelativeTimeString => Time.ToRelativeTimeString();
    }
}