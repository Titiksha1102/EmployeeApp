using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.AddressRepo
{
    public interface IAddressRepository
    {
        public Task<Address> Create(Address address);
        public Task<Address> Edit(Address address);
        public Task<bool> Delete(int id);
        public Task<Address> GetAddress(int id);
        public Task<IEnumerable<Address>> GetAddresses();
    }
}
