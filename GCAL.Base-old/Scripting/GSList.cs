using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    /// <summary>
    /// This is list of elements in LISP program. Therefore LISP is
    /// called programing language for lists.
    /// </summary>
    public class GSList: GSCore
    {
        public GSCoreCollection Parts = new GSCoreCollection();

        public GSList Parent = null;

        public GSList()
        {
        }

        public GSList(params GSCore[] list)
        {
            Parts.AddRange(list);
        }

        public GSList(IEnumerable<GSCore> list)
        {
            Parts.AddRange(list);
        }

        public void Add(object obj)
        {
            if (obj is int)
            {
                Add(new GSNumber((int)obj));
            }
            else if (obj is double)
            {
                Add(new GSNumber((double)obj));
            }
            else if (obj is long)
            {
                Add(new GSNumber((long)obj));
            }
            else
            {
                Add(new GSString(obj.ToString()));
            }
        }

        public void Add(GSCore obj)
        {
            Parts.Add(obj);
        }

        public int Count
        {
            get
            {
                return Parts.Count;
            }
        }

        public GSCore this[int index]
        {
            get
            {
                return Parts[index];
            }
        }

        public GSList createAndAddSublist()
        {
            GSList list = new GSList() { Parent = this };
            Parts.Add(list);
            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            bool firstSpace = false;
            foreach (GSCore item in Parts)
            {
                if (firstSpace)
                    sb.Append(" ");
                sb.Append(item.ToString());
                firstSpace = true;
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch(s)
            {
                case "count":
                    return new GSNumber(Parts.Count);
                default:
                    return base.GetPropertyValue(s);
            }
        }

        public override void writeScript(int level, System.IO.StreamWriter sw)
        {
            sw.WriteLine();
            sw.Write(string.Empty.PadLeft(level));
            sw.Write("(");
            bool b = false;
            foreach (GSCore item in Parts)
            {
                if (b)
                    sw.Write(" ");
                item.writeScript(level + 1, sw);
                b = true;
            }
            sw.Write(")");
        }

        /// <summary>
        /// Reading text with script
        /// Lines starting with # are comments
        /// Other lines are program
        /// </summary>
        /// <param name="text"></param>
        public void readList(String text)
        {
            Parts.Clear();
            Parent = this;

            StringReader sr = new StringReader(text);
            String line = null;
            GSList currentList = this;

            while ((line = sr.ReadLine()) != null)
            {
                readTextListLine(line, ref currentList);
            }
        }

        /// <summary>
        /// reading one line
        /// </summary>
        /// <param name="line"></param>
        /// <param name="currentList"></param>
        public void readTextListLine(string line, ref GSList currentList)
        {
            // if line starts with #, then 
            if (line.TrimStart().StartsWith("#"))
                return;

            int mode = 0;
            StringBuilder part = new StringBuilder();
            foreach (char C in line)
            {
                if (mode == 0)
                {
                    if (char.IsWhiteSpace(C))
                    {
                    }
                    else if (C == '\'')
                    {
                        mode = 1;
                    }
                    else if (C == '\"')
                    {
                        mode = 3;
                    }
                    else if (C == '(')
                    {
                        currentList = currentList.createAndAddSublist();
                    }
                    else if (C == ')')
                    {
                        currentList = currentList.Parent;
                    }
                    else
                    {
                        part.Append(C);
                        mode = 2;
                    }
                }
                else if (mode == 1)
                {
                    if (char.IsWhiteSpace(C) || C == ')')
                    {
                        currentList.Add(new GSString() { Value = part.ToString() });
                        part.Clear();
                        mode = 0;
                        if (C == ')')
                            currentList = currentList.Parent;
                    }
                    else
                    {
                        part.Append(C);
                    }
                }
                else if (mode == 2)
                {
                    if (char.IsWhiteSpace(C) || C == ')')
                    {
                        double d;
                        int i;
                        string value = part.ToString();
                        if (double.TryParse(value, out d))
                        {
                            currentList.Add(new GSNumber() { DoubleValue = d });
                        }
                        else if (int.TryParse(value, out i))
                        {
                            currentList.Add(new GSNumber() { IntegerValue = i });
                        }
                        else
                        {
                            currentList.Add(new GSToken() { Token = part.ToString() });
                        }
                        part.Clear();
                        mode = 0;
                        if (C == ')')
                            currentList = currentList.Parent;
                    }
                    else
                    {
                        part.Append(C);
                    }
                }
                else if (mode == 3)
                {
                    if (C == '\"')
                    {
                        currentList.Add(new GSString(part.ToString()));
                        part.Clear();
                        mode = 0;
                    }
                    else if (C == '\\')
                    {
                        mode = 4;
                    }
                    else
                    {
                        part.Append(C);
                    }
                }
                else if (mode == 4)
                {
                    part.Append(C);
                    mode = 3;
                }
            }

            if (part.Length > 0)
            {
                if (mode == 1)
                {
                    currentList.Add(new GSString() { Value = part.ToString() });
                }
                else
                {
                    double d;
                    int i;
                    string value = part.ToString();
                    if (double.TryParse(value, out d))
                    {
                        currentList.Add(new GSNumber() { DoubleValue = d });
                    }
                    else if (int.TryParse(value, out i))
                    {
                        currentList.Add(new GSNumber() { IntegerValue = i });
                    }
                    else
                    {
                        currentList.Add(new GSToken() { Token = part.ToString() });
                    }
                }
            }
        }

    }
}
