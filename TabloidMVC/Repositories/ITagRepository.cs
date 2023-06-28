using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        Tag GetTagById(int id);
        void DeleteTag(int id);
    }
}
