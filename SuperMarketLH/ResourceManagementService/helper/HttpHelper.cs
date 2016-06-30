using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ResourceManagementService.helper
{
    public class HttpHelper
    {
        
        private static string HEARD_URL = "rest/supermarket/connect";
        private static string SEND_FILE_URL = "rest/supermarket/file";
        private static string STOP_APPLICATION = "rest/supermarket/stop";
        private static string START_APPLICATION = "rest/supermarket/start";
        /// <summary>
        /// 发送心跳数据
        /// </summary>
        /// <returns></returns>
        public static Boolean heart(string ip) {
            string url = string.Format("http://{0}:8080/{1}",ip,HEARD_URL);
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest httpWebReq = (HttpWebRequest)WebRequest.Create(url);
                httpWebReq.Method = "GET";
                httpWebReq.ContentType = "application/json";
                httpWebResponse = (HttpWebResponse)httpWebReq.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                if (responseContent.Equals("OK"))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally {
                if(httpWebResponse!=null)httpWebResponse.Close();
                if(streamReader!=null)streamReader.Close();
            }
           
            return false;
        }

        public static Boolean sendFileShap(string ip, string fileName,string saveFileName)
        {
            string url = string.Format("http://{0}:8080", ip);
            var client = new RestClient(url);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(SEND_FILE_URL, Method.POST);
            request.AddParameter("fileName", saveFileName); // adds to POST or URL querystring based on Method
           

            // add files to upload (works with compatible verbs)
            request.AddFile("file", fileName);

            // execute the request
            IRestResponse response = client.Execute(request);
            string content = response.Content; // raw content as string

            if (content.Equals("OK"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Boolean sendFile(string ip,string fileName) {

            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 文件参数头  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var fileHeader = string.Format(filePartHeader, "file", fileName);
            var fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);

            // 开始拼数据  
            var memStream = new MemoryStream();
            memStream.Write(beginBoundary, 0, beginBoundary.Length);

            // 文件数据  
            memStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            var buffer = new byte[1024];
            int bytesRead; // =0  
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            // Key-Value数据  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            Dictionary<string, string> stringDict = new Dictionary<string, string>();
            stringDict.Add("fileName", fileName);
           
            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            //倒腾到tempBuffer?  
            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            // 创建webRequest并设置属性  
            HttpWebResponse httpWebResponse = null;
            HttpWebRequest webRequest = null;
            try
            {

                webRequest = (HttpWebRequest)WebRequest.Create(SEND_FILE_URL);
                webRequest.Method = "POST";
                webRequest.Timeout = 100000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                webRequest.ContentLength = tempBuffer.Length;

                var requestStream = webRequest.GetRequestStream();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                string responseContent;
                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    responseContent = httpStreamReader.ReadToEnd();
                    if (responseContent.Equals("OK"))
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }

            }
            catch (Exception e)
            {
                return false;
            }
            finally {
                httpWebResponse.Close();
                webRequest.Abort();
            }


           
        }
    



        /// <summary>
        /// 关闭应用程序
        /// </summary>
        /// <returns></returns>
        public static Boolean stop(string ip) {
            string url = string.Format("http://{0}:8080/{1}", ip, STOP_APPLICATION);
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest httpWebReq = (HttpWebRequest)WebRequest.Create(url);
                httpWebReq.Method = "GET";
                httpWebReq.ContentType = "application/json";
                httpWebResponse = (HttpWebResponse)httpWebReq.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                if (responseContent.Equals("OK"))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (httpWebResponse != null) httpWebResponse.Close();
                if (streamReader != null) streamReader.Close();
            }
            return false;
        }

        public static Boolean start(string ip) {
            string url = string.Format("http://{0}:8080/{1}", ip, START_APPLICATION);
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest httpWebReq = (HttpWebRequest)WebRequest.Create(url);
                httpWebReq.Method = "GET";
                httpWebReq.ContentType = "application/json";
                httpWebResponse = (HttpWebResponse)httpWebReq.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                if (responseContent.Equals("OK"))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (httpWebResponse != null) httpWebResponse.Close();
                if (streamReader != null) streamReader.Close();
            }
            return false;
        }
    }
}
