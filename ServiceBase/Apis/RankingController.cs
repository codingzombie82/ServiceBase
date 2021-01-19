using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBase.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RankingController : ControllerBase
    {
        //개체 형식
        //[HttpGet]
        //public Subject Get()
        //{
        //    return new Subject { kor = 95, Eng = 100, Total =  195};
        //}

        //컬렉션 형식
        //[HttpGet]
        //public IEnumerable<Student> Get()
        //{
        //    return new List<Student> {
        //        new Student{Id = 0, Name ="홍길동", Score = 1},
        //        new Student{Id = 1, Name ="이길동", Score = 2},
        //        new Student{Id = 3, Name ="삼길동", Score = 3}
        //    };
        //}

        //복합형식
        [HttpGet]
        public RankingDto Get()
        {
            var subject = new Subject { kor = 95, Eng = 100, Total = 195 };
            var students =  new List<Student> {
                new Student{Id = 0, Name ="홍길동", Score = 1},
                new Student{Id = 1, Name ="이길동", Score = 2},
                new Student{Id = 3, Name ="삼길동", Score = 3}
            };

            var rankings = new RankingDto
            {
                Subject = subject,
                Students = students
            };

            return rankings;
        }
    }

    public class Subject
    {
        public int kor { get; set; }
        public int Eng { get; set; }
        public int Total { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public class RankingDto
    {
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; }

    }
}
