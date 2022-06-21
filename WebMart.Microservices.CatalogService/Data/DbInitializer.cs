using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.CatalogService.Models;

namespace WebMart.Microservices.CatalogService.Data
{
    public static class DbInitializer
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CatalogDbContext>(), isProduction);
            }
        }

        private static void SeedData(CatalogDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run mirations: {ex.Message}");
                }
            }

            if (!context.Categories.Any())
            {
                Console.WriteLine("--> Seeding data in the Categories table...");
                context.Categories.AddRange(
                    new Category()
                    {
                        Name = "Одежда",
                        Description = "Всякая одежда"
                    },
                    new Category()
                    {
                        Name = "Обувь",
                        Description = "Всякая обувь"
                    },
                    new Category()
                    {
                        Name = "Аксессуары",
                        Description = "Всякие аксессуары"
                    }
                );
                context.SaveChanges();
            }
            if (!context.SubCategories.Any())
            {
                Console.WriteLine("--> Seeding data in the SubCategories table...");
                context.SubCategories.AddRange(
                    new SubCategory()
                    {
                        Name = "Летняя одежда",
                        Description = "Одежда для хорошего летнего отдыха",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Одежда").Id
                    },
                    new SubCategory()
                    {
                        Name = "Зимняя одежда",
                        Description = "Одежда, чтобы не мерзнуть",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Одежда").Id
                    },
                    new SubCategory()
                    {
                        Name = "Спортивная одежда",
                        Description = "Одежда для любителей активного отдыха",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Одежда").Id
                    },
                    new SubCategory()
                    {
                        Name = "Летняя обувь",
                        Description = "Обувь для хорошей прогулки летом",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Обувь").Id
                    },
                    new SubCategory()
                    {
                        Name = "Зимняя обувь",
                        Description = "Обувь, чтобы ноги не мерзли",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Обувь").Id
                    },
                    new SubCategory()
                    {
                        Name = "Спортивная обувь",
                        Description = "Обувь для хорошей пробежки",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Обувь").Id
                    },
                    new SubCategory()
                    {
                        Name = "Часы",
                        Description = "Чтобы выглядеть богаче, чем есть на самом деле",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Аксессуары").Id
                    },
                    new SubCategory()
                    {
                        Name = "Очки",
                        Description = "Чтобы не было видно мешков под глазами",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Аксессуары").Id
                    },
                    new SubCategory()
                    {
                        Name = "Браслеты",
                        Description = "Чтобы выглядеть богаче, чем есть на самом деле, но беднее, чем с часами",
                        CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Аксессуары").Id
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}