// These are the namespaces (libraries) that give us access to the tools we need:
// - Moq: for creating "fake" versions of interfaces or classes.
// - MSTest: for defining and running unit tests.
// - ASP.NET Core MVC: for controller and IActionResult testing.
// - MegaTravelAPI.*: for our real data models and interfaces.
// - System.*: for general .NET classes and async support.
using MegaTravelAPI.Data;
using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

// ================================
// EXAMPLE INTERFACE AND MODEL SETUP
// ================================

// This is a simplified version of a Data Access Layer (DAL) interface.
// It defines one operation: updating a user's profile.
// The method returns a Task<bool> — meaning it runs asynchronously
// and returns true if the update succeeded or false if the user was not found.
public interface IUserDAL
{
    Task<bool> UpdateUserProfileAsync(UpdateUserProfileRequest request); // true = updated, false = user not found
}

// This class represents the user profile update request that a client might send to the API.
// It includes validation attributes like [Required] and [StringLength] to ensure
// the data is valid before processing.
public class UpdateUserProfileRequest
{
    [Required] // UserId must be present
    public int UserId { get; set; }

    [Required, StringLength(100)] // Address must be present and under 100 chars
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(50)] // City must be present and under 50 chars
    public string City { get; set; } = string.Empty;

    [Required, StringLength(2)] // State abbreviation, 2 characters
    public string State { get; set; } = string.Empty;

    [Required, StringLength(10)] // Phone number, max 10 characters
    public string Phone { get; set; } = string.Empty;
}

// ================================
// CONTROLLER UNDER TEST
// ================================

// This is a mock version of a controller similar to what you'd see in an ASP.NET Core API.
// It's designed specifically for *seam testing* — meaning we’re testing the “seam” (boundary)
// between the controller and its data access layer.
public class MockUserController : ControllerBase
{
    private readonly IUserDAL _dal; // This is the seam — the DAL dependency.

    // The constructor receives the DAL interface (IUserDAL) as a dependency.
    // In a real application, this would be provided automatically by dependency injection.
    public MockUserController(IUserDAL dal) => _dal = dal;

    // This method represents the EditProfile API endpoint.
    // It uses [HttpPost] to accept data from a client and [FromBody] to bind JSON into a C# object.
    [HttpPost]
    public async Task<IActionResult> EditProfile([FromBody] UpdateUserProfileRequest request)
    {
        // Step 1: Validate the incoming model.
        // If the data violates any of the validation attributes (e.g., missing [Required] field),
        // ASP.NET adds errors to ModelState, and we immediately return a 400 Bad Request.
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        // Step 2: Ask the DAL to perform the update.
        // This calls the data layer — our seam — to attempt the update.
        var updated = await _dal.UpdateUserProfileAsync(request);

        // Step 3: Interpret the DAL’s response.
        // If the DAL says “false,” it means the user doesn’t exist — return a 404 NotFound.
        if (!updated) return NotFound();

        // Step 4: Otherwise, it succeeded — return 200 OK.
        return Ok(); // Could also be NoContent() in some APIs.
    }
}

// ================================
// UNIT TESTS
// ================================

// This test class contains *seam tests* for the controller behavior.
// Seam tests isolate the controller from its real database layer by using mocks.
[TestClass]
public class UserController_EditProfile_Tests
{
    // -----------------------------
    // ✅ TEST #1
    // DAL says “user not found” → Controller should return 404 NotFound.
    // -----------------------------
    [TestMethod]
    public async Task EditProfile_WhenUserNotFound_Returns404NotFound()
    {
        // ARRANGE
        // Create a mock DAL object that implements IUserDAL.
        var mockDal = new Mock<IUserDAL>();

        // Tell the mock what to do: whenever UpdateUserProfileAsync is called,
        // return "false" (meaning user not found).
        mockDal.Setup(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()))
               .ReturnsAsync(false);

        // Create a controller and inject our fake DAL.
        var controller = new MockUserController(mockDal.Object);

        // Create a fake user request with valid data.
        var req = new UpdateUserProfileRequest
        {
            UserId = 999,
            Address = "404 Nowhere Rd",
            City = "Nowhere",
            State = "NA",
            Phone = "555-0000"
        };

        // ACT
        // Call the controller action with the test request.
        var result = await controller.EditProfile(req);

        // ASSERT
        // Verify the controller responded with the correct type of HTTP result (404 NotFound).
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        // Also check that the DAL method was called exactly once.
        mockDal.Verify(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()), Times.Once);
    }

    // -----------------------------
    // ✅ TEST #2
    // DAL says “update succeeded” → Controller should return 200 OK.
    // -----------------------------
    [TestMethod]
    public async Task EditProfile_WhenUpdateSucceeds_Returns200Ok()
    {
        // ARRANGE
        // Create a mock DAL again, this time telling it to return "true" (success).
        var mockDal = new Mock<IUserDAL>();
        mockDal.Setup(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()))
               .ReturnsAsync(true);

        // Create the controller and pass in our fake DAL.
        var controller = new MockUserController(mockDal.Object);

        // Create a request with valid data that represents a successful update scenario.
        var req = new UpdateUserProfileRequest
        {
            UserId = 1,
            Address = "123 Main St",
            City = "Springfield",
            State = "MO",
            Phone = "5551234567"
        };

        // ACT
        // Call the EditProfile method.
        var result = await controller.EditProfile(req);

        // ASSERT
        // Expect the controller to return an HTTP 200 OK response.
        Assert.IsInstanceOfType(result, typeof(OkResult));

        // Verify that the DAL’s update method was called once.
        mockDal.Verify(d => d.UpdateUserProfileAsync(It.IsAny<UpdateUserProfileRequest>()), Times.Once);
    }

    // -----------------------------
    // ✅ TEST #3
    // Reflection injection example — using the REAL MegaTravel controller.
    // Here, instead of seam testing, we’re doing a *hybrid* approach:
    //   We use the real controller but inject a fake repository manually.
    // -----------------------------
    [TestMethod]
    public async Task UpdateUserRecord_SuccessfulUpdate_Returns200Ok()
    {
        // ARRANGE
        // Step 1: Create a mock IUser (this is the real interface used in MegaTravel).
        var mockDal = new Mock<IUser>();

        // Build an expected user record that would represent a successful update.
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

        // Step 2: Tell the mock DAL what to return when UpdateUserRecord() is called.
        mockDal.Setup(d => d.UpdateUserRecord(It.IsAny<UserData>()))
               .ReturnsAsync(new SaveUserResponse
               {
                   Status = true,
                   StatusCode = 200,
                   Message = "Update Successful",
                   Data = expectedUser
               });

        // Step 3: Create a mock configuration object — not important for logic here.
        var mockConfig = new Mock<IConfiguration>();

        // Step 4: Create the actual MegaTravel UserController.
        // This is the *real* API controller, not the mock one.
        var controller = new MegaTravelAPI.Controllers.UserController(mockConfig.Object);

        // Step 5: Use reflection to inject our mock DAL into the controller’s private field.
        // Normally, this field isn’t accessible from outside, but we can use reflection to set it.
        var repoField = typeof(MegaTravelAPI.Controllers.UserController)
            .GetField("repository", BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new InvalidOperationException("repository field not found on UserController");

        // Replace the real repository with our fake one.
        repoField.SetValue(controller, mockDal.Object);

        // Step 6: Create a user record to pass to the controller.
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
            Phone = "5551112222" // Slightly different from expected to simulate change
        };

        // ACT
        // Call the real UpdateUserRecord method.
        var result = await controller.UpdateUserRecord(userData);

        // ASSERT
        // Validate that the update succeeded with the correct return values.
        Assert.IsTrue(result.Status);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual("Update Successful", result.Message);

        // Verify the mock DAL was actually called once with the correct user ID.
        mockDal.Verify(d => d.UpdateUserRecord(It.Is<UserData>(u => u.UserId == 1)), Times.Once);
    }

    [TestMethod]
    public async Task GetTripsByUser_ReturnsOnlyTripsForThatUser()
    {
        // -----------------------------
        // ARRANGE: Set up our test environment
        // -----------------------------

        // Create a unique name for our in-memory database.
        // Each test run gets its own isolated "fake" database to avoid data mixing.
        var dbName = Guid.NewGuid().ToString();

        // Simulate an application configuration source.
        // This lets MegaTravelContext read settings as if they came from appsettings.json.
        var dict = new Dictionary<string, string?> { { "MegaTravel:DatabaseConnectionString", "InMemory" } };
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();

        // Build the EF Core DbContext options and tell EF to use an in-memory database.
        // No real SQL Server is needed — the data is stored temporarily in memory.
        var options = new DbContextOptionsBuilder<MegaTravelContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        // Create a new database context using the in-memory setup.
        // "using var" ensures the context is properly disposed at the end of the test.
        using var context = new MegaTravelContext(options, config);

        // -----------------------------
        // SEED REQUIRED DATA
        // -----------------------------

        // 1️⃣ Add test Users
        // These represent customers booking trips.
        // EF Core requires all non-nullable fields to have values,
        // so we fill in realistic data for both users.
        context.Users.AddRange(
            new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                Street1 = "123 Main St",
                City = "Springfield",
                State = "MO",
                ZipCode = 65802,
                Phone = "5551112222"
            },
            new User
            {
                UserId = 2,
                FirstName = "Mary",
                LastName = "Jones",
                Email = "mary.jones@example.com",
                Street1 = "456 Oak Ave",
                City = "Springfield",
                State = "MO",
                ZipCode = 65803,
                Phone = "5552223333"
            }
        );

        // 2️⃣ Add test Agents
        // Agents represent travel agents assigned to manage trips.
        // OfficeLocation and Phone are required fields, so we include them.
        context.Agents.AddRange(
            new Agent
            {
                AgentId = 1,
                FirstName = "Agent",
                LastName = "One",
                OfficeLocation = "Springfield HQ",
                Phone = "5557778888"
            },
            new Agent
            {
                AgentId = 2,
                FirstName = "Agent",
                LastName = "Two",
                OfficeLocation = "Kansas City Office",
                Phone = "5559990000"
            }
        );

        // 3️⃣ Add test Trips
        // Each trip connects a User and an Agent.
        // We're creating 3 trips — 2 for UserID 1 and 1 for UserID 2.
        // Later we’ll verify that the DAL correctly returns only the first 2.
        context.Trips.AddRange(
            new Trip
            {
                TripId = 1,
                UserId = 1,
                AgentId = 1,
                TripName = "Beach Getaway",
                Location = "Miami",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                NumAdults = 2,
                NumChildren = 0
            },
            new Trip
            {
                TripId = 2,
                UserId = 1,
                AgentId = 1,
                TripName = "Mountain Retreat",
                Location = "Aspen",
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                NumAdults = 2,
                NumChildren = 2
            },
            new Trip
            {
                TripId = 3,
                UserId = 2,
                AgentId = 2,
                TripName = "City Tour",
                Location = "New York",
                StartDate = DateTime.UtcNow.AddDays(20),
                EndDate = DateTime.UtcNow.AddDays(25),
                NumAdults = 1,
                NumChildren = 1
            }
        );

        // Commit all seeded data to the in-memory database.
        context.SaveChanges();

        // -----------------------------
        // ACT: Call the method under test
        // -----------------------------

        // Create the Data Access Layer object (DAL) using our fake context and config.
        // This is the same class the real API controller would use.
        var dal = new UserDAL(context, config);

        // Call the method we’re testing — it should return only trips for the given user.
        var result = dal.GetTripsByUser(1);

        // -----------------------------
        // ASSERT: Verify expected outcomes
        // -----------------------------

        // 1️⃣ Make sure the DAL didn’t return null (a broken query would do that).
        Assert.IsNotNull(result, "DAL returned null.");

        // 2️⃣ There should be exactly 2 trips for User 1.
        Assert.AreEqual(2, result.Count, "Expected 2 trips for User 1.");

        // 3️⃣ Double-check that every returned trip belongs to User 1.
        Assert.IsTrue(result.All(t => t.UserId == 1), "All trips should belong to User 1.");

        // If all assertions pass, it confirms:
        // ✅ The DAL filters correctly by user ID.
        // ✅ EF Core relationships are working.
        // ✅ Our seeding and context setup behave like a real database.
    }

}
