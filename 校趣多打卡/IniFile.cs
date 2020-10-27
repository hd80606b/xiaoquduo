using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace 校趣多打卡

{
    class IniFile
    {
        public string Path;

        public IniFile(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// 遍历INI文件的键
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="iniFilename">文件路径</param>
        /// <returns></returns>
        public static List<string> ReadKeys(string SectionName, string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(SectionName, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        /// <summary>
        /// 遍历INI文件节点
        /// </summary>
        /// <param name="iniFilename">文件路径</param>
        /// <returns></returns>
        public static List<string> ReadSections(string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// 读INI文件方法A
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbuilder对象</param>
        /// <param name="size">字节对象</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern uint GetPrivateProfileStringA(string section, string key, string def, byte[] retval, int size, string filePath);
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="iValue"></param>
        public void WriteContentValue(string section, string key, string iValue)
        {
            WritePrivateProfileString(section, key, iValue, this.Path);
        }

        /// <summary>
        /// 读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        public string ReadContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, this.Path);
            return temp.ToString();
        }
    }
}
