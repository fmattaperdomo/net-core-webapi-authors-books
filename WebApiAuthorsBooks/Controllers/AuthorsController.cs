using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAuthorsBooks.Contexts;
using WebApiAuthorsBooks.Entities;

namespace WebApiAuthorsBooks.Controllers
{
    [Route("api/Autores")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext context;
        public AuthorsController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            return context.Authors.Include(x => x.Books).ToList();
        }

        [HttpGet("{id}", Name ="ObtenerAutor")]
        public ActionResult<Author> Get(int id)
        {
            var author = context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return author;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Author author)
        {
            //Esto no es necesario en asp.net core 2.1 en adelante
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            */
            context.Authors.Add(author);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor",new { id = author.Id }, author );
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Author author)
        {
            // Esto no es necesario en asp.net core 2.1
            // if (ModelState.IsValid){

            // }

            if (id != author.Id)
            {
                return BadRequest();
            }

            context.Entry(author).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Author> Delete(int id)
        {
            var author = context.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            context.Authors.Remove(author);
            context.SaveChanges();
            return author;
        }
    }
}
