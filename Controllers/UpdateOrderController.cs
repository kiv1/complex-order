using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using updateorder.ViewModels;

namespace updateorder.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateOrderController : ControllerBase
{    
    private static readonly string AuthUrl = Environment.GetEnvironmentVariable("AUTH_URL") ?? "";
    private static readonly string OrderUrl = Environment.GetEnvironmentVariable("ORDER_URL") ?? "";
    private static readonly string EmailUrl = Environment.GetEnvironmentVariable("EMAIL_URL") ?? "";

    private readonly ILogger<UpdateOrderController> _logger;

    public UpdateOrderController(ILogger<UpdateOrderController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
    
    [HttpPut("sent/{orderId}")]
    public async Task<IActionResult> UpdateOrderToSent(string orderId)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync($"{OrderUrl}/order/orderid/{orderId}");
        var responseContent = await result.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<OrderModel>(responseContent);
        if (order.Orderstatus == "Pending")
        {                           
            var data = new StringContent("{\"orderStatus\":\"Sent\"}", Encoding.UTF8, "application/json");
            await client.PutAsync($"{OrderUrl}/order/"+orderId,data);
            
            var userResult = client.GetAsync($"{AuthUrl}/auth/user/{order.UserId}");
            responseContent = await userResult.Result.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseContent);
            data = new StringContent("{\"name\":\""+user.Name+"+\",\"email\":\""+user.Email+"\"}", Encoding.UTF8, "application/json");
            await client.PostAsync($"{EmailUrl}/email/sent/{orderId}", data);
        }

        return Ok(responseContent);
    }
    
    [HttpPut("receive/{orderId}")]
    public async Task<IActionResult> UpdateOrderToReceived(string orderId)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync($"{OrderUrl}/order/orderid/{orderId}");
        var responseContent = await result.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<OrderModel>(responseContent);
        if (order.Orderstatus == "Sent")
        {                           
            var data = new StringContent("{\"orderStatus\":\"Received\"}", Encoding.UTF8, "application/json");
            await client.PutAsync($"{OrderUrl}/order/"+orderId,data);
            
            var userResult = client.GetAsync($"{AuthUrl}/auth/user/{order.UserId}");
            responseContent = await userResult.Result.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseContent);
            data = new StringContent("{\"name\":\""+user.Name+"+\",\"email\":\""+user.Email+"\"}", Encoding.UTF8, "application/json");
            await client.PostAsync($"{EmailUrl}/email/receive/{orderId}", data);
        }

        return Ok(responseContent);
    }
}