using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;
public class TerrainGen : Generator{
    public override void Generate(object obj){
        MapTree map = (MapTree)obj;
        Random rnd = new Random();
        //This method will adjust map
        int chunkX = map.chunkMap.GetLength(0);
        int chunkY = map.chunkMap.GetLength(1);
        //Will Go chunk by chunk

        //Constants for generation
        double spread = 4;
        double offset = 0.3;
        //Surface Height generation
        int[,] heigthmarker = new int[chunkX*map.chunkMap[0,0].baseX,chunkY*map.chunkMap[0,0].baseY];
        for(int i = 0; i < chunkX; i++){
            for(int j = 0; j < chunkY; j++){
                //Gets size of current chunk
                int tileXcount = map.chunkMap[i,j].baseX;
                int tileYcount = map.chunkMap[i,j].baseY;
                int tileZcount = map.chunkMap[i,j].baseZ;
                int lastZ = 0;
                for(int p = 0; p < tileXcount; p++){
                    for(int q = 0; q < tileYcount; q++){

                        //Starting Chunk
                        if(q == 0 && p == 0 && j == 0 && i == 0){
                            //Start command,Start chunk
                            //string saying = "Starting " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int startz = rnd.Next(0,map.chunkMap[i,j].baseZ);
                            heigthmarker[ i*tileXcount + p,j*tileYcount + q] = startz;
                            //drawing to rep surface
                            map.chunkMap[i,j].tiles[p,q,startz].drawing = true;
                            lastZ = startz;
                        }
                        else if(q == 0 && p != 0 && j == 0 && i == 0){
                            //When no longer in the first column, but still first row, first chunk
                            //string saying = "Top " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[i * tileXcount +  (p - 1),j * tileYcount + q];
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p == 0 && j == 0 && i == 0){
                            //When no longer in the first row, but still first column, first chunk
                            //string saying = "Left " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)];
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p != 0 && i == 0 && j == 0){
                            //Regular pattern for starting chunk
                            //string saying = "Middle " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + (p - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }

                        //Top Chunk Row
                        else if(q == 0 && p == 0 && i != 0  && j == 0){
                            //First part of a new chunk on top row of chunks
                            //string saying = "Top Starting" + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[(i - 1) * tileXcount + (tileXcount - 1),j * tileYcount + q];
                            //saying = "polling from Heigthmarker [" + ((i - 1) * tileXcount + (tileXcount - 1)) + "," + (j * tileYcount + q) + "]";
                            //Debug.WriteLine(saying);
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q == 0 && p != 0 && i != 0  && j == 0){
                            //Top row chunk & tile
                            //string saying = "Top Top " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[i * tileXcount +  (p - 1),j * tileYcount + q];
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p == 0 && i != 0 && j == 0){
                            //Left pattern for top chunk
                            //string saying = "Top Left " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[(i - 1) * tileXcount + (tileXcount - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p != 0 && i != 0 && j == 0){
                            //middle pattern for top chunk
                            //string saying = "Top Middle " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + (p - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }

                        //Left Chunk Col
                        else if(q == 0 && p == 0 && i == 0  && j != 0){
                            //First part of a new chunk on left collumn
                            //string saying = "Left Starting" + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[i * tileXcount + p,(j - 1) * tileYcount + (tileYcount - 1)];
                            //saying = "polling from Heigthmarker [" + (i * tileXcount + p) + "," + ((j - 1) * tileYcount + (tileYcount - 1)) + "]";
                            //Debug.WriteLine(saying);
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q == 0 && p != 0 && i == 0  && j != 0){
                            //Top of a left chunk
                            //string saying = "Left Top " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + p,(j - 1) * tileYcount + (tileYcount - 1)]  + heigthmarker[i * tileXcount + (p - 1),j * tileYcount + q]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p == 0 && i == 0  && j != 0){
                            //left col chunk & tile
                            //string saying = "Left Left " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[i * tileXcount +  p,j * tileYcount + (q - 1)];
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p != 0 && i == 0  && j != 0){
                            //Regular pattern for starting chunk
                            //string saying = "Left Middle " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + (p - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }

                        //Middle Chunk
                        else if(q == 0 && p == 0 && i != 0  && j != 0){
                            //First part of a new chunk middle
                            //string saying = "Middle Starting" + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = heigthmarker[(i - 1) * tileXcount + (tileXcount - 1),(j - 1) * tileYcount + (tileYcount - 1)];
                            //saying = "polling from Heigthmarker [" + ((i - 1) * tileXcount + (tileXcount - 1)) + "," + ((j - 1) * tileYcount + (tileYcount - 1)) + "]";
                            //Debug.WriteLine(saying);
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q == 0 && p != 0 && i != 0  && j != 0){
                            //Top of a middle chunk
                            //string saying = "Middle Top " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + p,(j - 1) * tileYcount + (tileYcount - 1)]  + heigthmarker[i * tileXcount + (p - 1),j * tileYcount + q]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p == 0 && i != 0  && j != 0){
                            //left col chunk & tile
                            //string saying = "Middle Left " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[(i - 1) * tileXcount + (tileXcount - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                        else if(q != 0 && p != 0 && i != 0  && j != 0){
                            //Regular pattern for starting chunk
                            //string saying = "Middle Middle " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            //Debug.WriteLine(saying);
                            int newz = (heigthmarker[i * tileXcount + (p - 1),j * tileXcount + q]  + heigthmarker[i * tileXcount + p,j * tileYcount + (q - 1)]) / 2;
                            double tbl = Math.Floor(newz - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(newz + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }

                        //Last resort
                        else{
                            //If thres no surrounding area to pull from it will check with last z and approx
                            string saying = "Other " + "Chunk[" + i.ToString() + "," + j.ToString() + "] Tile[" + p.ToString() + "," + q.ToString() + "]";
                            Debug.WriteLine(saying);
                            double tbl = Math.Floor(lastZ - (spread)/2 + offset); 
                            double ttl = Math.Ceiling(lastZ + (spread)/2 + offset);
                            //corrects if falls off bounds
                            int bl = (int)Math.Max(map.chunkMap[i,j].minZ,tbl);
                            int tl = (int)Math.Min(map.chunkMap[i,j].maxZ,ttl);
                            int newz = rnd.Next(bl,tl);
                            map.chunkMap[i,j].tiles[p,q,newz].drawing = true;
                            lastZ = newz;
                            heigthmarker[i * tileXcount + p,j * tileYcount + q] = newz;
                        }
                    }
                }
            }
        }
    }








}