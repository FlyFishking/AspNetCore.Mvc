using EFCore.Kernal;
using EFCore.Model.Model;
using EFCore.Repository;
using EFCore.Service.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using EFCore.Kernal.Filter;

namespace WebApplication4.Controllers
{

    public class BaseController : Controller
    {
        
    }
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        public IValuesService svcValue { get; set; }

        public IStudentService svcStudent { get; set; }
        public IStudentRepository repoStudent { get; set; }
        public ValuesController()
        {
        }

        [CustomException]
        [Stopwatch]
        // GET api/values
        [Route("details/{id}")]
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            var allStudents = svcStudent.GetAllStudents();
            allStudents.ForEach(t =>
            {
                t.DynamicObj = new List<Student>()
                {
                    new Student(){LastName="111",FirstMidName = "first111"},
                    new Student(){LastName="222",FirstMidName = "secend222"}
                };
            });

//            return new string[] { "value1", "value2" };
            return allStudents;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
