using System.Transactions;
using apbd_10.Data;
using apbd_10.models;
using apbd_10.models.DTOs;
using apbd_10.Models.DTOs;
using apbd_10.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace apbd_10.Controllers;

[ApiController]
public class PrescriptionsController: ControllerBase
{
    private readonly IDbService service;

    public PrescriptionsController(IDbService service)
    {
        this.service = service;
    }

    [HttpPost("addPrescription")]
    public async Task<IActionResult> AddPrescription(NewPrescriptionDto dto)
    {
        for (int i = 0; i < dto.medicaments.Count; i++)
        {
            var medicamentId = dto.medicaments[i].IdMedicament;
            if (!await service.DoesMedicamentExist(medicamentId)) return BadRequest("Medicament with id = " + medicamentId + " does not exist");
        }

        if (dto.medicaments.Count > 10) return BadRequest("A prescription can include a maximum of 10 medications");
        if (dto.dueDate <= dto.date) return BadRequest("dueDate can not be earlier than date");

        Prescription prescription;
        if (!await service.DoesPatientExist(dto.patient.IdPatient))
        {
            var p = new Patient()
            {
                FirstName = dto.patient.FirstName,
                LastName = dto.patient.LastName,
                BirthDate = dto.patient.BirthDate
            };
            await service.addPatient(p);
            prescription = new Prescription()
            {
                IdDoctor = dto.doctor.IdDoctor,
                Date = dto.date,
                DueDate = dto.dueDate,
                Patient = p
            };
        }
        else
        {
            prescription = new Prescription()
            {
                IdDoctor = dto.doctor.IdDoctor,
                Date = dto.date,
                DueDate = dto.dueDate,
                IdPatient = dto.patient.IdPatient
            };
        }
        await service.addPrescription(prescription);
        
        return Created();
    }

    [HttpGet("getPatient")]
    public async Task<IActionResult> GetPatient(int id)
    {
        Patient patient = await service.getPatient(id);
        return Ok(new GetPatientDto()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            prescriptions = patient.Prescriptions.Select(pr => new GetPrescriptionDto()
            {
                IdPrescription = pr.IdPrescription,
                date = pr.Date,
                dueDate = pr.DueDate,
                doctor = new DoctorDto()
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName,
                    Email = pr.Doctor.Email
                },
                medicaments = pr.PrescriptionMedicaments.Select(prm => new MedicamentDto()
                {
                    IdMedicament = prm.IdMedicament,
                    Name = prm.Medicament.Name,
                    Description = prm.Medicament.Description,
                    Type = prm.Medicament.Type
                }).ToList()
            }).OrderBy(p => p.dueDate)
                .ToList()
        });
    }
    
}