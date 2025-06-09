using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Service.Interfaces;
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredPermissions;

    public PermissionAttribute(params string[] permissions)
    {
        _requiredPermissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userIdString = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdString, out int userId))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "System", null);
            return;
        }

        var accountService = context.HttpContext.RequestServices.GetService(typeof(IAccountService)) as IAccountService;
        var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService)) as IPermissionService;

        if (accountService == null || permissionService == null)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "System", null);
            return;
        }

        var account = accountService.GetById(userId);
        var userPermissions = permissionService.GetPermissionsByRoleId(account.RoleId);

        bool hasAccess = userPermissions.Any(p => _requiredPermissions.Contains(p.Name));

        if (!hasAccess)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "System", null);
        }
    }
}