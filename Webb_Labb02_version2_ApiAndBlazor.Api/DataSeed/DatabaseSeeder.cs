using System.Text;
using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

// Till Hash-funktionen:
using Microsoft.AspNetCore.Identity;
//using System.Text; // Redan importerat.

namespace Webb_Labb02_version2_ApiAndBlazor.Api.DataSeed
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            var hasher = new PasswordHasher<User>();

            if (!db.Users.Any())
            {
                /*
                db.Users.AddRange(new List<User>
                {
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@admin.se",
                        Password = "a",
                        PasswordHash = "",
                        Role = "admin",
                        PhoneNumber = "0700000000",
                        HomeAddress = "Testgatan 1"
                    },
                    new User
                    {
                        FirstName = "Kalle",
                        LastName = "Anka",
                        Email = "kalle@anka.se",
                        Password = "kalle",
                        PasswordHash = "",
                        Role = "user",
                        PhoneNumber = "0701112233",
                        HomeAddress = "Ankeborg 3"
                    }
                });
                */

                /*
                var admin = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.se",
                    Password = "a",
                    PasswordHash = "",
                    Role = "admin",
                    PhoneNumber = "0700000000",
                    HomeAddress = "Testgatan 1"
                };
                admin.PasswordHash = hasher.HashPassword(admin, admin.Password);
                */

                var users = new List<User>
                {
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@admin.se",
                        Password = "a",
                        PasswordHash = "",
                        Role = "admin",
                        PhoneNumber = "0700000000",
                        HomeAddress = "Testgatan 1"
                    },
                    new User
                    {
                        FirstName = "Kalle",
                        LastName = "Anka",
                        Email = "kalle@anka.se",
                        Password = "kalle",
                        PasswordHash = "",
                        Role = "user",
                        PhoneNumber = "0701112233",
                        HomeAddress = "Ankeborg 3"
                    }
                };

                foreach(var user in users)
                {
                    user.PasswordHash = hasher.HashPassword(user, user.Password);
                }

                db.Users.AddRange(users);
            }

            if (!db.Products.Any())
            {
                db.Products.AddRange(new List<Product>
                {
                    new Product { 
                        ProductNumber = 1001, 
                        ProductName = "Mjölk", 
                        ProductCategory = "Mejeri", 
                        Price = 15.90m, 
                        ProductStatus = "Tillgänglig" 
                    },
                    new Product { 
                        ProductNumber = 2001, 
                        ProductName = "TV", 
                        ProductCategory = "Elektronik", 
                        Price = 5999.00m, 
                        ProductStatus = "Tillgänglig" }
                });
            }







            if (!db.Orders.Any())
            {
                var kalle = db.Users.FirstOrDefault(u => u.Email == "kalle@anka.se");
                var milk = db.Products.FirstOrDefault(p => p.ProductName == "Mjölk");
                var tv = db.Products.FirstOrDefault(p => p.ProductName == "TV");

                if (kalle != null && milk != null && tv != null)
                {
                    var orderItems = new List<OrderItem>
        {
            new OrderItem
            {
                ProductID = milk.ProductID,
                Quantity = 1,
                Price = milk.Price
            },
            new OrderItem
            {
                ProductID = tv.ProductID,
                Quantity = 1,
                Price = tv.Price
            }
        };

                    var total = orderItems.Sum(i => i.Price * i.Quantity);

                    var order = new Order
                    {
                        UserID = kalle.UserID,
                        OrderDate = DateTime.UtcNow,
                        OrderStatus = "shipped",
                        TotalAmount = total,
                        OrderItems = orderItems
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();
                }
            }






            db.SaveChanges();
        }

        
        //string Hash(string input)
        //{
        //    using var sha256 = SHA256.Create();
        //    var bytes = Encoding.UTF8.GetBytes(input);
        //    var hash = sha256.ComputeHash(bytes);
        //    return Convert.ToBase64String(hash);
        //}


    }
}
