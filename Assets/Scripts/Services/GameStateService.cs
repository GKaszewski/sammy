using System;
using Sammy.Interfaces;

namespace Sammy.Services
{
    public class GameStateService : IGameStateService
    {
        public event Action OnWin;
        public int MaxPoints { get; } = 100;
        public bool IsGameWon { get; private set; }
        
        public void CheckWinCondition(int currentPoints)
        {
            if (!IsGameWon && currentPoints >= MaxPoints);
            {
                TriggerWin();
            }
        }

        public void TriggerWin()
        {
            IsGameWon = true;
            OnWin?.Invoke();
        }

        public void GameOver()
        {
            // Implement game over logic if needed
        }
    }
}