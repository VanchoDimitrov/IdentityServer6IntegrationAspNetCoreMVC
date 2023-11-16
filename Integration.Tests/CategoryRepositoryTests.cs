using Integration.DataLayer;
using Integration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class CategoryRepositoryTests
{
    // Helper method to create a mock ApplicationDbContext
    private static ApplicationDbContext CreateMockContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void Add_ValidCategory_SavesToDatabase()
    {
        // Arrange
        var context = CreateMockContext();
        var loggerMock = new Mock<ILogger<CategoryRepository>>();
        var repository = new CategoryRepositoryTests(context, loggerMock.Object);

        var category = new Category { /* initialize valid category properties */ };

        // Act
        repository.Add(category);

        // Assert
        Assert.Single(context.categories);
        // Add more assertions as needed
    }

    [Fact]
    public void Delete_ExistingCategory_DeletesFromDatabase()
    {
        // Arrange
        var context = CreateMockContext();
        var loggerMock = new Mock<ILogger<CategoryRepository>>();
        var repository = new CategoryRepository(context, loggerMock.Object);

        var category = new Category { /* initialize category with existing ID */ };
        context.categories.Add(category);
        context.SaveChanges();

        // Act
        repository.Delete(category);

        // Assert
        Assert.Empty(context.categories);
        // Add more assertions as needed
    }

    [Fact]
    public void DeleteRange_ValidCategories_DeletesRangeFromDatabase()
    {
        // Arrange
        var context = CreateMockContext();
        var loggerMock = new Mock<ILogger<CategoryRepository>>();
        var repository = new CategoryRepository(context, loggerMock.Object);

        var categories = new List<Category>
        {
            new Category { /* initialize category 1 */ },
            new Category { /* initialize category 2 */ }
        };

        context.categories.AddRange(categories);
        context.SaveChanges();

        // Act
        repository.DeleteRange(categories);

        // Assert
        Assert.Empty(context.categories);
        // Add more assertions as needed
    }

    // Similar tests can be written for other methods like Get, GetAll, and Update.
}
