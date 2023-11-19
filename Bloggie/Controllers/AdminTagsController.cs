using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Bloggie.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }


        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domainn model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List() {

            // use DbContext to read Tags from database
            var tags = bloggieDbContext.Tags.ToList();

            return View(tags);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //var tag = bloggieDbContext.Tags.Find(id);
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

            if (tag != null) {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public IActionResult Edit (EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag =   bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                // save the changes into the database
                bloggieDbContext.SaveChanges();
                // show success notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
            // show error notification
            return RedirectToAction("Edit" , new { id = editTagRequest.Id });
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);

            if (tag != null)
            {
                bloggieDbContext.Remove(tag);
                bloggieDbContext.SaveChanges();

                //show success notification 
                return RedirectToAction("List");
            }
            // show error notifications
            return RedirectToAction("Edit", new { id=editTagRequest.Id});
        }
    }
}
