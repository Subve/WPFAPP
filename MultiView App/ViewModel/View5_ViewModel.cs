#define CLIENT
#define GET
//#define DYNAMIC

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;

namespace AirApp.ViewModel
{
    using Model;

    /** 
      * @brief View model for MainWindow.xaml 
      */
    public class View5_ViewModel : BaseViewModel
    {
        #region Properties
        private string ipAddress;
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                if (ipAddress != value)
                {
                    ipAddress = value;
                    OnPropertyChanged("IpAddress");
                }
            }
        }
        private int sampleTime;
        public string SampleTime
        {
            get
            {
                return sampleTime.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (sampleTime != st)
                    {
                        sampleTime = st;
                        OnPropertyChanged("SampleTime");
                    }
                }
            }
        }

        public PlotModel DataPlotModel { get; set; }
        public PlotModel DataPlotModelP { get; set; }
        public PlotModel DataPlotModelH { get; set; }
        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }
        public ButtonCommand UpdateConfigButton { get; set; }
        public ButtonCommand DefaultConfigButton { get; set; }
        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion

        public View5_ViewModel()
        {
            DataPlotModel = new PlotModel { Title = "Temperature data" };

            DataPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -30,
                Maximum = 105,
                Key = "Vertical",
                Unit = "*C",
                Title = "Temperature"
            });

            DataPlotModel.Series.Add(new LineSeries() { Title = "temperature data series", Color = OxyColor.Parse("#FFFF0000") });
            //Pressure
            DataPlotModelP = new PlotModel { Title = "Pressure data" };

            DataPlotModelP.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModelP.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 260,
                Maximum = 1260,
                Key = "Vertical",
                Unit = "mbar",
                Title = "Pressure"
            });

            DataPlotModelP.Series.Add(new LineSeries() { Title = "Pressure data series", Color = OxyColor.Parse("#FFFF0000") });
            //Humidity
            DataPlotModelH = new PlotModel { Title = "Humidity data" };

            DataPlotModelH.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModelH.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 100,
                Key = "Vertical",
                Unit = "%",
                Title = "Humidity"
            });

            DataPlotModelH.Series.Add(new LineSeries() { Title = "Humidity data series", Color = OxyColor.Parse("#FFFF0000") });

            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;

            Server = new IoTServer("HTTP", IpAddress);
            
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePlot(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel.InvalidatePlot(true);
            
        }
        private void UpdatePlotP(double t, double d)
        {
            //
            LineSeries lineSeriesP = DataPlotModelP.Series[0] as LineSeries;

            lineSeriesP.Points.Add(new DataPoint(t, d));

            if (lineSeriesP.Points.Count > config.MaxSampleNumber)
                lineSeriesP.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModelP.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModelP.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModelP.InvalidatePlot(true);
        }
        private void UpdatePlotH(double t, double d)
        {
            //
            LineSeries lineSeriesH = DataPlotModelH.Series[0] as LineSeries;

            lineSeriesH.Points.Add(new DataPoint(t, d));

            if (lineSeriesH.Points.Count > config.MaxSampleNumber)
                lineSeriesH.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModelH.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModelH.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModelH.InvalidatePlot(true);
        }

        /**
          * @brief Asynchronous chart update procedure with
          *        data obtained from IoT server responses.
          * @param ip IoT server IP address.
          */
        private async void UpdatePlotWithServerResponse()
        {
#if CLIENT
#if GET
            string responseText = await Server.GETwithClient();
#else
            string responseText = await Server.POSTwithClient();
#endif
#else
#if GET
            string responseText = await Server.GETwithRequest();
#else
            string responseText = await Server.POSTwithRequest();
#endif
#endif
            try
            {
#if DYNAMIC
                dynamic resposneJson = JObject.Parse(responseText);
                UpdatePlot(timeStamp / 1000.0, (double)resposneJson.data);
#else
                ServerData resposneJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlot(timeStamp / 1000.0, resposneJson.Temperature);
                UpdatePlotP(timeStamp / 1000.0, resposneJson.Pressure);
                UpdatePlotH(timeStamp / 1000.0, resposneJson.Humidity);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);
                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }

        /**
          * @brief Synchronous procedure for request queries to the IoT server.
          * @param sender Source of the event: RequestTimer.
          * @param e An System.Timers.ElapsedEventArgs object that contains the event data.
          */
        private void RequestTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponse();
        }

        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimer()
        {
            if (RequestTimer == null)
            {
                RequestTimer = new Timer(config.SampleTime);
                RequestTimer.Elapsed += new ElapsedEventHandler(RequestTimerElapsed);
                RequestTimer.Enabled = true;

                DataPlotModel.ResetAllAxes();
                DataPlotModelP.ResetAllAxes();
                DataPlotModelH.ResetAllAxes();
            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimer()
        {
            if (RequestTimer != null)
            {
                RequestTimer.Enabled = false;
                RequestTimer = null;
            }
        }

        /**
         * @brief Configuration parameters update
         */
        private void UpdateConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams(ipAddress, sampleTime);
            Server = new IoTServer("HTTP", IpAddress);
            NetworkUtils.SetIPAddress(this.IpAddress);
            NetworkUtils.SetSampleTime(this.sampleTime);
            Console.WriteLine(this.IpAddress);
            if (restartTimer)
                StartTimer();
        }

        /**
          * @brief Configuration parameters defualt values
          */
        private void DefaultConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams();
            IpAddress = config.IpAddress;
            SampleTime = config.SampleTime.ToString();
            Server = new IoTServer("HTTP", IpAddress);

            if (restartTimer)
                StartTimer();
        }

        #endregion

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
