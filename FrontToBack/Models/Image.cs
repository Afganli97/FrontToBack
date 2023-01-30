using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FrontToBack.Models
{
	public class Image
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public bool IsMain { get; set; }
		public int ProductId { get; set; }

		[NotMapped]
        [Required]
        public IFormFile Photo { get; set; }

	}
}

