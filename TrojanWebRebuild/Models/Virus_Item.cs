using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TrojanWebRebuild.Models
{
    public class Virus_Item
    {
        [Key]
        public string ItemId { get; set; }

        public string VirusId { get; set; }

        public bool On_Off { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int AttributeId { get; set; }

        public virtual Attribute Attribute { get; set; }

        public virtual Category Category { get; set; }

        public bool userAdded { get; set; }

        public bool Saved { get; set; }

        public Virus_Item(string V_Id, int A_Id, Attribute Attr, Category Category_, bool user)
        {
            ItemId = Guid.NewGuid().ToString();
            VirusId = V_Id;
            On_Off = true;
            DateCreated = DateTime.Now;
            AttributeId = A_Id;
            Attribute = Attr;
            Category = Category_;
            userAdded = user;
            Saved = false;
        }

        public Virus_Item()
        {
            Saved = false;
        }

        public virtual Virus Virus { get; set; }
    }
}