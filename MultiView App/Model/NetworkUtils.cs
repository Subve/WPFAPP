using AirApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirApp.Model
{
    public static class NetworkUtils
    {
        public static string IP;
        private static int sampleTime;
        public static string SetIPAddress(string ipAdress)
        {
            
            IP = ipAdress;
            return IP;
        }
        public static string GetIPAddress()
        {
            return IP;
        }
        public static int SetSampleTime(int _sampleTime)
        {

            sampleTime = _sampleTime;
            return sampleTime;
        }
        public static int GetSampleTime()
        {
            return sampleTime;
        }
    }
}
