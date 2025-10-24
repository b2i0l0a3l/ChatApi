using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Req;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Interfaces;
using ChatApi.Application.Interfaces.Auth;
using ChatApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApi.Application.Services.AuthService.Register
{
    public class RegisterService : IRegister
    {
        private readonly UserManager<Infrastructure.Identity.ApplicationUser> _userManager; 
        private readonly IUploadFile _uploadFileService;
        public RegisterService(UserManager<Infrastructure.Identity.ApplicationUser> userManager, IUploadFile uploadFileService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _uploadFileService = uploadFileService;
        }

        private async Task<bool> CheckUserIfExists(string Email)
        {
            var IsUserExist = await _userManager.FindByNameAsync(Email);
            return IsUserExist != null;
        }
        private async Task<(string,string)> UploadProfileImage(IFormFile file)
             => await _uploadFileService.UploadFile(file);
        public async Task<GeneralResponse<string>> Register(SingUp model)
        {
            string uploadedImagePath = string.Empty;
            if (model == null)
                return GeneralResponse<string>.Failure("Invalid user data", StatusCodes.Status400BadRequest);

            try
            {
                if (await CheckUserIfExists(model.Email))
                    return GeneralResponse<string>.Failure("User already exists", StatusCodes.Status409Conflict);

                (string, string) i = await UploadProfileImage(model.ProfileImage);
                uploadedImagePath = i.Item2;
                ApplicationUser user = new()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImage = i.Item1
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (!result.Succeeded)
                {
                    return GeneralResponse<string>.Failure($"{string.Join(", " ,result.Errors.Select(e => e.Description))}", StatusCodes.Status500InternalServerError);
                }
                return GeneralResponse<string>.Success("", "User registered successfully", StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                if ( uploadedImagePath!= null && File.Exists(uploadedImagePath))
                    File.Delete(uploadedImagePath);
                return GeneralResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }

        }
    }
}