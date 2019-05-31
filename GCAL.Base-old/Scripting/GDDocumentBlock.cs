using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    public class GDDocumentBlock: GDNode
    {
        public GDDocumentBlock AddBlock()
        {
            GDDocumentBlock block = new GDDocumentBlock();
            Nodes.AddNode(block);
            return block;
        }

        public GDDocumentBlock AddBlock(GDDocumentBlock block)
        {
            Nodes.AddNode(block);
            return block;
        }

        public GDTextRun Append(string text)
        {
            GDTextRun textRun = new GDTextRun();
            textRun.Text = text;
            Nodes.AddNode(textRun);
            return textRun;
        }

        public GDTextRun AppendFormat(string fmt, params object[] args)
        {
            return Append(string.Format(fmt, args));
        }

        public GDLineBreak AddLineBreak()
        {
            GDLineBreak lb = new GDLineBreak();
            Nodes.AddNode(lb);
            return lb;
        }

        public GDTable AddTable()
        {
            GDTable table = new GDTable();
            Nodes.AddNode(table);
            return table;
        }

    }
}
