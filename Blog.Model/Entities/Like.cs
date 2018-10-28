namespace Blog.Model
{
  public class Like
  {
    public string StoryId { get; set; }
    public Story Story { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
  }
}