using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace TrojanWebRebuild.Models
{
    public class Connection
    {
        [Key]
        public int ConnectionId { get; set; }
        public int source { get; set; }
        public int target { get; set; }
        public bool direct { get; set; }
        public string VirusId { get; set; }
        public Connection(int source_, int destination_, bool direct_, string virusId)
        {
            source = source_;
            target = destination_;
            direct = direct_;
            VirusId = virusId;
        }
        public Connection()
        {

        }
    }
}