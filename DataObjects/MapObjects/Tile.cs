using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Quesar;

public class Tile{
    //Texture sheet for specific tile
    public Texture2D textureSheet{get;set;}
    public SpriteFont font{get;set;}
    public int baseX;
    public int baseY;
    public int baseZ;
    //origin
    public int originX;
    public int originY;
    public int[] tileIndex;
    //Drawing mode
    public bool drawing{get;set;}
    public Tile(int mode, int tileX, int tileY, int tileZ,int[] tilei,int orgx, int orgy){
        baseX = tileX;
        baseY = tileY;
        baseZ = tileZ;
        tileIndex = tilei;
        originX = orgx;
        originY = orgy;
    }
    public void Initialize(SpriteFont fon,GraphicsDeviceManager gdm){
        font = fon;
    }

    public void LoadContent(ContentManager content,GraphicsDeviceManager gdm){

    }

    public void Update(ref UpdatePackage up){

    }

    public void Draw(SpriteBatch sb){
        int x = baseX * tileIndex[0] + originX;
        int y = baseY * tileIndex[1] + originY;
        Vector2 temp = font.MeasureString(tileIndex[2].ToString());
        int measureX = x + (baseX - (int)temp.X)/2;
        int measureY = y + (baseY - (int)temp.Y)/2;
        sb.DrawString(font,tileIndex[2].ToString(),new Vector2(measureX,measureY),Color.White);
    }
}