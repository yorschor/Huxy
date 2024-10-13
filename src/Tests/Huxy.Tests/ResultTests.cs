using System;
using Xunit;

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
            Assert.Empty(result.Message);
            Assert.Null(result.Exception);
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
            Assert.Empty(result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void NopeResult_ShouldBeFailure()
        {
            // Arrange
            var message = "An error occurred";

            // Act
            var result = Result.Fail(message);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void NopeResult_WithException_ShouldContainException()
        {
            // Arrange
            var message = "An error occurred";
            var exception = new Exception("Test exception");

            // Act
            var result = Result.Fail(message, exception);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Equal(exception, result.Exception);
        }

        [Fact]
        public void NopeResult_WithExceptionOnly_ShouldSetMessageFromException()
        {
            // Arrange
            var exception = new Exception("Exception message");

            // Act
            var result = Result.Fail(exception);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(exception.Message, result.Message);
            Assert.Equal(exception, result.Exception);
        }

        [Fact]
        public void TypedNopeResult_ShouldBeFailure()
        {
            // Arrange
            var message = "An error occurred";

            // Act
            var result = Result.Fail<int>(message);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var data = result.Data;
            });
        }

        [Fact]
        public void TypedOkResult_ShouldBeSuccessfulAndContainData()
        {
            // Arrange
            var data = 42;

            // Act
            var result = Result.Ok(data);

            // Assert
            Assert.True(result.Success);
            Assert.False(result.Failure);
            Assert.Equal(data, result.Data);
            Assert.Empty(result.Message);
            Assert.Null(result.Exception);
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
        public void NopeResultWithImplicitConversion_ShouldReturnFalse()
        {
            // Arrange
            var result = Result.Fail("An error occurred");

            // Act & Assert
            Assert.False(result);
        }

        [Fact]
        public void AccessingDataOnNopeResult_ShouldThrowException()
        {
            // Arrange
            var result = Result.Fail<string>("An error occurred");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var data = result.Data;
            });
        }

        [Fact]
        public void AccessingDataOnSuccessResult_ShouldReturnData()
        {
            // Arrange
            var data = "Test Data";
            var result = Result.Ok(data);

            // Act
            var retrievedData = result.Data;

            // Assert
            Assert.Equal(data, retrievedData);
        }

        [Fact]
        public void OkResult_ShouldHaveNullException()
        {
            // Arrange
            var result = Result.Ok();

            // Act & Assert
            Assert.Null(result.Exception);
        }

        [Fact]
        public void NopeResult_WithNullException_ShouldHaveMessageOnly()
        {
            // Arrange
            var message = "An error occurred";

            // Act
            var result = Result.Fail(message, null);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.Failure);
            Assert.Equal(message, result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void Result_GenericConstructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var data = 123;
            var message = "Custom message";
            var exception = new Exception("Test exception");

            // Act
            var result = new Result<int>(data, message, exception, true);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(data, result.Data);
            Assert.Equal(message, result.Message);
            Assert.Equal(exception, result.Exception);
        }

        [Fact]
        public void NopeResult_FromAnotherResult_ShouldCopyMessageAndException()
        {
            // Arrange
            var originalException = new Exception("Original exception");
            var originalResult = Result.Fail("Original error message", originalException);

            // Act
            var newResult = Result.Fail(originalResult);

            // Assert
            Assert.False(newResult.Success);
            Assert.True(newResult.Failure);
            Assert.Equal(originalResult.Message, newResult.Message);
            Assert.Equal(originalResult.Exception, newResult.Exception);
        }

        [Fact]
        public void TypedNopeResult_FromAnotherTypedResult_ShouldCopyMessageAndException()
        {
            // Arrange
            var originalException = new Exception("Original exception");
            var originalResult = Result.Fail<string>("Original error message", originalException);

            // Act
            var newResult = Result.Fail<bool>(originalResult);

            // Assert
            Assert.False(newResult.Success);
            Assert.True(newResult.Failure);
            Assert.Equal(originalResult.Message, newResult.Message);
            Assert.Equal(originalResult.Exception, newResult.Exception);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var data = newResult.Data;
            });
        }
        
    }
}