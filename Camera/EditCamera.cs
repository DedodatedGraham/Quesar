using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Quesar;

public class EditCamera
{

    public Vector2 position{get;set;}
    public float zoom{get;set;}//def 0, will be adjusted for cutscenes if needed & stuff
    public float rotation{get;set;}//def 0 
    public int screenWidth{get;set;}
    public int screenHeight{get;set;}
    public Vector2 screenCenter{get;set;}
    public Matrix TranslationMatrix{get;set;}

    public EditCamera(int[] screensize){
        screenWidth = screensize[0];
        screenHeight = screensize[1];
        zoom = 1.0f;
        speed = 25;
        screenCenter =  new Vector2( screenWidth * 0.5f, screenHeight * 0.5f );
        TranslationMatrix = Matrix.CreateTranslation( -(int) position.X, -(int) position.Y,0) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(zoom,zoom,1)) * Matrix.CreateTranslation(new Vector3(screenCenter,0));
    }
    public float speed{get;set;}
    public void MoveCamera( Vector2 cameraMovement){
        position = position + cameraMovement * speed;
        TranslationMatrix = Matrix.CreateTranslation( -(int) position.X, -(int) position.Y,0) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(zoom,zoom,1)) * Matrix.CreateTranslation(new Vector3(screenCenter,0));
    }

    public Rectangle ViewportWorldBoundry(){
        Vector2 viewPortCorner = ScreenToWorld(new Vector2(0,0));
        Vector2 viewPortBottomCorner = ScreenToWorld( new Vector2( screenWidth, screenHeight ) );
        return new Rectangle((int) viewPortCorner.X,(int) viewPortCorner.Y,(int) ( viewPortBottomCorner.X - viewPortCorner.X ),(int) ( viewPortBottomCorner.Y - viewPortCorner.Y ));
    }
    public void CenterOn0( Vector2 newPosition ){
        position = newPosition;
    }
    public Vector2 WorldToScreen( Vector2 worldPosition ){
        return Vector2.Transform( worldPosition, TranslationMatrix );
    }

    public Vector2 ScreenToWorld( Vector2 screenPosition ){
        return Vector2.Transform(screenPosition,Matrix.Invert(TranslationMatrix));
    }
}
