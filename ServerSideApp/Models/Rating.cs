namespace ServerSideApp.Models
{
    public class Rating
    {
        public int CommentId{ get; set; }
        public string UserId { get; set; }
        public float Value { get; set; }
    }
}