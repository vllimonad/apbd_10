using apbd_10.models;

namespace apbd_10.Services;

public interface IDbService
{
    Task addPatient(Patient patient);
    Task addPrescription(Prescription prescription);
    Task<bool> DoesPatientExist(int id);
    Task<bool> DoesMedicamentExist(int id);
    Task<Patient> getPatient(int id);
    Task<List<Prescription>> getPrescriptionsOfPatient(int id);
    Task<Doctor> getDoctor(int id);
    Task<List<Medicament>> getMedicamentsOfPrescription(int id);
    Task<Medicament> getMedicament(int id);
}