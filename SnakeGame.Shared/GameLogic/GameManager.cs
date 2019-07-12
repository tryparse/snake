﻿using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings;

namespace SnakeGame.Shared.GameLogic
{
    // TODO: Make this manager as IUpdatable component
    public class GameManager : IGameManager
    {
        private readonly ILogger _logger;
        private readonly IFoodManager _foodManager;
        private readonly ISnake _snake;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public GameManager(ILogger logger, IFoodManager foodManager, ISnake snake, IGameField gameField, IGameSettings gameSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _foodManager = foodManager ?? throw new ArgumentNullException(nameof(foodManager));
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));
            _gameSettings = gameSettings;

            Initialize();
        }

        public void Initialize()
        {
        }

        public void NewGame()
        {
            _logger.Debug("GameManager.NewGame()");
            _snake.Reset();
        }

        public bool CheckSnakeCollision()
        {
            if (_snake.State != SnakeState.Moving)
            {
                var head = _snake.Tail.FirstOrDefault();

                if (head != null)
                {
                    var tail = _snake.Tail;

                    foreach (var part in tail)
                    {
                        if (head.Bounds.Intersects(part.Bounds))
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
            if (_snake.State == SnakeState.None)
            {
                var head = _snake.Head;

                if (head != null)
                {
                    foreach (var foodComponent in _foodManager.FoodComponents)
                    {
                        if (head.Bounds.Intersects(foodComponent.Food.Bounds))
                        {
                            _logger.Debug("GameManager: Detected food collision");
                            return foodComponent;
                        }
                    }
                }
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
            var head = _snake.Head;

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
            _logger.Debug($"GameManager.Update({gameTime.TotalGameTime})");

            if (CheckSnakeCollision() || CheckWallsCollision())
            {
                NewGame();
            }

            var collidedFood = CheckFoodCollision();

            if (collidedFood != null)
            {
                RemoveFood(collidedFood);
                _foodManager.Add(_foodManager.GenerateRandomFood());
                _snake.Grow();
                IncreaseGameSpeed();
            }
        }

        public void IncreaseGameSpeed()
        {
            _gameSettings.CurrentMoveIntervalTime = (int)Math.Truncate((double)_gameSettings.CurrentMoveIntervalTime);
            _gameSettings.CurrentSnakeMovingTime = (int)Math.Truncate((double)_gameSettings.CurrentSnakeMovingTime);
        }
    }
}
