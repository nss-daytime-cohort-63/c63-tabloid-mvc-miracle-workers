using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);

        List<Post> GetAllPublishedPostsByAuthorId(int authorId);
        void DeletePost(int postId, int userProfileId);
    }
}