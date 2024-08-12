using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDC.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] Loader _loader;
        public static int languageID = 0;
        private static List<string> _languages = new List<string>();
        private static Dictionary<string, string[]> _localization = new Dictionary<string, string[]>();
        public static Action OnLocalizationReady;

        public void Awake()
        {
            _loader.Load();
        }

        public static void SetLocalization(Dictionary<string, string[]> loca)
        {
            _localization = loca;
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
            { "[b]", "<b>" },
            { "[/b]", "</b>" },
            { "[i]", "<i>" },
            { "[/i]", "</i>" },
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

                if(c.Length == 9 && c.Contains('#'))
                {
                    var value = c.Replace("[", string.Empty).Replace("]", string.Empty).Replace("#", string.Empty);
                    t = t.Replace(c, $"<color=#{value}>");
                    continue;
                }

                t = t.Replace(c, "");
            }
            
            //Debug.Log(t);
            return t;
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
            return SimplifyText(t);
        }
    }
}