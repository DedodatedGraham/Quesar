using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Quesar;

public class LoadingBar : UIElement{
    public override int x{get; set;}
    public override int y{get; set;}
    public override int w{get; set;}
    public override int h{get; set;}
    public override bool _active{get; set;}
    public override List<Texture2D> textures{get;set;}
    public override int texture{get;set;}
    public override event EventHandler<UpdatePackage> lc;
    public override event EventHandler<UpdatePackage> rc;
    public override string words{get;set;}

    //to be overridden methods
    public LoadingBar(){
        
    }
    public override void Initialize(List<Texture2D> txs,SpriteFont fon){

    }
    public override void LoadContent(ContentManager content){

    }

    public override void Update(ref UpdatePackage up){

    }
    public override void Draw(SpriteBatch sb){

    }
    
    public override void LeftClick(EventArgs e,ref UpdatePackage up){
        lc?.Invoke(this,up);
    } 
    public override void RightClick(EventArgs e,ref UpdatePackage up){
        rc?.Invoke(this,up);
    }
    public override void resize()
    {
        
    }
}
