using System.ComponentModel.DataAnnotations;

namespace Vega.Core.Models
{
   public class Photo
   {
      public int Id { get; set; }

      [Required]
      [StringLength(255)]
      public string FileName { get; set; }
   }
}