// These are the namespaces (libraries) we need to make the code work.
// They allow access to the database context, models, configuration tools, and EF Core testing features.
using MegaTravelAPI.Data;
using MegaTravelAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// The [TestClass] attribute tells the testing framework (MSTest) that this class contains unit tests.
// MSTest will look inside this class and automatically run any methods marked with [TestMethod].
[TestClass]
public sealed class UserControllerTests
{
    // This field will hold our test configuration data.
    // The 'null!' means “I promise this will be initialized later” to avoid compiler warnings.
    private IConfiguration _config = null!;

    // The [TestInitialize] method runs *before* each test.
    // It’s used to set up anything that should exist fresh for every test (like configuration values).
    [TestInitialize]
    public void Init()
    {
        // Here we create a small, fake configuration source — a dictionary (key/value pairs)
        // that mimics what would normally come from appsettings.json.
        var dict = new Dictionary<string, string?>
        {
            // We provide a pretend database connection string.
            // EF Core will ignore this since we’re using an in-memory database for testing.
            { "MegaTravel:DatabaseConnectionString", "Ignored-For-InMemory" }
        };

        // Build the configuration object using our dictionary.
        // This simulates what ASP.NET Core normally does when it loads configuration files.
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(dict)
            .Build();
    }

    // This helper method creates a temporary, in-memory database.
    // It behaves just like a real SQL Server database, but it only exists in memory for testing.
    // Once the test finishes, the data disappears.
    private MegaTravelContext CreateContext(string dbName)
    {
        // Step 1: Build database options that tell EF Core to use an in-memory provider.
        var options = new DbContextOptionsBuilder<MegaTravelContext>()
            .UseInMemoryDatabase(databaseName: dbName) // Give each test its own isolated database name.
            .Options;

        // Step 2: Create the actual database context (the bridge to the database).
        var ctx = new MegaTravelContext(options, _config);

        // Step 3: Ensure the in-memory database exists before we use it.
        ctx.Database.EnsureCreated();

        // Step 4: Return this context to be used by tests.
        return ctx;
    }

    // This is our actual test.
    // [TestMethod] tells MSTest to treat this method as a runnable test case.
    [TestMethod]
    public async Task UpdateUserRecord_InvalidUser_ReturnsNotFound()
    {
        // -----------------------
        // ARRANGE PHASE
        // -----------------------
        // The “Arrange” step sets up all the data and context needed before running the actual code.
        // In this case, we’re testing what happens if a user tries to update their profile,
        // but that user doesn’t exist in the database.

        // Create a "fake" user record that doesn’t exist in our in-memory database.
        // We’ll use this to simulate an invalid update request.
        var invalidUser = new UserData
        {
            UserId = 999, // Intentionally invalid — no record with this ID will exist
            FirstName = "Ghost",
            LastName = "User",
            Email = "ghost@example.com"
        };

        // Next, we create a UserController instance from our MegaTravel API.
        // Normally, ASP.NET Core automatically builds this controller with dependency injection.
        // But here, we create it manually so that we can call its methods directly.
        //
        // The controller automatically creates a database context (MegaTravelContext)
        // and a UserDAL (data access layer) using the configuration we passed in.
        var controller = new MegaTravelAPI.Controllers.UserController(_config);

        // -----------------------
        // ACT PHASE
        // -----------------------
        // The “Act” step is where we execute the code being tested.
        // We call the UpdateUserRecord method with our invalid user data.
        //
        // Because we marked the test as async, we use 'await' to wait for the method to finish.
        var result = await controller.UpdateUserRecord(invalidUser);

        // -----------------------
        // ASSERT PHASE
        // -----------------------
        // The “Assert” step checks that the results match what we expected.
        // If any of these checks fail, MSTest will report the test as failed.

        // Check 1: The result’s Status property should be false
        // because updating a non-existent user should not succeed.
        Assert.IsFalse(result.Status, "Should return false for missing user.");

        // Check 2: The status code should be 500.
        // In this project, 500 represents an internal error or failure.
        Assert.AreEqual(500, result.StatusCode, "Expected a failure code.");

        // Check 3: The message should clearly indicate an update failure.
        Assert.AreEqual("Update Failed", result.Message);
    }
}
