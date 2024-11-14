using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace KingsCut.Web.Helper
{
    public interface IConverterHelper
    {
        public KingsCutRole ToRole(KingsCutRoleDTO dto);
        public Task<KingsCutRoleDTO> ToRoleDTOAsync(KingsCutRole role);
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _context;

        public ConverterHelper(ICombosHelper combosHelper, DataContext context)
        {
            _combosHelper = combosHelper;
            _context = context;
        }

        public  KingsCutRole ToRole(KingsCutRoleDTO dto)
        {

            return new KingsCutRole
            {

                Id = dto.Id,
                Name = dto.Name,

            };

        }

        public async Task<KingsCutRoleDTO> ToRoleDTOAsync(KingsCutRole role)
        {

            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {

                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)      

            }).ToListAsync();

            return new KingsCutRoleDTO

            {

                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,

            };
        }

        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id.ToString(),
                Document = dto.Document,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                KingsCutRoleId = dto.KingsCutRoleId,
                PhoneNumber = dto.PhoneNumber,
            };
        }

        public async Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true)
        {
            return new UserDTO
            {
                Id = isNew ? Guid.NewGuid() : Guid.Parse(user.Id),
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync(),
                KingsCutRoleId = user.KingsCutRoleId,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
