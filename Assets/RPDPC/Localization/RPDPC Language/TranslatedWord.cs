using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using PDC.Localization;
using System.Linq;

[CreateAssetMenu(menuName ="Game/TranslatedWord")]
public class TranslatedWord : ScriptableObject
{
    public string Key => _key;
    [SerializeField] string _key;

    public string Word => LocalizationManager.GetLocalizedText(Key);

    public string GetTranslatedWord()
    {
        int id = 0;
        for (int i = 0; i < Word.Length; i++)
        {
            var letter = Word[i];
            int index = char.ToUpper(letter) - 64;
            id += index;
        }

        //Debug.Log(id);
        List<char> list = Word.ToList();
        Tools.Shuffle(list, id);

        string trad = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            trad += $"<sprite={char.ToUpper(list[i]) - 65}>";
        }

        return trad;
    }
}