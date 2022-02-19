using CompanyCrm.Controllers;
using CustomerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CompanyCrm.Tests
{
    public class CustomersControllerShould
    {      
        [Fact]
        public async Task List_ReturnsViewResultOfCustomers()
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
                           Content = new StringContent("SomeContent"),
                       })
                       .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);


            //// Mock User implementation 1
            //var claim = new Claim("test", "mockUserId");
            //var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            //var mockContext = Mock.Of<ControllerContext>(cc => cc.HttpContext.User.Identity == mockIdentity);

            //// Mock User implementation 2
            //var context = new Mock<HttpContext>();
            //var mockIdentity = new Mock<IIdentity>();
            //context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            //mockIdentity.Setup(x => x.Name).Returns("test_name");

            //// Mock User implementation 3
            //var username = "FakeUserName";
            //var identity = new GenericIdentity(username, "");
            //var mockPrincipal = new Mock<ClaimsPrincipal>();
            //mockPrincipal.Setup(x => x.Identity).Returns(identity);
            //mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            //var mockHttpContext = new Mock<HttpContext>();
            //mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

            //Mock User implementation 4
            string mockUserId = "MockUserName";
            var identity = new GenericIdentity(mockUserId, "");
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

            string userId = "anyGivenUserId";

            var sut = new CustomersController(mockHttpClientFactory.Object);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
                //HttpContext = new DefaultHttpContext
                //{
                //    User = mockUser.Object
                //}
            };

            // Act
            var result = await sut.List();

            // Assert
            // List returns ViewResult(per MS Docs) or maybe ActionResult
            // List returns Customer[]
            // Customer counts are the same
            // Must create mock data to retrieve
            var expectedUri = new Uri($"https://localhost:44360/api/customers/{userId}");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == expectedUri),
                It.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task Details_PassValidCustomerId_ReturnsViewResultOfCustomerModel()
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
                           Content = new StringContent(JsonConvert.SerializeObject(MockCustomerData.mockCustomerModel))
                       })
                       .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Details(2);

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            Assert.IsType<ActionResult<CustomerModel>>(result);
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.NotNull(result.Result);
            var returnedCustomerModel = viewResult.Model as CustomerModel;
            Assert.Equal(MockCustomerData.mockCustomerModel.CustomerId, returnedCustomerModel.CustomerId);
            Assert.Equal(MockCustomerData.mockCustomerModel.FirstName, returnedCustomerModel.FirstName);
            Assert.Equal(MockCustomerData.mockCustomerModel.LastName, returnedCustomerModel.LastName);
            Assert.Equal(MockCustomerData.mockCustomerModel.Age, returnedCustomerModel.Age);
            Assert.Equal(MockCustomerData.mockCustomerModel.Gender, returnedCustomerModel.Gender);
            Assert.Equal(MockCustomerData.mockCustomerModel.Education, returnedCustomerModel.Education);
            Assert.Equal(MockCustomerData.mockCustomerModel.Interests, returnedCustomerModel.Interests);
            Assert.Equal(MockCustomerData.mockCustomerModel.Phone, returnedCustomerModel.Phone);
            Assert.Equal(MockCustomerData.mockCustomerModel.Email, returnedCustomerModel.Email);
            Assert.Equal(MockCustomerData.mockCustomerModel.DateSubmitted, returnedCustomerModel.DateSubmitted);
            Assert.Equal(MockCustomerData.mockCustomerModel.Inactive, returnedCustomerModel.Inactive);
            Assert.Equal(MockCustomerData.mockCustomerModel.UserId, returnedCustomerModel.UserId);
            Assert.Equal(MockCustomerData.mockCustomerModel.Addresses.Count, returnedCustomerModel.Addresses.Count);
        }

        [Fact]
        public async Task Details_PassInvalidCustomerId_Returns404NotFound()
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
                           StatusCode = HttpStatusCode.NotFound
                       });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Details(2);

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidCustomerModel_Returns201Created()
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

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Create(It.IsAny<CustomerModel>());

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Update_ValidCustomerModel_ReturnsDetailsViewOfCustomer()
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
                           StatusCode = HttpStatusCode.Created,
                           Content = new StringContent(JsonConvert.SerializeObject(MockCustomerData.mockCustomerModel))
                       });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Update(MockCustomerData.mockCustomerModel) as ViewResult;

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            var returnedCustomerModel = Assert.IsType<CustomerModel>(result.Model);
            Assert.Equal("Details", result.ViewName);
            Assert.Equal(MockCustomerData.mockCustomerModel.CustomerId, returnedCustomerModel.CustomerId);
            Assert.Equal(MockCustomerData.mockCustomerModel.FirstName, returnedCustomerModel.FirstName);
            Assert.Equal(MockCustomerData.mockCustomerModel.LastName, returnedCustomerModel.LastName);
            Assert.Equal(MockCustomerData.mockCustomerModel.Age, returnedCustomerModel.Age);
            Assert.Equal(MockCustomerData.mockCustomerModel.Gender, returnedCustomerModel.Gender);
            Assert.Equal(MockCustomerData.mockCustomerModel.Education, returnedCustomerModel.Education);
            Assert.Equal(MockCustomerData.mockCustomerModel.Interests, returnedCustomerModel.Interests);
            Assert.Equal(MockCustomerData.mockCustomerModel.Phone, returnedCustomerModel.Phone);
            Assert.Equal(MockCustomerData.mockCustomerModel.Email, returnedCustomerModel.Email);
            Assert.Equal(MockCustomerData.mockCustomerModel.DateSubmitted, returnedCustomerModel.DateSubmitted);
            Assert.Equal(MockCustomerData.mockCustomerModel.Inactive, returnedCustomerModel.Inactive);
            Assert.Equal(MockCustomerData.mockCustomerModel.UserId, returnedCustomerModel.UserId);
            Assert.Equal(MockCustomerData.mockCustomerModel.Addresses.Count, returnedCustomerModel.Addresses.Count);
        }

        [Fact]
        public async Task Update_InvalidCustomerModel_ReturnsUpdateView()
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
                           StatusCode = HttpStatusCode.BadRequest
                       });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Update(MockCustomerData.mockCustomerModel) as ViewResult;

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );
            Assert.Null(result.Model);
            Assert.Equal("Update", result.ViewName);
        }

        [Fact]
        public async Task Delete_ValidCustomerId_ReturnsDetailsViewOfCustomer()
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
                           StatusCode = HttpStatusCode.OK
                       });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Delete(2, null);

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidCustomerId_ReturnsDeleteView()
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
                           StatusCode = HttpStatusCode.NotFound
                       });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/api/")
            };

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CustomersController(mockHttpClientFactory.Object);

            // Act
            var result = await sut.Delete(2, null) as ViewResult;

            // Assert
            var expectedUri = new Uri($"https://localhost:44360/api/customers/2");
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Null(result.Model);
            Assert.Equal("Delete", result.ViewName);
        }
    }
}
