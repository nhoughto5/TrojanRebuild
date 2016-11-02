using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrojanWebRebuild.Models
{
    public class AttackVector
    {
        public int covert { get; set; }
        public int a { get; set; }
        public int r { get; set; }
        public int t { get; set; }

        public AttackVector(int a_, int r_, int t_)
        {
            a = a_;
            r = r_;
            t = t_;
            covert = 0;
        }
        public AttackVector()
        {

        }
    }
}