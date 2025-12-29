using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinicSystem.Models;

namespace VetClinicSystem.Controllers
{
    public class PetsController : Controller
    {
        private readonly AppDbContext _context;

        public PetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pets
        public async Task<IActionResult> Index()
        {
            var pets = await _context.Pets
                .Include(p => p.VetDoctor)
                .ToListAsync();
            return View(pets);
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _context.Pets
                .Include(p => p.VetDoctor)
                .Include(p => p.PetProfile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null) return NotFound();

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            ViewData["VetDoctorId"] = new SelectList(_context.VetDoctors, "Id", "Name");
            return View();
        }

        // POST: Pets/Create
        [HttpPost]
        public async Task<IActionResult> Create(Pet pet)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            ViewData["VetDoctorId"] = new SelectList(_context.VetDoctors, "Id", "Name", pet.VetDoctorId);

            return View(pet);
        }

        // POST: Pets/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Pet pet)
        {
            if (id != pet.Id) return NotFound();

            _context.Entry(pet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _context.Pets
                .Include(p => p.VetDoctor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null) return NotFound();

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
