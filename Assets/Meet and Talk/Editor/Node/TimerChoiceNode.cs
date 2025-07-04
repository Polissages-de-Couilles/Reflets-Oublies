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
    public class TimerChoiceNode : BaseNode
    {
        private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<string>> audioName = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> audioClip = new List<LanguageGeneric<AudioClip>>();
        private DialogueCharacterSO character = ScriptableObject.CreateInstance<DialogueCharacterSO>();
        private float durationShow = 0;
        private float time = 10;

        public List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();

        public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
        public List<LanguageGeneric<string>> AudioName { get => audioName; set => audioName = value; }
        public List<LanguageGeneric<AudioClip>> AudioClip { get => audioClip; set => audioClip = value; }
        public DialogueCharacterSO Character { get => character; set => character = value; }
        public float DurationShow { get => durationShow; set => durationShow = value; }
        public float ChoiceTime { get => time; set => time = value; }

        private TextField texts_Field;
        private TextField audioName_Field;
        private ObjectField audioClips_Field;
        private FloatField duration_Field;
        private FloatField time_Field;
        private ObjectField character_Field;

        public AvatarPosition avatarPosition;
        public AvatarType avatarType;
        private EnumField AvatarPositionField;
        private EnumField AvatarTypeField;

        public TimerChoiceNode()
        {

        }

        public TimerChoiceNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView)
        {
            editorWindow = _editorWindow;
            graphView = _graphView;

            title = "Timer Choice";
            SetPosition(new Rect(_position, defualtNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddValidationContainer();

            AddInputPort("Input ", Port.Capacity.Multi);
            //OutputPort("Time Out", Port.Capacity.Single);

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

            AvatarPositionField = new EnumField("Avatar Position", avatarPosition);
            AvatarPositionField.RegisterValueChangedCallback(value =>
            {
                avatarPosition = (AvatarPosition)value.newValue;
            });
            AvatarPositionField.name = "AvatarPosition";
            AvatarPositionField.SetValueWithoutNotify(avatarPosition);
            mainContainer.Add(AvatarPositionField);


            AvatarTypeField = new EnumField("Avatar Emotion", avatarType);
            AvatarTypeField.RegisterValueChangedCallback(value =>
            {
                avatarType = (AvatarType)value.newValue;
            });
            AvatarTypeField.name = "AvatarEmotion";
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

            /* DIALOGUE DURATION */
            Label label_duration = new Label("Time to Display Options");
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

            /* TEXT NAME */
            Label label_time = new Label("Time to Make Decision");
            label_time.AddToClassList("label_time");
            label_time.AddToClassList("Label");
            mainContainer.Add(label_time);

            time_Field = new FloatField("");
            time_Field.RegisterValueChangedCallback(value =>
            {
                time = value.newValue;
            });
            time_Field.SetValueWithoutNotify(time);
            time_Field.AddToClassList("TextTime");
            mainContainer.Add(time_Field);

            Button button = new Button()
            {
                text = "+ Add Choice Option"
            };
            button.clicked += () =>
            {
                AddChoicePort(this);
            };

            titleButtonContainer.Add(button);

            //if (outputContainer.Query("connector").ToList().Count() == 0) AddChoicePort(this);
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

            foreach (DialogueNodePort nodePort in dialogueNodePorts)
            {
                nodePort.TextField.RegisterValueChangedCallback(value =>
                              {
                                  nodePort.TextLanguage.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
                              });
                nodePort.TextField.SetValueWithoutNotify(nodePort.TextLanguage.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            }
        }

        public override void LoadValueInToField()
        {
            texts_Field.SetValueWithoutNotify(texts.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            audioName_Field.SetValueWithoutNotify(audioName.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            character_Field.SetValueWithoutNotify(character);
            AvatarPositionField.SetValueWithoutNotify(avatarPosition);
            AvatarTypeField.SetValueWithoutNotify(avatarType);
            audioClips_Field.SetValueWithoutNotify(audioClip.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);
            duration_Field.SetValueWithoutNotify(durationShow);
            time_Field.SetValueWithoutNotify(time);
        }

        public Port AddChoicePort(BaseNode _basenote, DialogueNodePort _dialogueNodePort = null)
        {
            Port port = GetPortInstance(Direction.Output);

            string outputPortName = "";
            int outputPortCount = _basenote.outputContainer.Query("connector").ToList().Count();
            if (outputPortCount < 9) { outputPortName = $"Choice 0{outputPortCount + 1}"; }
            else { outputPortName = $"Choice {outputPortCount + 1}"; }

            DialogueNodePort dialogueNodePort = new DialogueNodePort();
            dialogueNodePort.PortGuid = Guid.NewGuid().ToString(); //NOWE

            foreach (LocalizationEnum language in (LocalizationEnum[])Enum.GetValues(typeof(LocalizationEnum)))
            {
                dialogueNodePort.TextLanguage.Add(new LanguageGeneric<string>()
                {
                    languageEnum = language,
                    LanguageGenericType = outputPortName
                });
            }

            if (_dialogueNodePort != null)
            {
                dialogueNodePort.InputGuid = _dialogueNodePort.InputGuid;
                dialogueNodePort.OutputGuid = _dialogueNodePort.OutputGuid;

                if (_dialogueNodePort.PortGuid == "") { _dialogueNodePort.PortGuid = Guid.NewGuid().ToString(); } //NOWE
                dialogueNodePort.PortGuid = _dialogueNodePort.PortGuid; //NOWE

                foreach (LanguageGeneric<string> languageGeneric in _dialogueNodePort.TextLanguage)
                {
                    dialogueNodePort.TextLanguage.Find(language => language.languageEnum == languageGeneric.languageEnum).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            dialogueNodePort.TextField = new TextField();
            dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                dialogueNodePort.TextLanguage.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType = value.newValue;
            });
            dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguage.Find(language => language.languageEnum == editorWindow.LanguageEnum).LanguageGenericType);

            if (outputPortCount != 0)
            {
                dialogueNodePort.TextField.AddToClassList("ChoiceLabel");
                port.contentContainer.Add(dialogueNodePort.TextField);
                Button deleteButton = new Button(() => DeleteButton(_basenote, port))
                {
                    text = "X"
                };
                port.contentContainer.Add(deleteButton);

                port.portName = "";
            }
            else
            {
                dialogueNodePort.TextField.SetValueWithoutNotify("");
                dialogueNodePort.TextField.SetEnabled(false);
                dialogueNodePort.TextField.visible = false;
                port.contentContainer.Add(dialogueNodePort.TextField);
                port.portName = "Time Out";
                port.AddToClassList("FirstPort");
            }
#if UNITY_EDITOR
            dialogueNodePort.MyPort = port;
#endif

            dialogueNodePorts.Add(dialogueNodePort);

            _basenote.outputContainer.Add(port);

            _basenote.RefreshPorts();
            _basenote.RefreshExpandedState();

            return port;
        }

        private void DeleteButton(BaseNode _node, Port _port)
        {
#if UNITY_EDITOR
            DialogueNodePort tmp = dialogueNodePorts.Find(port => port.MyPort == _port);
#endif
            dialogueNodePorts.Remove(tmp);

            IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == _port);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                graphView.RemoveElement(edge);
            }

            _node.outputContainer.Remove(_port);

            _node.RefreshPorts();
            _node.RefreshExpandedState();
        }


        public override void SetValidation()
        {
            List<string> error = new List<string>();
            List<string> warning = new List<string>();

            Port input = inputContainer.Query<Port>().First();
            if (!input.connected) warning.Add("Node cannot be called");


            if (dialogueNodePorts.Count < 2) error.Add("You need to add more Choice");
            else
            {
                if (!dialogueNodePorts[0].MyPort.connected) error.Add("Time Out does not lead to any node");
                for(int i = 1; i < dialogueNodePorts.Count; i++)
                {
                    if (!dialogueNodePorts[i].MyPort.connected) error.Add($"Choice ID:{i} does not lead to any node");
                }
            }
            if (ChoiceTime < 3) warning.Add("Short time for Make Decision");
            for(int i = 0; i < Texts.Count; i++) { if (Texts[i].LanguageGenericType == "") warning.Add($"No Text for {Texts[i].languageEnum} Language"); }

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
