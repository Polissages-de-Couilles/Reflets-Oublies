using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDC.Localization
{
    public class GameLocalizationManager : MonoBehaviour
    {
        [SerializeField] Loader _loader;
        public static int languageID = 1;
        private static List<string> _languages = new List<string>();
        private static Dictionary<string, string[]> _localization = new Dictionary<string, string[]>();

        public void Start()
        {
            _loader.Load();
        }

        public static void SetLocalization(Dictionary<string, string[]> loca)
        {
            _localization = loca;
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
                return SimplifyText(text[languageID]);
            }
        }

        private static string SimplifyText(string text)
        {
            var t = text;
            t = t.Replace("[n]", "\n");
            return t;
        }

        public static string LocalizeText(string text)
        {
            var t = text;
            foreach (var key in _localization)
            {
                if (t.Contains($"[{key.Key}]"))
                {
                    t = t.Replace($"[{key.Key}]", key.Value[languageID]);
                }
            }
            t.Replace($"\r\n", string.Empty);
            return SimplifyText(t);
        }
    }
}