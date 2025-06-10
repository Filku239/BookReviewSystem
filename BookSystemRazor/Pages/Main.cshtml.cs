using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSystemRazor.Models;

namespace BookSystemRazor.Pages
{
    public class MainModel : PageModel
    {
        private readonly ILogger<MainModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Book> Books { get; set; } = new();

        public MainModel(ILogger<MainModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BookApi");
            var response = await client.GetAsync("book");

            if (response.IsSuccessStatusCode)
            {
                var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                if (books != null)
                {
                    Books = books;
                }
            }
        }

    }
}
