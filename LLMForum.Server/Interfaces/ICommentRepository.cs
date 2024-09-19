﻿using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);

        Task CreateAsync(Comment commentDto);

        Task<List<Comment>> GetPostCommentsAsync(string postId);

        Task<Comment> CreatePromptAsync(Comment promptModel);

        Task<List<CommentDto>> GetRepliesDtoAsync(string postId);

        Comment? GetOldestByPostId(string postId);

        Task<List<Comment>> GetRepliesAsync(string postId);

        Task DeleteCommentChainAsync(string commentId);

        Task CollectCommentsToDelete(string commentId, List<string> commentsToDelete);

        Task DeleteCommentsRecursively(List<string> comments);

        Task UpdateCommentAsync(string commentId, string content);
    }
}
