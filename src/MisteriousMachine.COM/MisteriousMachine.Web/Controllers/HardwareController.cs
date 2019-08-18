using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MisteriousMachine.COM;

namespace MisteriousMachine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static Dictionary<string, UcApi> Ports = new Dictionary<string, UcApi>();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{port}")]
        public ActionResult<string> Get(string port)
        {
            var query = this.HttpContext.Request.Query.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.First()));
            string command = new Commands().Match(query);

            var api = default(UcApi);
            if (Ports.ContainsKey(port.ToLower()))
            {
                api = Ports[port.ToLower()];
            }
            else
            {
                api = Ports[port.ToLower()] = new UcApi(port.ToLower());
            }

            api.Invoke(command);

            return this.Ok("ok");

            return this.BadRequest("Method cant be matched");
        }
    }
}
