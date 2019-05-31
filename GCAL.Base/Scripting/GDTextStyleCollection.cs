using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTextStyleCollection: IEnumerable
    {
        public GDNode Parent { get; set; }

        private List<GDTextStyle> p_list = null;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return p_list.GetEnumerator();
        }

        public GDTextStyle AddStyle(GDTextStyle node)
        {
            SafeList();

            p_list.Add(node);

            return node;
        }

        public bool ContainsStyle(GDStyleKey key)
        {
            foreach (GDTextStyle style in p_list)
            {
                if (style.Key == key)
                    return true;
            }
            return false;
        }

        public object GetValue(GDStyleKey key)
        {
            foreach (GDTextStyle style in p_list)
            {
                if (style.Key == key)
                    return style.Value;
            }

            return null;
        }

        public GDTextStyle SetStyle(GDStyleKey prop, object value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p_list[i].Key == prop)
                {
                    p_list[i].Value = value;
                    return p_list[i];
                }
            }

            GDTextStyle ns = new GDTextStyle();
            ns.Key = prop;
            ns.Value = value;
            return AddStyle(ns);
        }

        public void RemoveStyle(GDStyleKey prop)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p_list[i].Key == prop)
                {
                    p_list.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Remove(GDTextStyle node)
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



        public GDTextStyle this[int index]
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
                p_list = new List<GDTextStyle>();
                if (Parent != null)
                    Parent.Format = this;
            }
        }
    }
}
