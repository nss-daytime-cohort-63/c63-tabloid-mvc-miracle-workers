using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            PostIndexVM vm = new PostIndexVM()
            {
                CategoryOptions = _categoryRepository.GetAll(),
                Posts = _postRepository.GetAllPublishedPosts()

            };
            return View(vm);
        }


        public IActionResult MyIndex()
        {
            int myId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllPublishedPostsByAuthorId(myId);
            return View(posts);
        }
        [Authorize]
        public IActionResult UnapprovedIndex()
        {          
            var posts = _postRepository.GetAllUnapprovedPosts();
            return View(posts);
        }

        [Authorize]
        public IActionResult Approval(int id)
        {
            return View(_postRepository.GetUnapprovedPostById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approval(Post post)
        {
            try
            {
                _postRepository.EditPostApproval(post);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return View(post);
            }
        }
        public IActionResult Details(int id)
        {

            var post = _postRepository.GetPublishedPostById(id);

            PostAddTagViewModel vm = new PostAddTagViewModel()
            {
                Post = post,
                TagOptions = _postRepository.GetPostTags(id),
            };

            if (vm.Post == null)
            {
                int userId = GetCurrentUserProfileId();
                vm.Post = _postRepository.GetUserPostById(id, userId);
                if (vm.Post == null)
                {
                    return NotFound();
                }
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = false;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public IActionResult Edit(int id)
        {
            
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            vm.Post = _postRepository.GetPublishedPostById(id);

            if (vm.Post == null)
            {
                return NotFound();
            }
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post)
        {
            try
            {
                int userId = GetCurrentUserProfileId();
                _postRepository.EditPost(post, userId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return View(post);
            }
        }
        public IActionResult Delete(int id)
        {          
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id, GetCurrentUserProfileId());
                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }

        public IActionResult TagManagement(int id)
        {

            var tagM = new PostAddTagViewModel();
            tagM.TagOptions = _tagRepository.GetAll();
            tagM.Post = _postRepository.GetPublishedPostById(id);
            tagM.PostTags = _postRepository.GetPostTags(id);

            if (tagM.Post == null)
            {
                return NotFound();
            }

            return View(tagM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TagManagement(PostAddTagViewModel pt)
        {
            try
            {
               

                if (pt.Post == null) 
                {
                    return NotFound();
                }
                _postRepository.AddTagToPost(pt.Post.Id, pt.Tag.Id);
                
                return RedirectToAction("Details", "Post", new { id = pt.Post.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return View(pt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveTag(PostAddTagViewModel pt)
        {
            try
            {

                if (pt.Post == null)
                {
                    return NotFound();
                }

                 _postRepository.RemoveTagFromPost(pt.Post.Id, pt.Tag.Id);

                return RedirectToAction("Details", "Post", new { id = pt.Post.Id });
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"{ex.Message}");
                return View(pt);
            }
        }
        public IActionResult IndexByCategory(IFormCollection form)
        {

            List<Post> posts = _postRepository.GetPostsByCategory(int.Parse(form["CategoryOptions"]));

           
            return View(posts);
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
