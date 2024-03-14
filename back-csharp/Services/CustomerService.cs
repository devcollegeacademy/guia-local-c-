using Microsoft.EntityFrameworkCore;
using guialocal.Data;
using guialocal.Models;
using System.Threading.Tasks;

namespace guialocal.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer Create(Customer customer) 
        {
            var customerExist = _context.Customers.FirstOrDefault(c => c.Title == customer.Title);

            if (customerExist != null) {
                throw new Exception($"Já existe um cliente cadastrado com esse nome {customer.Title}");
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        public Customer[] ReadByFilter(string? title)
        {
            return _context.Customers.Where(c => EF.Functions.Like(c.Title, $"%{title}%") && c.Active).ToArray();
        }

        public Customer ReadOne(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email) ?? throw new Exception($"Não foi encontrado nenhuma clíente para esse email:  {email}");
        }

        public Customer Update(string email, Customer customerData)
        {
            var customerExists = _context.Customers.FirstOrDefault(c => c.Email == email) ?? throw new Exception($"Não foi encontrado nenhuma clíente para esse email:  {email}");

            customerExists.Title = customerData.Title;
            customerExists.ZipCode = customerData.ZipCode;
            customerExists.Number = customerData.Number;
            customerExists.Address = customerData.Address;            
            customerExists.Active = customerData.Active;

            _context.SaveChanges(); 

            return customerExists;
        }

        public void Delete(string email)
        {
            var customerExists = _context.Customers.FirstOrDefault(c => c.Email == email) ?? throw new Exception($"Não foi encontrado nenhuma clíente para esse email:  {email}");
            _context.Customers.Remove(customerExists);
            _context.SaveChanges();
        }
    }
}
