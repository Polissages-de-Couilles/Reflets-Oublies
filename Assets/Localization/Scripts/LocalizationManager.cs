using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PDC.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] Loader _loader;
        public static int languageID = 2;
        private static List<string> _languages = new List<string>();
        private static Dictionary<string, string[]> _localization = new Dictionary<string, string[]>();
        public static Action OnLocalizationReady;
        public static bool IsLocaReady = false;

        public void Awake()
        {
            _loader.Load();
        }

        public void ChangeLanguage(int language)
        {
            if (language >= _languages.Count || !IsLocaReady)
                return;
            languageID = language;
            ILocalization[] loca = FindObjectsOfType<MonoBehaviour>(true).OfType<ILocalization>().ToArray();
            foreach (var l in loca)
            {
                l.OnLanguageChange?.Invoke();
            }
        }

        public static void SetLocalization(Dictionary<string, string[]> loca)
        {
            _localization = loca;
            IsLocaReady = true;
            OnLocalizationReady?.Invoke();
        }

        public static void SetLanguage(List<string> languages)
        {
            _languages = languages;
        }

        public static string GetLocalizedText(string key)
        {
            _localization.TryGetValue(key, out var text);
            {
                //Debug.Log($"Key : {key} | Text found : {text != null}");
                if (text == null) return string.Empty;
                return text[languageID];
            }
        }

        private readonly static Dictionary<string, string> _simplifyDico = new()
        {
            { "[n]", "\n" },
            { "[/#]", "</color>" },
            { "[/%]", "</size>" },
            { "[b]", "<b>" },
            { "[/b]", "</b>" },
            { "[i]", "<i>" },
            { "[/i]", "</i>" },
            { "[s]", "<s>" },
            { "[/s]", "</s>" },
            { "[u]", "<u>" },
            { "[/u]", "</u>" },
            { "[v]", "," },
        };

        private static string SimplifyText(string text)
        {
            var t = text;

            while(t.Contains('[') && t.Contains("]"))
            {
                int startIndex = t.IndexOf('[');
                int endIndex = t.IndexOf(']');
                int length = endIndex - startIndex + 1;

                var c = t.Substring(startIndex, length);

                if (_simplifyDico.ContainsKey(c))
                {
                    t = t.Replace(c, _simplifyDico[c]);
                    continue;
                }

                //if (c.Contains('&'))
                //{
                //    var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("&", string.Empty);
                //    int id = 0;
                //    for (int i = 0; i < value.Length; i++)
                //    {
                //        var letter = value[i];
                //        int index = char.ToUpper(letter) - 64;
                //        id += index;
                //    }

                //    Debug.Log(id);
                //    List<char> list = value.ToList();
                //    Shuffle(list, id);
                //    string trad = string.Empty;

                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        trad += $"<sprite={char.ToUpper(list[i]) - 65}>";
                //    }

                //    t = t.Replace(c, trad);
                //    continue;
                //}

                if (c.Contains('#') && !c.Contains('/'))
                {
                    var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("#", string.Empty);
                    t = t.Replace(c, $"<color=#{value}>");
                    continue;
                }

                if (c.Contains('%') && !c.Contains('/'))
                {
                    var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("%", string.Empty);
                    t = t.Replace(c, $"<size={value}%>");
                    continue;
                }

                t = t.Replace(c, "");
            }
            
            //Debug.Log(t);
            return t;
        }
        private static string TranslateWord(string text)
        {
            //Debug.Log(text);
            text = text.ToLower();
            var words = GetWords(text);
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var w in words)
            {
                //Debug.Log(w);
                if (dic.ContainsKey(w.ToLower())) continue;
                if (!GameManager.Instance.LanguageManager.UnlockedWords.Any(x => x.Word.ToLower() == w.ToLower()))
                {
                    dic.Add(w.ToLower(), GetTranslatedWord(w.ToLower()));
                }
                else
                {
                    dic.Add(w.ToLower(), w.ToUpper());
                }
            }

            foreach (var kv in dic)
            {
                if(text.Contains($" {kv.Key} "))
                    text = text.Replace($" {kv.Key} ", $" {kv.Value} ");
                else if(text.Contains($" {kv.Key}"))
                    text = text.Replace($" {kv.Key}", $" {kv.Value}");
                else if(text.Contains($"{kv.Key} "))
                    text = text.Replace($"{kv.Key} ", $"{kv.Value} ");
                else if(text.Contains($"{kv.Key}"))
                    text = text.Replace($"{kv.Key}", $"{kv.Value}");
            }

            //Debug.Log(text);
            return text;
        }

        public static string LocalizeText(string text)
        {
            var t = text;
            bool AsKey()
            {
                foreach (var key in _localization)
                {
                    if (t.Contains($"[{key.Key}]"))
                    {
                        return true;
                    }
                }
                return false;
            }

            string ReplaceAllKey(string text)
            {
                var temp = t;
                foreach (var key in _localization)
                {
                    if (temp.Contains($"[{key.Key}]"))
                    {
                        temp = temp.Replace($"[{key.Key}]", key.Value[languageID]);
                    }
                }
                return temp;
            }

            while (AsKey())
            {
                t = ReplaceAllKey(t);
            }
            
            t.Replace($"\r\n", string.Empty);
            Debug.Log(t);
            t = SimplifyText(t);
            return TranslateWord(t);
        }

        public static string GetTranslatedWord(string text)
        {
            text = RemoveDiacritics(text);
            int id = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var letter = text[i];
                int index = char.ToUpper(letter) - 64;
                id += index;
            }

            //Debug.Log(id);
            List<char> list = text.ToList();
            Tools.Shuffle(list, id);

            string trad = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                if (char.IsLetter(list[i]))
                {
                    trad += $"<sprite={char.ToUpper(list[i]) - 65}>";
                    //Debug.Log($"{list[i]} = {char.ToUpper(list[i]) - 65} | Trad = {trad}");
                }
                else
                {
                    trad += list[i];
                }
            }

            return trad;
        }

        static string[] GetWords(string input)
        {
            var words = input.Split(" ");

            for (int i = 0; i < words.Length; i++)
            {
                foreach (var kv in _simplifyDico)
                {
                    if (words[i].Contains(kv.Value))
                    {
                        words[i] = words[i].Replace(kv.Value, string.Empty);
                    }
                }

                while (words[i].Contains('<') || words[i].Contains('>'))
                {
                    int startIndex = words[i].IndexOf('<');
                    int endIndex = words[i].IndexOf('>');
                    int length = endIndex - startIndex + 1;

                    var c = words[i].Substring(startIndex, length);
                    words[i] = words[i].Replace(c, string.Empty);
                }
            }

            List<string> list = new();
            foreach (var w in words)
            {
                if(w != string.Empty)
                    list.Add(w);
            }

            return list.ToArray();
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}