using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelsoftGames.PixelUI
{
    public class LoadGame : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        [Tooltip("The name of the scene to load on click.")]
        private string sceneToLoad = string.Empty;

        #endregion Fields & Properties

        #region Callbacks

        /// <summary>
        /// Executed when the button is clicked.
        /// </summary>
        public void On_Click()
        {
            if (sceneToLoad != string.Empty)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }

        #endregion Callbacks
    }
}