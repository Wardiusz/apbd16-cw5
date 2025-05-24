using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Entities;

namespace Tutorial5.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(PrescriptionRequestDto dto)
        {
            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("Prescription cannot contain more than 10 medicaments.");

            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate must be greater than or equal to Date.");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.FirstName == dto.Patient.FirstName && p.LastName == dto.Patient.LastName && p.BirthDate == dto.Patient.BirthDate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = dto.Patient.FirstName,
                    LastName = dto.Patient.LastName,
                    BirthDate = dto.Patient.BirthDate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var doctor = await _context.Doctors.FindAsync(dto.Doctor.IdDoctor);
            if (doctor == null)
                throw new KeyNotFoundException("Doctor not found.");

            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                PatientId = patient.IdPatient,
                DoctorId = doctor.IdDoctor
            };
            _context.Prescriptions.Add(prescription);

            foreach (var medDto in dto.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(medDto.IdMedicament);
                if (medicament == null)
                    throw new KeyNotFoundException($"Medicament with id {medDto.IdMedicament} not found.");

                var pm = new PrescriptionMedicament
                {
                    Prescription = prescription,
                    Medicament = medicament,
                    Dose = medDto.Dose,
                    Description = medDto.Description
                };
                _context.PrescriptionMedicaments.Add(pm);
            }

            await _context.SaveChangesAsync();
        }
    }
}