using TMPro;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.UI
{
    public class QuizUIView : MonoBehaviour, IQuizUIView
    {
        [SerializeField] private TextMeshProUGUI questionTMP;
        
        public void SetQuestion(string questionText)
        {
            questionTMP.text = questionText;
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}