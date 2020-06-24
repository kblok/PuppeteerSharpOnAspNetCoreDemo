using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharpOnAspNetCoreDemo.Models;

namespace PuppeteerSharpOnAspNetCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var options = new LaunchOptions
            {
                Headless = false,
                Args = new string[]
                {
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--incognito"
                }
            };
            Page page;

            var browser = await Puppeteer.LaunchAsync(options);
            var browserPages = await browser.PagesAsync();
            if (browserPages.Length > 0)
            {
                page = browserPages[0];
                await Task.WhenAll(browserPages.Skip(1).Select(x => x.CloseAsync()));
            }
            else
            {
                page = await browser.NewPageAsync();
            }

            // some other steps 

            await page.DisposeAsync();
            await browser.DisposeAsync();
            // this log sometime never print out
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
