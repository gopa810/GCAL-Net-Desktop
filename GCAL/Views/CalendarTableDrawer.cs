using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using GCAL.Base;

namespace GCAL.Views
{
    public class CalendarTableDrawer
    {
        public float PaddingTop = 10.0f;
        public float PaddingBottom = 10.0f;
        public float PaddingLeft = 10.0f;
        public float PaddingRight = 10.0f;

        public bool ShowTodayBorder = false;

        private Size prevSize;
        private float prevH1Size = 0;
        private Font fontH1;
        private Font fontH2;
        private Font fontH3;
        private Font fontH4;
        private Font fontS1;
        private Font fontS2;
        private Font fontS3;
        private Pen monthDivider;
        private Pen dayDivider;
        private Pen todayBorder;


        private float xMarginLeft;
        private float yMarginTop;
        private float xMarginRight;
        private float yMarginBottom;
        private float yUnderTitleBar;
        private float yUnderMainTitle;
        private int firstDayOfWeekGeneral;
        private float yTableTop;
        private float xCellWidth;
        private float yCellHeight;
        private float xCellMarginLeft;
        private float yCellMarginTop;
        private float xCellAnchor1, xCellMarginRight;
        private float yCellAnchor1, yCellMarginBottom;


        public CalendarTableDrawer()
        {
            prevSize = Size.Empty;
            monthDivider = new Pen(Color.Gray, 3);
            dayDivider = new Pen(Color.Black, 1);
            todayBorder = new Pen(Color.Blue, 3);
        }

        public string GetTitleString(int nYear, int nMonth)
        {
            return string.Format("{0} {1}", GregorianDateTime.GetMonthName(nMonth), nYear);
        }

        internal void Draw(Graphics graphics, Size size, Base.TResultCalendar tResultCalendar, int nYear, int nMonth)
        {
            Graphics g = graphics;
            SizeF textSize = Size.Empty;
            float fontH1Size = Math.Min(size.Height - PaddingLeft - PaddingRight, size.Width * 0.707f - PaddingTop - PaddingBottom) / 30;
            xMarginLeft = PaddingLeft;
            yMarginTop = PaddingTop;
            xMarginRight = size.Width - PaddingRight;
            yUnderTitleBar = 0;
            yUnderMainTitle = 0;
            float tx;
            float cellInset = fontH1Size / 5;
            StringFormat rectFormat = new StringFormat();
            rectFormat.Trimming = StringTrimming.EllipsisWord;
            DateTime dtToday = DateTime.Now;


            InitializeFonts(fontH1Size);

            string str;
            
            DrawTitle(nYear, nMonth, g);

            if (tResultCalendar == null)
                return;

            DrawRightTitleInfo(tResultCalendar, g);

            g.DrawLine(monthDivider, xMarginLeft, yUnderTitleBar, xMarginRight, yUnderTitleBar);

            firstDayOfWeekGeneral = GCDisplaySettings.Current.getValue(GCDS.GENERAL_FIRST_DOW);
            xCellWidth = (xMarginRight - xMarginLeft) / 7.0f;

            float tempTextHeight = fontH1Size/5;

            // draw names of weekdays
            textSize = DrawWeekdayHeader(g, tempTextHeight);

            yTableTop = yUnderTitleBar + 2 * tempTextHeight + textSize.Height;


            yMarginBottom = size.Height - PaddingBottom;
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

            RectangleF rcf = new RectangleF();
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
                    g.FillRectangle(Brushes.LightGreen, rcf);
                else if (vd.nFastID != 0)
                    g.FillRectangle(Brushes.LightSkyBlue, rcf);
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

                Font f = fontS2;
                textSize = g.MeasureString(str, fontS2, Convert.ToInt32(xCellMarginRight - xCellMarginLeft));
                if (textSize.Height + yCellAnchor1 + cellInset > yCellMarginTop + yCellHeight)
                {
                    f = fontS3;
                    textSize = g.MeasureString(str, f, Convert.ToInt32(xCellMarginRight - xCellMarginLeft));
                }
                rcf.Y = yCellAnchor1;
                rcf.X = xCellMarginLeft;
                rcf.Width = textSize.Width;
                rcf.Height = Math.Min(yCellMarginBottom - yCellAnchor1, textSize.Height);
                g.DrawString(str, f, Brushes.Black, rcf, rectFormat);


            }

            // masa info text
            g.DrawString(masaText.ToString(), fontS2, Brushes.Black, xMarginLeft, yUnderMainTitle);

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

        private static string GetEventsLineText(VAISNAVADAY vd)
        {
            string str;
            StringBuilder sb = new StringBuilder();
            foreach (VAISNAVAEVENT ed in vd.dayEvents)
            {
                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }
                int disp = ed.dispItem;
                if (ed.dispItem != 0 && (disp == -1 || GCDisplaySettings.Current.getValue(disp) != 0))
                {
                    sb.Append(ed.text);
                }
            }

            sb.Replace("-- Appearance", "[app]");
            sb.Replace("-- Disappearance", "[dis]");
            str = sb.ToString();
            return str;
        }

        private float DrawTithiText(Graphics g, VAISNAVADAY vd)
        {
            SizeF textSize;
            float tempTextHeight = 0;
            string str2 = GCTithi.GetName(vd.astrodata.sunRise.Tithi);
            textSize = g.MeasureString(str2, fontH3);
            g.DrawString(str2, fontH3, Brushes.Black, xCellAnchor1, yCellMarginTop);
            tempTextHeight = yCellMarginTop + textSize.Height;
            if (vd.HasExtraFastingNote())
            {
                str2 = vd.GetExtraFastingNote();
                textSize = g.MeasureString(str2, fontS2);
                g.DrawString(str2, fontS2, Brushes.Black, xCellAnchor1, tempTextHeight);
                tempTextHeight += textSize.Height;
            }

            return tempTextHeight;
        }

        private SizeF DrawCellDayNumber(Graphics g, int i)
        {
            string str;
            str = i.ToString();
            SizeF textSize = g.MeasureString(str, fontH4);
            g.DrawString(str, fontH4, Brushes.Black, xCellMarginLeft, yCellMarginTop);
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

        private SizeF DrawWeekdayHeader(Graphics g, float tempTextHeight)
        {
            SizeF textSize = SizeF.Empty;
            string str;
            for (int i = 0; i < 7; i++)
            {
                str = GCCalendar.GetWeekdayName((i + firstDayOfWeekGeneral) % 7);
                textSize = g.MeasureString(str, fontH2);

                g.DrawString(str, fontH2, Brushes.Black, xMarginLeft + xCellWidth * i + xCellWidth / 2 - textSize.Width / 2, yUnderTitleBar + tempTextHeight);
            }
            return textSize;
        }

        private void DrawRightTitleInfo(Base.TResultCalendar tResultCalendar, Graphics g)
        {
            string str = tResultCalendar.m_Location.GetFullName();
            SizeF textSize = g.MeasureString(str, fontH3);
            g.DrawString(str, fontH3, Brushes.Black, xMarginRight - textSize.Width, yMarginTop);
            float tx = textSize.Height;
            str = GCStrings.ShortVersionText;
            textSize = g.MeasureString(str, fontS1);
            g.DrawString(str, fontS1, Brushes.Black, xMarginRight - textSize.Width, yMarginTop + tx);
        }

        private void DrawTitle(int nYear, int nMonth, Graphics g)
        {
            string str = GetTitleString(nYear, nMonth);
            yUnderMainTitle = yMarginTop + g.MeasureString(str, fontH1).Height;
            yUnderTitleBar = yUnderMainTitle + g.MeasureString(str, fontH1).Height * 0.5f;
            g.DrawString(str, fontH1, Brushes.Black, new PointF(xMarginLeft, yMarginTop));
        }

        private void InitializeFonts(float fontH1Size)
        {
            if (prevH1Size != fontH1Size)
            {
                fontH1 = new Font(FontFamily.GenericSansSerif, fontH1Size);
                prevH1Size = fontH1Size;
                fontH2 = new Font(FontFamily.GenericSansSerif, fontH1Size * 10.0f / 17.0f);
                fontH3 = new Font(FontFamily.GenericSansSerif, fontH1Size * 8.0f / 17.0f);
                fontH4 = new Font(FontFamily.GenericSansSerif, fontH1Size * 12.0f / 17.0f);
                fontS1 = new Font(FontFamily.GenericSansSerif, fontH1Size * 7.0f / 17.0f);
                fontS2 = new Font(FontFamily.GenericSansSerif, fontH1Size * 6.0f / 17.0f);
                fontS3 = new Font(FontFamily.GenericSansSerif, fontH1Size * 5.0f / 17.0f);
            }
        }


        public static void ExportPng(GCLocation calLocation, string directory, string locname, int year, int month)
        {
            using (Bitmap b = new Bitmap(1280, 1024))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    CalendarTableDrawer ctd = new CalendarTableDrawer();
                    TResultCalendar rc = new TResultCalendar();
                    rc.CalculateCalendar(calLocation, new GregorianDateTime(year, month, 1), 32);
                    ctd.Draw(g, new Size(1280, 1024), rc, year, month);
                }
                b.Save(Path.Combine(directory, string.Format("{0}-{1}-{2:00}.png", locname, year, month)), ImageFormat.Png);
            }
        }

        public static void ExportPngYear(GCLocation calLocation, string dir, string locationFileName, int currentYear)
        {
            Size imageSize = new Size(1280, 1024);
            using (Bitmap b = new Bitmap(imageSize.Width, imageSize.Height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    CalendarTableDrawer ctd = new CalendarTableDrawer();
                    TResultCalendar rc = new TResultCalendar();
                    rc.CalculateCalendar(calLocation, new GregorianDateTime(currentYear, 1, 1), 366);

                    for (int i = 1; i <= 12; i++)
                    {
                        g.Clear(Color.White);
                        ctd.Draw(g, imageSize, rc, currentYear, i);
                        b.Save(Path.Combine(dir, string.Format("{0}-{1}-{2:00}.png", locationFileName, currentYear, i)), ImageFormat.Png);
                    }
                }
            }

            string text = string.Format("<html><head><title>{0} {1}</title></head><body>", locationFileName, currentYear);
            StringBuilder sb = new StringBuilder();
            sb.Append(text);
            for (int i = 1; i < 13; i++)
            {
                sb.AppendFormat("<p><img src=\"{0}-{1}-{2:00}.png\"></p>", locationFileName, currentYear, i);
            }
            sb.AppendFormat("</body></html>");

            File.WriteAllText(Path.Combine(dir, string.Format("{0}-{1}.html", locationFileName, currentYear)), sb.ToString());
        }
    }
}
