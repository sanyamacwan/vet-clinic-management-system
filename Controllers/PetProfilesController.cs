using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinicSystem.Models;

namespace VetClinicSystem.Controllers
{
    public class PetProfilesController : Controller
    {
        private readonly AppDbContext _context;

        public PetProfilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PetProfiles
        public async Task<IActionResult> Index()
        {
            var profiles = await _context.PetProfiles
                .Include(p => p.Pet)
                .ToListAsync();
            return View(profiles);
        }

        // GET: PetProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var profile = await _context.PetProfiles
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null) return NotFound();

            return View(profile);
        }

        // GET: PetProfiles/Create
        public IActionResult Create()
        {
            // Pets that already have a profile
            var usedPetIds = _context.PetProfiles
                .Select(pp => pp.PetId)
                .ToList();

            // Only show pets that don't have a profile yet
            var petsWithoutProfile = _context.Pets
                .Where(p => !usedPetIds.Contains(p.Id))
                .ToList();

            ViewData["PetId"] = new SelectList(petsWithoutProfile, "Id", "Name");
            return View();
        }

        // POST: PetProfiles/Create
        [HttpPost]
        public async Task<IActionResult> Create(PetProfile petProfile, IFormFile notesFile)
        {
            // If a file is uploaded, read its text and override VetNotes
            if (notesFile != null && notesFile.Length > 0)
            {
                using var reader = new StreamReader(notesFile.OpenReadStream());
                var text = await reader.ReadToEndAsync();
                petProfile.VetNotes = text;
            }

            _context.PetProfiles.Add(petProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PetProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var profile = await _context.PetProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Name", profile.PetId);
            return View(profile);
        }

        // POST: PetProfiles/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PetProfile petProfile, IFormFile notesFile)
        {
            if (id != petProfile.Id) return NotFound();

            if (notesFile != null && notesFile.Length > 0)
            {
                using var reader = new StreamReader(notesFile.OpenReadStream());
                var text = await reader.ReadToEndAsync();
                petProfile.VetNotes = text;
            }

            _context.Entry(petProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PetProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var profile = await _context.PetProfiles
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null) return NotFound();

            return View(profile);
        }

        // POST: PetProfiles/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.PetProfiles.FindAsync(id);
            if (profile != null)
            {
                _context.PetProfiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
