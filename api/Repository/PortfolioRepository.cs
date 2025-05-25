using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository(AppDbContext context) : IPortfolioRepository {
    public async Task<List<StockDto>> GetUserPortfolio(AppUser user) {
        return await context.Portfolios
            .Where(p => p.AppUserId == user.Id)  
            .Select(p => new StockDto  
            {  
                Id = p.Stock.Id,  
                Symbol = p.Stock.Symbol,  
                CompanyName = p.Stock.CompanyName,  
                Purchase = p.Stock.Purchase,  
                LastDiv = p.Stock.LastDiv,  
                Industry = p.Stock.Industry,  
                MarketCap = p.Stock.MarketCap  
            })  
            .ToListAsync();  
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio) {
        await context.Portfolios.AddAsync(portfolio);

        await context.SaveChangesAsync();

        return portfolio;
    }

    public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol) {
        var portfolioModel = await context.Portfolios.FirstOrDefaultAsync(x =>
            x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null) return null;

        context.Portfolios.Remove(portfolioModel);
        await context.SaveChangesAsync();

        return portfolioModel;
    }
}