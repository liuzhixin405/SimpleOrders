using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Notifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifierController : ControllerBase
    {
        [HttpPost]
        public int Post([FromBody] Notifier notifier)
        {
            Console.WriteLine($"Sent notifacation for : {notifier.ProductName}");
            return 3;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Sent rollback transaction notification: {id}");
        }
    }

    public class Notifier
    {
       
        public string ProductName { get; set; }
    }
}
