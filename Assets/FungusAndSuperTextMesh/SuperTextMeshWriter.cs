using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Fungus
{
    public class SuperTextMeshWriter : Writer
	{
        

        protected SuperTextMesh superText;

        protected string lastText = "";

        [Tooltip("Extra delay in seconds if it's not waiting for input.")]
        public float extraDelay = 0.5f;

        [HideInInspector]
        public SuperSayDialog.DisappearMode disappearMode;

        public SuperTextMesh getSuperText() {
            
            if (superText != null)
                return superText;
            
            if (targetTextObject != null)
            {
                superText = targetTextObject.GetComponent<SuperTextMesh>();
                
                return superText;
            }
            superText = GetComponentInChildren<SuperTextMesh>();
            
            return superText;
        }

		protected override void Awake()
		{
			GameObject go = targetTextObject;
			if (go == null)
			{
				go = gameObject;
			}

			textUI = go.GetComponent<Text>();
            superText = go.GetComponent<SuperTextMesh>();
			inputField = go.GetComponent<InputField>();
			textMesh = go.GetComponent<TextMesh>();

			// Try to find any component with a text property
			if (textUI == null && inputField == null && textMesh == null)
			{
				foreach (Component c in GetComponents<Component>())
				{
					textProperty = c.GetType().GetProperty("text");
					if (textProperty != null)
					{
						textComponent = c;
						break;
					}
				}
			}

			// Cache the list of child writer listeners
			foreach (Component component in GetComponentsInChildren<Component>())
			{
				IWriterListener writerListener = component as IWriterListener;
				if (writerListener != null)
				{
					writerListeners.Add(writerListener);
				}
			}
		}

		protected override void Start()
		{
			
		}
		
		public override bool HasTextObject()
		{
			return (textUI != null || inputField != null || textMesh != null || textComponent != null || superText != null);
		}
		
		

		public override void SetTextColor(Color textColor)
		{
            if (superText != null)
            {
                superText.color = textColor;
            } else if (textUI != null)
			{
				textUI.color = textColor;
			}
			else if (inputField != null)
			{
				if (inputField.textComponent != null)
				{
					inputField.textComponent.color = textColor;
				}
			}
			else if (textMesh != null)
			{
				textMesh.color = textColor;
			}
		}
		
		public override void SetTextAlpha(float textAlpha)
		{
            if (superText != null)
            {
                Color tempColor = textUI.color;
                tempColor.a = textAlpha;
                superText.color = tempColor;
                
            }
            else if (textUI != null)
			{
				Color tempColor = textUI.color;
				tempColor.a = textAlpha;
				textUI.color = tempColor;
			}
			else if (inputField != null)
			{
				if (inputField.textComponent != null)
				{
					Color tempColor = inputField.textComponent.color;
					tempColor.a = textAlpha;
					inputField.textComponent.color = tempColor;
				}
			}
			else if (textMesh != null)
			{
				Color tempColor = textMesh.color;
				tempColor.a = textAlpha;
				textMesh.color = tempColor;
			}
		}


        public override IEnumerator Write (string content, bool clear, bool waitForInput, bool stopAudio, bool waitForVO, AudioClip audioClip, Action onComplete)
        {

            if (!HasTextObject())
            {
                yield break;
            }

            isWriting = true;

            gameObject.SetActive(true);

            
            
            
            NotifyStart(audioClip);

            UnityAction action = null;
            UnityAction action2 = null;

            if (disappearMode == SuperSayDialog.DisappearMode.fadeOnly)
            {
                action = () =>
                {
                    superText.onCompleteEvent.RemoveListener(action2);
                    isWriting = false; 
                    onComplete();
                };
            }
            else
            {
                //it starts fading when isWriting is false, so we might need to postpone that until the undraw is complete
                action = () =>
                {
                    superText.onCompleteEvent.RemoveListener(action2);
                    action2 = () =>
                    {
                        isWriting = false;
                        onComplete();
                    };
                    superText.onUndrawnEvent.AddListener(action2);
                    superText.UnRead();
                    
                    
                };
            }



            if (waitForInput)
            {
                action2 = () =>
                {
                    StartCoroutine(DoWaitForInputOnComplete(clear, action));
                };
                superText.onCompleteEvent.AddListener(action2);
                //superText.onUndrawnEvent.AddListener(action2);

            }
            else
            {
                action2 = () =>
                {
                    StartCoroutine(DoWaitForTimeOnComplete(clear, action));
                };
                superText.onCompleteEvent.AddListener(action2);
                //superText.onCompleteEvent.AddListener(action);
            }

            if (clear)
            {
                superText.Text = content;
                //superText.Rebuild();
            }
            else
            {
                superText.Append(content);
            }
            
            
            

        }
	    

/*		public override string GetTagHelp()
		{
			return "";
		}*/




        protected virtual IEnumerator DoWaitForTimeOnComplete(bool clear, UnityAction onComplete)
        {
            NotifyPause();

            yield return new WaitForSeconds(extraDelay);

            if (clear)
            {
                //superText.text = "";
            }

            NotifyResume();
            onComplete();
        }

        protected virtual IEnumerator DoWaitForInputOnComplete(bool clear, UnityAction onComplete)
        {
            NotifyPause();

            inputFlag = false;
            isWaitingForInput = true;

            
            while (!inputFlag && !exitFlag)
            {
                yield return null;
            }

            isWaitingForInput = false;
            inputFlag = false;

            if (clear)
            {
                //superText.text = "";
            }

            NotifyResume();
            onComplete();
        }


		public override void OnNextLineEvent()
		{
			
			inputFlag = true;
			superText.SkipToEnd();

			if (isWriting)
			{
				NotifyInput();
			}
		}
		
	}

}

