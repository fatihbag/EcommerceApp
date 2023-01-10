using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using EcommerceApp.Application.Services.AdminService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminServices;
        public AdminController(IAdminService adminService)
        {
            _adminServices = adminService;
        }

        [HttpGet("GetAdmins")]
        public async Task<ActionResult<List<ListOfAdminVM>>> GetAllAdmin()
        {
            var managers = await _adminServices.GetAdmins();
            if (managers == null)
            {
                return NotFound();
            }
            return Ok(managers);
        }

        [HttpGet("id")]
        public async Task<ActionResult<UpdateManagerDTO>> GetAdmin([FromRoute] Guid id)
        {
            var admin = await _adminServices.GetAdmin(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }
        [HttpDelete("id")]
        public async Task<ActionResult<UpdateManagerDTO>> DeleteAdmin([FromRoute] Guid id)
        {
            await _adminServices.GetAdmin(id);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmin([FromForm] AddAdminDTO addAdminDTO)
        {

            try
            {
                await _adminServices.CreateAdmin(addAdminDTO);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return Ok();
        }
    }
}
