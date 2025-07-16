using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Agri_Energy_Connect.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FarmersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Farmers.ToListAsync());
        }

        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public IActionResult Create()
        {
            var model = new FarmerRegistrationViewModel
            {
                Farmer = new Farmer()
            };
            return View(model);
        }

        // POST: Farmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FarmerRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email already exists
                    var existingUser = await _userManager.FindByEmailAsync(model.Farmer.FarmerEmail);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("", "Email is already in use.");
                        return View(model);
                    }

                    // Create the Identity user
                    var user = new IdentityUser
                    {
                        UserName = model.Farmer.FarmerEmail,
                        Email = model.Farmer.FarmerEmail,
                        EmailConfirmed = true // Auto-confirm for simplicity
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Assign the Farmer role
                        await _userManager.AddToRoleAsync(user, "Farmer");

                        // Create the Farmer entity and link to user
                        model.Farmer.UserId = user.Id;
                        _context.Add(model.Farmer);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Farmer created successfully with login credentials!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating farmer: {ex.Message}");
                }
            }
            return View(model);
        }

        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmerId,FarmerName,FarmerEmail,FarmerPhone")] Farmer farmer)
        {
            if (id != farmer.FarmerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Farmer updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.FarmerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. The farmer was modified by another user.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating farmer: {ex.Message}");
                }
            }
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var farmer = await _context.Farmers
                    .Include(f => f.Products)
                    .FirstOrDefaultAsync(f => f.FarmerId == id);
                
                if (farmer == null)
                {
                    return NotFound();
                }

                // Check if farmer has associated products
                if (farmer.Products != null && farmer.Products.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete farmer because they have associated products. Please delete the products first.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Farmer deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting farmer: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.FarmerId == id);
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

