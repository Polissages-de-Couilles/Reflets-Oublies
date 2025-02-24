using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName ="Game/PNJData")]
public class PNJData : ScriptableObject
{
#if UNITY_EDITOR
    [EditorCools.Button]
    public void InitializeData()
    {
        if (_character == null)
        {
            var character = ScriptableObject.CreateInstance<DialogueCharacterSO>();
            character.name = this.name.ToUpper().Replace("DATA", string.Empty);
            _character = character;

            AssetDatabase.AddObjectToAsset(character, this);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(character);
        }

        _characterNameKey = this.name.ToUpper().Replace("DATA", string.Empty) + "_NAME";
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }

    [EditorCools.Button]
    public void CreateDialogue()
    {
        var dialogue = ScriptableObject.CreateInstance<DialogueContainerSO>();
        dialogue.name = $"{_characterNameKey.Replace("NAME", "")}_Dialogue_{_allDialogue.Count + 1}";
        _allDialogue.Add(dialogue);

        AssetDatabase.AddObjectToAsset(dialogue, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(dialogue);
    }

    [EditorCools.Button]
    public void AddDialogue()
    {
        var dialogue = ScriptableObject.CreateInstance<DialogueContainerSO>();
        dialogue.name = $"Dialogue_{_allDialogue.Count + 1}";
        dialogue.NodeLinkDatas = _dialogueToAdd.NodeLinkDatas;
        dialogue.DialogueChoiceNodeDatas = _dialogueToAdd.DialogueChoiceNodeDatas;
        dialogue.DialogueNodeDatas = _dialogueToAdd.DialogueNodeDatas;
        dialogue.TimerChoiceNodeDatas = _dialogueToAdd.TimerChoiceNodeDatas;
        dialogue.EndNodeDatas = _dialogueToAdd.EndNodeDatas;
        dialogue.EventNodeDatas = _dialogueToAdd.EventNodeDatas;
        dialogue.StartNodeDatas = _dialogueToAdd.StartNodeDatas;
        dialogue.RandomNodeDatas = _dialogueToAdd.RandomNodeDatas;
        dialogue.CommandNodeDatas = _dialogueToAdd.CommandNodeDatas;
        dialogue.IfNodeDatas = _dialogueToAdd.IfNodeDatas;
        _allDialogue.Add(dialogue);

        AssetDatabase.AddObjectToAsset(dialogue, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(dialogue);
        _dialogueToAdd = null;
    }

    [EditorCools.Button]
    public void DeleteLastDialogue()
    {
        if (_allDialogue == null || _allDialogue.Count <= 0) return;

        var dialogue = _allDialogue.Last();
        _allDialogue.Remove(dialogue);
        Undo.DestroyObjectImmediate(dialogue);
        AssetDatabase.SaveAssets();
    }

    [EditorCools.Button]
    public void Rename()
    {
        foreach(var dialogue in _allDialogue)
        {
            //dialogue.name = _characterNameKey.Replace("NAME", "") + dialogue.name.Replace(_characterNameKey.Replace("NAME", ""), "");
            dialogue.name = dialogue.name.Replace("_Act1", "");
        }
    }
#endif

    [SerializeField] DialogueCharacterSO _character;
    public string CharacterNameKey => _characterNameKey;
    [SerializeField] string _characterNameKey;
    [SerializeField] DialogueContainerSO _dialogueToAdd;
    [SerializeField] List<DialogueContainerSO> _allDialogue = new List<DialogueContainerSO>();
    
    public DialogueContainerSO GetDialogue(int index)
    {
        if(index > _allDialogue.Count) return null;
        return _allDialogue[index - 1];
    }
}
