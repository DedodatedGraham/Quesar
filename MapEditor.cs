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
using System.Xml.Serialization;

namespace Quesar
{
    public class MapEditor
    {
        //kinda lazy rn and dont wana recode for events, so waiting is 1 of 3 numbers(0 = do nothing/no need, 1 = has been clicked and now waiting for trigger, 2 = done being typed)
        //guess this will work?
        public int isWaiting1 { get; set; }
        public int wtm { get; set; }


        public Map currentMap { get; set; }

        public UiElement[] tools { get; set; }
        public TextBox box { get; set; }
        public string location { get; set; }
        public string saveName { get; set; }
        public string loadName { get; set; }

        public string currentdown;
        public string lastdown;
        public string words;
        public GameTime lastback;

        //so the map editor will have a way to set all quadtrees to each individual assets such as hitbox and stuff liek that
        //as well as it will have a way in which the map can be designed efficiently 
        public MapEditor(GraphicsDevice gd, Texture2D btn1, Texture2D btn2, GraphicsDeviceManager gdm)
        {
            //needed for typing
            currentdown = "";
            lastdown = "";
            words = "";
            lastback = new GameTime();
            box = new TextBox(gd, gdm.PreferredBackBufferWidth / 2 - btn1.Width / 2, gdm.PreferredBackBufferHeight / 2 - btn1.Height / 2, btn1.Width, btn1.Height, 20, "FileName", btn1, false);
           

            isWaiting1 = 0;

            //
            location = @"C:\Users\graha\source\repos\Quesar\Data\";
            saveName = "";
            loadName = "";

            //UiElements for the actual Base tools for the map editor
            tools = new UiElement[2];
            tools[0] = new Button(gd, 0, gdm.PreferredBackBufferHeight - btn1.Height, btn1.Width, btn1.Height, "Load Map", btn1, false);
            tools[1] = new Button(gd, btn1.Width, gdm.PreferredBackBufferHeight - btn1.Height, btn1.Width, btn1.Height, "Save Map", btn1, false);

            
            
        }

        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            //draws the ui for the level editor
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i].isActive)
                {
                    tools[i].Draw(sb, sf);
                }
            }
            if (box.isActive)
            {
                box.Draw(sb, sf);
            }

        }

        public void ToolsOn()
        {
            for (int i = 0; i < tools.Length; i++)
            {
                tools[i].isActive = true;
            }

        }
        public void ToolsOff()
        {
            for (int i = 0; i < tools.Length; i++)
            {
                tools[i].isActive = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            //Save dialog box checker/ only pops up if no // typing logic starter
            if (box.isClicked() && box.isActive)
            {
                box.isTyping = true;
            }
            else if (box.isTyping == true)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) || (Mouse.GetState().LeftButton == ButtonState.Pressed))
                {
                    box.isTyping = false;
                    box.isActive = false;
                    if (wtm == 2)
                    {
                        saveName = box.typed;
                        isWaiting1 = 2;
                    }
                    if (wtm == 1)
                    {
                        loadName = box.typed;
                        isWaiting1 = 1;
                    }
                }
                    box.typed = GetKeys(gameTime);

            }
             

            //SaveMapButton
            if (tools[1].isClicked())
            {
                //is waiting value 1 is when active but save isnt ready yet
                if(currentMap.hasSave == false)
                {
                    box.isActive = true;
                    isWaiting1 = 0;
                    wtm = 2;
                }
                else
                {
                    isWaiting1 = 2;
                }
            }
            //LoadMapButton
            if (tools[0].isClicked())
            {
                box.isActive = true;
                wtm = 1;
            }

            //if already has name then it will pass straight through
            if (isWaiting1 == 2)
            {
                SaveMap();
                isWaiting1 = 0;
                wtm = 0;
            }
            if(isWaiting1 == 1)
            {
                LoadMap();
                isWaiting1 = 0;
                wtm = 0;
            }

        }


        //should be sorta working save and load functions now using save,load,encode,and decode
        public void LoadMap()
        {
            currentMap = Decoder<Map>(loadName);
            loadName = "";
        }

        public void SaveMap()
        {
            //need to work on this logic when back
            if (currentMap.saveName == "" && location == @"C:\Users\graha\source\repos\Quesar\Data\")
            {
                currentMap.saveName = saveName;
                location = location + saveName + ".txt";
                currentMap.saveLocation = location;
            }

            Encoder(currentMap);

            saveName = "";
        }


        
        
        public void Encoder<T>(T data)
        {
            XmlSerializer serializer =new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(location);
            serializer.Serialize(writer,data);
            writer.Close();
        }
        public T Decoder<T>(string input)
        where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
                
        }



        public string GetKeys(GameTime gameTime)
        {

            if (box.isTyping == true)
            {
                //Gets Typed Character
                if (box.typed.Length < box.maxLength)
                {
                    if (currentdown != lastdown && currentdown != lastdown.ToLower() && currentdown != lastdown.ToUpper())
                    {
                        words = words + currentdown;

                    }
                }
                //Backspace method
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    if (words.Length > 0 && (gameTime.TotalGameTime.TotalMilliseconds - lastback.TotalGameTime.TotalMilliseconds) > 200.0)
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
