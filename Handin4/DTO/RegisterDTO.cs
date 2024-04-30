﻿using System.ComponentModel.DataAnnotations;

namespace Handin4.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
