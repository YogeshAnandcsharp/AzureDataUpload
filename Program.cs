using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace AzureDataUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
             try
            {
              //  DateTime T1 = DateTime.Now;
                string constring = ConfigurationManager.ConnectionStrings["AzureStorageAccount"].ConnectionString;
                string localfolder = ConfigurationManager.AppSettings["SourceFolder"];
                string destconatiner = ConfigurationManager.AppSettings["DestFolder"];
                // Get a Reference to Storage Account
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Connecting to Azure Account....");
                //Console.WriteLine("\n \n \n");
                CloudStorageAccount csa = CloudStorageAccount.Parse(constring);
                CloudBlobClient cbc = csa.CreateCloudBlobClient();
                Console.WriteLine("Connection to Azure Account has been Established..");

                //Get a Reference to a Container
                Console.WriteLine("Getting Reference to a container");

               // Console.WriteLine("\n \n");

               

                CloudBlobContainer container = cbc.GetContainerReference(destconatiner);

                //Create this if it doesnot exist

                container.CreateIfNotExists();

                // Upload Files

                string[] files = Directory.GetFiles(localfolder);
                int count = 1;
                foreach (string filepath in files)
                {
                    try
                    {

                     //   TimerCallback callback = new TimerCallback(Tick);

                     //   Console.WriteLine("Creating timer: {0}\n",
                     //                      DateTime.Now.ToString("h:mm:ss"));

                     //   // create a one second timer tick
                     //   Timer stateTimer = new Timer(callback, null, 0, 1000);
                     //   Thread.Sleep(100);
                        if (count == 1)
                        {
                            watch.Start();
                            System.Threading.Thread.Sleep(1);

                            string key = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss") + "-" + Path.GetFileName(filepath);
                            UploadBlob(container, key, filepath, true);
                            //   watch.Stop();

                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("Total Elapsed Time in Miliseconds is " + count + " .  " + watch.ElapsedMilliseconds);
                            Console.ForegroundColor = ConsoleColor.White;
                            count += 1;
                            Console.Read();
                            return;
                        }
                        else
                        {

                        }
                    }
                    catch (Exception E)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(" Error Message thrown by the programme is " + E.Message);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        // Console.WriteLine("\n \n");
                        Console.WriteLine("Connection got Failed.........");
                        //  Console.WriteLine("\n \n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                

            //    DateTime T2 = DateTime.Now;
              //  TimeSpan Ts = (T2 - T1);
              //  string S = Ts.Milliseconds.ToString();
              //  Console.WriteLine("Elapsed time is " + S);
                
            }
            catch (Exception X)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Error Message thrown by the programme is "+X.Message);
                Console.ForegroundColor = ConsoleColor.DarkRed;
               // Console.WriteLine("\n \n");
                Console.WriteLine("Connection got Failed.........");
              //  Console.WriteLine("\n \n");
                Console.ForegroundColor = ConsoleColor.White;
            }
             
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("End of the Program");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
        static void UploadBlob(CloudBlobContainer Container,string key,string fileName,bool deleteAfter)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(" Uploading File to Container: Key="+key+" Source file = "+fileName);
           // Getting a Blob Reference to write to this file
            CloudBlockBlob cbb = Container.GetBlockBlobReference(key);

            // write this File
            using (var fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                cbb.UploadFromStream(fs);
              
            }

            // if Delete of the file requested then do that
            if (deleteAfter)
            {
                File.Delete(fileName);
                Console.WriteLine("file deleted");
            }
        }

        }
    }

