using AutoMapper;
using CompanyCrmApi.Data.Entities;
using CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            this.CreateMap<Customer, CustomerModel>()
                .ReverseMap()
                .ForMember(c => c.Addresses, opt => opt.Ignore())
                .ForMember(c => c.CustomerId, opt => opt.Ignore());

            this.CreateMap<Address, AddressModel>()
                .ReverseMap()
                .ForMember(a => a.AddressId, opt => opt.Ignore());
        }
    }
}
