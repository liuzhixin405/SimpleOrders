using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public int Post([FromBody] Inventory inventory)
        {
            //throw new Exception(" inventory is zero , Error ");   测试订单失败用
            Console.WriteLine($"Update inventory for: {inventory.ProductName}");
            return 2;
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Delete inventory: {id}");
        }
    }

    public class Inventory
    {
        public string ProductName { get; set; }
    }
}
