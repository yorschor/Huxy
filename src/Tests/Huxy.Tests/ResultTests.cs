namespace Huxy.Tests
{
    public class ResultTests
    {
        [Fact]
        public void OkResult_ShouldBeSuccessful()
        {
            // Arrange & Act
            var result = Result.Ok();

            // Assert
            Assert.True(result.Success);
            Assert.False(result.Failure);
        }

        [Fact]
        public void OkResultWithData_ShouldBeSuccessfulAndContainData()
        {
            // Arrange
            const string data = "Test Data";

            // Act
            var result = Result.Ok(data);

            // Assert
            Assert.True(result.Success);
            Assert.False(result.Failure);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void ErrorResult_ShouldBeFailure()
        {
            // Arrange
            var message = "An error occurred";

            // Act
            var result = Result.Error(message);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ErrorResultWithErrors_ShouldBeFailureAndContainErrors()
        {
            // Arrange
            const string message = "An error occurred";
            var errors = new List<Error>
            {
                new("ERR001", "Error 1 details"),
                new("ERR002", "Error 2 details")
            };

            // Act
            var result = Result.Error(message, errors);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void ErrorResultFromAnotherResult_ShouldBeFailureAndContainErrors()
        {
            // Arrange
            const string message = "An error occurred";
            var errors = new List<Error>
            {
                new("ERR001", "Error 1 details"),
                new("ERR002", "Error 2 details")
            };
            var originalResult = Result.Error(message, errors);

            // Act
            var result = Result.Error(originalResult);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void OkResultWithImplicitConversion_ShouldReturnTrue()
        {
            // Arrange
            var result = Result.Ok();

            // Act & Assert
            Assert.True(result);
        }

        [Fact]
        public void ErrorResultWithImplicitConversion_ShouldReturnFalse()
        {
            // Arrange
            var result = Result.Error("An error occurred");

            // Act & Assert
            Assert.False(result);
        }

        [Fact]
        public void AccessingDataOnErrorResult_ShouldThrowException()
        {
            // Arrange
            var result = Result.Error<string>("An error occurred");

            // Act & Assert
            Assert.Throws<Exception>(() => { _ = result.Data; });
        }
    }

    public class ErrorTests
    {
        [Fact]
        public void Error_ShouldInitializeCorrectly()
        {
            // Arrange
            const string code = "ERR001";
            const string details = "Error details";

            // Act
            var error = new Error(code, details);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(details, error.Details);
        }

        [Fact]
        public void ErrorWithoutCode_ShouldInitializeCorrectly()
        {
            // Arrange
            const string details = "Error details";

            // Act
            var error = new Error(details);

            // Assert
            Assert.Null(error.Code);
            Assert.Equal(details, error.Details);
        }
    }
}