using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GCAL.CalendarDataView
{
    public class CDVAtom
    {
        public CDVAtom Parent { get; set; }

        public CDVSpan SpanWidth { get; set; }

        public Rectangle ContentRect;


        private Rectangle p_bounds;

        public Size Size {  get { return p_bounds.Size; } set { p_bounds.Size = value; } }

        public Point Location {  get { return p_bounds.Location; } set { p_bounds.Location = value; } }

        public Rectangle Bounds {  get { return p_bounds; } set { p_bounds = value; } }

        public int X {  get { return p_bounds.X; } set { p_bounds.X = value; } }
        public int Height {  get { return p_bounds.Height; } }
        public int Width {  get { return p_bounds.Width; } set { p_bounds.Width = value; }  }

        private Rectangle border = Rectangle.Empty;
        protected CDVParaStyle p_para_style = CDVParaStyle.Empty;
        private CDVTextStyle p_text_style = null;
        private CDVVisibilityStyle p_visibility_style = null;

        public int Left { get { return Location.X; } }
        public int Right {  get { return Location.X + Size.Width; } }
        public int Top {  get { return Location.Y; } }
        public int Bottom {  get { return Location.Y + Size.Height; } }


        public CDVAtom()
        {
            SpanWidth = CDVSpan.Minimum;
        }

        public virtual void Offset(int x, int y)
        {
            border.Offset(x, y);
            p_bounds.Offset(x, y);
        }

        public virtual void MoveTo(int x, int y)
        {
            Offset(x - Location.X, y - Location.Y);
        }


        public CDVVisibilityStyle Visibility
        {
            get
            {
                if (p_visibility_style == null && Parent != null)
                    return Parent.Visibility;
                return p_visibility_style;
            }
            set
            {
                p_visibility_style = value;
            }
        }

        public bool Visible
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public CDVParaStyle ParaStyle
        {
            get
            {
                if (p_para_style == null && Parent != null)
                    return Parent.ParaStyle;
                return p_para_style;
            }
            set
            {
                p_para_style = value;
            }
        }

        public CDVTextStyle TextStyle
        {
            get
            {
                if (p_text_style == null && Parent != null)
                    return Parent.TextStyle;
                return p_text_style;
            }
            set
            {
                p_text_style = value;
            }
        }


        public CDVAtom(CDVAtom parent)
        {
            Parent = parent;
        }

        public virtual int GetMinimumWidth(CDVContext context)
        {
            return 10;
        }

        public virtual void MeasureRect(CDVContext context, int maxWidth)
        {
            if (!Visible)
            {
                Size = Size.Empty;
            }
        }

        public void GetAbsoluteLocation(out int x, out int y)
        {
            x = Bounds.X + ContentRect.X;
            y = Bounds.Y + ContentRect.Y;
            if (Parent != null)
            {
                int xp, yp;
                Parent.GetAbsoluteLocation(out xp, out yp);
                x += xp;
                y += yp;
            }
        }

        public virtual void DrawInRect(CDVContext context)
        {

            int x, y;
            GetAbsoluteLocation(out x, out y);
            border.X = x - ContentRect.X + ParaStyle.Margin.Left;
            border.Y = y - ContentRect.Y + ParaStyle.Margin.Top;
            border.Width = Bounds.Width - ParaStyle.Margin.Left - ParaStyle.Margin.Right;
            border.Height = Bounds.Height - ParaStyle.Margin.Top - ParaStyle.Margin.Bottom;

            if (!ParaStyle.BackgroundColor.Equals(CDVColor.Transparent))
            {
                context.g.FillRectangle(CDVContext.GetBrush(ParaStyle.BackgroundColor), border);
            }

            if (ParaStyle.BorderWidth.GetMax() > 0)
            {
                context.g.DrawLine(CDVContext.GetPen(ParaStyle.BorderWidth.Top, ParaStyle.BorderColor), border.Left, border.Top, border.Right, border.Top);
                context.g.DrawLine(CDVContext.GetPen(ParaStyle.BorderWidth.Bottom, ParaStyle.BorderColor), border.Left, border.Bottom, border.Right, border.Bottom);
                context.g.DrawLine(CDVContext.GetPen(ParaStyle.BorderWidth.Left, ParaStyle.BorderColor), border.Left, border.Top, border.Left, border.Bottom);
                context.g.DrawLine(CDVContext.GetPen(ParaStyle.BorderWidth.Right, ParaStyle.BorderColor), border.Right, border.Top, border.Right, border.Bottom);
            }

            //context.g.DrawRectangle(Pens.Blue, border);

        }

        protected virtual void OnWidthChanged()
        {

        }

        public virtual void ApplyContentAlignment()
        {
            int bx = p_para_style == null ? 0 : p_para_style.Margin.Left + p_para_style.Padding.Left;
            int bw = p_para_style == null ? 0 : p_para_style.Margin.Right + p_para_style.Padding.Right;

            switch (p_para_style == null ? CDVAlign.Left : p_para_style.Align)
            {
                case CDVAlign.Left:
                    ContentRect.X = bx;
                    break;
                case CDVAlign.Center:
                    ContentRect.X = (Bounds.Width - bx - bw - ContentRect.Width) / 2;
                    break;
                case CDVAlign.Right:
                    ContentRect.X = Bounds.Width - bw - ContentRect.Width;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns width available for child items.</returns>
        public virtual int ApplySpanWidths(int width)
        {
            if (SpanWidth != CDVSpan.Maximum || ContentRect.Width > width)
                return ContentRect.Width;

            int insideWidth = width;

            if (p_para_style != null)
                insideWidth = width - p_para_style.Padding.Left - p_para_style.Padding.Right 
                    - p_para_style.Margin.Left - p_para_style.Margin.Right;

            //int difference = 0;

            if (width > this.Width)
            {
                //difference = insideWidth - this.Width;
                this.Width = width;
            }

            return insideWidth;
        }
    }
}
