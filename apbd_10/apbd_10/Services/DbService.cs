using apbd_10.Data;
using apbd_10.models;
using Microsoft.EntityFrameworkCore;

namespace apbd_10.Services;

public class DbService: IDbService
{
    private readonly ApplicationContext _context;

    public DbService(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task addPatient(Patient patient)
    {
        await _context.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task addPrescription(Prescription prescription)
    {
        await _context.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DoesPatientExist(int id)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == id);
    }

    public async Task<bool> DoesMedicamentExist(int id)
    {
        return await _context.Medicaments.AnyAsync(m => m.IdMedicament == id);
    }

    public async Task<Patient> getPatient(int id)
    {
        return await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(p => p.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Where(p => p.IdPatient == id)
            .FirstAsync();
    }
    
    public async Task<Doctor> getDoctor(int id)
    {
        return _context.Doctors.Find(id);
    }

    public async Task<List<Prescription>> getPrescriptionsOfPatient(int id)
    {
        return _context.Prescriptions.Where(p => p.IdPatient == id).ToList();
    }
    
    public async Task<List<Medicament>> getMedicamentsOfPrescription(int id)
    {
        List<int> medicamentsId = _context.PrescriptionMedicaments
            .Where(p => p.IdPrescription == id)
            .Select(p => p.IdMedicament).ToList();
        
        List<Medicament> medicaments = new List<Medicament>();
        for (int i = 0; i < medicamentsId.Count; i++)
        {
            medicaments.Add(await getMedicament(medicamentsId[i]));
        }
        return medicaments;
    }
    
    public async Task<Medicament> getMedicament(int id)
    {
        return _context.Medicaments.Find(id);
    }
}