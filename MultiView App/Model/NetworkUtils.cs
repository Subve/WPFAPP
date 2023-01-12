using MultiViewApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiViewApp.Model
{
    public static class NetworkUtils
    {
        public static string IP;
        public static string SetIPAddress(string ipAdress)
        {
            
            IP = ipAdress;
            return IP;
        }
        public static string GetIPAddress()
        {
            return IP;
        }
    }
}
