﻿namespace Agri_Energy_Connect.Models;
using System.ComponentModel.DataAnnotations;

public class ContactForm
{
    [Required]
    public string? Name { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Message { get; set; }
}

