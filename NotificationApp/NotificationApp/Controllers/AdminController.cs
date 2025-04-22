using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;

public class AdminController : Controller
{
    public IActionResult AdminPanel()
    {
        return View();
    }
}