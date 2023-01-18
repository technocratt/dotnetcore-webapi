using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService patientService;

    public PatientController(IPatientService ps)
    {
        patientService = ps;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = patientService.GetAllPatients();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Patient patient)
    {
        var result = await patientService.CreatePatient(patient);
        return Created(nameof(Post), patient);
    }
}