using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrojanWebRebuild.Models
{
    public class SeverityRating
    {
        public int id { get; set; }
        public string VirusId { get; set; }
        public bool Saved { get; set; }
        public bool coverage { get; set; }
        public string nickName { get; set; }
        public string userName { get; set; }

        public string iR { get; set; }
        public string iA { get; set; }
        public string iE { get; set; }
        public string iL { get; set; }
        public string iF { get; set; }
        public string iC { get; set; }
        public string iP { get; set; }
        public string iO { get; set; }

        public string cR { get; set; }
        public string cA { get; set; }
        public string cE { get; set; }
        public string cL { get; set; }
        public string cF { get; set; }
        public string cC { get; set; }
        public string cP { get; set; }
        public string cO { get; set; }

        public SeverityRating()
        {
            Saved = false;
        }
    }
}