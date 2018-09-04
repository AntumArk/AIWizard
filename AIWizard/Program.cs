using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using AIWizard.res;

namespace AIWizard
{
    internal class Program
    {
        private static string buildFolderPath = "";
        private static string databaseWorkspaceFolderPath = "";
        private static string darknetFolderPath = "";
        private static string dbName = "";

        /// <summary>
        /// The list to hold past used databases
        /// </summary>
        private static string projects = "";

        private static void Main()
        {
            //    Console.OutputEncoding = Encoding.UTF8;

            FancyWriter.WriteSlow("Hello World!");
            FancyWriter.WriteSlow(strings.Intro);
            if (FancyReader.AwaitConfirmation())
            {
                while (!Run())
                {
                }
            }
        }

        /// <summary>
        /// Initializes first database.
        /// </summary>
        private static void Init()
        {
            FancyWriter.WriteSlow("Loading configurations...");
            // You can new up an instance
            //Load path
            if (File.Exists("bfp.txt"))
            {
                buildFolderPath = File.ReadAllText("bfp.txt");
                Console.WriteLine("Your tools build path: " + buildFolderPath);
            }
            else
            {
                buildFolderPath = FancyReader.AwaitDirectory(strings.BuildFolderPrompt);
                File.WriteAllText("bfp.txt", buildFolderPath);
                FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("bfp.txt") +
                                      " as your build folder");
            }
            if (File.Exists("drk.txt"))
            {
                darknetFolderPath = File.ReadAllText("drk.txt");
                Console.WriteLine("Your darknet build path: " + darknetFolderPath);
            }
            else
            {
                darknetFolderPath = FancyReader.AwaitDirectory(strings.DarknetPathPrompt);
                File.WriteAllText("drk.txt", darknetFolderPath);
                FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("drk.txt") +
                                      " as your darknet build folder");
            }

            if (File.Exists(Files.Projects))
            {   //It is not a first run. Request if you want to open different project.
                var projec = File.ReadAllLines(Files.Projects);
                FancyWriter.WriteSlow("Please select database you would like to edit");
                for (int i = 0; i < projec.Length; i++)
                {
                    FancyWriter.WriteSlow("[" + i + "]" + " " + projec[i]);
                }

                databaseWorkspaceFolderPath = File.ReadAllText("dbw.txt");
                Console.WriteLine("Your database workspace path: " + databaseWorkspaceFolderPath);
            }
            else
            {   //Thhis is the first run, lets request for a place and name of new database and add it to the project list.
                //Create projects file
                File.Create(Files.Projects);

                //Request for path for newe database
                databaseWorkspaceFolderPath = FancyReader.AwaitDirectory(strings.DBWorkspacePrompt);
                //Request name for new database
                dbName = FancyReader.AwaitAnswer(strings.DBNamePrompt);
                //Combine both
                databaseWorkspaceFolderPath = Path.Combine(databaseWorkspaceFolderPath, dbName);
                //Append new database to project list
                File.AppendAllText(Files.Projects, databaseWorkspaceFolderPath + '\n');
                FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("dbw.txt") +
                                      " as your  database workspace folder");
            }
        }

        private static bool Run()
        {
            Start:
            Console.WriteLine();
            FancyWriter.WriteSlow(strings.FirstPrompt);
            var answer = FancyReader.AwaitAnswer();
            switch (answer)
            {
                case "exit":
                    goto Exit;
                case "AI setup":
                    goto AISetup;
                case "prepare new database":
                    goto DBSetup;
                case "add to database":
                    goto DBSetup;
                case "change build folder":
                    ChangeToolBuildFolder(); break;
                case "change database folder":
                    ChangeDBWorkspaceFolder(); break;
                case "change darknet folder":
                    ChangeDaknetFolder(); break;
                case "backup current":
                    BackupDB();
                    break;

                case "transfer new dataset":
                    TransferAddition(FancyReader.AwaitDirectory("Please enter the directory of your dataset. Additions/Data . It should contain txt and jpg files."));
                    break;

                case "prepare for training":
                    GenerateTrainValidFiles();
                    break;

                case "test":
                    FancyWriter.WriteSlow("Please read the guide on testing");
                    OpenFolder(Path.Combine(buildFolderPath, "TestYolo4Video", "Debug"));
                    break;
            }
            goto Start;

            #region DatabaseSetupRegion

            DBSetup:

            if (!FancyReader.AwaitConfirmation("Have you done this before?"))
            {
                FancyWriter.WriteSlow(strings.DBPrep);
            }
            if (FancyReader.AwaitConfirmation("Are you sure you want to continue? There are a lot of steps for it."))
            {
                Console.WriteLine();
                Console.WriteLine("Creating folder structure...");
                Directory.CreateDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database"));
                Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database"));
                foreach (var folder in strings.DBFolderTree.Split('\n'))
                {
                    Directory.CreateDirectory(folder);
                }
                Console.WriteLine();
                FancyWriter.WriteHeader("Gather Videos");
                if (!FancyReader.AwaitConfirmation("Have you done this before?"))
                {
                    FancyWriter.WriteSlow(strings.GatherVideosHint);
                }
                GatherVideos:
                if (FancyReader.AwaitConfirmation("Have you gathered your videos?"))
                {
                    //TODO possible problems when the user wants to use only frames and not videos.
                    var curDir = "Additions";
                    Directory.SetCurrentDirectory(curDir);
                    var newDir = FancyReader.AwaitAnswer(
                        "Please enter the name for your new databse addition. I suggest using yy_mm_dd format.");
                    var videoDir = Path.Combine(newDir, "Videos");
                    var frameDir = Path.Combine(newDir, "Frames");
                    var gtDir = Path.Combine(newDir, "GTFiles");
                    var txtDir = Path.Combine(newDir, "TxtFiles");
                    //Directory containing both frames and txt files
                    var dataDir = Path.Combine(newDir, "Data");
                    Directory.CreateDirectory(videoDir);
                    Directory.CreateDirectory(frameDir);
                    Directory.CreateDirectory(txtDir);
                    Directory.CreateDirectory(dataDir);
                    //TODO Make this cross platform
                    OpenFolder(Path.Combine(databaseWorkspaceFolderPath, "Database", curDir, videoDir));
                    PasteVideos:
                    FancyWriter.WriteSlow("Paste in all of those videos and nothing else in this new folder.");

                    if (FancyReader.AwaitConfirmation("Have you pasted your videos?"))
                    {
                        if (CheckForVideos(videoDir))
                        {
                            FancyWriter.WriteSlow("We shall proceed with cutting.");
                            if (Directory.GetFiles(frameDir).Length > 0)
                            {
                                if (FancyReader.AwaitConfirmation(
                                    "There are already some cut frames inside frame folder. Do you still want to cut the video? This will overwrite current frames with the same name."))
                                {
                                    if (CutVideoToFrames(
                                        Path.Combine(databaseWorkspaceFolderPath, "Database", curDir, videoDir), frameDir))
                                    {
                                        FancyWriter.WriteSlow("Frames cut. Please inspect frames for any possible glitches.");
                                        Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database",
                                            curDir));
                                        OpenFolder(frameDir);
                                    }
                                }
                            }

                            {
                                Marking:
                                Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database", curDir));
                                FancyWriter.WriteSlow("Now when as we have our frames, we can begin marking them");
                                FancyWriter.WriteSlow("Please open your YoloGuide and follow 'Marking Objects' paragraph.");
                                SavingGTFiles:
                                FancyWriter.WriteSlow("Save your GT files to ");
                                Directory.CreateDirectory(gtDir);
                                OpenFolder(gtDir);
                                FancyWriter.WriteSlow("Come back to me when you are finished.");
                                if (FancyReader.AwaitConfirmation("Are you finished marking objects?"))
                                {
                                    FancyWriter.WriteSlow("Doing some data parsing...");
                                    FancyWriter.WriteHeader("Data Parsing");
                                    if (Directory.GetFiles(gtDir).All(c => c.EndsWith(".txt")))
                                    {
                                        var files = Directory.GetFiles(gtDir);
                                        FancyWriter.WriteSlow("Please select you latest file from this list, by entering integer.");
                                        var j = 0;
                                        foreach (var file in files)
                                        {
                                            Console.WriteLine("[" + j + "]" + file);
                                            j++;
                                        }

                                        var gtFile = files[FancyReader.AwaitPosUInt()];
                                        uint imgWidth = FancyReader.AwaitPosUInt("Please enter the width of your used video.");
                                        uint imgHeight = FancyReader.AwaitPosUInt("Please enter the height of your used video.");

                                        var rectMode = FancyReader.AwaitAnswer("Please enter rectangle mode you used for marking objects. It can be 'xywh' or 'cxcywh'");
                                        while (rectMode != "xywh" || rectMode != "cxcywh") //This is an odd solution

                                        {
                                            if (rectMode == "xywh" || rectMode == "cxcywh")
                                            {
                                                break;
                                            }
                                            FancyWriter.WriteSlow(rectMode + " - is not a correct answer. Try again.");
                                            rectMode = FancyReader.AwaitAnswer("Please enter rectangle mode you used for marking objects. It can be 'xywh' or 'cxcywh'");
                                        }

                                        Console.WriteLine("Attempting to parse data for txt files.");

                                        if (
                                           DataParserParse(
                                                Path.Combine(databaseWorkspaceFolderPath, "Database", curDir, frameDir), Path.Combine(databaseWorkspaceFolderPath, "Database", curDir, gtFile), rectMode
                                                , imgWidth, imgHeight, txtDir, dataDir, Path.Combine(databaseWorkspaceFolderPath, "Database", curDir)))
                                        {
                                            Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database", curDir));
                                            FancyWriter.WriteSlow("DataParsing successful! Cropping images for validation. This may take a while...");
                                            if (CropImages(Path.Combine(databaseWorkspaceFolderPath, "Database", curDir, dataDir)))
                                            {
                                                FancyWriter.WriteSlow(
                                                    "Cropping successful! Please carefully inspect all of the cut frames.");
                                                Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database", curDir));
                                                OpenFolder(dataDir);
                                                if (FancyReader.AwaitConfirmation("Have you finished inspection?"))
                                                {
                                                    if (FancyReader.AwaitConfirmation("Did you find any mistakes?"))
                                                    {
                                                        FancyWriter.WriteSlow(
                                                            "Please fix them and redo these steps again.");
                                                        goto Marking;
                                                    }
                                                    if (FancyReader.AwaitConfirmation("Do you want to augument new dataset? This will increase the quality of the database."))
                                                    {
                                                        Augment(Path.Combine(databaseWorkspaceFolderPath,
                                                            "Database", curDir, dataDir));
                                                    }
                                                    if (FancyReader.AwaitConfirmation("Do you want to add your new dataset to working('Current') folder?"))
                                                    {
                                                        TransferAddition(Path.Combine(databaseWorkspaceFolderPath,
                                                            "Database", curDir, dataDir));
                                                    }
                                                    if (FancyReader.AwaitConfirmation("Do you want to generate train and valid files for your new dataset?"))
                                                    {
                                                        GenerateTrainValidFiles();
                                                        FancyWriter.WriteSlow(
                                                            "Congratulations! You have finished database preparation! For the next step use training command.");
                                                    }
                                                    else
                                                    {
                                                        FancyWriter.WriteSlow(
                                                            "Congratulations! You have finished database preparation! For the next step use training preparation command.");
                                                    }

                                                    FancyWriter.WriteHeader("THE END");
                                                }
                                            }
                                            else
                                            {
                                                FancyWriter.WriteSlow(
                                                    "Cropping failed! ");
                                            }
                                        }
                                        else
                                        {
                                            FancyWriter.WriteSlow("Failed to parse data from gt file. Unfortunately you will have to try again.");
                                        }
                                    }
                                    else
                                    {
                                        FancyWriter.WriteSlow("You did not put GT file in the correct place...");
                                        goto SavingGTFiles;
                                    }
                                }
                            }
                        }
                        else
                        {
                            FancyWriter.WriteSlow("You liar! Do it again!");
                            goto PasteVideos;
                        }
                    }
                }
                else
                {
                    goto GatherVideos;
                }
            }
            goto Start;

            #endregion DatabaseSetupRegion

            #region AISetupRegion

            AISetup:
            FancyWriter.WriteSlow(strings.YoloSetup);
            if (FancyReader.AwaitConfirmation())
            {
                OpenUrl(strings.DarkNetGitSite);
            }
            goto Start;

            #endregion AISetupRegion

            #region ExitRegion

            Exit:
            FancyWriter.WriteSlow("Do you want to exit?");
            FancyReader.AwaitConfirmation();
            Thread.Sleep(1000);
            return true;

            #endregion ExitRegion
        }

        private static bool Augment(string crPath)
        {
            // BackupDB();
            FancyWriter.WriteSlow("Augmenting dataset");
            var dataPath = crPath;
            var targetPath = crPath;
            Directory.SetCurrentDirectory(crPath);
            var exeFolderPath = Path.Combine(buildFolderPath, "AugmentWholeImage", "Debug");
            var exePath = Path.Combine(buildFolderPath, exeFolderPath, "AugmentWholeImage.exe");
            if (!File.Exists(exePath))
            {
                FancyWriter.WriteSlow("Either your folder with videos is invalid, or your tools path. Please reconsider reconfiguring.");
                return false;
            }
            var myProcess = new Process();
            var info = new ProcessStartInfo(exePath, dataPath + "/ " + targetPath + "/") { UseShellExecute = true };
            myProcess.StartInfo = info;
            myProcess.Start();
            //string error = myProcess.StandardError.ReadToEnd();
            myProcess.WaitForExit();
            FancyWriter.WriteSlow("Generation completed.");
            return true;
        }

        private static bool GenerateTrainValidFiles()
        {
            FancyWriter.WriteHeader("Generating train.txt and valid.txt");
            Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database", "Current", "Configs"));
            var exeFolderPath = Path.Combine(buildFolderPath, "TrainValidFiles", "Debug");
            var exePath = Path.Combine(buildFolderPath, exeFolderPath, "TrainValidFiles.exe");
            if (!File.Exists(exePath))
            {
                FancyWriter.WriteSlow("Either your folder with videos is invalid, or your tools path. Please reconsider reconfiguring.");
                return false;
            }
            var myProcess = new Process();
            var info = new ProcessStartInfo(exePath, Path.Combine(databaseWorkspaceFolderPath, "Database", "Current", "logoData") + "/") { UseShellExecute = true };
            myProcess.StartInfo = info;
            myProcess.Start();
            //string error = myProcess.StandardError.ReadToEnd();
            myProcess.WaitForExit();
            FancyWriter.WriteSlow("Generation completed.");
            return true;
        }

        private static void TransferAddition(string crPath)
        {
            BackupDB();
            FancyWriter.WriteSlow("Copying to current");
            var dataPath = crPath;
            var targetPath = Path.Combine(databaseWorkspaceFolderPath, "Database", "Current", "logoData");
            DirectoryInfo drInfo = new DirectoryInfo(dataPath);
            int i = 0;
            foreach (var file in drInfo.GetFiles())
            {
                FancyWriter.UpdateStatus("Copying new dataset to current database", (ushort)i,
                    (ushort)drInfo.GetFiles().Length, true);
                file.CopyTo(Path.Combine(targetPath, file.Name), true);
                i++;
            }
        }

        private static bool CropImages(string dataDir)
        {
            FancyWriter.WriteHeader("Cropping for validation");
            if (!Directory.Exists(dataDir))
            {
                FancyWriter.WriteSlow("Your data directory does not exist");
                return false;
            }
            var exeFolderPath = Path.Combine(buildFolderPath, "cropImage", "Debug");
            var exePath = Path.Combine(buildFolderPath, exeFolderPath, "cropImage.exe"); //TODO make cross platform
            if (!File.Exists(exePath))
            {
                FancyWriter.WriteSlow("Your tools path is invalid. Please reconsider reconfiguring.");
                return false;
            }
            var cropFolder = Path.Combine(dataDir, "cropedImg");
            Directory.CreateDirectory(cropFolder);
            var classesFile = Path.Combine(darknetFolderPath, "data", "obj.names");
            if (!File.Exists(classesFile))
            {
                return false;
            }

            var objects = File.ReadAllLines(classesFile);
            foreach (var o in objects)
            {
                Directory.CreateDirectory(Path.Combine(cropFolder, o));
            }
            Directory.SetCurrentDirectory(dataDir);
            //Start application
            var myProcess = new Process();
            var info = new ProcessStartInfo(exePath,
                    dataDir + "/")
            { UseShellExecute = true };
            myProcess.StartInfo = info;
            myProcess.Start();
            myProcess.WaitForExit();

            //string error = myProcess.S
            return true;
        }

        private static bool DataParserParse(string framesPath, string gtFilePath, string rectMode, uint width,
            uint height, string txtSavePath, string combinedSavePath, string curDir)
        {
            FancyWriter.WriteHeader("GT Parsing");
            var exeFolderPath = Path.Combine(buildFolderPath, "DarkLabelDataParser", "Debug");
            var exePath = Path.Combine(buildFolderPath, exeFolderPath, "DarkLabelDataParser.exe"); //TODO make cross platform
            if (!File.Exists(exePath))
            {
                FancyWriter.WriteSlow("Your tools path is invalid. Please reconsider reconfiguring.");
                return false;
            }
            if (!File.Exists(gtFilePath))
            {
                FancyWriter.WriteSlow("Your gtFile path is invalid. Please check.");
                return false;
            }
            Directory.SetCurrentDirectory(txtSavePath);
            var myProcess = new Process();
            var info = new ProcessStartInfo(exePath,
                framesPath + "/ " + gtFilePath + " " + rectMode + " " + width + " " + height)
            { UseShellExecute = true };
            myProcess.StartInfo = info;
            myProcess.Start();
            //string error = myProcess.StandardError.ReadToEnd();
            myProcess.WaitForExit();
            Console.WriteLine("Parsing finished. Copying files to same directory.");

            var frames = Directory.GetFiles(Directory.GetCurrentDirectory()).Where(c => c.EndsWith(".txt")).ToList();
            Directory.SetCurrentDirectory(curDir);
            ushort p = 0;
            foreach (var frame in frames)
            {
                FancyWriter.UpdateStatus("Copying txt files... ", p, (ushort)frames.Count(), true);
                var pathToSaveTo = Path.Combine(combinedSavePath, Path.GetFileName(frame));
                if (!File.Exists(pathToSaveTo))
                {
                    File.Copy(frame, pathToSaveTo);
                }
                p++;
            }
            Directory.SetCurrentDirectory(framesPath);
            frames = Directory.GetFiles(framesPath).Where(c => c.EndsWith(".jpg")).ToList();
            Directory.SetCurrentDirectory(curDir);
            p = 0;
            foreach (var frame in frames)
            {
                FancyWriter.UpdateStatus("Copying jpg files... ", p, (ushort)frames.Count(), true);
                var pathToSaveTo = Path.Combine(combinedSavePath, Path.GetFileName(frame));
                if (!File.Exists(pathToSaveTo))
                {
                    File.Copy(frame, pathToSaveTo);
                }
                p++;
            }
            return true;
        }

        /// <summary>
        /// Opens the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <remarks>Author: https://github.com/mellinoe </remarks>
        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private static void ChangeToolBuildFolder()
        {
            FancyWriter.WriteSlow("Please enter new tools build folder");
            buildFolderPath = FancyReader.AwaitDirectory(strings.BuildFolderPrompt);
            File.WriteAllText("bfp.txt", buildFolderPath);
            FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("bfp.txt") +
                                  " as your build folder");
        }

        private static void ChangeDBWorkspaceFolder()
        {
            FancyWriter.WriteSlow("Please enter new Database workspace folder");

            databaseWorkspaceFolderPath = FancyReader.AwaitDirectory(strings.DBWorkspacePrompt);
            File.WriteAllText("dbw.txt", databaseWorkspaceFolderPath);
            FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("dbw.txt") +
                                  " as your database workspace folder");
        }

        private static void ChangeDaknetFolder()
        {
            FancyWriter.WriteSlow("Please enter new Darknet folder");
            darknetFolderPath = FancyReader.AwaitDirectory(strings.DBWorkspacePrompt);
            File.WriteAllText("drk.txt", darknetFolderPath);
            FancyWriter.WriteSlow("You have selected :" + File.ReadAllText("drk.txt") +
                                  " as your darknet build folder");
        }

        private static bool CheckForVideos(string path)
        {
            var files = Directory.GetFiles(path);
            return files.All(file => (file.Contains(".avi") || file.Contains(".mov") || file.Contains(".mp4")));
        }

        private static bool CutVideoToFrames(string videosPath, string savePath)
        {
            FancyWriter.WriteHeader("Video to frames cutting");
            var exeFolderPath = Path.Combine(buildFolderPath, "FrameCut", "Debug");
            var exePath = Path.Combine(buildFolderPath, exeFolderPath, "FrameCut.exe");
            if (!CheckForVideos(videosPath) || !File.Exists(exePath))
            {
                FancyWriter.WriteSlow("Either your folder with videos is invalid, or your tools path. Please reconsider reconfiguring.");
                return false;
            }
            Directory.SetCurrentDirectory(savePath);
            var myProcess = new Process();
            var info = new ProcessStartInfo(exePath, videosPath + "/") { UseShellExecute = true };
            myProcess.StartInfo = info;
            myProcess.Start();
            //string error = myProcess.StandardError.ReadToEnd();
            myProcess.WaitForExit();
            //            var frames = Directory.GetFiles(exeFolderPath).Where(c => c.EndsWith(".jpg"));
            //            ushort p = 0;
            //            foreach (var frame in frames)
            //            {
            //                FancyWriter.UpdateStatus("Copying frames... ", p, (ushort)frames.Count(), true);
            //                File.Copy(Path.Combine(exeFolderPath, frame), Path.Combine(savePath, frame));
            //                p++;
            //            }

            return true;
        }

        private static void OpenFolder(string path)
        {
            Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", path);
        }

        public static void CopyDir(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyDirAll(diSource, diTarget);
        }

        public static void CopyDirAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            Directory.CreateDirectory(Path.Combine(target.FullName, source.Name));

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, source.Name, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private static void BackupDB()
        {   //The way backup may work:
            //We backup our current database, which will be located in workspace.
            //After backup is made, we can then add additions to the current database
            //And then transfer our current database to training build folder.
            FancyWriter.WriteHeader("Database backup");
            if (FancyReader.AwaitConfirmation(
                "Are you sure to continue? This action could take a really long time if your database is huge."))
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd_hh_mm");
                var backupDir = Path.Combine("Backup", date);
                var currentDir = "Current";
                var logoBackupDir = Path.Combine(backupDir, "logoData");
                var weightsBackupDir = Path.Combine(backupDir, "weights");
                Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database"));

                Directory.CreateDirectory(backupDir);
                //                Directory.CreateDirectory(logoBackupDir);
                //                Directory.CreateDirectory(weightsBackupDir);
                Directory.GetDirectories(currentDir).ToList().ForEach(c => CopyDir(c, backupDir));
                FancyWriter.WriteSlow("Backup finished");
            }
        }

        private static void AddObjects()
        {
            Redo:
            FancyWriter.WriteSlow("Please enter objects you are planning to track. Type 'finished' if you are done.");

            var input = FancyReader.AwaitAnswer();
            List<string> oobjList = new List<string>();
            while (input != "finished")
            {
                oobjList.Add(input);
                input = FancyReader.AwaitAnswer();
            }

            if (oobjList.Count <= 0)
            {
                FancyWriter.WriteSlow("Your list is empty! We can't have that. Try again with at least one object.");
                goto Redo;
            }

            for (int i = 0; i < oobjList.Count; i++)
            {
                FancyWriter.WriteSlow("[" + i + "]" + " " + oobjList[i]);
            }
            if (!FancyReader.AwaitConfirmation("Is the list looking correct?"))
            {
                FancyWriter.WriteSlow("You will try again");
                goto Redo;
            }
            FancyWriter.WriteSlow("Appending to obj.names file");
            Directory.SetCurrentDirectory(Path.Combine(databaseWorkspaceFolderPath, "Database", Branches.CurrentConfigs));
            File.AppendAllLines(Files.Classes, oobjList);
            var newList = File.ReadAllLines(Files.Classes).ToList();
            FancyWriter.WriteSlow("This is your new object list. Count: " + newList.Count);
            for (int i = 0; i < newList.Count; i++)
            {
                FancyWriter.WriteSlow("[" + i + "]" + " " + newList[i]);
            }

            var objdata = File.ReadAllLines(Files.ClassData).ToList();
            objdata[0] = "classes = " + newList.Count;
            File.WriteAllLines(Files.ClassData, objdata);
            FancyWriter.WriteSlow("Please inspect obj files");
            OpenFolder(Directory.GetCurrentDirectory());
        }
    }
}

//string[] filePaths = Directory.GetFiles(@"c:\Maps\", "*.txt",SearchOption.TopDirectoryOnly);