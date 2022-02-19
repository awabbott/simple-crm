using CustomerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyCrm.Tests
{
    public class MockCustomerData
    {
        public static readonly CustomerModel[] mockCustomerList = new CustomerModel[]
{
            new CustomerModel()
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
                Addresses = new List<AddressModel>()
                {
                    new AddressModel()
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
                    new AddressModel()
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
            new CustomerModel()
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
                Addresses = new List<AddressModel>()
            }
        };

        public static CustomerModel mockCustomerModel = new CustomerModel()
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
            Addresses = new List<AddressModel>()
        };

        //public static string mockJsonCustomerModel = [
        //    {
        //        'CustomerId': 2,
        //        'FirstName' = 'Jane',
        //        'LastName' = 'Smith',
        //    Age = 51,
        //    Gender = Gender.Female,
        //    Education = Education.Masters,
        //    Interests = "reading, yoga, traveling",
        //    Phone = "667456789",
        //    Email = "jane.smith23@hotmail.com",
        //    DateSubmitted = new DateTime(2021, 08, 15),
        //    Inactive = false,
        //    UserId = "2",
        //    Addresses = new List<AddressModel>()
        //};
    }
}
