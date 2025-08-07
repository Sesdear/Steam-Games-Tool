using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SteamGamesTool
{
    internal class Main
    {
        public void UnZip(string fileName, string extract_dir)
        {
            if (Directory.Exists(extract_dir) == false) { Directory.CreateDirectory(extract_dir); }

            try
            {
                ZipFile.ExtractToDirectory(fileName, extract_dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "❌ Error");
            }
        }
        public void DownloadManifest(string appid, CheckBox checkBox)
        {
            SteamAutoAdd steamAutoAdd = new SteamAutoAdd();
            if (!CheckForExist(appid)) { return; }
            string url = $"";
            if (checkBox.Checked)
            {
                string selectedPath = "./";
                ManifestResponse(url, selectedPath, appid);
                UnZip($"./{appid}.zip", $"./{appid}");
                string game_path = $"./{appid}";

                steamAutoAdd.StartAdd(game_path);
                MessageBox.Show("✔ Ready", "Game Add");
            }
            else
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "Choose folder to save";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    ManifestResponse(url, selectedPath, appid);
                }
            }
            
            
        }
        public void ManifestResponse(string url, string path, string appid)
        {
            if (string.IsNullOrEmpty(url)) { return; }
            string path_filename = $"{path}/{appid}.zip";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, path_filename);
                    MessageBox.Show("Manifest saved in path: " + path_filename, "✔ Ready");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "❌ Error");
            }
        }
        public bool CheckForExist(string appid)
        {
            string url = $"";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true ;
                    }
                    else
                    {
                        MessageBox.Show("AppId doesn't exist", "❌ Error");
                        return false ;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    MessageBox.Show($"{(int)errorResponse.StatusCode} {errorResponse.StatusDescription}", "❌ Error");
                    return false ;
                }
                else
                {
                    MessageBox.Show(ex.Message, "❌ Error on connect");
                    return false ;
                }
            }

        }
    }
}
