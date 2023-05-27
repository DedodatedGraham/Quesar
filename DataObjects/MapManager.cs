using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;
//Class which pieces together map locations and tree, animations, and can save/load in a map given a path
//We can Also give this rendering Logic and it will use the tree to only load what is needed


public class MapManager{

    //Map Constants
    //Tile
    public const int tileX = 64;//Pixel Witdth of tile in x
    public const int tileY = 64;//Pixel Width of tile in y

    //Chunk Size(Used for rendering)(32x32 tile collections rangin 0-100 high)
    public const int baseX = 32;
    public const int baseY = 32;
    public const int maxZ = 200;//Highest Z
    public const int minZ = 0;//Lowest Z

    //Map Elements
    public TerrainGen terrainGenerator;
    public MapTree loadedMap;//This is the contained map data, alittle bit seperate from structures
    public SpriteFont font;

    //Main Methods
    public MapManager(){
        loadedMap = new MapTree(tileX,tileY,baseX,baseY,maxZ,minZ,6,6);
        loadedMap.mapname = "First Map";
        //terrainGenerator = new TerrainGenerator();
        //terrainGenerator.Generate(loadedMap);
    }

    public void GenerateNow(){
        terrainGenerator = new TerrainGen();
        terrainGenerator.Generate(loadedMap);
    }
    public void Initialize(SpriteFont fon,GraphicsDeviceManager gdm){
        loadedMap.Initialize(fon,gdm);
    }
    public void LoadContent(ContentManager content,GraphicsDeviceManager gdm){

    }
    public void Update(ref UpdatePackage up){
        loadedMap.Update(ref up);
    }
    public void Draw(SpriteBatch sb){
        loadedMap.Draw(sb);
    }
    public bool SaveMap(){
        //Save map and return true if good

        return false;
    }
    public bool LoadMap(){
        //Load map and return true if good
        return false;
    }
}
