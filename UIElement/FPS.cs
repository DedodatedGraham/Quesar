using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Quesar;
public class FPS : UIElement
{ 
    //override
    public override int x{get; set;}
    public override int y{get; set;}
    public override int w{get; set;}
    public override int h{get; set;}
    public override string words{get;set;}
    public override bool _active{get; set;}
    public override List<Texture2D> textures{get;set;}
    public override int texture{get;set;}
    public override event EventHandler<UpdatePackage> lc;
    public override event EventHandler<UpdatePackage> rc;
    //elements
    public SpriteFont font{get;set;}
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public float AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }

    public const int MaximumSamples = 100;

    private Queue<float> _sampleBuffer = new();

    //to be overridden methods
    public override void Initialize(List<Texture2D> txs,SpriteFont fon){
        this.font = fon;
    }
    public override void LoadContent(ContentManager content){

    }
    public override void Draw(SpriteBatch sb){
        sb.DrawString(font,AverageFramesPerSecond.ToString(),new Vector2(0,0),Color.White);
    }
    public override void LeftClick(EventArgs e,ref UpdatePackage up){
        lc?.Invoke(this,up);
    }
    public override void RightClick(EventArgs e,ref UpdatePackage up){
        rc?.Invoke(this,up);
    }
    public override void resize(){

    }
    public override void Update(ref UpdatePackage up)
    {
        float deltaTime = (float)up.gameTime.ElapsedGameTime.TotalSeconds;
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
    }
}