using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Core.Entities;
using Core.Models;
using DAL;

namespace MeltProperties
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BaseCompositionModel> MyList = new ObservableCollection<BaseCompositionModel>();

        public ObservableCollection<string> Oxides = new ObservableCollection<string>();

        private Oxide firstOxide;

        private Oxide secondOxide;

        public string SelectedOxide;

        private int startTemperature;

        private int endTemperature;

        private int range;

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        public int StartTemperature
        {
            get { return startTemperature; }
            set
            {
                startTemperature = value;
                OnPropertyChanged("StartTemperature");
            }
        }

        public int EndTemperature
        {
            get { return endTemperature; }
            set
            {
                endTemperature = value;
                OnPropertyChanged("EndTemperature");
            }
        }

        public int Range
        {
            get { return range; }
            set
            {
                range = value;
                OnPropertyChanged("Range");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public PhasesSystem FirstSystem;

        public PhasesSystem SecondSystem;

        private float firstModule;

        private float secondModule;

        public ObservableCollection<SystemsByTemperaturesModel> TemperaturesModels { get; set; }

        public MainWindow()
        {
            Sqlite.Start();
            InitializeComponent();
            InitializeOxides();
            ((CollectionViewSource)this.Resources["VS"]).Source = MyList;
            DataContext = this;

        }

        private void InitializeOxides()
        {
            var list = Sqlite.GetAllOxides().ToList();
            foreach (var oxide in list.Where(x => x.IsDefault).OrderByDescending(x => x.IsRequred))
            {
                MyList.Add(new BaseCompositionModel()
                {
                    Composition = oxide,
                    List = list
                });
            }
        }

    
        private void Go(object sender, RoutedEventArgs e)
        {
            string res = string.Empty;
            foreach (var model in MyList)
            {
                res += string.Format("{0}: {1} \n", model.Composition.Formula, model.Percentage);
            }

            var count = 0;
            do
            {
                SetFullPercentage();
                if (count > 3)
                    break;
                res = string.Empty;
                foreach (var model in MyList)
                {
                    res += string.Format("{0}: {1} \n", model.Composition.Formula, model.Percentage);
                }
                MessageBox.Show(res);

                count++;

            } while (!MyList.Sum(x => x.Percentage).Equals(100));

            this.SetSystems();
            this.Main.Visibility = Visibility.Hidden;
            this.Temperatures.Visibility = Visibility.Visible;
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            this.Temperatures.Visibility = Visibility.Hidden;
            this.Main.Visibility = Visibility.Visible;
        }

        private void BackToTemp(object sender, RoutedEventArgs e)
        {
            this.Phases.Visibility = Visibility.Hidden;
            this.Temperatures.Visibility = Visibility.Visible;
        }

        public void SetTemperatures(object sender, RoutedEventArgs e)
        {
            this.TemperaturesModels = new ObservableCollection<SystemsByTemperaturesModel>();
            var currentTemperature = StartTemperature;
            while (currentTemperature <= EndTemperature)
            {
                TemperaturesModels.Add(new SystemsByTemperaturesModel(this.FirstSystem, this.SecondSystem, currentTemperature));
                currentTemperature += Range;
            }
            SetTable();
            this.Temperatures.Visibility = Visibility.Hidden;

            this.Phases.Visibility = Visibility.Visible;

        }

        private void SetTable()
        {
            this.Phases.Children.Clear();
            this.Phases.RowDefinitions.Clear();
            this.Phases.ColumnDefinitions.Clear();
            var countCol = this.TemperaturesModels.First().FirstSystem.Phases.Count;
            countCol += this.TemperaturesModels.First().SecondSystem.Phases.Count;
            countCol++;
            for (int i = 0; i < countCol; i++)
            {
                this.Phases.ColumnDefinitions.Add(new ColumnDefinition(){Width = new GridLength(50)});
            }

            this.Phases.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });

            foreach (var model in this.TemperaturesModels)
            {
                this.Phases.RowDefinitions.Add(new RowDefinition(){Height = new GridLength(25)});            
            }

            this.Phases.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            this.Phases.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            this.Phases.RowDefinitions.Add(new RowDefinition());

            this.SetTitle("Фазовый состав в интервале температур", countCol);
            this.AddTextBlockInGrid("Temp", 1, 0, 1, 3, HorizontalAlignment.Center, true);
            this.AddTextBlockInGrid(this.TemperaturesModels.First().FirstSystem.Formula, 1, 1, this.TemperaturesModels.First().FirstSystem.Phases.Count, 1, HorizontalAlignment.Center, true);
            this.AddTextBlockInGrid(this.TemperaturesModels.First().SecondSystem.Formula, 1, 1 + this.TemperaturesModels.First().FirstSystem.Phases.Count, this.TemperaturesModels.First().SecondSystem.Phases.Count, 1, HorizontalAlignment.Center, true);
            var currentColumn = 1;
            foreach (var model in this.TemperaturesModels.First().FirstSystem.Phases)
            {
                this.AddTextBlockInGrid(model.Phase.Formula, 2, currentColumn, 1, 1, HorizontalAlignment.Center, true);
                currentColumn++;
            }

            foreach (var model in this.TemperaturesModels.First().SecondSystem.Phases)
            {
                this.AddTextBlockInGrid(model.Phase.Formula, 2, currentColumn, 1, 1, HorizontalAlignment.Center, true);
                currentColumn++;
            }
            
            var currentRow = 3;
            foreach (var model in this.TemperaturesModels)
            {
               currentColumn = 0;
               this.AddTextBlockInGrid(model.Temperature.ToString(), currentRow, currentColumn, 1, 1, HorizontalAlignment.Left, true);
                currentColumn++;
                foreach (var phase in model.FirstSystem.Phases)
                {
                    this.AddTextBoxInGrid(currentRow, currentColumn, phase);
                    currentColumn++;
                }
                foreach (var phase in model.SecondSystem.Phases)
                {
                    this.AddTextBoxInGrid(currentRow, currentColumn, phase);
                    currentColumn++;
                }
                currentRow++;
            }

            var backButton = new Button();
            backButton.Content = "Назад";
            backButton.Width = 145;
            backButton.Height = 30;
            backButton.FontSize = 18;
            backButton.Margin = new Thickness(0, 20, 0, 0);
            backButton.Click += BackToTemp;
            backButton.SetValue(Grid.RowProperty, currentRow);
            backButton.SetValue(Grid.ColumnProperty, 1);
            backButton.SetValue(Grid.ColumnSpanProperty, 3);
            this.Phases.Children.Add(backButton);

            var button = new Button();
            button.Content = "Рассчитать";
            button.Width = 145;
            button.Height = 30;
            button.FontSize = 18;
            button.Margin = new Thickness(0, 20, 0, 0);
            button.Click += PhaseGo;
            button.SetValue(Grid.RowProperty, currentRow);
            button.SetValue(Grid.ColumnProperty, countCol - 3);
            button.SetValue(Grid.ColumnSpanProperty, 3);
            this.Phases.Children.Add(button);

        }

        private void SetTitle(string text, int colSpan)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = 18;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.SetValue(Grid.RowProperty, 0);
            textBlock.SetValue(Grid.ColumnProperty, 0);
            textBlock.SetValue(Grid.ColumnSpanProperty, colSpan);
            this.Phases.Children.Add(textBlock);
        }

        private void AddTextBoxInGrid(int row, int column, object bind, int colSpan = 1, int rowSpan = 1)
        {
            var textBox = new TextBox();
            textBox.SetValue(Grid.RowProperty, row);
            textBox.SetValue(Grid.ColumnProperty, column);
            textBox.SetValue(Grid.ColumnSpanProperty, colSpan);
            textBox.SetValue(Grid.RowSpanProperty, rowSpan);
            Binding myBinding = new Binding();
            myBinding.Source = bind;
            myBinding.Path = new PropertyPath("Percentage");
            myBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, myBinding);
            this.Phases.Children.Add(textBox);
        }

        private void AddTextBlockInGrid(string text, int row, int column, int colSpan = 1, int rowSpan = 1, HorizontalAlignment alignment = HorizontalAlignment.Left, bool isBold = false)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            if (isBold)
            {
                textBlock.FontWeight = FontWeights.Bold;
            }

            textBlock.HorizontalAlignment = alignment;
            textBlock.SetValue(Grid.RowProperty, row);
            textBlock.SetValue(Grid.ColumnProperty, column);
            textBlock.SetValue(Grid.ColumnSpanProperty, colSpan);
            textBlock.SetValue(Grid.RowSpanProperty, rowSpan);
            this.Phases.Children.Add(textBlock);
        }

        private void SetFullPercentage()
        {
            var sum = MyList.Sum(x => x.Percentage);
            if (sum.Equals(100))
                return;
            foreach (var composition in MyList)
            {
                composition.Percentage = (composition.Percentage * 100) / sum;
            }
        }

        private void SetSystems()
        {
            this.GetSystemsOxides();
            var systems = Sqlite.GetSystems(firstOxide, secondOxide).ToList();
            this.FirstSystem = systems.FirstOrDefault();
            this.SecondSystem = systems.LastOrDefault();
            MessageBox.Show(string.Format("{0}\n{1}", this.FirstSystem.Formula, this.SecondSystem.Formula));
        }

        private void GetSystemsOxides()
        {
            float firstMax, secondMax;
            var maxPercent = MyList.Where(x => !x.Composition.IsRequred && x.Composition.IsDefault).Max(x => x.Percentage);
            firstMax = maxPercent;
            firstOxide = MyList.First(x => x.Percentage.Equals(maxPercent)).Composition;
            maxPercent = MyList.Where(x => !x.Composition.IsRequred && x.Composition.IsDefault && !x.Composition.Equals(firstOxide)).Max(x => x.Percentage);
            secondMax = maxPercent;
            secondOxide = MyList.First(x => !x.Composition.Equals(firstOxide) && x.Percentage.Equals(maxPercent)).Composition;
            this.SetModules(firstMax, secondMax);
        }

        private void SetModules(float firstMax, float secondMax)
        {
            this.firstModule = firstMax/(firstMax + secondMax);
            this.secondModule = secondMax / (firstMax + secondMax);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            MyList.Add(new BaseCompositionModel(Sqlite.GetAllOxides()));
        }

        private void PhaseGo(object sender, RoutedEventArgs e)
        {
            string res = this.TemperaturesModels.First().FirstSystem.Phases.First().Phase.Formula;
            res += "\n";
            res += this.TemperaturesModels.First().FirstSystem.Phases.First().Percentage.ToString();
            MessageBox.Show(res);
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            MyList.RemoveAt(MyList.Count - 1);
        }

        private void SetComposition(object sender, RoutedEventArgs e)
        {
            var currentTextBlock = ((TextBlock)sender);
            currentTextBlock.Visibility = Visibility.Collapsed;
            var textBox = ((StackPanel)currentTextBlock.Parent).Children[1];
            textBox.Visibility = Visibility.Visible;
            textBox.LostFocus += TextBoxLostFocus;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var currentTextBox = ((TextBox)sender);
            currentTextBox.Visibility = Visibility.Collapsed;
            ((StackPanel)currentTextBox.Parent).Children[0].Visibility = Visibility.Visible;
        }
    }
}