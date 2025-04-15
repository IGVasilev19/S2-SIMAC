using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;

public class AdminController : Controller
{
    public IActionResult AdminPanel()
    {
        return View();
    }
}