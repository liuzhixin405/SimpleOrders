using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpPost]
        public async Task<OrderResponse> Post([FromBody] Order order)
        {
            var request = System.Text.Json.JsonSerializer.Serialize(order);
          
            //Create order
            var orderClient = _httpClientFactory.CreateClient("Order");
            var orderResponse =await orderClient.PostAsync("/api/order", new StringContent(request,Encoding.UTF8,"application/json"));
            var orderId =await orderResponse.Content.ReadAsStringAsync();
            //update inventory
            string inventoryId = String.Empty;
            try
            {
                var inventoryClient = _httpClientFactory.CreateClient("Inventory");
                var inventoryResponse = await inventoryClient.PostAsync("/api/inventoy", new StringContent(request, Encoding.UTF8, "application/json"));
                if (inventoryResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(inventoryResponse.ReasonPhrase);
                inventoryId = await inventoryResponse.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                await orderClient.DeleteAsync($"/api/order/{orderId}");
                return new OrderResponse { Success = false,Reason = ex.Message};
            }
          
            //send notification
            var notificationClient = _httpClientFactory.CreateClient("notifier");
            var notifierResponse = await notificationClient.PostAsync("/api/notifier", new StringContent(request, Encoding.UTF8, "application/json"));
            var notifierId = await notifierResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Order:{orderId}, Inventory:{inventoryId},Notifier:{notifierId}");
            return new OrderResponse { OrderId = orderId ,Success = true};
        }
    }

    public class OrderResponse
    {
        public string OrderId { get; internal set; }
        public bool Success { get; internal set; }
        public string Reason { get; internal set; }
    }

    public class Order
    {
        public string ProductName { get; set; }
    }
}
