using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDC
{
    public class PetRock : Interactible
    {
        [SerializeField] int pdc;
        List<string> messages;

        public override void OnInteraction()
        {
            StartCoroutine(OnInteractionCoroutine());
        }

        IEnumerator OnInteractionCoroutine()
        {
            var t = GameManager.Instance.PdCManager.GoogleSheetManager.GetMessages(pdc);
            yield return t;
            messages = t.Current;

            for (int i = 0; i < messages.Count; i++)
            {
                Debug.Log(DisplayMessage(i));
            }
        }

        string DisplayMessage(int id)
        {
            if(id >= messages.Count) return string.Empty;

            string text = messages[id];
            text = Localization.LocalizationManager.LocalizeText(text);
            text = "[f]" + text;
            text = Localization.LocalizationManager.LocalizeText(text);
            return text;
        }
    }
}