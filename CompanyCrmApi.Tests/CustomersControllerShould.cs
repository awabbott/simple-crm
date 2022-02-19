using AutoMapper;
using CompanyCrmApi.Controllers;
using CompanyCrmApi.Data;
using CompanyCrmApi.Data.Entities;
using CompanyCrmApi.Data.Repositories;
using CustomerModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CompanyCrmApi.Tests
{
    [TestFixture]
    public class CustomersControllerShould
    {
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerProfile()));
            _mapper = new Mapper(mapperConfiguration);
        }

        [Test]
        public async Task Get_RequestCustomers_ReturnsAllCustomersAsCustomerModels()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetAllCustomersAsync())
                                  .ReturnsAsync(MockData.mockCustomerList);

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get();

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<CustomerModel[]>), result);
            Assert.AreEqual(MockData.mockCustomerList.Length, result.Value.Length);
            Assert.AreEqual(MockData.mockCustomerList[0].CustomerId, result.Value[0].CustomerId);
            Assert.AreEqual(MockData.mockCustomerList[1].CustomerId, result.Value[1].CustomerId);
        }

        [Test]
        public async Task Get_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetAllCustomersAsync())
                                   .ThrowsAsync(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get();
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task GetByCustomerId_PassValidCustomerId_ReturnsActionResultWithCustomerModel()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(1, true))
                                   .ReturnsAsync(MockData.mockCustomerList[0]);

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get(1);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<CustomerModel>), result);
            Assert.AreEqual(MockData.mockCustomerList[0].CustomerId, result.Value.CustomerId);
            Assert.AreEqual(MockData.mockCustomerList[0].FirstName, result.Value.FirstName);
            Assert.AreEqual(MockData.mockCustomerList[0].LastName, result.Value.LastName);
            Assert.AreEqual(MockData.mockCustomerList[0].Age, result.Value.Age);
            Assert.AreEqual(MockData.mockCustomerList[0].Gender, result.Value.Gender);
            Assert.AreEqual(MockData.mockCustomerList[0].Education, result.Value.Education);
            Assert.AreEqual(MockData.mockCustomerList[0].Interests, result.Value.Interests);
            Assert.AreEqual(MockData.mockCustomerList[0].Phone, result.Value.Phone);
            Assert.AreEqual(MockData.mockCustomerList[0].Email, result.Value.Email);
            Assert.AreEqual(MockData.mockCustomerList[0].DateSubmitted, result.Value.DateSubmitted);
            Assert.AreEqual(MockData.mockCustomerList[0].Inactive, result.Value.Inactive);
            Assert.AreEqual(MockData.mockCustomerList[0].UserId, result.Value.UserId);
            Assert.AreEqual(MockData.mockCustomerList[0].Addresses.Count, result.Value.Addresses.Count);
        }

        [Test]
        public async Task GetByCustomerId_PassInvalidCustomerId_ReturnsNotFound()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(It.IsAny<int>(), true))
                                  .Returns(Task.FromResult<Customer>(null));

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<int>());

            // Assert
            _mockCustomerRepository.Verify(x => x.GetCustomerAsync(It.IsAny<int>(), true), Times.Once);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetByCustomerId_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(It.IsAny<int>(), true))
                                   .ThrowsAsync(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<int>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task GetByUserId_PassValidUserId_ReturnsListOfCustomers()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetAllCustomersForUserAsync("2"))
                                  .Returns(Task.FromResult(MockData.mockCustomerList));

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get("2");

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<CustomerModel[]>), result);
            Assert.AreEqual(MockData.mockCustomerList.Length, result.Value.Length);
            Assert.AreEqual(MockData.mockCustomerList[0].CustomerId, result.Value[0].CustomerId);
            Assert.AreEqual(MockData.mockCustomerList[1].CustomerId, result.Value[1].CustomerId);
        }

        [Test]
        public async Task GetByUserId_PassInvalidUserId_ReturnsNotFound()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetAllCustomersForUserAsync(It.IsAny<string>()))
                                  .Returns(Task.FromResult<Customer[]>(null));

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<string>());

            // Assert
            _mockCustomerRepository.Verify(x => x.GetAllCustomersForUserAsync(It.IsAny<string>()), Times.Once);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetByUserId_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetAllCustomersForUserAsync(It.IsAny<string>()))
                                   .ThrowsAsync(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<string>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task Post_ValidCustomerModel_CallsRepositoryAndReturnsCreated201Response()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.Add(It.IsAny<Customer>()));
            _mockCustomerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Post(MockData.mockCustomerModel);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<CustomerModel>), result);
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), result.Result);
            _mockCustomerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);
            _mockCustomerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Post_InvalidCustomerModel_Returns400BadRequest()
        {
            // Arrange
            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Post(MockData.mockCustomerModel);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        }

        [Test]
        public async Task Post_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.Add(It.IsAny<Customer>()));
            _mockCustomerRepository.Setup(x => x.Add(It.IsAny<Customer>()))
                                   .Throws(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Post(It.IsAny<CustomerModel>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task Put_ValidCustomerIdAndCustomerModel_ReturnsUpdatedCustomerModel()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(2, false)).ReturnsAsync(MockData.mockCustomerList[1]);
            _mockCustomerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Put(2, MockData.updatedCustomerModel);

            // Assert
            _mockCustomerRepository.Verify(x => x.GetCustomerAsync(2, false), Times.Once);
            _mockCustomerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOf(typeof(ActionResult<CustomerModel>), result);
            Assert.AreEqual(MockData.updatedCustomerModel.CustomerId, result.Value.CustomerId);
            Assert.AreEqual(MockData.updatedCustomerModel.FirstName, result.Value.FirstName);
            Assert.AreEqual(MockData.updatedCustomerModel.LastName, result.Value.LastName);
            Assert.AreEqual(MockData.updatedCustomerModel.Age, result.Value.Age);
            Assert.AreEqual(MockData.updatedCustomerModel.Gender, result.Value.Gender);
            Assert.AreEqual(MockData.updatedCustomerModel.Education, result.Value.Education);
            Assert.AreEqual(MockData.updatedCustomerModel.Interests, result.Value.Interests);
            Assert.AreEqual(MockData.updatedCustomerModel.Phone, result.Value.Phone);
            Assert.AreEqual(MockData.updatedCustomerModel.Email, result.Value.Email);
            Assert.AreEqual(MockData.updatedCustomerModel.Inactive, result.Value.Inactive);
            Assert.AreEqual(MockData.updatedCustomerModel.UserId, result.Value.UserId);
            Assert.AreEqual(MockData.updatedCustomerModel.Addresses.Count, result.Value.Addresses.Count);
        }

        [Test]
        public async Task Put_InvalidCustomerId_Returns404NotFound()
        {
            // Arrange
            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Put(It.IsAny<int>(), It.IsAny<CustomerModel>());

            // Assert
            _mockCustomerRepository.Verify(x => x.GetCustomerAsync(It.IsAny<int>(), false), Times.Once);
            _mockCustomerRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task Put_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(It.IsAny<int>(), false))
                                   .ThrowsAsync(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Put(It.IsAny<int>(), It.IsAny<CustomerModel>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task Delete_ValidCustomerId_Returns200Ok()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(1, false))
                       .ReturnsAsync(MockData.mockCustomerList[0]);
            _mockCustomerRepository.Setup(x => x.Delete(It.IsAny<Customer>()));
            _mockCustomerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(1);

            // Assert
            _mockCustomerRepository.Verify(x => x.GetCustomerAsync(1, false), Times.Once);
            _mockCustomerRepository.Verify(x => x.Delete(It.IsAny<Customer>()), Times.Once);
            _mockCustomerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Delete_InvalidCustomerId_Returns404NotFound()
        {
            // Arrange
            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(It.IsAny<int>());

            // Assert
            _mockCustomerRepository.Verify(x => x.GetCustomerAsync(It.IsAny<int>(), false), Times.Once);
            _mockCustomerRepository.Verify(x => x.Delete(It.IsAny<Customer>()), Times.Never);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Delete_ThrowInternalError_Returns500InternalServerError()
        {
            // Arrange
            _mockCustomerRepository.Setup(x => x.GetCustomerAsync(It.IsAny<int>(), false))
                                   .ThrowsAsync(new SystemException());

            var sut = new CustomersController(_mockCustomerRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(It.IsAny<int>()) as ObjectResult;

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Database error: System error.", result.Value);
        }
    }
}
