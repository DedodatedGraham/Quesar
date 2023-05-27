using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;

public class Button : UIElement{
    public override int x{get; set;}
    public override int y{get; set;}
    public override int w{get; set;}
    public override int h{get; set;}    
    public override string words{get;set;}
    public Rectangle perso{get;set;}
    public override bool _active{get; set;}
    public override List<Texture2D> textures{get;set;}
    public override int texture{get;set;}
    public SpriteFont font;
    private Vector2 measure;
    private Color defcolor;
    public override event EventHandler<UpdatePackage> lc;
    public override event EventHandler<UpdatePackage> rc;


    public Button(int x, int y, int w, int h,string word = "",int col = 0){
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.words = word;
        measure = new Vector2();
        if(col == 0){
            //defcolor = Color.SpringGreen;
            defcolor = Color.White;
        }
        else if(col == 1){
            defcolor = Color.Red;
        }
    }
    public override void Initialize(List<Texture2D> txs,SpriteFont fon){
        textures = txs; // For Button 0 is base 1 is on 2 is off
        this.font = fon;
    }
    public override void LoadContent(ContentManager content){

    }
    public override void Update(ref UpdatePackage up){
        if(up.mouseData[0] == 1){
            LeftClick(EventArgs.Empty,ref up);
        }
        else if(up.mouseData[0] == 2){
            RightClick(EventArgs.Empty,ref up);
        }
    }
    public override void Draw(SpriteBatch sb){
        sb.Draw(textures[0],perso,Color.White);
        if(words.Length > 0){
            sb.DrawString(this.font,words,measure,defcolor);
        }
    }
    public override void LeftClick(EventArgs e,ref UpdatePackage up)
    {
        lc?.Invoke(this,up);
    }
    public override void RightClick(EventArgs e,ref UpdatePackage up)
    {
        rc?.Invoke(this,up);
    }
    public override void resize()
    {
        perso = new Rectangle(x,y,w,h);
        if(words.Length > 0){
            //gets location for string placement
            Vector2 temp = font.MeasureString(words);
            measure.X = x + (w - temp.X)/2;
            measure.Y = y + (h - temp.Y)/2;
        }
    }
}