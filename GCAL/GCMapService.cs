using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

using GCAL.Base;

namespace GCAL
{
    public class GCMapService
    {
        public static List<GCMap> Maps = new List<GCMap>();

        public static void OnStart()
        {
            string rootFile = GCGlobal.GetFileName(GCGlobal.MapsFolderPath, "root.xml");
            if (File.Exists(rootFile))
            {
                Maps.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(rootFile);
                foreach(XmlNode mapXmlNode in doc.GetElementsByTagName("maprec"))
                {
                    if (mapXmlNode is XmlElement mapXmlElement)
                    {
                        GCMap map = new GCMap();
                        map.Id = new Guid(mapXmlElement.GetAttribute("guid"));
                        map.ImageFilePath = mapXmlElement.GetAttribute("filePath");
                        map.Title = mapXmlElement.GetAttribute("title");
                        map.ImageSize = new Size(int.Parse(mapXmlElement.GetAttribute("sizex")), int.Parse(mapXmlElement.GetAttribute("sizey")));
                        map.LatitudeEnd = double.Parse(mapXmlElement.GetAttribute("latend"));
                        map.LatitudeStart = double.Parse(mapXmlElement.GetAttribute("latstart"));
                        map.LongitudeEnd = double.Parse(mapXmlElement.GetAttribute("longend"));
                        map.LongitudeStart = double.Parse(mapXmlElement.GetAttribute("longstart"));
                        map.MapUsable = (int.Parse(mapXmlElement.GetAttribute("usable")) == 1);
                        foreach (XmlElement anchorXmlNode in mapXmlElement.GetElementsByTagName("anchor"))
                        {
                            GCMapAnchor anchorPoint = new GCMapAnchor();
                            anchorPoint.relX = double.Parse(anchorXmlNode.GetAttribute("relX"));
                            anchorPoint.relY = double.Parse(anchorXmlNode.GetAttribute("relY"));
                            anchorPoint.Location = anchorXmlNode.GetAttribute("title");
                            anchorPoint.Latitude = double.Parse(anchorXmlNode.GetAttribute("latitude"));
                            anchorPoint.Longitude = double.Parse(anchorXmlNode.GetAttribute("longitude"));
                            map.AnchorPoints.Add(anchorPoint);
                        }

                        map.RecalculateDimensions();
                        Maps.Add(map);
                    }
                }
            }
        }

        public static string GetNewFileName(ref int startIndex, string extension)
        {
            string dir = GCGlobal.MapsFolderPath;
            for(int i = startIndex; i < 100; i++)
            {
                string fileName = Path.Combine(dir, "file" + (i + startIndex) + extension);
                if (!File.Exists(fileName))
                {
                    startIndex = i + 1;
                    return fileName;
                }
            }

            return "file_last" + extension;
        }

        public static void OnSaveState()
        {
            int startIndex = 1;
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("maps");
            doc.AppendChild(root);
            XmlElement mapXmlElem;

            foreach(GCMap map in Maps)
            {
                mapXmlElem = doc.CreateElement("maprec");
                root.AppendChild(mapXmlElem);

                mapXmlElem.SetAttribute("guid", map.Id.ToString());
                if (!File.Exists(map.ImageFilePath))
                {
                    map.ImageFilePath = GetNewFileName(ref startIndex, ".png");
                    map.ImageChanged = true;
                }
                if (map.ImageChanged)
                {
                    map.Image.Save(map.ImageFilePath, ImageFormat.Png);
                    map.ImageChanged = false;
                }
                mapXmlElem.SetAttribute("filePath", map.ImageFilePath);
                mapXmlElem.SetAttribute("title", map.Title);
                mapXmlElem.SetAttribute("sizex", map.ImageSize.Width.ToString());
                mapXmlElem.SetAttribute("sizey", map.ImageSize.Height.ToString());
                mapXmlElem.SetAttribute("latend", map.LatitudeEnd.ToString());
                mapXmlElem.SetAttribute("latstart", map.LatitudeStart.ToString());
                mapXmlElem.SetAttribute("longend", map.LongitudeEnd.ToString());
                mapXmlElem.SetAttribute("longstart", map.LongitudeStart.ToString());
                mapXmlElem.SetAttribute("usable", map.MapUsable ? "1" : "0");
                foreach(GCMapAnchor mapAnchor in map.AnchorPoints)
                {
                    XmlElement mae = doc.CreateElement("anchor");
                    mapXmlElem.AppendChild(mae);

                    mae.SetAttribute("relX", mapAnchor.relX.ToString());
                    mae.SetAttribute("relY", mapAnchor.relY.ToString());
                    mae.SetAttribute("title", mapAnchor.Location);
                    mae.SetAttribute("latitude", mapAnchor.Latitude.ToString());
                    mae.SetAttribute("longitude", mapAnchor.Longitude.ToString());
                }
            }

            string rootFile = GCGlobal.GetFileName(GCGlobal.MapsFolderPath, "root.xml");
            doc.Save(rootFile);
        }
    }

    public class GCMap
    {
        public string Title = "";
        public Guid Id = Guid.Empty;

        public double LatitudeStart = 0.0;
        public double LatitudeEnd = 0.0;
        public double LongitudeStart = 0.0;
        public double LongitudeEnd = 0.0;

        public bool MapUsable = false;
        public string ImageFilePath = "";
        public bool ImageChanged = false;
        public Image Image = null;
        public Size ImageSize = Size.Empty;
        public List<GCMapAnchor> AnchorPoints = new List<GCMapAnchor>();

        public double ax, ay, bx, by;

        public void Preparefactors()
        {
            /*double rx1 = AnchorPoints[0].relX;
            double rx2 = AnchorPoints[1].relX;
            double lon1 = AnchorPoints[0].Longitude;
            double lon2 = AnchorPoints[1].Longitude;
            ax = (rx1 - rx2) / (lon1 - lon2);
            bx = rx1 - lon1 * ax;

            double ry1 = AnchorPoints[0].relY;
            double ry2 = AnchorPoints[1].relY;
            double lat1 = AnchorPoints[0].Latitude;
            double lat2 = AnchorPoints[1].Latitude;
            ay = (ry1 - ry2) / (lat1 - lat2);
            by = ry1 - lat1 * ay;*/

        }

        private double RelYToLatitude(double relY)
        {
            return (relY - by) / ay;
        }

        public int LatitudeToY(double latitude, ref Rectangle mapArea)
        {
            return mapArea.Top + Convert.ToInt32((ay * latitude + by) * mapArea.Height);
        }


        public int LongitudeToX(double longitude, ref Rectangle mapArea)
        {
            return mapArea.Left + Convert.ToInt32(mapArea.Width * (ax * longitude + bx));
        }

        public double LongitudeRange
        {
            get
            {
                return Math.Abs(LongitudeStart - LongitudeEnd);
            }
        }

        public double LatitudeRange
        {
            get
            {
                return Math.Abs(LatitudeStart - LatitudeEnd);
            }
        }


        public override string ToString()
        {
            return Title;
        }

        public void ResetDisplay()
        {
            foreach(GCMapAnchor ma in AnchorPoints)
            {
                ma.drawSelect = false;
            }
        }

        public void RecalculateDimensions()
        {
            MapUsable = false;
            if (AnchorPoints.Count < 2)
                return;

            double minLong = AnchorPoints[0].Longitude,
                maxLong = AnchorPoints[0].Longitude,
                minLat = AnchorPoints[0].Latitude,
                maxLat = AnchorPoints[0].Latitude;
            double minRelX = AnchorPoints[0].relX,
                maxRelX = AnchorPoints[0].relX,
                minRelY = AnchorPoints[0].relY,
                maxRelY = AnchorPoints[0].relY;

            foreach(GCMapAnchor ma in AnchorPoints)
            {
                minLong = Math.Min(minLong, ma.Longitude);
                maxLong = Math.Max(maxLong, ma.Longitude);
                minLat = Math.Min(minLat, ma.Latitude);
                maxLat = Math.Max(maxLat, ma.Latitude);
                minRelX = Math.Min(minRelX, ma.relX);
                maxRelX = Math.Max(maxRelX, ma.relX);
                minRelY = Math.Min(minRelY, ma.relY);
                maxRelY = Math.Max(maxRelY, ma.relY);
            }


            double dx = maxLong - minLong;
            double dy = maxLat - minLat;

            if (Math.Abs(dx) < 0.0001 || Math.Abs(dy) < 0.0001)
                return;

            if (dx / dy > 50 || dy / dx > 50)
                return;


            double ex = maxRelX - minRelX;
            double ey = maxRelY - minRelY;

            if (Math.Abs(ex) < 0.0001 || Math.Abs(ey) < 0.0001)
                return;

            if (ex / ey > 50 || ey / ex > 50)
                return;

            ax = ex / dx;
            bx = maxRelX - maxLong * ax;

            ay = (minRelY - maxRelY) / (maxLat - minLat);
            by = minRelY - maxLat * ay;




            double xdegsPerUnit = dx / ex;
            double ydegsPerUnit = dy / ey;

            LongitudeStart = minLong - xdegsPerUnit * minRelX;
            LongitudeEnd = maxLong + xdegsPerUnit * (1 - maxRelX);
            LatitudeStart = RelYToLatitude(1);
            LatitudeEnd = RelYToLatitude(0);

            MapUsable = true;
        }

        public double YtoLatitude(int y, Rectangle mapArea)
        {
            return ((double)(y - mapArea.Top) / mapArea.Height  - by) / ay;
        }

        public double XtoLongitude(int x, Rectangle mapArea)
        {
            return ((double)(x - mapArea.Left) / mapArea.Width - bx) / ax;
        }
    }

    public class GCMapAnchor
    {
        /// <summary>
        /// value 0.0 - 1.0 as relative X coordinate within image
        /// </summary>
        public double relX = 0.0;
        /// <summary>
        /// value 0.0 - 1.0 as relative Y coordinate within image
        /// </summary>
        public double relY = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public string Location = "";
        public double Longitude = 0.0;
        public double Latitude = 0.0;

        public Rectangle drawRect = Rectangle.Empty;
        public bool drawSelect = false;
    }
}
