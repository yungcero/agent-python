﻿using System;
using System.IO;
using System.Diagnostics;


namespace hashtopussy
{

    class _7zClass
    {

        public class dlProps
        {
            public string action = "download";
            public string type = "7zr";
            public string token { get; set; }
        }

        public int osID { get; set; }
        public string tokenID { get; set; }
        public string appPath { get; set; }
        public string connectURL { get; set; }

        string binPath = "";

        public Boolean init7z()
        {

            binPath = Path.Combine(appPath, "7zr");
            if (osID == 1)
            {
                binPath += ".exe";
            }
            Console.WriteLine(binPath);

            if (!File.Exists(binPath))
            {
                Console.WriteLine("Download 7zip binary");
                jsonClass jsC = new jsonClass { debugFlag = true, connectURL = connectURL };

                dlProps dlzip = new dlProps
                {
                    token = tokenID
                };


                string jsonString = jsC.toJson(dlzip);
                string ret = jsC.jsonSend(jsonString);

                if (jsC.isJsonSuccess(ret))
                {
                    string base64bin = jsC.getRetVar(ret, "executable");
                    byte[] binArray = System.Convert.FromBase64String(base64bin);
                    File.WriteAllBytes(binPath, binArray);
                }

            }

            if (File.Exists(binPath))
            {
                return true;
            }

            return false;
            
        }

        //Code from hashtopus
        public Boolean xtract(string archivePath, string outDir, string files = "")
        {
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.FileName = binPath;
            pinfo.WorkingDirectory = appPath;
            pinfo.Arguments = "x -y -o\"" + outDir + "\" \"" + archivePath + "\"";

            Process unpak = new Process();
            unpak.StartInfo = pinfo;

            if (files != "") unpak.StartInfo.Arguments += " " + files;

            Console.WriteLine("Extracting archive " + archivePath + "...");

            try
            {
                if (!unpak.Start()) return false;
            }
            catch
            {
                Console.WriteLine("Could not start 7zr.");
                return false;
            }
            finally
            {
                unpak.WaitForExit();
            }
            
            return true;

        }
    }
}
