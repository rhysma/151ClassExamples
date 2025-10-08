using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

// --- Example controller under test ---
public class UserController : ControllerBase
{
    private readonly IUserDAL _dal;
    public UserController(IUserDAL dal) => _dal = dal;

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

    //If the DAL signals “user not found,” the controller should catch that and return a NotFound result.
    [TestMethod]
    public async Task EditProfile_WhenUserNotFound_Returns404NotFound()
    {
        // Arrange
        var mockDal = new Mock<IUserDAL>();
        mockDal.Setup(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()))
               .ReturnsAsync(false); // DAL signals user not found

        var controller = new UserController(mockDal.Object);
        var req = new UpdateUserProfileRequest
        {
            UserId = 999, // nonexistent
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

   
}
