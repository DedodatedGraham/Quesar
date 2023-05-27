using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;
//x,y,z tile posisioning
public class MapTree{

    //Map Properties
    
    public string mapname{get;set;}//self explanitory
    public List<Structure> structures;//structures are Contained Tile Sets of a structure
    public SpriteFont font{get;set;}

    //Container for chunks, which are organized by x & y as z is always rendered
    public Chunk[,] chunkMap{get;set;}
    public List<int[]> chunkGrapple{get;set;}
    public List<Chunk> chunksLoaded{get;set;}
    //Size Properties
    public int sizeX{get;set;}
    public int sizeY{get;set;}
    //Player Chunk
    public Point playerChunk{get;set;}
    public MapTree(int tileX, int tileY, int baseX,int baseY,int maxZ,int minZ,int sx = 1,int sy = 1){
        //Loads in structures or creates empty, depending if loading or creating
        sizeX = sx;
        sizeY = sy;
        chunkMap = new Chunk[sizeX,sizeY];//Creates Chunkmap
        chunksLoaded = new List<Chunk>();
        for(int i = 0; i < sizeX; i++){
            for(int j = 0; j < sizeY; j++){
                chunkMap[i,j] = new Chunk(0,tileX,tileY,baseX,baseY,maxZ,minZ,new Point(i,j));
                chunksLoaded.Add(chunkMap[i,j]);
            }
        }
    }
    public void Initialize(SpriteFont fon,GraphicsDeviceManager gdm){
        font = fon;
        for(int i = 0; i < sizeX; i++){
            for(int j = 0; j < sizeY; j++){
                chunkMap[i,j].Initialize(font,gdm);
            }
        }
    }

    public void LoadContent(ContentManager content,GraphicsDeviceManager gdm){

    }

    public void Update(ref UpdatePackage up){
        foreach(Chunk chunk in chunksLoaded){
            chunk.Update(ref up);
        }
    }

    public void Draw(SpriteBatch sb){
        foreach(Chunk chunk in chunksLoaded){
            chunk.Draw(sb,0);
        }
    }
}