using EmployeeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.ServiceApi.Controllers.AdressesService
{
    public interface IAddressesService
    {
        public Task<ActionResult<Address>> PostAddress(Address Address);//create
        public Task<ActionResult<IEnumerable<Address>>> GetAddresses();
        public Task<ActionResult<Address>> GetAddress(int id);
        public Task<ActionResult<Address>> PutAddress(Address e);//edit
        public Task<ActionResult<bool>> DeleteAddress(int id);
    }
}
