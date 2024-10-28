using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using EditorCools;
using System.Linq;
using System.IO;
using System;

namespace PDC
{
    public class GoogleSheetDB : MonoBehaviour
    {
        [SerializeField] TextAsset csv;
        List<List<string>> Messages = new List<List<string>>();
        Action OnDataUpdated;

        [Button]
        public void Load()
        {
            StartCoroutine(CSVDownloader.DownloadGoogleDBData(AfterDownload));
        }

        public void AfterDownload(string data)
        {
            if (null == data)
            {
                Debug.LogError("Was not able to download data or retrieve stale data.");
                // TODO: Display a notification that this is likely due to poor internet connectivity
                //       Maybe ask them about if they want to report a bug over this, though if there's no internet I guess they can't
                StartCoroutine(ProcessData(csv.text, AfterProcessData));
            }
            else
            {
                string filePath = Application.dataPath + @"/Resources/PdC DB.csv";
                File.WriteAllText(filePath, data);
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

            List<List<string>> messages = new List<List<string>>();

            foreach (string collum in splitDataset[0].Split(new char[] { ',' }).ToList())
            {
                messages.Add(new List<string>());
            }

            // Iterating through the split dataset in order to spli into rows
            for (var i = 1; i < splitDataset.Length; i++)
            {
                string[] row = splitDataset[i].Split(new char[] { ',' });
                for (int j = 1; j < row.Length; j++)
                {
                    Debug.Log(row.Length + " | " + j + " | " + messages.Count);
                    Debug.Log(row[j]);
                    if (row[j] == null)
                    {
                        messages[j-1].Add(string.Empty);
                    }
                    else
                    {
                        messages[j-1].Add(row[j]);
                    }
                }
            }

            foreach (var list in messages)
            {
                string t = "| ";
                foreach (var text in list)
                {
                    t += text;
                    t += " | ";
                }
                Debug.Log(t);
            }

            Messages = messages;
            OnDataUpdated?.Invoke();
        }

        public IEnumerator<List<string>> GetMessages(int pdc, int language)
        {
            bool isDataUpdated = false;
            OnDataUpdated += () => isDataUpdated = true;

            Load();
            while(!isDataUpdated)
                yield return null;

            var list = Messages[pdc];
            List<string> finalList = new List<string>();
            foreach (var t in list)
            {
                if (int.TryParse(t[0].ToString(), out var value))
                {
                    if(value == language)
                    {
                        finalList.Add(t.Substring(1));
                        Debug.Log(t.Substring(1));
                    }
                }
                else
                {

                }
            }

            yield return finalList;
        }
    }
}