using System;
using TMPro;

namespace UnityEngine.PlayerIdentity.UI
{
    public class AntiAddictionPanel : MonoBehaviour
    {

        public TextMeshProUGUI _titleLable;
        public TextMeshProUGUI _messageLable;
        public GameObject _quitGameButton;
        public GameObject _tryAgainButton;

        private Canvas canvas;
        private Action _tryAgainCallback;

        private void Awake()
        {
            canvas = gameObject.GetComponent<Canvas>();
        }

        public void ShowPanel(String title,String msg,Action callback)
        {
            canvas.enabled = true;
            _titleLable.text = title;
            _messageLable.text = msg;
            _tryAgainCallback = callback;
            _quitGameButton.SetActive(false);
            _tryAgainButton.SetActive(false);
            if (callback != null)
            {
                _tryAgainButton.SetActive(true);
            }
            else
            {
                _quitGameButton.SetActive(true);
            }
        }

        public void QuitGame()
        {
            canvas.enabled = false;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void TryAgain()
        {
            canvas.enabled = false;
            _tryAgainCallback();
            _tryAgainCallback = null;
        }
    }
}
