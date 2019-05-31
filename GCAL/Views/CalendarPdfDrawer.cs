using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCAL.Base;
using System.Drawing;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace GCAL.Views
{
    public class CalendarPdfDrawer
    {
        public float PaddingTop = 10.0f;
        public float PaddingBottom = 10.0f;
        public float PaddingLeft = 10.0f;
        public float PaddingRight = 10.0f;

        public bool ShowTodayBorder = false;

        private XSize prevSize;
        private float prevH1Size = 0;
        private XFont fontH1;
        private XFont fontH2;
        private XFont fontH3;
        private XFont fontH4;
        private XFont fontS1;
        private XFont fontS2;
        private XFont fontS3;
        private XPen monthDivider;
        private XPen dayDivider;
        private XPen todayBorder;


        private double xMarginLeft;
        private double yMarginTop;
        private double xMarginRight;
        private double yMarginBottom;
        private double yUnderTitleBar;
        private double yUnderMainTitle;
        private int firstDayOfWeekGeneral;
        private double yTableTop;
        private double xCellWidth;
        private double yCellHeight;
        private double xCellMarginLeft;
        private double yCellMarginTop;
        private double xCellAnchor1, xCellMarginRight;
        private double yCellAnchor1, yCellMarginBottom;


        public CalendarPdfDrawer()
        {
            prevSize = XSize.Empty;
            monthDivider = new XPen(XColor.FromArgb(127, 127, 127), 3);
            dayDivider = new XPen(XColor.FromArgb(0, 0, 0), 1);
            todayBorder = new XPen(XColor.FromArgb(0, 0, 0, 255), 3);
        }

        public string GetTitleString(int nYear, int nMonth)
        {
            return string.Format("{0} {1}", GregorianDateTime.GetMonthName(nMonth), nYear);
        }

        internal void Draw(XGraphics graphics, XSize size, Base.TResultCalendar tResultCalendar, int nYear, int nMonth)
        {
            XGraphics g = graphics;
            XSize textSize = XSize.Empty;
            double fontH1Size = (float) Math.Min(size.Height - PaddingLeft - PaddingRight, size.Width * 0.707f - PaddingTop - PaddingBottom) / 30;
            xMarginLeft = PaddingLeft;
            yMarginTop = PaddingTop;
            xMarginRight = (float)(size.Width - PaddingRight);
            yUnderTitleBar = 0;
            yUnderMainTitle = 0;
            double tx;
            double cellInset = fontH1Size / 5;
            XStringFormat rectFormat = new XStringFormat();
            DateTime dtToday = DateTime.Now;
            GCLocation loc = tResultCalendar.m_Location;


            InitializeFonts((float)fontH1Size);

            string str;

            DrawTitle(nYear, nMonth, g);

            if (tResultCalendar == null)
                return;

            DrawRightTitleInfo(tResultCalendar, g);

            g.DrawLine(monthDivider, xMarginLeft, yUnderTitleBar, xMarginRight, yUnderTitleBar);

            firstDayOfWeekGeneral = (loc.Country.FirstDayOfWeek < 0 || loc.Country.FirstDayOfWeek > 6) 
                ? GCDisplaySettings.Current.getValue(GCDS.GENERAL_FIRST_DOW)
                : loc.Country.FirstDayOfWeek;
            xCellWidth = (xMarginRight - xMarginLeft) / 7.0f;

            double tempTextHeight = fontH1Size / 5;

            // draw names of weekdays
            textSize = DrawWeekdayHeader(g, tempTextHeight);

            yTableTop = (float)(yUnderTitleBar + 2 * tempTextHeight + textSize.Height);


            yMarginBottom = (float)(size.Height - PaddingBottom);
            int firstIndex = 0;
            int firstDayDow = 0;

            for (int i = 0; i < tResultCalendar.m_nCount; i++)
            {
                GregorianDateTime gdt = tResultCalendar.m_pData[i].date;
                if (gdt.day == 1 && gdt.month == nMonth && gdt.year == nYear)
                {
                    firstIndex = i;
                    firstDayDow = gdt.dayOfWeek;
                }
            }

            int daysBeforeFirstDay = (firstDayDow - firstDayOfWeekGeneral + 14) % 7;
            int monthDays = GregorianDateTime.GetMonthMaxDays(nYear, nMonth);

            int totalDays = daysBeforeFirstDay + monthDays;
            int weeksCount = ((totalDays % 7) == 0) ? (totalDays / 7) : (totalDays / 7 + 1);

            yCellHeight = (yMarginBottom - yTableTop) / weeksCount;

            XRect rcf = new XRect();
            int row, column, di;
            StringBuilder masaText = new StringBuilder();
            for (int i = 1; i <= monthDays; i++)
            {
                di = daysBeforeFirstDay + i - 1;
                row = di / 7;
                column = di % 7;

                VAISNAVADAY vd = tResultCalendar.m_pData[firstIndex + i - 1];
                VAISNAVADAY vd_prev = tResultCalendar.m_pData[firstIndex + i - 2];
                VAISNAVADAY vd_next = tResultCalendar.m_pData[firstIndex + i];

                ConstructMasaText(masaText, i, vd, vd_prev, vd_next);

                yCellMarginTop = yTableTop + row * yCellHeight + cellInset;
                xCellMarginLeft = xMarginLeft + column * xCellWidth + cellInset;

                rcf.X = xCellMarginLeft - cellInset;
                rcf.Y = yCellMarginTop - cellInset;
                rcf.Width = xCellWidth;
                rcf.Height = yCellHeight;
                if (vd.nFastID == FastType.FAST_EKADASI)
                    g.DrawRectangle(Brushes.LightGreen, rcf);
                else if (vd.nFastID != 0)
                    g.DrawRectangle(Brushes.LightSkyBlue, rcf);
                if (dtToday.Day == i && dtToday.Month == nMonth && dtToday.Year == nYear)
                {
                    g.DrawRectangle(todayBorder, rcf.Left, rcf.Top, rcf.Width, rcf.Height);
                }


                textSize = DrawCellDayNumber(g, i);

                yCellAnchor1 = yCellMarginTop + textSize.Height + cellInset;
                xCellAnchor1 = xCellMarginLeft + textSize.Width + cellInset;
                xCellMarginRight = xCellMarginLeft + xCellWidth - 2 * cellInset;
                yCellMarginBottom = yCellMarginTop + yCellHeight - 2 * cellInset;


                tempTextHeight = DrawTithiText(g, vd);

                yCellAnchor1 = Math.Max(tempTextHeight + cellInset, yCellAnchor1);


                rcf.X = xCellAnchor1;
                rcf.Y = yCellMarginTop;
                rcf.Width = xCellMarginRight - xCellAnchor1;
                rcf.Height = textSize.Height + 2;

                str = GetEventsLineText(vd);

                XFont f = fontS2;
                textSize = g.MeasureString(str, fontS2);
                double tsh = textSize.Width / (xCellWidth - 2 * cellInset) * textSize.Height;
                if (textSize.Height + yCellAnchor1 + cellInset > yCellMarginTop + yCellHeight)
                {
                    f = fontS3;
                    textSize = g.MeasureString(str, f);
                }
                rcf.Y = yCellAnchor1;
                rcf.X = xCellMarginLeft;
                rcf.Width = xCellWidth - 2 * cellInset;
                rcf.Height = yCellMarginBottom - yCellAnchor1 + 2;

                DrawTextRectangle(g, rectFormat, str, rcf, f);

            }

            // masa info text
            string masaTextStr = masaText.ToString();
            textSize = g.MeasureString(masaTextStr, fontS2);
            g.DrawString(masaText.ToString(), fontS2, Brushes.Black, xMarginLeft, yUnderMainTitle + textSize.Height);

            // final step: draw matrix
            for (int i = 0; i < 8; i++)
            {
                g.DrawLine(dayDivider, xMarginLeft + i * xCellWidth, yTableTop, xMarginLeft + i * xCellWidth, yMarginBottom);
            }
            for (int j = 0; j <= weeksCount; j++)
            {
                g.DrawLine(dayDivider, xMarginLeft, yTableTop + j * yCellHeight, xMarginRight, yTableTop + j * yCellHeight);
            }




        }

        private static void DrawTextRectangle(XGraphics g, XStringFormat rectFormat, string str, XRect rcf, XFont f)
        {
            int startL = 0;
            int startP = 0;
            int endP = 0;
            int lastEndP = 0;
            XPoint point = rcf.Location;

            XSize textSize;
            str = str.Trim();
            while (endP < str.Length - 1)
            {
                endP = FindNextSpaceFit(g, str, rcf, f, startP);
                if (endP <= startP)
                    return;
                string subs = str.Substring(startP, endP - startP);
                textSize = g.MeasureString(subs, f);
                g.DrawString(subs, f, Brushes.Black, point.X, point.Y + textSize.Height);
                point.Y += textSize.Height * 1.2;
                startP = endP + 1;
            }
        }

        private static int FindNextSpaceFit(XGraphics g, string str, XRect rcf, XFont f, int startP)
        {
            XSize textSize;
            int startL = startP;
            int lastEndP = startP;
            int endP = 0;
            while (endP >= 0)
            {
                endP = str.IndexOf(' ', startP);
                if (endP < 0)
                {
                    // till end of string
                    return str.Length;
                }
                else
                {
                    try
                    {
                    textSize = g.MeasureString(str.Substring(startL, endP - startL), f);
                    if (textSize.Width > rcf.Width)
                    {
                        // go back to last endP
                        return lastEndP;
                    }
                    else
                    {
                        startP = endP + 1;
                    }
                }
                    catch (Exception ex)
                    {
                        startP = endP + 1;
                    }
                }
                lastEndP = endP;
            }

            return endP;
        }

        private static string GetEventsLineText(VAISNAVADAY vd)
        {
            string str;
            StringBuilder sb = new StringBuilder();
            foreach (VAISNAVAEVENT ed in vd.dayEvents)
            {
                int disp = ed.dispItem;
                if (ed.dispItem != 0 && (disp == -1 || GCDisplaySettings.Current.getValue(disp) != 0))
                {
                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }
                    sb.Append(ed.text);
                }
            }

            sb.Replace("-- Appearance", "[app]");
            sb.Replace("-- Disappearance", "[dis]");
            str = sb.ToString();
            return str;
        }

        private double DrawTithiText(XGraphics g, VAISNAVADAY vd)
        {
            XSize textSize;
            double tempTextHeight = 0;
            string str2 = GCTithi.GetName(vd.astrodata.sunRise.Tithi);
            textSize = g.MeasureString(str2, fontH3);
            g.DrawString(str2, fontH3, Brushes.Black, xCellAnchor1, yCellMarginTop + textSize.Height);
            tempTextHeight = yCellMarginTop + textSize.Height;
            if (vd.HasExtraFastingNote())
            {
                str2 = vd.GetExtraFastingNote();
                textSize = g.MeasureString(str2, fontS2);
                g.DrawString(str2, fontS2, Brushes.Black, xCellAnchor1, tempTextHeight + textSize.Height);
                tempTextHeight += textSize.Height;
            }

            return tempTextHeight;
        }

        private XSize DrawCellDayNumber(XGraphics g, int i)
        {
            string str;
            str = i.ToString();
            XSize textSize = g.MeasureString(str, fontH4);
            g.DrawString(str, fontH4, Brushes.Black, xCellMarginLeft, yCellMarginTop + textSize.Height);
            return textSize;
        }

        private static void ConstructMasaText(StringBuilder masaText, int i, VAISNAVADAY vd, VAISNAVADAY vd_prev, VAISNAVADAY vd_next)
        {
            if (vd.astrodata.Masa != vd_prev.astrodata.Masa || i == 1)
            {
                if (masaText.Length > 0)
                    masaText.Append(", ");
                masaText.AppendFormat("{0} Masa ", GCMasa.GetName(vd.astrodata.Masa));
                if (vd.astrodata.Masa != vd_prev.astrodata.Masa)
                    masaText.AppendFormat("from {0} ", vd.date.ToString());
            }
            else if (vd.astrodata.Masa != vd_next.astrodata.Masa)
            {
                masaText.AppendFormat("to {0}", vd.date.ToString());
            }
        }

        private XSize DrawWeekdayHeader(XGraphics g, double tempTextHeight)
        {
            XSize textSize = XSize.Empty;
            string str;
            for (int i = 0; i < 7; i++)
            {
                str = GCCalendar.GetWeekdayName((i + firstDayOfWeekGeneral) % 7);
                textSize = g.MeasureString(str, fontH2);

                g.DrawString(str, fontH2, Brushes.Black, xMarginLeft + xCellWidth * i + xCellWidth / 2 - textSize.Width / 2, yUnderTitleBar + tempTextHeight + textSize.Height);
            }
            return textSize;
        }

        private void DrawRightTitleInfo(Base.TResultCalendar tResultCalendar, XGraphics g)
        {
            string str = tResultCalendar.m_Location.GetFullName();
            XSize textSize = g.MeasureString(str, fontH3);
            g.DrawString(str, fontH3, Brushes.Black, xMarginRight - textSize.Width, yMarginTop + textSize.Height);
            double tx = textSize.Height;
            str = GCStrings.ShortVersionText;
            textSize = g.MeasureString(str, fontS1);
            g.DrawString(str, fontS1, Brushes.Black, xMarginRight - textSize.Width, yMarginTop + tx + textSize.Height);
        }

        private void DrawTitle(int nYear, int nMonth, XGraphics g)
        {
            string str = GetTitleString(nYear, nMonth);
            yUnderMainTitle = (float)(yMarginTop + g.MeasureString(str, fontH1).Height);
            yUnderTitleBar = (float)(yUnderMainTitle + g.MeasureString(str, fontH1).Height * 0.5f);
            XSize textSize = g.MeasureString(str, fontH1);
            g.DrawString(str, fontH1, Brushes.Black, new XPoint(xMarginLeft, yMarginTop + textSize.Height));
        }

        private void InitializeFonts(float fontH1Size)
        {
            if (prevH1Size != fontH1Size)
            {
                fontH1 = new XFont("Arial", fontH1Size);
                prevH1Size = fontH1Size;
                fontH2 = new XFont("Arial", fontH1Size * 10.0f / 17.0f);
                fontH3 = new XFont("Arial", fontH1Size * 8.0f / 17.0f);
                fontH4 = new XFont("Arial", fontH1Size * 12.0f / 17.0f);
                fontS1 = new XFont("Arial", fontH1Size * 7.0f / 17.0f);
                fontS2 = new XFont("Arial", fontH1Size * 6.0f / 17.0f);
                fontS3 = new XFont("Arial", fontH1Size * 5.0f / 17.0f);
            }
        }
    }
}
