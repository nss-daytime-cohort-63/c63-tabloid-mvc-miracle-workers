using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        void Add(UserProfile userProfile);

        List<UserProfile> GetAll();

        UserProfile GetById(int id);
      
        void DeactivateById(int id);
    }
}