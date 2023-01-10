using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using EcommerceApp.Application.Services.AdminService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _adminService;
        public ManagerController(IManagerService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddManager()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddManager(AddManagerDTO addManagerDTO)
        {
            //if (ModelState.IsValid)
            //{
            //    await _adminService.CreateManager(addManagerDTO);
            //    return RedirectToAction(nameof(ListOfManagers));
            //}
            //return View();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7186/");

                var responseTask = client.PostAsJsonAsync<AddManagerDTO>("api/Manager/PostManager", addManagerDTO);

                responseTask.Wait();

                var resulTask = responseTask.Result;
                if (responseTask.IsCompletedSuccessfully)
                {
                    return RedirectToAction(nameof(ListOfManagers));
                }
                else
                {
                    return BadRequest();
                }
            }

        }

        public async Task<IActionResult> ListOfManagers()
        {
            //var managers = await _adminService.GetManagers();
            //return View(managers);  //apideen istek atmak için yoruma alddık
            using (var client = new HttpClient())//HttpClient api ye istek atılırken kulanılan sınıf
            {
                client.BaseAddress = new Uri("https://localhost:7186/");//Api'nini senin localinddedkia dresi ya da server adresi!!!
                var responseTask = client.GetAsync("api/Manager/GetManager");
                //Api'de bize bilgileri getirecek olan route'u yani actionResult'ı tetikledim.
                responseTask.Wait();//Bu işlemin gerçekleşmesini bekle

                var resultTask = responseTask.Result;

                if (responseTask.IsCompletedSuccessfully)
                {
                    var readTask = resultTask.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var readData = JsonConvert.DeserializeObject<List<ListOfManagerVM>>(readTask.Result);

                    return View(readData);
                }
                else
                {
                    ViewBag.EmptyList = "List is not found";
                    return View(new List<ListOfManagerVM>());
                }
            }
        }

        //[Route("[Manager]/[update]")]
        public async Task<IActionResult> UpdateManager(Guid id)
        {
            var updateManager =await _adminService.GetManager(id);
            return View(updateManager);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateManager(UpdateManagerDTO updateManagerDTO)
        {
            if (ModelState.IsValid)
            {
                await _adminService.updateManager(updateManagerDTO);

                return RedirectToAction(nameof(ListOfManagers));
            }
            return View(updateManagerDTO);
        }

        public async Task<IActionResult> DeleteManager(Guid id)
        {
            await _adminService.DeleteManager(id);
            return RedirectToAction(nameof(ListOfManagers));
        }
    }
}
