using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostIndexVM
    {
        public List<Post> Posts { get; set; }
        public List<Category> CategoryOptions { get; set; }
        public List<UserProfile> UserOptions { get; set; }
        public int SelectedCategoryId { get; set; }
        public int SelectedProfileId { get; set;}
    }
}
