using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.ServiceApi.Controllers.AdressesService
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesServiceController : ControllerBase, IAddressesService
    {
        private readonly IAddressRepository _addrepo;
        public AddressesServiceController(IAddressRepository addrepo)
        {
             _addrepo=addrepo;
        }
        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult<bool>> DeleteAddress(int id)
        {
            var result = await _addrepo.Delete(id);
            return result;
        }
        [HttpGet("GetAddress")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var result = await _addrepo.GetAddress(id);


            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("GetAddresses")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            var result = await _addrepo.GetAddresses();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost("CreateAddress")]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            var result = await _addrepo.Create(address);
            if (result is Address)
                return Ok(result);
            else
                return StatusCode(500, "Internal server error");
        }
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<Address>> PutAddress(Address e)
        {
            var result = await _addrepo.Edit(e);
            if (result is Address)
                return Ok(result);
            else
                return StatusCode(500, "Internal server error");
        }
    }
}
