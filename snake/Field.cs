using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
	class Field : DrawableGameComponent
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public Field(Game game, int width, int height) : base(game)
		{
			Width = width;
			Height = height;
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}
