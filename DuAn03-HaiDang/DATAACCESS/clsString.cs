using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace DuAn03_HaiDang.DATAACCESS
{
    public class clsString
    {
        public static byte[] StrToByteArray(string s) //, out int discarded)
        {
            s = s.Replace(" ", string.Empty);
            //byte byteVal = 0;
            char[] arrayC;
            byte[] buffer = new byte[s.Length];
            arrayC = new char[s.Length];
            arrayC = s.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                //    chrT = System.Convert.ToChar(s.Substring(i, 1));
                buffer[i] = System.Convert.ToByte(arrayC[i]);
                //MessageBox.Show(arrayC[i].ToString());
            }

            return buffer;
        }

        public static string ByteArrayToStr(byte[] input)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string str = enc.GetString(input);
            return str;
        }

        public static string XOR(string s) //, out int discarded)
        {
            //s = s.Replace(" ", string.Empty);
            //byte byteVal = 0;
            string sResult = "";
            try
            {
                char[] arrayC;
                byte buffer = 0;
                arrayC = new char[s.Length];
                arrayC = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    //    chrT = System.Convert.ToChar(s.Substring(i, 1));
                    //buffer[i] = System.Convert.ToByte(arrayC[i]);
                    //MessageBox.Show(arrayC[i].ToString());
                    buffer ^= System.Convert.ToByte(arrayC[i]);
                }

                //StringBuilder builder = new StringBuilder(1 * 3);
                //builder.Append(Convert.ToString(buffer, 16).PadLeft(2, '0').PadRight(3, ' '));
                sResult = Convert.ToString(buffer, 16).PadLeft(2, '0').PadRight(3, ' ');
            }
            catch (Exception  )
            {
            }
            return sResult.ToUpper().Trim(); // buffer;
        }

        public static byte XorString(string s) //, out int discarded)
        {
            s = s.Replace(" ", string.Empty);
            //byte byteVal = 0;
            //string sResult = "";
            char[] arrayC;
            byte buffer = 0;
            arrayC = new char[s.Length];
            arrayC = s.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                //chrT = System.Convert.ToChar(s.Substring(i, 1));
                //buffer[i] = System.Convert.ToByte(arrayC[i]);
                //MessageBox.Show(arrayC[i].ToString());
                buffer ^= System.Convert.ToByte(arrayC[i]);
            }

            //StringBuilder builder = new StringBuilder(1 * 3);
            //builder.Append(Convert.ToString(buffer, 16).PadLeft(2, '0').PadRight(3, ' '));
            //sResult = Convert.ToString(buffer, 16).PadLeft(2, '0').PadRight(3, ' ');
            return buffer;
        }

        public static byte XORN(string s) //, out int discarded)
        {
            //s = s.Replace(" ", string.Empty);
            //byte byteVal = 0;
            char[] arrayC;
            byte buffer = 0;
            arrayC = new char[s.Length];
            arrayC = s.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                //    chrT = System.Convert.ToChar(s.Substring(i, 1));
                //buffer[i] = System.Convert.ToByte(arrayC[i]);
                //MessageBox.Show(arrayC[i].ToString());
                buffer ^= System.Convert.ToByte(arrayC[i]);
            }

            return buffer;
        }

        public static string ByteToHex(byte[] comByte)
        {
            //create a new StringBuilder object
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            //return the converted value
            return builder.ToString().ToUpper();
        }

        public static byte[] HexToByte(string msg)
        {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");
            //create a byte array the length of the
            //string divided by 2
            byte[] comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte
                //and add to the array
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
            //return the array
            return comBuffer;
        }

        public static string GenerateString(int Length, string sinput, int p1, int p2, int p3, int p4, int NumAdd, int pNumAdd)
        {
            try
            {
                int size = 0;
                string Input = "";
                char ch;
                string strTmp = "";
                Random rd = new Random((int)DateTime.Now.Ticks);
                string CodeChar = "0123456789";//ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                StringBuilder sb = new StringBuilder();
                size = Length;
                Input = sinput;
                for (int i = 0; i < size; i++)
                {

                    if (i == (p1 - 1))
                    {
                        strTmp = (Convert.ToInt32(Input.Substring(0, 1)) + NumAdd).ToString();
                        ch = Convert.ToChar(strTmp.Substring(strTmp.Length - 1, 1));
                        sb.Append(ch);
                    }
                    else if (i == (p2 - 1))
                    {
                        strTmp = (Convert.ToInt32(Input.Substring(1, 1)) + NumAdd).ToString();
                        ch = Convert.ToChar(strTmp.Substring(strTmp.Length - 1, 1));
                        sb.Append(ch);
                    }
                    else if (i == (p3 - 1))
                    {
                        strTmp = (Convert.ToInt32(Input.Substring(2, 1)) + NumAdd).ToString();
                        ch = Convert.ToChar(strTmp.Substring(strTmp.Length - 1, 1));
                        sb.Append(ch);
                    }
                    else if (i == (p4 - 1))
                    {
                        strTmp = (Convert.ToInt32(Input.Substring(3, 1)) + NumAdd).ToString();
                        ch = Convert.ToChar(strTmp.Substring(strTmp.Length - 1, 1));
                        sb.Append(ch);
                    }
                    else if (i == (pNumAdd - 1))
                    {
                        sb.Append(NumAdd);
                    }
                    else
                    {
                        sb.Append(CodeChar.Substring(rd.Next(0, CodeChar.Length - 1), 1));
                    }

                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        public string RandomString()
        {
            int size = 6;
            Random rd = new Random((int)DateTime.Now.Ticks); ;
            string Khoa = "0123456789"; //ABCDEFGHIJKLMNOPQRSTUVWXYZ
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
                sb.Append(Khoa.Substring(rd.Next(0, Khoa.Length - 1), 1));
            return sb.ToString();

        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public static string Ascii2HexString(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:X2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string Ascii2HexStringNull(string sInput)
        {
            string hex = "";
            try
            {
                foreach (char c in sInput)
                {
                    int tmp = c;
                    hex += String.Format("{0:X2}", (uint)System.Convert.ToUInt32(tmp.ToString())) + " ";
                }
            }
            catch (Exception)
            {
            }
            return hex.Trim();
        }

        public static string HexString2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                hexString = hexString.Replace(" ", ""); // Xóa khoảng trắng
                hexString = hexString.Replace("-", ""); // Xóa gạch ngang
                for (int i = 0; i <= hexString.Length - 2; i += 2)
                {
                    sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
                }
            }
            catch (Exception)
            {
            }
            return sb.ToString();
        }


        public static string ConvertHexStringToString(String hexInput, System.Text.Encoding encoding)
        {
            hexInput = hexInput.Replace(" ", "");
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        public static byte[] HexString2Byte(string hex)
        {
            hex = hex.Replace("-", "");
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
            //String to bytes:
            //byte[] bytes = Encoding.UTF8.GetBytes("foo");

            //Bytes to string:
            //string foo = Encoding.ASCII.GetString(bytes);
        }

        public static string ChangeNum2VNStr(double num)
        {
            string strTemp;
            strTemp = num.ToString();

            int daucham = strTemp.IndexOf(".") + strTemp.IndexOf(",") + 1;
            if (daucham > 0)
            {
                string s1 = strTemp.Substring(0, daucham);
                string s2 = (strTemp + "00").Substring(daucham + 1, 2);
                return Num2VNStr(s1) + " phẩy " + Num2VNStr(s2) + " đồng";
            }
            else
            {
                return Num2VNStr(strTemp) + " đồng";
            }
        }

        private static string RemoveNumberZero(string pnum)
        {
            int vt = 0;
            try
            {
                while (pnum[vt].Equals('0'))
                {
                    vt++;
                }
            }
            catch (Exception)
            {
            }
            return pnum.Substring(vt, pnum.Length - vt);
        }

        public static string Num2VNStr(string num)//doc so chan
        {
            string strResult = ""; // Chuoi chua ket qua tra ve
            try
            {
                if (num.Equals("0"))
                {
                    strResult = "không";
                }
                else
                {
                    num = RemoveNumberZero(num);
                    if (num[0].ToString().Equals("-"))
                    {
                        strResult = "âm ";
                        num = num.Substring(1, num.Length - 1);
                    }
                    if (num[0].ToString().Equals("+"))
                    {
                        num = num.Substring(1, num.Length - 1);
                    }
                    string[] arrayNumStr = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

                    string[] arrayDonVi = new string[4] { "", "ngàn", "triệu", "tỷ" };

                    string strDonVi;
                    int temp;

                    string strTemp;
                    strTemp = num;

                    int nLen;
                    nLen = strTemp.Length;

                    int i, vt;
                    int a;

                    for (i = 0; i < nLen; i++)
                    {
                        strDonVi = "";
                        vt = (nLen - i - 1) % 3;
                        a = int.Parse(strTemp.Substring(i, 1));

                        switch (a)
                        {
                            case 1:
                                if ((vt == 0) && (i > 0))
                                {
                                    if ((strTemp.Substring(i - 1, 1) != "1") && (strTemp.Substring(i - 1, 1) != "0"))
                                        arrayNumStr[1] = "mốt";
                                    else
                                        arrayNumStr[1] = "một";
                                }
                                else
                                {
                                    if (vt == 1) arrayNumStr[1] = "mười";
                                    else arrayNumStr[1] = "một";
                                }
                                break;

                            case 5:
                                if ((vt == 0) && (i != 0))
                                {
                                    if (strTemp.Substring(i - 1, 1) != "0")
                                        arrayNumStr[5] = "lăm";
                                    else
                                        arrayNumStr[5] = "năm";
                                }
                                else
                                    arrayNumStr[5] = "năm";
                                break;

                            case 0:
                                arrayNumStr[0] = "";
                                if (vt == 0 && nLen.Equals(1))
                                {
                                    arrayNumStr[0] = "không";
                                }
                                else if (vt == 1)
                                {
                                    if (strTemp.Substring(i + 1, 1) != "0")
                                        arrayNumStr[0] = "lẻ";
                                }

                                else if (vt == 2)
                                {
                                    if ((strTemp.Substring(i + 1, 1) != "0") || (strTemp.Substring(i + 2, 1) != "0"))
                                        arrayNumStr[0] = "không";
                                }

                                break;
                        }// end of switch

                        switch (vt)
                        {
                            case 2:
                                if (i == 0)
                                    strDonVi = "trăm";
                                else if ((strTemp.Substring(i, 1) != "0") ||
                                (strTemp.Substring(i + 1, 1) != "0") ||
                                (strTemp.Substring(i + 2, 1) != "0"))
                                    strDonVi = "trăm";
                                break;
                            case 1:
                                if ((strTemp.Substring(i, 1) != "1") && (strTemp.Substring(i, 1) != "0"))
                                    strDonVi = "mươi";
                                break;
                            default:
                                temp = (nLen - i) / 3;
                                if (temp > 3) temp = temp % 3;
                                if (i > 2)
                                {
                                    if (strTemp.Substring(i - 2, 1) != "0" ||
                                    strTemp.Substring(i - 1, 1) != "0" ||
                                    strTemp.Substring(i, 1) != "0" ||
                                    i == strTemp.Length)
                                        strDonVi = arrayDonVi[temp];
                                }
                                else
                                    strDonVi = arrayDonVi[temp];
                                break;
                        }// end of switch

                        if (arrayNumStr[a] == "")
                            strResult = strResult.Trim() + " " + strDonVi;
                        else
                            strResult = strResult.Trim() + " " + arrayNumStr[a] + " " + strDonVi;
                    }// end of for
                }
            }
            catch (Exception)
            {
            }
            return strResult.Trim();
        }

        public static string Num2VNUnSign(string num)//doc so chan
        {
            string strResult;
            strResult = "";      // Chuoi chua ket qua tra ve
            if (num.Equals("0"))
            {
                strResult = "khong";
            }
            else
            {
                num = RemoveNumberZero(num);
                if (num[0].ToString().Equals("-"))
                {
                    strResult = "am ";
                    num = num.Substring(1, num.Length - 1);
                }
                if (num[0].ToString().Equals("+"))
                {
                    num = num.Substring(1, num.Length - 1);
                }
                string[] arrayNumStr = new string[10] { "khong", "mot", "hai", "ba", "bon", "nam", "sau", "bay", "tam", "chin" };

                string[] arrayDonVi = new string[4] { "", "ngan", "trieu", "ty" };

                string strDonVi;
                int temp;

                string strTemp;
                strTemp = num;

                int nLen;
                nLen = strTemp.Length;

                int i, vt;
                int a;

                for (i = 0; i < nLen; i++)
                {
                    strDonVi = "";
                    vt = (nLen - i - 1) % 3;
                    a = int.Parse(strTemp.Substring(i, 1));

                    switch (a)
                    {
                        case 1:
                            if ((vt == 0) && (i > 0))
                            {
                                if ((strTemp.Substring(i - 1, 1) != "1") && (strTemp.Substring(i - 1, 1) != "0"))
                                    arrayNumStr[1] = "mot1";
                                else
                                    arrayNumStr[1] = "mot";
                            }
                            else
                            {
                                if (vt == 1) arrayNumStr[1] = "muoi2";
                                else arrayNumStr[1] = "mot";
                            }
                            break;

                        case 5:
                            if ((vt == 0) && (i != 0))
                            {
                                if (strTemp.Substring(i - 1, 1) != "0")
                                    arrayNumStr[5] = "lam";
                                else
                                    arrayNumStr[5] = "nam";
                            }
                            else
                                arrayNumStr[5] = "nam";
                            break;

                        case 0:
                            arrayNumStr[0] = "";
                            if (vt == 0 && nLen.Equals(1))
                            {
                                arrayNumStr[0] = "khong";
                            }
                            else if (vt == 1)
                            {
                                if (strTemp.Substring(i + 1, 1) != "0")
                                    arrayNumStr[0] = "lẻ";
                            }

                            else if (vt == 2)
                            {
                                if ((strTemp.Substring(i + 1, 1) != "0") || (strTemp.Substring(i + 2, 1) != "0"))
                                    arrayNumStr[0] = "khong";
                            }

                            break;
                    }// end of switch

                    switch (vt)
                    {
                        case 2:
                            if (i == 0)
                                strDonVi = "tram";
                            else if ((strTemp.Substring(i, 1) != "0") ||
                            (strTemp.Substring(i + 1, 1) != "0") ||
                            (strTemp.Substring(i + 2, 1) != "0"))
                                strDonVi = "tram";
                            break;
                        case 1:
                            if ((strTemp.Substring(i, 1) != "1") && (strTemp.Substring(i, 1) != "0"))
                                strDonVi = "muoi";
                            break;
                        default:
                            temp = (nLen - i) / 3;
                            if (temp > 3) temp = temp % 3;
                            if (i > 2)
                            {
                                if (strTemp.Substring(i - 2, 1) != "0" ||
                                strTemp.Substring(i - 1, 1) != "0" ||
                                strTemp.Substring(i, 1) != "0" ||
                                i == strTemp.Length)
                                    strDonVi = arrayDonVi[temp];
                            }
                            else
                                strDonVi = arrayDonVi[temp];
                            break;
                    }// end of switch

                    if (arrayNumStr[a] == "")
                        strResult = strResult.Trim() + " " + strDonVi;
                    else
                        strResult = strResult.Trim() + " " + arrayNumStr[a] + " " + strDonVi;
                }// end of for

            }
            return strResult.Trim();
        }

        private void SendData(string value, SerialPort P)
        {
            try
            {
                string strCS = "";
                strCS = clsString.XOR(value);
                value = value + strCS;
                value = "02" + clsString.Ascii2HexStringNull(value) + "03";
                byte[] newMsg = HexStringToByteArray(value);
                //send the message to the port
                P.Write(newMsg, 0, newMsg.Length);
            }
            catch
            {

            }
        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            if (s.Length % 2 != 0)
                return new byte[] { 0x00 };
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

    }
}
