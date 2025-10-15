using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MegaTravelAPI.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

// --- Example contracts you likely already have ---
public interface IUserDAL
{
    Task<bool> UpdateUserProfileAsync(UpdateUserProfileRequest request); // true = updated, false = user not found
}

public class UpdateUserProfileRequest
{
    [Required]
    public int UserId { get; set; }

    [Required, StringLength(100)]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(2)]
    public string State { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string Phone { get; set; } = string.Empty;
}

// --- Mock controller under test for seam testing ---
public class MockUserController : ControllerBase
{
    private readonly IUserDAL _dal;
    public MockUserController(IUserDAL dal) => _dal = dal;

    [HttpPost]
    public async Task<IActionResult> EditProfile([FromBody] UpdateUserProfileRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var updated = await _dal.UpdateUserProfileAsync(request);
        if (!updated) return NotFound();

        return Ok(); // or NoContent()
    }
}

// --- TESTS ---
[TestClass]
public class UserController_EditProfile_Tests
{
    // ✅ Test #1: DAL says “user not found” → controller returns 404 NotFound
    [TestMethod]
    public async Task EditProfile_WhenUserNotFound_Returns404NotFound()
    {
        // Arrange
        var mockDal = new Mock<IUserDAL>();
        mockDal.Setup(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()))
               .ReturnsAsync(false); // DAL signals user not found

        var controller = new MockUserController(mockDal.Object);
        var req = new UpdateUserProfileRequest
        {
            UserId = 999,
            Address = "404 Nowhere Rd",
            City = "Nowhere",
            State = "NA",
            Phone = "555-0000"
        };

        // Act
        var result = await controller.EditProfile(req);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        mockDal.Verify(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()), Times.Once);
    }

    // ✅ Test #2: DAL returns success → controller returns 200 OK
    [TestMethod]
    public async Task EditProfile_WhenUpdateSucceeds_Returns200Ok()
    {
        // Arrange
        var mockDal = new Mock<IUserDAL>();
        mockDal.Setup(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()))
               .ReturnsAsync(true); // DAL signals successful update

        var controller = new MockUserController(mockDal.Object);
        var req = new UpdateUserProfileRequest
        {
            UserId = 1,
            Address = "123 Main St",
            City = "Springfield",
            State = "MO",
            Phone = "5551234567"
        };

        // Act
        var result = await controller.EditProfile(req);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
        mockDal.Verify(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()), Times.Once);
    }

    // ✅ Test #3: Real MegaTravel controller → mock repository injected via reflection
    [TestMethod]
    public async Task UpdateUserRecord_SuccessfulUpdate_Returns200Ok()
    {
        // Arrange
        var mockDal = new Mock<IUser>();
        var expectedUser = new User
        {
            UserId = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            Street1 = "123 Main St",
            City = "Springfield",
            State = "MO",
            ZipCode = 65802,
            Phone = "5559998888"
        };

        mockDal.Setup(d => d.UpdateUserRecord(It.IsAny<UserData>()))
               .ReturnsAsync(new SaveUserResponse
               {
                   Status = true,
                   StatusCode = 200,
                   Message = "Update Successful",
                   Data = expectedUser
               });

        var mockConfig = new Mock<IConfiguration>();

        // Create the REAL MegaTravel controller
        var controller = new MegaTravelAPI.Controllers.UserController(mockConfig.Object);

        // Inject mock DAL into private field 'repository'
        var repoField = typeof(MegaTravelAPI.Controllers.UserController)
            .GetField("repository", BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new InvalidOperationException("repository field not found on UserController");

        repoField.SetValue(controller, mockDal.Object);

        var userData = new UserData
        {
            UserId = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            Street1 = "123 Main St",
            City = "Springfield",
            State = "MO",
            ZipCode = 65802,
            Phone = "5551112222"
        };

        // Act
        var result = await controller.UpdateUserRecord(userData);

        // Assert
        Assert.IsTrue(result.Status);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual("Update Successful", result.Message);
        mockDal.Verify(d => d.UpdateUserRecord(It.Is<UserData>(u => u.UserId == 1)), Times.Once);
    }
}
