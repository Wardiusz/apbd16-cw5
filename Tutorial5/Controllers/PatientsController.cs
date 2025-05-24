using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;

namespace Tutorial5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Where(p => p.IdPatient == id)
                .Select(p => new
                {
                    p.IdPatient,
                    p.FirstName,
                    p.LastName,
                    p.BirthDate,
                    Prescriptions = p.Prescriptions
                        .OrderBy(pr => pr.DueDate)
                        .Select(pr => new
                        {
                            pr.IdPrescription,
                            pr.Date,
                            pr.DueDate,
                            Doctor = new
                            {
                                pr.Doctor.IdDoctor,
                                pr.Doctor.FirstName,
                                pr.Doctor.LastName
                            },
                            Medicaments = pr.PrescriptionMedicaments.Select(pm => new
                            {
                                pm.Medicament.IdMedicament,
                                pm.Medicament.Name,
                                pm.Dose,
                                pm.Description
                            }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }
    }
}