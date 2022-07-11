using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace MP3Manager
{
    public partial class FormMain : Form
    {
        DataTable dtFiles_  = new DataTable();

        #region COLUMN_NAMES
        static string ColFilePath              = "Path";
        static string ColFileName              = "File Name";
        static string ColFileSize              = "Size (MB)";

        static string ColTitle                 = "Title";
        static string ColBitRate               = "BitRate/m";
        static string ColAlbum                 = "Album Title";
        static string ColGenre                 = "Genre";
        static string ColAlbumArtist           = "Album Artist";
        static string ColPerformers            = "Contributing Artist";
        static string ColLyrics                = "Lyrics";
        static string ColComments              = "Comments";
        static string ColComposers             = "Composer";
        static string ColTrack                 = "Track";
        static string ColYear                  = "Year";

        static string ColAlbumArtFrontCover    = "Album Art";
        #endregion

        public FormMain()
        {
            InitializeComponent();

            dtFiles_.Columns.Add(ColFileName);
            dtFiles_.Columns.Add(ColTitle);
            dtFiles_.Columns.Add(ColAlbum);
            dtFiles_.Columns.Add(ColAlbumArtist);            
            dtFiles_.Columns.Add(ColPerformers);
            dtFiles_.Columns.Add(ColGenre);
            dtFiles_.Columns.Add(ColLyrics);
            dtFiles_.Columns.Add(ColComposers);
            dtFiles_.Columns.Add(ColTrack, typeof(uint));
            dtFiles_.Columns.Add(ColComments);
            dtFiles_.Columns.Add(ColYear, typeof(uint));

            dtFiles_.Columns.Add(ColAlbumArtFrontCover, typeof(Image));

            dtFiles_.Columns.Add(ColBitRate, typeof(uint)); 
            dtFiles_.Columns.Add(ColFileSize, typeof(double));            
            dtFiles_.Columns.Add(ColFilePath);


            this.dataGridViewFiles_.DataSource = dtFiles_;

            this.dataGridViewFiles_.Columns[ColBitRate].ReadOnly = true;
            this.dataGridViewFiles_.Columns[ColFileSize].ReadOnly = true;
            this.dataGridViewFiles_.Columns[ColFilePath].ReadOnly = true;
            this.dataGridViewFiles_.Columns[ColFileName].ReadOnly = true;

            this.dataGridViewFiles_.Columns[ColBitRate].Visible = false;
            this.dataGridViewFiles_.Columns[ColFileSize].Width = 100;
            this.dataGridViewFiles_.Columns[ColFilePath].Width = 400;
            this.dataGridViewFiles_.Columns[ColFileName].Width = 300; 
            this.dataGridViewFiles_.Columns[ColTitle].Width = 300;
            this.dataGridViewFiles_.Columns[ColAlbum].Width = 200;
            this.dataGridViewFiles_.Columns[ColAlbumArtist].Width = 250;
            this.dataGridViewFiles_.Columns[ColPerformers].Width = 250;
            this.dataGridViewFiles_.Columns[ColGenre].Width = 100;
            this.dataGridViewFiles_.Columns[ColLyrics].Width = 200;
            this.dataGridViewFiles_.Columns[ColTrack].Width = 50;
            this.dataGridViewFiles_.Columns[ColYear].Width = 75;
            this.dataGridViewFiles_.Columns[ColComments].Width = 200;
            this.dataGridViewFiles_.Columns[ColComposers].Width = 200;

            this.dataGridViewFiles_.Columns[ColAlbumArtFrontCover].Width = 100;


            this.dataGridViewFiles_.Columns[ColFileSize].DefaultCellStyle.Format = "0.00";


            this.dataGridViewFiles_.Columns[ColFileSize].DefaultCellStyle.BackColor = Color.LightGray;
            this.dataGridViewFiles_.Columns[ColFilePath].DefaultCellStyle.BackColor = Color.LightGray;
            this.dataGridViewFiles_.Columns[ColFileName].DefaultCellStyle.BackColor = Color.LightGray;

           


            this.dataGridViewFiles_.RowTemplate.Height = 45;


            this.Text = HelpAboutBox.AssemblyProduct;
        }
        

        void UpdateUICommands()
        {
            bool EnableSameValueToSelectedCells = this.IsSameOptionEnable;
            bool EnableRemoveFile = this.dataGridViewFiles_.SelectedRows.Count > 0;
            bool EnablePlay = this.IsPlayOptionEnable;
            bool EnableSelectAll = this.dataGridViewFiles_.Rows.Count > 0;
            bool EnableSave = this.dataGridViewFiles_.Rows.Count > 0;
            
            //main menu
            this.setSameValueToSelectedCellsToolStripMenuItem.Enabled = EnableSameValueToSelectedCells;
            this.selectAllToolStripMenuItem1.Enabled = EnableSelectAll;
            this.saveToolStripMenuItem1.Enabled = EnableSave;
            this.removeToolStripMenuItem1.Enabled = EnableRemoveFile;
            this.makeTitleCaseToolStripMenuItem.Enabled = this.IsChangeCaseOptionEnable;
            this.setFileNameAsTitleToolStripMenuItem.Enabled = this.IsSetFileNameAsTitleOptionEnable;
            //context menu
            Dictionary<int, List<DataGridViewCell>> rangeEditable = GetCellRanges(dataGridViewFiles_.SelectedCells, false);

            this.changeAlbumArtToolStripMenuItem.Enabled = rangeEditable.ContainsKey(dataGridViewFiles_.Columns[ColAlbumArtFrontCover].Index);
            this.sameToolStripMenuItem.Enabled = EnableSameValueToSelectedCells;
            this.removeToolStripMenuItem.Enabled = EnableRemoveFile;
            this.setFileNameAsTitleToolStripMenuItem1.Enabled = this.IsSetFileNameAsTitleOptionEnable;
            //toolbar
            this.saveToolStripButton.Enabled = EnableSave;
            this.toolStripButtonRemoveFile.Enabled = EnableRemoveFile;
            this.toolStripButtonSetSameValue.Enabled = EnableSameValueToSelectedCells;
            this.toolStripButtonSelectAll.Enabled = EnableSelectAll;
        }

        #region MAIN FORM EVENTS
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string sFile in files)
            {
                AddRow(sFile);
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.UpdateUICommands();
        }
        #endregion

        private void CommandAddOpen__Click(object sender, EventArgs e)
        {
            this.AddOpenFiles();
        }

        private void CommandSave__Click(object sender, EventArgs e)
        {           
            this.SaveRows();
        }

        #region GRID VIEW
        private void dataGridViewFiles__DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn col in dataGridViewFiles_.Columns)
            {
                DataGridViewImageColumn imagecol = col as DataGridViewImageColumn;
                if (null != imagecol)
                {
                    imagecol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
            }

            this.dataGridViewFiles_.Columns[ColFileName].Frozen = true;
        }

        private void dataGridViewFiles__CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewFiles_.Columns[e.ColumnIndex].Name == ColAlbumArtFrontCover)
            {
                string file = GetUserSelectedImageFile();
                if (file != string.Empty)
                {
                    dataGridViewFiles_.Rows[e.RowIndex].Cells[ColAlbumArtFrontCover].Value = Image.FromFile(file);
                }
            }
        }

        private void dataGridViewFiles__DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dataGridViewFiles_.CancelEdit();
            e.Cancel = true;
        }

        private void dataGridViewFiles__RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.UpdateUICommands();
        }

        private void dataGridViewFiles__RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.UpdateUICommands();
        }

        private void dataGridViewFiles__SelectionChanged(object sender, EventArgs e)
        {
            this.UpdateUICommands();
        }

        private void dataGridViewFiles__CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        #endregion

        #region GRID VIEW CONTEXT MENU
        private void changeAlbumArtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetAlbumArtToSelectedCells();           
        }

         private void contextMenuStripFileGrid__Opening(object sender, CancelEventArgs e)
        {
            this.UpdateUICommands();
        }

        private void sameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetSameValueToSelectedCells();
        }       
        #endregion

        #region MAIN MENUE
        private void menuStripMain__MenuActivate(object sender, EventArgs e)
        {
            this.UpdateUICommands();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridViewFiles_.SelectAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpAboutBox abtDlg = new HelpAboutBox();
            abtDlg.ShowDialog();
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles_.SelectedRows != null && dataGridViewFiles_.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgr in dataGridViewFiles_.SelectedRows)
                {
                    this.RemoveRow(dgr.Index);
                }
            }
        }

        private void mediaPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void setFileNameAsTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles_.SelectedCells != null)
            {
                List<DataGridViewRow> rows = GetRowsFromSlection();
                foreach (DataGridViewRow row in rows)
                {
                    string filepath = row.Cells[ColFilePath].Value as string;
                    if (!string.IsNullOrEmpty(filepath) && !string.IsNullOrWhiteSpace(filepath))
                    {
                        string filename = Path.GetFileNameWithoutExtension(filepath);
                        row.Cells[ColTitle].Value = filename;
                    }
                }
            }
        }

        private void makeTitleCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MakeTitleCaseToSelectedCells();
        }
        #endregion     
    }
}
