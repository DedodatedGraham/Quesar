﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class MapTile
    {
        private const int tileWidth = 32;
        private const int tileHeight = 32;
        private int width;
        private int height;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;

        public MapTile(GraphicsDevice gd, int pWidth, int pHeight)
        {
            graphicsDevice = gd;
            width = pWidth;
            height = pHeight;

        }

        public void Draw(SpriteBatch sp)
        {
            // pass in the graphics device to make a
            //new solo spritebatch independent for the tiling system
            
            Vector2 tilePosition = Vector2.Zero;

            

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    //spriteBatch.FillRectangle(tilePosition, new Size2(tileWidth, tileHeight), Color.Black);
                    //spriteBatch.FillRectangle(tilePosition + new Vector2(1, 1), new Size2(tileWidth - 2, tileHeight - 2), Color.White);
                    //we can now make a box not filled rectangles so we can display over something
                    //top side
                    sp.FillRectangle(new Rectangle(new Point((int)tilePosition.X, (int)tilePosition.Y), new Point(tileWidth, 1)), Color.Black);
                    //left side
                    sp.FillRectangle(new Rectangle(new Point((int)tilePosition.X, (int)tilePosition.Y), new Point(1, tileHeight)), Color.Black);
                    //bottom side
                    sp.FillRectangle(new Rectangle(new Point((int)tilePosition.X, (int)tilePosition.Y + tileHeight - 1), new Point(tileWidth, 1)), Color.Black);
                    //right side
                    sp.FillRectangle(new Rectangle(new Point((int)tilePosition.X + tileWidth - 1, (int)tilePosition.Y), new Point(1, tileHeight)), Color.Black);



                    tilePosition.Y += tileHeight;
                }
                tilePosition.Y = 0;
                tilePosition.X += tileWidth;
            }
        }
    }
}
    

