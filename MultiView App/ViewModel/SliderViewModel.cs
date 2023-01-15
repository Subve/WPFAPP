namespace AirApp.ViewModel
{
    public class SliderViewModel : BaseViewModel
    {
        private int _r;
        public int R //!< Red color component
        {
            get { return _r; }
            set
            {
                if(_r != value)
                {
                    _r = value;
                    _preview.SetViewColor(_r, _g, _b);
                    OnPropertyChanged("R");
                }
            }
        }

        private int _g;
        public int G //!< Green color component
        {
            get { return _g; }
            set
            {
                if (_g != value)
                {
                    _g = value;
                    _preview.SetViewColor(_r, _g, _b);
                    OnPropertyChanged("G");
                }
            }
        }

        private int _b;
        public int B //!< Blue color component
        {
            get { return _b; }
            set
            {
                if (_b != value)
                {
                    _b = value;
                    _preview.SetViewColor(_r, _g, _b);
                    OnPropertyChanged("B");
                }
            }
        }

        private LedViewModel _preview; //!< Selected color preview 

        /**
         * @brief Parametric contrustor
         * @param preview LED ViewModel for selected color preview 
         */ 
        public SliderViewModel(LedViewModel preview)
        {
            _r = 0;
            _g = 0;
            _b = 0;
            _preview = preview;
        }
    }
}
