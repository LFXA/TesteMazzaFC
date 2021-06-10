using System;
using System.ComponentModel.DataAnnotations;

namespace TesteMazzaAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
