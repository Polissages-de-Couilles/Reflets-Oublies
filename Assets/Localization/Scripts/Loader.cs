using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace PDC.Localization
{
    public class Loader : MonoBehaviour
    {
        public void Load()
        {
            StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
        }

        public void AfterDownload(string data)
        {
            if (null == data)
            {
                Debug.LogError("Was not able to download data or retrieve stale data.");
                // TODO: Display a notification that this is likely due to poor internet connectivity
                //       Maybe ask them about if they want to report a bug over this, though if there's no internet I guess they can't
            }
            else
            {
                StartCoroutine(ProcessData(data, AfterProcessData));
            }
        }

        private void AfterProcessData(string errorMessage)
        {
            if (null != errorMessage)
            {
                Debug.LogError("Was not able to process data: " + errorMessage);
                // TODO: 
            }
            else
            {

            }
        }

        public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Debug.Log(data);

            // Splitting the dataset in the end of line
            var splitDataset = data.Split("\r\n");

            var languages = splitDataset[0].Split(new char[] { ',' }).ToList();
            languages.Remove("Key");
            LocalizationManager.SetLanguage(languages);

            Dictionary<string, string[]> localization = new Dictionary<string, string[]>();

            // Iterating through the split dataset in order to spli into rows
            for (var i = 1; i < splitDataset.Length; i++)
            {
                string[] row = splitDataset[i].Split(new char[] { ',' });
                var key = row[0];
                var list = row.ToList();
                list.Remove(key);
                localization.Add(row[0], list.ToArray());
            }
            LocalizationManager.SetLocalization(localization);
        }
    }
}