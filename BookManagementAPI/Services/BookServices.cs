using BookmanagementAPI.Core.IServices;
using BookmanagementAPI.Core.IRepository;
using BookmanagementAPI.Core.Model;

namespace BookmanagementAPI.Services
{
    public class BookServices : IBookService
    {
        #region Declaration
        private readonly IBookRepository BookRepository;
        #endregion


        #region Constructor
        public BookServices(IBookRepository BookRepository) 
        { 
            this.BookRepository = BookRepository;
        }
        #endregion


        #region Get Book List 
        public List<BookDetails> GetBookList()
        {
           return  BookRepository.GetBookList();
        }
        #endregion

        #region Create Book
        public bool CreateBook(BookDetails bookDetails)
        {
            return BookRepository.CreateBook(bookDetails);
        }
        #endregion

        #region Get Book Detail
        public BookDetails GetBookDetail(int bookId)
        {
            return BookRepository.GetBookDetail(bookId);
        }
        #endregion

        #region Delete Book Detail
        public bool DeleteBookDetail(int bookId)
        {
            return BookRepository.DeleteBookDetail(bookId);
        }
        #endregion


    }
}
