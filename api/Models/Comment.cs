namespace api.Models;

public class Comment {
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}