using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace GoalSeek.APITests.Controllers
{
    [TestClass()]
    public class GoalSeekIntegrationTests
    {
        private readonly IHttpClientFactory _factory;
        
        public GoalSeekIntegrationTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient();
            _factory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

        }
        [TestMethod()]
        public void PostTest_With_Valid_Input_Return_OK()
        {
            // Arrange
            var client = _factory.CreateClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7035/api/goalseek/");
            requestMessage.Content = JsonContent.Create(new
                                    {
                                        formula="2.5 * input",
                                        input= 100,
                                        targetResult= 2500,
                                        maximumIterations= 10
                                    });
            // Act
            var response = client.SendAsync(requestMessage).Result;

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [DataTestMethod]
        [DataRow("2.5*324343443243243243234 * Input", "56646465454653.644646", "435435435.646788645685", 1, DisplayName = "badrequest 1")]
        [DataRow("2.5dsdfs *input", "56646465454653.644646", "654646446", 10, DisplayName = "badrequest 2")]
        [DataRow("adfdfda", "56646465454653.644646", "654646446", 10, DisplayName = "badrequest 3")]
        [DataRow("4546546544664644685446464542.565446554 * 6445466544654.54646", "56646465454653.644646", "654646446", 10, DisplayName = "badrequest 4")]
        public void PostTest_With_Invalid_Input_Return_Badrequest(string formula, string input, string targetResult, int maxIterations)
        {
            // Arrange
            var client = _factory.CreateClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7035/api/goalseek/");
            requestMessage.Content = JsonContent.Create(new
            {
                formula = formula,
                input = decimal.Parse(input),
                targetResult = decimal.Parse(targetResult),
                maximumIterations = maxIterations
            });
            // Act
            var response = client.SendAsync(requestMessage).Result;

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
