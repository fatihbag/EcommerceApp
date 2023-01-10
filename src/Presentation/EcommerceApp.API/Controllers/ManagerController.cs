using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using EcommerceApp.Application.Services.AdminService;
using EcommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApp.API.Controllers
{
    [Route("api/[controller]")]//locakhost:7001/api/Manager-->httppost varsa onu çalıştıırır
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _adminServices;
        public ManagerController(IManagerService adminService)
        {
            _adminServices= adminService;
        }

        [HttpGet("GetManagers")]
        public async Task<ActionResult<List<ListOfManagerVM>>> GetAllManagers()
        {
            var managers = await _adminServices.GetManagers();
            if (managers == null)
            {
                return NotFound();
            }
            return Ok(managers);
        }

        [HttpGet("id")]
        public async Task<ActionResult<UpdateManagerDTO>> GetManager([FromRoute]Guid id)
        {
            var manager = await _adminServices.GetManager(id);
            if (manager==null)
            {
                return NotFound();
            }
            return Ok(manager);
        }
        [HttpDelete("id")]
        public async Task<ActionResult<UpdateManagerDTO>> DeleteManager([FromRoute]Guid id)
        {
           await _adminServices.GetManager(id);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateManager([FromForm]AddManagerDTO addManagerDTO)
        {
            
            try
            {
                await _adminServices.CreateManager(addManagerDTO);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return Ok();
        }

    }

    
}
