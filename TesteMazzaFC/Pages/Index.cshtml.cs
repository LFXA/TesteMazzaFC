using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TesteMazzaFC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string txtEmail { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string txtPassword { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Produtos");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44326/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, String>("Email", txtEmail),
                       new KeyValuePair<string, String>("Password", txtPassword)

                   });
                    var body = JsonConvert.SerializeObject(new { Email = txtEmail, Password = txtPassword });

                    var result = await client.PostAsync("/api/auth/login", formContent);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(response);
                        var token = jObject.GetValue("token").ToString();
                        
                        HttpContext.Session.Set("token", Encoding.ASCII.GetBytes(token));

                        _logger.LogInformation("User logado.");
                        return LocalRedirect(returnUrl);
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Login Inválido.");
                        return Page();
                    }
                }

                // If we got this far, something failed, redisplay form
            } 
             return Page();
        } 
    }        
}
