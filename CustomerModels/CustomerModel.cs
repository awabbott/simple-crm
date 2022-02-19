using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerModels
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        private string _fullName;
        public string FullName
        {
            get
            {
                _fullName = LastName;
                if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    if (!string.IsNullOrWhiteSpace(_fullName))
                    {
                        _fullName += ", ";
                    }

                    _fullName += FirstName;
                }
                return _fullName;
            }
        }

        [Range(0, 120)]
        public int Age { get; set; }

        public Gender Gender { get; set; }

        public Education Education { get; set; }

        public string Interests { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.MinValue;

        public bool Inactive { get; set; }

        public string UserId { get; set; }

        public ICollection<AddressModel> Addresses { get; set; }
            = new List<AddressModel>();
    }
}
