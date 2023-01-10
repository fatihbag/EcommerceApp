﻿using AutoMapper;
using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Models.VMs;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Enums;
using EcommerceApp.Domain.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Services.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepo _employeeRepo;

        public AdminService(IMapper mapper, IEmployeeRepo employeeRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
        }

        public async Task CreateAdmin(AddAdminDTO addAdminDTO)
        {
            var addAdmin = _mapper.Map<Employee>(addAdminDTO);

            if (addAdmin.UploadPath != null)
            {
                using var image = Image.Load(addAdminDTO.UploadPath.OpenReadStream());
                //Dosyayı yolunu okuduk

                image.Mutate(x => x.Resize(600, 560));//Resim boyutu ayarladık

                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");

                addAdmin.ImagePath = ($"/images/{guid}.jpg");
                await _employeeRepo.Create(addAdmin);

            }
            else
            {
                addAdmin.ImagePath = ($"/images/default.jpeg");
                await _employeeRepo.Create(addAdmin);
            }

        }

       
        public  Task<List<ListOfAdminVM>> GetAdmins()
        {
            var admins =  _employeeRepo.GetFilteredList(
                select: x => new ListOfAdminVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Roles = x.Roles,
                    ImagePath = x.ImagePath,
                },
                where: x => ((x.Status == Status.Active || x.Status == Status.Modified) && x.Roles == Roles.Admin),
                orderBy: x => x.OrderBy(x => x.Name));

            return admins;
        }

        public async Task<UpdateAdminDTO> GetAdmin(Guid id)
        {
            var admin = await _employeeRepo.GetFilteredFirstOrDefault(
                select: x => new UpdateAdminVM
                {
                    Id = id,
                    Name = x.Name,
                    Surname = x.Surname,
                    ImagePath = x.ImagePath,
                    //UploadPath=x.UploadPath,-->veri tabanında yok gitmesini engellemiştik
                },
                where: x => x.Id == id);
            var updateAdminDTO = _mapper.Map<UpdateAdminDTO>(admin);
            return updateAdminDTO;
        }

        public async Task updateAdmin(UpdateAdminDTO updateAdminDTO)
        {
            var model = await _employeeRepo.GetDefault(x => x.Id == updateAdminDTO.Id);

            model.Name = updateAdminDTO.Name;
            model.Surname = updateAdminDTO.Surname;
            //model.ImagePath=updateManagerDTO.ImagePath;
            model.UpdateDate = updateAdminDTO.UpdateDate;
            model.Status = updateAdminDTO.Status;

            //dosyayı buraya kaydetmek için

            using var image = Image.Load(updateAdminDTO.UploadPath.OpenReadStream());//Dosyayı yolunu okuduk


            image.Mutate(x => x.Resize(600, 560));//Resim boyutu ayarladık

            Guid guid = Guid.NewGuid();
            image.Save($"wwwroot/images/{guid}.jpg");

            model.ImagePath = ($"/images/{guid}.jpg");
            await _employeeRepo.Update(model);
        }
        public async Task DeleteAdmin(Guid id)
        {
            var model = await _employeeRepo.GetDefault(x => x.Id == id);
            model.DeleteDate = DateTime.Now;
            model.Status = Status.Passive;

            await _employeeRepo.Delete(model);

        }

        

      
    }
}
