using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static BookManagement.Core.APIGenerateAuthHeader;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BookManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Get Book Detail
        [HttpGet]
        public ActionResult GetBookDetail(int bookId)
        {
            BookDetails BookDetail = new BookDetails();
            using (var client = new HttpClient())
            {
                string URL = Utility.Utility.GetAppSettings("APIURL");
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestUri = "api/Book/GetBookDetail?bookId=" + bookId;
                NewAPIGenerateAuthHeader.NewGenerateAuthHeader(client, requestUri, "GET");
                var response = client.GetAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    BookDetail = JsonConvert.DeserializeObject<BookDetails>(jsonString.Result);
                }
            }

            return Json(new { BookDetail });
        }
        #endregion

        #region Delete Book Detail
        [HttpDelete]
        public ActionResult DeleteBookDetail(int bookId)
        {
            bool isDeleted = false;
            using (var client = new HttpClient())
            {
                string URL = Utility.Utility.GetAppSettings("APIURL");
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestUri = "api/Book/DeleteBookDetail?bookId=" + bookId;
                NewAPIGenerateAuthHeader.NewGenerateAuthHeader(client, requestUri, "DELETE");
                var response = client.DeleteAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    isDeleted = JsonConvert.DeserializeObject<bool>(jsonString.Result);
                }
            }

            return Json(isDeleted);
        }
        #endregion

        #region Create Book - Get
        [HttpGet]
        public ActionResult CreateBook(int bookId)
        {
            BookDetails BookDetail = new BookDetails();

            if (bookId > 0)
            {
                using (var client = new HttpClient())
                {
                    string URL = Utility.Utility.GetAppSettings("APIURL");
                    client.BaseAddress = new Uri(URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string requestUri = "api/Book/GetBookDetail?bookId=" + bookId;
                    NewAPIGenerateAuthHeader.NewGenerateAuthHeader(client, requestUri, "GET");
                    var response = client.GetAsync(requestUri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        BookDetail = JsonConvert.DeserializeObject<BookDetails>(jsonString.Result);
                    }
                }
            }
            return View(BookDetail);
        }
        #endregion

        #region Create Book - Post
        [HttpPost]
        public ActionResult CreateBook(BookDetails BookDetails)
        {
            bool isCreated = false;
            using (var client = new HttpClient())
            {
                string URL = Utility.Utility.GetAppSettings("APIURL");
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestUri = "api/Book/CreateBook";
                NewAPIGenerateAuthHeader.NewGenerateAuthHeader(client, requestUri, "POST");
                var response = client.PostAsJsonAsync(requestUri, BookDetails).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    isCreated = JsonConvert.DeserializeObject<bool>(jsonString.Result);
                }
            }

            return Json(isCreated);
        }
        #endregion

        #region Get Book List
        [HttpGet]
        public ActionResult BookList()
        {
            List<BookDetails> BookDetailsList = new List<BookDetails>();
            using (var client = new HttpClient())
            {
                string URL = Utility.Utility.GetAppSettings("APIURL");
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestUri = "api/Book/GetBookList";
                NewAPIGenerateAuthHeader.NewGenerateAuthHeader(client, requestUri, "GET");
                var response = client.GetAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    BookDetailsList = JsonConvert.DeserializeObject<List<BookDetails>>(jsonString.Result);
                }
            }

            return View(BookDetailsList);
        }
        #endregion
    }
}
