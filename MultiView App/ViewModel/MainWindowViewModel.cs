using System;
using System.Collections.Generic;
using System.Text;

namespace MultiViewApp.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _contetnViewModel;
        public BaseViewModel ContentViewModel
        {
            get { return _contetnViewModel; }
            set
            {
                if(_contetnViewModel != value)
                {
                    _contetnViewModel = value;
                    OnPropertyChanged("ContentViewModel");
                }
            }
        }

        public ButtonCommand MenuCommandView1 { get; set; }
        public ButtonCommand MenuCommandView2 { get; set; }
        public ButtonCommand MenuCommandView3 { get; set; }
        public ButtonCommand MenuCommandView4 { get; set; }
        public ButtonCommand MenuCommandView5 { get; set; }

        public MainWindowViewModel()
        {
            MenuCommandView1 = new ButtonCommand(MenuSetView1);
            MenuCommandView2 = new ButtonCommand(MenuSetView2);
            MenuCommandView3 = new ButtonCommand(MenuSetView3);
            MenuCommandView4 = new ButtonCommand(MenuSetView4);
            MenuCommandView5 = new ButtonCommand(MenuSetView5);

            ContentViewModel = new View1_ViewModel(); // View1_ViewModel.Instance
        }

        private void MenuSetView1()
        {
            ContentViewModel = new View1_ViewModel(); // View1_ViewModel.Instance
        }

        private void MenuSetView2()
        {
            ContentViewModel = new View2_ViewModel(); // View1_ViewModel.Instance
        }
        private void MenuSetView3()
        {
            ContentViewModel = new View3_ViewModel(); // View1_ViewModel.Instance
        }
        private void MenuSetView4()
        {
            ContentViewModel = new View4_ViewModel(); // View1_ViewModel.Instance
        }
        private void MenuSetView5()
        {
            ContentViewModel = new View5_ViewModel(); // View1_ViewModel.Instance
        }
    }
}
