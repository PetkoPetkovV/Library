using Library.Data.Models;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllAsync();

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task AddBookAsync(AddBookViewModel model);

        Task AddBookToCollectionAsync(int bookId, string userId);

        Task <IEnumerable<BookViewModel>> GetMineAsync(string userId);

        Task RemoveBookFromCollectionAsync(int bookId, string userId);
    }
}
