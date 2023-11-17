using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Integration.DataLayer;
using Integration.DataLayer.Repositories.CategoryRepository;
using Integration.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Integration.Tests
{
    public class CategoryRepositoryTests
    {
        // Helper method to create a mock of ApplicationDbContext
        private static ApplicationDbContext CreateMockContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Add_ValidCategory_ShouldSaveToDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);
            var category = new Category { CategoryID = 1, Name = "Example Category", DisplayOrder = "1" };

            // Act
            categoryRepository.Add(category);

            // Assert
            Assert.Equal(1, mockContext.Categories.Count());
        }

        [Fact]
        public void Delete_ValidCategory_ShouldRemoveFromDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create a category and add it to the context
            var category = new Category { CategoryID = 2, Name = "Example Category", DisplayOrder = "1" };
            mockContext.Categories.Add(category);
            mockContext.SaveChanges();

            // Act
            var categoryToDelete = new Category { CategoryID = category.CategoryID, DisplayOrder = category.DisplayOrder };
            categoryRepository.Delete(categoryToDelete);

            // Assert
            Assert.Equal(0, mockContext.Categories.Count());
        }

        [Fact]
        public void Delete_NonexistentCategory_ShouldLogInformation()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);
            var nonexistentCategory = new Category { CategoryID = -1, Name = "Nonexistent Category", DisplayOrder = "1" };

            // Act
            categoryRepository.Delete(nonexistentCategory);

            // Assert
            Assert.False(mockContext.ChangeTracker.Entries<Category>().Any(e => e.Entity == nonexistentCategory && e.State == EntityState.Deleted));
        }

        [Fact]
        public void Delete_InvalidCategoryId_ShouldNotRemoveFromDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Act
            var invalidCategoryId = -1;
            var categoryToDelete = new Category { CategoryID = invalidCategoryId, Name = "Invalid Category", DisplayOrder = "1" };
            categoryRepository.Delete(categoryToDelete);

            // Assert
            Assert.Equal(0, mockContext.Categories.Count()); // Ensure nothing was deleted
        }

        [Fact]
        public void DeleteRange_ValidCategories_ShouldRemoveAllFromDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create categories and add them to the context
            var categories = new List<Category>
            {
                new Category { CategoryID = 1, Name = "Category 1", DisplayOrder = "1" },
                new Category { CategoryID = 2, Name = "Category 2", DisplayOrder = "2" },
                new Category { CategoryID = 3, Name = "Category 3", DisplayOrder = "3" }
            };
            mockContext.Categories.AddRange(categories);
            mockContext.SaveChanges();

            // Act
            categoryRepository.DeleteRange(categories);

            // Assert
            Assert.Equal(0, mockContext.Categories.Count()); // Ensure all categories were deleted
        }

        [Fact]
        public void Get_ValidCategoryId_ShouldReturnCategory()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create a category and add it to the context
            var categoryToAdd = new Category { CategoryID = 1, Name = "Example Category", DisplayOrder = "1" };
            mockContext.Categories.Add(categoryToAdd);
            mockContext.SaveChanges();

            // Act
            var retrievedCategory = categoryRepository.Get(x => x.CategoryID == categoryToAdd.CategoryID);

            // Assert
            Assert.NotNull(retrievedCategory);
            Assert.Equal(categoryToAdd.CategoryID, retrievedCategory.CategoryID);
        }

        [Fact]
        public void Get_InvalidCategoryId_ShouldReturnNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Act
            var retrievedCategory = categoryRepository.Get(x => x.CategoryID == -1);

            // Assert
            Assert.Null(retrievedCategory);
        }

        [Fact]
        public void GetAll_NoPredicate_ShouldReturnAllCategories()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create categories and add them to the context
            var categories = new List<Category>
            {
                new Category { CategoryID = 1, Name = "Category 1", DisplayOrder = "1" },
                new Category { CategoryID = 2, Name = "Category 2", DisplayOrder = "2" },
                new Category { CategoryID = 3, Name = "Category 3", DisplayOrder = "3" }
            };
            mockContext.Categories.AddRange(categories);
            mockContext.SaveChanges();

            // Act
            var retrievedCategories = categoryRepository.GetAll();

            // Assert
            Assert.Equal(categories.Count, retrievedCategories.Count());
        }

        [Fact]
        public void GetAll_WithPredicate_ShouldReturnFilteredCategories()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create categories and add them to the context
            var categories = new List<Category>
            {
                new Category { CategoryID = 1, Name = "Category 1", DisplayOrder = "1" },
                new Category { CategoryID = 2, Name = "Category 2", DisplayOrder = "2" },
                new Category { CategoryID = 3, Name = "Category 3", DisplayOrder = "3" }
            };
            mockContext.Categories.AddRange(categories);
            mockContext.SaveChanges();

            // Act
            var retrievedCategories = categoryRepository.GetAll(x => x.DisplayOrder == "2");

            // Assert
            Assert.Single(retrievedCategories); // Only one category with DisplayOrder "2"
            Assert.Equal(2, retrievedCategories.First().CategoryID);
        }

        [Fact]
        public void Update_ExistingCategory_ShouldUpdateDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Create a category and add it to the context
            var originalCategory = new Category { CategoryID = 1, Name = "Original Category", DisplayOrder = "1" };
            mockContext.Categories.Add(originalCategory);
            mockContext.SaveChanges();

            // Act
            var updatedCategory = new Category { CategoryID = originalCategory.CategoryID, Name = "Updated Category", DisplayOrder = "2" };
            categoryRepository.Update(updatedCategory);

            // Assert
            var retrievedCategory = mockContext.Categories.Find(updatedCategory.CategoryID);
            Assert.NotNull(retrievedCategory);
            Assert.Equal(updatedCategory.Name, retrievedCategory.Name);
            Assert.Equal(updatedCategory.DisplayOrder, retrievedCategory.DisplayOrder);
        }

        [Fact]
        public void Update_NonexistentCategory_ShouldNotUpdateDatabase()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CategoryRepository>>();
            var mockContext = CreateMockContext();
            var categoryRepository = new CategoryRepository(mockContext, mockLogger.Object);

            // Act
            var nonexistentCategory = new Category { CategoryID = -1, Name = "Nonexistent Category", DisplayOrder = "1" };
            categoryRepository.Update(nonexistentCategory);

            // Assert
            Assert.Empty(mockContext.ChangeTracker.Entries()); // Ensure no changes were tracked
        }
    }
}
