using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Security.Cryptography;
using System.Configuration;
using System.Drawing;
using DuAn03_HaiDang.POJO;

/// <summary>
/// Summary description for DBClass
/// </summary>
namespace DuAn03_HaiDang.DATAACCESS
{
    public static class dbclass
    {       
        private static SqlConnection con=null;
        private static SqlDataAdapter da=null;
        private static DataTable dt = new DataTable();
        private static SqlCommand cmd;
        private static DataSet ds = null;
        public static String error;
        public static SqlDataReader dr = null;
        public static String Strsql;
        public static String password = "myhanh123456";
        public static String server;
        public static String data;
        public static String user;
        public static String pass;
        public static List<AppConfig> listAppConfig;

        public static string GetConnectionString()
        {
            string connectionString = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(data))
                {
                    if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(pass))
                    {
                        connectionString = "Data Source=" + server + ";Initial Catalog=" + data + ";Integrated Security=True;";
                    }
                    else
                        connectionString = "Data Source=" + server + ";Initial Catalog=" + data + "; User Id=" + user + ";Password=" + pass + ";";                    
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return connectionString;
        }
        
        public static SqlConnection taoketnoi()
        {
            try
            {
                con = new SqlConnection(GetConnectionString());
                con.Open();
            }
            catch (Exception e)
            {
                error = e.Message;
                throw e;
               
            }
            return con;
        }

        public static SqlConnection taoketnoi(string strConnect)
        {
            try
            {
                con = new SqlConnection(strConnect);
                con.Open();                
            }
            catch (Exception e)
            {
                throw e;
            }
            return con;
        }

        public static void ngatketnoi()
        {
            con.Close();
        }

        public static int ExecuteSqlTransaction(List<string> listStrSQL)
        {
            int result = 0;
            con = taoketnoi();
            cmd = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("Transaction");
            cmd.Connection = con;
            cmd.Transaction = transaction;
            try
            {
                if(listStrSQL!=null && listStrSQL.Count>0)
                {
                    foreach(string strSQL in listStrSQL)
                    {
                        cmd.CommandText = strSQL;
                        cmd.ExecuteNonQuery();
                    }
                }                
                transaction.Commit();
                result = 1;
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            return result;
        }

        public static DataTable TruyVan_TraVe_DataTable(string strSQL)
        {
            dt = new DataTable();
            try
            {
                con = taoketnoi();
                if (con.State == ConnectionState.Open)
                {
                    da = new SqlDataAdapter(strSQL, con);
                    da.Fill(dt);
                    ngatketnoi();                    
                }
                else
                 error = "Không thể kết nối với CSDL"; 
            }
            catch (Exception ex)
            {
                ngatketnoi();             
            }          
            
           return dt;
            
        }

        public static DataTable TruyVan_TraVe_DataTable_Con(string strSQL, SqlConnection conn)
        {
            dt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    da = new SqlDataAdapter(strSQL, conn);
                    da.Fill(dt);
                    ngatketnoi(); 
                }
                else
                {
                    error = "Không thể kết nối với CSDL";                    
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return dt;
        }

        public static int TruyVan_XuLy(string strSQL)
        {
            int result = -1;
            try
            {
                con = taoketnoi();
                if (con.State == ConnectionState.Open)
                {
                    cmd = new SqlCommand(strSQL, con);
                    result = cmd.ExecuteNonQuery();
                    ngatketnoi();                    
                }
                else
                {
                    error = "Không thể kết nối với CSDL";
                }
            }
            catch (Exception ex)
            {
                ngatketnoi();  
                throw ex;
            }
            return result;    
           
        }

        public static int TruyVan_XuLy_Con(string strSQL, SqlConnection conn, SqlTransaction transaction)
        {
            int result = -1;
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.Transaction = transaction;
                    result = cmd.ExecuteNonQuery();
                    ngatketnoi();
                }
                else
                {
                    error = "Không thể kết nối với CSDL";                   
                }
            }
            catch (Exception ex)
            {
                ngatketnoi();
                throw ex;
            }
            return result;


        }
        
        public static void loadcombobox(ComboBox cb, string strselect, byte chiso)
        {            
            try
            {
                dt = new DataTable();
                dt = TruyVan_TraVe_DataTable(strselect);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cb.Items.Add(dt.Rows[i][chiso]);

                    }
                    cb.Text = dt.Rows[0][chiso].ToString();
                }
                    
            }
            catch (Exception e)
            {
                throw e;                    
            }
       
        }

        public static void TextboxAutocomplete(TextBox txt, string strselect, byte chiso)
        {
            try
            {
                con = taoketnoi();
                if (con.State == ConnectionState.Open)
                {
                    cmd = new SqlCommand(strselect, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        txt.AutoCompleteCustomSource.Add(dr[chiso].ToString());
                    }
                    ngatketnoi();
                }
            }
            catch (Exception ex)
            {
                ngatketnoi();
                throw ex;
            }
            
        }

        public static void loaddatagridview(DataGridView dg, string strselect)
        {
            try
            {
                con = taoketnoi();
                if (con.State == ConnectionState.Open)
                {
                    ds = new DataSet();
                    da = new SqlDataAdapter(strselect, con);
                    da.Fill(ds, "tenbang");
                    dg.DataSource = ds.Tables[0];
                    ngatketnoi();
                }
            }
            catch (Exception ex)
            {
                ngatketnoi();
                throw ex;
            }
            
        }

        public static void loaddataridviewcolorrow(DataGridView dg, DataTable dt)
        {
            try
            {
                dg.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    DataGridViewCell cell;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dynamic value = dt.Rows[i][j].ToString().Trim();
                        if (value == "False" || value == "True")
                        {
                            cell = new DataGridViewCheckBoxCell();
                            cell.Value = value;
                        }
                        else
                        {
                            cell = new DataGridViewTextBoxCell();
                            cell.Value = value;
                        }
                        row.Cells.Add(cell);
                    }
                    dg.Rows.Add(row);
                }
                for (int i = 1; i < dt.Rows.Count; i += 2)
                {
                    dg.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
         
       
        public static int Truyvan_Xuly_co_Thamso(string strSQL, SqlParameter[] thamso)
        {           
            con = taoketnoi();
            if (con.State == ConnectionState.Open)
            {
                try
                {
                    cmd = new SqlCommand(strSQL, con);
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        cmd.Parameters.Add(thamso[i]);
                    }
                    int sodong = cmd.ExecuteNonQuery();
                    ngatketnoi();
                    return sodong;
                }
                catch (Exception e)
                {
                    error = e.Message;
                    ngatketnoi();
                    return -1;
                }
            }
            else
            {
                error = "Lỗi kết nối CSDL";
                return -1;
            }
        }

        public static DataTable Truyvan_TraVe_Table_co_Thamso(string strSQL, SqlParameter[] thamso)
        {            
            dt = new DataTable();
            con = taoketnoi();
            if (con.State == ConnectionState.Open)
            {
                try
                {
                    cmd = new SqlCommand(strSQL, con);
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        cmd.Parameters.Add(thamso[i]);
                    }
                    da = new SqlDataAdapter(cmd);

                    da.Fill(dt);
                    ngatketnoi();
                    return dt;
                }
                catch (Exception e)
                {
                    error = e.Message;
                    ngatketnoi();
                    return dt;
                }
            }
            else
            {
                error = "Lỗi kết nối CSDL";
                return dt;
            }
        }

        public static string LayMaCuoiCung(string nameTable, string nameSelectColumn, string orderByColumn)
        {
            string sql = "SELECT TOP 1 * FROM " + nameTable + " ORDER BY " + orderByColumn + " DESC";
            string LastID = "";
            dt = TruyVan_TraVe_DataTable(sql);
            if (dt.Rows.Count != 0)
            {
                LastID = dt.Rows[0][nameSelectColumn].ToString();
            }
            return LastID;
        }

        public static string TaoMaKeTiep(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0001";
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }
        
        public static bool kt(string dauvao, string strsql, byte chiso)
        {
            bool ok = false;
            con = taoketnoi();
            cmd = new SqlCommand(strsql, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[chiso].ToString().ToLower() == dauvao.ToLower())
                    ok = true;
            }
            ngatketnoi();
            return ok;
        }

        public static string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool verifyMd5Hash(string input, string hash)
        {
            string hashOfInput = getMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
                return true;
            else
                return false;
        }

        //Hàm mã hóa chuỗi
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Buoc 1: Bam chuoi su dung MD5

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Cai dat bo ma hoa

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert chuoi (Message) thanh dang byte[]

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Ma hoa chuoi

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }

            finally
            {
                // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Tra ve chuoi da ma hoa bang thuat toan Base64
            return Convert.ToBase64String(Results);

        }

        //Hàm giải mã chuỗi
        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            // Step 1. Bam chuoi su dung MD5
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
            // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            // Step 3. Cai dat bo giai ma
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            // Step 4. Convert chuoi (Message) thanh dang byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            // Step 5. Bat dau giai ma chuoi
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            // Step 6. Tra ve ket qua bang dinh dang UTF8
            return UTF8.GetString(Results);
        }

    }
}

