using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace PDC.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] Loader _loader;
        private static string _fontName = "";
        public static int languageID = 0;
        public static List<string> _languages = new List<string>();
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

        public static void OnLocaReady(Action action)
        {
            if(IsLocaReady)
            {
                action();
            }
            else
            {
                OnLocalizationReady += action;
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
            { "[/f]", "</font>" },
            { "[v]", "," },
            { "[V]", "," },
            { "[/$1]", "<font=rpdpcfont>" },
            { "[/$2]", "<font=rpdpcfont>" },
            { "[/$3]", "<font=rpdpcfont>" },
            { "[/$4]", "<font=rpdpcfont>" }
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

                if(c.Contains("$") && !c.Contains('/'))
                {
                    if(int.TryParse(c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("$", string.Empty), out int id))
                    {
                        if(GameManager.Instance.MemoryManager.EncounteredMemory.Any(x => x._isTaken && x.Act == (Act)(id - 1)))
                        {
                            t = t.Replace(c, "</font>");
                            continue;
                        }
                    }
                    else
                    {
                        Debug.LogError("$ key is not valid for : " + c);
                    }

                }

                if (c.Contains('#') && !c.Contains('/'))
                {
                    var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("#", string.Empty);
                    t = t.Replace(c, $"<color=#{value}>");
                    continue;
                }

                if (c.Contains('f') && !c.Contains('/'))
                {
                    //var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("f", string.Empty);
                    t = t.Replace(c, $"<font=rpdpcfont>");
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
            if(!text.Contains("rpdpcfont")) return text;

            var words = GetWords(text);
            bool isTranslated = false;
            for(int i = 0; i < words.Length; i++)
            {
                bool CheckIfKey()
                {
                    if(i > 0 && i < words.Length - 1)
                    {
                        if(words[i-1] == "<" && words[i+1] == ">")
                        {
                            return true;
                        }
                    }
                    return false;
                }
                var word = words[i];
                //Debug.Log(word);
                
                if(CheckIfKey() && (word == "/font" || word == "font=rpdpcfont"))
                {
                    isTranslated = word == "/font" ? false : word == "font=rpdpcfont";
                    continue;
                }

                //Debug.Log(word + " : " + (word.Contains('<') || word.Contains('>')) + " | " + word.Length);
                if(word.Contains('<') || word.Contains('>')/* || word.Contains("/color") || word.Contains("color=") */|| CheckIfKey() || word.Length == 0) continue;


                //Debug.Log("Word : " + word + " | " + GameManager.Instance.LanguageManager.UnlockedWords.Any(x => x.Word.ToLower() == word.ToLower()) + " | " + isTranslated);
                if(GameManager.Instance != null && !GameManager.Instance.LanguageManager.UnlockedWords.Any(x => x.Word.ToLower() == word.ToLower()) && !word.Equals(string.Empty) && isTranslated)
                {
                    text = text.Replace(word, GetTranslatedWord(word), StringComparison.OrdinalIgnoreCase);
                }
                else if(!word.Equals(string.Empty))
                {
                    //Debug.Log(word.ToLower());
                    if(text.Contains($" {word}"))
                    {
                        text = text.Replace($" {word}", $" {word}", StringComparison.OrdinalIgnoreCase);
                    }
                    else if(text.Contains($"{word} "))
                    {
                        text = text.Replace($"{word} ", $"{word} ", StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        text = text.Replace($"{word}", $"{word}", StringComparison.OrdinalIgnoreCase);
                    }

                    if(GameManager.Instance != null && GameManager.Instance.LanguageManager.UnlockedWords.Any(x => x.Word.ToLower() == word.Replace(" ", string.Empty).ToLower()) && isTranslated)
                    {
                        text = text.Replace(word, $"</font>{word} <font=rpdpcfont>");
                    }
                }

                //Debug.Log(text);
            }

            for(int i = 0; i < words.Length; i++)
            {
                if(i > 0 && i < words.Length - 2)
                {
                    if(words[i - 1].Contains('<') && words[i + 1].Contains('>'))
                    {
                        text = text.Replace($"{words[i]}", $"{words[i]}", StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return text;
        }

        public static string LocalizeText(string text, bool isKey = false)
        {
            var t = isKey ? GetLocalizedText(text) : text;
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
            //Debug.Log(t);
            t = SimplifyText(t);
            //Debug.Log(t);
            t = RemoveDiacritics(t);
            //Debug.Log(t);
            t = TranslateWord(t);
            //Debug.Log(t);
            return t;
        }

        private static string GetTranslatedWord(string text)
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
                trad += list[i];
            }

            return trad;
        }

        static string[] GetWords(string input)
        {
            var words = Regex.Split(input, @"([ ])");
            List<string> list = new List<string>();
            foreach (var word in words)
            {
                //Debug.Log(word);
                if(word.Contains('<'))
                {
                    var wordleft = Regex.Split(word, @"([<])");
                    foreach(var w in wordleft)
                    {
                        //Debug.Log(w);
                        list.Add(w);
                    }
                }
                else
                {
                    list.Add(word);
                }
            }

            List<string> secondList = new List<string>();
            foreach(var word in list)
            {
                //Debug.Log(word);
                if(word.Contains('>'))
                {
                    var wordleft = Regex.Split(word, @"([>])");
                    foreach(var w in wordleft)
                    {
                        //Debug.Log(w);
                        secondList.Add(w);
                    }
                }
                else
                {
                    secondList.Add(word);
                }
            }
            return secondList.ToArray();
        }

        public static string RemoveDiacritics(string text)
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

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}