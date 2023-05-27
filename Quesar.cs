using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Quesar{
    public static class Quesar
    {
        [STAThread]
        static void Main(){
            
            bool mode = false;
            if(mode){
                Debug.WriteLine("Starting Game");
                using var game = new GameMode();
                game.Run();
            }
            else{
                Debug.WriteLine("Starting Dev");
                using var dev = new DevMode();
                dev.Run();
            }
        }
    }
}

