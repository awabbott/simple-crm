using CompanyCrmApi.Data.Entities;
using CompanyCrmApi.Data.Repositories;
using CustomerModels;
using CustomerModels.Battleship;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCrmApi.Tests
{
    public static class MockData
    {
        public static readonly Customer[] mockCustomerList = new Customer[]
        {
            new Customer()
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Smith",
                Age = 54,
                Gender = Gender.Male,
                Education = Education.Diploma,
                Interests = "hiking, backpacking, traveling",
                Phone = "667899585",
                Email = "john.smith2331@gmail.com",
                DateSubmitted = new DateTime(2021, 08, 15),
                Inactive = false,
                UserId = "2",
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressId = 1,
                        AddressType = AddressType.Physical,
                        StreetLine1 = "123 Main St",
                        StreetLine2 = "Unit 3",
                        City = "Riverside",
                        State = "California",
                        PostalCode = "91234",
                        Country = "United States",
                        CustomerId = 1
                    },
                    new Address()
                    {
                        AddressId = 2,
                        AddressType = AddressType.Mailing,
                        StreetLine1 = "PO Box 123",
                        StreetLine2 = null,
                        City = "Riverside",
                        State = "California",
                        PostalCode = "91235",
                        Country = "United States",
                        CustomerId = 1
                    }
                }
            },
            new Customer()
            {
                CustomerId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Age = 51,
                Gender = Gender.Female,
                Education = Education.Masters,
                Interests = "reading, yoga, traveling",
                Phone = "667456789",
                Email = "jane.smith23@hotmail.com",
                DateSubmitted = new DateTime(2021, 08, 15),
                Inactive = false,
                UserId = "2",
                Addresses = new List<Address>()
            }
        };

        public static readonly CustomerModel mockCustomerModel = new CustomerModel()
        {
            FirstName = "New",
            LastName = "Customer",
            Age = 40,
            Gender = Gender.Unspecified,
            Education = Education.Doctorate,
            Interests = "studying",
            Phone = "667456789",
            Email = "new.customer@aol.com",
            DateSubmitted = new DateTime(2021, 08, 15),
            Inactive = false,
            UserId = "1",
            Addresses = new List<AddressModel>()
        };

        public static readonly CustomerModel updatedCustomerModel = new CustomerModel()
        {
            CustomerId = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Age = mockCustomerList[1].Age++,
            Gender = Gender.Female,
            Education = Education.Masters,
            Interests = "reading, yoga, traveling",
            Phone = "667456789",
            Email = "jane.smith23@hotmail.com",
            DateSubmitted = new DateTime(2021, 08, 15),
            Inactive = false,
            UserId = "2",
            Addresses = new List<AddressModel>()
        };

        public static readonly Address[] mockAddressList = new Address[]
        {
            new Address()
            {
                AddressId = 1,
                AddressType = AddressType.Physical,
                StreetLine1 = "123 Main St",
                StreetLine2 = "Unit 3",
                City = "Riverside",
                State = "California",
                PostalCode = "91234",
                Country = "United States",
                CustomerId = 1
            },
            new Address()
            {
                AddressId = 2,
                AddressType = AddressType.Mailing,
                StreetLine1 = "PO Box 123",
                StreetLine2 = null,
                City = "Riverside",
                State = "California",
                PostalCode = "91235",
                Country = "United States",
                CustomerId = 1
            },
            new Address()
            {
                AddressId = 3,
                AddressType = AddressType.Physical,
                StreetLine1 = "123 Main St",
                StreetLine2 = null,
                City = "Helena",
                State = "Montana",
                PostalCode = "68484",
                Country = "United States",
                CustomerId = 2
            },
            new Address()
            {
                AddressId = 4,
                AddressType = AddressType.Billing,
                StreetLine1 = "2242 W Main St",
                StreetLine2 = null,
                City = "Billings",
                State = "Montana",
                PostalCode = "68554",
                Country = "United States",
                CustomerId = 2
            }
        };

        public static readonly Address mockAddress = new Address()
        {
            AddressId = 1,
            AddressType = AddressType.Physical,
            StreetLine1 = "123 Main St",
            StreetLine2 = "Unit 3",
            City = "Riverside",
            State = "California",
            PostalCode = "91234",
            Country = "United States",
            CustomerId = 1
        };

        public static readonly AddressModel mockAddressModel = new AddressModel()
        {
            AddressId = 1,
            AddressType = AddressType.Physical,
            StreetLine1 = "123 Main St",
            StreetLine2 = "Unit 3",
            City = "Riverside",
            State = "California",
            PostalCode = "91234",
            Country = "United States",
            CustomerId = 1
        };

        public static readonly Game mockGame = new Game
        {
            GameId = 10,
            Ships = new List<Ship>()
        };

        public static readonly Ship mockShip = new Ship(ShipName.Destroyer)
        {
            ShipId = 1,
            HitCount = 0,
            IsSunk = false,
            BelongsToUser = true,
            GameId = 5,
            Coordinates = new List<Coordinate>()
        };

        public static readonly Ship mockShip2 = new Ship(ShipName.Destroyer)
        {
            ShipId = 1,
            HitCount = 0,
            IsSunk = false,
            BelongsToUser = true,
            GameId = 5,
            Coordinates = new List<Coordinate>()
        };

        public static readonly Coordinate mockCoordinate = new Coordinate
        {
            CoordinateId = 2,
            Row = 1,
            Column = 4,
            ShipId = 1
        };
    }
}
