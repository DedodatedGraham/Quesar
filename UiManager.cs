﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class UiManager
    {
        //This is the UI Manager Class, The main goal is for this class to handle all buttons, onscreen interactions and the changing out backgrounds in/out of menus.
        //Aiming to do this by having both the images and sensing all happen in this class and subclasses 

        public GraphicsDevice gd;
        public Texture2D skin1;
        public GraphicsDeviceManager gdm;

        public UiElement[] startMenu;
        public UiElement[] optionMenu;
        public UiElement[] charCreateMenu;
        

        //The Constructor more serves for loading& initializing all the ui menus to be ready & defined when needed.
        public UiManager(GraphicsDevice graph,Texture2D sk1,GraphicsDeviceManager grdm)
        {

            gd = graph;
            skin1 = sk1;
            gdm = grdm;
                // Theres a few ways to do what i want to do but my idea is attach every menu to an array of buttons and assign a background to each aswell,
                // then in update logic we can sense if its clicekd and what happens if it is. so this is more of the design and layout part of the uimanager in which there will only be one 
                //Im thinking just about 10 pixels worth of space between each button on the main menu?
                startMenu = new UiElement[3];
                startMenu[0] = new Button( gd, (gdm.PreferredBackBufferWidth/2) - 50,(gdm.PreferredBackBufferHeight/2) - 85,100,50,"Start",skin1,true);
                startMenu[1] = new Button(gd, (gdm.PreferredBackBufferWidth/2) - 50, (gdm.PreferredBackBufferHeight/2) - 25, 100, 50, "Options", skin1, true);
                startMenu[2] = new Button(gd, (gdm.PreferredBackBufferWidth / 2) - 50, (gdm.PreferredBackBufferHeight / 2) + 35, 100, 50, "Exit Game", skin1, true);
            
                //Option buttons 
                optionMenu = new Button[1];


                //CreateChar 
                charCreateMenu = new UiElement[1];
                charCreateMenu[0] = new TextBox(gd, (gdm.PreferredBackBufferWidth/2)-100,(gdm.PreferredBackBufferHeight/2) - 85,200,50,"Name:",skin1,true);

        }


        public void Draw(SpriteBatch sp, SpriteFont font, int curUiStage)
        {
            if(curUiStage == 0)
            {
                DrawStartMenu (sp,font);
            }
            if(curUiStage == 2)
            {
                DrawOptionsMenu(sp,font);
            }

            if(curUiStage == 11)
            {
                DrawCharCreate(sp,font);
            }

        }

        public void DrawStartMenu(SpriteBatch sp, SpriteFont font)
        {
            int i = 0;
            while (i < startMenu.Length)
            {
                startMenu[i].Draw(sp, font);
                i++;
            }
        }
        public void DrawOptionsMenu(SpriteBatch sp, SpriteFont font)
        {
            int i = 0;
            while (i < optionMenu.Length)
            {
                optionMenu[i].Draw(sp, font);
                i++;
            }
        }

        public void DrawCharCreate(SpriteBatch sp, SpriteFont font)
        {
            int i = 0;
            while (i < charCreateMenu.Length)
            {
                charCreateMenu[i].Draw(sp, font);
                i++;
            }
        }
        
        
        
        
        
        public int UpdateManager(GameTime gameTime,int curUiStage)
        {
            if(curUiStage == 0)
            {
                if (startMenu[0].isClicked())
                {
                    //Goes to base Background
                    return 1;
                }
                if (startMenu[1].isClicked())
                {
                    //Goes to option menu
                    return 2;
                }
                if (startMenu[2].isClicked())
                {
                    //Exits
                    return 3;
                }
            }
            if(curUiStage == 1)
            {
                //Character Selection Menu

                return 11;
            }



            if(curUiStage == 11)
            {
                if (charCreateMenu[0].isClicked())
                {
                    ((TextBox)charCreateMenu[0]).isTyping = true;
                }

            }
            
            return curUiStage;
            


            //Returns base Value if nothing is doable
            

        }
        public void UpdateLogic()
        {

        }
        
        









    }
}
