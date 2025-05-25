using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager) : ControllerBase{

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comments = await commentRepo.GetAllAsync();
        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }

    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment = await commentRepo.GetByIdAsync(id);
    
        if (comment == null) return NotFound();
    
        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await stockRepo.StockExists(stockId)) {
            return BadRequest("Stock does not exist.");
        }

        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);

        var commentModel = commentDto.ToCommentFromCreate(stockId);
        commentModel.AppUserId = appUser.Id;
        await commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment = await commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());

        if (comment == null) {
            return NotFound("Comment not found.");
        }

        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var commentModel = await commentRepo.DeleteAsync(id);

        if (commentModel == null) return NotFound("Comment does not exist.");

        return NoContent();
    }
    
}