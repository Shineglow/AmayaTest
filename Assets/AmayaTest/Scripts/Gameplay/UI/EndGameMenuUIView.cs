using System;
using UnityEngine;
using UnityEngine.UI;

namespace AmayaTest.Scripts.Gameplay.UI
{
    public class EndGameMenuUIView : MonoBehaviour, IEndGameMenuUIView
    {
        [SerializeField] private Button restartButton;

        public event Action OnRestartButtonClicked;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnRestartClicked()
        {
            OnRestartButtonClicked?.Invoke();
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}
