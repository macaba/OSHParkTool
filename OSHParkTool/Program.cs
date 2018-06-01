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
            rules.Add("CADCAM Inner 1.TXT", ".G2L");
            rules.Add("CADCAM Inner 2.TXT", ".G3L");
            rules.Add("CADCAM Inner  1.TXT", ".G2L");
            rules.Add("CADCAM Inner  2.TXT", ".G3L");
            rules.Add("CADCAM Top Solder Resist.TXT", ".GTS");
            rules.Add("CADCAM Bottom Solder Resist.TXT", ".GBS");
            rules.Add("CADCAM Top Silk Screen.TXT", ".GTO");
            rules.Add("CADCAM Bottom Silk Screen.TXT", ".GBO");
            rules.Add("CADCAM Mechanical 1.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 2.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 3.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 4.TXT", ".GKO");     //Board outline
            rules.Add("CADCAM Drill.TXT", ".XLN");

            //New proteus
            rules.Add("CADCAM Top Copper.GBR", ".GTL");
            rules.Add("CADCAM Bottom Copper.GBR", ".GBL");
            rules.Add("CADCAM Inner 1.GBR", ".G2L");
            rules.Add("CADCAM Inner 2.GBR", ".G3L");
            rules.Add("CADCAM Inner  1.GBR", ".G2L");
            rules.Add("CADCAM Inner  2.GBR", ".G3L");
            rules.Add("CADCAM Top Solder Resist.GBR", ".GTS");
            rules.Add("CADCAM Bottom Solder Resist.GBR", ".GBS");
            rules.Add("CADCAM Top Silk Screen.GBR", ".GTO");
            rules.Add("CADCAM Bottom Silk Screen.GBR", ".GBO");
            rules.Add("CADCAM Mechanical 1.GBR", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 2.GBR", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 3.GBR", ".GKO");     //Board outline
            rules.Add("CADCAM Mechanical 4.GBR", ".GKO");     //Board outline
            rules.Add("CADCAM Drill.DRL", ".XLN");

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
