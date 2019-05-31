using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCConfigRatedManager
    {
        public static List<GCConfigRatedEvents> Configurations = new List<GCConfigRatedEvents>();
        private static string directoryPath = "";

        public static void RefreshListFromDirectory(string folder)
        {
            Configurations.Clear();
            directoryPath = folder;
            foreach (string file in Directory.EnumerateFiles(folder))
            {
                if (file.EndsWith(".recn"))
                {
                    GCConfigRatedEvents gc = new GCConfigRatedEvents();
                    gc.Load(Path.Combine(folder, file));
                    Configurations.Add(gc);
                }
            }

            if (Configurations.Count < 1)
            {
                Configurations.Add(DefaultConfiguration);
                DefaultConfiguration.Save(Path.Combine(folder, "Default.recn"));
            }
        }

        private static GCConfigRatedEvents p_default = null;

        /// <summary>
        /// Default configuration (showing sandhya vandanam times)
        /// </summary>
        public static GCConfigRatedEvents DefaultConfiguration
        {
            get
            {
                if (p_default == null)
                {
                    p_default = new GCConfigRatedEvents();
                    p_default.Title = "<Default>";
                    p_default.Description = "Default configuration for rated events. This is only for demo purposes. It gives preffered daily times for sandhya vandanam.";
                    p_default.rateDayHours[0].AddMargin("Pratah Sandhyam", -1440, 1440, 1.0);
                    p_default.rateDayHours[1].AddMargin("Madhyahnikam", -1440, 1440, 1.0);
                    p_default.rateDayHours[2].AddMargin("Sayam Sandhyam", -1440, 1440, 1.0);
                }

                return p_default;
            }
        }

        public static GCConfigRatedEvents GetConfiguration(string s)
        {
            if (s == null)
                return DefaultConfiguration;
            foreach (GCConfigRatedEvents ge in Configurations)
            {
                if (ge.Title.Equals(s))
                    return ge;
            }

            return DefaultConfiguration;
        }

        public static int DeleteConfiguration(GCConfigRatedEvents gCConfigRatedEvents)
        {
            int i = Configurations.IndexOf(gCConfigRatedEvents);
            if (i >= 0 && i < Configurations.Count)
            {
                Configurations.RemoveAt(i);
            }

            return i;
        }

        /// <summary>
        /// - save to disk
        /// - add to configurations
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static GCConfigRatedEvents CreateConfiguration(string title)
        {
            GCConfigRatedEvents ne = new GCConfigRatedEvents();
            ne.Title = title;
            bool notFound = true;
            int i = 0;
            string file = "";
            string fileName = null;
            while (notFound)
            {
                file =  string.Format("ratev{0:000}", i++);
                fileName = Path.Combine(directoryPath, file);
                notFound = ExistsFile(fileName);
            }

            ne.Save(fileName);

            Configurations.Add(ne);

            return ne;
        }

        public static bool ExistsFile(string file)
        {
            foreach(GCConfigRatedEvents ge in Configurations)
            {
                if (ge.FileName.Equals(file))
                    return true;
            }
            return false;
        }
    }
}
