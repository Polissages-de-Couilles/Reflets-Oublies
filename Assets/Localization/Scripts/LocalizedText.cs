using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PDC.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour, ILocalization
    {
        TextMeshProUGUI m_TextMeshProUGUI;
        [SerializeField] string _key;
        public string Key => _key;
        public Action OnLanguageChange { get; set; }

        public void Start()
        {
            m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            OnLanguageChange += () => m_TextMeshProUGUI.text = GetLocalizedText(Key);

            if (LocalizationManager.IsLocaReady)
            {
                m_TextMeshProUGUI.text = GetLocalizedText(Key);
            }
            else
            {
                LocalizationManager.OnLocalizationReady += () =>
                {
                    m_TextMeshProUGUI.text = GetLocalizedText(Key);
                };
            }
        }

        public string GetLocalizedText(string key)
        {
            var t = LocalizationManager.GetLocalizedText(key);
            //[#VALUE]
            return LocalizationManager.LocalizeText(t);
        }
    }
}