using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Fungus;
using System;

public class SuperSayDialog : SayDialog {

    public enum DisappearMode { fadeOnly, undrawThenFade, undrawOnly }

    public DisappearMode disappearMode;

    protected bool undrawWhenDone=false;

    protected SuperTextMeshWriter superWriter;

    [Tooltip("The name text UI object")]
    [SerializeField] protected SuperTextMesh nameSuperText;

    protected new void Start()
    {
        // Dialog always starts invisible, will be faded in when writing starts
        GetCanvasGroup().alpha = 0f;

        // Add a raycaster if none already exists so we can handle dialog input
        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();    
        }

        // It's possible that SetCharacterImage() has already been called from the
        // Start method of another component, so check that no image has been set yet.
        // Same for nameText.

        if (nameText != null && nameText.text == "")
        {
            SetCharacterName("", Color.white);
        }
        if (nameSuperText != null && nameSuperText.text == "")
        {
            SetCharacterName("", Color.white);
        }
        if (currentCharacterImage == null)
        {                
            // Character image is hidden by default.
            SetCharacterImage(null);
        }

    }

    public override void SetCharacter(Character character)
    {
        base.SetCharacter(character);

        if (character == null)
        {
            if (nameSuperText != null && nameSuperText.text == "")
            {
                SetCharacterName("", Color.white);
            }
        }
        else 
        {
            string characterName = character.NameText;

            if (characterName == "")
            {
                // Use game object name as default
                characterName = character.GetObjectName();
            }

            SetCharacterName(characterName, character.NameColor);
        }
    }

    public override void SetCharacterName(string name, Color color)
    {
        if (nameSuperText != null)
        {
                var subbedName = stringSubstituter.SubstituteStrings(name);
                nameSuperText.text = subbedName;
                nameSuperText.color = color;
        }
    }

    protected SuperTextMeshWriter GetSuperWriter()
    {
        GetWriter();
        if (superWriter != null)
        {
            return superWriter;
        }
       
        superWriter = GetComponent<SuperTextMeshWriter>();
        
        if (writer == null)
        {
            superWriter = gameObject.AddComponent<SuperTextMeshWriter>();
        }
        writer = superWriter;

        return superWriter;
    }


	public override IEnumerator DoSay (string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, bool stopVoiceover, bool waitForVO, AudioClip voiceOverClip, Action onComplete)
    {
        SuperTextMeshWriter writer = GetSuperWriter();

        if (writer.IsWriting || writer.IsWaitingForInput)
        {
            writer.Stop();
			while (writer.IsWriting || writer.IsWaitingForInput)
            {
                yield return null;
            }
        }

        switch (disappearMode)
        {
            case DisappearMode.fadeOnly:
                this.fadeWhenDone = fadeWhenDone;
                undrawWhenDone = false;
                break;
            case DisappearMode.undrawOnly:
                undrawWhenDone = fadeWhenDone;
                this.fadeWhenDone = false;
                break;
            case DisappearMode.undrawThenFade:
                undrawWhenDone = fadeWhenDone;
                this.fadeWhenDone = fadeWhenDone;
                break;
        }
        writer.disappearMode = disappearMode;
        

        // Voice over clip takes precedence over a character sound effect if provided

        AudioClip soundEffectClip = null;
        if (voiceOverClip != null)
        {
            WriterAudio writerAudio = GetWriterAudio();
			writerAudio.OnVoiceover(voiceOverClip);
        }
        else if (speakingCharacter != null)
        {
            
            CharacterSuperAudio charSuper =  speakingCharacter.gameObject.GetComponent<CharacterSuperAudio>();
            SuperTextMesh superText = GetComponent<SuperTextMeshWriter>().getSuperText();
            if (charSuper != null && superText!=null)
            {
                setSuperAudioFromCharacter(superText,charSuper);
            }
            else
            {
                text = "<v=" + speakingCharacter.name + ">" + text;
                //soundEffectClip = speakingCharacter.soundEffect;
            }
        }
        StartCoroutine(writer.Write(text, clearPrevious, waitForInput, stopVoiceover, waitForVO, soundEffectClip, onComplete));

    }

    //Gets properties from charSuper and applies them to the SuperTextMesh
    public static void setSuperAudioFromCharacter(SuperTextMesh stm, CharacterSuperAudio charSuper)
    {
        stm.audioClips = charSuper.audioClips;
        stm.stopPreviousSound = charSuper.stopPreviousSound;
        stm.minPitch = charSuper.minPitch;
        stm.maxPitch = charSuper.maxPitch;
    }
}
