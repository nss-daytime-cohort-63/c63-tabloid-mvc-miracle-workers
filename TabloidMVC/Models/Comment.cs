using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Comment Date")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Post")]
        public int PostId { get; set; }

        public string PostTitle { get; set; }


        [DisplayName("Author")]
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
