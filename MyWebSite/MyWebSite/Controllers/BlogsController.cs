using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebSite.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace MyWebSite.Controllers
{
    public class BlogsController : Controller
    {
        private readonly MyWebSiteContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public BlogsController(MyWebSiteContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blog.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .SingleOrDefaultAsync(m => m.BlogID == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogID,Tag,Title,Description,Photo")] Blog blog, IFormFile upload)
        {

            //dosya uzantısı ıcın gecerliik denetimi
            if (upload != null && !IsExtensionValid(upload))
            {
                ModelState.AddModelError("Photo", "Dosya uzantısı jpg, jpeg ,gif , png olmalıdır.");
            }
            else if (upload == null)
            {
                ModelState.AddModelError("Photo", "Resim yüklemeniz gerekmektedir");

            }




            if (ModelState.IsValid)
            {

                //dosya yuklemesi
                if (upload != null && upload.Length > 0 && IsExtensionValid(upload))
                {
                    blog.Photo = await UploadFileAsync(upload);
                }



                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


                

            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.SingleOrDefaultAsync(m => m.BlogID == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("BlogID,Tag,Title,Description,Photo")] Blog blog, IFormFile upload)
        {
            if (id != blog.BlogID)
            {
                return NotFound();
            }


            //dosya uzantısı ıcın gecerliik denetimi
            if (upload != null && !IsExtensionValid(upload))
            {
                ModelState.AddModelError("Photo", "Dosya uzantısı jpg, jpeg ,gif , png olmalıdır.");
            }
            else if (upload == null && blog.Photo == null)//eger resim yüklenmisse bir daha sectirmiyor
            {
                ModelState.AddModelError("Photo", "Resim yüklemeniz gerekmektedir");

            }


            if (ModelState.IsValid)
            {
                try
                {

                    //dosya yuklemesi
                    if (upload != null && upload.Length > 0 && IsExtensionValid(upload))
                    {
                        blog.Photo = await UploadFileAsync(upload);
                    }


                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .SingleOrDefaultAsync(m => m.BlogID == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var blog = await _context.Blog.SingleOrDefaultAsync(m => m.BlogID == id);
            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int? id)
        {
            return _context.Blog.Any(e => e.BlogID == id);
        }


        //upload edilecek dosyanın geçerli mi onu kontrol eder.
        private bool IsExtensionValid(IFormFile upload)
        {
            if (upload != null)
            {
                var allowedExtension = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
                var extension = Path.GetExtension(upload.FileName).ToLowerInvariant();
                return allowedExtension.Contains(extension);
            }
            return false;
        }


        private async Task<string> UploadFileAsync(IFormFile upload)
        {
            //dosya upload
            if (upload != null && upload.Length > 0 && IsExtensionValid(upload))
            {
                var fileName = upload.FileName;
                var extension = Path.GetExtension(fileName);
                var uploadLocation = Path.Combine(hostingEnvironment.WebRootPath, "uploads");



                //yoksa olustur kodu
                if (!Directory.Exists(uploadLocation))
                {
                    Directory.CreateDirectory(uploadLocation);
                }

                uploadLocation += "/" + fileName;

                using (var stream = new FileStream(uploadLocation, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

    }
}
