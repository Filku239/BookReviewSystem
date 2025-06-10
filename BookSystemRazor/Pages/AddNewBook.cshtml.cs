using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookSystemRazor.Models;
using System.Text.Json;
using System.Net.Http;

namespace BookSystemRazor.Pages
{
    public class AddNewBookModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Book Book { get; set; }

        public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public AddNewBookModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BookApi");
        }

        public async Task OnGetAsync()
        {
            Book = new Book
            {
                Title = "",
                Author = "",
                Description = "",
                Price = 0,
                ReleaseYear = 2025
            };

            var response = await _httpClient.GetAsync("Genre");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiGenres = JsonSerializer.Deserialize<List<Genre>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiGenres != null)
                {
                    Genres = apiGenres.Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToList();
                }
            }
            else
            {
                Console.WriteLine($"Error fetching genres: {response.StatusCode}");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("Book", Book);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error adding book: {response.StatusCode}");
            }

            return RedirectToPage("./Main");
        }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}