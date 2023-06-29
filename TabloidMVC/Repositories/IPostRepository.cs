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
        Post GetUnapprovedPostById(int id);

        List<Post> GetAllPublishedPostsByAuthorId(int authorId);
        void DeletePost(int postId, int userProfileId);
        void EditPost(Post post, int userProfileId);
        List<Post> GetAllUnapprovedPosts();
        void EditPostApproval(Post post);
    }
}