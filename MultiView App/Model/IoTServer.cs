﻿/**
 ******************************************************************************
 * @file    LED Display Control Example/Model/IoTServer.cs
 * @author  Adrian Wojcik  adrian.wojcik@put.poznan.pl
 * @version V1.0
 * @date    07-May-2021
 * @brief   LED display controller: IoT server model
 ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiViewApp.Model
{
    public class IoTServer
    {
        private string _ip;

        public IoTServer(string ip)
        {
            _ip = ip;
        }

        /**
         * @brief obtaining the address of the PHP script from IoT server IP.
         * @return LED display control script URL
         */
        public string ScriptUrl
        {
            get => "http://" + _ip + "/server/led_display.php";
        }

        /**
         * @brief Send control request using HttpClient with POST method
         * @param data Control data in JSON format
         */
        public async Task<string> PostControlRequest(List<KeyValuePair<string, string>> data)
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var requestData = new FormUrlEncodedContent(data);
                    // Sent POST request
                    var result = await client.PostAsync(ScriptUrl, requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
    }
}