using Microsoft.Data.SqlClient;
using BookmanagementAPI.Core.IRepository;
using BookmanagementAPI.Core.Model;
using BookManagementAPI.Entity;
using System.Collections.Generic;
using System.Data;

namespace BookmanagementAPI.Repository
{

    public class BookRepository : IBookRepository
    {
        #region Declaration
        private IDbConnection _connection => new SqlConnection(_config.GetConnectionString("DbConnection"));
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public BookRepository(IConfiguration config)
        {
            this._config = config;
        }
        #endregion

        #region Get Book List
        public List<BookDetails> GetBookList()
        {
            List<BookDetails> BooksDetailsList = new List<BookDetails>();
            using (var _dbContext = new BookManagementContext(_config))
            {
                var _BooksDetailsList = _dbContext.BookDetails.Where(x => !x.IsDeleted).ToList();
                if (_BooksDetailsList != null && _BooksDetailsList.Any())
                {
                    foreach (var item in _BooksDetailsList)
                    {
                        BookDetails Book = new BookDetails();

                        Book.BookId = item.BookId;
                        Book.Title = !string.IsNullOrWhiteSpace(item.Title) ? item.Title : "-";
                        Book.Author = !string.IsNullOrWhiteSpace(item.Author) ? item.Author : "-";
                        Book.Genre = !string.IsNullOrWhiteSpace(item.Genre) ? item.Genre : "-";
                        Book.PublishedYear = !string.IsNullOrWhiteSpace(item.PublishedYear) ? item.PublishedYear : "-";
                        Book.Price = item.Price > 0 ? item.Price : 0;
                        Book.Discount = !string.IsNullOrWhiteSpace(item.DiscountPercentage) ? item.DiscountPercentage : "-";
                        Book.FinalPrice = item.FinalPrice;
                        Book.Ratings = !string.IsNullOrWhiteSpace(item.Ratings) ? item.Ratings : "-";
                        BooksDetailsList.Add(Book);
                    }
                }
            }
            return BooksDetailsList;
        }
        #endregion

        #region Create Book
        public  bool CreateBook(BookDetails BookDetails)
        {
            bool isCreated = false;
            BookDetail dbBookDetail = null;
            if (BookDetails != null)
            {
                using (var _dbContext = new BookManagementContext(_config))
                {
                    bool IsRecordExists = false;
                    if (BookDetails.BookId > 0)
                    {
                        dbBookDetail =  _dbContext.BookDetails.SingleOrDefault(f => f.BookId == BookDetails.BookId && !f.IsDeleted);
                    }
                    if (dbBookDetail != null && dbBookDetail.BookId > 0)
                    {
                        IsRecordExists = true;
                    }
                    else
                    {
                        dbBookDetail = new BookDetail();
                    }
                    dbBookDetail.Title = !string.IsNullOrWhiteSpace(BookDetails.Title) ? BookDetails.Title : "-";
                    dbBookDetail.Author = BookDetails.Author;
                    dbBookDetail.Genre = BookDetails.Genre;
                    dbBookDetail.PublishedYear = BookDetails.PublishedYear;
                    dbBookDetail.Price = BookDetails.Price;
                    dbBookDetail.DiscountPercentage = BookDetails.Discount;
                    dbBookDetail.FinalPrice = BookDetails.FinalPrice;
                    dbBookDetail.Ratings = BookDetails.Ratings;
                    dbBookDetail.IsDeleted = false;
                    if (!IsRecordExists)
                    {
                        dbBookDetail.CreatedTimeStamp = DateTime.Now;
                        dbBookDetail.UpdatedTimeStamp = DateTime.Now;
                        _dbContext.BookDetails.Add(dbBookDetail);
                    }
                    else
                    {
                        dbBookDetail.UpdatedTimeStamp = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                    isCreated = true;
                }
            }
            return isCreated;
        }
        #endregion

        #region Get Book Detail
        public BookDetails GetBookDetail(int bookId)
        {
            BookDetails BookDetails = new BookDetails();

            using (var _dbContext = new BookManagementContext(_config))
            {
                var dbBookDetail = (from s in _dbContext.BookDetails
                                       where s.BookId == bookId && !s.IsDeleted
                                       select s).SingleOrDefault();

                if (dbBookDetail != null && dbBookDetail.BookId > 0)
                {
                    BookDetails.BookId = dbBookDetail.BookId;
                    BookDetails.Title = dbBookDetail.Title;
                    BookDetails.Author = dbBookDetail.Author;
                    BookDetails.Genre = dbBookDetail.Genre ;
                    BookDetails.PublishedYear = dbBookDetail.PublishedYear ;
                    BookDetails.Price = dbBookDetail.Price ;
                    BookDetails.Discount = dbBookDetail.DiscountPercentage;
                    BookDetails.FinalPrice = dbBookDetail.FinalPrice;
                    BookDetails.Ratings = dbBookDetail.Ratings ;
                }
            }
            return BookDetails;
        }
        #endregion

        #region Delete Book Detail
        public bool DeleteBookDetail(int bookId)
        {
            bool isDeleted = false;
            using (var _dbContext = new BookManagementContext(_config))
            {
                var dbBookDetail = (from s in _dbContext.BookDetails
                                       where s.BookId == bookId && !s.IsDeleted
                                       select s).SingleOrDefault();
                if (dbBookDetail != null && dbBookDetail.BookId > 0)
                {
                    dbBookDetail.IsDeleted = true;
                    dbBookDetail.UpdatedTimeStamp = DateTime.Now;
                    _dbContext.SaveChanges();
                    isDeleted = true;
                }
            }
            return isDeleted;
        }
        #endregion

    }
}
