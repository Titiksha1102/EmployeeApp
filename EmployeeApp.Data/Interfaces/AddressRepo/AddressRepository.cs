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
        public async Task<Address> Create(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> Edit(Address address)
        {
            _context.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
