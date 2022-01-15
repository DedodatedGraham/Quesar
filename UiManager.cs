using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public Texture2D charBox;
        public Texture2D defaultSkin;
        public GraphicsDeviceManager gdm;
        
        public string currentdown;
        public string lastdown;
        public string words;


        public GameTime lastback;


        public UiElement[] startMenu;
        public UiElement[] optionMenu;
        public UiElement[] charCreateMenu;

        public string outputNewPlayer;
        

        //The Constructor more serves for loading& initializing all the ui menus to be ready & defined when needed.
        public UiManager(GraphicsDevice graph,Texture2D sk1,Texture2D cb,GraphicsDeviceManager grdm,Texture2D ds)
        {

            gd = graph;
            skin1 = sk1;
            charBox = cb;
            gdm = grdm;
            defaultSkin = ds;

            currentdown = "";
            lastdown = "";
            words = "";
            lastback = new GameTime();
                // Theres a few ways to do what i want to do but my idea is attach every menu to an array of buttons and assign a background to each aswell,
                // then in update logic we can sense if its clicekd and what happens if it is. so this is more of the design and layout part of the uimanager in which there will only be one 
                //Im thinking just about 10 pixels worth of space between each button on the main menu?
                startMenu = new UiElement[3];
                startMenu[0] = new Button( gd, (gdm.PreferredBackBufferWidth/2) - 50, (gdm.PreferredBackBufferHeight/2) - 85,100,50,"Start",skin1,true);
                startMenu[1] = new Button(gd, (gdm.PreferredBackBufferWidth/2) - 50, (gdm.PreferredBackBufferHeight/2) - 25, 100, 50, "Options", skin1, true);
                startMenu[2] = new Button(gd, (gdm.PreferredBackBufferWidth / 2) - 50, (gdm.PreferredBackBufferHeight / 2) + 35, 100, 50, "Exit Game", skin1, true);
            
                //Option buttons 
                optionMenu = new UiElement[2];
                optionMenu[0] = new Button(gd, 50, 50, 100, 50, "Start Map Editor",skin1,false);
                optionMenu[1] = new Button(gd, 4000, 4000, 100, 50, "blah", skin1, false);
                

                //CreateChar 
                charCreateMenu = new UiElement[3];
                charCreateMenu[0] = new TextBox(gd, (gdm.PreferredBackBufferWidth/2)-100,(gdm.PreferredBackBufferHeight/2) - 85,200,50,10,"Name:",skin1,false);
                charCreateMenu[1] = new PlayerBox(gd,(gdm.PreferredBackBufferWidth/2) - 150,(gdm.PreferredBackBufferHeight/2) -85 -10 - 300,300,300,charBox,defaultSkin,false);
                charCreateMenu[2] = new Button(gd, (gdm.PreferredBackBufferWidth / 2) - 50, (gdm.PreferredBackBufferHeight / 2) + (gdm.PreferredBackBufferHeight / 4), 100, 50,"Begin Game!", skin1, false);
        }


        public void Draw(SpriteBatch sp, SpriteFont font, int curUiStage)
        {
            
            if(curUiStage == 0)
            {
                DrawStartMenu (sp,font);
            }
            if(curUiStage == 21)
            {
                DrawOptionsMenu(sp,font);
            }

            if(curUiStage == 11)
            {
                DrawCharCreate(sp,font);
            }
            if(curUiStage == 111)
            {
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
            for (int j = 0; j > optionMenu.Length; j++)
            {
                optionMenu[j].isActive = false;
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
        public void ActiveStartMenu()
        {
            for(int i = 0; i < startMenu.Length; i++)
            {
                startMenu[i].isActive = true;
            }
        }
        public void ActiveOptionsMenu()
        {
            for (int i = 0; i < optionMenu.Length; i++)
            {
                optionMenu[i].isActive = true;
            }
        }
        public void ActiveCharCreate()
        {
            for (int i = 0; i < charCreateMenu.Length; i++)
            {
                charCreateMenu[i].isActive = true;
            }
        }
        public void ClearCharCreate()
        {

            for (int j = 0; j > charCreateMenu.Length; j++)
            {
                charCreateMenu[j].isActive = false;
            }
        }
        public void ClearOptionsMenu()
        {

            for (int j = 0; j > optionMenu.Length; j++)
            {
                optionMenu[j].isActive = false;
            }
        }
        public void ClearStartMenu()
        {

            for (int j = 0; j > startMenu.Length; j++)
            {
                startMenu[j].isActive = false;
            }
        }




        public int UpdateManager(GameTime gameTime,int curUiStage)
        {
            //Start menu update / checks what is clicked
            if(curUiStage == 0)
            {
                if (startMenu[0].isClicked())
                {
                    //start menu
                    Task.Delay(500);

                    ClearStartMenu();
                    ActiveCharCreate();
                    return 11;
                }
                if (startMenu[1].isClicked())
                {
                    //Goes to option menu
                    Task.Delay(500);

                    ClearStartMenu();
                    ActiveOptionsMenu();
                    return 21;
                }
                if (startMenu[2].isClicked())
                {
                    //Exits
                    Task.Delay(500);
                    ClearStartMenu();
                    return 3;
                }
            }


            //checks if text box is clicked
            if(curUiStage == 11)
            {
                if (charCreateMenu[0].isClicked())
                {
                    ((TextBox)charCreateMenu[0]).isTyping = true;
                }
                else if(((TextBox)charCreateMenu[0]).isTyping == true)
                {
                    if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) || (Mouse.GetState().LeftButton == ButtonState.Pressed))
                    {
                        ((TextBox)charCreateMenu[0]).isTyping = false;
                        outputNewPlayer =  ((TextBox)charCreateMenu[0]).typed;
                    }
                    
                    

                        ((TextBox)charCreateMenu[0]).typed = GetKeys(gameTime);
                    
                }
                if (charCreateMenu[2].isClicked())
                {
                    ///111 Will be the start code 
                    ClearCharCreate();
                    return 111;
                }

            }
            
            //options menu updates
            if(curUiStage == 21)
            {
                if (optionMenu[0].isClicked())
                {
                    ClearOptionsMenu();
                    return 22;
                }
            }
            return curUiStage;
            


            //Returns base Value if nothing is doable
            

        }

        
        public string GetKeys(GameTime gameTime)
        {

            if(((TextBox)charCreateMenu[0]).isTyping == true)
            {
                //Gets Typed Character
                if (((TextBox)charCreateMenu[0]).typed.Length < ((TextBox)charCreateMenu[0]).maxLength)
                {
                    if (currentdown != lastdown && currentdown != lastdown.ToLower() && currentdown != lastdown.ToUpper())
                    {
                        words = words + currentdown;

                    }
                }
                //Backspace method
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    if(words.Length > 0 && (gameTime.TotalGameTime.TotalMilliseconds - lastback.TotalGameTime.TotalMilliseconds) > 200.0)
                    {
                        words = words.Substring(0, words.Length - 1);
                        lastback.TotalGameTime = gameTime.TotalGameTime;
                    }
                    
                    
                }
                 

            }
            lastdown = currentdown;
            GetKeyStroke();


            return words;
        }
        
        public string GetKeyStroke()
        {
            //gets lowercase letters
            if (Keyboard.GetState().GetPressedKeyCount() == 1)
            {
                switch (Keyboard.GetState().GetPressedKeys()[0])
                {
                    case Keys.A:
                        currentdown = "a";
                        break;
                    case Keys.B:
                        currentdown = "b";
                        break;
                    case Keys.C:
                        currentdown = "c";
                        break;
                    case Keys.D:
                        currentdown = "d";
                        break;
                    case Keys.E:
                        currentdown = "e";
                        break;
                    case Keys.F:
                        currentdown = "f";
                        break;
                    case Keys.G:
                        currentdown = "g";
                        break;
                    case Keys.H:
                        currentdown = "h";
                        break;
                    case Keys.I:
                        currentdown = "i";
                        break;
                    case Keys.J:
                        currentdown = "j";
                        break;
                    case Keys.K:
                        currentdown = "k";
                        break;
                    case Keys.L:
                        currentdown = "l";
                        break;
                    case Keys.M:
                        currentdown = "m";
                        break;
                    case Keys.N:
                        currentdown = "n";
                        break;
                    case Keys.O:
                        currentdown = "o";
                        break;
                    case Keys.P:
                        currentdown = "p";
                        break;
                    case Keys.Q:
                        currentdown = "q";
                        break;
                    case Keys.R:
                        currentdown = "r";
                        break;
                    case Keys.S:
                        currentdown = "s";
                        break;
                    case Keys.T:
                        currentdown = "t";
                        break;
                    case Keys.U:
                        currentdown = "u";
                        break;
                    case Keys.V:
                        currentdown = "v";
                        break;
                    case Keys.W:
                        currentdown = "w";
                        break;
                    case Keys.X:
                        currentdown = "x";
                        break;
                    case Keys.Y:
                        currentdown = "y";
                        break;
                    case Keys.Z:
                        currentdown = "z";
                        break;
                    
                    //Allowable Characters
                    
                    case Keys.D1:
                        currentdown = "1";
                        break;
                    case Keys.D2:
                        currentdown = "2";
                        break;
                    case Keys.D3:
                        currentdown = "3";
                        break;
                    case Keys.D4:
                        currentdown = "4";
                        break;
                    case Keys.D5:
                        currentdown = "5";
                        break;
                    case Keys.D6:
                        currentdown = "6";
                        break;
                    case Keys.D7:
                        currentdown = "7";
                        break;
                    case Keys.D8:
                        currentdown = "8";
                        break;
                    case Keys.D9:
                        currentdown = "9";
                        break;
                    case Keys.D0:
                        currentdown = "0";
                        break;
                    case Keys.OemMinus:
                        currentdown = "-";
                        break;


                }

            }
            //uppercase letters
            else if (Keyboard.GetState().GetPressedKeyCount() == 2)
            {
                if (Keyboard.GetState().GetPressedKeys()[0] == Keys.LeftShift || Keyboard.GetState().GetPressedKeys()[0] == Keys.RightShift)
                {
                    switch (Keyboard.GetState().GetPressedKeys()[1])
                    {
                        case Keys.A:
                            currentdown = "A";
                            break;
                        case Keys.B:
                            currentdown = "B";
                            break;
                        case Keys.C:
                            currentdown = "C";
                            break;
                        case Keys.D:
                            currentdown = "D";
                            break;
                        case Keys.E:
                            currentdown = "E";
                            break;
                        case Keys.F:
                            currentdown = "F";
                            break;
                        case Keys.G:
                            currentdown = "G";
                            break;
                        case Keys.H:
                            currentdown = "H";
                            break;
                        case Keys.I:
                            currentdown = "I";
                            break;
                        case Keys.J:
                            currentdown = "J";
                            break;
                        case Keys.K:
                            currentdown = "K";
                            break;
                        case Keys.L:
                            currentdown = "L";
                            break;
                        case Keys.M:
                            currentdown = "M";
                            break;
                        case Keys.N:
                            currentdown = "N";
                            break;
                        case Keys.O:
                            currentdown = "O";
                            break;
                        case Keys.P:
                            currentdown = "P";
                            break;
                        case Keys.Q:
                            currentdown = "Q";
                            break;
                        case Keys.R:
                            currentdown = "R";
                            break;
                        case Keys.S:
                            currentdown = "S";
                            break;
                        case Keys.T:
                            currentdown = "T";
                            break;
                        case Keys.U:
                            currentdown = "U";
                            break;
                        case Keys.V:
                            currentdown = "V";
                            break;
                        case Keys.W:
                            currentdown = "W";
                            break;
                        case Keys.X:
                            currentdown = "X";
                            break;
                        case Keys.Y:
                            currentdown = "Y";
                            break;
                        case Keys.Z:
                            currentdown = "Z";
                            break;


                        case Keys.D1:
                            currentdown = "!";
                            break;
                        case Keys.D2:
                            currentdown = "@";
                            break;
                        case Keys.D3:
                            currentdown = "#";
                            break;
                        case Keys.D4:
                            currentdown = "$";
                            break;
                        case Keys.D5:
                            currentdown = "%";
                            break;
                        case Keys.D6:
                            currentdown = "^";
                            break;
                        case Keys.D7:
                            currentdown = "&";
                            break;
                        case Keys.D8:
                            currentdown = "*";
                            break;
                        case Keys.D9:
                            currentdown = "(";
                            break;
                        case Keys.D0:
                            currentdown = ")";
                            break;
                        case Keys.OemMinus:
                            currentdown = "_";
                            break;
                    }
                }
                if (Keyboard.GetState().GetPressedKeys()[1] == Keys.LeftShift || Keyboard.GetState().GetPressedKeys()[1] == Keys.RightShift)
                {
                    switch (Keyboard.GetState().GetPressedKeys()[0])
                    {
                        case Keys.A:
                            currentdown = "A";
                            break;
                        case Keys.B:
                            currentdown = "B";
                            break;
                        case Keys.C:
                            currentdown = "C";
                            break;
                        case Keys.D:
                            currentdown = "D";
                            break;
                        case Keys.E:
                            currentdown = "E";
                            break;
                        case Keys.F:
                            currentdown = "F";
                            break;
                        case Keys.G:
                            currentdown = "G";
                            break;
                        case Keys.H:
                            currentdown = "H";
                            break;
                        case Keys.I:
                            currentdown = "I";
                            break;
                        case Keys.J:
                            currentdown = "J";
                            break;
                        case Keys.K:
                            currentdown = "K";
                            break;
                        case Keys.L:
                            currentdown = "L";
                            break;
                        case Keys.M:
                            currentdown = "M";
                            break;
                        case Keys.N:
                            currentdown = "N";
                            break;
                        case Keys.O:
                            currentdown = "O";
                            break;
                        case Keys.P:
                            currentdown = "P";
                            break;
                        case Keys.Q:
                            currentdown = "Q";
                            break;
                        case Keys.R:
                            currentdown = "R";
                            break;
                        case Keys.S:
                            currentdown = "S";
                            break;
                        case Keys.T:
                            currentdown = "T";
                            break;
                        case Keys.U:
                            currentdown = "U";
                            break;
                        case Keys.V:
                            currentdown = "V";
                            break;
                        case Keys.W:
                            currentdown = "W";
                            break;
                        case Keys.X:
                            currentdown = "X";
                            break;
                        case Keys.Y:
                            currentdown = "Y";
                            break;
                        case Keys.Z:
                            currentdown = "Z";
                            break;



                        case Keys.D1:
                            currentdown = "!";
                            break;
                        case Keys.D2:
                            currentdown = "@";
                            break;
                        case Keys.D3:
                            currentdown = "#";
                            break;
                        case Keys.D4:
                            currentdown = "$";
                            break;
                        case Keys.D5:
                            currentdown = "%";
                            break;
                        case Keys.D6:
                            currentdown = "^";
                            break;
                        case Keys.D7:
                            currentdown = "&";
                            break;
                        case Keys.D8:
                            currentdown = "*";
                            break;
                        case Keys.D9:
                            currentdown = "(";
                            break;
                        case Keys.D0:
                            currentdown = ")";
                            break;
                        case Keys.OemMinus:
                            currentdown = "_";
                            break;
                    }
                }

            }
            //default, add nothing
            else
            {
                currentdown = "";
            }



            
            return currentdown;
        }
        









    }

   
}
