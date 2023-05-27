using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Quesar;

public abstract class UIElement{

    public abstract int x{get; set;}
    public abstract int y{get; set;}
    public abstract int w{get; set;}
    public abstract int h{get; set;}
    public abstract string words{get;set;}
    public abstract bool _active{get; set;}
    public abstract List<Texture2D> textures{get;set;}
    public abstract int texture{get;set;}
    public abstract event EventHandler<UpdatePackage> lc;
    public abstract event EventHandler<UpdatePackage> rc;

    //to be overridden methods
    public abstract void Initialize(List<Texture2D> txs,SpriteFont fon);
    public abstract void LoadContent(ContentManager content);

    public abstract void Update(ref UpdatePackage up);
    //click
    //0 = no
    //1 = left
    //2 = right
    public abstract void Draw(SpriteBatch sb);
    public abstract void LeftClick(EventArgs e,ref UpdatePackage up);    
    public abstract void RightClick(EventArgs e,ref UpdatePackage up);
    public abstract void resize();


}