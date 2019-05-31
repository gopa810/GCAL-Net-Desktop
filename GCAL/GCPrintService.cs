using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.IO;

using GCAL.Views;
using GCAL.Base;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing;

namespace GCAL
{
    public class GCPrintService
    {
        public void Print()
        {
            PrintDocument doc = new PrintDocument();

        }

        public static void ExportTableCalendar(GCLocation calLocation, int printYearStart, int printYearEnd, string folderPath)
        {

            TResultCalendar m_calendarToPrint = new TResultCalendar();
            GregorianDateTime startDate = new GregorianDateTime(printYearStart, 1, 1);
            GregorianDateTime endDate = new GregorianDateTime(printYearEnd, 12, 1);
            m_calendarToPrint.CalculateCalendar(calLocation, startDate, endDate.GetJulianInteger() - startDate.GetJulianInteger() + 31);

            string DocumentName = string.Format("{0}_{1}", calLocation.Title, printYearStart);

            PdfDocument doc = new PdfDocument();

            CalendarPdfDrawer ctd = new CalendarPdfDrawer();

            for (int year = printYearStart; year <= printYearEnd; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    PdfPage page = doc.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    page.Orientation = PdfSharp.PageOrientation.Landscape;

                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    SizeF pageSizeF = gfx.PageSize.ToSizeF();
                    ctd.PaddingTop = Convert.ToInt32(pageSizeF.Height / 20);
                    ctd.PaddingBottom = Convert.ToInt32(pageSizeF.Height / 20);
                    ctd.PaddingLeft = Convert.ToInt32(pageSizeF.Width / 20);
                    ctd.PaddingRight = Convert.ToInt32(pageSizeF.Width / 20);

                    ctd.Draw(gfx, gfx.PageSize, m_calendarToPrint, year, month);
                }
            }

            doc.Save(folderPath);

        }

        public static void ExportTableCalendar(TResultCalendar calendar, int printYearStart, int printYearEnd, string folderPath)
        {

            PdfDocument doc = new PdfDocument();

            CalendarPdfDrawer ctd = new CalendarPdfDrawer();

            for (int year = printYearStart; year <= printYearEnd; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    PdfPage page = doc.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    page.Orientation = PdfSharp.PageOrientation.Landscape;

                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    SizeF pageSizeF = gfx.PageSize.ToSizeF();
                    ctd.PaddingTop = Convert.ToInt32(pageSizeF.Height / 20);
                    ctd.PaddingBottom = Convert.ToInt32(pageSizeF.Height / 20);
                    ctd.PaddingLeft = Convert.ToInt32(pageSizeF.Width / 20);
                    ctd.PaddingRight = Convert.ToInt32(pageSizeF.Width / 20);

                    ctd.Draw(gfx, gfx.PageSize, calendar, year, month);
                }
            }

            doc.Save(folderPath);

        }
    }
}
