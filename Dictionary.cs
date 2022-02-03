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
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

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
            data = new Data(c, gp);

        }
        //dictionary takes in push's and requests and makes data do them.



        //when we want a fully loaded map to the dictionary 
        public Map getMap(string name)
        {
            return data.getMap(name);

        }
        public void saveMap(Map map, string name, bool overwrite)
        {
            data.saveMap(map, name, overwrite);
        }


        
        


        //the data class is the beefy boy that starts up and makes me everything i want
        private class Data
        {
            private ID ID;
            private List<string> Key;
            private List<string> KeyPath;

            private List<int> typeIndex;


            private List<Map> loadedMaps;

            private string globalPath;


            //now here is our data which is the objects loaded in them selves//

            //now we need to store all 
            public Data(ContentManager c,string gp)
            {
                ID = new ID(gp);
                globalPath = gp;
                //the idea is key will be made according to the id's tags and pulls all according file names
                MakeKey();

                //then it will produce all objects
                Create(c);
            


            }
            private void MakeKey()
            {
                //this decides if it will load the entire key or just the most current and applicapble
                //can apply this when making actual game
                bool sort = false;

                Key = new List<string>();
                KeyPath = new List<string>();
                for(int i = 0; i < ID.layers.Count; i++)
                {
                    //this first loop will take us through each layer of ID, each with objects to load in which case a key is needed
                    int tempcount = ID.getFileCount(ID.layers[i]);
                    if (tempcount != 0)
                    {
                        //if it has files then it will load them into a Key
                        for (int j = 0; j < tempcount; j++)
                        {
                            //we then have all loaded objects
                            Key.Add(ID.getFileName(j, ID.layers[i]));
                            KeyPath.Add(ID.layers[i]);
                        }
                    }
                    
                }

                //if everything doesnt need to be loaded, the key will also pull then file dates and load the newest of each occurance 
                //will be implemented later, but saves memory and processes when we cut out the un needed stuff early on
                // Remeber "#" will be used at the end of files to denote newness, aka if its sorting, 
                if (sort)
                {

                }
            }

            private void Create(ContentManager c)
            {
                //initialize all of our lists that contain all the objects that can be sent/accesed by the dictionary
                loadedMaps = new List<Map>();


                //we also want to get the counts of each kind of object in the ketpath so we can load them relatively, we can use an index num list for easy introduction to loops.


                typeIndex = new List<int>();
                string relative = "";
                int indx = 0;
                //should now give us an ordered top down knowledge of how many objects of each type need to be loaded
                for(int i = 0; i < KeyPath.Count; i++)
                {
                    if(relative != KeyPath[i])
                    {
                        //if the cycle is starting 
                        if(typeIndex.Count == 0)
                        {
                            relative = KeyPath[i];
                            indx++;
                        }
                        //if its started so needs to reset
                        else
                        {
                            typeIndex.Add(indx);
                            indx = 1;
                            relative = KeyPath[i];
                        }




                    }
                    else
                    {
                        indx++;
                    }
                }
                
                if(KeyPath.Count != 0 && typeIndex.Count == 0)
                {
                    typeIndex.Add(KeyPath.Count);
                }

                ;



                //we now use the key to make the obecjts and errything
                //type index's size lets us cycle through each type of element at a time

                //indx we can use again
                int index = 0;
                for (int i = 0; i < typeIndex.Count; i++)
                {
                    for(int j = 0; j < typeIndex[i]; j++)
                    {
                        switch (KeyPath[i])
                        {
                            //when tagged as map 
                            case "Map":
                                loadedMaps.Add(loadMap(index + j));
                                //this will grab the texture sheet for enviorment objects that is there.
                                //skip texture loading for now while i work on actual maps
                                //loadedMaps[index + j].setSprite(loadTexture(c,loadedMaps[index+j].sheetName));
                                break;
                            

                        }
                    }
                    //this keeps index at an absoulte startof each 
                    index += typeIndex[i];
                }




            }

            //this can pass an initalizing objects' saved texture path and load it into game
            //basically stored string -> generated texture super easily
            private Texture2D loadTexture(ContentManager c,string path)
            {
                //anything with textures can use this 
                return c.Load<Texture2D>(path);
            }

            private Map loadMap(int x)
            {
                string location = ID.getPath(ID.getNum("Map"));
                location += @"\" + Key[getAbsolute("Map") + x];

                Map ret = new Map();
                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()); 

               







                return ret;
            }


            //heres the interaction stuff, basically the dictionary is used to get objects, so this is the public function which can be interacted with by command through dictionary, but everything else is private D:
            public Map getMap(string name)
            {
                for(int i = 0; i < loadedMaps.Count; i++)
                {
                    if(name == loadedMaps[i].worldName)
                    {
                        return loadedMaps[i];
                    }
                }

                return null;

            }
            
            //the save map name should be the most recent with updated #'s, the only time save map is used here 
            public void saveMap(Map data, string name,bool overwrite)
            {
                string location = ID.getPath(ID.getNum("Map"));
                
                //true here will set the map to overwrite one with the same save name,
                if (overwrite)
                {
                    

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    XmlWriter writer = XmlWriter.Create(location, settings);


                    writer.WriteStartDocument();
                    writer.WriteStartElement(data.saveName);

                    //gives a date stamp on file of last saved
                    writer.WriteStartElement("Date");
                    writer.WriteValue(DateTime.Now.ToString());
                    writer.WriteEndElement();
                    //gives saved location name to get sprite sheets
                    writer.WriteStartElement("EnviornmentSheetName");
                    writer.WriteValue(data.enviormentSheetLocation);
                    writer.WriteEndElement();

                    //boundary data all maps must have one
                    writer.WriteStartElement("Boundary");
                    writer.WriteStartElement("X");
                    writer.WriteValue(data.boundary.X);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Y");
                    writer.WriteValue(data.boundary.Y);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Width");
                    writer.WriteValue(data.boundary.Width);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Height");
                    writer.WriteValue(data.boundary.Height);
                    writer.WriteEndElement();
                    writer.WriteEndElement();


                    //first we want to go through the quadtree of points and write each's id & position
                    int qtcount = data.worldObjects.getCount();
                    writer.WriteStartElement(data.worldObjects.ToString());
                    if (qtcount > 0)
                    {

                        List<MyPoint> allPoints = data.worldObjects.gatherAll();

                        for (int i = 0; i < qtcount; i++)
                        {
                            writer.WriteStartElement("MyPoint");

                            writer.WriteStartElement("X");
                            writer.WriteValue(allPoints[i].X);
                            writer.WriteEndElement();

                            writer.WriteStartElement("Y");
                            writer.WriteValue(allPoints[i].Y);
                            writer.WriteEndElement();

                            writer.WriteStartElement("ID");
                            writer.WriteValue(allPoints[i].id);
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                        }

                    }
                    //next we will go through each trace that exists
                    if (!(data.worldObjects.traces is null))
                    {
                        int traceCount = data.worldObjects.traces.Count;
                        if (traceCount > 0)
                        {
                            for (int i = 0; i < traceCount; i++)
                            {
                                writer.WriteStartElement("Trace");
                                List<string> ids = data.worldObjects.traces[i].getIds();
                                for (int j = 0; j < ids.Count; j++)
                                {
                                    writer.WriteStartElement("Id");
                                    writer.WriteValue(ids[j]);
                                    writer.WriteEndElement();
                                }

                                writer.WriteEndElement();
                            }
                        }
                    }
                    writer.WriteEndElement();

                    //now the map has saved all of its quadtree needed, will be able to pull out and make the quadtree from x,y, and id points after awhile
                    //next we will record all the mapElements in each map by type and id, for refrence, might need to track more later
                    if (!(data.mapElements is null))
                    {
                        int objCount = data.mapElements.Count;
                        if (objCount > 0)
                        {
                            writer.WriteStartElement("MapElement");
                            for (int q = 0; q < objCount; q++)
                            {
                                writer.WriteStartElement("Id");
                                writer.WriteValue(data.mapElements[q].id);
                                writer.WriteEndElement();

                                writer.WriteStartElement("Type");
                                writer.WriteValue(data.mapElements[q].type);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                    }




                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }
                //if its false then it will do the same saving sequence, but add a #
                //havent written all of the logic to go though, so this is under the intention that a map being saved more than once is already being loaded and therefore will retain mark of newness "#"
                else
                {
                    if (File.Exists(location))
                    {
                        location += "#";
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    XmlWriter writer = XmlWriter.Create(location, settings);


                    writer.WriteStartDocument();
                    writer.WriteStartElement(data.saveName);

                    //gives a date stamp on file of last saved
                    writer.WriteStartElement("Date");
                    writer.WriteValue(DateTime.Now.ToString());
                    writer.WriteEndElement();
                    //gives saved location name to get sprite sheets
                    writer.WriteStartElement("EnviornmentSheetName");
                    writer.WriteValue(data.enviormentSheetLocation);
                    writer.WriteEndElement();

                    //boundary data all maps must have one
                    writer.WriteStartElement("Boundary");
                    writer.WriteStartElement("X");
                    writer.WriteValue(data.boundary.X);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Y");
                    writer.WriteValue(data.boundary.Y);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Width");
                    writer.WriteValue(data.boundary.Width);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Height");
                    writer.WriteValue(data.boundary.Height);
                    writer.WriteEndElement();
                    writer.WriteEndElement();


                    //first we want to go through the quadtree of points and write each's id & position
                    int qtcount = data.worldObjects.getCount();
                    writer.WriteStartElement(data.worldObjects.ToString());
                    if (qtcount > 0)
                    {

                        List<MyPoint> allPoints = data.worldObjects.gatherAll();

                        for (int i = 0; i < qtcount; i++)
                        {
                            writer.WriteStartElement("MyPoint");

                            writer.WriteStartElement("X");
                            writer.WriteValue(allPoints[i].X);
                            writer.WriteEndElement();

                            writer.WriteStartElement("Y");
                            writer.WriteValue(allPoints[i].Y);
                            writer.WriteEndElement();

                            writer.WriteStartElement("ID");
                            writer.WriteValue(allPoints[i].id);
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                        }

                    }
                    //next we will go through each trace that exists
                    if (!(data.worldObjects.traces is null))
                    {
                        int traceCount = data.worldObjects.traces.Count;
                        if (traceCount > 0)
                        {
                            for (int i = 0; i < traceCount; i++)
                            {
                                writer.WriteStartElement("Trace");
                                List<string> ids = data.worldObjects.traces[i].getIds();
                                for (int j = 0; j < ids.Count; j++)
                                {
                                    writer.WriteStartElement("Id");
                                    writer.WriteValue(ids[j]);
                                    writer.WriteEndElement();
                                }

                                writer.WriteEndElement();
                            }
                        }
                    }
                    writer.WriteEndElement();

                    //now the map has saved all of its quadtree needed, will be able to pull out and make the quadtree from x,y, and id points after awhile
                    //next we will record all the mapElements in each map by type and id, for refrence, might need to track more later
                    if (!(data.mapElements is null))
                    {
                        int objCount = data.mapElements.Count;
                        if (objCount > 0)
                        {
                            writer.WriteStartElement("MapElement");
                            for (int q = 0; q < objCount; q++)
                            {
                                writer.WriteStartElement("Id");
                                writer.WriteValue(data.mapElements[q].id);
                                writer.WriteEndElement();

                                writer.WriteStartElement("Type");
                                writer.WriteValue(data.mapElements[q].type);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                    }




                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }


            }

            public int getAbsolute(string path)
            {
                int a = 0;
                for(int i = 0; i < typeIndex.Count; i++)
                {
                    if(KeyPath[a + i] == path)
                    {
                        a += i;
                        break;
                    }
                    else
                    {
                        a = typeIndex[i];
                    }
                }


                return a;
            }
        }

        //only 1 of these should exist across game ever
        private class ID
        {

            
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
            private ID(int s,string path)
            {
                ids = new List<ID>();
                size = s + 1;
                layerName = Path.GetFileNameWithoutExtension(path);
                //if the folder isnt completely empty it wont get skipped when making 
                if (Directory.GetFiles(path).Length != 0 || Directory.GetDirectories(path).Length != 0)
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

            private void condense()
            {
                //this condense's task it to take everything below it and put it all into a key
                List<string> temp = new List<string>();
                List<int> templayer = new List<int>();
                List<int> tempdepth = new List<int>();


                //first element will always be top layer
                //this then sets temp concated with the output of the recursive & sets the layer counts aswell
                temp = condenseR(out templayer, out tempdepth);
                
                //then makes other half of index
                
                //now assigns temps
                layers = temp;
                layercounts = templayer;
                depth = tempdepth;

                //finally nulls out id's to make them not exist as everything needed exists at the top now
                ids = null;
                size = 0;
                layercount = 0;


            }

            private List<string> condenseR(out List<int> layers,out List<int> depth)
            {
                //this will now recurssively pass up things
                List<string> outstring = new List<string>();
                List<int> layercts = new List<int>();
                List<int> tempdep = new List<int>();
                outstring.Add(layerName);
                layercts.Add(layercount);
                tempdep.Add(size);
                
                
                for(int i = 0; i < ids.Count; i++)
                {
                    //this should add in a recursive element and keep everything in order
                    List<int> supertemp = new List<int>();
                    List<int> superdepth = new List<int>();
                    List<string> superstring = new List<string>();
                    superstring = ids[i].condenseR(out supertemp, out superdepth);
                    for(int j = 0; j < superstring.Count; j++)
                    {
                        outstring.Add(superstring[j]);
                        layercts.Add(supertemp[j]);
                        tempdep.Add(superdepth[j]);
                    }

                    //debugging key condensing
                    //for (int j = 0; j < superstring.Count; j++)
                    //{
                    //    Debug.WriteLine(supertemp[j]);
                    //    Debug.WriteLine(superdepth[j]);
                    //    Debug.WriteLine(outstring[j]);
                    //}

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

            //file name is the actual save name of it, path is the simpler object refrence
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
            public string getFileName(int i,string path)
            {
                string tempFinder = globalPath;
                List<int> numPath = getNumPath(getNum(path));
                for (int j = 0; j < numPath.Count; j++)
                {
                    if(j != 0)
                    {
                        tempFinder = tempFinder + @"\" + layers[numPath[j]];
                    }
                }
                string ret = Directory.GetFiles(tempFinder)[i];
                ret = Path.GetFileNameWithoutExtension(ret);
                return ret;

            }
           
            public int getFileCount(string path)
            {

                string tempFinder = globalPath;
                List<int> numPath = getNumPath(getNum(path));

                if(path == "MapElement")
                {
                    ;
                }
                for (int j = 1; j < numPath.Count; j++)
                {

                    tempFinder = tempFinder + @"\" + layers[numPath[j]];


                        
                    
                }
                
                return Directory.GetFiles(tempFinder).Length;

            }

            public string getFilePath(int i,string path)
            {
                string tempFinder = globalPath;
                List<int> numPath = getNumPath(getNum(path));
                for (int j = 0; j < numPath.Count; j++)
                {
                    tempFinder = tempFinder + @"\" + layers[numPath[j]];
                }
                tempFinder = tempFinder + @"\" + getFileName(i, path);
                return tempFinder;

            }


            //get number path will return a list of ints that are the positions in order to reach a folder
            public string getPath(int x)
            {
                string ret = globalPath;
                if (x != 0)
                {
                    if (depth[x] == 1)
                    {
                        ret = ret + @"\" + layers[x];
                    }
                    else
                    {
                        //so here we want to go up the list starting from x and basically we will go down to each closest -1 level of depth
                        int index = x;
                        //index tracks the position of each time we need to step back
                        while (depth[index] >= 1)
                        {
                            for (int i = index; i > 0; i--)
                            {
                                if (depth[i] == depth[index] - 1)
                                {
                                    index = i;
                                    ret = ret.Insert(globalPath.Length, @"\" + layers[i]);
                                    break;
                                }

                            }
                        }

                    }

                }
                return ret;
            }
            private List<int> getNumPath(int x)
            {
                //here x represents the location of the string according to the key
                //what this will do is send out a list of ints that are the path to the file being looked at
                List<int> a = new List<int>();
                if (x != 0)
                {
                    if(depth[x] == 1)
                    {

                        a.Add(0);
                        a.Add(x);
                    }
                    else
                    {

                        a.Add(0);
                        //so here we want to go up the list starting from x and basically we will go down to each closest -1 level of depth
                        int index = x;
                        //index tracks the position of each time we need to step back
                        a.Add(x);
                            for(int i = index; depth[index] > 1; )
                            {
                                if(depth[i] == depth[index] - 1)
                                {
                                    index = i;
                                    a.Insert(1,index);
                                    i--;
                                }
                                else
                                {
                                    i--;
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
            public int getNum(string path)
            {
                int ret = 0;
                for (int i = 0; i < layers.Count ;i++)
                {
                    if( path.Equals(layers[i]))
                    {
                        ret = i;
                        break;
                    }

                }

                 return ret;
            }
                
        }

        

    }
    


}
