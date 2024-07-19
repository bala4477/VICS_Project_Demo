using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project1.Domain.Models;
using Project1.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Infrastructure.Common
{
    public static class SeedData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name="ADMIN",NormalizedName="ADMIN"},
                new IdentityRole{Name="USER",NormalizedName="USER"}
            };
            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedDataAsync(ApplicationDbContext _dbContext)
        {
            if(!_dbContext.Users.Any())
            {
                await _dbContext.AddRangeAsync(
                new Users
                {
                    Name="Balasubramaniyam T",
                    Location="AruppuKottai"
                },
                new Users
                {
                    Name = "Prethesh Kumar K.S",
                    Location = "Madurai"
                },
                new Users
                {
                    Name = "Mohammad Unais R",
                    Location = "Bangalore"
                },
                new Users
                {
                    Name = "Vignesh U",
                    Location = "Chennai"
                }
                );

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
