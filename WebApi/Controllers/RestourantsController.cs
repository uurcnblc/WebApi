using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestourantsController : ControllerBase
    {
        // GET: api/<RestourantsController>
        [HttpGet]
        [Route("List")]
        [Authorize(Permissions.Restourants.List)]
        public IEnumerable<string> List()
        {
            return new string[] { "Restorant 1", "Restorant 2" };
        }

        // GET api/<RestourantsController>/5
        [HttpGet]
        [Route("Detail")]
        [Authorize(Permissions.Restourants.Detail)]
        public string Detail(int id)
        {
            return "Restorant 1 Detay";
        }

        // POST api/<RestourantsController>
        [HttpGet]
        [Route("Create")]
        [Authorize(Permissions.Restourants.Create)]
        public async Task<Object> Create([FromBody] string value)
        {
            return Ok("Restorant kaydedildi");
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Permissions.Restourants.Edit)]
        public async Task<Object> Edit(int id, [FromBody] string value)
        {
            return Ok("Restorant guncellendi");
        }

        // DELETE api/<RestourantsController>/5
        [HttpGet]
        [Route("Delete")]
        [Authorize(Permissions.Restourants.Delete)]
        public async Task<Object> Delete(int id)
        {
            return Ok("Restorant silindi");
        }
    }
}
