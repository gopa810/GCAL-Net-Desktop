using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GCAL
{
    public class GVButton: Button
    {
        public Color HighlightedBkgColor { get; set; }
        public Color HighlightedForeColor { get; set; }
        public Color OverBackColor { get; set; }
        private bool _highlighted = false;
        private Color _bkgBackup;
        private Color _foreBackup;

        public GVButton(): base()
        {
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            FlatAppearance.MouseOverBackColor = Color.LightGreen;
            FlatAppearance.BorderSize = 0;
            OverBackColor = Color.LightGreen;
            HighlightedBkgColor = Color.DarkGreen;
            HighlightedForeColor = Color.White;            
        }

        public bool Highlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                _highlighted = value;
                if (_highlighted)
                {
                    _bkgBackup = BackColor;
                    _foreBackup = ForeColor;
                    BackColor = HighlightedBkgColor;
                    ForeColor = HighlightedForeColor;
                    FlatAppearance.MouseOverBackColor = Color.Empty;
                }
                else
                {
                    BackColor = _bkgBackup;
                    ForeColor = _foreBackup;
                    FlatAppearance.MouseOverBackColor = OverBackColor;
                }
            }
        }
    }
}
