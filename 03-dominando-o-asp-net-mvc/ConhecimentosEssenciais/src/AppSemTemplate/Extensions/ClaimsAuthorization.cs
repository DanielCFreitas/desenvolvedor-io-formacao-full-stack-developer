
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AppSemTemplate.Extensions
{
    public class ClaimsAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext httpContext, string claimName, string claimValue)
        {
            if (httpContext.User.Identity == null) throw new InvalidOperationException();

            return httpContext.User.Identity.IsAuthenticated &&
                    httpContext.User.Claims.Any(
                        claim => claim.Type == claimName &&
                        claim.Value.Split(",").Contains(claimValue));
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity == null) throw new InvalidOperationException();

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                RedirecionarUsuarioParaTelaDeLogin(context);
                return;
            }

            if (!ClaimsAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }

        private void RedirecionarUsuarioParaTelaDeLogin(AuthorizationFilterContext context)
        {
            var informacoesDeRedirecionamento = new
            {
                area = "Identity",
                page = "/Account/Login",
                ReturnUrl = context.HttpContext.Request.Path.ToString()
            };

            var routeValueDictionary = new RouteValueDictionary(informacoesDeRedirecionamento);

            context.Result = new RedirectToRouteResult(routeValueDictionary);
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = [new Claim(claimName, claimValue)];
        }
    }
}
