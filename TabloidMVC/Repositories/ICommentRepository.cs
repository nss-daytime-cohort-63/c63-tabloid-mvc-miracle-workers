using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetAllCommentsByPostId(int postId);
        void AddComment(Comment comment);
    }
}
