using System.Security.Principal;
using System.Web;
using Gallifrey.SharedKernel.Application.Infrastructure;

namespace Gallifrey.Infrastructure.Application.Provider
{
    public class WebUserPrincipalProvider : IPrincipalProvider
    {
        private readonly HttpContext _context;

        public WebUserPrincipalProvider(HttpContext context)
        {
            _context = context;
        }

        public IPrincipal GetPrincipal()
        {
            return _context.User;
        }
    }
}