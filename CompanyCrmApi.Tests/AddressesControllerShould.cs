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
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrmApi.Tests
{
    [TestFixture]
    class AddressesControllerShould
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerProfile()));
            _mapper = new Mapper(mapperConfiguration);
        }

        [Test]
        public async Task GetByAddressId_PassValidAddressId_ReturnsActionResultWithAddressModel()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAddressAsync(1)).ReturnsAsync(MockData.mockAddress);

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Get(1);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<AddressModel>), result);
            Assert.AreEqual(MockData.mockAddress.AddressId, result.Value.AddressId);
            Assert.AreEqual(MockData.mockAddress.AddressType, result.Value.AddressType);
            Assert.AreEqual(MockData.mockAddress.StreetLine1, result.Value.StreetLine1);
            Assert.AreEqual(MockData.mockAddress.StreetLine2, result.Value.StreetLine2);
            Assert.AreEqual(MockData.mockAddress.City, result.Value.City);
            Assert.AreEqual(MockData.mockAddress.State, result.Value.State);
            Assert.AreEqual(MockData.mockAddress.PostalCode, result.Value.PostalCode);
            Assert.AreEqual(MockData.mockAddress.Country, result.Value.Country);
            Assert.AreEqual(MockData.mockAddress.CustomerId, result.Value.CustomerId);
        }

        [Test]
        public async Task GetByAddressId_PassInvalidAddressId_ReturnsNotFound()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAddressAsync(It.IsAny<int>()))
                                  .Returns(Task.FromResult<Address>(null));

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<int>());

            // Assert
            _mockAddressRepository.Verify(x => x.GetAddressAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetByAddressId_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAddressAsync(It.IsAny<int>()))
                                   .ThrowsAsync(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Get(It.IsAny<int>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task GetByCustomerId_PassValidCustomerId_ReturnsListOfAddresses()
        {
            // Arrange
            Address[] mockCustomer2Addresses = MockData.mockAddressList.Where(a => a.CustomerId == 2).ToArray();
            _mockAddressRepository.Setup(x => x.GetAllAddressesForCustomerAsync(2))
                                  .Returns(Task.FromResult(mockCustomer2Addresses));

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetByCustomer(2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<AddressModel[]>), result);
            Assert.AreEqual(mockCustomer2Addresses.Length, result.Value.Length);
            Assert.AreEqual(mockCustomer2Addresses[0].AddressId, result.Value[0].AddressId);
            Assert.AreEqual(mockCustomer2Addresses[1].AddressId, result.Value[1].AddressId);
        }

        [Test]
        public async Task GetByCustomerId_PassInvalidCustomerId_ReturnsNotFound()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllAddressesForCustomerAsync(It.IsAny<int>()))
                                  .Returns(Task.FromResult<Address[]>(null));

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetByCustomer(It.IsAny<int>());

            // Assert
            _mockAddressRepository.Verify(x => x.GetAllAddressesForCustomerAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetByCustomerId_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllAddressesForCustomerAsync(It.IsAny<int>()))
                                   .ThrowsAsync(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetByCustomer(It.IsAny<int>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task GetAllCountries_CallsRepositoryAndReturnsCountryArray()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllCountriesAsync()).Returns(Task.FromResult(It.IsAny<Country[]>()));

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetAllCountries();

            // Assert
            _mockAddressRepository.Verify(x => x.GetAllCountriesAsync(), Times.Once);
            Assert.IsInstanceOf(typeof(ActionResult<Country[]>), result);
        }

        [Test]
        public async Task GetAllCountries_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllCountriesAsync())
                                  .ThrowsAsync(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetAllCountries();
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task GetRegions_PassValidCountryCode_CallsRepositoryAndReturnsRegionArray()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllRegionsForCountryAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<Region[]>()));

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetRegions(It.IsAny<string>());

            // Assert
            _mockAddressRepository.Verify(x => x.GetAllRegionsForCountryAsync(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf(typeof(ActionResult<Region[]>), result);
        }

        [Test]
        public async Task GetRegions_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAllRegionsForCountryAsync(It.IsAny<string>()))
                                   .ThrowsAsync(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.GetRegions(It.IsAny<string>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task Post_ValidAddressModel_CallsRepositoryAndReturnsCreated201Response()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.Add(It.IsAny<Address>()));
            _mockAddressRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Post(MockData.mockAddressModel);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<AddressModel>), result);
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), result.Result);
            _mockAddressRepository.Verify(x => x.Add(It.IsAny<Address>()), Times.Once);
            _mockAddressRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Post_InvalidAddressModel_Returns400BadRequest()
        {
            // Arrange
            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Post(It.IsAny<AddressModel>());

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        }

        [Test]
        public async Task Post_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.Add(It.IsAny<Address>()))
                                  .Throws(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Post(It.IsAny<AddressModel>());
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Database error: System error.", objectResult.Value);
        }

        [Test]
        public async Task Delete_ValidAddressId_Returns200Ok()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAddressAsync(1))
                       .ReturnsAsync(MockData.mockAddressList[0]);
            _mockAddressRepository.Setup(x => x.Delete(It.IsAny<Address>()));
            _mockAddressRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(1);

            // Assert
            _mockAddressRepository.Verify(x => x.GetAddressAsync(1), Times.Once);
            _mockAddressRepository.Verify(x => x.Delete(It.IsAny<Address>()), Times.Once);
            _mockAddressRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Delete_InvalidAddressId_Returns404NotFound()
        {
            // Arrange
            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(It.IsAny<int>());

            // Assert
            _mockAddressRepository.Verify(x => x.GetAddressAsync(It.IsAny<int>()), Times.Once);
            _mockAddressRepository.Verify(x => x.Delete(It.IsAny<Address>()), Times.Never);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Delete_ThrowSystemException_Returns500InternalServerError()
        {
            // Arrange
            _mockAddressRepository.Setup(x => x.GetAddressAsync(It.IsAny<int>()))
                                   .ThrowsAsync(new SystemException());

            var sut = new AddressesController(_mockAddressRepository.Object, _mapper);

            // Act
            var result = await sut.Delete(It.IsAny<int>()) as ObjectResult;

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Database error: System error.", result.Value);
        }
    }
}
