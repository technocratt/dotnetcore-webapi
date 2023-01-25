using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Controllers;
using webapi.Services;
using webapi.Models;
using Microsoft.AspNetCore.Http;
using webapi.Utils;
using Microsoft.Net.Http.Headers;

namespace test.Controllers;

public class PatientControllerTest
{
    private readonly Faker<Patient> _mockPatient;

    private readonly Mock<IPatientService> _patientService;

    private readonly PatientController _controller;

    public PatientControllerTest()
    {
        _mockPatient = new Faker<Patient>()
                        .StrictMode(true)
                        .RuleFor(u => u.Id, f => f.Random.Int())
                        .RuleFor(u => u.Name, f => f.Person.FullName)
                        .RuleFor(u => u.Age, f => f.Random.Number(1, 100));

        _patientService = new Mock<IPatientService>();

        _controller = new PatientController(_patientService.Object);
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public void GetTest()
    {
        var expectedStatus = 200;
        var expectedResult = _mockPatient.GenerateBetween(1, 10);

        //// Arrange
        _patientService.Setup(_ => _.GetAllPatients()).Returns(expectedResult);

        //// Act
        var result = (ObjectResult)_controller.Get();

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedResult);
        result.Value.As<List<Patient>>().Count().Should().Be(expectedResult.Count());
    }

    [Fact]
    public void GetByIdTest()
    {
        var expectedStatus = 200;
        var expectedResult = _mockPatient.Generate();
        var patientId = expectedResult.Id;

        //// Arrange
        _patientService.Setup(_ => _.GetPatientById(It.IsAny<int>())).Returns(expectedResult);

        //// Act
        var result = (ObjectResult)_controller.GetById(patientId);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedResult);
    }

    [Fact]
    public async void PostTest()
    {
        var expectedStatus = 201;
        var expectedResult = true;
        var patientToCreate = _mockPatient.Generate();

        //// Arrange
        _patientService.Setup(_ => _.CreatePatient(It.IsAny<Patient>())).ReturnsAsync(expectedResult);

        //// Act
        var result = (ObjectResult)await _controller.Post(patientToCreate);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(patientToCreate);
    }

    [Fact]
    public async void PutTest()
    {
        var expectedStatus = 200;
        var expectedResult = true;
        var patientToModify = _mockPatient.Generate();
        var patientId = patientToModify.Id;

        //// Arrange
        _patientService.Setup(_ => _.GetPatientById(It.IsAny<int>())).Returns(patientToModify);
        _patientService.Setup(_ => _.ModifyPatient(It.IsAny<int>(), It.IsAny<Patient>())).ReturnsAsync(expectedResult);
        _controller.ControllerContext.HttpContext.Request.Headers.Add(HeaderNames.IfMatch, patientToModify.ToETag());

        //// Act
        var result = (ObjectResult)await _controller.Put(patientId, patientToModify);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedResult);
    }

    [Fact]
    public async void PutNegativeTest()
    {
        var expectedStatus = 412;
        var expectedResult = true;
        var patientToModify = _mockPatient.Generate();
        var patientId = patientToModify.Id;

        //// Arrange
        _patientService.Setup(_ => _.GetPatientById(It.IsAny<int>())).Returns(patientToModify);
        _patientService.Setup(_ => _.ModifyPatient(It.IsAny<int>(), It.IsAny<Patient>())).ReturnsAsync(expectedResult);
        _controller.ControllerContext.HttpContext.Request.Headers.Add(HeaderNames.IfMatch, patientToModify.ToETag() + 1);

        //// Act
        var result = (StatusCodeResult)await _controller.Put(patientId, patientToModify);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
    }

    [Fact]
    public async void DeleteTest()
    {
        var expectedStatus = 200;
        var expectedResult = true;
        var patientToDelete = _mockPatient.Generate();
        var patientId = patientToDelete.Id;

        //// Arrange
        _patientService.Setup(_ => _.DeletePatient(It.IsAny<int>())).ReturnsAsync(expectedResult);

        //// Act
        var result = (ObjectResult)await _controller.Delete(patientId);

        //// Assert
        result.Should().NotBe(null);
        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().Be(expectedResult);
    }
}