using CustomerModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCrmApi.Data.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

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
        
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
