using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Stocks")]
public class Stock {
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string CompanyName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Purchase { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LastDiv { get; set; }
    public required string Industry { get; set; }
    public long MarketCap { get; set; }
    public List<Comment> Comments { get; set; } = [];
    public List<Portfolio> Portfolios { get; set; } = [];
}