using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Controllers
{
   // [Authorize(Roles = "Admin")] 
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // List of roles
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // GET: UserRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _roleManager.FindByIdAsync(id);
                    existingRole.Name = role.Name;
                    await _roleManager.UpdateAsync(existingRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
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
            return View(role);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        // GET: UserRoles/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserRoles/Create
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Avoid duplicacy
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return RedirectToAction("Index");
        }

        private bool RoleExists(string id)
        {
            return _roleManager.RoleExistsAsync(id).GetAwaiter().GetResult();
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

