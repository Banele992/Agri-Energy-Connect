using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Agri_Energy_Connect.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string? Name { get; set; }        

        public string? EmailAddress { get; set; }

        public string? Address { get; set; }
    }
}
