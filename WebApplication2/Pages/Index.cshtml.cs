using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        public List<Node> SortedNodeList { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            List<Node> nodes = this.GetData();
            this.BuildHierachy(nodes);   
        }

        private List<Node> GetData()
        {
            //"parent_id, node_id, node_name"
            string input = "null,0,grandpa|0,1,son|0,2,daugther|1,3,grandkid|1,4,grandkid|2,5,grandkid|5,6,greatgrandkid|3,7,grandgrandkid|7,8,grandgrandgrandkid";
            string[] splitStrings = input.Split("|".ToCharArray());
            List<Node> nodes = new List<Node>();

            foreach (var item in splitStrings)
            {
                // split per node
                List<string> nodeLine = item.Split(",").ToList();

                Node node = new Node();
                node.ParentId = nodeLine[0].ToString() == "null" || String.IsNullOrEmpty(nodeLine[0].ToString()) ? null : Convert.ToInt32(nodeLine[0]);
                node.NodeId = Convert.ToInt32(nodeLine[1].ToString());
                node.NodeName = nodeLine[2].ToString();

                nodes.Add(node);
            }

            return nodes;
        }

        private void BuildHierachy(List<Node> nodes)
        {
            List<Node> sortedNodes = nodes.OrderBy(n => n.ParentId).ToList();
            List<Node> sortedNodestemp = sortedNodes.Where(n => 1 == 1).ToList();
            List<Node> hierarchyNodes = new List<Node>();

            foreach (var item in sortedNodes)
            {
                if (sortedNodestemp.Contains(item))
                {
                    sortedNodestemp.Remove(item);
                    item.HierarchyLevel = 0;
                    hierarchyNodes.Add(item);

                    this.ProcessData(hierarchyNodes, sortedNodestemp, item);
                }
            }

            SortedNodeList = hierarchyNodes;

            List<string> hierarchyList = new List<string>();

            foreach (var item in hierarchyNodes)
            {
                hierarchyList.Add(item.ParentId + ", " + item.NodeId + ", " + item.NodeName + ", " + item.HierarchyLevel);
            }
        }

        private void ProcessData(List<Node> hierarchyNodes, List<Node> sortedNodestemp, Node item)
        {
            List<Node> temp = sortedNodestemp.Where(node => node.ParentId == item.NodeId).ToList();

            foreach (Node tempItem in temp)
            {
                tempItem.HierarchyLevel = item.HierarchyLevel + 1;
                hierarchyNodes.Add(tempItem);
                sortedNodestemp.Remove(tempItem);

                this.ProcessData(hierarchyNodes, sortedNodestemp, tempItem);
            }
        }
    }
}
