using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1Charts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(MainWindow));

        public MainWindow()
        {
            Items = new ObservableCollection<string>();
            DataContext = this;
            InitializeComponent();
            for (int i = 0; i < 100; i++)
            {
                Items.Add(i.ToString());
            }

            ContentRendered += MainWindow_ContentRendered;
        }

        private bool isFirstTime = true;
        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            if (isFirstTime)
            {
                DoMeasure();
                isFirstTime = false;
            }

        }

        private void DoMeasure()
        {
            for (int i = 0; i < 5000; i++)
            {
                DoEvents();
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int c = 0; c < 100; c++)
            {
                Items.Clear();
                for (int i = 0; i < 100; i++)
                {
                    Items.Add(i.ToString());
                }

                DoEvents();
            }
            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            int start = Items.Count;
            for (int i = 0; i < 10; i++)
            {
                int val = start + i;
                Items.Add(val.ToString());
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {

            Items.Clear();
        }

        private void Button_New(object sender, RoutedEventArgs e)
        {
            Items = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Items {
            get => (ObservableCollection<string>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        private void Button_Clear2(object sender, RoutedEventArgs e)
        {
            int itemsCount = Items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                Items.RemoveAt(0);
            }
        }
    }

    public class MyVirtualizingStackPanel : StackPanel
    {
        //protected override void OnClearChildren()
        //{
        //    //base.OnClearChildren();
        //   // BeforeClearChildren();
        //}

        //public void BeforeClearChildren()
        //{
        //    IRecyclingItemContainerGenerator irig = ItemContainerGenerator as IRecyclingItemContainerGenerator;
        //    if (irig != null) irig.Recycle(new GeneratorPosition(0, 0), RealizedChildren.Count);
        //}

        //IList RealizedChildren {
        //    get {
        //        PropertyInfo pi = this.GetType().BaseType.GetProperty("RealizedChildren", BindingFlags.NonPublic | BindingFlags.Instance);
        //        if (pi == null) return null;
        //        else
        //            return pi.GetValue(this) as IList;
        //    }
        //}
    }

    public class MyItemsControl : ItemsControl
    {
        private readonly Stack<DependencyObject> recycled = new Stack<DependencyObject>();
        private readonly bool allowReuse = true;
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            recycled.Push(element);
            base.ClearContainerForItemOverride(element, item);
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            if (recycled.Count > 0 && allowReuse)
            {
                return recycled.Pop();
            }
            else
            {
                return base.GetContainerForItemOverride();
            }
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }
    }

    public class MyBorder : Border
    {
        private static int instancrsCounter = 0;
        public static readonly DependencyProperty SomeValueProperty =
            DependencyProperty.Register("SomeValue", typeof(string), typeof(MyBorder), new PropertyMetadata(OnSomeValueChanged));

        public MyBorder()
        {
            Background = new SolidColorBrush(Color.FromRgb(200, 200, (byte)(instancrsCounter++ * 10)));
            BorderThickness = new Thickness(1, 1, 1, 0);
        }

        private static void OnSomeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }


        public string SomeValue {
            get => (string)GetValue(SomeValueProperty);
            set => SetValue(SomeValueProperty, value);
        }
    }
}
