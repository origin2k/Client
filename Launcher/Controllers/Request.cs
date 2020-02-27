﻿using System;
using System.IO;
using System.Net;
using System.Text;
using ComponentAce.Compression.Libs.zlib;

namespace Launcher
{
    public static class Request
    {
        public static string Send(string url, string data)
        {
            // set https protocol
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // create request  
            WebRequest request = WebRequest.Create(new Uri(Globals.LauncherConfig.BackendUrl + url));
            byte[] bytes = SimpleZlib.CompressToBytes(data, zlibConst.Z_BEST_COMPRESSION);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;

            // send request
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            // receive response
            WebResponse response = request.GetResponse();
            string result = "";

            // get response data
            using (Stream stream = response.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZOutputStream zip = new ZOutputStream(ms, zlibConst.Z_BEST_COMPRESSION))
                    {
                        stream.CopyTo(zip);
                        zip.CopyTo(ms);
                        result = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            
            return result;
        }
    }
}