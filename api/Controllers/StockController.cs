using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace api.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController(AppDbContext context, IStockRepository stockRepo) : ControllerBase {
    // List Endpoint
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stocks = await stockRepo
            .GetAllAsync(query);
        var stockDto = stocks
            .Select(s => s.ToStockDto())
            .ToList();
        return Ok(stockDto);
    }

    // Details Endpoint
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stock = await stockRepo.GetByIdAsync(id);

        if (stock == null) {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = stockDto.ToStockFromCreateDTO();
        await stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = await stockRepo.UpdateAsync(id, updateDto);

        if (stockModel == null) return NotFound();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = await stockRepo.DeleteAsync(id);

        if (stockModel == null) return NotFound();

        return NoContent();
    }
    
    
}