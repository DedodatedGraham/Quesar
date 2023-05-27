using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Quesar;


public class Chunk : Mappable{

    public override Point renderTarget{get;set;}
    //Grid of tiles
    public Tile[,,] tiles{get;set;}
    public int baseX{get;set;}
    public int baseY{get;set;}
    public int baseZ{get;set;}
    public int minZ{get;set;}
    public int maxZ{get;set;}
    public Point chunkIndex{get;set;}
    //other
    public SpriteFont font{get;set;}
    public Chunk(int mode ,int tileX, int tileY, int baseX,int baseY,int maxZ,int minZ,Point chunki){
        chunkIndex = chunki;
        switch(mode){
            //mode 0 => create tiles now
            //mode 1 => load tiles now
            case 0:
                //Make all Tiles, makes a 'level' surface(named drawing for now)
                int RealbaseZ = (maxZ - minZ);
                this.baseZ = RealbaseZ;
                int tileZ = tileX;
                this.baseX = baseX;
                this.baseY = baseY;
                this.maxZ = maxZ;
                this.minZ = minZ;
                int orgx = chunki.X*baseX*tileX;
                int orgy = chunki.Y*baseY*tileY;
                tiles = new Tile[baseX,baseY,baseZ]; 
                for(int i = 0; i < baseX; i++){
                    for(int j = 0; j < baseY; j++){
                        for(int k = 0; k < baseZ; k++){
                            tiles[i,j,k] = new Tile(mode,tileX,tileY,tileZ,new int[]{i,j,k},orgx,orgy);
                        }
                    }
                }
                break;  
            case 1:
                break;  
        }

    }
    public void Initialize(SpriteFont fon,GraphicsDeviceManager gdm){
        font = fon;
        for(int i = 0; i < baseX; i++){
            for(int j = 0; j < baseY; j++){
                for(int k = 0; k < baseZ; k++){
                    tiles[i,j,k].Initialize(font,gdm);
                }
            }
        }
    }

    public void LoadContent(ContentManager content,GraphicsDeviceManager gdm){

    }   
    public void Update(ref UpdatePackage up){
        //for(int i = 0; i < baseX; i++){
        //    for(int j = 0; j < baseY; j++){
        //        for(int k = 0; k < baseZ; k++){
        //            if(tiles[i,j,k].drawing){
        //                tiles[i,j,k].Update(ref up);
        //            }
        //        }
        //    }
        //}
    }
    public void Draw(SpriteBatch sb, int mode){
        switch(mode){
            case 0:
                for(int i = 0; i < baseX; i++){
                    for(int j = 0; j < baseY; j++){
                        for(int k = 0; k < baseZ; k++){
                            if(tiles[i,j,k].drawing){
                                tiles[i,j,k].Draw(sb);
                            }
                        }
                    }
                }
            break;
        }
    }
}