using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        void Add(UserProfile userProfile);

        void EditType(UserProfile userProfile);

        List<UserProfile> GetAll();
        List<UserType> GetUserTypes();

        UserProfile GetById(int id);
      
        void DeactivateById(int id);


    }
}