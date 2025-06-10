using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookSystemRazor.Models;

namespace BookSystemRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public int bookCount { get; set; }

        public int genreCount { get; set; }
        public int userCount { get; set; }
        public int opinionCount { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BookApi");
            try
            {
                var bookResponse = await client.GetAsync("book/count");

                if (bookResponse.IsSuccessStatusCode)
                {
                    bookCount = await bookResponse.Content.ReadFromJsonAsync<int>();
                }

                var genreResponse = await client.GetAsync("genre/count");

                if (genreResponse.IsSuccessStatusCode)
                {
                    genreCount = await genreResponse.Content.ReadFromJsonAsync<int>();
                }

                var userResponse = await client.GetAsync("user/count");

                if (userResponse.IsSuccessStatusCode)
                {
                    userCount = await userResponse.Content.ReadFromJsonAsync<int>();
                }

                var opinionResponse = await client.GetAsync("opinion/count");

                if (opinionResponse.IsSuccessStatusCode)
                {
                    opinionCount = await opinionResponse.Content.ReadFromJsonAsync<int>();
                }
            }
            catch (Exception ex)
            {
                bookCount = 0;
            }

        }
    }
}