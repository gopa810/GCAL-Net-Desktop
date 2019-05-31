using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using GCAL.Base;

namespace GCAL
{
    public partial class ExportCompleteProgressDlg : Form
    {
        public ExportCompleteProgressDlg()
        {
            InitializeComponent();
        }

        public bool IsWorking = false;
        public bool CancelRequested = false;

        private void ExportCompleteProgressDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsWorking && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsWorking)
            {
                CancelRequested = true;
                backgroundWorker1.CancelAsync();
                button1.Enabled = false;
            }
            else
            {
                Close();
            }
        }

        public List<TLocation> SelectedLocations = new List<TLocation>();
        public int StartYear = 2020;
        public int EndYear = 2020;
        public string OutputDir = "";
        public bool includeSun = false;
        public bool includeCore = false;

        public void SetData(List<TLocation> locs, int start, int end, string dir, bool isun, bool icore)
        {
            SelectedLocations.AddRange(locs);
            StartYear = start;
            EndYear = end;
            OutputDir = dir;
            includeSun = isun;
            includeCore = icore;
        }

        public int WorkType = 0;

        public void Start(int i)
        {
            WorkType = i;

            if (WorkType == 1)
            {
                // calculate locs/years complete data
                label2.Text = OutputDir;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = SelectedLocations.Count;
                progressBar1.Value = 0;
                progressBar2.Minimum = StartYear;
                progressBar2.Maximum = EndYear;
                progressBar2.Value = StartYear;
                backgroundWorker1.RunWorkerAsync();
                button1.Text = "Cancel";
            }

        }

        private class FileRec
        {
            public string country;
            public string city;
            public int year;
            public string filename;
            public string filenameDetail;
            public string filenamePdf;
        }

        public string ToFilePart(string s)
        {
            byte[] array = Encoding.ASCII.GetBytes(s);
            char[] chars = Encoding.ASCII.GetChars(array);
            for (int i = 0; i < chars.Length; i++)
            {
                if (Char.IsLetterOrDigit(chars[i]))
                    chars[i] = Char.ToLower(chars[i]);
                else
                    chars[i] = '_';
            }
            return new String(chars);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SetProgressValue locdel = new SetProgressValue(SetLocationsProgressValue);
            SetProgressValue yrdel = new SetProgressValue(SetYearsProgressvalue);

            List<FileRec> files = new List<FileRec>();
            HashSet<string> countries = new HashSet<string>();

            GCDisplaySettings stateDetailedText = new GCDisplaySettings("Detailed Calendar");
            GCDisplaySettings stateText = new GCDisplaySettings("Brief Calendar");
            GCDisplaySettings stateOrig = GCDisplaySettings.Current;

            try
            {
//                stateText.Clear();
                stateText.setValue(GCDS.CAL_DST_CHANGE, 1);
                stateText.setValue(GCDS.CAL_EKADASI_PARANA, 1);
                stateText.setValue(GCDS.CAL_FEST_0, 1);
                stateText.CalHeaderType = 1;
                stateText.setValue(GCDS.CAL_MASA_CHANGE, 1);
                stateText.setValue(GCDS.CAL_MOON_LONG, 0);
                stateText.setValue(GCDS.CAL_MOON_RISE, 0);
                stateText.setValue(GCDS.CAL_MOON_SET, 0);
                stateText.setValue(GCDS.CAL_SANKRANTI, 0);
                stateText.setValue(GCDS.CAL_SUN_LONG, 0);
                stateText.setValue(GCDS.CAL_SUN_RISE, 0);
                stateText.setValue(GCDS.CAL_SUN_SANDHYA, 0);
                stateText.setValue(GCDS.CAL_VRDDHI, 0);
                stateText.setValue(GCDS.COREEVENTS_ABHIJIT_MUHURTA, 0);
                stateText.setValue(GCDS.COREEVENTS_ASCENDENT, 0);
                stateText.setValue(GCDS.COREEVENTS_CONJUNCTION, 0);
                stateText.setValue(GCDS.COREEVENTS_GULIKALAM, 0);
                stateText.setValue(GCDS.COREEVENTS_MOON, 0);
                stateText.setValue(GCDS.COREEVENTS_MOONRASI, 0);
                stateText.setValue(GCDS.COREEVENTS_NAKSATRA, 0);
                stateText.setValue(GCDS.COREEVENTS_RAHUKALAM, 0);
                stateText.setValue(GCDS.COREEVENTS_SANKRANTI, 0);
                stateText.setValue(GCDS.COREEVENTS_SUN, 0);
                stateText.setValue(GCDS.COREEVENTS_TITHI, 0);
                stateText.setValue(GCDS.COREEVENTS_YAMAGHANTI, 0);
                stateText.setValue(GCDS.COREEVENTS_YOGA, 0);

                stateDetailedText.setValue(GCDS.CAL_ARUN_TIME, 0);
                stateDetailedText.setValue(GCDS.CAL_ARUN_TITHI, 0);
                stateDetailedText.setValue(GCDS.CAL_AYANAMSHA, 0);
                stateDetailedText.setValue(GCDS.CAL_BRAHMA_MUHURTA, 1);
                stateDetailedText.setValue(GCDS.CAL_COREEVENTS, 1);
                stateDetailedText.setValue(GCDS.CAL_DST_CHANGE, 1);
                stateDetailedText.setValue(GCDS.CAL_EKADASI_PARANA, 1);
                stateDetailedText.setValue(GCDS.CAL_FEST_0, 1);
                stateDetailedText.CalHeaderType = 1;
                stateDetailedText.setValue(GCDS.CAL_JULIAN, 0);
                stateDetailedText.setValue(GCDS.CAL_KSAYA, 0);
                stateDetailedText.setValue(GCDS.CAL_MASA_CHANGE, 1);
                stateDetailedText.setValue(GCDS.CAL_SUN_RISE, 1);
                stateDetailedText.setValue(GCDS.CAL_SUN_SANDHYA, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_CONJUNCTION, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_GULIKALAM, 0);
                stateDetailedText.setValue(GCDS.COREEVENTS_MOON, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_MOONRASI, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_NAKSATRA, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_TITHI, 1);
                stateDetailedText.setValue(GCDS.COREEVENTS_YAMAGHANTI, 0);
                stateDetailedText.setValue(GCDS.COREEVENTS_YOGA, 1);
                stateDetailedText.setValue(GCDS.GENERAL_FIRST_DOW, stateOrig.getValue(GCDS.GENERAL_FIRST_DOW));


                string content;
                FileRec currentFileRec;

                for (int locIndex = 0; locIndex < SelectedLocations.Count; locIndex++)
                {
                    TLocation loc = SelectedLocations[locIndex];
                    if (!countries.Contains(loc.Country.Name))
                        countries.Add(loc.Country.Name);
                    progressBar1.Invoke(locdel, locIndex);
                    for (int year = StartYear; year <= EndYear; year++)
                    {
                        if (backgroundWorker1.CancellationPending)
                            return;
                        if (CancelRequested)
                            return;

                        progressBar2.Invoke(yrdel, year);
                        //                        Thread.Sleep(1000);

                        TResultCalendar calendar;
                        GCLocation locRef = loc.GetLocationRef();


                        currentFileRec = new FileRec()
                        {
                            filename = year.ToString() + "_" + ToFilePart(loc.CityName) + ".html",
                            filenameDetail = year.ToString() + "_" + ToFilePart(loc.CityName) + "_d.html",
                            filenamePdf = year.ToString() + "_" + ToFilePart(loc.CityName) + ".pdf",
                            city = loc.CityName,
                            country = loc.Country.Name,
                            year = year
                        };

                        files.Add(currentFileRec);

                        GCDisplaySettings.Current = stateText;
                        calendar = new TResultCalendar(loc.GetLocationRef(), year);
                        content = calendar.formatText(GCDataFormat.HTML);
                        // print plain TXT
                        File.WriteAllText(Path.Combine(OutputDir, currentFileRec.filename), content);
                        // print PDF
                        PrintPdfYear(calendar, year, Path.Combine(OutputDir, currentFileRec.filenamePdf));

                        GCDisplaySettings.Current = stateDetailedText;
                        calendar = new TResultCalendar(loc.GetLocationRef(), year);
                        content = calendar.formatText(GCDataFormat.HTML);
                        // print detailed TXT
                        File.WriteAllText(Path.Combine(OutputDir, currentFileRec.filenameDetail), content);

                    }

                    // write location index file
                    if (StartYear != EndYear)
                    {
                        File.WriteAllText(Path.Combine(OutputDir, ToFilePart(loc.CityName) + ".html"), GenerateYearIndex(loc, StartYear, EndYear));
                    }

                }

                for (int year = StartYear; year <= EndYear; year++)
                {
                    // write main index file
                    File.WriteAllText(Path.Combine(OutputDir, "y" + year.ToString() + ".html"), GenerateMainIndex(countries, year, year));
                }

                // write main index file
                File.WriteAllText(Path.Combine(OutputDir, "index.html"), GenerateMainIndex(countries, StartYear, EndYear));

                // write main years file
                //File.WriteAllText(Path.Combine(OutputDir, "years.html"), GenerateYearsOverview(StartYear, EndYear));

                progressBar1.Invoke(new SetProgressValue(SetDialogCompleted), 0);

            }
            catch(Exception ex)
            {
                Debugger.Log(0, "", "Error: " + ex.Message + "\n" + ex.StackTrace + "\n");
            }
            finally
            {
                GCDisplaySettings.Current = stateOrig;
            }
        }

        public string GenerateMainIndex(HashSet<string> countries, int sy, int ey)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><head></title>Vaisnava calendars</title></head>");
            sb.Append("<body>");

            if (sy != ey)
            {
                sb.AppendLine("<h1>All Years</h1>");
                sb.AppendLine("<p>");
                for (int y = sy; y <= ey; y++)
                {
                    if (y > sy)
                        sb.AppendLine(" | ");
                    sb.AppendFormat("<a href=\"{1}\">{0}</a> ", y, "y" + y.ToString() + ".html");
                }
                sb.AppendLine("</p>");
                sb.AppendLine("<hr>");
            }

            foreach (string s in countries)
            {
                sb.AppendLine("<h1>" + s + "</h1>");
                sb.AppendLine("<hr>");
                foreach (TLocation loc in SelectedLocations)
                {
                    if (loc.Country.Name.Equals(s))
                    {
                        if (sy == ey)
                        {
                            sb.Append("<p>");
                            sb.AppendFormat("<a href=\"{1}\">{0} {2}</a> ", loc.CityName,
                                sy.ToString() + "_" + ToFilePart(loc.CityName) + ".html", sy);
                            sb.AppendFormat(" | <a href=\"{1}\">{0} {2} (Full)</a>", loc.CityName,
                                sy.ToString() + "_" + ToFilePart(loc.CityName) + "_d.html", sy);
                            sb.AppendFormat(" | <a href=\"{1}\">{0} {2} (PDF)</a>", loc.CityName,
                                sy.ToString() + "_" + ToFilePart(loc.CityName) + ".pdf", sy);
                            sb.AppendFormat(" {0} {1}", GCEarthData.GetTextLatitude(loc.Latitude), GCEarthData.GetTextLongitude(loc.Longitude));
                            sb.Append("</p>\n");
                        }
                        else
                        {
                            sb.AppendFormat("<p><a href=\"{1}\">{0} {2}-{3}</a> {4} {5}</p>\n",
                                loc.CityName, ToFilePart(loc.CityName) + ".html", sy, ey,
                                GCEarthData.GetTextLatitude(loc.Latitude), GCEarthData.GetTextLongitude(loc.Longitude));
                        }
                    }
                }
            }

            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public string GenerateYearIndex(TLocation loc, int sy, int ey)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><head></title>Vaisnava calendars</title></head>");
            sb.Append("<body>");

            sb.AppendLine("<h1>" + loc.CityName + " (" + loc.Country.Name + ")" + "</h1>");
            sb.AppendFormat("<p>{0} {1}</p>", GCEarthData.GetTextLatitude(loc.Latitude), GCEarthData.GetTextLongitude(loc.Longitude));
            sb.AppendLine("<hr>");
            sb.AppendLine("<h2>Text (Brief)</h2>");
            sb.AppendLine("<p>");
            for (int y = sy; y <= ey; y++)
            {
                if (y > sy)
                    sb.AppendLine(" | ");
                sb.AppendFormat("<a href=\"{1}\">{0}</a> ", y, y.ToString() + "_" + ToFilePart(loc.CityName) + ".html", sy);
            }
            sb.AppendLine("</p>");
            sb.AppendLine("<hr>");
            sb.AppendLine("<h2>Text (Full)</h2>");
            sb.AppendLine("<p>");
            for (int y = sy; y <= ey; y++)
            {
                if (y > sy)
                    sb.AppendLine(" | ");
                sb.AppendFormat("<a href=\"{1}\">{0}</a> ", y, y.ToString() + "_" + ToFilePart(loc.CityName) + "_d.html", sy);
            }
            sb.AppendLine("</p>");
            sb.AppendLine("<hr>");
            sb.AppendLine("<h2>PDF</h2>");
            sb.AppendLine("<p>");
            for (int y = sy; y <= ey; y++)
            {
                if (y > sy)
                    sb.AppendLine(" | ");
                sb.AppendFormat("<a href=\"{1}\">{0}</a> ", y, y.ToString() + "_" + ToFilePart(loc.CityName) + ".pdf", sy);
            }
            sb.AppendLine("</p>");

            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        /// <summary>
        /// This generates 
        /// </summary>
        /// <param name="sy"></param>
        /// <param name="ey"></param>
        /// <returns></returns>
        public string GenerateYearsOverview(int sy, int ey)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><head></title>Vaisnava calendars</title></head>");
            sb.Append("<body>");

            sb.AppendLine("<h1>All Years</h1>");
            sb.AppendLine("<hr>");
            sb.AppendLine("<p>");

            for (int y = sy; y <= ey; y++)
            {
                if (y > sy)
                    sb.AppendLine(" | ");
                sb.AppendFormat("<a href=\"{1}\">{0}</a> ", y, "y" + y.ToString() + ".html");
            }

            sb.AppendLine("</p>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public delegate void SetProgressValue(int val);

        public void SetLocationsProgressValue(int l)
        {
            progressBar1.Value = l;
        }

        public void SetYearsProgressvalue(int i)
        {
            progressBar2.Value = i;
        }

        public void SetDialogCompleted(int i)
        {
            progressBar1.Value = progressBar1.Maximum;
            progressBar2.Visible = false;
            button1.Text = "Exit";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public void PrintPdfYear(GCLocation loc, int year, string fileName)
        {
            GCPrintService.ExportTableCalendar(loc, year, year, fileName);
        }

        public void PrintPdfYear(TResultCalendar loc, int year, string fileName)
        {
            GCPrintService.ExportTableCalendar(loc, year, year, fileName);
        }

    }
}
