﻿using DeadInternet.Server.Data;
using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeadInternet.Server.Repositories
{
    public class CommentRepository(ApplicationDBContext context, ICommentMapper commentMapper)
        : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;
        private readonly ICommentMapper _commentMapper = commentMapper;

        public async Task<Comment?> GetByIdAsync(string id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetPostCommentsAsync(string postId)
        {
            return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<Comment> CreatePromptAsync(Comment promptModel)
        {
            await _context.Comments.AddAsync(promptModel);
            await _context.SaveChangesAsync();
            return promptModel;
        }

        public async Task<List<CommentDto>> GetRepliesDtoAsync(string commentId)
        {
            var comment = await _context
                .Comments.Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                return [];

            var commentDto = _commentMapper.ToCommentDto(comment);

            foreach (var reply in commentDto.Replies)
            {
                reply.Replies = await GetRepliesDtoAsync(reply.Id);
            }

            return [.. commentDto.Replies];
        }

        public async Task<Comment?> GetOldestByPostIdAsync(string postId)
        {
            return await _context
                .Comments.Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetRepliesAsync(string commentId)
        {
            var comment = await _context
                .Comments.Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                return [];

            foreach (var reply in comment.Replies)
            {
                reply.Replies = await GetRepliesAsync(reply.Id);
            }

            return [.. comment.Replies];
        }

        public async Task DeleteCommentChainAsync(string commentId)
        {
            {
                var commentsToDelete = new List<string> { commentId };

                await CollectCommentsToDelete(commentId, commentsToDelete);
                await DeleteCommentsRecursively(commentsToDelete);
            }
        }

        public async Task CollectCommentsToDelete(string commentId, List<string> commentsToDelete)
        {
            var replies = await GetRepliesAsync(commentId);
            foreach (var reply in replies)
            {
                await CollectCommentsToDelete(reply.Id, commentsToDelete);
            }
            commentsToDelete.Add(commentId);
        }

        public async Task DeleteCommentsRecursively(List<string> commentIds)
        {
            foreach (var id in commentIds)
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment != null)
                {
                    _context.Comments.Remove(comment);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(string commentId, string content)
        {
            var existingComment =
                await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId)
                ?? throw new DirectoryNotFoundException("Comment");

            existingComment.Content = content;

            await _context.SaveChangesAsync();
        }
    }
}
