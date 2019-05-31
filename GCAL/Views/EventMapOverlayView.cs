using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.MapCalculator;

namespace GCAL.Views
{
    public partial class EventMapOverlayView : UserControl
    {
        public EventMapOverlayViewDataSource DataSource { get; set; }

        public Bitmap overlayBitmap = null;
        public Graphics overlayGraphics = null;
        public Rectangle globalMapArea;

        public event EventMapOverlayViewClickDelegate OnClick;

        public bool CalculationFinished = false;

        public EventMapOverlayView()
        {
            InitializeComponent();
        }

        public Dictionary<int, Brush> brushes = new Dictionary<int, Brush>();
        private Font legendFont = new Font("Arial", 10);
        public Brush getBrush(Color c)
        {
            int key = c.ToArgb();
            if (brushes.ContainsKey(key))
                return brushes[key];

            brushes[key] = new SolidBrush(c);
            return brushes[key];
        }

        private void EventMapOverlayView_Paint(object sender, PaintEventArgs earg)
        {
            if (DataSource == null) return;

            // paint map, margin, legend
            Size clientArea = this.Size;
            Graphics g = earg.Graphics;

            DrawmapWithOverlay(clientArea, g);

        }


        public Bitmap ExportBitmap()
        {
            if (DataSource == null) return null;
            GCMap map = DataSource.EMOV_GetMap();

            Size clientArea = new Size(map.ImageSize.Width + 20, map.ImageSize.Height + 60);
            Bitmap bmp = new Bitmap(clientArea.Width, clientArea.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, map.ImageSize.Width + 20, map.ImageSize.Height + 60);
            DrawmapWithOverlay(clientArea, g);

            return bmp;
        }

        private void DrawmapWithOverlay(Size clientArea, Graphics g)
        {
            GCMap map = DataSource.EMOV_GetMap();

            map.Preparefactors();

            // insufficient dimensions
            if (clientArea.Width < 50 || clientArea.Height < 50 || map.ImageSize.Width < 50 || map.ImageSize.Height < 50)
                return;

            Size clientAreaWithoutLegend = new Size(clientArea.Width - 10, clientArea.Height - 50);
            double ratioImageToScreen = Math.Min((double)clientAreaWithoutLegend.Width / map.ImageSize.Width,
                (double)clientAreaWithoutLegend.Height / map.ImageSize.Height);
            clientAreaWithoutLegend = new Size(Convert.ToInt32(ratioImageToScreen * map.ImageSize.Width),
                Convert.ToInt32(ratioImageToScreen * map.ImageSize.Height));

            Rectangle mapArea = new Rectangle(clientArea.Width / 2 - clientAreaWithoutLegend.Width / 2, 10,
                clientAreaWithoutLegend.Width, clientAreaWithoutLegend.Height);
            Rectangle mapAreaBitmap = new Rectangle(0, 0,
                clientAreaWithoutLegend.Width, clientAreaWithoutLegend.Height);

            globalMapArea = mapArea;

            g.DrawImage(map.Image, mapArea);

            QuadrantArray qa = DataSource.EMOV_GetOverlay();
            if (qa != null)
            {
                if (CalculationFinished)
                {
                    overlayBitmap = null;
                    if (overlayBitmap == null)
                    {
                        overlayBitmap = new Bitmap(mapArea.Width + 1, mapArea.Height + 1, PixelFormat.Format32bppArgb);
                        overlayGraphics = Graphics.FromImage(overlayBitmap);
                    }
                    // e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    // e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
                    //overlayGraphics.FillRectangle(getBrush(Color.Transparent), 0, 0, mapArea.Width, mapArea.Height);
                    DrawQuandrantOverlay(overlayGraphics, map, mapAreaBitmap, qa);
                    g.DrawImage(overlayBitmap, mapArea);
                }
                else
                {
                    DrawQuandrantOverlay(g, map, mapArea, qa);
                }
                //g.DrawRectangle(Pens.Red, mapArea);
            }

            DrawLongitudeLine(g, 0, map, mapArea);
            DrawLatitudeLine(g, 0, map, mapArea);
            DrawLatitudeLine(g, -66, map, mapArea);
            DrawLatitudeLine(g, 66, map, mapArea);

            List<QuandrantResult> results = DataSource.EMOV_GetResults();
            for (int i = 0; i < results.Count; i++)
            {
                int y = clientArea.Height - results.Count*20 - 4 + i * 20;
                Color c = results[i].color;
                g.FillRectangle(getBrush(Color.FromArgb(255, c)), mapArea.Left, y, 20, 15);
                g.FillRectangle(getBrush(results[i].color), mapArea.Left, y, 20, 15);
                g.DrawRectangle(Pens.Black, mapArea.Left, y, 20, 15);
                g.DrawString(results[i].ToString(), legendFont, Brushes.Black, mapArea.Left + 40, y);
            }
        }

        private void DrawQuandrantOverlay(Graphics g, GCMap map, Rectangle mapArea, QuadrantArray qa)
        {

            for (int x = 0; x < qa.Quadrants.GetLength(0); x++)
            {
                for (int y = 0; y < qa.Quadrants.GetLength(1); y++)
                {
                    double longitude = qa.Longitudes[x];
                    double latitude = qa.Latitudes[y];
                    double longitude2 = qa.Longitudes[x + 1];
                    double latitude2 = qa.Latitudes[y + 1];

                    int x0, y0, x1, y1;

                    x0 = map.LongitudeToX(longitude, ref mapArea);
                    y0 = map.LatitudeToY(latitude, ref mapArea);
                    x1 = map.LongitudeToX(longitude2, ref mapArea);
                    y1 = map.LatitudeToY(latitude2, ref mapArea);

                    Quadrant q = qa.Quadrants[x, y];

                    //g.DrawString(q.ResultState.ToString(), SystemFonts.MenuFont, Brushes.Black, (x0 + x1) / 2, (y0 + y1) / 2);
                    switch (q.ResultState)
                    {
                        case QuadrantResultState.Consistent:
                            Color c = q.WS.Result.color;
                            g.FillRectangle(getBrush(c), x0, y1, x1 - x0, y0 - y1);
                            break;
                        case QuadrantResultState.Inconsistent:
                            if (q.Details != null)
                            {
                                DrawQuandrantOverlay(g, map, mapArea, q.Details);
                            }
                            break;
                        case QuadrantResultState.Decomposable:
                            if (q.Details != null)
                            {
                                DrawQuandrantOverlay(g, map, mapArea, q.Details);
                            }
                            break;
                        case QuadrantResultState.NotAvailable:
                            break;
                    }
                }
            }
        }

        double ax, bx;
        double ay, by;

        private void DrawLatitudeLine(Graphics g, double latitude, GCMap map, Rectangle mapArea)
        {
            int ay0 = map.LatitudeToY(latitude, ref mapArea);
            if (ay0 > mapArea.Top && ay0 < mapArea.Bottom)
                g.DrawLine(Pens.Red, mapArea.Left, ay0, mapArea.Right, ay0);
        }

        private void EventMapOverlayView_MouseClick(object sender, MouseEventArgs e)
        {
            EventmapOverlayEventArgs args = new EventmapOverlayEventArgs();

            args.ClientX = e.X;
            args.ClientY = e.Y;

            if (DataSource == null) return;
            GCMap map = DataSource.EMOV_GetMap();

            args.Longitude = map.XtoLongitude(e.X, globalMapArea);
            args.Latitude = map.YtoLatitude(e.Y, globalMapArea);

            if (OnClick != null)
                OnClick(this, args);
        }

        private void DrawLongitudeLine(Graphics g, double longitude, GCMap map, Rectangle mapArea)
        {
            int ax0 = map.LongitudeToX(longitude, ref mapArea);
            if (ax0 > mapArea.Left && ax < mapArea.Right)
                g.DrawLine(Pens.Red, ax0, mapArea.Top, ax0, mapArea.Bottom);
        }

        private void EventMapOverlayView_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
            overlayBitmap = null;
            overlayGraphics = null;
        }
    }

    public interface EventMapOverlayViewDataSource
    {
        GCMap EMOV_GetMap();
        QuadrantArray EMOV_GetOverlay();
        List<QuandrantResult> EMOV_GetResults();
    }

    public class EventmapOverlayEventArgs: EventArgs
    {
        public double Latitude;
        public double Longitude;
        public int ClientX;
        public int ClientY;
    }

    public delegate void EventMapOverlayViewClickDelegate(object o, EventmapOverlayEventArgs e);

}
