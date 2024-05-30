using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.AddressRepo
{
    public class AddressRepository:IAddressRepository
    {
        private readonly EmployeeDB2Context _context;
        public AddressRepository(EmployeeDB2Context context)
        {
            _context = context;
        }
        public Address Create(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
            return address;
        }

        public Address Edit(Address address)
        {
            _context.Update(address);
            _context.SaveChanges();
            return address;
        }
    }
}
