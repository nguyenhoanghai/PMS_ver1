using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class Chuyen_SanPham
    {
        public Chuyen_SanPham()
        {
            this.IsFinishBTPThoatChuyen = false;
        }
        public string STT { get; set; }
        public int STTThucHien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string MaChuyen { get; set; }
        public string MaSanPham { get; set; }
        public int SanLuongKeHoach { get; set; }
        public float NangXuatSanXuat { get; set; }
        public int LuyKeTH { get; set; }
        public int LuyKeBTPThoatChuyen { get; set; }
        public int IsFinish { get; set; }
        public bool IsFinishBTPThoatChuyen { get; set; }
        public DateTime TimeAdd { get; set; }
        public int IsFinishNow { get; set; }
        public string SoundChuyen { get; set; }
        public int LuyKeBTP { get; set; }
        public string TenChuyen { get; set; }
        public string TenSanPham { get; set; }
        public float TGCheTaoSP { get; set; }
        public int SoPhanCong { get; set; }
        public bool  IsMoveQuantityFromMorthOld { get; set; }
    }
}
