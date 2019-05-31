using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.IO;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalBookCollection
    {
        public static bool BooksModified = false;

        public static int BookCollectionIdCounter = 0;

        public static List<GCFestivalBook> Books = new List<GCFestivalBook>();

        public static string RootFilePath = string.Empty;

        public static GCFestivalBook getSafeBook(int i)
        {
            foreach (GCFestivalBook book in Books)
            {
                if (book.CollectionId == i)
                    return book;
            }

            GCFestivalBook newBook = new GCFestivalBook();
            newBook.CollectionId = i;
            newBook.FileName = string.Format("events{0:00}.ev.rl", i);
            Books.Add(newBook);

            // check NextCollectionId
            BookCollectionIdCounter = Math.Max(i + 1, BookCollectionIdCounter);

            return newBook;
        }

        public static GCFestivalBook CreateBook(string bookName)
        {
            GCFestivalBook fb = getSafeBook(BookCollectionIdCounter);
            fb.CollectionName = bookName;
            SaveFestivalBook(GCGlobal.ConfigurationFolderPath, fb);
            return fb;
        }

        public static void MoveBook(int oldIndex, int newIndex)
        {
            GCFestivalBook fb = Books[oldIndex];
            Books.RemoveAt(oldIndex);
            Books.Insert(newIndex, fb);
            BooksModified = true;
        }

        public static void Export(string pszFile, int format)
        {
            String strc;

            using (StreamWriter f = new StreamWriter(pszFile))
            {
                foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
                {
                    switch (format)
                    {
                        case 1:
                            foreach (GCFestivalBase fb in book.Festivals)
                            {
                                if (fb is GCFestivalTithiMasa)
                                {
                                    GCFestivalTithiMasa pce = fb as GCFestivalTithiMasa;
                                    strc = string.Format("{0}\n\t{1} Tithi,{2} Paksa,{3} Masa\n", pce.Text,
                                        GCTithi.GetName(pce.nTithi), GCPaksa.GetName(pce.nTithi / 15), GCMasa.GetName(pce.nMasa));
                                    f.Write(strc);
                                    if (pce.FastID != 0)
                                    {
                                        strc = string.Format("\t{0}\n", GCStrings.GetFastingName(pce.FastID));
                                        f.Write(strc);
                                    }
                                }
                                else if (fb is GCFestivalSankranti)
                                {
                                    GCFestivalSankranti se = fb as GCFestivalSankranti;
                                    f.Write(string.Format("{0}\n\t\t{1} Sankranti, {2}\n", se.Text, GCRasi.GetName(se.RasiOfSun), se.DayOffset));
                                }
                            }

                            break;
                        case 2:
                            {
                                f.Write("<xml>\n");
                                foreach (GCFestivalBase fb in book.Festivals)
                                {
                                    f.Write("\t<event>\n");
                                    if (fb is GCFestivalTithiMasa)
                                    {
                                        GCFestivalTithiMasa pce = fb as GCFestivalTithiMasa;
                                        strc = string.Format("\t\t<name>{0}</name>\n", pce.Text);
                                        f.Write(strc);
                                        strc = string.Format("\t\t<tithi>{0}</tithi>\n", GCTithi.GetName(pce.nTithi));
                                        f.Write(strc);
                                        strc = string.Format("\t\t<paksa>{0}</paksa>\n", GCPaksa.GetName(pce.nTithi / 15));
                                        f.Write(strc);
                                        strc = string.Format("\t\t<masa>{0}</masa>\n", GCMasa.GetName(pce.nMasa));
                                        f.Write(strc);
                                    }
                                    else if (fb is GCFestivalSankranti)
                                    {
                                        GCFestivalSankranti se = fb as GCFestivalSankranti;
                                        f.Write("\t\t<name>" + se.Text + "</name>\n");
                                        f.Write("\t\t<sankranti>" + GCRasi.GetName(se.RasiOfSun) + "</sankranti>\n");
                                        f.Write("\t\t<rel>" + se.DayOffset + "</rel>\n");
                                    }

                                    if (fb.EventsCount > 0)
                                    {
                                        foreach (GCFestivalBase fb2 in fb.Events)
                                        {
                                            if (fb2 is GCFestivalRelated)
                                            {
                                                GCFestivalRelated rel = fb2 as GCFestivalRelated;
                                                f.Write("\t\t<relatedEvent>\n");
                                                f.Write("\t\t\t<name>" + rel.Text + "</name>\n");
                                                f.Write("\t\t\t<offset>" + rel.DayOffset + "</offset>\n");
                                                f.Write("\t\t</relatedEvent>\n");
                                            }
                                        }
                                    }
                                    
                                    f.Write("\t</event>\n");
                                }

                                f.Write("</xml>\n");
                            }
                            break;
                        case 3:
                            GSScript script = new GSScript();
                            script.Add(new GSToken("book"));
                            script.Add(new GSList(new GSToken("bookId"), new GSNumber(book.CollectionId)));
                            script.Add(new GSList(new GSToken("bookName"), new GSString(book.CollectionName)));
                            foreach (GCFestivalBase fb in book.Festivals)
                            {
                                script.Add(fb.ExecuteMessage("getScript"));
                            }
                            script.writeScript(f);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static bool HasFile(string fileNameOnly)
        {
            foreach(GCFestivalBook b in Books)
            {
                if (b.FileName.Equals(fileNameOnly))
                    return true;
            }
            return false;
        }

        public static List<string> ScanEventFiles(string dir)
        {
            List<string> files = new List<string>();
            RootFilePath = Path.Combine(dir, "root.ev.rl");
            if (File.Exists(RootFilePath))
            {
                files.Clear();
                foreach(string s in File.ReadAllLines(RootFilePath))
                {
                    files.Add(Path.Combine(dir, s));
                }
            }
            else
            {
                foreach (string fileName in Directory.EnumerateFiles(dir))
                {
                    if (fileName.EndsWith(".ev.rl"))
                        files.Add(fileName);
                }
            }
            return files;
        }

        public static void SaveRootFile()
        {
            int i = 0;
            string[] a = new string[Books.Count];
            foreach(GCFestivalBook fb in Books)
            {
                a[i] = fb.FileName;
                i++;
            }
            if (Directory.Exists(Path.GetDirectoryName(RootFilePath)))
            {
                File.WriteAllLines(RootFilePath, a);
            }
        }

        public static int OpenFile(string folderName)
        {
            int result = 0;
            List<string> files = ScanEventFiles(folderName);
            if (files.Count == 0)
            {
                ResetAllBooks(folderName);
                files = ScanEventFiles(folderName);
            }

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    OpenEventsFile(file);
                    result++;
                }
            }

            return result;
        }

        public static void ResetAllBooks(string folderName)
        {
            string path = Path.Combine(folderName, "temp.tmp1");
            File.WriteAllBytes(path, Properties.Resources.events);
            OpenEventsFile(path);
            foreach (GCFestivalBook b in Books)
                b.Changed = true;
            SaveAllChangedFestivalBooks(folderName);
            File.Delete(path);

            RootFilePath = Path.Combine(folderName, "root.ev.rl");
            SaveRootFile();
        }

        public static int OpenEventsFile(string pszFile)
        {
            int nMode = 0;
            int nRet = -1;
            StringBuilder sb = new StringBuilder();
            GCFestivalBase lastFestival = null;
            using (StreamReader sr = new StreamReader(pszFile))
            {
                nRet++;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string tag = "";
                    string data = "";

                    if (nMode == 0)
                    {
                        // preparation
                        if (line.Equals(GCFestivalSpecial.InstructionTag))
                        {
                            tag = line;
                        }
                        else if (line.IndexOf(':') > 0)
                        {
                            tag = line.Substring(0, line.IndexOf(':'));
                            data = line.Substring(line.IndexOf(':') + 1);
                        }

                        // processing of tag and data
                        if (tag.Equals(typeof(GCFestivalBook).Name))
                        {
                            string[] p = data.Split('|');
                            if (p.Length > 2)
                            {
                                GCFestivalBook fb = GCFestivalBookCollection.getSafeBook(GCFestivalBase.StringToInt(p[1], 6));
                                fb.CollectionName = GCFestivalBase.SafeToString(p[0]);
                                if (pszFile.EndsWith(".ev.rl"))
                                    fb.FileName = Path.GetFileName(pszFile);
                                fb.Visible = p[2].Equals("1");
                                nRet++;
                            }
                        }
                        else if (tag.Equals(typeof(GCFestivalTithiMasa).Name))
                        {
                            nRet++;
                            GCFestivalTithiMasa pce = new GCFestivalTithiMasa();
                            pce.EncodedString = data;
                            GCFestivalBook book = GCFestivalBookCollection.getSafeBook(pce.BookID);
                            book.Add(pce);
                            lastFestival = pce;
                        }
                        else if (tag.Equals(typeof(GCFestivalSankranti).Name))
                        {
                            nRet++;
                            GCFestivalSankranti se = new GCFestivalSankranti();
                            se.EncodedString = data;
                            GCFestivalBook book = GCFestivalBookCollection.getSafeBook(se.BookID);
                            book.Add(se);
                            lastFestival = se;
                        }
                        else if (tag.Equals(typeof(GCFestivalRelated).Name))
                        {
                            nRet++;
                            GCFestivalRelated re = new GCFestivalRelated();
                            re.EncodedString = data;
                            if (lastFestival != null)
                                lastFestival.AddRelatedFestival(re);
                        }
                        else if (tag.Equals(typeof(GCFestivalEkadasi).Name))
                        {
                            nRet++;
                            GCFestivalEkadasi fe = new GCFestivalEkadasi();
                            fe.EncodedString = data;
                            GCFestivalBook book = GCFestivalBookCollection.getSafeBook(fe.BookID);
                            book.Add(fe);
                            lastFestival = fe;
                        }
                        else if (tag.Equals(typeof(GCFestivalMasaDay).Name))
                        {
                            nRet++;
                            GCFestivalMasaDay me = new GCFestivalMasaDay();
                            me.EncodedString = data;
                            GCFestivalBook book = GCFestivalBookCollection.getSafeBook(me.BookID);
                            book.Add(me);
                            lastFestival = me;
                        }
                        else if (tag.Equals(typeof(GCFestivalSpecial).Name))
                        {
                            nRet++;
                            GCFestivalSpecial se = new GCFestivalSpecial();
                            se.EncodedString = data;
                            GCFestivalBook book = GCFestivalBookCollection.getSafeBook(se.BookID);
                            book.Add(se);
                            lastFestival = se;
                        }
                        else if (tag.Equals(GCFestivalSpecial.InstructionTag))
                        {
                            nMode = 1;
                            sb.Clear();
                        }
                    }
                    else if (nMode == 1)
                    {
                        if (line.Equals(GCFestivalSpecial.InstructionEndTag))
                        {
                            if (lastFestival is GCFestivalSpecial)
                            {
                                GCFestivalSpecial se = lastFestival as GCFestivalSpecial;
                                se.Script = sb.ToString();
                            }
                            nMode = 0;
                        }
                        else
                        {
                            sb.AppendLine(line);
                        }
                    }
                }

            }

            return nRet;

        }

        /// <summary>
        /// Saving file as text
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static int SaveAllChangedFestivalBooks(string folderName)
        {
            /*if (!GCGlobal.customEventListModified)
                return 0;*/

            HashSet<string> fileNames = new HashSet<string>();
            foreach (GCFestivalBook book in Books)
            {
                if (fileNames.Contains(book.FileName))
                {
                    book.FileName = string.Format("events{0:00}.ev.rl", book.CollectionId);
                    book.Changed = true;
                }

                fileNames.Add(book.FileName);
            }

            int nRet = 0;
            foreach (GCFestivalBook book in Books)
            {
                if (!book.Changed)
                    continue;
                nRet = SaveFestivalBook(folderName, book);
            }

            return nRet;
        }

        private static int SaveFestivalBook(string folderName, GCFestivalBook book)
        {
            int nRet = 0;
            using (StreamWriter sw = new StreamWriter(Path.Combine(folderName, book.FileName)))
            {
                sw.WriteLine("{0}:{1}|{2}|{3}", book.GetType().Name, GCFestivalBase.StringToSafe(book.CollectionName), book.CollectionId,
                    (book.Visible ? 1 : 0));
                foreach (GCFestivalBase fb in book.Festivals)
                {
                    if (fb.nDeleted == 0)
                    {
                        nRet++;
                        sw.WriteLine("{0}:{1}", fb.getToken(), fb.EncodedString);
                        if (fb.EventsCount > 0)
                        {
                            foreach (GCFestivalBase fb2 in fb.Events)
                            {
                                sw.WriteLine("{0}:{1}", fb2.getToken(), fb2.EncodedString);
                            }
                        }
                        if (fb is GCFestivalSpecial)
                        {
                            GCFestivalSpecial se = fb as GCFestivalSpecial;
                            sw.WriteLine(GCFestivalSpecial.InstructionTag);
                            sw.WriteLine(se.Script);
                            sw.WriteLine(GCFestivalSpecial.InstructionEndTag);
                        }
                    }
                }
            }

            return nRet;
        }
    }
}
