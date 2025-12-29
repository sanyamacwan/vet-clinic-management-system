using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinicSystem.Models;

namespace VetClinicSystem.Controllers
{
    public class VetDoctorsController : Controller
    {
        private readonly AppDbContext _context;

        public VetDoctorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VetDoctors
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.VetDoctors.ToListAsync();
            return View(doctors);
        }

        // GET: VetDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.VetDoctors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // GET: VetDoctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VetDoctors/Create
        [HttpPost]
        public async Task<IActionResult> Create(VetDoctor vetDoctor)
        {
            // ALWAYS try to save
            _context.VetDoctors.Add(vetDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: VetDoctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vetDoctor = await _context.VetDoctors.FindAsync(id);
            if (vetDoctor == null) return NotFound();

            return View(vetDoctor);
        }

        // POST: VetDoctors/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, VetDoctor vetDoctor)
        {
            if (id != vetDoctor.Id) return NotFound();

            // ALWAYS try to update
            _context.Entry(vetDoctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: VetDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vetDoctor = await _context.VetDoctors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vetDoctor == null) return NotFound();

            return View(vetDoctor);
        }

        // POST: VetDoctors/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vetDoctor = await _context.VetDoctors.FindAsync(id);
            if (vetDoctor != null)
            {
                _context.VetDoctors.Remove(vetDoctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
