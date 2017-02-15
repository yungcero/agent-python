﻿using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

public class updateClass
{

    private string htpVersion;
    private string parentProc;
    public string parentPath { set; get; }
    public string[] arguments { set; get; }

    private static string launcherProcName = "HTPLauncherUpd.exe";

    public void setVersion(string versionID)
    {
        htpVersion = versionID;
    }


    public void setParent(string strParentProc)
    {
        parentProc = strParentProc;
    }

    public void runUpdate()
	{


        string currentBin = Environment.GetCommandLineArgs()[0]; //Grab current bin name
        if (currentBin == launcherProcName) //Check if we are the launcher
        {

            //If spawned bin, grab original passed name
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] != "debug")
                {
                    parentProc = (arguments[i]);
                }
                break;
            }

            //Looks like user isn't using custom name, use the default one
            if (string.IsNullOrEmpty(parentProc))
            {
                parentProc = "hashtopussy.exe";
            }

            waitForProcess(parentProc);
            File.Copy(launcherProcName, parentProc, true);
            Process reSpawn = new Process();
            reSpawn.StartInfo.FileName = parentProc;
            reSpawn.Start();
            Environment.Exit(0);
        }
        else //We are either user-run bin or spanwed bin
        {


            waitForProcess(launcherProcName);
            if (File.Exists(launcherProcName))
            {
                Console.WriteLine("Cleaning up files post update");
                File.Delete(launcherProcName);
            }

            //Check whether we need updating
                //Need code to send current version to server, probably a hash?
            //Download the launcher
                //Need to process res and download 

            //Run the launcher and exit current
            /*
            Process Spawn = new Process();
            Spawn.StartInfo.FileName = launcherProcName;
            Spawn.StartInfo.Arguments = currentBin;
            Spawn.Start();
            Environment.Exit(0);
            */
        }
    }


    private Boolean waitForProcess(string procName)
    {
        Process[] proc = Process.GetProcessesByName(procName);
        while (proc.Length != 0)
        {
            Thread.Sleep(500);
            proc = Process.GetProcessesByName(procName);
            Console.WriteLine("Waiting for parent");
        }
        return true;
    }
}