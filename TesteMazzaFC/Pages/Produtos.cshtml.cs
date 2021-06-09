using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TesteMazzaFC.Pages.Shared
{
    public class ProdutosModel : PageModel
    {
        public List<ProdutosModel> produtos { get; set; }
        public void OnGet()
        {

        }
    }
}
