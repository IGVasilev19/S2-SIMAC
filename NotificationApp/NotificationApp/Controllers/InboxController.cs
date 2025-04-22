using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;

public class InboxController : Controller
{
    public IActionResult Inbox()
    {
        return View();
    }
}