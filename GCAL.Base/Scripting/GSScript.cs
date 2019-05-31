using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    /// <summary>
    /// This is representation of LISP program in the memory
    /// Main separators are space, (, )
    /// tokens are writen without parenthesses
    /// string constants are writen with starting '
    /// number constants are writen without parenthesses and are automaticaly recognized
    /// example:
    /// 
    /// (add (mul 12.0 a) 25)
    /// 
    /// if token is on first position in the list, then it is regarded as command
    /// if token is within the list, it is regarded as variable or object
    /// </summary>
    public class GSScript: GSList
    {
        /// <summary>
        /// Reading text with text template
        /// Lines starting with ## are comments
        /// Lines starting with # are program
        /// Other lines are printed to output
        /// </summary>
        /// <param name="text"></param>
        public void readTextTemplate(String text)
        {
            Parts.Clear();
            Parent = this;

            StringReader sr = new StringReader(text);
            String line = null;
            GSList currentList = this;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("#"))
                {
                    readTextListLine(line.Substring(1), ref currentList);
                }
                else
                {
                    GSList list = currentList.createAndAddSublist();
                    list.Add(new GSToken(){ Token = "println" });
                    list.Add(new GSString(){ Value = line });
                }
            }
        }



        public void writeScript(StreamWriter f)
        {
            base.writeScript(0, f);
        }
    }
}
