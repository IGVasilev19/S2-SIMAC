using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;

public class DevicesController : Controller
{
    public IActionResult devicespanel()
    {
        return View();
    }
}