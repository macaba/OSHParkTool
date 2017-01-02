using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSHParkTool
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Dictionary<string, string> rules = new Dictionary<string, string>();
            rules.Add("CADCAM Top Copper.TXT", ".GTL");
            rules.Add("CADCAM Bottom Copper.TXT", ".GBL");
            rules.Add("CADCAM Top Solder Resist.TXT", ".GTS");
            rules.Add("CADCAM Bottom Solder Resist.TXT", ".GBS");
            rules.Add("CADCAM Top Silk Screen.TXT", ".GTO");
            rules.Add("CADCAM Bottom Silk Screen.TXT", ".GBO");
            rules.Add("CADCAM Mech 1.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mech 2.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mech 3.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mech 4.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Drill.TXT", ".XLN");
            //To do: add rules for Inner 1 and Inner 2   (G2L, G3L)

            string directory = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            Console.WriteLine("Source directory: " + directory);
            string[] files = Directory.GetFiles(directory);
            int count = 0;
            foreach (string file in files)
                if (rules.Keys.Any(r => file.Contains(r)))
                    count++;

            Console.WriteLine("Found " + count.ToString() + " applicable files in source directory.");

            if (count > 0)
            {
                int i = 0;
                using (ZipArchive zip = ZipFile.Open(Path.Combine(directory, "OSH Park.zip"), ZipArchiveMode.Create))
                {
                    foreach (string file in files)
                    {
                        if (rules.Keys.Any(r => file.Contains(r)))
                        {
                            string key = rules.Keys.First(r => file.Contains(r));
                            string extension = rules[key];
                            string newFile = Path.ChangeExtension(file, extension);
                            
                            //File.Move(file, newFile);
                            zip.CreateEntryFromFile(file, Path.GetFileName(newFile));
                            Console.WriteLine("Added file to zip: " + Path.GetFileName(newFile));
                            i++;
                        }
                    }
                }
                
                    
                

            }
            Console.WriteLine("Press any key to exit.");
            Console.Read();
            
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine("Exception caught : " + ex.Message);
            //Console.WriteLine("Runtime terminating: {0}", e.IsTerminating);
            Console.WriteLine("Press any key to exit.");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
