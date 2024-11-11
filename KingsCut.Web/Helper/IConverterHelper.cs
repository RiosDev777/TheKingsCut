using KingsCut.Web.Data.Entities;
using KingsCut.Web.DTOs;
using System.Reflection.Metadata;

namespace KingsCut.Web.Helper
{
    public interface IConverterHelper
    {
        
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(ICombosHelper combosHelper)
        {
            _combosHelper = combosHelper;
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
