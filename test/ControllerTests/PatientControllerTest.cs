using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Controllers;
using webapi.Services;
using webapi.Models;
using Microsoft.AspNetCore.Http;

namespace test.Controllers;

public class PatientControllerTest
{
    private readonly Faker<Patient> mockPatient;

    public PatientControllerTest()
    {
        mockPatient = new Faker<Patient>()
                        .StrictMode(true)
                        .RuleFor(u => u.Id, f => f.Random.Int())
                        .RuleFor(u => u.Name, f => f.Person.FullName)
                        .RuleFor(u => u.Age, f => f.Random.Number(1, 100));
    }

    [Fact]
    public void GetTest()
    {
        var expectedStatus = 200;
        var expectedOutput = mockPatient.GenerateBetween(1, 10);

        //// Arrange
        var patientService = new Mock<IPatientService>();
        patientService.Setup(_ => _.GetAllPatients()).Returns(expectedOutput);
        var controller = new PatientController(patientService.Object);

        //// Act
        var result = (ObjectResult)controller.Get();

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedOutput);
        result.Value.As<List<Patient>>().Count().Should().Be(expectedOutput.Count());
    }

    [Fact]
    public void GetByIdTest()
    {
        var expectedStatus = 200;
        var expectedOutput = mockPatient.Generate();

        //// Arrange
        var patientService = new Mock<IPatientService>();
        patientService.Setup(_ => _.GetPatientById(expectedOutput.Id)).Returns(expectedOutput);
        var controller = new PatientController(patientService.Object);

        //// Act
        var result = (ObjectResult)controller.GetById(expectedOutput.Id);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedOutput);
    }

    [Fact]
    public async void PostTest()
    {
        var expectedStatus = 201;
        var expectedOutput = true;
        var patientToCreate = mockPatient.Generate();

        //// Arrange
        var patientService = new Mock<IPatientService>();
        patientService.Setup(_ => _.CreatePatient(patientToCreate)).ReturnsAsync(expectedOutput);
        var controller = new PatientController(patientService.Object);

        //// Act
        var result = (ObjectResult)await controller.Post(patientToCreate);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(patientToCreate);
    }

    [Fact]
    public async void PutTest()
    {
        var expectedStatus = 200;
        var expectedOutput = true;
        var patientToModify = mockPatient.Generate();

        //// Arrange
        var patientService = new Mock<IPatientService>();
        patientService.Setup(_ => _.GetPatientById(patientToModify.Id)).Returns(patientToModify);
        patientService.Setup(_ => _.ModifyPatient(patientToModify.Id, patientToModify)).ReturnsAsync(expectedOutput);
        
        var controller = new PatientController(patientService.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();

        //// Act
        var result = (ObjectResult)await controller.Put(patientToModify.Id, patientToModify);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedOutput);
    }

    [Fact]
    public async void DeleteTest()
    {
        var expectedStatus = 200;
        var expectedOutput = true;
        var patientToDelete = mockPatient.Generate();

        //// Arrange
        var patientService = new Mock<IPatientService>();
        patientService.Setup(_ => _.DeletePatient(patientToDelete.Id)).ReturnsAsync(expectedOutput);
        var controller = new PatientController(patientService.Object);

        //// Act
        var result = (ObjectResult)await controller.Delete(patientToDelete.Id);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedOutput);
    }
}