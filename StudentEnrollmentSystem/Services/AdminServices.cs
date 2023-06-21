using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudentEnrollmentSystem.Authentication;

namespace StudentEnrollmentSystem.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IMapper _mapper;
        public AdminServices(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<StatusResponse> RegisterAdmin(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new StatusResponse { Status = "Error", Message = "User already exists." };
            }

            IdentityUser newUser = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return new StatusResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." };
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Student))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Student))
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
            }
            return new StatusResponse { Status = "Success", Message = "User created successfully!" };
        }

        public async Task<IEnumerable<AdminDTO>> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
            List<AdminDTO> adminList = new List<AdminDTO>();
            foreach (var admin in admins)
            {
                var adminDTO = _mapper.Map<AdminDTO>(admin);
                adminList.Add(adminDTO);
            }
            return adminList;
        }

        public async Task<AdminDTO> GetAdmin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new NotFoundException("No such admin!");
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin) == false)
            {
                throw new BadRequestException("User is not an admin!");
            }
            var admin = _mapper.Map<AdminDTO>(user);
            return admin;
        }

        //Can only change email, change password is in AuthServices
        public async Task<StatusResponse> UpdateAdmin(AdminDTO adminDTO)
        {
            var user = await _userManager.FindByNameAsync(adminDTO.UserName);
            if (user == null)
            {
                throw new NotFoundException("No such admin!");
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin) == false)
            {
                throw new BadRequestException("User is not an admin!");
            }
            user.Email = adminDTO.Email;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) 
            {
                return new StatusResponse { Status = "Error", Message = "Failed to change email of this admin!" };
            }
            return new StatusResponse { Status = "Success", Message = "Successfully updated email of admin!" };
        }

        public async Task<StatusResponse> HardDeleteAdmin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new NotFoundException("No such admin!");
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin) == false)
            {
                throw new BadRequestException("User is not an admin!");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new StatusResponse { Status = "Error", Message = "Cannot delete this admin!" };
            }
            return new StatusResponse { Status = "Success", Message = "Admin deleted successfully!" };
        }
    }
}
