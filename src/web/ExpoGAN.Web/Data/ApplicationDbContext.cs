using Microsoft.EntityFrameworkCore;
using ExpoGAN.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpoGAN.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ArtPiece> ArtPieces { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
