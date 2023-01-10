using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Services.AdminService
{
    public interface IAdminService
    {
        Task CreateAdmin(AddAdminDTO addAdminDTO);
        Task<List<ListOfAdminVM>> GetAdmins();
        Task<UpdateAdminDTO> GetAdmin(Guid id);//Controller içerisine IAdmin çağırdık içindeki metotları görsün eklesin diye yazdık
        Task updateAdmin(UpdateAdminDTO updateAdminDTO);

        Task DeleteAdmin(Guid id);
    }
}
