using CompanyCrm.Controllers;
using CompanyCrm.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CompanyCrm.Tests
{
    public class BattleshipControllerShould
    {
        [Fact]
        public async Task Setup_PassViewModelWithInboundCoordinates_ReturnsGame()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new BattleshipController(mockHttpClientFactory.Object);

            var model = MockBattleshipData.mockBattleshipViewModel;


            //Act
            var result = await sut.Setup(model);

            //Assert
            var expectedUri = new Uri($"https://localhost:44360/api/battleship");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            Assert.IsType<CreatedResult>(result);
        }
    }
}
