using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunTree<T>
    {
        public class TreeNode<T>
        {
            public TreeNode(TreeNode<T> prnt, T data)
            {
                Parent = prnt;
                Data = data;
            }

            public T Data { get; set; }
            public TreeNode<T> Parent { get; set; }
            public List<TreeNode<T>> Children { get; set; }
            public int GetHeight()
            {
                int height = 1;
                TreeNode<T> current = this;
                while (current.Parent != null)
                {
                    height++;
                    current = current.Parent;
                }
                return height;
            }
        }

        public TreeNode<T> Root { get; set; }

        public SunTree(T dt)
        {
            Root = new TreeNode<T>(null, dt);
        }


    }
}
