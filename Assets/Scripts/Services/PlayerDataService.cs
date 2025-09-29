using Sammy.Interfaces;
using Sammy.Models;
using UniRx;

namespace Sammy.Services
{
    public class PlayerDataService : IPlayerDataService
    {
        private readonly IGameStateService _gameStateService;
        private readonly IAudioService _audioService;
        
        private int _points;
        private int _lives;
        private int _health;
        private readonly int _maxHealth = 4;

        private readonly ReactiveProperty<int> _wasps = new(0);
        private readonly ReactiveProperty<CrystalColor> _currentCrystal = new(CrystalColor.NONE);
        
        public IReadOnlyReactiveProperty<int> Wasps => _wasps;
        public IReadOnlyReactiveProperty<CrystalColor> CurrentCrystal => _currentCrystal;
        public WaspType CurrentWaspType { get; private set; }
        public int Points => _points;
        public int Lives => _lives;

        public PlayerDataService(IGameStateService gameStateService, IAudioService audioService)
        {
            _gameStateService = gameStateService;
            _audioService = audioService;
        }

        public void CollectPoints(int amount)
        {
            _points += amount;
            _gameStateService.CheckWinCondition(_points);
        }

        public void AddWasps(int amount)
        {
            _wasps.Value += amount;
            if (_wasps.Value > 99)
                _wasps.Value = 99;
            _audioService.Play(AudioSounds.WaspPickup);
        }

        public void SetWasps(int amount)
        {
            _wasps.Value = amount;
            if (_wasps.Value > 99)
                _wasps.Value = 99;
            _audioService.Play(AudioSounds.WaspPickup);
        }

        public void DecreaseWasps()
        {
            if (_wasps.Value <= 0) return;
            _wasps.Value--;
        }

        public void ChangeCrystal(CrystalColor newColor)
        {
            _currentCrystal.Value = newColor;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _lives--;
                if (_lives < 0)
                {
                    _lives = 0;
                    _gameStateService.GameOver();
                }
                ResetHealth();
            }
            _audioService.Play(AudioSounds.PlayerHurt);
        }

        public void ResetHealth()
        {
            _health = _maxHealth;
        }
    }
}