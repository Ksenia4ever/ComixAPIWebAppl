namespace ComixAPIWebApp.Models
{
    public class ComicAuthor
    {
        public int ComicId { get; set; }
        public int AuthorId { get; set; }

        public virtual Comic Comic { get; set; }
        public virtual Author Author { get; set; }
    }
}