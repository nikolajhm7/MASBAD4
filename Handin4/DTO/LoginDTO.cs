﻿using System.ComponentModel.DataAnnotations;

namespace Handin4.DTO
{
    public class LoginDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
