using EditorCools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace PDC
{
    public static class MyExtensions
    {
        public static string TrimLastCharacter(this String str)
        {
            if(String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return str.TrimEnd(str[str.Length - 1]);
            }
        }
    }

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

        public enum PdC
        {
            PdC1,
            PdC2, 
            PdC3,
            PdC4, 
            PdC5,
            PdC6,
            PdC7, 
            PdC8,
            PdC9,
        }
        public PdC CurrentPdC {  get; private set; }

        public void SendMessage()
        {
            string message = "";
            string id = "";

            switch(CurrentPdC)
            {
                case PdC.PdC1:
                    id = PdC1;
                    break;
                case PdC.PdC2:
                    id = PdC2;
                    break;
                case PdC.PdC3:
                    id = PdC3;
                    break;
                case PdC.PdC4:
                    id = PdC4;
                    break;
                case PdC.PdC5:
                    id = PdC5;
                    break;
                case PdC.PdC6:
                    id = PdC6;
                    break;
                case PdC.PdC7:
                    id = PdC7;
                    break;
                case PdC.PdC8:
                    id = PdC8;
                    break;
                case PdC.PdC9:
                    id = PdC9;
                    break;
                default:
                    break;
            }

            for(int i = 0; i < _currentWords.Count; i++)
            {
                message += _currentWords[i].Key + " ";
            }

            StartCoroutine(PostMessage($"{message}", id));
        }

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

        [Space(20), SerializeField] List<HorizontalLayoutGroup> _wordsHolders;
        [SerializeField] VerticalLayoutGroup _wordsHoldersHolder;
        [SerializeField] PdCWord _wordPrefab;

        private List<PdCWord> _wordsButton = new();

        [SerializeField] TextMeshProUGUI _currentMessageText;
        List<TranslatedWord> _currentWords = new();

        [SerializeField] Transform _messagesHolder;
        [SerializeField] PdCWord _messagePrefab;

        private List<PdCWord> _messagesObjects = new();

        public IEnumerator Start()
        {
            yield return null;
            GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Cancel.performed += RemoveLastWord;
            //yield return Setup(PdC.PdC1);
        }

        public void AddWord(TranslatedWord word)
        {
            _currentWords.Add(word);
            _currentMessageText.text += word.Word + " ";
        }

        public void RemoveLastWord(InputAction.CallbackContext context)
        {
            if(_currentWords.Count <= 0) return;
            var word = _currentWords.Last();
            string newText = _currentMessageText.text;
            for(int i = 0; i < word.Word.Length + 1; i++)
            {
                newText.TrimLastCharacter();
            }
            _currentMessageText.text = newText;
            _currentWords.RemoveAt(_currentWords.Count - 1);
        }

        public IEnumerator Setup(PdC Pdc)
        {
            CurrentPdC = Pdc;
            _currentMessageText.text = "";

            var t = _googleSheetManager.GetMessages((int)Pdc);
            yield return t;

            var messages = t.Current;

            for(int i = 0; i < messages.Count; i++)
            {
                Debug.Log("Message " +  (messages[i] == null));
                if(messages[i] == null || messages[i] == string.Empty) continue;
                Debug.Log("Message " + i + " : " + messages[i] + " | Prefab : " + (_messagePrefab == null));
                var m = Instantiate(_messagePrefab, _messagesHolder);
                m.SetText(messages[i], -1);
                _messagesObjects.Add(m);
                yield return null;
            }

            yield return null;
            var listWords = GameManager.Instance.LanguageManager.UnlockedWords;
            HorizontalLayoutGroup currentRows = _wordsHolders.First();
            int rows = 0;

            for(int i = 0; i < listWords.Count; i++)
            {
                var word = Instantiate(_wordPrefab, currentRows.transform);
                word.SetText(listWords[i], i);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsHoldersHolder.transform as RectTransform);
                yield return null;
                //Debug.Log((currentRows.transform as RectTransform).sizeDelta.x + " | " + (_wordsHoldersHolder.transform as RectTransform).sizeDelta.x);
                if((currentRows.transform as RectTransform).sizeDelta.x > (_wordsHoldersHolder.transform as RectTransform).sizeDelta.x)
                {
                    Destroy(word.gameObject);
                    i--;
                    rows++;
                    currentRows = _wordsHolders[rows];
                    continue;
                }
                _wordsButton.Add(word);
            }

            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsHoldersHolder.transform as RectTransform);
        }
    }
}