using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TesteMazzaAPI.Models;

namespace TesteMazzaFC.Pages
{
    public class AtualizaProdutoModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [Required]
        public String Nome { get; set; }
       
        [BindProperty(SupportsGet = true)]
        public String[] Categoria { get; set; }

        public SelectList Categorias { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public String Preco { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public ISession Session { get; set; }
        public AtualizaProdutoModel(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext.Session;
        }

        public void OnGet(string preco)
        {          

        }

       
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44326/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var token = Session.GetString("token");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var newPreco = Preco.Replace('.', ',');
                    var body = JsonConvert.SerializeObject(new { Id = Id, Nome = Nome, Preco = Convert.ToDecimal(newPreco), Categoria = Categoria[0] });
                    var result = await client.PutAsync("/api/produtos", new StringContent(body, Encoding.UTF8, "application/json"));
                    if (result.IsSuccessStatusCode)
                    {
                        await result.Content.ReadAsStringAsync();

                        return LocalRedirect("~/Produtos");
                    }
                    ModelState.AddModelError(string.Empty, "Produto não criado");
                }

            }
            return Page();
        }
    }
}
