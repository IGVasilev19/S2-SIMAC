using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Interfaces;

namespace NotificationApp.Controllers
{
    public class AdminController : Controller
    {
       public IActionResult AdminPanel()
        {
            return View();
        }
        
        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }
    }
}