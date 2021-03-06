﻿using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.SceneManagement;

namespace SnakeGame.Shared.GameLogic
{
    public class GameManager : IGameManager
    {
        private readonly IGameLogger _logger;
        private readonly IFoodManager _foodManager;
        private readonly ISnakeGameComponent _snakeComponent;
        private readonly IGameFieldEntity _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly IGamePoints _gamePoints;
        private readonly ISnakeEntity _snakeEntity;
        private readonly ISceneManager _sceneManager;
        private readonly Game _game;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameKeys _gameKeys;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public GameManager(IGameLogger logger, IFoodManager foodManager, ISnakeGameComponent snake, IGameFieldEntity gameField,
            IGameSettings gameSettings, IGamePoints gamePoints, ISnakeEntity snakeEntity, ISceneManager sceneManager, Game game, IGraphicsSystem graphicsSystem, IGameKeys gameKeys)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _foodManager = foodManager ?? throw new ArgumentNullException(nameof(foodManager));
            _snakeComponent = snake ?? throw new ArgumentNullException(nameof(snake));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gamePoints = gamePoints ?? throw new ArgumentNullException(nameof(gamePoints));
            _snakeEntity = snakeEntity ?? throw new ArgumentNullException(nameof(snakeEntity));
            _sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _gameKeys = gameKeys ?? throw new ArgumentNullException(nameof(gameKeys));

            Initialize();
        }

        public void Initialize()
        {
            IsPaused = false;
        }

        public void NewGame()
        {
            _logger.Debug("GameManager.NewGame(): BEGIN");

            _snakeComponent.Reset();
            _gamePoints.Reset();
            _gamePoints.DecrementLives();

            if (_gamePoints.RemainingLives <= 0)
            {
                GameOver();
            }

            _logger.Debug("GameManager.NewGame(): COMPLETE");
        }

        private void GameOver()
        {
            _sceneManager.Load(new GameOverScene(_game, _sceneManager, _graphicsSystem, _gameSettings, _logger, _gameKeys, _gamePoints));
        }

        public bool CheckSnakeCollision()
        {
            if (_snakeEntity.State != SnakeState.Moving)
            {
                var head = _snakeEntity.Head;

                if (head != null)
                {
                    var tail = _snakeEntity.Tail;

                    foreach (var part in tail)
                    {
                        if (Vector2.Distance(head.Position, part.Position) < _gameSettings.TileSize)
                        {
                            _logger.Debug($"Collision: r1={head.Bounds} r2={part.Bounds}");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public IFoodGameComponent CheckFoodCollision()
        {
            if (_snakeEntity.State != SnakeState.None)
            {
                return null;
            }

            var head = _snakeEntity.Head;

            if (head == null)
            {
                return null;
            }

            foreach (var foodComponent in _foodManager.FoodComponents)
            {
                if (!head.Bounds.Intersects(foodComponent.Bounds))
                {
                    continue;
                }

                _logger.Debug("GameManager: Detected food collision");
                return foodComponent;
            }

            return null;
        }

        public void RemoveFood(IFoodGameComponent food)
        {
            _logger.Debug("GameManager: Removed food");

            _foodManager.Remove(food);
        }

        public void GenerateRandomFood()
        {
            _logger.Debug("GameManager: Random food generated");

            var food = _foodManager.GenerateRandomFood();
            _foodManager.Add(food);
        }

        public bool CheckWallsCollision()
        {
            var head = _snakeEntity.Head;

            if (head != null
                && (head.Bounds.Left < _gameField.Bounds.Left
                    || head.Bounds.Right > _gameField.Bounds.Right
                    || head.Bounds.Top < _gameField.Bounds.Top
                    || head.Bounds.Bottom > _gameField.Bounds.Bottom))
            {
                _logger.Debug("GameManager: Detected walls collision");
                return true;
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (CheckSnakeCollision() || CheckWallsCollision())
            {
                NewGame();
            }

            var collidedFood = CheckFoodCollision();

            if (collidedFood != null)
            {
                RemoveFood(collidedFood);
                _foodManager.Add(_foodManager.GenerateRandomFood());
                _snakeEntity.Grow();
                IncreaseGameSpeed();
                _gamePoints.IncrementPoints();
            }
        }

        public void IncreaseGameSpeed()
        {
            if (_gameSettings.CurrentMoveIntervalTime > _gameSettings.LimitMoveIntervalTime)
            {
                _gameSettings.CurrentMoveIntervalTime = Math.Max(_gameSettings.LimitMoveIntervalTime, (int)Math.Truncate(_gameSettings.CurrentMoveIntervalTime * .9d));
            }
        }

        public bool IsPaused { get; private set; }

        public void TogglePause()
        {
            _snakeComponent.ToggleEnabled();
        }

        public void TogglePause(bool isPaused)
        {
            IsPaused = isPaused;
            _snakeComponent.ToggleEnabled(!isPaused);
        }
    }
}
