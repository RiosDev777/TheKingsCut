using KingsCut.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KingsCut.Web.Core.Attributes
{
    public class CustomAuthorizedAttribute : TypeFilterAttribute
    {
        public CustomAuthorizedAttribute(string permission, string module) : base(typeof(CustomAuthorizedFilter))
        {
            Arguments = [permission, module];
        }

        public class CustomAuthorizedFilter : IAsyncAuthorizationFilter
        {

            private readonly string _permission;
            private readonly string _module;
            private readonly IUsersService _usersService;

        public CustomAuthorizedFilter(IUsersService usersService, string permission, string module)
            {
                _usersService = usersService;
                _permission = permission;
                _module = module;
            }


            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {

                bool isAuthorized = await _usersService.CurrentUserIsAuthorizedAsync(_permission, _module);

                if (!isAuthorized) 
                { 
                
                    context.Result = new ForbidResult();
                
                }
            }
        }
    }

      
}
