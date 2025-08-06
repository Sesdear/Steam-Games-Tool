using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamGamesTool
{
    public class SteamAutoAdd
    {
        public static void MoveManifests(string manifest, string dir_name)
        {
            Console.WriteLine(Path.GetFileName(manifest));
            File.Move(manifest, $"C:\\Program Files (x86)\\Steam\\config\\depotcache\\{Path.GetFileName(manifest)}");
        }
        public static void MoveLuas(string lua, string dir_name)
        {
            Console.WriteLine(Path.GetFileName(lua));
            File.Move(lua, $"C:\\Program Files (x86)\\Steam\\config\\stplug-in\\{Path.GetFileName(lua)}");

        }
        public static void RestartSteam()
        {
            try
            {
                
                var steamProcesses = Process.GetProcessesByName("steam");

                if (steamProcesses.Length == 0)
                {
                    Console.WriteLine("Steam не запущен.");
                }
                else
                {
                    Console.WriteLine("Завершение Steam...");

                    foreach (var proc in steamProcesses)
                    {
                        proc.Kill();
                    }

                    
                    foreach (var proc in steamProcesses)
                    {
                        proc.WaitForExit();
                    }

                    Console.WriteLine("Steam завершён.");
                }

                
                string steamPath = @"C:\Program Files (x86)\Steam\steam.exe";

                if (!File.Exists(steamPath))
                {
                    Console.WriteLine("Не найден файл steam.exe по пути: " + steamPath);
                    return;
                }

                Console.WriteLine("Запуск Steam...");
                Process.Start(steamPath);
                Console.WriteLine("Steam перезапущен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        public void StartAdd(string path_name)
        {
            
            string[] files = Directory.GetFiles($"./{path_name}");
            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                if (extension == ".lua")
                {
                    Console.WriteLine($"{file}");
                    MoveLuas(file, path_name);
                }
                else if (extension == ".manifest")
                {
                    Console.WriteLine($"{file}");
                    MoveManifests(file, path_name);
                }
            }
            RestartSteam();
        }
    }
}
