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
    public class Dictionary
    {
        //dictionary is essentially a look up for everything?
        //so my initial idea is it will have everything saved here, 
        //it will store lists of quad trees along with dates & times of the time it was saved,
        //this will be what is initially loaded into the game, it will use a data file where all the games contents are stored/ there will be 2 settings, one is if the dictionary is in play mode
        //if its in playmode then it will only load the latest data structures,
        //however in mapeditor mode it will load the latest, but its able to interschange 
        //so it can refrence modify lookup save and load pretty much any type of anything i need, then have saves essentially incase anything ever needs to be reverted, and can also be used for a 
        //object menu in the editor where things about the object can be changed, and saved aswell as ordered well
        //game implementation will use this to find the current map, and get objects from said map that is loaded
        //this would be a good place to implement [tags] for object properties then game engine can be passed some information that isnt relevent at the creation or until some event has happened


        private string globalPath;

        private Data data;

        //this is all we theoretically should need to pass it to load in textures as everything else will have a file path stored already
        //we will also send it a string of the file orgin, this will be based on save location
        public Dictionary(ContentManager c,string gp )
        {
            globalPath = gp;
            data = new Data();
            makeLayers();
        }

        private void makeLayers()
        {

        }
        


        //this is the section with the actual data
        private class Data
        {
            public Data()
            {

            }
        }
    }


    //data is going to be the actual 
    
}
