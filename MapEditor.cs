using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class MapEditor
    {


        public Map currentMap { get; set; }

        public UiElement[] tools { get; set; }

        //so the map editor will have a way to set all quadtrees to each individual assets such as hitbox and stuff liek that
        //as well as it will have a way in which the map can be designed efficiently 
        public MapEditor(GraphicsDevice gd, Texture2D btn1, Texture2D btn2, GraphicsDeviceManager gdm)
        {


            tools = new UiElement[2];
            tools[0] = new Button(gd, 0, gdm.PreferredBackBufferHeight - btn1.Height, btn1.Width, btn1.Height, "Load", btn1, false);
            tools[1] = new Button(gd, btn1.Width, gdm.PreferredBackBufferHeight - btn1.Height, btn1.Width, btn1.Height, "Save", btn1, false);
            tools[2] = new TextBox(gd, gdm.PreferredBackBufferWidth / 2 - btn1.Width / 2, gdm.PreferredBackBufferHeight / 2 - btn1.Height / 2, btn1.Width, btn1.Height, 20, "Save Location", btn1, false);
        }

        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i].isActive)
                {
                    tools[i].Draw(sb, sf);
                }
            }
        }

        public void Update()
        {


        }

        public void LoadMap()
        {


        }

        public void SaveMap()
        {
            if (currentMap.hasSave)
            {

            }
            else
            {
                string location = "";

                currentMap.saveLocation = location;
            }

        }

        public void Loadqts()
        {

        }

        public void Saveqts()
        {

        }



    }
}
