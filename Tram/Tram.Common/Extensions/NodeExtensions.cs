using System.Collections.Generic;
using System.Linq;
using Tram.Common.Models;

namespace Tram.Common.Extensions
{
    public static class NodeExtensions
    {
        public static float GetDistanceToChild(this Node node, Node child)
        {
            if (node.Child != null)
            {
                return node.Child.Distance;
            }
            else
            {
                return node.Children.Single(n => n.Node.Equals(child)).Distance;
            }
        }

        public static List<Node.Next> GetAllNextNodes(this Node node)
        {
            if (node.Child != null)
            {
                return new List<Node.Next>() { node.Child };
            }
            else if (node.Children != null && node.Children.Any())
            {
                return node.Children;
            }

            return null;
        }
    }
}
