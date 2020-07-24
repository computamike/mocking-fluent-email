using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using MockFluentEmail.Controllers;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace NUnitTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestThatWebControllerCallsFluentEmail()
        {
            //Arrange
            Mock<IFluentEmail> mockFluentEmail = new Mock<IFluentEmail>();
            Mock<ILogger<WeatherForecastController>> mocklogger = new Mock<ILogger<WeatherForecastController>>();

            // Set up the Fluent interface for mocking.
            // This is not 100% complete... just enough mocking to make it work.
            mockFluentEmail.Setup(x => x.To(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(x => x.Subject(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(x => x.Body(It.IsAny<string>(),It.IsAny<bool>())).Returns(mockFluentEmail.Object);
            
            // Act
            WeatherForecastController SUT = new WeatherForecastController(mocklogger.Object, mockFluentEmail.Object);
            var res = SUT.Get();
            
            //Assert
            Assert.IsTrue(res.ToList().Count > 0 , "Weather Service should return more than 0 results.");
            Assert.IsTrue(res.ToList().Count <= 5,"Weather Service should return 5 or fewer results.");

            // Assert that the email was sent - with the expected content.
            mockFluentEmail.Verify(foo => foo.To("testEmail@example.com"), Times.Once(), "Email should be sent to : testEmail@example.com");
            mockFluentEmail.Verify(foo => foo.Subject(It.IsAny<string>()), Times.Once(), "Subject Line should be set once");
            mockFluentEmail.Verify(foo => foo.Subject("This Is a Test Email"), Times.Once(), "Subject Line should be This Is a Test Email");
            mockFluentEmail.Verify(foo => foo.Body(It.IsAny<string>(),It.IsAny<bool>()), Times.Once(), "Subject Line should be set once");
            mockFluentEmail.Verify(foo => foo.Body("This is a sample email generated from the controller",false), Times.Once(), "Subject Line should be This Is a Test Email");
            mockFluentEmail.Verify(foo => foo.Send(null), Times.Once(),"Only 1 email should be sent");
         }
    }
}