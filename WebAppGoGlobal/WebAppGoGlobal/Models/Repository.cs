namespace WebAppGoGlobal.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsDeleted { get; set; }
        public User User { get; set; }
        
    }
}
