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
            string targetDir = Path.Combine(Settings1.Default.SteamPath, "config", "depotcache");
            Directory.CreateDirectory(targetDir); // Создаёт, если не существует

            string destinationPath = Path.Combine(targetDir, Path.GetFileName(manifest));
            Console.WriteLine(Path.GetFileName(manifest));
            File.Move(manifest, destinationPath);
        }

        public static void MoveLuas(string lua, string dir_name)
        {
            string targetDir = Path.Combine(Settings1.Default.SteamPath, "config", "stplug-in");
            Directory.CreateDirectory(targetDir); // Создаёт, если не существует

            string destinationPath = Path.Combine(targetDir, Path.GetFileName(lua));
            Console.WriteLine(Path.GetFileName(lua));
            File.Move(lua, destinationPath);
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

                
                string steamPath = $@"{Settings1.Default.SteamPath}\steam.exe";

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
            string fullPath = Path.Combine("./", path_name);
            string[] files = Directory.GetFiles(fullPath);
            bool found = ProcessFiles(files);

            if (!found)
            {
                string[] subDirs = Directory.GetDirectories(fullPath);
                if (subDirs.Length == 1)
                {
                    string subDir = subDirs[0];
                    string[] subFiles = Directory.GetFiles(subDir);
                    ProcessFiles(subFiles);
                    RestartSteam();
                }
            }
        }

        private bool ProcessFiles(string[] files)
        {
            bool anyFound = false;

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                if (extension == ".lua")
                {
                    Console.WriteLine($"{file}");
                    MoveLuas(file, "");
                    anyFound = true;
                }
                else if (extension == ".manifest")
                {
                    Console.WriteLine($"{file}");
                    MoveManifests(file, "");
                    anyFound = true;
                }
            }

            return anyFound;
        }

    }
}
