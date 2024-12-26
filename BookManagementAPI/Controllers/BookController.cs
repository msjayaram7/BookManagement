using Microsoft.AspNetCore.Mvc;
using BookmanagementAPI.Core.IServices;
using BookmanagementAPI.Core.Model;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        #region Declaration
        private readonly ILogger<BookController> _logger;
        private readonly IBookService BookService;
        #endregion

        #region Constructor
        public BookController(ILogger<BookController> logger, IBookService BookService)
        {
            _logger = logger;
            this.BookService = BookService;
        }
        #endregion

        #region Get Book List
        [HttpGet]
        [Route("GetBookList")]
        public List<BookDetails> GetBookList()
        {
            return BookService.GetBookList();
        }
        #endregion

        #region Create Book
        [HttpPost]
        [Route("CreateBook")]
        public bool CreateBook(BookDetails BookDetails)
        {
            return BookService.CreateBook(BookDetails);
        }
        #endregion

        #region Get Book Detail
        [HttpGet]
        [Route("GetBookDetail")]
        public BookDetails GetBookDetail(int bookId)
        {
            return BookService.GetBookDetail(bookId);
        }
        #endregion

        #region Delete Book Detail
        [HttpDelete]
        [Route("DeleteBookDetail")]
        public bool DeleteBookDetail(int bookId)
        {
            return BookService.DeleteBookDetail(bookId);
        }
        #endregion
    }
}
