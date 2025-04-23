using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;

public class AdminController : Controller
{
    [Route("admin/panel")]
    public IActionResult AdminPanel()
    {
        return View();
    }

    [Route("admin/create-edit")]
    public IActionResult AdminCreateEditPanel()
    {
        return View();
    }

}