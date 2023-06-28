using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void Add(Category category);
        void EditCategory(Category category);
        Category FindCategoryById(int id);
        void DeleteCategory(int id);
    }
}