using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelsoftGames.PixelUI
{
    public class LoadGame : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        [Tooltip("The index of the scene to load on click.")]
        int indexToLoad = -1;

        #endregion

        #region Callbacks

        /// <summary>
        /// Executed when the button is clicked.
        /// </summary>
        public void On_Click()
        {
            if (indexToLoad == -1)
                return;

            SceneManager.LoadScene(indexToLoad);
        }

        #endregion
    }
}