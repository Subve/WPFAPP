namespace MultiViewApp.Model
{
    /**
     * @brief Simple parseable data model for IoT server response
     */ 
    public class ServerData
    {
        //public double data { get; set; }
        public double  Pressure { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
