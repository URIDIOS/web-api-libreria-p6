using CREAR_API.Data.Models;
using CREAR_API.Data.ViewModels;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Publisher = CREAR_API.Data.Models.Publisher;

namespace CREAR_API.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;
        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        //metodo para crear autor
        public Publisher AddPublisher(PublisherVM publisher)
        {
            var _publisher = new Models.Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherByID(int id) => _context.Publishers.FirstOrDefault(n => n.Id == id); 

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVM()
                    {
                        BookName = n.Titulo,
                        BookAuthors = n.Book_Author.Select(n => n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();
            return _publisherData;
        }

		internal void DeletePublisherById(int id)
		{
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);
            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
		}
	}
}
