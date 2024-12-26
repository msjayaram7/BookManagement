using BookmanagementAPI.Core.Model;

namespace BookmanagementAPI.Core.IRepository
{
    public interface IBookRepository
    {
        public List<BookDetails> GetBookList();
        public bool CreateBook(BookDetails BookDetails);
        public BookDetails GetBookDetail(int bookId);
        public bool DeleteBookDetail(int bookId);
    }
}
