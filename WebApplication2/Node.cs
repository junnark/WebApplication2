using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class Node
    {
        public int? ParentId { get; set; }
        public int? NodeId { get; set; }
        public string NodeName { get; set; }
        public int HierarchyLevel { get; set; }
    }
}
