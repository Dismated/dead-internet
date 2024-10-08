﻿using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Mappers
{
    public class PostMapper : IPostMapper
    {
        public PostDto ToPostDto(Post postModel)
        {
            return new PostDto
            {
                Id = postModel.Id,
                Title = postModel.Title,
                CreatedAt = postModel.CreatedAt,
            };
        }

        public Post ToPostFromCreateDto(string userId, string prompt)
        {
            return new Post { AppUserId = userId, Title = prompt };
        }
    }
}
