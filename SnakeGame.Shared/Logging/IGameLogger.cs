﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Logging
{
    public interface IGameLogger
    {
        void Debug(string message);
    }
}
