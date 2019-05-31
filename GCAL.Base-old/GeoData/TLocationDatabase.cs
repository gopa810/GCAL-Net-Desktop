using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace GCAL.Base
{
    public class TLocationDatabase
    {
        private static List<TLocation> locationList = null;

        /// <summary>
        /// This should be initialized as soon as possible, 
        /// because this will be used for lazy loading of database
        /// </summary>
        public static string FileName { get; set; }

        /// <summary>
        /// List of all built-in locations
        /// </summary>
        public static List<TLocation> LocationList
        {
            get
            {
                if (locationList != null) return locationList;
                LoadFile(FileName, true);
                return locationList;
            }
        }

        /// <summary>
        /// Flag denoting modification of database and the need of saving it to storage
        /// </summary>
        public static bool Modified { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public TLocationDatabase()
        {
            Modified = true;
        }

        /// <summary>
        /// Loading file in the format, where values are separated with TAB character
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="flagStandardFile">TRUE if loaded file is standard one, FALSE if loaded file is imported external file</param>
        /// <returns>TRUE if file was successfuly loaded, FALSE if the file was not loaded</returns>
        public static bool LoadFile(string fileName, bool flagStandardFile)
        {
            locationList = new List<TLocation>();

            if (!File.Exists(fileName))
            {
                if (flagStandardFile)
                    File.WriteAllText(fileName, Properties.Resources.cities2016);
                else
                    return false;
            }

            using(StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("#")) continue;
                    TLocation loc = new TLocation(line);
                    if (loc.Valid)
                        locationList.Add(loc);
                }
            }

            return true;
        }

        public static TLocation FindLocation(string v)
        {
            foreach(TLocation tl in LocationList)
            {
                if (tl.CityName.Equals(v))
                    return tl;
            }
            return null;
        }

        public static void SaveFile(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (TLocation lc in LocationList)
                {
                    sw.WriteLine(lc.EncodedString);
                }
            }
        }

        /// <summary>
        /// This function produces 1.5 times bigger file than SaveFile 
        /// and loading is at least 2 times slower
        /// </summary>
        /// <param name="lpszFileName"></param>
        public static void SaveFileXml(string lpszFileName)
        { 
            XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("Locations");
            doc.AppendChild(e1);
            foreach (TLocation lc in LocationList)
            {
                XmlElement e = doc.CreateElement("Location");
                e1.AppendChild(e);
                lc.SaveToNode(e);
            }
            doc.Save(lpszFileName);
        }

        /// <summary>
        /// Importing new file into database
        /// </summary>
        /// <param name="fileName">Path for importing file</param>
        /// <param name="flagDeleteCurrent">Flag for replacing of database or adding to database. Value TRUE means existing record are deleted.</param>
        /// <returns>Success of loading the specified file</returns>
        public static bool ImportFile(string fileName, bool flagDeleteCurrent)
        {
            if (flagDeleteCurrent)
            {
                locationList.Clear();
            }

            Modified = true;

            return LoadFile(fileName, false);
        }

        /// <summary>
        /// Reseting user database to the built-in one
        /// </summary>
        public static void SetDefaultDatabase()
        {
            File.Delete(FileName);
            LoadFile(FileName, true);
            Modified = true;
        }

    }
}
