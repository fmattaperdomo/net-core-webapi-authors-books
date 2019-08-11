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
    [Route("api/libros")]
    [ApiController]
    public class BooksController:ControllerBase
    {
        private readonly AppDbContext context;

        public BooksController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            return context.Books.Include(x => x.Author).ToList();
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<Book> Get(int id)
        {
            var book = context.Books.Include(x => x.Author).FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            context.Entry(book).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Book> Delete(int id)
        {
            var book = context.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            context.SaveChanges();
            return book;
        }

    }
}
