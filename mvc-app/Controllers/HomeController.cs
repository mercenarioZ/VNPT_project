using Microsoft.AspNetCore.Mvc;
using mvc_app.Data;
using mvc_app.Models;
using System;
using System.Diagnostics;

namespace mvc_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult UserLog()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                
                return View();
            }

            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult UserLog(IFormCollection formCollection)
        {
            string arg = "";
            foreach (string key in formCollection.Keys)
            {
                // Console.WriteLine("asss"+ key+" "+formCollection[key]);
                // Response.WriteAsync(string.Format("", key));
                arg += " " + formCollection[key];
            }
            string resultString = RunCMD(arg);
            var multiString = resultString.Split("\r\n");
            // Console.WriteLine(resultString);
            // _logger.Log(LogLevel.Debug,resultString);
            // _logger.LogWarning(resultString);
            string User1;
            User1 = (string?)formCollection["user"].FirstOrDefault()??"";
            ViewData["User"] = User1;
            _logger.LogInformation(resultString);
            return View((object)multiString);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private string RunCMD(string args)
        {
            var start = new ProcessStartInfo();
            start.FileName = "C:/Users/PhuTuan/anaconda3/envs/pythonProject/python.exe";
            start.Arguments = string.Format("{0} {1}", "C:/Users/PhuTuan/Documents/VSC/C#/VNPT_project/check_log_demo.py", args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            string resultString = "";
            using(Process process = Process.Start(start))
            {
                using(StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    resultString += result;
                    // Console.Write(result);
                }
            }
            return resultString;

        }
    }
}