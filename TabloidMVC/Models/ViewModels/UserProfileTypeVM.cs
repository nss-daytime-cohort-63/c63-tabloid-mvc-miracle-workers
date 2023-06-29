using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class UserProfileTypeVM
    {
        public UserProfile user { get; set; }

        public List<UserType> roleTypes { get; set; }
    }
}
