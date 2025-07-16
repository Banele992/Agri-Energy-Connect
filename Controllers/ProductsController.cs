using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Agri_Energy_Connect.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, int? categoryId, DateTime? startDate, DateTime? endDate)
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            
            // Check if the user is in the Farmer role
            bool isFarmer = user != null && await _userManager.IsInRoleAsync(user, "Farmer");
            
            // Prepare category filter for the view
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CatName");
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchString = searchString;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            // Start with a base query
            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Farmer);
            
            // If the user is a farmer, only show their products
            if (isFarmer)
            {                
                // Find the farmer associated with this user
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                
                if (farmer != null)
                {
                    productsQuery = productsQuery.Where(p => p.FarmerId == farmer.FarmerId);
                }
                else
                {
                    // If no farmer profile is found, show empty list
                    return View(new List<Product>());
                }
            }
            
            // Apply search filters if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.ProdName.Contains(searchString) || 
                                                        p.ProdDescription.Contains(searchString));
            }
            
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }
            
            // Apply date range filters if provided
            if (startDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DateTime >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                // Add one day to include the end date fully
                DateTime endDatePlusOne = endDate.Value.AddDays(1).AddSeconds(-1);
                productsQuery = productsQuery.Where(p => p.DateTime <= endDatePlusOne);
            }
            
            // Execute the query and return results
            return View(await productsQuery.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProdId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CatName");
            
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            
            // Check if the user is in the Farmer role
            bool isFarmer = user != null && await _userManager.IsInRoleAsync(user, "Farmer");
            
            if (isFarmer)
            {
                // Find the farmer associated with this user
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                
                if (farmer != null)
                {
                    // Pre-select the farmer and hide the dropdown
                    ViewBag.FarmerId = new SelectList(new[] { farmer }, "FarmerId", "FarmerName");
                    ViewBag.IsFarmer = true;
                    ViewBag.CurrentFarmerId = farmer.FarmerId;
                    ViewBag.CurrentFarmerName = farmer.FarmerName;
                    return View();
                }
            }
            
            // For employees/admins, show all farmers
            ViewBag.FarmerId = new SelectList(_context.Farmers, "FarmerId", "FarmerName");
            ViewBag.IsFarmer = false;
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create([Bind("ProdId,ProdName,ProdDescription,Price,Stock,CategoryId,DateTime,ImageUrl,FarmerId")] Product product, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Ensure the images folder exists
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/" + uniqueFileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CatName", product.CategoryId);
            ViewBag.FarmerId = new SelectList(_context.Farmers, "FarmerId", "FarmerName", product.FarmerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName", product.CategoryId);
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerName", product.FarmerId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdId,ProdName,ProdDescription,Price,Stock,CategoryId,DateTime,ImageUrl,FarmerId")] Product product, IFormFile ImageFile)
        {
            if (id != product.ProdId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image file upload
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        product.ImageUrl = "/images/" + uniqueFileName;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProdId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName", product.CategoryId);
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerName", product.FarmerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProdId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
                _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProdId == id);
        }
    }
}


//Microsoft. [n.d.]. Role-based authorization in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles [Accessed 1 May 2025].
//Microsoft. [n.d.]. Shopping Cart. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/ecommerce [Accessed 2 May 2025].
//Ravindra Devrani. 2023. Asp .NET Core | Role Based Authorization in ASP.NET Core MVC 7. YouTube video. [Online]. Available at: https://youtu.be/xhCstGA9WVI?si=vDmAAibZi9YTWLYN [Accessed 4 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Website Shopping Cart. YouTube video. [Online]. Available at: https://youtu.be/PwQyRQuEor0?si=3et8vPiidJnFwz9- [Accessed 5 May 2025].
//W3Schools. [n.d.]. HTML Tutorial. [Online]. Available at: https://www.w3schools.com/html [Accessed 6 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Shop with Admin Area. YouTube video. [Online]. Available at: https://youtu.be/T9d90fcYJvM?si=9ZJeS_qDzq8UlW_o [Accessed 8 May 2025].
//Ravindra Devrani. 2023. Shopping Cart Project in .NET Core MVC (With Authentication). YouTube video. [Online]. Available at: https://youtu.be/JPFlSXejgKc?si=VXSsHSydovhZY3TA [Accessed 10 May 2025].
//Microsoft. [n.d.]. Role-based authorization in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles [Accessed 11 May 2025].
//Microsoft. [n.d.]. Shopping Cart. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/ecommerce [Accessed 12 May 2025].
//Ravindra Devrani. 2023. Asp .NET Core | Role Based Authorization in ASP.NET Core MVC 7. YouTube video. [Online]. Available at: https://youtu.be/xhCstGA9WVI?si=vDmAAibZi9YTWLYN [Accessed 12 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Website Shopping Cart. YouTube video. [Online]. Available at: https://youtu.be/PwQyRQuEor0?si=3et8vPiidJnFwz9- [Accessed 12 May 2025].
//W3Schools. [n.d.]. HTML Tutorial. [Online]. Available at: https://www.w3schools.com/html [Accessed 12 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Shop with Admin Area. YouTube video. [Online]. Available at: https://youtu.be/T9d90fcYJvM?si=9ZJeS_qDzq8UlW_o [Accessed 12 May 2025].
//Ravindra Devrani. 2023. Shopping Cart Project in .NET Core MVC (With Authentication). YouTube video. [Online]. Available at: https://youtu.be/JPFlSXejgKc?si=VXSsHSydovhZY3TA [Accessed 13 May 2025].
//Code With Ayan. 2024. Role-based Authentication and Authorization in ASP.NET Core MVC. YouTube video. [Online]. Available at: https://youtu.be/bzWJOxBR-MY?si=g4pOkzy59KOEdqdi [Accessed 11 May 2025].
//Code With Ayan. 2024. Login and Registration with Identity in ASP.NET Core MVC. YouTube video. [Online]. Available at: https://youtu.be/uE9nXpPNzBE?si=SYo6r1Ww_eHq-KZt [Accessed 13 May 2025]. 
//Microsoft. [n.d.]. Role-based authorization in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles [Accessed 1 May 2025].
//Microsoft. [n.d.]. Shopping Cart. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/ecommerce [Accessed 2 May 2025].
//Ravindra Devrani. 2023. Asp .NET Core | Role Based Authorization in ASP.NET Core MVC 7. YouTube video. [Online]. Available at: https://youtu.be/xhCstGA9WVI?si=vDmAAibZi9YTWLYN [Accessed 4 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Website Shopping Cart. YouTube video. [Online]. Available at: https://youtu.be/PwQyRQuEor0?si=3et8vPiidJnFwz9- [Accessed 5 May 2025].
//W3Schools. [n.d.]. HTML Tutorial. [Online]. Available at: https://www.w3schools.com/html [Accessed 6 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Shop with Admin Area. YouTube video. [Online]. Available at: https://youtu.be/T9d90fcYJvM?si=9ZJeS_qDzq8UlW_o [Accessed 8 May 2025].
//Ravindra Devrani. 2023. Shopping Cart Project in .NET Core MVC (With Authentication). YouTube video. [Online]. Available at: https://youtu.be/JPFlSXejgKc?si=VXSsHSydovhZY3TA [Accessed 10 May 2025].
//Microsoft. [n.d.]. Role-based authorization in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles [Accessed 11 May 2025].
//Microsoft. [n.d.]. Shopping Cart. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/ecommerce [Accessed 12 May 2025].
//Ravindra Devrani. 2023. Asp .NET Core | Role Based Authorization in ASP.NET Core MVC 7. YouTube video. [Online]. Available at: https://youtu.be/xhCstGA9WVI?si=vDmAAibZi9YTWLYN [Accessed 12 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Website Shopping Cart. YouTube video. [Online]. Available at: https://youtu.be/PwQyRQuEor0?si=3et8vPiidJnFwz9- [Accessed 12 May 2025].
//W3Schools. [n.d.]. HTML Tutorial. [Online]. Available at: https://www.w3schools.com/html [Accessed 12 May 2025].
//Evan Gudmestad. 2023. ASP.NET Core MVC Tutorial – Build a Shop with Admin Area. YouTube video. [Online]. Available at: https://youtu.be/T9d90fcYJvM?si=9ZJeS_qDzq8UlW_o [Accessed 12 May 2025].
//Ravindra Devrani. 2023. Shopping Cart Project in .NET Core MVC (With Authentication). YouTube video. [Online]. Available at: https://youtu.be/JPFlSXejgKc?si=VXSsHSydovhZY3TA [Accessed 13 May 2025].
//Code With Ayan. 2024. Role-based Authentication and Authorization in ASP.NET Core MVC. YouTube video. [Online]. Available at: https://youtu.be/bzWJOxBR-MY?si=g4pOkzy59KOEdqdi [Accessed 11 May 2025].
//Code With Ayan. 2024. Login and Registration with Identity in ASP.NET Core MVC. YouTube video. [Online]. Available at: https://youtu.be/uE9nXpPNzBE?si=SYo6r1Ww_eHq-KZt [Accessed 13 May 2025]. 

