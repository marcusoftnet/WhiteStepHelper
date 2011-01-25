using System.Collections.Generic;
using White.Core.AutomationElementSearch;
using White.Core.UIItems.TreeItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    ///Extension methods for the White application class
    /// </summary>
    public static class WhiteTreeExtensions
    {
        /// <summary>
        /// Returns the node with the given text in the tree
        /// </summary>
        /// <param name="instance">the the tree to get node from</param>
        /// <param name="nodeText">the text of the node</param>
        /// <returns>the node with the given text in the tree</returns>
        /// <remarks></remarks>
        public static TreeNode GetNodeByName(this Tree instance, string nodeText)
        {
            var node = FindTreeNodeByText(instance.Nodes, nodeText);
            if(node == null)
            {
                var mess = string.Format("Could not find node with text '{1}' in tree '{0}'", instance.AutomationElement.Current.Name, nodeText);
                throw new AutomationElementSearchException(mess);
            }
            return node;
        }

        /// <summary>
        /// Returns the level for the node of the tree the node is present
        /// </summary>
        /// <param name="instance">the tree to search</param>
        /// <param name="nodeText">the text of the node</param>
        /// <returns>the level where the node is present</returns>
        /// <remarks></remarks>
        public static List<TreeNode> GetPathToTreeNode(this Tree instance, string nodeText)
        {
            var node = instance.GetNodeByName(nodeText);

            return instance.GetPathTo(node);
        }

        /// <summary>
        /// Returns the level for the node of the tree the node is present
        /// </summary>
        /// <param name="instance">the tree to search</param>
        /// <param name="nodeText">the text of the node</param>
        /// <returns>the level where the node is present</returns>
        /// <remarks></remarks>
        public static int GetTreeNodeLevel(this Tree instance, string nodeText)
        {
            return instance.GetPathToTreeNode(nodeText).Count;
        }


        /// <summary>
        /// Searches the tree recursive and looks for the node with the given name
        /// </summary>
        /// <param name="nodesToSearch">the nodes to search</param>
        /// <param name="nodeText">the text on the node we're looking for</param>
        /// <returns></returns>
        private static TreeNode FindTreeNodeByText(TreeNodes nodesToSearch, string nodeText)
        {
            TreeNode nodeToReturn = null;

            foreach (var node in nodesToSearch)
            {
                if (node.Text == nodeText)
                {
                    nodeToReturn = node;
                    break;
                }

                if ((node.Nodes.Count > 0))
                {
                    nodeToReturn = FindTreeNodeByText(node.Nodes, nodeText);
                }
            }

            return nodeToReturn;
        }
    }
}
