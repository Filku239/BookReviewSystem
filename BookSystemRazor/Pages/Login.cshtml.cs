using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    private readonly HttpClient _httpClient;

    public LoginModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BookApi");
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var loginData = new
        {
            Email = Input.Email,
            HashedPassword = Input.Password
        };

        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("User/login", content);

        if (response.IsSuccessStatusCode)
        {
            TempData["username"] = Input.Email;
            return RedirectToPage("/Main");
        }
        else
        {
            ErrorMessage = "Invalid email or password.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }
    }
}
