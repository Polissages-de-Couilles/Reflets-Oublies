using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PdCWord : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    public string Word {  get; private set; }
    public string Key {  get; private set; }
    public void SetText(string text, int id)
    {
        _text.text = text;
    }

    public void SetText(TranslatedWord text, int id)
    {
        if(TryGetComponent(out Button button))
        {
            button.onClick.AddListener(() => GameManager.Instance.PdCManager.AddWord(Key));
        }
        _text.text = text.Word;
        Word = text.GetTranslatedWord();
        Key = text.Key;
        if(id == 0)
        {
            GetComponent<DefaultUiControllerInput>().enabled = true;
        }
    }
}
