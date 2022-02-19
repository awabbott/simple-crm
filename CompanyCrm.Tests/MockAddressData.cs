using CustomerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyCrm.Tests
{
    public class MockAddressData
    {
        public static readonly AddressModel[] mockAddressList = new AddressModel[]
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
        };
    }
}
