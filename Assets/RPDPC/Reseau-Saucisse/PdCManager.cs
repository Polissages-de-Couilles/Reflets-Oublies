using EditorCools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PDC
{
    public class PdCManager : MonoBehaviour
    {
        public GoogleSheetDB GoogleSheetManager => _googleSheetManager;
        [SerializeField] GoogleSheetDB _googleSheetManager;

        private const string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScY3BI2SMaX3mcg_g0ypchfXCK2g9TlC-BS2_UkLsXR3YD3KQ/formResponse";

        public const string PdC1 = "entry.264085814";
        public const string PdC2 = "entry.1750431439";
        public const string PdC3 = "entry.504257996";
        public const string PdC4 = "entry.921727994";
        public const string PdC5 = "entry.90729322";
        public const string PdC6 = "entry.1514620632";
        public const string PdC7 = "entry.1535528696";
        public const string PdC8 = "entry.188076372";
        public const string PdC9 = "entry.1113729072";

        public void SendMessage(string message, string PdC)
        {
            StartCoroutine(PostMessage($"{message}", PdC));
        }

        private IEnumerator PostMessage(string message, string PdC)
        {
            WWWForm form = new WWWForm();
            form.AddField(PdC, message);

            using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Send Sucess");
                }
                else
                {
                    Debug.Log("Send Error : " + www.error);
                }
            }
        }

        [SerializeField] string Message;
        [SerializeField] string PdC;
        [SerializeField] int Language;

        [Button]
        public void TestMessage()
        {
            SendMessage(Message, PdC);
        }
    }
}