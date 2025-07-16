using Agri_Energy_Connect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Farmer> Farmers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Seed data
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CatName = "Fruits" },
                new Category { CategoryId = 2, CatName = "Vegetables" },
                new Category { CategoryId = 3, CatName = "Meat" },
                new Category { CategoryId = 4, CatName = "Dairy" },
                 new Category { CategoryId = 5, CatName = "Other" }
                );
            //Add farmer data
            modelBuilder.Entity<Farmer>().HasData(
    new Farmer
    {
        FarmerId = 1,
        FarmerName = "John Doe",
        FarmerEmail = "john@example.com",
        FarmerPhone = "1234567890"
    },


            new Farmer
            {
                FarmerId = 2,
                FarmerName = "Sandile Nosi",
                FarmerEmail = "sandile@example.com",
                FarmerPhone = "4576349870"
            },

              new Farmer
              {
                  FarmerId = 3,
                  FarmerName = "Alex Chan",
                  FarmerEmail = "alex@example.com",
                  FarmerPhone = "0238765983"
              },

            new Farmer
            {
                FarmerId = 4,
                FarmerName = "Mike Smith",
                FarmerEmail = "mike@example.com",
                FarmerPhone = "9876453245"
            }
);


            modelBuilder.Entity<Product>().HasData(

                //Add product data
                new Product
                {
                    ProdId = 1,
                    ProdName = "Apple",
                    ProdDescription = "100% organic farmed apples",
                    Price = 3.50m,
                    Stock = 80,
                    CategoryId = 1,
                    FarmerId = 1,
                    ImageUrl = "apple.jpg",
                    DateTime = new DateTime(2024, 05, 01, 0, 0, 0, DateTimeKind.Utc)
                },

                  new Product
                  {
                      ProdId = 2,
                      ProdName = "Potato",
                      ProdDescription = "100% organic farmed potato",
                      Price = 3.50m,
                      Stock = 80,
                      CategoryId = 2,
                      FarmerId = 2,
                      ImageUrl = "potato.jpg",
                      DateTime = new DateTime(2024, 05, 01, 0, 0, 0, DateTimeKind.Utc)
                  },

                   new Product
                   {
                       ProdId = 3,
                       ProdName = "Beef",
                       ProdDescription = "Grass fed beef",
                       Price = 3.50m,
                       Stock = 80,
                       CategoryId = 3,
                       FarmerId = 3,
                       ImageUrl = "beef.jpg",
                       DateTime = new DateTime(2024, 05, 01, 0, 0, 0, DateTimeKind.Utc)
                   },

                     new Product
                     {
                         ProdId = 4,
                         ProdName = "Greek Yogurt",
                         ProdDescription = "Fresh greek yogurt, unpasturised",
                         Price = 3.50m,
                         Stock = 80,
                         CategoryId = 4,
                         FarmerId = 4,
                         ImageUrl = "yogurt.jpg",
                         DateTime = new DateTime(2024, 05, 01, 0, 0, 0, DateTimeKind.Utc)
                     }
                     );  

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

