using EmployeeApp.Data.Data;


using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Address>> GetAddresses()
        {
            var result = await _context.Addresses
                .ToListAsync();
            return result;
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
        public async Task<bool> Delete(int id)
        {
            var address= await _context.Addresses.FirstOrDefaultAsync(a=>a.Id==id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            var deletedobj = _context.Addresses.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }
        public async Task<Address> GetAddress(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            return address;
        }
    }
}
