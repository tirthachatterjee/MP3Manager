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
        bool IsFileAdded(string path)
        {
            foreach (DataRow dr in dtFiles_.Rows)
            {
                if (dr[ColFilePath].ToString() == path)
                {
                    return true;
                }
            }
            return false;
        }
                
        bool AddRow(string path)
        {
            bool bSuccess = false;
            try
            {
                if (File.Exists(path) && Path.GetExtension(path).ToLower() == ".mp3")
                {
                    if (!IsFileAdded(path))
                    {
                        TagLib.File FileID3 = TagLib.File.Create(path);
                        if (!FileID3.PossiblyCorrupt)
                        {
                            FileInfo finfo = new FileInfo(path);
                            DataRow dr = dtFiles_.NewRow();

                            dr[ColFileName] = Path.GetFileNameWithoutExtension(path);
                            dr[ColBitRate] = FileID3.Tag.BeatsPerMinute;

                            dr[ColTitle] = FileID3.Tag.Title;
                            dr[ColAlbum] = FileID3.Tag.Album;
                            dr[ColAlbumArtist] = ConvertToString(FileID3.Tag.AlbumArtists);
                            dr[ColPerformers] = ConvertToString(FileID3.Tag.Performers);
                            dr[ColGenre] = ConvertToString(FileID3.Tag.Genres);
                            dr[ColLyrics] = FileID3.Tag.Lyrics;
                            dr[ColComments] = FileID3.Tag.Comment;
                            dr[ColComposers] = ConvertToString(FileID3.Tag.Composers);
                            dr[ColTrack] = FileID3.Tag.Track;
                            dr[ColYear] = FileID3.Tag.Year;
                            //dr[ColAlbumArtFrontCover] = FileID3.Tag.Pictures[0].Data;

                            dr[ColFileSize] = (double)finfo.Length / 1048576.00;
                            dr[ColFilePath] = path;

                            dr[ColAlbumArtFrontCover] = GetTagImage(FileID3.Tag.Pictures, TagLib.PictureType.FrontCover);//.GetThumbnailImage(50, 50, null, System.IntPtr.Zero);


                            dtFiles_.Rows.Add(dr);
                            bSuccess = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (!bSuccess)
                MessageBox.Show("Failed to process file :" + path, "File Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return bSuccess;
        }

        void SaveRow(DataGridViewRow row)
        {
            string path = row.Cells[ColFilePath].Value as string;
            if (File.Exists(path))
            {
                TagLib.File FileID3 = TagLib.File.Create(path);
                if (!FileID3.PossiblyCorrupt)
                {
                    FileID3.Tag.Title = row.Cells[ColTitle].Value as string;
                    FileID3.Tag.Album = row.Cells[ColAlbum].Value as string;
                    FileID3.Tag.Genres = ConvertToArray(row.Cells[ColGenre].Value as string);
                    FileID3.Tag.AlbumArtists = ConvertToArray(row.Cells[ColAlbumArtist].Value as string);
                    FileID3.Tag.Performers = ConvertToArray(row.Cells[ColPerformers].Value as string);
                    FileID3.Tag.Lyrics = row.Cells[ColLyrics].Value as string;
                    FileID3.Tag.Comment = row.Cells[ColComments].Value as string;
                    FileID3.Tag.Composers = ConvertToArray(row.Cells[ColComposers].Value as string);
                    FileID3.Tag.Track = (uint)row.Cells[ColTrack].Value;
                    FileID3.Tag.Year = (uint)row.Cells[ColYear].Value;

                    Image FrontCover = row.Cells[ColAlbumArtFrontCover].Value as System.Drawing.Image;
                    SetTagImage(ref FileID3, TagLib.PictureType.FrontCover, FrontCover);

                    FileID3.Save();
                }
            }
        }

        void RemoveRow(int index)
        {            
            this.dataGridViewFiles_.Rows.RemoveAt(index);
        }

        void SaveRows()
        {
            foreach (DataGridViewRow dr in this.dataGridViewFiles_.Rows)
            {
                SaveRow(dr);
            }
        }     

        

        void AddOpenFiles()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "MP3 files (*.MP3)|*.MP3";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = true;
            //openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //dtFiles_.Clear();

                string[] files = openFileDialog1.FileNames;
                foreach (string sFile in files)
                {
                    AddRow(sFile);
                }
            }
        }

        void SetSameValueToSelectedCells()
        {
            Dictionary<int, List<DataGridViewCell>> range = GetCellRanges(dataGridViewFiles_.SelectedCells, false);
            foreach (KeyValuePair<int, List<DataGridViewCell>> colrange in range)
            {
                if (this.dataGridViewFiles_.Columns[colrange.Key].Name != ColTitle)
                {
                    List<DataGridViewCell> cells = colrange.Value;
                    cells.Sort(CompareCellByRows);
                    if (cells.Count > 1)
                    {
                        object firstcellvalue = cells[0].Value;

                        for (int index = 1; index < cells.Count; index++)
                        {
                            cells[index].Value = firstcellvalue;
                        }
                    }
                }
            }
        }

        void MakeTitleCaseToSelectedCells()
        {
            Dictionary<int, List<DataGridViewCell>> range = GetCellRanges(dataGridViewFiles_.SelectedCells, false);
            foreach (KeyValuePair<int, List<DataGridViewCell>> colrange in range)
            {
                if (this.dataGridViewFiles_.Columns[colrange.Key].Name != ColFileName
                    && !this.dataGridViewFiles_.Columns[colrange.Key].ReadOnly
                    && this.dataGridViewFiles_.Columns[colrange.Key].ValueType == typeof(string))
                {
                    List<DataGridViewCell> cells = colrange.Value;
                    foreach (DataGridViewCell cell in cells)
                    {
                        string text = cell.Value as string;
                        if (text != null && !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
                        {
                            text = text.ToLower();
                            System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                            text = textInfo.ToTitleCase(text);
                            cell.Value = text;
                        }
                    }
                }
            }
        }

        void SetAlbumArtToSelectedCells()
        {
            Dictionary<int, List<DataGridViewCell>> range = GetCellRanges(dataGridViewFiles_.SelectedCells, false);
            int albumArtColIndex = dataGridViewFiles_.Columns[ColAlbumArtFrontCover].Index;
            if (range.ContainsKey(albumArtColIndex))
            {
                List<DataGridViewCell> cells = range[albumArtColIndex];
                string file = GetUserSelectedImageFile();
                foreach (DataGridViewCell cell in cells)
                {
                    dataGridViewFiles_.Rows[cell.RowIndex].Cells[albumArtColIndex].Value = Image.FromFile(file);
                }
            } 
        }

        bool IsSameOptionEnable
        {
            get
            {
                bool enablesame = false;
                Dictionary<int, List<DataGridViewCell>> rangeEditable = GetCellRanges(dataGridViewFiles_.SelectedCells, false);
                if (rangeEditable.Count > 0)
                {
                    enablesame = (!(rangeEditable.Count == 1 && rangeEditable.ContainsKey(dataGridViewFiles_.Columns[ColTitle].Index)));
                }
                return enablesame;
            }
        }

        bool IsChangeCaseOptionEnable
        {
            get
            {
                SortedSet<string> items = GetColumnsFromSelection();
                foreach (string item in items)
                {
                    if (item != ColFileName
                        && !this.dataGridViewFiles_.Columns[item].ReadOnly
                        && this.dataGridViewFiles_.Columns[item].ValueType == typeof(string))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        bool IsSetFileNameAsTitleOptionEnable
        {
            get
            {
                SortedSet<string> items = GetColumnsFromSelection();
                return items.Contains(ColFileName);
            }
        }

        bool IsPlayOptionEnable
        {
            get
            {
                return (dataGridViewFiles_.SelectedRows != null && dataGridViewFiles_.SelectedRows.Count > 0); ;
            }
        }

        List<DataGridViewRow> GetRowsFromSlection()
        {
            DataGridViewSelectedCellCollection selection = this.dataGridViewFiles_.SelectedCells;
            SortedSet<int> rowset = new SortedSet<int>();
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            if (selection != null)
            {
                foreach (DataGridViewCell cell in selection)
                {
                    if (!rowset.Contains(cell.RowIndex))
                    {
                        rowset.Add(cell.RowIndex);
                        rows.Add(this.dataGridViewFiles_.Rows[cell.RowIndex]);
                    }
                }
            }
            return rows;
        }

        SortedSet<string> GetColumnsFromSelection()
        {
            SortedSet<string> items = new SortedSet<string>();
            
            DataGridViewSelectedCellCollection selection = this.dataGridViewFiles_.SelectedCells;
            if (selection != null)
            {
                foreach (DataGridViewCell cell in selection)
                {
                    if (!items.Contains(dataGridViewFiles_.Columns[cell.ColumnIndex].Name))
                    {
                        items.Add(dataGridViewFiles_.Columns[cell.ColumnIndex].Name);                        
                    }
                }
            }

            return items;
        }

        #region STATIC_METHODS

        static string GetUserSelectedImageFile()
        {
            string file = string.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";//Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = openFileDialog1.FileName;
            }
            return file;
        }

        static string ConvertToString(string[] items)
        {
            string sRet = string.Empty;

            foreach (string item in items)
            {
                if (sRet != string.Empty)
                    sRet += ";";
                sRet += item;
            }
            return sRet;
        }

        static string[] ConvertToArray(string text)
        {
            return text.Split(';');
        }

        static TagLib.Picture ConvertToTagLibPicture(Image image)
        {
            TagLib.Picture pic = null;
            if (null != image)
            {
                ImageFormat fmt = new ImageFormat(image.RawFormat.Guid);

                pic = new TagLib.Picture();
                pic.Type = TagLib.PictureType.FrontCover;
                pic.MimeType = GetMimeType(image);
                pic.Description = "Cover";
                pic.Data = TagLib.ByteVector.FromStream(ToStream(image, fmt));
            }
            return pic;
        }

        static string GetMimeType(Image i)
        {
            var imgguid = i.RawFormat.Guid;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                    return codec.MimeType;
            }
            return "image/unknown";
        }

        static Stream ToStream(Image image, ImageFormat formaw)
        {
            
            MemoryStream stream = new System.IO.MemoryStream();
            image.Save(stream, formaw);
            stream.Position = 0;
            return stream;
        }

        static void SetTagImage(ref TagLib.File FileID3, TagLib.PictureType type, Image image)
        {
            if (null != image)
            {

                List<TagLib.IPicture> imgs = new List<TagLib.IPicture>();
                imgs.Add(ConvertToTagLibPicture(image));
                FileID3.Tag.Pictures = imgs.ToArray();

                ////Convert to map
                //Dictionary<TagLib.PictureType, TagLib.IPicture> imgMap = new Dictionary<TagLib.PictureType, TagLib.IPicture>();
                //foreach (TagLib.IPicture TgPic in FileID3.Tag.Pictures)
                //{
                //    imgMap.Add(TgPic.Type, TgPic);
                //}

                //if (imgMap.ContainsKey(type))
                //{
                //    imgMap[type] = ConvertToTagLibPicture(image);
                //}
                //else
                //{
                //    imgMap.Add(type, ConvertToTagLibPicture(image));
                //}

                //FileID3.Tag.Pictures = imgMap.Values.ToArray();
            }
        }

        static Image GetTagImage(TagLib.IPicture[] Pictures, TagLib.PictureType type)
        {
            Image albuminmage = null;

            if (Pictures.Length > 0)
            {
                TagLib.IPicture picCover = null;
                foreach (TagLib.IPicture pic in Pictures)
                {
                    if (pic.Type == type)
                    {
                        picCover = pic;
                    }
                }

                if (null != picCover)
                {
                    MemoryStream ms = new MemoryStream(picCover.Data.Data);
                    if (ms != null && ms.Length > 4096)
                    {
                        albuminmage = System.Drawing.Image.FromStream(ms);
                        // Load thumbnail into PictureBox
                        ///albuminmage = img.GetThumbnailImage(Width, Height, null, System.IntPtr.Zero);
                    }
                    //ms.Close();
                }
            }

            return albuminmage;
        }

        static Image GetAlbumArt(string filepath, int Width, int Height)
        {
            Image albuminmage = null;
            if (File.Exists(filepath))
            {
                TagLib.File FileID3 = TagLib.File.Create(filepath);
                if (!FileID3.PossiblyCorrupt)
                {
                    albuminmage = GetTagImage(FileID3.Tag.Pictures, TagLib.PictureType.FrontCover);
                }
            }
            return albuminmage;
        }

        static Dictionary<int, List<DataGridViewCell>> GetCellRanges(DataGridViewSelectedCellCollection selection, bool IncludeReadonlyCells)
        {
            Dictionary<int, List<DataGridViewCell>> range = new Dictionary<int, List<DataGridViewCell>>();
            if (selection != null)
            {
                foreach (DataGridViewCell cell in selection)
                {
                    if (IncludeReadonlyCells || cell.ReadOnly == false)
                    {
                        List<DataGridViewCell> celllist = null;
                        if (range.ContainsKey(cell.ColumnIndex))
                        {
                            celllist = range[cell.ColumnIndex];
                        }
                        else
                        {
                            celllist = new List<DataGridViewCell>();
                            range.Add(cell.ColumnIndex, celllist);
                        }
                        celllist.Add(cell);
                    }
                }
            }
            return range;
        }

        static int CompareCellByRows(DataGridViewCell left, DataGridViewCell right)
        {
            int ret = 0;
            if (left.RowIndex > right.RowIndex)
            {
                ret = 1;
            }
            else if (left.RowIndex < right.RowIndex)
            {
                ret = -1;
            }

            return ret;
        }
        #endregion
    }
}
