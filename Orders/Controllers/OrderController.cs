using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public int Post([FromBody] Order order)
        {
            Console.WriteLine($"Create new order: {order.ProductName}");
            return 1;
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Delete order: {id}");
        }
    }

    public class Order
    {
        public string ProductName { get; set; }
        public int Id { get; set; }
    }
}
