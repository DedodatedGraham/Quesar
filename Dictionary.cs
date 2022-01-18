using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
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
        public Dictionary(ContentManager c,string gp)
        {
            globalPath = gp;
            data = new Data(gp); 
            data.makeLayers();
        }

        
        


        //this is the section with the actual data
        private class Data
        {
            private ID ID;
            public Data(string gp)
            {
                ID = new ID(gp);




            }

            public void makeLayers()
            {
                
            }

        }

        private class ID
        {

            public List<int> idType;
            public List<string> layers;
            private List<int> layercounts;

            public int layercount;
            public int size;
            public string layerName;

            private List<ID> ids;
            
            
            //so this is the first function which only needs to filter through the files and assign a key to them
            //data is what has the ability to read using the id paths
            //however id will be used to pull those files and send them to and from the directory, so only id messes
            //with directory
            public ID(string gp)
            {
                idType = new List<int>();
                layers = new List<string>();
                layercounts = new List<int>();
                ids = new List<ID>();
                if (Directory.Exists(gp))
                {
                    if (Directory.GetDirectories(gp).Length != 0)
                    {
                        layercount = Directory.GetDirectories(gp).Length;
                        layerName = Path.GetFileNameWithoutExtension(gp);
                        foreach (string path in Directory.GetDirectories(gp))
                        {
                            if (Directory.Exists(path))
                            {
                                ids.Add(new ID(size, path));
                            }

                        }
                    }
                    else
                    {
                        throw new ArgumentException("no more depth, only files/nothing in first layer");
                    }
                }
                else
                {
                    throw new ArgumentException("directory does not exist");
                }
            }    
            //recursive function into its self, dives 1 step deeper into the folders each time, 
            //
            public ID(int s,string path)
            {
                size = s + 1;
                layerName = Path.GetFileNameWithoutExtension(path);
                if (Directory.GetDirectories(path).Length != 0)
                {
                    layercount = Directory.GetDirectories(path).Length;
                    foreach (string newpath in Directory.GetDirectories(path))
                    {
                        if (Directory.Exists(newpath))
                        {
                            ids.Add(new ID(size, newpath));
                        }

                    }

                }
                else
                {
                    //this will set layer count to 0 letting us know its the end of a tree in the compression
                    layercount = 0;
                }
                

            }

            public void condense()
            {
                //this condense's task it to take everything below it and put it all into a key
                List<int> tempint = new List<int>();
                List<string> temp = new List<string>();
                List<int> templayer = new List<int>();

                

                //first element will always be top layer
                //this then sets temp concated with the output of the recursive & sets the layer counts aswell
                temp = condenseR(out templayer);
                
                //then makes other half of index
                for(int i = 0; i < temp.Count; i ++)
                {
                    tempint.Add(i);
                }
                //now assigns temps
                idType = tempint;
                layers = temp;
                layercounts = templayer;

                //finally nulls out id's to make them not exist as everything needed exists at the top now
                ids = null;
                size = 0;
                layercount = 0;


            }

            public List<string> condenseR(out List<int> layers)
            {
                
                //this will now recurssively pass up things
                if (ids.Count > 0)
                {
                    //initialises
                    List<string> tempstring = new List<string>();
                    List<int> templayer = new List<int>();
                    tempstring.Add(layerName);
                    templayer.Add(layercount);
                    for(int i = 0; i < ids.Count; i++)
                    {
                        //goes through and returns everything needed
                        tempstring.Concat(ids[i].condenseR(out templayer));
                    }
                    layers = templayer;

                    return tempstring;
                }
                else
                {
                    layers = new List<int>();
                    return new List<string>();
                }


            }
                
                
                
                
                
                
                
                
                
                
        }



    }
    

    //data is going to be the actual 

}
