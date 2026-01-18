using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP_NET_05._ASP_Filters.Filters;

public class ApiKeyQuery : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAutorized = context
            .HttpContext
            .Request
            .Query
            .Any(q => q.Key == "apiKey" && q.Value == "654321ytrewq");
        if (!isAutorized) context.Result = new UnauthorizedResult();

    }
}
