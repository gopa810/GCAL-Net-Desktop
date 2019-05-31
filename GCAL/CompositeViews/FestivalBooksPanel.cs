using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class FestivalBooksPanel : UserControl
    {
        public FestivalBooksPanelDelegate Controller { get; set; }

        public FestivalBooksPanel()
        {
            InitializeComponent();

            InitFBList();
        }

        private void InitFBList()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
            {
                listBox1.Items.Add(book);
            }

            listBox1.EndUpdate();
        }

        private StringFormat stringFormatCenterLeft = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

        private GCFestivalBook GetBook(int i)
        {
            if (i < 0 || i >= listBox1.Items.Count)
                return null;

            return (GCFestivalBook)listBox1.Items[i];
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            }

            Rectangle rc = new Rectangle(e.Bounds.X + e.Bounds.Height + 8, e.Bounds.Y + 2, e.Bounds.Width - e.Bounds.Height - 8, e.Bounds.Height - 4);

            Rectangle rcImage = new Rectangle(e.Bounds.X + e.Bounds.Height / 2 - 10, e.Bounds.Y + e.Bounds.Height / 2 - 10, 20, 20);

            GCFestivalBook fb = GetBook(e.Index);
            if (fb != null)
            {
                e.Graphics.DrawString(fb.CollectionName, SystemFonts.CaptionFont, Brushes.Black, rc, stringFormatCenterLeft);
                e.Graphics.DrawImage(fb.Visible ? Properties.Resources.eye : Properties.Resources.eye2, rcImage);
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            SizeF sf = e.Graphics.MeasureString("AM", SystemFonts.CaptionFont);
            e.ItemHeight = Convert.ToInt32(sf.Height * 2);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                if (index > 0)
                {
                    object o = listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    listBox1.Items.Insert(index - 1, o);
                    listBox1.SelectedIndex = index - 1;

                    GCFestivalBookCollection.MoveBook(index, index - 1);
                    GCFestivalBookCollection.SaveRootFile();
                }
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                if (index < listBox1.Items.Count - 1)
                {
                    object o = listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    listBox1.Items.Insert(index + 1, o);
                    listBox1.SelectedIndex = index + 1;

                    GCFestivalBookCollection.MoveBook(index, index + 1);

                    GCFestivalBookCollection.SaveRootFile();
                }
            }
        }

        private void buttonShowHide_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                GCFestivalBook fb = GCFestivalBookCollection.Books[index];
                fb.Visible = !fb.Visible;
                fb.Changed = true;
                listBox1.Invalidate();
                //no needed saving, because there is only order of books in the root file
                //GCFestivalBookCollection.SaveRootFile();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            DlgImportFestivalBook d = new DlgImportFestivalBook();
            if (d.ShowDialog() != DialogResult.OK)
                return;

            switch(d.ResultWay)
            {
                case 1:
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Event files (*.ev.rl)|*.ev.rl||";
                    ofd.InitialDirectory = GCGlobal.ConfigurationFolderPath;
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string fileNameOnly = ofd.FileName;
                        if (GCFestivalBookCollection.HasFile(fileNameOnly))
                        {
                            MessageBox.Show("File is already imported. Choose another one.");
                        }
                        else
                        {
                            int eventCount = GCFestivalBookCollection.OpenEventsFile(ofd.FileName);
                            if (eventCount > 0)
                            {
                                MessageBox.Show(string.Format("Imported {0} events from file {1}", eventCount, fileNameOnly));
                            }
                            else
                            {
                                MessageBox.Show("No events were imported. Maybe file does not conform to the required file format.");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                DlgEditFestivalBook d = new DlgEditFestivalBook();
                GCFestivalBook fb = GetBook(index);
                d.CollectionTitle = fb.CollectionName;
                if (d.ShowDialog() == DialogResult.OK && !d.CollectionTitle.Equals(fb.CollectionName))
                {
                    fb.CollectionName = d.CollectionTitle;
                    fb.Changed = true;
                    listBox1.Invalidate();
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DlgEditFestivalBook d = new DlgEditFestivalBook();
            d.CollectionTitle = string.Format("Events {0}", GCFestivalBookCollection.BookCollectionIdCounter);

            if (d.ShowDialog() == DialogResult.OK)
            {
                GCFestivalBook fb = GCFestivalBookCollection.CreateBook(d.CollectionTitle);
                listBox1.Items.Add(fb);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                DlgDeleteFestivalBook df = new DlgDeleteFestivalBook();
                GCFestivalBook fb = GetBook(index);
                df.FileNameReimport = Path.Combine(GCGlobal.ConfigurationFolderPath, fb.FileName);

                if (df.ShowDialog() == DialogResult.Yes)
                {
                    GCFestivalBookCollection.Books.RemoveAt(index);
                    listBox1.Items.RemoveAt(index);
                    GCFestivalBookCollection.SaveRootFile();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm reset", "This will reset all festival definitions.", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.OK)
            {
                GCFestivalBookCollection.ResetAllBooks(GCGlobal.ConfigurationFolderPath);
                InitFBList();
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                GCFestivalBook fb = GetBook(index);
                Controller.Parent.ExecuteMessage("setTabChanged", new GSString("events"));
                Controller.Parent.ExecuteMessage("currExec", new GSString("setCollection"), new GSNumber(fb.CollectionId));
            }
        }
    }

    public class FestivalBooksPanelDelegate : GVCore
    {
        public FestivalBooksPanelDelegate(FestivalBooksPanel v)
        {
            View = v;
            v.Controller = this;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            return base.ExecuteMessage(token, args);
        }
    }


}
