using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Views;
using GCAL.MapCalculator;

namespace GCAL
{
    public partial class DlgCalcEkadasiBoundaries : Form, EventMapOverlayViewDataSource
    {
        public GregorianDateTime SelectedDate;
        public VAISNAVADAY SelectedVaisnavaDay;
        public GCMap SelectedMap;
        private bool stopRequested = false;
        public QuadrantArray RootArray = null;
        public bool CalculationFinished = false;

        /// <summary>
        /// 0 - none
        /// 1 - Ekadasi fast
        /// </summary>
        public int FindType = 0;

        public DlgCalcEkadasiBoundaries(GregorianDateTime sd)
        {
            InitializeComponent();

            eventMapOverlayView1.DataSource = this;

            SelectedDate = sd;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSaveImage_Click(object sender, EventArgs e)
        {
            Bitmap bmp = eventMapOverlayView1.ExportBitmap();
            if (bmp != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG Images (*.png)|*.png";
                sfd.FileName = SelectedMap.Title +  " " + SelectedDate.ToString() + ".png";
                sfd.DefaultExt = ".ext";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, ImageFormat.Png);
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopRequested = true;
            buttonStop.Enabled = false;
        }

        private void PerformeStop()
        {
            timerMapRefresh.Stop();
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            button1.Enabled = true;
            stopRequested = false;
            eventMapOverlayView1.CalculationFinished = true;
            eventMapOverlayView1.Invalidate();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (SelectedDate == null || SelectedVaisnavaDay == null || FindType < 1)
                return;

            StartMethodDelegate startMethod = new StartMethodDelegate(FunctionCalcBoundariesSync);

            Quadrant q = new Quadrant(SelectedMap);
            q.Trim();

            startMethod.BeginInvoke(q, SelectedDate, null, null);

            timerMapRefresh.Start();
            stopRequested = false;
            buttonStop.Enabled = true;
            buttonStart.Enabled = false;
            button1.Enabled = false;
        }

        private int calculateSplits(double range, double pixels)
        {
            int a = Convert.ToInt32(Math.Abs(range) / 5.0);
            return (a < 1) ? (pixels < 3 ? 1 : 2) : a;
        }

        List<QuandrantResult> resultsList = new List<QuandrantResult>();
        Dictionary<string, QuandrantResult> results = new Dictionary<string, QuandrantResult>();
        Color[] result_colors = new Color[] { Color.FromArgb(30, 255, 0, 0), Color.FromArgb(30, 0, 0, 255), Color.FromArgb(30, 0, 255, 0),
            Color.FromArgb(30,255,255,0)};

        private QuandrantResult GetSafeResult(VAISNAVADAY day)
        {
            string key = day.date.ToString();
            if (results.ContainsKey(key))
                return results[key];

            QuandrantResult qr = new QuandrantResult();
            qr.day = day;
            qr.ResultId = results.Count;
            qr.color = result_colors[qr.ResultId];

            results[key] = qr;
            resultsList.Add(qr);

            return qr;
        }

        public void RefreshPresentation()
        {
            eventMapOverlayView1.Invalidate();
        }

        private void FunctionCalcBoundariesSync(Quadrant map, GregorianDateTime date)
        {
            GCLog.Write("Ekadasi Boundaries Calculation START");
            RootArray = null;
            CalculationFinished = false;

            GregorianDateTime dt = new GregorianDateTime(date);
            dt.PreviousDay();
            dt.PreviousDay();

            GCLocation loc = new GCLocation();
            TTimeZone tz = new TTimeZone();
            loc.Latitude = 0;
            loc.Longitude = 0;
            loc.TimeZone = tz;

            TResultCalendar cal = new TResultCalendar();
            cal.EkadasiOnly = true;

            List<Quadrant> quandrantList = new List<Quadrant>();
            quandrantList.Add(map);
            QuadrantArray qa;
            map = null;
            int count = 0, maxCount = 2000;
            while(quandrantList.Count > 0)
            {
                map = quandrantList[0];
                quandrantList.RemoveAt(0);
                if (stopRequested)
                    break;
                map.Details = CalculateQuadrantArray(map, dt, loc, cal, quandrantList);
                if (RootArray == null)
                    RootArray = map.Details;

                // for testing
                count++;
                if (count >= maxCount)
                    break;
            }

            CalculationFinished = true;

            GCLog.Write("Ekadasi Boundaries Calculation END");

            StopMethodDelegate stopMethod = new StopMethodDelegate(PerformeStop);
            this.Invoke(stopMethod);
        }

        public delegate void StopMethodDelegate();
        public delegate void StartMethodDelegate(Quadrant map, GregorianDateTime date);

        public VAISNAVADAY GetCalcDay(GregorianDateTime dt, GCLocation loc, TResultCalendar cal, double longitude, double latitude)
        {
            loc.SetCoordinates(longitude, latitude);
            cal.CalculateCalendar(loc, dt, 5);
            for (int i = 0; i < 5; i++)
            {
                VAISNAVADAY vd = cal.GetDay(i);
                if (vd.nFastID == FastType.FAST_EKADASI)
                {
                    return vd;
                }
            }

            return null;
        }
        private QuadrantArray CalculateQuadrantArray(Quadrant map, GregorianDateTime dt, GCLocation loc, TResultCalendar cal, List<Quadrant> qlist)
        {
            QuadrantArray qa = new QuadrantArray(map, calculateSplits(map.LongitudeRange, map.pixelsX), calculateSplits(map.LatitudeRange, map.pixelsY));
            for (int lo = 0; lo < qa.Longitudes.Length; lo++)
            {
                for (int la = 0; la < qa.Latitudes.Length; la++)
                {
                    if (qa.Points[lo, la].Result != null)
                        continue;
                    VAISNAVADAY vd = GetCalcDay(dt, loc, cal, qa.Longitudes[lo], qa.Latitudes[la]);
                    /*                    loc.SetCoordinates(qa.Longitudes[lo], qa.Latitudes[la]);
                                        cal.CalculateCalendar(loc, dt, 5);
                                        for (int i = 0; i < 5; i++)
                                        {
                                            VAISNAVADAY vd = cal.GetDay(i);
                                            if (vd.nFastID == FastType.FAST_EKADASI)
                                            {
                                                qa.Points[lo, la].Result = GetSafeResult(vd);
                                            }
                                        }*/
                    if (vd != null)
                        qa.Points[lo, la].Result = GetSafeResult(vd);
                }
            }

            for (int lo = 0; lo < qa.Quadrants.GetLength(0); lo++)
            {
                for (int la = 0; la < qa.Quadrants.GetLength(1); la++)
                {
                    if (qa.Quadrants[lo, la].ResultState == QuadrantResultState.Decomposable)
                    {
                        qlist.Add(qa.Quadrants[lo, la]);
                    }
                }
            }

            return qa;
        }

        public GCMap EMOV_GetMap()
        {
            return SelectedMap;
        }

        public QuadrantArray EMOV_GetOverlay()
        {
            return RootArray;
        }

        public List<QuandrantResult> EMOV_GetResults()
        {
            return resultsList;
        }

        private void timerMapRefresh_Tick(object sender, EventArgs e)
        {
            eventMapOverlayView1.CalculationFinished = CalculationFinished;
            eventMapOverlayView1.Invalidate();
        }

        private void eventMapOverlayView1_OnClick(object o, EventmapOverlayEventArgs e)
        {
            GregorianDateTime dt = new GregorianDateTime(SelectedDate);
            dt.PreviousDay();
            dt.PreviousDay();

            GCLocation loc = new GCLocation();
            TTimeZone tz = new TTimeZone();
            loc.Latitude = 0;
            loc.Longitude = 0;
            loc.TimeZone = tz;

            TResultCalendar cal = new TResultCalendar();
            cal.EkadasiOnly = true;

            VAISNAVADAY vd = GetCalcDay(dt, loc, cal, e.Longitude, e.Latitude);

            string str =  string.Format("{0}, sunrise: {1}, longitude: {2}, latitude: {3}"
                , vd.date.ToString(), vd.astrodata.sunRise.ToLongTimeString()
                , e.Longitude, e.Latitude);

            labelStatus.Text = str;


        }
    }







}
