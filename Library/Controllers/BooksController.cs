using Library.Contracts;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController(IBookService _bookService)
        {
            bookService = _bookService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await bookService.GetAllAsync();

            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel()
            {
                Categories = await bookService.GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.AddBookAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        public async Task<IActionResult> AddToCollection(int bookId)
        {
            try
            {
                var appUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                await bookService.AddBookToCollectionAsync(bookId, appUserId);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Mine()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var model = await bookService.GetMineAsync(userId);

            return View("Mine", model);
        }

        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            await bookService.RemoveBookFromCollectionAsync(bookId, userId);

            return RedirectToAction(nameof(Mine));
        }
    }
}
