using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TrojanWebRebuild.Models
{
    public class TrojanContext : DbContext
    {
        public TrojanContext() : base("Trojan")
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<Virus_Item> Virus_Item { get; set; }
        public DbSet<Matrix_Cell> Matrix_Cell { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<SeverityRating> SeverityRating { get; set; }
        public DbSet<Virus> Virus { get; set; }
    }
}