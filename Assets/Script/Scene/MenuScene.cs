using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Scene
{
    public class MenuScene : MonoBehaviour
    {
        void Start ()
        {
            AudioManager.Instance.PlayBGM();
        }
        public void StartGame()
        {
            SaveManager.Instance.DeleteSaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void ContinuGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void FreeModeGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartGame();
            }
        }
    }
}
