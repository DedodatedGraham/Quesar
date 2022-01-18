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
        //newer more specific definition of this:
        //so basically dictionary is like a search interface of data, so when we want objects/textures/hitboxes, we can use the dictionary to pull usable objects and modify them
        //data interacts with the dictionary to pass up constructed objects,(theoretically should only have to code object types in Data)
        //data uses the ID to first make a key of where different kinds of things are stored in the files
        //then it will be able to give data any sort of file which data can read and turn into something
        //ID will also be able to write new data, and modify existing data

        //if this can work well gameengine will be incredibly op
        //basically it can save and load anything super easy 


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
            data = new Data(c,gp); 
            data.makeLayers();
        }

        
        


        //the data class is the beefy boy that starts up and makes me everything i want
        private class Data
        {
            private ID ID;
            
           
            public Data(ContentManager c,string gp)
            {
                ID = new ID(gp);
                Create(c);



            }

            public void Create(ContentManager c)
            {
                //this is a hard code thing, create initializes what types of eveyrthing its going to need
                //aka creat will hold the absoulte database on objects




            }

            //this can pass an initalizing objects' saved texture path and load it into game
            //basically stored string -> generated texture super easily
            public Texture2D loadTexture(ContentManager c,string path)
            {
                //the string it is given is the type & 
                return c.Load<Texture2D>(path);
            }

            public Map loadMap()
            {

            }

            

            public void makeLayers()
            {
                
            }

        }

        //only 1 of these should exist across game ever
        private class ID
        {

            public List<int> idType;
            public List<string> layers;
            private List<int> layercounts;
            private List<int> depth;

            public int layercount;
            public int size;
            public string layerName;

            public string globalPath;

            private List<ID> ids;
            
            
            //id is the key system that sends data the files to save & load based on 
            public ID(string gp)
            {
                idType = new List<int>();
                layers = new List<string>();
                layercounts = new List<int>();
                ids = new List<ID>();
                globalPath = gp;
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

                condense();

                //so when the id gets created, it recursively is able to find everything that exists, gets the names, and then 
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
            //so now this should create a key using lists, one is more just a quick numerical refrence if needed

            public void condense()
            {
                //this condense's task it to take everything below it and put it all into a key
                List<int> tempint = new List<int>();
                List<string> temp = new List<string>();
                List<int> templayer = new List<int>();
                List<int> tempdepth = new List<int>();


                //first element will always be top layer
                //this then sets temp concated with the output of the recursive & sets the layer counts aswell
                temp = condenseR(out templayer, out tempdepth);
                
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

            public List<string> condenseR(out List<int> layers,out List<int> depth)
            {
                //this will now recurssively pass up things
                List<string> outstring = new List<string>();
                List<int> layercts = new List<int>();
                List<int> tempdep = new List<int>();
                outstring.Add(layerName);
                layercts.Add(layercount);
                tempdep.Add(size);
                if(layercount != 0)
                {
                   
                    for(int i = 0; i < layercount; i++)
                    {
                        //this should add in a recursive element and keep everything in order
                        List<int> supertemp = new List<int>();
                        List<int> superdepth = new List<int>();
                        outstring.Concat(ids[i].condenseR(out supertemp, out superdepth));
                        layercts.Concat(supertemp);
                        tempdep.Concat(superdepth);
                    }

                }


                //nulls node and returns important values, removing heftyness from the program
                ids = null;
                size = 0;
                layercount = 0;
                layers = layercts;
                depth = tempdep;
                return outstring;


            }

            //i want 2 functions, one will be given a id of a folder and a file name, one will be of a id of a folder.
            //essentially when ran by Data, it will be able to pull entire folders of files, or just one depending on what is needed by the system
            public FileInfo getFile(string path, string fileName)
            {
                string tempFinder = globalPath;
                //defines the path to take
                List<int> numPath = getNumPath(getNum(path));
                for(int i = 0; i < numPath.Count; i++)
                {
                    tempFinder = tempFinder + @"\" + layers[numPath[i]];
                }
                tempFinder = tempFinder + fileName;

                FileInfo fileInfo = new FileInfo(tempFinder);
                if (fileInfo.Exists)
                {
                    return fileInfo;
                }
                else
                {
                    throw new AggregateException("couldnt find file");
                }

            }
            //getFiles should be able to just run a loop through the count of files & use getFile a few times and put it together
            public List<FileInfo> getFiles(string path)
            {
                List<FileInfo> fileInfo = new List<FileInfo>();
                string tempFinder = globalPath;
                List<int> numPath = getNumPath(getNum(path));
                for (int i = 0; i < numPath.Count; i++)
                {
                    tempFinder = tempFinder + @"\" + layers[numPath[i]];
                }
                foreach (string newpath in Directory.GetFiles(tempFinder))
                {
                    FileInfo tempinfo = new FileInfo(newpath);
                    if (tempinfo.Exists)
                    {
                        fileInfo.Add(tempinfo);
                    }
                }
                return fileInfo;
            }
            
            //need to add way to create a new file and folder

           
           

            //get number path will return a list of ints that are the positions in order to reach a folder
            private List<int> getNumPath(int x)
            {
                //here x represents the location of the string according to the key
                //what this will do is send out a list of ints that are the path to the file being looked at
                List<int> a = new List<int>();
                if(x != 0)
                {
                    a.Add(0);
                    a.Add(x);
                    //in the layer list it is set up like an n-array 
                    //so we must be able to read it
                    int index = x;
                    //maybe use the layer count reversly, it can make guesses and check what it will send out

                    while(index != 0)
                    {
                        int temp = depth[index];
                        for(int i = index; i > 0; i--)
                        {
                            if(depth[i] == temp - 1)
                            {
                                //so first it will scan if an object is one layer up, this signifies that the layer is a parent because the way the key sets up
                                a.Insert(i, 1);
                                index = i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    a.Add(0);
                }
                
                
                




                return a;
            }
            private int getNum(string path)
            {
                int ret = 0;
                for (int i = 0; i < layers.Count ;i++)
                {
                    if( path == layers[i])
                    {
                        ret = i;
                        break;
                    }

                }

                 return ret;
            }
                
        }



    }
    

    //data is going to be the actual 

}
