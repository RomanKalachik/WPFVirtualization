using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public ObservableCollection<string> Items {
            get { return (ObservableCollection<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(MainWindow));


        public MainWindow()
        {
            Items = new ObservableCollection<string>();
            for (int i = 0; i < 1000; i++) Items.Add(i.ToString());
            DataContext = this;
            InitializeComponent();
        }
    }
    public class MyBorder : Border {
        static int instancrsCounter = 0;
        public MyBorder() {
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, (byte)(instancrsCounter++ * 10)));
            BorderThickness = new Thickness(1,1,1,0);
        }
    }
}
