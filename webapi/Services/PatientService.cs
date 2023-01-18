using JsonFlatFileDataStore;
using webapi.Models;

namespace webapi.Services;

public interface IPatientService
{
    IEnumerable<Patient> GetAllPatients();

    Task<bool> CreatePatient(Patient patient);
}

public class PatientService : IPatientService
{
    private readonly IDocumentCollection<Patient> patientCollection;

    public PatientService()
    {
        var store = new DataStore("data.json");
        patientCollection = store.GetCollection<Patient>();
    }

    public IEnumerable<Patient> GetAllPatients()
    {
        return patientCollection.AsQueryable().ToList();
    }

    public async Task<bool> CreatePatient(Patient patient)
    {
        return await patientCollection.InsertOneAsync(patient);
    }
}