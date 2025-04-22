using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;

public class InboxController : Controller
{
    public IActionResult Inbox()
    {
        return View();
    }
}