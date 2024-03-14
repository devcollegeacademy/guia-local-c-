using System.ComponentModel.DataAnnotations;

namespace guialocal.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set;  }                       
        public string Title { get; set; }
        public string ZipCode { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }   
    }
}