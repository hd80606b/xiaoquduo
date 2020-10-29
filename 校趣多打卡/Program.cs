using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using RestSharp;

namespace 校趣多打卡
{
    class Program
    {
  

        static void Main(string[] args)
        {
            IniFile newIniFile;
            string appDataDic = Directory.GetCurrentDirectory() + @"\数据.ini";
            string path = Directory.GetCurrentDirectory() + @"\log.txt";
            if (!File.Exists(path))  //判断是否存在log，如不存在则创建
            {
                FileStream stream = System.IO.File.Create(path);
                stream.Close();
                stream.Dispose();
            }
           
                if (!File.Exists(appDataDic))  //判断是否存在ini，如不存在则创建
            {
                FileStream fs = new FileStream(appDataDic, FileMode.Create);
                fs.Flush();
                fs.Close();
                newIniFile = new IniFile(appDataDic);
                newIniFile.WriteContentValue("test", "Cookie", "JSESSIONID=XXXXXXXXXXXXXXX");
                newIniFile.WriteContentValue("test", "CheckPlace", "宇宙-银河系-地球");
                newIniFile.WriteContentValue("test", "Temperature", "36.5");
                newIniFile.WriteContentValue("test", "Phone", "XXXXXXXXXXX");
                newIniFile.WriteContentValue("test", "livingPlace", "宇宙-银河系-地球");
                newIniFile.WriteContentValue("test", "livingPlaceDetail", "亚洲");
                newIniFile.WriteContentValue("test", "checkPlaceProvince", "宇宙");
                newIniFile.WriteContentValue("test", "checkPlaceCity", "银河系");
                newIniFile.WriteContentValue("test", "checkPlaceArea", "地球");
                newIniFile.WriteContentValue("1", "Cookie", "JSESSIONID=");
                newIniFile.WriteContentValue("1", "CheckPlace", "");
                newIniFile.WriteContentValue("1", "Temperature", "36.4");
                newIniFile.WriteContentValue("1", "Phone", "");
                newIniFile.WriteContentValue("1", "livingPlace", "");
                newIniFile.WriteContentValue("1", "livingPlaceDetail", "");
                newIniFile.WriteContentValue("1", "checkPlaceProvince", "");
                newIniFile.WriteContentValue("1", "checkPlaceCity", "");
                newIniFile.WriteContentValue("1", "checkPlaceArea", "");
            }
            StreamWriter writer = new StreamWriter(path, true);
            newIniFile = new IniFile(appDataDic);  //建立对象，并遍历所有节点名，不为test的运行
            List<string> list = IniFile.ReadSections(appDataDic);
            for (int i=1;i<list.Count;i++)
            {
                var client = new RestClient("https://mps.zocedu.com/corona/submitHealthCheck/submit");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cookie", newIniFile.ReadContentValue(list[i], "Cookie"));
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("checkPlace", newIniFile.ReadContentValue(list[i], "CheckPlace"));
                request.AddParameter("contactMethod", newIniFile.ReadContentValue(list[i], "Phone"));
                request.AddParameter("teacher", "");
                request.AddParameter("temperature", newIniFile.ReadContentValue(list[i], "Temperature"));
                request.AddParameter("isCohabitFever", "否");
                request.AddParameter("isLeavePalce", "否");
                request.AddParameter("beenPlace", "");
                request.AddParameter("isContactNcov", "否");
                request.AddParameter("livingPlace", newIniFile.ReadContentValue(list[i], "livingPlace"));
                request.AddParameter("livingPlaceDetail", newIniFile.ReadContentValue(list[i], "livingPlaceDetail"));
                request.AddParameter("name1", "");
                request.AddParameter("relation1", "");
                request.AddParameter("phone1", "");
                request.AddParameter("name2", "");
                request.AddParameter("relation2", "");
                request.AddParameter("phone2", "");
                request.AddParameter("remark", "");
                request.AddParameter("extraInfo", "[]");
                request.AddParameter("healthStatus", "z");
                request.AddParameter("emergencyContactMethod", "[]");
                request.AddParameter("checkPlacePoint", "124,37");
                request.AddParameter("checkPlaceDetail", "");
                request.AddParameter("checkPlaceCountry", "");
                request.AddParameter("checkPlaceProvince", newIniFile.ReadContentValue(list[i], "checkPlaceProvince"));
                request.AddParameter("checkPlaceCity", newIniFile.ReadContentValue(list[i], "checkPlaceCity"));
                request.AddParameter("checkPlaceArea", newIniFile.ReadContentValue(list[i], "checkPlaceArea"));
                IRestResponse response = client.Execute(request);
                if (response.Content == "")
                {
                    Console.WriteLine("已成功打卡\n");
                    writer.WriteLine(DateTime.Now + "\0" + list[i] + "已成功打卡\n");
                    writer.Flush();
                    
                }
                else
                if(response.Content.Contains("html"))
                {
                    Console.WriteLine(response.Content + "\n打卡失败，请检查cookie是否出错\n");
                    writer.WriteLine(DateTime.Now + "\0" + list[i] + "打卡失败，请检查cookie是否出错\n");
                    writer.Flush();
                   
                }
                else
                {
                    Console.WriteLine(response.Content + "\n打卡失败\n");
                    writer.WriteLine(DateTime.Now + "\0"+ list[i] + "打卡失败"+ response.Content);
                    writer.Flush();
                    
                    //Console.WriteLine(list[i]);
                }
                
            }
            writer.Close();
            Thread.Sleep(3000);
        }
    }
}
