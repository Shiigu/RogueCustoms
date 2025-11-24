using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class TreeViewHelpers
    {
        public static void SelectNodeByTag(this TreeView treeView, object tagToFind)
        {
            SelectNodeByTag(treeView.Nodes, tagToFind, treeView);
        }

        private static void SelectNodeByTag(TreeNodeCollection nodes, object tagToFind, TreeView treeView)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag?.Equals(tagToFind) == true)
                {
                    treeView.SelectedNode = node;
                    return;
                }

                if (node.Nodes.Count > 0)
                {
                    SelectNodeByTag(node.Nodes, tagToFind, treeView);
                }
            }
        }
    }
}
