using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDNodeCollection: IEnumerable
    {
        public GDNode Parent { get; set; }

        private List<GDNode> p_list = null;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return p_list.GetEnumerator();
        }

        public GDNode AddNode(GDNode node)
        {
            SafeList();

            p_list.Add(node);
            node.ParentNode = Parent;

            return node;
        }

        public void InsertNode(int index, GDNode node)
        {
            SafeList();

            p_list.Insert(index, node);
        }

        public void Remove(GDNode node)
        {
            if (p_list == null)
                return;

            p_list.Remove(node);
        }

        public void RemoveAt(int index)
        {
            if (p_list == null)
                return;

            p_list.RemoveAt(index);
        }

        public GDNode this[int index]
        {
            get
            {
                if (p_list == null)
                    return null;
                return p_list[index];
            }
            set
            {
                SafeList();
                p_list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                if (p_list == null)
                    return 0;
                return p_list.Count;
            }
        }

        private void SafeList()
        {
            if (p_list == null)
            {
                p_list = new List<GDNode>();
                if (Parent != null)
                    Parent.Nodes = this;
            }
        }
    }
}
