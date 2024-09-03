using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TimesJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeStoriesController : ControllerBase
    {
        [HttpGet("getTimeStories")]
        public async Task<IActionResult> GetTimeStories()
        {
            var url = "https://time.com";
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var stories = htmlDocument.DocumentNode
                .SelectNodes("//a[@href]")
                .Where(node => node.Attributes["href"].Value.StartsWith("/"))
                .Take(6)
                .Select(node => new
                {
                    title = node.InnerText.Trim(),
                    link = "https://time.com" + node.Attributes["href"].Value
                })
                .ToList();

            return Ok(stories);
        }
    }
}
