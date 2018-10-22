using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PMS.Data;
using PMS.Business.Enum;

namespace DuAn03_HaiDang
{
    public partial class FrmSoundFile : FormBase
    {
        // private SoundDAO soundDAO;
        private int soundId = 0;
        private int videoId = 0;
        private OpenFileDialog dlg = null;
        private string pathChooseFile = string.Empty;
        //  private AppConfigDAO appConfigDAO;
        int AppId = 0;
        string UrlSaveMediaFile = string.Empty;
        public FrmSoundFile(int _Appid, string _UrlSaveMediaFile)
        {
            InitializeComponent();
            try
            {
                //     soundDAO = new SoundDAO();
                //   appConfigDAO = new AppConfigDAO();
                this.AppId = _Appid;
                UrlSaveMediaFile = _UrlSaveMediaFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void FrmSoundFile_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabControl.SelectedTabPageIndex == 0)
                LoadDataToGridView();
            else
                LoadVideoToGridView();
        }

        #region Sound File
        #region event

        private void LoadDataToGridView()
        {
            try
            {
                dgListSound.DataSource = null;
                dgListSound.DataSource = BLLSound.Gets();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtPath_Enter(object sender, EventArgs e)
        {
            try
            {
                //dlg = new OpenFileDialog();
                //dlg.Filter = "file hinh|*.wav|all file|*.*";
                //if (string.IsNullOrEmpty(pathChooseFile))
                //    dlg.InitialDirectory = @"C:\";
                //else
                //    dlg.InitialDirectory = pathChooseFile;
                //dlg.Multiselect = false;
                //if (dlg.ShowDialog() == DialogResult.OK)
                //{
                //    string[] tmp = dlg.FileNames;
                //    foreach (string i in tmp)
                //    {
                //        FileInfo fi = new FileInfo(i);
                //        string[] xxx = i.Split('\\');
                //        string tenFile = xxx[xxx.Length - 1];
                //        txtName.Text = tenFile;
                //        string[] yyy = tenFile.Split('.');
                //        string type = yyy[yyy.Length - 1];
                //        tenFile = tenFile.Replace("." + type, "");
                //        DateTime dtime = DateTime.Now;
                //        txtPath.Text = tenFile + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                //        pathChooseFile = i.Substring(0, i.Length - tenFile.Length);
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butChooseFile_Enter(object sender, EventArgs e)
        {
            try
            {
                dlg = new OpenFileDialog();
                dlg.Filter = "file âm thanh|*.wav|all file|*.*";
                if (string.IsNullOrEmpty(pathChooseFile))
                    dlg.InitialDirectory = @"C:\";
                else
                    dlg.InitialDirectory = pathChooseFile;
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] tmp = dlg.FileNames;
                    foreach (string i in tmp)
                    {
                        FileInfo fi = new FileInfo(i);
                        string[] xxx = i.Split('\\');
                        string tenFile = xxx[xxx.Length - 1];
                        txtName.Text = tenFile;
                        string[] yyy = tenFile.Split('.');
                        string type = yyy[yyy.Length - 1];
                        tenFile = tenFile.Replace("." + type, "");
                        DateTime dtime = DateTime.Now;
                        var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                        shortPath = convertToUnSign(shortPath);

                        txtPath.Text = shortPath;
                        pathChooseFile = i.Substring(0, i.Length - tenFile.Length);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Chương trình sẽ xóa bỏ tất cả các tệp Âm thanh cũ và tạo lại danh sách Tệp Âm thanh mới như trong Thư mục bạn chỉ định.\n Bạn có muốn thực muốn thực hiện thao tác này không ?", "Thông Báo", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    var folderResult = folderBrowserDialog1.ShowDialog();
                    if (folderResult == DialogResult.OK)
                    {
                        var filterStr = "*.wav,*.mp3";
                        //   dbclass.listAppConfig = appConfigDAO.GetListAppConfig(AppId);
                        //  if (dbclass.listAppConfig != null && dbclass.listAppConfig.Count > 0)
                        //  {
                        //       var cf = dbclass.listAppConfig.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.SoundFileExtentions));
                        //  filterStr = cf != null ? cf.Value.Trim() : "*.wav,*.mp3";
                        filterStr = "*.wav ";
                        //    }

                        var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories).Where(x => filterStr.Contains(System.IO.Path.GetExtension(x).ToLower()));
                        if (files.Count() > 0)
                        {
                            var oldSounds = BLLSound.Gets();
                            if (oldSounds.Count > 0)
                            {
                                foreach (var item in oldSounds)
                                {
                                    BLLSound.Delete(item.Id);
                                    if (!string.IsNullOrEmpty(item.Path))
                                    {
                                        item.Path = Application.StartupPath + @"\Sound\" + item.Path;
                                        if (File.Exists(item.Path))
                                            File.Delete(item.Path);
                                    }
                                }
                            }

                            var dtime = DateTime.Now;
                            int count = 0;
                            foreach (string f in files)
                            {
                                try
                                {
                                    var sound = new PMS.Data.SOUND();
                                    var fi = new FileInfo(f);
                                    string[] xxx = f.Split('\\');
                                    string tenFile = xxx[xxx.Length - 1];
                                    sound.Name = tenFile;
                                    string[] yyy = tenFile.Split('.');
                                    string type = yyy[yyy.Length - 1];
                                    tenFile = tenFile.Replace("." + type, "");
                                    sound.Code = tenFile;
                                    var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                                    shortPath = convertToUnSign(shortPath);

                                    string fullPath = Application.StartupPath + @"\Sound\" + shortPath;
                                    File.Copy(f, fullPath);

                                    sound.Description = "Tệp được Thêm tự động";
                                    sound.Path = shortPath;
                                    sound.IsActive = true;
                                    sound.IsDeleted = false;
                                    BLLSound.CreateOrUpdate(sound);
                                    count++;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            LoadDataToGridView();
                            MessageBox.Show("Đã thêm " + count + " Tệp âm thanh thành công.", "Thông Báo");
                        }
                        else
                            MessageBox.Show("Không tìm thấy tệp Âm Thanh trong thư mục bạn vừa chọn.", "Thông Báo");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
                SaveSound();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidate())
                    SaveSound();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (soundId > 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá?", "Xoá file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        BLLSound.Delete(soundId);
                        ResetForm();
                        LoadDataToGridView();
                    }
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn đối tượng để xoá.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                txtCode.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code").ToString() : string.Empty;
                txtName.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString() : string.Empty;
                txtDescription.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description").ToString() : string.Empty;
                txtPath.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Path") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Path").ToString() : string.Empty;
                bool isActive = false;
                bool.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsActive").ToString(), out isActive);
                chkIsActive.Checked = isActive;
                soundId = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out soundId);
                chkIsActive.Enabled = true;

                btnAdd.Enabled = false;
                btnrefresh.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butChooseFile_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region common
        private bool CheckValidate()
        {
            var flag = true;
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã tệp.");
                    flag = false;
                }
                else if (string.IsNullOrEmpty(txtCode.Name))
                {
                    MessageBox.Show("Vui lòng nhập Tên tệp.");
                    flag = false;
                }
                else if (soundId == 0 && string.IsNullOrEmpty(txtPath.Text))
                {
                    MessageBox.Show("Vui lòng chọn tệp.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }
        private void ResetForm()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPath.Text = string.Empty;
            chkIsActive.Checked = true;
            chkIsActive.Enabled = false;
            soundId = 0;

            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = true;
            btnrefresh.Enabled = true;
            if (dlg != null)
                dlg.FileName = string.Empty;
        }
        private SOUND GetSoundData()
        {
            try
            {
                var sound = new SOUND();
                sound.Id = soundId;
                sound.Code = txtCode.Text;
                sound.Name = txtName.Text;
                sound.Description = txtDescription.Text;
                sound.Path = txtPath.Text;
                sound.IsActive = chkIsActive.Checked;
                return sound;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SaveSound()
        {
            try
            {
                var sound = GetSoundData();
                if (dlg != null && !string.IsNullOrEmpty(dlg.FileName))
                {
                    string f = dlg.FileName;
                    string fullPath = Application.StartupPath + @"\Sound\" + txtPath.Text;
                    File.Copy(f, fullPath);
                }
                var result = BLLSound.CreateOrUpdate(sound);
                if (result.IsSuccess)
                {
                    ResetForm();
                    LoadDataToGridView();
                }
                else
                    MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // ham convert tu co dau thanh ko dau
        public static string convertToUnSign(string s)
        {
            try
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD);
                return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            catch (Exception ex)
            {
                MessageBox.Show("convert sign +" + ex.Message);
            }
            return null;
        }
        private Sound BuilModel()
        {
            Sound model = null;
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên file không được để trống.");
                else if (string.IsNullOrEmpty(txtPath.Text))
                {
                    MessageBox.Show("Lỗi: Bạn chưa chọn đường dẫn đến file.");
                }
                else if (string.IsNullOrEmpty(txtCode.Text))
                {
                    MessageBox.Show("Lỗi: Mã file không được để trống.");
                }
                else
                {
                    model = new Sound();
                    model.Id = soundId;
                    model.Name = txtName.Text;
                    model.Description = txtDescription.Text;
                    model.IsActive = chkIsActive.Checked;
                    model.Code = txtCode.Text;
                    model.Path = txtPath.Text;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        #endregion

        #endregion

        #region Video File
        #region Video Event
        private void btnAdd_v_Click(object sender, EventArgs e)
        {
            if (CheckVideoValidate())
                SaveVideo();
        }

        private void btnUpdate_v_Click(object sender, EventArgs e)
        {
            if (CheckVideoValidate())
                SaveVideo();
        }

        private void btnDelete_v_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoId > 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá?", "Xoá file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        BLLVideoLibrary.Delete(videoId);
                        ResetVideoForm();
                        LoadVideoToGridView();
                    }
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn đối tượng để xoá.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_v_Click(object sender, EventArgs e)
        {
            ResetVideoForm();
        }

        private void btnAddMulti_v_Click(object sender, EventArgs e)
        {
            try
            {
                openVideoFileDialog.Filter = "Video (*.mp4;*.MP4)" + "All files (*.*)|*.*";
                openVideoFileDialog.Multiselect = true;
                var folderResult = openVideoFileDialog.ShowDialog();
                if (folderResult == DialogResult.OK)
                {
                    var filterStr = "*.mp4,*.mp3";
                    var files = openVideoFileDialog.FileNames;
                    if (files.Count() > 0)
                    {
                        var dtime = DateTime.Now;
                        int count = 0;
                        foreach (string f in files)
                        {
                            string fullPath = string.Empty;
                            try
                            {
                                var video = new P_VideoLibrary();
                                var fi = new FileInfo(f);
                                string[] xxx = f.Split('\\');
                                string tenFile = xxx[xxx.Length - 1];
                                video.Name = tenFile;
                                string[] yyy = tenFile.Split('.');
                                string type = yyy[yyy.Length - 1];
                                tenFile = tenFile.Replace("." + type, "");
                                video.Type = fi.Extension;
                                video.Length = 0;
                                video.Size = fi.Length;

                                var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                                shortPath = convertToUnSign(shortPath);

                                //if (!Directory.Exists(Application.StartupPath + @"\Media"))
                                //    Directory.CreateDirectory(Application.StartupPath + @"\Media");
                                //string fullPath = Application.StartupPath + @"\Media\" + shortPath;

                                if (!Directory.Exists(UrlSaveMediaFile))
                                    Directory.CreateDirectory(UrlSaveMediaFile);
                                fullPath = UrlSaveMediaFile + shortPath;

                                File.Copy(f, fullPath);

                                video.Path = shortPath;
                                video.IsDeleted = false;
                                BLLVideoLibrary.CreateOrUpdate(video);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "/n path: " + fullPath + " : " + UrlSaveMediaFile);
                            }
                        }
                        LoadVideoToGridView();
                        MessageBox.Show("Đã thêm " + count + " Tệp video thành công.", "Thông Báo");
                    }
                    else
                        MessageBox.Show("Không tìm thấy tệp video trong thư mục bạn vừa chọn.", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridVideo_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                txtName_v.Text = gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "Name") != null ? gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "Name").ToString() : string.Empty;
                videoId = 0;
                int.TryParse(gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "Id").ToString(), out videoId);
                chkIsActive.Enabled = true;

                btnAdd_v.Enabled = false;
                btnAddMulti_v.Enabled = false;
                btnUpdate_v.Enabled = true;
                btnDelete_v.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnChooseVideo_Click(object sender, EventArgs e)
        {
            try
            {
                dlg = new OpenFileDialog();
                dlg.Filter = "file Video|*.mp4|all file|*.*";
                if (string.IsNullOrEmpty(pathChooseFile))
                    dlg.InitialDirectory = @"C:\";
                else
                    dlg.InitialDirectory = pathChooseFile;
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] tmp = dlg.FileNames;
                    foreach (string i in tmp)
                    {
                        FileInfo fi = new FileInfo(i);
                        string[] xxx = i.Split('\\');
                        string tenFile = xxx[xxx.Length - 1];
                        txtName.Text = tenFile;
                        string[] yyy = tenFile.Split('.');
                        string type = yyy[yyy.Length - 1];
                        tenFile = tenFile.Replace("." + type, "");
                        DateTime dtime = DateTime.Now;
                        var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                        shortPath = convertToUnSign(shortPath);

                        txtVideoName.Text = shortPath;
                        txtName_v.Text = tenFile;
                        pathChooseFile = i.Substring(0, i.Length - tenFile.Length);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        #endregion


        #region Video Method
        private void LoadVideoToGridView()
        {
            try
            {
                gridControl1.DataSource = null;
                gridControl1.DataSource = BLLVideoLibrary.Gets();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveVideo()
        {
            try
            {
                if (dlg != null && !string.IsNullOrEmpty(dlg.FileName))
                {
                    var dtime = DateTime.Now;
                    var f = dlg.FileName;
                    var video = new P_VideoLibrary();
                    var fi = new FileInfo(f);
                    string[] xxx = f.Split('\\');
                    string tenFile = xxx[xxx.Length - 1], fullPath = "";

                    string[] yyy = tenFile.Split('.');
                    string type = yyy[yyy.Length - 1];
                    tenFile = tenFile.Replace("." + type, "");

                    var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                    shortPath = convertToUnSign(shortPath);

                    if (!Directory.Exists(UrlSaveMediaFile))
                        Directory.CreateDirectory(UrlSaveMediaFile);
                    fullPath = UrlSaveMediaFile + shortPath;

                    File.Copy(f, fullPath);

                    video.Name = txtName_v.Text;
                    video.Type = fi.Extension;
                    video.Length = 0;
                    video.Size = fi.Length;
                    video.Path = shortPath;
                    video.IsDeleted = false;
                    var result = BLLVideoLibrary.CreateOrUpdate(video);


                    //var video = new P_VideoLibrary();
                    //var fi = new FileInfo(f);
                    //string[] xxx = f.Split('\\');
                    //string tenFile = xxx[xxx.Length - 1];
                    //video.Name = tenFile;
                    //string[] yyy = tenFile.Split('.');
                    //string type = yyy[yyy.Length - 1];
                    //tenFile = tenFile.Replace("." + type, "");
                    //video.Type = fi.Extension;
                    //video.Length = 0;
                    //video.Size = fi.Length;

                    //var shortPath = (tenFile.Length > 17 ? tenFile.Substring(0, 17) : tenFile) + "_" + dtime.ToString("yyMMddhhmmss") + "." + type;
                    //shortPath = convertToUnSign(shortPath); 

                    //if (!Directory.Exists(UrlSaveMediaFile))
                    //    Directory.CreateDirectory(UrlSaveMediaFile);
                    //fullPath = UrlSaveMediaFile + shortPath;

                    //File.Copy(f, fullPath);

                    //video.Path = shortPath;
                    //video.IsDeleted = false;
                    //BLLVideoLibrary.CreateOrUpdate(video);


                    if (result.IsSuccess)
                    {
                        ResetVideoForm();
                        LoadVideoToGridView();
                    }
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
                else
                    MessageBox.Show("Vui lòng chọn tệp!.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetVideoForm()
        {
            txtName_v.Text = string.Empty;
            videoId = 0;

            btnDelete_v.Enabled = false;
            btnUpdate_v.Enabled = false;
            btnAdd_v.Enabled = true;
            btnAddMulti_v.Enabled = true;
            if (dlg != null)
                dlg.FileName = string.Empty;
        }

        private bool CheckVideoValidate()
        {
            var flag = true;
            try
            {
                if (string.IsNullOrEmpty(txtName_v.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tên tệp.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }
        #endregion
        #endregion
    }
}
