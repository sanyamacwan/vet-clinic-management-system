using Microsoft.EntityFrameworkCore;
using VetClinicSystem.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace VetClinicSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register AppDbContext with SQL Server using the connection string from appsettings.json
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // ---------- DATABASE CREATION + SEEDING (NO DROP) ----------
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();

                // Just make sure DB exists
                context.Database.EnsureCreated();

                // Seed only if there are no doctors yet
                if (!context.VetDoctors.Any())
                {
                    // Doctors
                    var d1 = new VetDoctor { Name = "Dr. Smith", Specialty = "Surgery" };
                    var d2 = new VetDoctor { Name = "Dr. Lee", Specialty = "Dermatology" };
                    var d3 = new VetDoctor { Name = "Dr. Patel", Specialty = "General Practice" };

                    context.VetDoctors.AddRange(d1, d2, d3);
                    context.SaveChanges();

                    // Pets
                    var p1 = new Pet { MicrochipId = "MC001", Name = "Bella", Species = "Dog", VetDoctorId = d1.Id };
                    var p2 = new Pet { MicrochipId = "MC002", Name = "Milo", Species = "Cat", VetDoctorId = d1.Id };
                    var p3 = new Pet { MicrochipId = "MC003", Name = "Rocky", Species = "Dog", VetDoctorId = d2.Id };
                    var p4 = new Pet { MicrochipId = "MC004", Name = "Luna", Species = "Rabbit", VetDoctorId = d3.Id };

                    context.Pets.AddRange(p1, p2, p3, p4);
                    context.SaveChanges();

                    // PetProfiles (one per pet)
                    var pp1 = new PetProfile { PetId = p1.Id, VetNotes = "Initial checkup. Healthy." };
                    var pp2 = new PetProfile { PetId = p2.Id, VetNotes = "Skin allergy treatment ongoing." };
                    var pp3 = new PetProfile { PetId = p3.Id, VetNotes = "Post-surgery follow-up required." };
                    var pp4 = new PetProfile { PetId = p4.Id, VetNotes = "Dental cleaning recommended." };

                    context.PetProfiles.AddRange(pp1, pp2, pp3, pp4);
                    context.SaveChanges();
                }
            }
            // ---------- END SEEDING ----------

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
