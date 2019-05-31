using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    public class GDDocument: GDDocumentBlock
    {
        /// <summary>
        /// Document title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Value can be "80em" which means 80 characters
        /// or "800px" which means 800 pixels
        /// </summary>
        public GDLength MinimumPageWidth { get; set; }

        private GDStylesBook p_styles = new GDStylesBook();

        public GDStylesBook Styles { get { return p_styles;  } }


        public override void ExportPlainText(TextWriter stream)
        {
        }

        public override void ExportRichText(GDExportContext context)
        {
            context.Stream.Write("{\\rtf1\\ansi\\ansicpg1252 ");
            int defaultFontId = -1;

            Dictionary<object,int> fontDictionary = HarvestFormatValues(GDStyleKey.FontFamily);
            Dictionary<object,int> colorDictionary = HarvestFormatValues(GDStyleKey.TextColor, GDStyleKey.BackgroundColor);

            context.fontMaps = fontDictionary;
            context.colorMaps = colorDictionary;

            // writing default font
            if (Format.ContainsStyle(GDStyleKey.FontFamily))
            {
                defaultFontId = fontDictionary[Format.GetValue(GDStyleKey.FontFamily)];
                context.Stream.Write("\\deff{0} ", defaultFontId);
            }
            context.Stream.Write("\\deflang1033 ");

            // writing font table
            if (fontDictionary.Count > 0)
            {
                context.Stream.Write("{\\fonttbl");
                List<int> ids = new List<int>();
                Dictionary<int, object> dict = new Dictionary<int, object>();
                foreach (KeyValuePair<object,int> pair in fontDictionary)
                {
                    ids.Add(pair.Value);
                    dict.Add(pair.Value, pair.Key);
                }
                ids.Sort();
                foreach (int fid in ids)
                {
                    string fontName = dict[fid].ToString();
                    context.Stream.Write('{');
                    context.Stream.Write("\\f{0}\\fswiss\\fcharset0 {1};", fid, fontName);
                    context.Stream.Write('}');
                }
                context.Stream.WriteLine("}");
            }

            // writing color table
            if (colorDictionary.Count > 0)
            {
                context.Stream.Write("{\\colortbl ;");
                List<int> ids = new List<int>();
                Dictionary<int, object> dict = new Dictionary<int, object>();
                foreach (KeyValuePair<object, int> pair in colorDictionary)
                {
                    ids.Add(pair.Value);
                    dict.Add(pair.Value, pair.Key);
                }
                ids.Sort();
                foreach (int fid in ids)
                {
                    GDStyleColor color = (GDStyleColor)dict[fid];
                    context.Stream.Write('{');
                    context.Stream.Write("\\red{0}\\green{1}\\blue{2};", color.Red255, color.Green255, color.Blue255);
                    context.Stream.Write('}');
                }
                context.Stream.Write("}");
            }

            // introduction to text part
            context.Stream.Write("{{\\*\\generator GCAL;}\\viewkind4\\uc1\\pard ");
            if (defaultFontId >= 0)
                context.Stream.Write("\\f{0} ", defaultFontId);
            if (Format.ContainsStyle(GDStyleKey.TextSize))
            {
                GDLength fontSize = (GDLength)Format.GetValue(GDStyleKey.TextSize);
                context.Stream.Write("\\fs{0} ", fontSize.Points);
            }

            // body of document


            // end of document
            context.Stream.Write("}");
        }

        public override void ExportHtml(TextWriter stream)
        {
            stream.WriteLine("<html>");
            stream.WriteLine("<head>");
            stream.Write("<title>{0}</title>", Title);
            stream.WriteLine("</head>");
            stream.WriteLine("<body>");
            stream.WriteLine("</body>");
            stream.WriteLine("</html>");
        }

    }
}
