
using MegaTravelAPI.Data;
using MegaTravelAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;




[TestClass]
public sealed class UserControllerTests
{

    private IConfiguration _config = null!;

    [TestInitialize]
    public void Init()
    {
        var dict = new Dictionary<string, string?>
        {
            { "MegaTravel:DatabaseConnectionString", "Ignored-For-InMemory" }
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(dict)
            .Build();
    }

    private MegaTravelContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MegaTravelContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var ctx = new MegaTravelContext(options, _config);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [TestMethod]
    public async Task UpdateUserRecord_InvalidUser_ReturnsNotFound()
    {
        // Arrange
        // test a single invalid user
        // Create a test user that doesn't exist in the DB
        var invalidUser = new UserData
        {
            UserId = 999, // invalid ID
            FirstName = "Ghost",
            LastName = "User",
            Email = "ghost@example.com"
        };

        // Create controller (it builds DAL automatically)
        var controller = new MegaTravelAPI.Controllers.UserController(_config);

        
        // Act
        var result = await controller.UpdateUserRecord(invalidUser);

        // Assert
        Assert.IsFalse(result.Status, "Should return false for missing user.");
        Assert.AreEqual(500, result.StatusCode, "Expected a failure code.");
        Assert.AreEqual("Update Failed", result.Message);
    }

   



}
