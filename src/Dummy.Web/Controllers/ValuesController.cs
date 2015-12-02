using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dummy.Web.Controllers
{
    public class Val
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Result
    {
        public Val[] results { get; set; }
    }

    public class ValuesController : ApiController
    {
        private readonly Val[] _vals;

        public ValuesController()
        {
            _vals = new []
            {
                new Val {Id = 1, Title = "One"},
                new Val {Id = 2, Title = "Two"}
            };
        }

        // GET api/values
        public Result Get()
        {
            return new Result {results = _vals};
        }

        // GET api/values/5
        public Result Get(int id)
        {
            return new Result { results = _vals.Where(v => v.Id == id).ToArray()};
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
