using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IUserRepository db;
        public HomeController(IUserRepository context)
        {
            db = context;
        }
        
        [Route("{id}/GetReferendums")]
        public ActionResult<IEnumerable<SimpleReferendums>> GetRefer()
        {
            return db.GetReferendumsDbNew();
        }

        [Route("{id}/GetSingleReferendum")]
        public ActionResult<IEnumerable<GetReferendums>> GetSingleRefer([FromBody] User user, string id)
        {
            return db.GetSingleReferendumDb(user, id);
        }

        [Route("{id}/GetMyReferendums")]
        public ActionResult<IEnumerable<MyReferendum>> GetMyRefer(string id)
        {
            return db.GetMyReferendumsDb(id);
        }

        // GET: api/Home/5
        /*[HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            return "value";
        }

        [Route("{id}/update")]
        public string GetId(string id)
        {
            return id;
        }*/

        // POST: api/Home
        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            db.Create(newUser);
            return Ok(newUser);
        }

        [Route("{id}/Vote")]
        public IActionResult AddVote([FromBody] AllAnswer answer)
        {
            db.Vote(answer);
            return Ok(answer); 
        }

        [Route("{id}/AddReferendum")]
        public IActionResult AddReferendum([FromBody] Referendum referendum)
        {
            db.AddReferendum(referendum);
            return Ok(referendum);
        }

        [Route("{id}/ReceiveAllPropositions")]        
        public ActionResult<IEnumerable<ReceiveAllReferendums>> ReceiveAllPropositions()
        {
            return db.ReceiveAllReferendumsDb();
        }

        [Route("{id}/SendAnswer")]
        public IActionResult SaveAnswer([FromBody] AddAnswer answer)
        {
            db.SaveNewAnswer(answer);
            return Ok(answer);
        }

        [Route("{id}/Authorize")]
        public ActionResult<User> Authorization([FromBody] User user)
        {
            return db.Authorize(user);
        }

        // PUT: api/Home/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
