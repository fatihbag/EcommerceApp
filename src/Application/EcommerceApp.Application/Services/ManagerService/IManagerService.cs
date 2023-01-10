using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Services.AdminService
{
    public interface IManagerService
    {
        Task CreateManager(AddManagerDTO addManagerDTO);
        Task<List<ListOfManagerVM>> GetAdmins();
        Task<List<ListOfManagerVM>> GetManagers();
        Task<UpdateManagerDTO> GetManager(Guid id);//Controller içerisine IAdmin çağırdık içindeki metotları görsün eklesin diye yazdık
        Task updateManager(UpdateManagerDTO updateManagerDTO);

        Task DeleteManager(Guid id);
        
    }
}
