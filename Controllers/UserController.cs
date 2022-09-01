using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListUsers.Models;

namespace ListUsers.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<User> Users = await _context.Users.ToListAsync();

              return View(await _context.Users.Select(x=>new User(){
                  Id = x.Id,
                  Name = x.Name,
                  Email = x.Email,
                  Profession = x.Profession,
                  ImageName = x.ImageName,
              }).ToListAsync());                 
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new User());
            else
                return View(_context.Users.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Name,Email,Profession,ImageName,imageFile")] User user)
        {
            //[Bind("Id,Name,Email,Profession,ImageName,imageFile")]
            if (ModelState.IsValid)
            {
                 if(user.Id!=0)
                  {
                    if (user.imageFile != null)
                    {
                        DeleteImage(user.ImageName);
                        user.ImageName = await SaveImage(user.imageFile, user.ImageName);

                    }
                    _context.Update(user);
                }
                else
                {
                    user.ImageName = await SaveImage(user.imageFile, user.ImageName);
                    _context.Add(user);
                }
               
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        /* public async Task<IActionResult> Delete(int? id)
         {
             if (id == null || _context.Users == null)
             {
                 return NotFound();
             }

             var user = await _context.Users
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (user == null)
             {
                 return NotFound();
             }

             return View(user);
         }*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if(user!=null)
            {
                DeleteImage(user.ImageName);
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                DeleteImage(user.ImageName);
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       */

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile, string imgName)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(wwwRootPath, "Image", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                 return imageName;
            }
            else
            {
                return imgName;
            }   
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}





///////////////////////////////////////

/* public IActionResult Create()
{
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Name,Email,Profession,imageFile")] User user)
{
    if (ModelState.IsValid)
    {
        user.ImageName = await SaveImage(user.imageFile, user.ImageName);

        _context.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(user);
}

// GET: User/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null || _context.Users == null) { return NotFound(); }

    var user = await _context.Users.FindAsync(id);
    if (user == null) { return NotFound(); }
    return View(user);
}

*/


/* [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Profession,ImageName,imageFile")] User user)
       {
           if (id != user.Id)
           {
               return NotFound();
           }

           if (ModelState.IsValid)
           {
               try
               { 

                   if(user.imageFile !=null)
                   {
                      DeleteImage(user.ImageName);
                   }
                   user.ImageName = await SaveImage(user.imageFile, user.ImageName);
                   _context.Update(user);
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!UserExists(user.Id))
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
           return View(user);
       }
      */