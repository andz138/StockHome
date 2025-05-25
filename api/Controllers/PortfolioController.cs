using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, 
    IPortfolioRepository portfolioRepo) : ControllerBase {
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio() {
        // 1. Extract the username from the authenticated user's claims (via HttpContext / ControllerBase.User)
        //    - Uses a custom extension method (e.g., ClaimTypes.GivenName → username)
        var username = User.GetUsername();
        // 2. Fetch the full AppUser object from the Identity user store
        //    - UserManager handles database lookups via the normalized username
        var appUser = await userManager.FindByNameAsync(username);
        // 3. Retrieve the user's portfolio data from the repository
        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

        // 4. Return the portfolio data as a 200 OK response
        //    - Ensures only the authenticated user's data is returned (security)
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol) {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        var stock = await stockRepo.GetBySymbolAsync(symbol);

        if (stock == null) return BadRequest("Stock not found");

        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Cannot add same stock to Portfolio");

        var portfolioModel = new Portfolio {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        await portfolioRepo.CreateAsync(portfolioModel);

        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol) {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);

        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredStock.Count() == 1) {
            await portfolioRepo.DeletePortfolio(appUser, symbol);
        }
        else {
            return BadRequest("Stock is not in your Portfolio.");
        }

        return Ok();
    }
}