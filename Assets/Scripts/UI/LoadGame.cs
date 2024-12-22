using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelsoftGames.PixelUI
{
    public class LoadGame : MonoBehaviour
    {
        public AudioClip buttonSound; // 用于播放的音效
        private AudioSource audioSource;

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = buttonSound;
        }

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
            audioSource.Play();
            Invoke("LoadNextScene", buttonSound.length);
        }

        void LoadNextScene()
        {
            SceneManager.LoadScene(sceneToLoad); // 更改为目标场景名称
        }

        #endregion Callbacks

    }
}