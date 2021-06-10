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

namespace TesteMazzaFC.Pages
{
    public class CriaProdutoModel : PageModel
    {
        [BindProperty]
        [Required]
        public String txtTitulo { get; set; }
       
        [BindProperty]
        public String[] selecionado { get; set; }

        public SelectList Categorias { get; set; }
        [BindProperty]
        [Required]
        public Decimal txtPreco { get; set; }

        public ISession Session { get; set; }
        public CriaProdutoModel(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext.Session;
        }

        public void OnGet()
        {
            Categorias = new SelectList(new string[] { "Alimento", "Bebida", "Outros" });
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

                    var body = JsonConvert.SerializeObject(new { Nome = txtTitulo, Preco = txtPreco, Categoria = selecionado[0] });
                    var result = await client.PostAsync("/api/produtos", new StringContent(body, Encoding.UTF8, "application/json"));
                    if (result.IsSuccessStatusCode)
                    {
                        await result.Content.ReadAsStringAsync();

                        return Page();
                    }
                    ModelState.AddModelError(string.Empty, "Produto não criado");
                }

            }
            return Page();
        }
    }
}
