using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultBase: GSCore
    {
        public virtual string formatText(string templateName)
        {
            return string.Empty;
        }

        public virtual TResultFormatCollection getFormats()
        {
            TResultFormatCollection collection = new TResultFormatCollection();
            return collection;
        }
    }

    /// <summary>
    /// Collection of formats eligible for exporting from given source
    /// </summary>
    public class TResultFormatCollection
    {
        public string ResultName = string.Empty;
        public List<TResultFormat> Formats = new List<TResultFormat>();

        public string getDialogFilterString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TResultFormat fmt in Formats)
            {
                sb.AppendFormat("{0} (*.{1})|*.{1}|", fmt.FileTypeDescription, fmt.FileExtension);
            }
            sb.Append("|");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Description of one single data format
    /// </summary>
    public class TResultFormat
    {
        public string FileTypeDescription = string.Empty;
        public string FileExtension = string.Empty;
        public string TemplateName = string.Empty;

        public TResultFormat()
        {
        }

        public TResultFormat(string desc, string ext, string templateName)
        {
            FileTypeDescription = desc;
            FileExtension = ext;
            TemplateName = templateName;
        }

    }
}
