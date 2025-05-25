namespace api.Dtos.Comment;

public class CommentDto {
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public int StockId { get; set; }
}