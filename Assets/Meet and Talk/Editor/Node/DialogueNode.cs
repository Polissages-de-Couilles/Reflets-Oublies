using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using MeetAndTalk.Editor;
using MeetAndTalk.Localization;
using MeetAndTalk.Event;

namespace MeetAndTalk.Nodes
{
    public class DialogueNode : BaseNode
    {
        private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<string>> audioName = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> audioClip = new List<LanguageGeneric<AudioClip>>();
        private DialogueCharacterSO character = ScriptableObject.CreateInstance<DialogueCharacterSO>();
        private float durationShow = 0;
        private bool canBeSkip = false;
        private bool canMove = false;


        public List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();

        public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
        public List<LanguageGeneric<string>> AudioName { get => audioName; set => audioName = value; }
        public List<LanguageGeneric<AudioClip>> AudioClip { get => audioClip; set => audioClip = value; }
        public DialogueCharacterSO Character { get => character; set => character = value; }
        public float DurationShow { get => durationShow; set => durationShow = value; }
        public bool CantBeSkip { get => canBeSkip; set => canBeSkip = value; }
        public bool CanMove { get => canMove; set => canMove = value; }

        private TextField texts_Field;
        private TextField audioName_Field;
        private ObjectField audioClips_Field;
        private TextField name_Field;
        private ObjectField character_Field;
        private FloatField duration_Field;
        private Toggle canBeSkip_Field;
        private Toggle canMove_Field;

        public AvatarPosition avatarPosition;
        public AvatarType avatarType;
        private EnumField AvatarPositionField;
        private EnumField AvatarTypeField;

        public DialogueNode()
        {

        }

        public DialogueNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView)
        {
            editorWindow = _editorWindow;
            graphView = _graphView;

            title = "Dialogue";
            SetPosition(new Rect(_position, defualtNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);


            AddValidationContainer();

            foreach (LocalizationEnum language in (LocalizationEnum[])Enum.GetValues(typeof(LocalizationEnum)))
            {
                texts.Add(new LanguageGeneric<string>
                {
                    languageEnum = language,
                    LanguageGenericType = ""
                });
                audioName.Add(new LanguageGeneric<string>
                {
                    languageEnum = language,
                    LanguageGenericType = ""
                });
                AudioClip.Add(new LanguageGeneric<AudioClip>
                {
                    languageEnum = language,
                    LanguageGenericType = null
                });
            }
            /* AUDIO CLIPS */
            audioClips_Field = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClip.Find(audioClips => audioClips.languageEnum == editorWindow.LanguageEnum).LanguageGenericType,
            };
            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                audioClip.Find(audioClips => audioClips.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClips_Field.SetValueWithoutNotify(audioClip.Find(audioClips => audioClips.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            mainContainer.Add(audioClips_Field);

            Label label_audioName = new Label("Audio Name (Wwise Event)");
            label_audioName.AddToClassList("label_audioName");
            label_audioName.AddToClassList("Label");
            mainContainer.Add(label_audioName);

            audioName_Field = new TextField("");
            audioName_Field.RegisterValueChangedCallback(value =>
            {
                audioName.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
            });
            audioName_Field.SetValueWithoutNotify(texts.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            audioName_Field.multiline = true;

            audioName_Field.AddToClassList("TextBox");
            mainContainer.Add(audioName_Field);

            /* Character CLIPS */
            Label label_character = new Label("Character SO");
            label_character.AddToClassList("label_name");
            label_character.AddToClassList("Label");
            mainContainer.Add(label_character);
            character_Field = new ObjectField()
            {
                objectType = typeof(DialogueCharacterSO),
                allowSceneObjects = false,
            };
            character_Field.RegisterValueChangedCallback(value =>
            {
                character = value.newValue as DialogueCharacterSO;
            });
            character_Field.SetValueWithoutNotify(character);
            mainContainer.Add(character_Field);

            AvatarPositionField = new EnumField("Avatar Position",avatarPosition);
            AvatarPositionField.RegisterValueChangedCallback(value =>
            {
                avatarPosition = (AvatarPosition)value.newValue;
            });
            AvatarPositionField.SetValueWithoutNotify(avatarPosition);
            mainContainer.Add(AvatarPositionField);


            AvatarTypeField = new EnumField("Avatar Emotion", avatarType);
            AvatarTypeField.RegisterValueChangedCallback(value =>
            {
                avatarType = (AvatarType)value.newValue;
            });
            AvatarTypeField.SetValueWithoutNotify(avatarType);
            mainContainer.Add(AvatarTypeField);

            /* TEXT BOX */
            Label label_texts = new Label("Displayed Text");
            label_texts.AddToClassList("label_texts");
            label_texts.AddToClassList("Label");
            mainContainer.Add(label_texts);

            texts_Field = new TextField("");
            texts_Field.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
            });
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            texts_Field.multiline = true;

            texts_Field.AddToClassList("TextBox");
            mainContainer.Add(texts_Field);

            /* TEXT NAME */
            Label label_duration = new Label("Display Time");
            label_duration.AddToClassList("label_duration");
            label_duration.AddToClassList("Label");
            mainContainer.Add(label_duration);

            duration_Field = new FloatField("");
            duration_Field.RegisterValueChangedCallback(value =>
            {
                durationShow = value.newValue;
            });
            duration_Field.SetValueWithoutNotify(durationShow);

            duration_Field.AddToClassList("TextDuration");
            mainContainer.Add(duration_Field);


            Label label_cantBeSkip = new Label("Cant be skip");
            label_cantBeSkip.AddToClassList("label_cantBeSkip");
            label_cantBeSkip.AddToClassList("Label");
            mainContainer.Add(label_cantBeSkip);

            canBeSkip_Field = new Toggle("");
            canBeSkip_Field.RegisterValueChangedCallback(value =>
            {
                CantBeSkip = value.newValue;
            });
            canBeSkip_Field.SetValueWithoutNotify(canBeSkip);

            canBeSkip_Field.AddToClassList("Cant be skip");
            mainContainer.Add(canBeSkip_Field);

            Label label_cantMove = new Label("Can Move");
            label_cantMove.AddToClassList("label_cantMove");
            label_cantMove.AddToClassList("Label");
            mainContainer.Add(label_cantMove);

            canMove_Field = new Toggle("");
            canMove_Field.RegisterValueChangedCallback(value =>
            {
                CanMove = value.newValue;
            });
            canMove_Field.SetValueWithoutNotify(canMove);

            canMove_Field.AddToClassList("Cant be skip");
            mainContainer.Add(canMove_Field);
        }

        public void ReloadLanguage()
        {
            texts_Field.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
            });
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);

            audioName_Field.RegisterValueChangedCallback(value =>
            {
                audioName.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
            });
            audioName_Field.SetValueWithoutNotify(audioName.Find(text => text.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);

            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                audioClip.Find(audioClips => audioClips.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClips_Field.SetValueWithoutNotify(audioClip.Find(audioClips => audioClips.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
        }

        public override void LoadValueInToField()
        {
            texts_Field.SetValueWithoutNotify(texts.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            audioName_Field.SetValueWithoutNotify(audioName.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            audioClips_Field.SetValueWithoutNotify(audioClip.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            character_Field.SetValueWithoutNotify(character);
            AvatarPositionField.SetValueWithoutNotify(avatarPosition);
            AvatarTypeField.SetValueWithoutNotify(avatarType);
            duration_Field.SetValueWithoutNotify(durationShow);
            canBeSkip_Field.SetValueWithoutNotify(canBeSkip);
            canMove_Field.SetValueWithoutNotify(canMove);
        }

        public override void SetValidation()
        {
            List<string> error = new List<string>();
            List<string> warning = new List<string>();

            Port input = inputContainer.Query<Port>().First();
            if (!input.connected) warning.Add("Node cannot be called");

            Port output = outputContainer.Query<Port>().First();
            if (!output.connected) error.Add("Output does not lead to any node");

            if (durationShow < 1 && durationShow != 0) warning.Add("Short time for Make Decision");
            for (int i = 0; i < Texts.Count; i++) { if (Texts[i].LanguageGenericType == "") warning.Add($"No Text for {Texts[i].languageEnum} Language"); }

            ErrorList = error;
            WarningList = warning;

            // Update List
            if (character != null)
            {
                AvatarPositionField.style.display = DisplayStyle.Flex;
                AvatarTypeField.style.display = DisplayStyle.Flex;
            }
            else
            {
                AvatarPositionField.style.display = DisplayStyle.None;
                AvatarTypeField.style.display = DisplayStyle.None;
            }
        }
    }
}