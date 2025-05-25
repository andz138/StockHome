using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(AppDbContext context) : ICommentRepository {
    public async Task<List<Comment>> GetAllAsync() {
        return await context.Comments
            .Include(c => c.AppUser)
            .ToListAsync();
    }
    
    public async Task<Comment?> GetByIdAsync(int id) {
        var comment = await context.Comments
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);
    
        return comment ?? null;
    }

    public async Task<Comment> CreateAsync(Comment commentModel) {
        await context.Comments.AddAsync(commentModel);
        await context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel) {
        var existingComment = await context.Comments.FindAsync(id);

        if (existingComment == null) return null;

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id) {
        var comment = await context.Comments.FindAsync(id);

        if (comment == null) return null;

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();

        return comment;
    }
}