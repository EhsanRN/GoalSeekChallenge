using FluentAssertions;
using GoalSeek.API.Models;
using GoalSeek.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GoalSeek.API.Controllers.Tests
{
    [TestClass()]
    public class GoalSeekControllerTests
    {
        private Mock<GoalSeekController> _controllerMock = null;
        private Mock<IGoalSeekRepository> _goalSeekRepositoryMock = null;
        private Mock<ILogger<GoalSeekController>> _loggerMock = null;
        public GoalSeekControllerTests()
        {
            _goalSeekRepositoryMock = new Mock<IGoalSeekRepository>();
        }

        [TestMethod()]
        public void PostTest_Return_ShouldBe_OK()
        {
            _loggerMock = new Mock<ILogger<GoalSeekController>>();
            var goalSeekRequestMock = new Mock<GoalSeekRequest>();

            _goalSeekRepositoryMock.Setup(repo => repo.ProcessCalculateAsync(goalSeekRequestMock.Object))
                                  .Returns(Task.FromResult(new GoalSeekResponse { Iterations = 10, TargetInput = 1000 }));

            _controllerMock = new Mock<GoalSeekController>();
            var sut = new GoalSeekController(_loggerMock.Object, _goalSeekRepositoryMock.Object);

            //act
            var response = sut.Post(goalSeekRequestMock.Object);

            //assert
            response.Should().NotBeNull();
            response.Result.As<ObjectResult>().Should().BeOfType<OkObjectResult>();
        }

        [TestMethod()]
        public void PostTest_Return_ShouldBe_BadRequest()
        {
            //arrange
            _loggerMock=new Mock<ILogger<GoalSeekController>>();
            var goalSeekRequestMock = new Mock<GoalSeekRequest>();
            _goalSeekRepositoryMock.Setup(repo => repo.ProcessCalculateAsync(goalSeekRequestMock.Object))
                                  .Returns(Task.FromResult(new GoalSeekResponse()));
            _controllerMock = new Mock<GoalSeekController>();
            var sut = new GoalSeekController(_loggerMock.Object, _goalSeekRepositoryMock.Object);
            sut.ModelState.AddModelError("Formula", "Formula is invalid");

            //act
            var response = sut.Post(goalSeekRequestMock.Object);

            //assert
            response.Should().NotBeNull();
            response.Result.As<ObjectResult>().Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod()]
        public void PostTest_Return_ShouldBe_500()
        {
            //arrange
            _loggerMock = new Mock<ILogger<GoalSeekController>>();
            var goalSeekRequestMock = new Mock<GoalSeekRequest>();
            _goalSeekRepositoryMock.Setup(repo => repo.ProcessCalculateAsync(goalSeekRequestMock.Object)).Throws(new Exception());
            _controllerMock = new Mock<GoalSeekController>();

            var sut = new GoalSeekController(_loggerMock.Object, _goalSeekRepositoryMock.Object);
            
            //act
            var response = sut.Post(goalSeekRequestMock.Object);

            //assert
            response.Should().NotBeNull();
            response.Result.As<ObjectResult>().Value.Should().Be("");
            response.Result.As<ObjectResult>().StatusCode.Should().Be(500);
        }
    }
}