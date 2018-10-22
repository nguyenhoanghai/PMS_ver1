using System;
using System.Collections.Generic;
using System.Text;
//using WMPLib;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Media;
using System.Threading;
using System.Runtime.InteropServices;
//using System.Windows.Media;

namespace DuAn03_HaiDang.DATAACCESS
{
    class clsSound
    {
        private ArrayList alInput=null;
        //private WMPLib.WindowsMediaPlayer wm;
        private SoundExArray SoundArray = new SoundExArray();
        private SoundTempArray SoundTmpArray = new SoundTempArray();
        //private string sTemplate_Code = "1";
        private static string sTemplate_Code = "1";

        public static string SoundTemplate
        {
            get
            {
                return sTemplate_Code;
            }
            set
            {
                sTemplate_Code = value;
            }
        }

        

        // Add Input 1: doc tung so, 2: doc tung tu
        public void AddInput(string Field_Name, string Type)
        {
            alInput.Add(Field_Name);
            alInput.Add(Type);
        }

        

        public void PlaySound(string sRegion, string sRoom, string sBed)
        {
            //int sLen = 0;
            //string ki_tu = "";
            //string ten_file = "";

            //string myPlaylist = "Sample";

            //WMPLib.IWMPPlaylist pl;
            //WMPLib.IWMPPlaylistArray plItems;

            //plItems = wm.playlistCollection.getByName(myPlaylist);
            //if (plItems.count == 0)
            //    pl = wm.playlistCollection.newPlaylist(myPlaylist);
            //else
            //{
            //    pl = plItems.Item(0);
            //    pl.clear();
            //}

            //// Add file
            ////MyMsgBox.MsgInformation(SoundTmpArray.Count.ToString(),"");
            //foreach (SoundTempInfo sti in SoundTmpArray)
            //{
            //    switch (sti.iSound_type_id)
            //    {
            //        case 4: //Other
            //            {
            //                if (sti.iSound_id.ToString() != "")
            //                {
            //                    ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
            //                    WMPLib.IWMPMedia m1 = wm.newMedia(ten_file);
            //                    pl.appendItem(m1);
            //                }
            //                break;
            //            }
            //        case 3: //Bed
            //            {
            //                //Add Thêm âm Giường

            //                //Add Âm số giuong
            //                sLen = sBed.Length;
            //                for (int i = 0; i < sLen; i++)
            //                {
            //                    ki_tu = sBed.Substring(i, 1);
            //                    ten_file = SoundArray.GetValue(ki_tu).sound_path;
            //                    WMPLib.IWMPMedia m1 = wm.newMedia(ten_file);
            //                    pl.appendItem(m1);
            //                }
            //                break;
            //            }
            //        case 2: //Room
            //            {
            //                //Add Thêm âm Room

            //                // So Room
            //                sLen = sRoom.Length;
            //                for (int i = 0; i < sLen; i++)
            //                {
            //                    ki_tu = sRoom.Substring(i, 1);
            //                    ten_file = SoundArray.GetValue(ki_tu).sound_path;
            //                    WMPLib.IWMPMedia m1 = wm.newMedia(ten_file);
            //                    pl.appendItem(m1);
            //                }
            //                break;
            //            }
            //        case 1: //Region
            //            {
            //                //Add Thêm âm Region

            //                // So Room
            //                sLen = sRegion.Length;
            //                for (int i = 0; i < sLen; i++)
            //                {
            //                    ki_tu = sRegion.Substring(i, 1);
            //                    ten_file = SoundArray.GetValue(ki_tu).sound_path;
            //                    WMPLib.IWMPMedia m1 = wm.newMedia(ten_file);
            //                    pl.appendItem(m1);
            //                }
            //                break;
            //            }
            //        //default:
            //        //    break;
            //    }
            //}

            ////Load temlate sound
            //wm.currentPlaylist = pl;
            //wm.controls.play();
        }

        public void PlaySoundWav(string sRegion, string sRoom, string sBed)
        {
            int sLen = 0;
            string ki_tu = "";
            string ten_file = "";
            ArrayList alFiles = new ArrayList();
            SoundPlayer sp = new SoundPlayer();

            foreach (SoundTempInfo sti in SoundTmpArray)
            {
                switch (sti.iSound_type_id)
                {
                    case 4: //Other
                        {
                            if (sti.iSound_id.ToString() != "")
                            {
                                try
                                {
                                    ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                                }
                                catch
                                {
                                    ten_file = "";
                                }

                                if (ten_file.Length > 0)
                                {
                                    alFiles.Add(ten_file);
                                }
                            }
                            break;
                        }
                    case 3: //Bed
                        {
                            if (sBed == "00")
                            {
                                //Add Thêm Toilet
                                try
                                {
                                    ten_file = SoundArray.GetValue(sBed).sound_path;
                                }
                                catch
                                {
                                    ten_file = "";
                                }

                                if (ten_file.Length > 0)
                                {
                                    alFiles.Add(ten_file);
                                }
                            }
                            else
                            {
                                //Add Thêm âm Giường
                                try
                                {
                                    ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                                }
                                catch
                                {
                                    ten_file = "";
                                }

                                if (ten_file.Length > 0)
                                {
                                    alFiles.Add(ten_file);
                                }

                                //Add Âm số giuong
                                sLen = sBed.Length;

                                if (sti.iNread > 0)
                                {
                                    if (sti.iNread <= sLen)
                                    {
                                        sBed = sBed.Substring(sLen - sti.iNread, sti.iNread);
                                    }
                                }

                                sLen = sBed.Length; // Tính chiều dài lại sau khi cắt bớt
                                for (int i = 0; i < sLen; i++)
                                {
                                    ki_tu = sBed.Substring(i, 1);
                                    try
                                    {
                                        ten_file = SoundArray.GetValue(ki_tu).sound_path;
                                    }
                                    catch
                                    {
                                        ten_file = "";
                                    }

                                    if (ten_file.Length > 0)
                                    {
                                        alFiles.Add(ten_file);
                                    }
                                }
                            }
                            break;
                        }
                    case 2: //Room
                        {
                            //Add Thêm âm Room
                            try
                            {
                                ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                            }
                            catch
                            {
                                ten_file = "";
                            }

                            //ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                            if (ten_file.Length > 0)
                            {
                                alFiles.Add(ten_file);
                            }

                            // So Room
                            sLen = sRoom.Length;

                            if (sti.iNread > 0)
                            {
                                //sLen = sRoom.Length;
                                if (sti.iNread <= sLen)
                                {
                                    sRoom = sRoom.Substring(sLen - sti.iNread, sti.iNread);
                                }

                            }

                            sLen = sRoom.Length;
                            for (int i = 0; i < sLen; i++)
                            {
                                ki_tu = sRoom.Substring(i, 1);
                                try
                                {
                                    //ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                                    ten_file = SoundArray.GetValue(ki_tu).sound_path;
                                }
                                catch
                                {
                                    ten_file = "";
                                }
                                //ten_file = SoundArray.GetValue(ki_tu).sound_path;
                                if (ten_file.Length > 0)
                                {
                                    alFiles.Add(ten_file);
                                }
                            }
                            break;
                        }
                    case 1: //Region
                        {
                            //Add Thêm âm Region
                            try
                            {
                                ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                            }
                            catch
                            {
                                ten_file = "";
                            }

                            //ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                            if (ten_file.Length > 0)
                            {
                                alFiles.Add(ten_file);
                            }
                            // So Region
                            //sLen = sRegion.Length;
                            //for (int i = 0; i < sLen; i++)
                            //{
                            //    ki_tu = sRegion.Substring(i, 1);
                            try
                            {
                                //ten_file = SoundArray.GetValue(int.Parse(sti.iSound_id.ToString())).sound_path;
                                ten_file = SoundArray.GetValue(sRegion).sound_path;
                            }
                            catch
                            {
                                ten_file = "";
                            }

                            //ten_file = SoundArray.GetValue(sRegion).sound_path;
                            if (ten_file.Length > 0)
                            {
                                alFiles.Add(ten_file);
                            }
                            //}
                            break;
                        }
                    default:
                        break;
                }


            }

            for (int j = 0; j < alFiles.Count; j++)
            {
                try
                {
                    int iTime = SoundInfo.GetSoundLength(alFiles[j].ToString());
                    sp.SoundLocation = alFiles[j].ToString();
                    sp.Play();
                    Thread.Sleep(iTime);
                }
                catch
                {

                }
            }
        }

        //public void PlaySound1()
        //{
        //    // co the cai tien doc file tu thu muc luon neu doc tu database cham
        //    int sLen = 0;
        //    string ki_tu = "";
        //    string sdau_vao = "";
        //    string ten_file = "";

        //    DataProvider provider = new DataProvider();
        //    string strSQL = "";

        //    SqlDataReader dr;

        //    System.Media.SoundPlayer player = new System.Media.SoundPlayer();

        //    sLen = sdau_vao.Length;

        //    //plItems = wm.playlistCollection.getByName(myPlaylist);
        //    //if (plItems.count == 0)
        //    //    pl = wm.playlistCollection.newPlaylist(myPlaylist);
        //    //else
        //    //{
        //    //    pl = plItems.Item(0);
        //    //    pl.clear();
        //    //}

        //    // New ngay 13/09
        //    if (alInput.Count > 0)
        //    {
        //        for (int j = 0; j < alInput.Count - 1; j += 2)
        //        {
        //            if (alInput[j + 1].ToString() == "1")
        //            {
        //                sdau_vao = alInput[j].ToString().Trim();
        //                sLen = sdau_vao.Length;
        //                for (int i = 0; i < sLen; i++)
        //                {
        //                    ki_tu = sdau_vao.Substring(i, 1);
        //                    strSQL = "SELECT FILE_AM_THANH FROM AM_THANH WHERE AM_THANH_ID = '" + ki_tu + "'";
        //                    dr = provider.excuteQuery(strSQL);

        //                    if (dr.HasRows)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            ten_file = dr["FILE_AM_THANH"].ToString();
        //                        }
        //                    }

        //                    if (ten_file == "")
        //                    {
        //                        MessageBox.Show("Thiếu file âm thanh " + ki_tu + "");
        //                        break;
        //                        //exit for;
        //                    }
        //                    else
        //                    {
        //                        //System.Media.SoundPlayer player = new System.Media.SoundPlayer(ten_file);
        //                        player.SoundLocation = ten_file;
        //                        player.Play();
        //                        Thread.Sleep(400);
        //                    }
        //                }
        //            }

        //            if (alInput[j + 1].ToString() == "2")
        //            {
        //                string pattern = " ";
        //                Regex rg = new Regex(pattern);
        //                string[] arResult = rg.Split(alInput[j].ToString());
        //                foreach (string subString in arResult)
        //                {
        //                    strSQL = "SELECT FILE_AM_THANH FROM AM_THANH WHERE AM_THANH_ID = '" + subString + "'";
        //                    dr = provider.excuteQuery(strSQL);
        //                    if (dr.HasRows)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            ten_file = dr["FILE_AM_THANH"].ToString();
        //                        }
        //                    }

        //                    if (ten_file == "")
        //                    {
        //                        MessageBox.Show("Thiếu file âm thanh " + subString + "");
        //                        break;
        //                        //exit for;
        //                    }
        //                    else
        //                    {
        //                        //System.Media.SoundPlayer player = new System.Media.SoundPlayer(ten_file);
        //                        player.SoundLocation = ten_file;
        //                        player.Play();
        //                        Thread.Sleep(400);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //player.Dispose();
        //}

        public void Close()
        {
            //if (wm.openState == WMPOpenState.wmposMediaOpen)
            try
            {
                //wm.close();
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            // Thực hiện công việc dọn dẹp
            // Yêu cầu bộ thu dọc GC trong thực hiện kết thúc
            GC.SuppressFinalize(this);
        }

        //public override void Finalize()
        //{
        //    Dispose();
        //    base.Finalize();
        //}

            

        

        private string ReadSoundFile(int iSound_id)
        {
            //foreach (SoundExInfo si in SoundArray)
            //{
            //    MessageBox.Show(si.sound_path);
            //}
            return (SoundArray.GetIndex(iSound_id).ToString());
        }

        private string ReadSoundFile(string sSound_code)
        {
            //foreach (SoundExInfo si in SoundArray)
            //{
            //    MessageBox.Show(si.sound_path);
            //}
            //return (SoundArray.GetValue("8").sound_path);
            return (SoundArray.GetValue("8").ToString());
        }
    }

    public class SoundExInfo
    {
        public int sound_id;
        public string sound_code;
        public int sound_type;
        public string sound_path;

        public SoundExInfo()
        {
        }
        public SoundExInfo(int i_sound_id, string s_sound_code, int i_sound_type, string s_sound_path)
        {
            this.sound_id = i_sound_id;
            this.sound_code = s_sound_code;
            this.sound_type = i_sound_type;
            this.sound_path = s_sound_path;
        }
    }

    public class SoundExArray : CollectionBase
    {
        public void Add(SoundExInfo si)
        {
            List.Add(si);
        }

        public SoundExInfo this[int index]
        {
            get { return (SoundExInfo)List[index]; }
            set { List[index] = value; }
        }

        public int GetIndex(int i_sound_id)
        {
            int iFound = -1;
            bool bFound = false;
            int i = 0;
            while (i < this.Count && !bFound)
            {
                if (this[i].sound_id == i_sound_id)
                {
                    iFound = i;
                    bFound = true;
                }
                else
                {
                    i++;
                }
            }
            return iFound;
        }

        public int GetIndex(string s_sound_code)
        {
            int iFound = -1;
            bool bFound = false;
            int i = 0;
            while (i < this.Count && !bFound)
            {
                if (this[i].sound_code == s_sound_code)
                {
                    iFound = i;
                    bFound = true;
                }
                else
                {
                    i++;
                }
            }
            return iFound;
        }

        public SoundExInfo GetValue(int i_sound_id)
        {
            int iIndex = this.GetIndex(i_sound_id);
            return (iIndex != -1 ? this[iIndex] : null);
        }

        public SoundExInfo GetValue(string s_sound_code)
        {
            int iIndex = this.GetIndex(s_sound_code);
            return (iIndex != -1 ? this[iIndex] : null);
        }
    }

    public class SoundTempInfo
    {
        public int iSound_template_id;
        public string sSound_template_code;
        public int iSound_type_id;
        public int iSound_id;
        public int iNread;

        public SoundTempInfo()
        {
        }

        public SoundTempInfo(int isound_template_id, string ssound_template_code, int isound_type_id, int isound_id)
        {
            this.iSound_template_id = isound_template_id;
            this.sSound_template_code = ssound_template_code;
            this.iSound_type_id = isound_type_id;
            this.iSound_id = isound_id;
        }

        public SoundTempInfo(int isound_template_id, string ssound_template_code, int isound_type_id, int isound_id, int inread)
        {
            this.iSound_template_id = isound_template_id;
            this.sSound_template_code = ssound_template_code;
            this.iSound_type_id = isound_type_id;
            this.iSound_id = isound_id;
            this.iNread = inread;
        }
    }

    public class SoundTempArray : CollectionBase
    {
        public void Add(SoundTempInfo si)
        {
            List.Add(si);
        }

        public SoundTempInfo this[int index]
        {
            get { return (SoundTempInfo)List[index]; }
            set { List[index] = value; }
        }

        public int GetIndex(int isound_template_id)
        {
            int iFound = -1;
            bool bFound = false;
            int i = 0;
            while (i < this.Count && !bFound)
            {
                if (this[i].iSound_template_id == isound_template_id)
                {
                    iFound = i;
                    bFound = true;
                }
                else
                {
                    i++;
                }
            }
            return iFound;
        }

        public int GetIndex(string ssound_template_code)
        {
            int iFound = -1;
            bool bFound = false;
            int i = 0;
            while (i < this.Count && !bFound)
            {
                if (this[i].sSound_template_code == ssound_template_code)
                {
                    iFound = i;
                    bFound = true;
                }
                else
                {
                    i++;
                }
            }
            return iFound;
        }

        public SoundTempInfo GetValue(int isound_template_id)
        {
            int iIndex = this.GetIndex(isound_template_id);
            return (iIndex != -1 ? this[iIndex] : null);
        }

        public SoundTempInfo GetValue(string ssound_template_code)
        {
            int iIndex = this.GetIndex(ssound_template_code);
            return (iIndex != -1 ? this[iIndex] : null);
        }
    }

    public static class SoundInfo
    {
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        public static int GetSoundLength(string fileName)
        {
            try
            {
                StringBuilder lengthBuf = new StringBuilder(32);

                mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
                mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
                mciSendString("close wave", null, 0, IntPtr.Zero);

                int length = 0;
                int.TryParse(lengthBuf.ToString(), out length);

                return length;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //public static int GetTime(string fileName)
        //{
        //    MediaPlayer player = new MediaPlayer();
        //    Uri path = new Uri(@"C:\test.wav");
        //    player.Open(path);
        //    Duration duration = player.NaturalDuration;
        //    if (duration.HasTimeSpan)
        //    {
        //        Console.WriteLine(player.NaturalDuration.TimeSpan.ToString());
        //    }
        //    player.Close();
        //}
    }
}


