using CompanyCrm.Controllers;
using CustomerModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
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
    public class AddressesControllerShould
    {
        [Fact]
        public async Task List_PassValidCustomerId_ReturnsViewResultOfAssociatedAddressModels()
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
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonConvert.SerializeObject(MockAddressData.mockAddressList)),
                       })
                       .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new AddressesController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.List(1);

            // Assert
            var expectedUri = new Uri("https://localhost:44360/api/addresses/GetByCustomer/1");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(), 
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
           );

            Assert.IsType<ActionResult<AddressModel[]>>(result);
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.NotNull(result.Result);
            var returnedAddressList = viewResult.Model as AddressModel[];
            Assert.Equal(MockAddressData.mockAddressList.Length, returnedAddressList.Length);
        }

        [Fact]
        public async Task List_PassInvalidCustomerId_Returns/*ViewResultOfAssociatedAddressModels*/()
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
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonConvert.SerializeObject(MockAddressData.mockAddressList)),
                       })
                       .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new AddressesController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.List(1);

            // Assert
            var expectedUri = new Uri("https://localhost:44360/api/addresses/GetByCustomer/1");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
           );

            Assert.IsType<ActionResult<AddressModel[]>>(result);
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.NotNull(result.Result);
            var returnedAddressList = viewResult.Model as AddressModel[];
            Assert.Equal(MockAddressData.mockAddressList.Length, returnedAddressList.Length);
        }
    }
}
