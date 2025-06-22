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

        public enum PdCType
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
        public PdCType CurrentPdC {  get; private set; }

        public void SendMessage()
        {
            string message = "";
            string id = "";

            switch(CurrentPdC)
            {
                case PdCType.PdC1:
                    id = PdC1;
                    break;
                case PdCType.PdC2:
                    id = PdC2;
                    break;
                case PdCType.PdC3:
                    id = PdC3;
                    break;
                case PdCType.PdC4:
                    id = PdC4;
                    break;
                case PdCType.PdC5:
                    id = PdC5;
                    break;
                case PdCType.PdC6:
                    id = PdC6;
                    break;
                case PdCType.PdC7:
                    id = PdC7;
                    break;
                case PdCType.PdC8:
                    id = PdC8;
                    break;
                case PdCType.PdC9:
                    id = PdC9;
                    break;
                default:
                    break;
            }

            for(int i = 0; i < _currentWords.Count; i++)
            {
                message += $"[{_currentWords[i].Key}] ";
            }

            Debug.Log("PDC Message : " +  message + " | Id : " + id);
            StartCoroutine(PostMessage($"{message}", id));
            _currentWords.Clear();
            _currentMessageText.text = "";
        }

        private void SendMessage(string message, string PdC)
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
                    PdCType type = PdCType.PdC1;

                    switch(PdC)
                    {
                        case PdC1:
                            type = PdCType.PdC1;
                            break;

                        case PdC2:
                            type = PdCType.PdC2;
                            break;

                        case PdC3:
                            type = PdCType.PdC3;
                            break;

                        case PdC4:
                            type = PdCType.PdC4;
                            break;

                        case PdC5:
                            type = PdCType.PdC5;
                            break;

                        case PdC6:
                            type = PdCType.PdC6;
                            break;

                        case PdC7:
                            type = PdCType.PdC7;
                            break;

                        case PdC8:
                            type = PdCType.PdC8;
                            break;

                        case PdC9:
                            type = PdCType.PdC9;
                            break;

                        default:
                            break;
                    }

                    yield return DisplayMessage(type);

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

        [SerializeField] GameObject _pdcHolder;

        public IEnumerator Start()
        {
            yield return null;
            GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Cancel.performed += RemoveLastWord;
            //yield return Setup(PdC.PdC1);
        }

        public void AddWord(TranslatedWord word)
        {
            if(_currentWords.Count >= 5) return;
            _currentWords.Add(word);
            _currentMessageText.text += $"{word.Word} ";
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

        public IEnumerator DisplayMessage(PdCType Pdc)
        {
            for(int i = 0; i < _messagesObjects.Count; i++)
            {
                Destroy(_messagesObjects[i].gameObject);
            }
            _messagesObjects.Clear();

            yield return null;
            var t = _googleSheetManager.GetMessages((int)Pdc);
            yield return t;

            var messages = t.Current;

            for(int i = 0; i < messages.Count; i++)
            {
                if(messages[i] == null || messages[i] == string.Empty) continue;
                var m = Instantiate(_messagePrefab, _messagesHolder);

                string text = messages[i];
                text = Localization.LocalizationManager.LocalizeText("<font=rpdpcfont>" + text);
                Debug.Log(text);

                m.SetText(text, -1);
                _messagesObjects.Add(m);
                yield return null;
            }
        }

        public IEnumerator Setup(PdCType Pdc)
        {
            StateMachineManager.StopAllStateMachines();
            _pdcHolder.SetActive(true);
            for(int i = 0; i < _wordsButton.Count; i++)
            {
                Destroy(_wordsButton[i].gameObject);
            }
            _wordsButton.Clear();

            for(int i = 0; i < _messagesObjects.Count; i++)
            {
                Destroy(_messagesObjects[i].gameObject);
            }
            _messagesObjects.Clear();

            CurrentPdC = Pdc;
            _currentWords.Clear();
            _currentMessageText.text = "";

            yield return DisplayMessage(Pdc);

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

        public void ExitPdC()
        {
            StateMachineManager.RestartAllStateMachines();
            _pdcHolder.SetActive(false);
        }
    }
}