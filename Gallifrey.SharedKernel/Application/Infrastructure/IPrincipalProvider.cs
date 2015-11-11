using System.Security.Principal;

namespace Gallifrey.SharedKernel.Application.Infrastructure
{
    public interface IPrincipalProvider
    {
        IPrincipal GetPrincipal();
    }
}
