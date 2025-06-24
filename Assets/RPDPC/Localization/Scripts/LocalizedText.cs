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

        [SerializeField] int nbParentToRebuild = 0;

        public void Start()
        {
            m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            OnLanguageChange += () => m_TextMeshProUGUI.text = GetLocalizedText(Key);

            if (LocalizationManager.IsLocaReady)
            {
                m_TextMeshProUGUI.text = GetLocalizedText(Key);
                RebuildUI();
            }
            else
            {
                LocalizationManager.OnLocalizationReady += () =>
                {
                    m_TextMeshProUGUI.text = GetLocalizedText(Key);
                    RebuildUI();
                };
            }
        }

        public string GetLocalizedText(string key)
        {
            var t = LocalizationManager.GetLocalizedText(key);
            //[#VALUE]
            return LocalizationManager.LocalizeText(t);
        }

        private void RebuildUI()
        {
            var parent = this.transform;
            for(int i = 0; i < nbParentToRebuild; i++)
            {
                parent = parent.parent;
            }
            GameManager.Instance.RebuildLayout(parent);
        }
    }
}