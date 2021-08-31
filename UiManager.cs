using System;
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

        public Button[] startMenu;
        

        //The Constructor more serves for loading& initializing all the ui menus to be ready & defined when needed.
        public UiManager(GraphicsDevice graph,Texture2D sk1,GraphicsDeviceManager grdm)
        {

            gd = graph;
            skin1 = sk1;
            gdm = grdm;
            // Theres a few ways to do what i want to do but my idea is attach every menu to an array of buttons and assign a background to each aswell,
            // then in update logic we can sense if its clicekd and what happens if it is. so this is more of the design and layout part of the uimanager in which there will only be one 
            //startMenu = startMenu + new Button(gd,,skin1);

















        }










    }
}
