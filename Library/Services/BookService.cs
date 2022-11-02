using Library.Contracts;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task AddBookAsync(AddBookViewModel model)
        {
            var book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                Description =  model.Description
            };

            await context.Books.AddAsync(book);

            await context.SaveChangesAsync();
        }

        public async Task AddBookToCollectionAsync(int bookId, string userId)
        {
            var appUser = await context.Users
                 .Where(u => u.Id == userId)
                 .Include(u => u.ApplicationUsersBooks)
                 .FirstOrDefaultAsync();

            if (appUser == null)
            {
                throw new ArgumentException("Invalid User ID");
            }

            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                throw new ArgumentException("Invalid book ID");
            }

            if (!appUser.ApplicationUsersBooks.Any(x => x.BookId == bookId))
            {
                appUser.ApplicationUsersBooks.Add(new ApplicationUserBook()
                {
                    ApplicationUserId = appUser.Id,
                    BookId = book.Id,
                    ApplicationUser = appUser,
                    Book = book
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookViewModel>> GetAllAsync()
        {
            var entities = await context.Books
               .Include(b => b.Category)
               .ToListAsync();
            return entities
                 .Select(b => new BookViewModel()
                 {
                     Author = b.Author,
                     Category = b.Category.Name,
                     Id = b.Id,
                     Rating = b.Rating,
                     Title = b.Title,
                     ImageUrl = b.ImageUrl,
                 });
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<BookViewModel>> GetMineAsync(string userId)
        {
            var user = await context.Users
                 .Where(u => u.Id == userId)
                 .Include(u => u.ApplicationUsersBooks)
                 .ThenInclude(u => u.Book)
                 .ThenInclude(u => u.Category)
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid User ID");
            }

            return user.ApplicationUsersBooks.Select(b => new BookViewModel()
            {
                Id = b.BookId,
                Title = b.Book.Title,
                ImageUrl = b.Book.ImageUrl,
                Description = b.Book.Description,
                Author = b.Book.Author,
                Category = b.Book.Category.Name,
                Rating = b.Book.Rating
            });
        }

        public async Task RemoveBookFromCollectionAsync(int bookId, string userId)
        {
            var appUser = await context.Users
              .Where(u => u.Id == userId)
              .Include(u => u.ApplicationUsersBooks)
              .FirstOrDefaultAsync();

            if (appUser == null)
            {
                throw new ArgumentException("Invalid User ID");
            }

            var book = appUser.ApplicationUsersBooks.FirstOrDefault(x => x.BookId == bookId);

            if (book != null)
            {
                appUser.ApplicationUsersBooks.Remove(book);

                await context.SaveChangesAsync();
            }
        }
    }
}
