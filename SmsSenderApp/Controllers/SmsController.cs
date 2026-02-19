using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

public class SmsController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public SmsController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string mobileNumber, string message)
    {
        if (string.IsNullOrEmpty(mobileNumber) || string.IsNullOrEmpty(message))
        {
            ViewBag.Error = "All fields are required!";
            return View();
        }

        var client = _clientFactory.CreateClient();

        // Demo SMS Body
        var smsBody = new
        {
            mobile = mobileNumber,
            message = message
        };

        var smsJson = JsonSerializer.Serialize(smsBody);
        var smsContent = new StringContent(smsJson, Encoding.UTF8, "application/json");

        // Demo API Call
        var response = await client.PostAsync("https://jsonplaceholder.typicode.com/posts", smsContent);

        if (response.IsSuccessStatusCode)
        {
            ViewBag.Success = "Demo API Call SMS Sent Successfully 😎";
        }
        else
        {
            ViewBag.Error = "Demo SMS Failed!";
        }

        return View();
    }
}
