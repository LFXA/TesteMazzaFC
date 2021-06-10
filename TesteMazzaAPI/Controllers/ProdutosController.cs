using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteMazzaAPI.Data;
using TesteMazzaAPI.Models;

namespace TesteMazzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly TesteMazzaContext _context;
        public ProdutosController(TesteMazzaContext context)
        {
            _context = context;

        }
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _context.Produtos.ToList();
            return Ok(new { produtos });
        }
        [HttpPost]
        public IActionResult Create(Produto produto)
        {
            var selectProduto = _context.Produtos.Where(p => p.Nome == produto.Nome).FirstOrDefault();
            if (selectProduto != null)
                return Unauthorized();

            _context.Produtos.Add(produto);
            var result = _context.SaveChanges();
            return Ok("Produto Criado");
        }
    }
}
