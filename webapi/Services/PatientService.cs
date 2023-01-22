using JsonFlatFileDataStore;
using webapi.Models;

namespace webapi.Services;

public interface IPatientService
{
    IEnumerable<Patient> GetAllPatients();

    Patient GetPatientById(int id);

    Task<bool> CreatePatient(Patient patient);

    Task<bool> ModifyPatient(int id, Patient patient);

    Task<bool> DeletePatient(int id);
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
        return patientCollection
                .AsQueryable()
                .ToList();
    }

    public Patient GetPatientById(int id)
    {
        return patientCollection
                .AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefault();
    }

    public async Task<bool> CreatePatient(Patient patient)
    {
        return await patientCollection.InsertOneAsync(patient);
    }

    public async Task<bool> ModifyPatient(int id, Patient patient)
    {
        var patientToModify = patientCollection
                                .AsQueryable()
                                .Where(x => x.Id == id)
                                .FirstOrDefault();

        patientToModify.Name = patient.Name;
        patientToModify.Age = patient.Age;

        return await patientCollection.UpdateOneAsync(id, patientToModify);
    }

    public async Task<bool> DeletePatient(int id)
    {
        return await patientCollection.DeleteOneAsync(x => x.Id == id);
    }
}