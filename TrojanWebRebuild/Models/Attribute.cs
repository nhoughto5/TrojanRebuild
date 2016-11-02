using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TrojanWebRebuild.Models
{
    public class Attribute
    {
        [ScaffoldColumn(false)]
        public int AttributeId { get; set; }

        [Required, StringLength(100), Display(Name = "Name")]
        public string AttributeName { get; set; }

        [Required, StringLength(10000), Display(Name = "Attribute Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImagePath { get; set; }
        public int F_in { get; set; }
        public int F_out { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}