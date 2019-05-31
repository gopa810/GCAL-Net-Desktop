using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Xml;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCGlobal
    {
        public static GCLocation myLocation = new GCLocation();

        public static List<GCLocation> recentLocations = new List<GCLocation>();

        public static GregorianDateTime dateTimeShown = new GregorianDateTime();

        public static GregorianDateTime dateTimeToday = new GregorianDateTime();

        public static GregorianDateTime dateTimeTomorrow = new GregorianDateTime();

        public static GregorianDateTime dateTimeYesterday = new GregorianDateTime();

        public static TLangFileList languagesList;

        public static string dialogLastRatedSpec = string.Empty;


        public static string GetFileName(string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        public static void CheckExistingFolders(params string[] folders)
        {
            foreach(string folder in folders)
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
        }

        public static int initFolders()
        {
            // main data folder
            LocalDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            LocalDataFolderPath = Path.Combine(LocalDataFolderPath, "GCAL");

            // init sub folders
            ConfigurationFolderPath = Path.Combine(LocalDataFolderPath, "config");
            LanguagesFolderPath = Path.Combine(LocalDataFolderPath, "lang");
            TemporaryFolderPath = Path.Combine(LocalDataFolderPath, "temp");
            CoreDataFolderPath = Path.Combine(LocalDataFolderPath, "cores");
            MapsFolderPath = Path.Combine(LocalDataFolderPath, "maps");

            CheckExistingFolders(LocalDataFolderPath, ConfigurationFolderPath, MapsFolderPath, LanguagesFolderPath, TemporaryFolderPath, CoreDataFolderPath);

            // set file names
            TipsFilePath = Path.Combine(ConfigurationFolderPath, "gcal_tips1.txt");

            ApplicationSettingsFilePath = Path.Combine(ConfigurationFolderPath, "appsets.xml");
            CountriesFilePath = Path.Combine(ConfigurationFolderPath, "countries2.xml");
            DisplaySettingsFilePath = Path.Combine(ConfigurationFolderPath, "dispsets.xml");
            LocationsFilePath = Path.Combine(ConfigurationFolderPath, "locations4.xml");
            StringsFilePath = Path.Combine(ConfigurationFolderPath, "strings3.xml");
            TimezonesFilePath = Path.Combine(ConfigurationFolderPath, "timezones2.xml");

            return 1;
        }

        public static string LocalDataFolderPath { get; set; }
        public static string ConfigurationFolderPath { get; set; }
        public static string LanguagesFolderPath { get; set; }
        public static string TemporaryFolderPath { get; set; }
        public static string CoreDataFolderPath { get; set; }
        public static string MapsFolderPath { get; set; }

        public static string ApplicationSettingsFilePath { get; set; }
        public static string DisplaySettingsFilePath { get; set; }
        public static string LocationsFilePath { get; set; }
        public static string TimezonesFilePath { get; set; }
        public static string CountriesFilePath { get; set; }
        public static string StringsFilePath { get; set; }
        public static string TipsFilePath { get; set; }


        public static void LoadApplicationSettings(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(fileName))
                return;

            doc.Load(fileName);


            XmlElement e1 = doc["AppSettings"];

            if (e1["MyLocation"] != null)
                myLocation.LoadFromNode(e1["MyLocation"]);

            recentLocations.Clear();
            foreach (XmlElement e2 in e1.GetElementsByTagName("RecentLocation"))
            {
                GCLocation loc = new GCLocation();
                loc.LoadFromNode(e2);
                recentLocations.Add(loc);
            }

            XmlElement e3 = e1["MainRectangle"];
            if (e3 != null && e3.HasAttribute("value"))
            {
                if (GCUserInterface.windowController != null)
                    GCUserInterface.windowController.ExecuteMessage("setMainRectangle", new GSString(e3.GetAttribute("value")));
            }

            e3 = e1["ShowMode"];
            if (e3 != null && e3.HasAttribute("value"))
                GCUserInterface.ShowMode = int.Parse(e3.GetAttribute("value"));

            e3 = e1["LayoutSize"];
            if (e3 != null && e3.HasAttribute("value"))
                GCLayoutData.LayoutSizeIndex = int.Parse(e3.GetAttribute("value"));

            e3 = e1["LastRatedSpec"];
            if (e3 != null && e3.HasAttribute("value"))
                dialogLastRatedSpec = e3.GetAttribute("value");

        }


        public static void SaveApplicationSettings(string fileName)
        {
            GSCore rcs = GCUserInterface.windowController.ExecuteMessage("getMainRectangle", (GSCore)null);

            XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("AppSettings");
            doc.AppendChild(e1);

            e1.AppendChild(doc.CreateElement("MyLocation"));
            myLocation.SaveToNode(e1["MyLocation"]);

            e1.AppendChild(doc.CreateElement("RecentLocations"));
            foreach (GCLocation loc in recentLocations)
            {
                XmlElement e2 = doc.CreateElement("RecentLocation");
                e1.AppendChild(e2);
                loc.SaveToNode(e2);
            }

            e1.AppendChild(doc.CreateElement("MainRectangle"));
            e1["MainRectangle"].SetAttribute("value", rcs.getStringValue());

            e1.AppendChild(doc.CreateElement("ShowMode"));
            e1["ShowMode"].SetAttribute("value", GCUserInterface.ShowMode.ToString());

            e1.AppendChild(doc.CreateElement("LayoutSize"));
            e1["LayoutSize"].SetAttribute("value", GCLayoutData.LayoutSizeIndex.ToString());

            e1.AppendChild(doc.CreateElement("LastRatedSpec"));
            e1["LastRatedSpec"].SetAttribute("value", dialogLastRatedSpec);

            doc.Save(fileName);
        }


        public static bool GetLangFileForAcr(string pszAcr, out string strFile)
        {
            foreach (TLangFileInfo p in languagesList.list)
            {
                if (p.m_strAcr.Equals(pszAcr, StringComparison.CurrentCultureIgnoreCase))
                {
                    strFile = p.m_strFile;
                    return true;
                }
            }
            strFile = null;
            return false;
        }


        public static void LoadInstanceData()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            // initialization for AppDir
            initFolders();

            // initialization of global strings
            GCStrings.LoadStringsFile(StringsFilePath);

            // inicializacia timezones
            TTimeZone.LoadFile(TimezonesFilePath);

            // inicializacia countries
            TCountry.LoadFile(CountriesFilePath);

            // inicializacia miest a kontinentov
            // lazy loading of data
            TLocationDatabase.FileName = LocationsFilePath;
            //CLocationList.OpenFile();

            // inicializacia zobrazovanych nastaveni
            GCDisplaySettings.LoadDisplaySettingsFile(DisplaySettingsFilePath);

            // inicializacia custom events
            GCFestivalBookCollection.OpenFile(ConfigurationFolderPath);

            // looking for files *.recn
            GCConfigRatedManager.RefreshListFromDirectory(ConfigurationFolderPath);

            // initialization of global variables
            myLocation.EncodedString = GCLocation.DefaultEncodedString;

            recentLocations.Add(new GCLocation(myLocation));

            LoadApplicationSettings(ApplicationSettingsFilePath);
            // refresh fasting style after loading user settings
            //GCFestivalBook.SetOldStyleFasting(GCDisplaySettings.Current.getValue(42));

            // inicializacia tipov dna
            if (!File.Exists(TipsFilePath))
            {
                File.WriteAllText(TipsFilePath, Properties.Resources.tips);
            }

        }


        public static void SaveInstanceData()
        {
            SaveApplicationSettings(ApplicationSettingsFilePath);

            if (TLocationDatabase.Modified)
            {
                TLocationDatabase.SaveFile(LocationsFilePath);
            }

            if (TCountry.IsModified())
            {
                TCountry.SaveFile(CountriesFilePath);
            }

            if (GCStrings.gstr_Modified)
            {
                GCStrings.SaveStringsFile(StringsFilePath);
            }

            GCDisplaySettings.SaveDisplaySettingsFile(DisplaySettingsFilePath);

            GCFestivalBookCollection.SaveAllChangedFestivalBooks(ConfigurationFolderPath);

            if (TTimeZone.Modified)
            {
                TTimeZone.SaveFile(TimezonesFilePath);
            }

        }


        public static void AddRecentLocation(GCLocation cLocationRef)
        {
            int rl = recentLocations.IndexOf(cLocationRef);
            if (rl != 0)
            {
                if (rl > 0)
                {
                    recentLocations.RemoveAt(rl);
                }
                recentLocations.Insert(0, cLocationRef);
            }
        }

        public static GCLocation LastLocation
        {
            get
            {
                if (recentLocations.Count == 0)
                    return myLocation;
                return recentLocations[0];
            }
        }
    }
}
