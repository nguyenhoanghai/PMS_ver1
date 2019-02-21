/*
cập nhật tính năng web ko load dc thông tin MaHang 
để nhập SL Hoàn Tất sau khi MaHang kết thúc sản xuất
*/

ALTER TABLE CHUYEN_SANPHAM ADD HideForever bit default(0)
GO

UPDATE Chuyen_SanPham SET HideForever = IsFinish  
GO