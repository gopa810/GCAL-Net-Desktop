using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GCAL.CalendarDataView
{
    public class CDVPara: CDVAtom
    {
        public class CDVLine: List<CDVAtom>
        {
            public int MaxHeight
            {
                get
                {
                    int max = 0;
                    foreach(CDVAtom atom in this)
                    {
                        if (atom is CDVRuler) continue;
                        max = Math.Max(max, atom.Height);
                    }
                    return max;
                }
            }
            public int Right
            {
                get
                {
                    int max = 0;
                    foreach (CDVAtom atom in this)
                    {
                        if (atom is CDVRuler) continue;
                        max = Math.Max(max, atom.Bounds.Right);
                    }
                    return max;
                }
            }

            public void ApplyAlignment(int maxWidth, CDVAlign align)
            {
                if (this.Count > 0)
                {
                    int remainingWidth = maxWidth - this[this.Count - 1].Bounds.Right;
                    int correction = (align == CDVAlign.Left ? 0 : align == CDVAlign.Center ? remainingWidth / 2 : remainingWidth);
                    for (int i = 0; i < this.Count; i++)
                    {
                        this[i].Location.Offset(correction, 0);
                    }
                }
            }

        }

        public CDVOrientation Orientation = CDVOrientation.Horizontal;
        public List<CDVAtom> Words = new List<CDVAtom>();
        public List<CDVLine> Lines = new List<CDVLine>();

        public CDVPara(CDVAtom owner): base(owner)
        {

        }

        public CDVPara(CDVAtom owner, params object[] args): base(owner)
        {
            foreach(object obj in args)
            {
                if (obj is CDVParaStyle)
                {
                    this.ParaStyle = (CDVParaStyle)obj;
                }
                else if (obj is CDVOrientation)
                {
                    this.Orientation = (CDVOrientation)obj;
                }
                else if (obj is CDVTextStyle)
                {
                    this.TextStyle = (CDVTextStyle)obj;
                }
                else if (obj is CDVSpan)
                {
                    this.SpanWidth = (CDVSpan)obj;
                }
                else if (obj is CDVAtom)
                {
                    CDVAtom atom = (CDVAtom)obj;
                    atom.Parent = this;
                    Words.Add(atom);
                }
                else if (obj is CDVAtom[])
                {
                    foreach (CDVAtom atom in (CDVAtom[])obj)
                    {
                        atom.Parent = this;
                        Words.Add(atom);
                    }
                }
            }
        }


        public override int GetMinimumWidth(CDVContext context)
        {
            int minWidth = 0;
            foreach(CDVAtom atom in Words)
            {
                minWidth = Math.Max(minWidth, atom.GetMinimumWidth(context));
            }

            return minWidth;
        }


        public override void DrawInRect(CDVContext context)
        {
            if (!Visible) return;

            base.DrawInRect(context);

            foreach (CDVAtom atom in Words)
            {
                atom.DrawInRect(context);
            }

        }

        public override void MeasureRect(CDVContext context, int maxWidth)
        {
            Location = new Point(0,0);
            Size = new Size(0, 4);
            int bx = p_para_style == null ? 0 : p_para_style.Margin.Left + p_para_style.Padding.Left;
            int by = p_para_style == null ? 0 : p_para_style.Margin.Top + p_para_style.Padding.Top;
            int bw = p_para_style == null ? 0 : p_para_style.Margin.Right + p_para_style.Padding.Right;
            int bh = p_para_style == null ? 0 : p_para_style.Margin.Bottom + p_para_style.Padding.Bottom;

            int x = 0;
            int y = 0;
            int maxLineWidth = 0;
            CDVLine line = new CDVLine();
            if (Orientation == CDVOrientation.Vertical)
            {
                foreach (CDVAtom atom in Words)
                {
                    atom.MeasureRect(context, maxWidth);
                    atom.Location = new Point(x, y);
                    y += atom.Height;
                    maxLineWidth = Math.Max(maxLineWidth, atom.Width);
                }

                ContentRect = new Rectangle(bx, by, maxLineWidth, y);
                Size = new Size(ContentRect.Width + bx + bw, ContentRect.Height + by + bh);
            }
            else
            {
                foreach (CDVAtom atom in Words)
                {
                    if (atom is CDVRuler)
                    {
                        CDVRuler ruler = (CDVRuler)atom;
                        if (ruler.Value - ruler.Width / 2 > x)
                            x = ruler.Value + ruler.Width / 2;
                        else
                        {
                            context.rulersChanged = true;
                            ruler.Value = Math.Max(ruler.Value, x + ruler.Width / 2);
                            x += ruler.Width;
                        }
                    }
                    else
                    {
                        atom.MeasureRect(context, maxWidth);
                        if (x + atom.Bounds.Width + bx + bw > maxWidth && line.Count > 0)
                        {
                            x = 0;
                            y += line.MaxHeight;
                            line.ApplyAlignment(maxWidth - bx - bw, ParaStyle.Align);
                            maxLineWidth = Math.Max(maxLineWidth, line.Right);
                            Lines.Add(line);
                            line = new CDVLine();
                            atom.Location = new Point(x, y);
                        }
                        else
                        {
                            atom.Location = new Point(x, y);
                            x += atom.Bounds.Width;
                        }
                    }
                    line.Add(atom);
                }
                maxLineWidth = Math.Max(maxLineWidth, line.Right);
                line.ApplyAlignment(maxWidth - bx - bw, ParaStyle.Align);
                Lines.Add(line);

                ContentRect = new Rectangle(bx, by, maxLineWidth, y + line.MaxHeight);

                Size = new Size(ContentRect.Width + bx + bw, ContentRect.Height + by + bh);
            }


            base.MeasureRect(context, maxWidth);
        }

        public void Add(CDVAtom atom)
        {
            atom.Parent = this;
            Words.Add(atom);
        }

        public override int ApplySpanWidths(int width)
        {
            int insideWidth = base.ApplySpanWidths(width);
            int difference = insideWidth - ContentRect.Width;

            if (Orientation == CDVOrientation.Horizontal && difference >= 0)
            {
                int movement = 0;
                int dif1 = 0;
                int dif2 = difference / Words.Count;
                for(int i = 0; i < Words.Count; i++)
                {
                    dif1 = i < Words.Count - 1 ? dif2 : difference;
                    Words[i].X += movement;
                    Words[i].ApplySpanWidths(Words[i].Width + dif1);
                    difference -= dif2;
                    movement += dif2;
                }
            }
            else if (Orientation == CDVOrientation.Vertical && difference >= 0)
            {
                foreach(CDVAtom atom in Words)
                {
                    atom.ApplySpanWidths(insideWidth);
                }
            }

            ContentRect.Width = insideWidth;

            return insideWidth;
        }

        public override void ApplyContentAlignment()
        {
            base.ApplyContentAlignment();

            foreach(CDVAtom atom in Words)
            {
                atom.ApplyContentAlignment();
            }
        }
    }
}
