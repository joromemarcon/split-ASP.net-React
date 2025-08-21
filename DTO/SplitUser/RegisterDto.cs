using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace split_api.DTO.SplitUser
{
    /*
        Request DTO
    */
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(5)]
        public string? Password { get; set; }
        [Required]
        public string? FullName { get; set; }
    }
}