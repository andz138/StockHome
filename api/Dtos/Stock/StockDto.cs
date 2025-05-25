namespace api.Dtos.Stock;
using api.Dtos.Comment;

public class StockDto {
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string CompanyName { get; set; }
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public required string Industry { get; set; }
    public long MarketCap { get; set; }
    public List<CommentDto> Comments { get; set; } = [];
}