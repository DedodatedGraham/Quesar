using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Quesar;
//These are combonations of parts, Allows for Non Sequential and randomly shapped structures to exist
public class Structure : Mappable{
    //Bounds, according to map
    public int maxX{get;set;}
    public int maxY{get;set;}
    public int maxZ{get;set;}
    public int minX{get;set;}
    public int minY{get;set;}
    public int minZ{get;set;}

    //poisiton
    public override Point renderTarget{get;set;}
    public List<Part> parts{get;set;}
    public void Initialize(GraphicsDeviceManager gdm){

    }

    public void LoadContent(ContentManager content,GraphicsDeviceManager gdm){

    }

    public void Update(ref UpdatePackage up){

    }

    public void Draw(SpriteBatch sb){

    }
}