using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Models
{
    public class ArtPiece
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Filename { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public ArtPiece(Guid id, string filename, DateTime dateCreated)
        {
            Id = id;
            Filename = filename;
            DateCreated = dateCreated;
        }

        public ArtPiece(string unprocessedFilename)
        {
            Id = Guid.NewGuid();
            Filename = $"{Id.ToString().ToUpper()}-{unprocessedFilename}";
            DateCreated = DateTime.Now;
        }
    }
}
