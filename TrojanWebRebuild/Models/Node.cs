using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrojanWebRebuild.Models
{
    public class Node
    {
        public int nodeID;
        public string nodeName;
        public string Category;
        public string Description;
        public int F_in;
        public int F_out;
        public Node(int ID, string name, string Cat, int Fin, int Fout, string desc)
        {
            nodeID = ID;
            Category = Cat;
            F_in = Fin;
            F_out = Fout;
            nodeName = name;
            Description = desc;
        }
        public Node()
        {

        }
    }
}