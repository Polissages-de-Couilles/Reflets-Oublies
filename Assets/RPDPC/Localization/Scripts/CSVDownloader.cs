using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// CSVDownloader.cs
using UnityEngine.Networking;

namespace PDC.Localization
{
    public static class CSVDownloader
    {
        private const string k_googleSheetDocID = "1xAgtKrBaAj8lnMWsigVwZPA3PpUcATjA_d02YkoxyMo";

        // docs.google.com/spreadsheets/d/13zXZxMWmS5ShIIxXd8OIOIf6JCBYmwziav9OsLdPH1U/edit#gid=0
        private const string url = "https://docs.google.com/spreadsheets/d/" + k_googleSheetDocID + "/export?format=csv";

        internal static IEnumerator DownloadData(System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();

            string downloadData = null;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                //Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();
                //int equalsIndex = ExtractEqualsIndex(webRequest.downloadHandler);
                if (webRequest.isNetworkError)
                {
                    Debug.Log("...Download Error: " + webRequest.error);
                    downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                    //string versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
                    Debug.Log("Using stale data");
                }
                else
                {
                    //string versionText = webRequest.downloadHandler.text;
                    downloadData = webRequest.downloadHandler.text;
                    //PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
                    PlayerPrefs.SetString("LastDataDownloaded", downloadData);
                    //Debug.Log("...Downloaded");

                }
            }

            onCompleted(downloadData);
        }

        private const string k2_googleSheetDocID = "1Ya7cava7Ka5ln1qNVw4igsawQ4dcKFKSoTBRUOVphyU";

        private const string url2 = "https://docs.google.com/spreadsheets/d/" + k2_googleSheetDocID + "/export?format=csv";

        internal static IEnumerator DownloadGoogleDBData(System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();

            string downloadData = null;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url2))
            {
                //Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();
                //int equalsIndex = ExtractEqualsIndex(webRequest.downloadHandler);
                if (webRequest.isNetworkError)
                {
                    Debug.Log("...Download Error: " + webRequest.error);
                    downloadData = PlayerPrefs.GetString("LastGoogleDataDownloaded", null);
                    //string versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
                    Debug.Log("Using stale data");
                }
                else
                {
                    //string versionText = webRequest.downloadHandler.text;
                    downloadData = webRequest.downloadHandler.text;
                    //PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
                    PlayerPrefs.SetString("LastGoogleDataDownloaded", downloadData);
                    //Debug.Log("...Downloaded");

                }
            }

            onCompleted(downloadData);
        }

        //private static int ExtractEqualsIndex(DownloadHandler d)
        //{
        //    if (d.text == null || d.text.Length < 10)
        //    {
        //        return -1;
        //    }
        //    // First term will be preceeded by version number, e.g. "100=English"
        //    string versionSection = d.text.Substring(0, 5);
        //    int equalsIndex = versionSection.IndexOf('=');
        //    if (equalsIndex == -1)
        //        Debug.Log("Could not find a '=' at the start of the CVS");
        //    return equalsIndex;
        //}
    }
}