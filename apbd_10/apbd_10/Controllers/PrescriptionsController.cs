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
        
        List <Prescription> prescriptions = await service.getPrescriptionsOfPatient(id);
        List<GetPrescriptionDto> prescriptionDtos = new List<GetPrescriptionDto>();
        for (int i = 0; i < prescriptions.Count; i++)
        {
            Doctor doctor = await service.getDoctor(prescriptions[i].IdDoctor);
            DoctorDto doctorDto = new DoctorDto()
            {
                IdDoctor = doctor.IdDoctor,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            };
            
            List<Medicament> medicaments = await service.getMedicamentsOfPrescription(prescriptions[i].IdPrescription);
            List<MedicamentDto> medicamentDtos = new List<MedicamentDto>();
            for (int j = 0; j < medicaments.Count; j++)
            {
                medicamentDtos.Add(new MedicamentDto()
                {
                    IdMedicament = medicaments[i].IdMedicament,
                    Name = medicaments[i].Name,
                    Description = medicaments[i].Description,
                    Type = medicaments[i].Type
                });
            }
            
            prescriptionDtos.Add(new GetPrescriptionDto()
            {
                IdPrescription = prescriptions[i].IdPrescription,
                date = prescriptions[i].Date,
                dueDate = prescriptions[i].DueDate,
                doctor = doctorDto,
                medicaments = medicamentDtos
            });
        }
        
        var patientDto = new GetPatientDto()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            prescriptions = prescriptionDtos
        };
        return Ok(patientDto);
    }
    
}