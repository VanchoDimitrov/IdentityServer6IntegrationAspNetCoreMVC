using Integration.Models.Categories;
using System.ComponentModel.DataAnnotations;

public class CategoryTests
{
    [Theory]
    [InlineData(1, "TestName", "1", "2023-11-17")]
    [InlineData(2, null, "2", "2023-11-17")]
    //[InlineData(3, "TestName", null, "2023-11-17")]
    [InlineData(4, "TestName", "3", null)]
    public void Category_Validations(int categoryId, string name, string displayOrder, string createdDate)
    {
        // Arrange
        var category = new Category
        {
            CategoryID = categoryId,
            Name = name,
            DisplayOrder = displayOrder
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        Assert.Equal(string.IsNullOrEmpty(name), validationResults.Contains("The Name field is required."));
        Assert.Equal(string.IsNullOrEmpty(displayOrder), validationResults.Contains("The Display Order field is required."));
        Assert.True((DateTime.Now - category.CreatedDateTie).Duration() < TimeSpan.FromSeconds(1));
    }

    private string ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);

        return string.Join(", ", validationResults.Select(r => r.ErrorMessage));
    }
}
