using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test 
    {
        public Test()
        {

        }
        [HttpGet]
        public string Test2(string hello)
        {
            return hello;
        }
    }
}
