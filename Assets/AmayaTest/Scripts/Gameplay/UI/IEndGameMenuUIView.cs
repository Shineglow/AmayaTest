using System;
using AmayaTest.Scripts.General.UI;

namespace AmayaTest.Scripts.Gameplay.UI
{
    public interface IEndGameMenuUIView : IView
    {
        event Action OnRestartButtonClicked;
    }
}