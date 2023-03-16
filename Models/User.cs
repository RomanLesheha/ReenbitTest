using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReenbitTest.Models
{
    public class User
    {
        public string UserEmail { get; set; }
        public IFormFile File { get; set; }
    }
}
