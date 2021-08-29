using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quesar
{ 
    public class ShipTile
    {
        private const int tileWidth = 32;
        private const int tileHeight = 32;
        private int width;
        private int height;



        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        
        public ShipTile(GraphicsDevice gd,int pWidth, int pHeight)
        {
            graphicsDevice = gd;
            width = pWidth;
            height = pHeight;
            
            
          
        }

        public void Draw(OrthographicCamera camera)
        {
            // pass in the graphics device to make a
            //new solo spritebatch independent for the tiling system
            spriteBatch = new SpriteBatch(graphicsDevice);
            Vector2 tilePosition = Vector2.Zero;
            

            spriteBatch.Begin(SpriteSortMode.Deferred,null ,null,transformMatrix: camera.GetViewMatrix());
            
            
            

            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y<height; y++)
                {

                    //spriteBatch.FillRectangle(tilePosition, new Size2(tileWidth, tileHeight), Color.Black);
                    //spriteBatch.FillRectangle(tilePosition + new Vector2(1, 1), new Size2(tileWidth - 2, tileHeight - 2), Color.White);
                    //we can now make a box not filled rectangles so we can display over something
                    //top side
                    spriteBatch.FillRectangle(new Rectangle(new Point((int)tilePosition.X,(int)tilePosition.Y),new Point(tileWidth,1)), Color.Black);
                    //left side
                    spriteBatch.FillRectangle(new Rectangle(new Point((int)tilePosition.X, (int)tilePosition.Y), new Point(1,tileHeight)), Color.Black);
                    //bottom side
                    spriteBatch.FillRectangle(new Rectangle(new Point((int)tilePosition.X, (int)tilePosition.Y+tileHeight-1), new Point(tileWidth, 1)), Color.Black);
                    //right side
                    spriteBatch.FillRectangle(new Rectangle(new Point((int)tilePosition.X+tileWidth-1, (int)tilePosition.Y), new Point(1,tileHeight)), Color.Black);




                    tilePosition.Y += tileHeight;
                }
                tilePosition.Y = 0;
                tilePosition.X += tileWidth;
            }

            
            spriteBatch.End();
        }
    }
}
