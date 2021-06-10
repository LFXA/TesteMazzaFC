using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesteMazzaAPI.Models;

namespace TesteMazzaFC.Pages.Shared
{
    public class ProdutosModel : PageModel
    {
        public String Nome { get; set; }
        public String Categoria { get; set; }
        public String Preco { get; set; }
        public List<Produto> produtos = new List<Produto>();
        public ISession Session { get; set; }

        public ProdutosModel(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext.Session;

        }

        public async Task OnGetAsync()
        {
            produtos = await GetProdutos();
        }

        private async Task<List<Produto>> GetProdutos()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44326/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var token = Session.GetString("token");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



                var result = await client.GetAsync("/api/produtos");
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(response);
                    var produtosJson = jObject.GetValue("produtos").ToString();

                    produtos = JsonConvert.DeserializeObject<List<Produto>>(produtosJson);

                    return produtos;
                }


            }
            return new List<Produto>();
        }
        public async void OnGetDelete(int prodId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44326/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var token = Session.GetString("token");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.DeleteAsync($"/api/produtos/{prodId}");
            }
        }
        public IActionResult OnGetAtualiza(int id, string nome, string categoria, string preco)
        {
            var produto = new Produto
            {
                Id = id,
                Categoria = categoria,
                Nome = nome,
                Preco = Convert.ToDecimal(preco)
            };
           return RedirectToPage("/AtualizaProduto","OnGet", produto);
        }
    }
}
