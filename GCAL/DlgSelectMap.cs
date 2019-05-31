using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GCAL
{
    public partial class DlgSelectMap : Form
    {
        public GCMap SelectedMap { get; set; }

        public Rectangle MapImageRect { get; set; }

        public bool AddAnchorMode = false;

        public DlgSelectMap()
        {
            InitializeComponent();
            SelectedMap = null;

            if (GCMapService.Maps.Count == 0)
                GCMapService.OnStart();

            UpdateMapList();

            EnableButtonsAccordingSelectedMap();
        }

        private void UpdateMapList()
        {
            listBoxMaps.BeginUpdate();
            listBoxMaps.Items.Clear();
            foreach (GCMap map in GCMapService.Maps)
            {
                listBoxMaps.Items.Add(map);
            }
            listBoxMaps.EndUpdate();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            GCMap map = new GCMap();
            map.Title = "New Map " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            map.Id = Guid.NewGuid();

            GCMapService.Maps.Add(map);
            listBoxMaps.Items.Add(map);
            listBoxMaps.SelectedIndex = listBoxMaps.Items.Count - 1;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ListBox lb = listBoxMaps;
            if (lb.SelectedIndex >= 0 && lb.SelectedIndex < lb.Items.Count)
            {
                GCMapService.Maps.RemoveAt(lb.SelectedIndex);
                lb.Items.RemoveAt(lb.SelectedIndex);
                if (lb.SelectedIndex >= lb.Items.Count)
                {
                    lb.SelectedIndex = lb.Items.Count - 1;
                }
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            if (SelectedMap == null)
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(ofd.FileName);
                    SelectedMap.ImageFilePath = "";
                    SelectedMap.Image = img;
                    SelectedMap.ImageChanged = true;
                    SelectedMap.AnchorPoints.Clear();
                    panelImage.Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading image. File not recognized as image.\n" + ex.Message);
                }
            }
        }

        private Color OldSetAnchorButtonBackColor = Color.Transparent;

        private void buttonSetAnchorPoint_Click(object sender, EventArgs e)
        {
            AddAnchorMode = true;
            OldSetAnchorButtonBackColor = buttonSetAnchorPoint.BackColor;
            buttonSetAnchorPoint.BackColor = Color.LightPink;
        }

        private void panelImage_SizeChanged(object sender, EventArgs e)
        {
            panelImage.Invalidate();
        }

        private void panelImage_MouseDown(object sender, MouseEventArgs e)
        {
            buttonSetAnchorPoint.BackColor = SystemColors.Control;
            AddAnchorMode = false;

            if (MapImageRect.Contains(e.Location))
            {
                DlgSelectLocation dlg3 = new DlgSelectLocation();

                if (dlg3.ShowDialog() == DialogResult.Cancel)
                    return;

                GCMapAnchor ma = new GCMapAnchor();
                ma.Location = dlg3.SelectedLocation.Title;
                ma.Longitude = dlg3.SelectedLocation.Longitude;
                ma.Latitude = dlg3.SelectedLocation.Latitude;
                ma.relX = (double)(e.X - MapImageRect.X) / MapImageRect.Width;
                ma.relY = (double)(e.Y - MapImageRect.Y) / MapImageRect.Height;

                SelectedMap.AnchorPoints.Add(ma);

                SelectedMap.RecalculateDimensions();

                panelImage.Invalidate();
                buttonSelect.Enabled = SelectedMap.MapUsable;
            }

        }


        private void panelImage_Paint(object sender, PaintEventArgs e)
        {
            MapImageRect = Rectangle.Empty;

            if (SelectedMap != null)
            {
                if (SelectedMap.Image == null)
                {
                    if (File.Exists(SelectedMap.ImageFilePath))
                    {
                        SelectedMap.Image = Image.FromFile(SelectedMap.ImageFilePath);
                    }
                }

                if (SelectedMap.Image != null)
                {
                    Size szPanel = panelImage.Size;
                    Size szImage = SelectedMap.Image.Size;

                    SelectedMap.ImageSize = szImage;

                    if (szPanel.Width > 0 && szPanel.Height > 0 && szImage.Width > 0 && szImage.Height > 0)
                    {
                        double ratio = Math.Min((double)szPanel.Width/szImage.Width, (double)szPanel.Height/szImage.Height);
                        szImage = new Size(Convert.ToInt32(ratio*szImage.Width), Convert.ToInt32(ratio*szImage.Height));

                        MapImageRect = new Rectangle(szPanel.Width / 2 - szImage.Width / 2, szPanel.Height/2 - szImage.Height/2,
                            szImage.Width, szImage.Height);
                        e.Graphics.DrawImage(SelectedMap.Image, MapImageRect);

                        foreach(GCMapAnchor ma in SelectedMap.AnchorPoints)
                        {
                            Point pt = new Point(Convert.ToInt32(ma.relX * MapImageRect.Width) + MapImageRect.Left,
                                Convert.ToInt32(ma.relY * MapImageRect.Height) + MapImageRect.Top);

                            e.Graphics.FillRectangle(Brushes.Red, pt.X - 2, pt.Y - 2, 4, 4);

                            SizeF sf = e.Graphics.MeasureString(ma.Location, SystemFonts.MenuFont);
                            Rectangle rct = new Rectangle(0, 0, (int)sf.Width + 1, (int)sf.Height + 1);
                            if (pt.Y > MapImageRect.Left + MapImageRect.Height/2)
                            {
                                rct.X = pt.X - rct.Width / 2;
                                rct.Y = pt.Y - 6 - rct.Height;
                            }
                            else
                            {
                                rct.X = pt.X - rct.Width / 2;
                                rct.Y = pt.Y + 6;
                            }

                            if (rct.Right > panelImage.Width)
                                rct.X -= (rct.Right - panelImage.Width);
                            if (rct.Left < 0)
                                rct.X = 0;

                            e.Graphics.FillRectangle(ma.drawSelect ? Brushes.Yellow : Brushes.White, rct.X - 2, rct.Y - 2, rct.Width + 4, rct.Height + 4);
                            e.Graphics.DrawRectangle(ma.drawSelect ? Pens.Blue : Pens.Gray, rct.X - 2, rct.Y - 2, rct.Width + 4, rct.Height + 4);
                            e.Graphics.DrawString(ma.Location, SystemFonts.MenuFont, ma.drawSelect ? Brushes.Blue : Brushes.Black, rct.X, rct.Y);

                            ma.drawRect = rct;
                        }
                    }

                }
            }
        }

        private void textBoxMapTitle_TextChanged(object sender, EventArgs e)
        {
            if (SelectedMap != null)
            {
                SelectedMap.Title = textBoxMapTitle.Text;
                listBoxMaps.Invalidate();
            }
        }

        private void listBoxMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = listBoxMaps;
            if (lb.SelectedIndex >= 0 && lb.SelectedIndex < lb.Items.Count)
            {
                SelectedMap = (GCMap)lb.Items[lb.SelectedIndex];
                SelectedMap.ResetDisplay();
            }
            else
            {
                SelectedMap = null;
            }

            panelImage.Invalidate();
            EnableButtonsAccordingSelectedMap();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            GCMapService.OnSaveState();
        }

        private void panelImage_MouseMove(object sender, MouseEventArgs e)
        {
            panelImage.Cursor = (AddAnchorMode ? Cursors.Cross : Cursors.Default);
        }

        private void buttonImageFromClipboard_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage() && SelectedMap != null)
            {
                SelectedMap.ImageFilePath = "";
                SelectedMap.Image = Clipboard.GetImage();
                SelectedMap.ImageChanged = true;
                panelImage.Invalidate();
            }
        }

        private void EnableButtonsAccordingSelectedMap()
        {
            buttonLoadImage.Enabled = (SelectedMap != null);
            textBoxMapTitle.Enabled = (SelectedMap != null);
            buttonImageFromClipboard.Enabled = (SelectedMap != null);
            buttonSetAnchorPoint.Enabled = (SelectedMap != null);
            buttonDelete.Enabled = (SelectedMap != null);
            textBoxMapTitle.Text = (SelectedMap == null ? "" : SelectedMap.Title);
            buttonSelect.Enabled = (SelectedMap == null ? false : SelectedMap.MapUsable);
        }
    }
}
