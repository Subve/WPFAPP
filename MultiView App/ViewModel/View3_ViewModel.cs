using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
namespace MultiViewApp.ViewModel
{
    using Model;
    using Newtonsoft.Json;
    using System;

    public class View3_ViewModel : BaseViewModel
    {
        public ObservableCollection<MeasurementViewModel> Measurements { get;  set; }
        public ButtonCommand Refresh { get; set; }
        public ButtonCommand PressMid { get; set; }
        public ButtonCommand PressLeft { get; set; }
        public ButtonCommand PressRight { get; set; }
        public ButtonCommand PressUp { get; set; }
        public ButtonCommand PressDown { get; set; }

        private ServerIoTmock ServerMock = new ServerIoTmock();
        private IoTServer Server;
        private string ipAddress;

        public View3_ViewModel()
        {
            // Create new collection for measurements data
            Measurements = new ObservableCollection<MeasurementViewModel>();
            var newIp = NetworkUtils.GetIPAddress();
            Server = new IoTServer("HTTP", newIp);
            // Bind button with action
            Refresh = new ButtonCommand(RefreshHandler);
            PressMid = new ButtonCommand(PressedMidHandler);
            PressLeft = new ButtonCommand(PressedLeftHandler);
            PressRight = new ButtonCommand(PressedRightHandler);
            PressUp = new ButtonCommand(PressedUpHandler);
            PressDown = new ButtonCommand(PressedDownHandler);
        }
        private async void UpdateDataWithServerResponse()
        {
            string responseText = await Server.GETDatawithRequest();
            //var measurementsList = measurementsJsonArray.ToObject<List<MeasurementModel>>();
            ServerDataAngles resposneJson = JsonConvert.DeserializeObject<ServerDataAngles>(responseText);
            string clickText=await Server.GETClickswithRequest();
            ServerClicksCounter responseClicksJson=JsonConvert.DeserializeObject<ServerClicksCounter>(clickText);
            
            JArray measurementsJsonArray = Server.getMeasurements(resposneJson.Roll,resposneJson.Pitch,resposneJson.Yaw,responseClicksJson.counter_mid,responseClicksJson.counter_x,responseClicksJson.counter_y);
            var measurementsList = measurementsJsonArray.ToObject<List<MeasurementModel>>();

            // Add new elements to collection
            if (Measurements.Count < measurementsList.Count)
            {
                foreach (var m in measurementsList)
                    Measurements.Add(new MeasurementViewModel(m));
            }
            // Update existing elements in collection
            else
            {
                for (int i = 0; i < Measurements.Count; i++)
                    Measurements[i].UpdateWithModel(measurementsList[i]);
            }
        }
        public void RefreshHandler()
        {

            // Read data from server in JSON array format
            // TODO: replace mock with network comunnication
            JArray measurementsJsonArray = ServerMock.getMeasurements();
            UpdateDataWithServerResponse();
            // Convert generic JSON array container to list of specific type
            /*var measurementsList = measurementsJsonArray.ToObject<List<MeasurementModel>>();
            
            // Add new elements to collection
            if (Measurements.Count < measurementsList.Count)
            {
                foreach (var m in measurementsList)
                    Measurements.Add(new MeasurementViewModel(m));
            }
            // Update existing elements in collection
            else
            {
                for (int i = 0; i < Measurements.Count; i++)
                    Measurements[i].UpdateWithModel(measurementsList[i]);
            }
            */
        }
        public void PressedMidHandler()
        {
            var result = Server.GETMidwithRequest();
        }
        public void PressedUpHandler()
        {
            var result = Server.GETUpwithRequest();
        }
        public void PressedDownHandler()
        {
            var result = Server.GETDownwithRequest();
        }
        public void PressedLeftHandler()
        {
            var result = Server.GETLeftwithRequest();
        }
        public void PressedRightHandler()
        {
            var result = Server.GETRightwithRequest();
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
