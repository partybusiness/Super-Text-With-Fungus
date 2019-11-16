using UnityEngine;
using Fungus;
using UnityEngine.EventSystems;
using System.Linq;

public class SuperMenuDialog : MenuDialog {

    public override bool AddOption(string text, bool interactable, bool hideOption, Block targetBlock)
    {
        bool addedOption = false;
        for (int i = 0; i < cachedButtons.Length; i++)
        {
            var button = cachedButtons[i];
            if (!button.gameObject.activeSelf)
            {
                button.gameObject.SetActive(true);
                button.interactable = interactable;
                if (interactable && autoSelectFirstButton && !cachedButtons.Select(x => x.gameObject).Contains(EventSystem.current.currentSelectedGameObject))
                {
                    EventSystem.current.SetSelectedGameObject(button.gameObject);
                }
                SuperTextMesh textComponent = button.GetComponentInChildren<SuperTextMesh>();
                if (textComponent != null)
                {
                    textComponent.text = text;
                }
                var block = targetBlock;
                button.onClick.AddListener(delegate
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    StopAllCoroutines();
                    // Stop timeout
                    Clear();
                    HideSayDialog();
                    if (block != null)
                    {
                        var flowchart = block.GetFlowchart();
                        #if UNITY_EDITOR
                        // Select the new target block in the Flowchart window
                        flowchart.SelectedBlock = block;
                        #endif
                        gameObject.SetActive(false);
                        // Use a coroutine to call the block on the next frame
                        // Have to use the Flowchart gameobject as the MenuDialog is now inactive
                        flowchart.StartCoroutine(CallBlock(block));
                    }
                });
                addedOption = true;
                break;
            }
        }

            return addedOption;
        }
}