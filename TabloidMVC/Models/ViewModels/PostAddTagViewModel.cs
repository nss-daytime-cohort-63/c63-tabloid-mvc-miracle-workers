﻿using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostAddTagViewModel
    {
        public Post Post {  get; set; }
        public Tag Tag { get; set; }
        public List<Tag> PostTags { get; set; }
        public List<Tag> TagOptions { get; set; }
    }
}
