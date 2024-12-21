using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace PixelsoftGames.PixelUI
{
    [RequireComponent(typeof(Button))]
    public class DemoDialogue : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        [Tooltip("The dialogue window prefab to spawn")]
        GameObject DialoguePrefab;

        GameObject dialogueInstance = null;
        Button button;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnDestroy()
        {
            if (dialogueInstance != null)
            {
                Destroy(dialogueInstance);
            }
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Instantiates the dialogue window whenever the button is clicked and disables the button
        /// until the window is closed.
        /// </summary>
        public void On_Click()
        {
            if (dialogueInstance != null)
            {
                Destroy(dialogueInstance);
            }

            Transform canvas = GameObject.Find("Canvas")?.transform;
            if (canvas == null)
            {
                UnityEngine.Debug.LogError("Canvas not found in the scene. Make sure there's a Canvas object.");
                return;
            }

            dialogueInstance = Instantiate(DialoguePrefab, canvas, false);
            button.interactable = false;

            // Add an invisible background to detect clicks outside the dialogue
            GameObject background = new GameObject("DialogueBackground", typeof(RectTransform), typeof(Image), typeof(Button));
            background.transform.SetParent(canvas, false);
            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            Image bgImage = background.GetComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black background

            Button bgButton = background.GetComponent<Button>();
            bgButton.onClick.AddListener(() =>
            {
                // Destroy the dialogue and background when clicking outside
                if (dialogueInstance != null)
                {
                    Destroy(dialogueInstance);
                    dialogueInstance = null;
                }
                Destroy(background);
                button.interactable = true;
            });

            // Ensure the dialogue is above the background
            dialogueInstance.transform.SetAsLastSibling();

            // Set up the dialogue window
            //UIDialogueWindow dialogueWindow = dialogueInstance.GetComponent<UIDialogueWindow>();
            //if (dialogueWindow != null)
            //{
            //    dialogueWindow.SetData(
            //        (UIDialogueWindow.DialogueType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(UIDialogueWindow.DialogueType)).Length),
            //        "Demo Dialogue Window",
            //        "This is a sample dialogue window"
            //    );
            //}
            //else
            //{
            //    UnityEngine.Debug.LogError("UIDialogueWindow component is missing on the DialoguePrefab.");
            //}
        }

        #endregion
    }
}