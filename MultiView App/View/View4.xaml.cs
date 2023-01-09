using MultiViewApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiViewApp.View
{
    /// <summary>
    /// Interaction logic for View4.xaml
    /// </summary>
    public partial class View4 : UserControl
    {
        private readonly View4_ViewModel _viewModel;
        public View4()
        {
            InitializeComponent();
            _viewModel = new View4_ViewModel();
            DataContext = _viewModel;

            /* Button matrix grid definition */
            for (int i = 0; i < _viewModel.DisplaySizeX; i++)
            {
                ButtonMatrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
                ButtonMatrixGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }

            for (int i = 0; i < _viewModel.DisplaySizeY; i++)
            {
                ButtonMatrixGrid.RowDefinitions.Add(new RowDefinition());
                ButtonMatrixGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
            }

            for (int i = 0; i < _viewModel.DisplaySizeX; i++)
            {
                for (int j = 0; j < _viewModel.DisplaySizeY; j++)
                {
                    // <Button
                    Button led = new Button()
                    {
                        // Name = "LEDij"
                        Name = "LED" + i.ToString() + j.ToString(),
                        // Style="{StaticResource LedButtonStyle}"
                        Style = (Style)FindResource("LedIndicatorStyle"),
                        // BorderThicness="2"
                        BorderThickness = new Thickness(2),
                    };
                    // Command="{Binding LedCommands[i][j]}" 
                    led.SetBinding(Button.CommandProperty, _viewModel.GetCommandBinding(i, j));
                    // Color="{Binding Leds[i][j].ViewColor}" 
                    led.SetBinding(Button.BackgroundProperty, _viewModel.GetColordBinding(i, j));
                    // Grid.Column="i" 
                    Grid.SetColumn(led, i);
                    // Grid.Row="j"
                    Grid.SetRow(led, j);
                    // />

                    ButtonMatrixGrid.Children.Add(led);
                    ButtonMatrixGrid.RegisterName(led.Name, led);
                }
            }
        }
    }
}
