using System;
using System.IO;
using System.Media;
using System.Windows;

namespace Assignment2
{
    //start of namespace
    public class voice_greeting
    {//start of class
        //void method to play the sound named greet
        public void greet()
        { //star of greet method

            //replace the \bin\Debug\ from the path with greeting.wav

            string auto_path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\greeting.wav");

            //create an instance for the soundPlayer class
            SoundPlayer greetMe = new SoundPlayer(auto_path);
            //then greet
            greetMe.Play();


        }//end of greet method


    }//end of class
}//end of namespace