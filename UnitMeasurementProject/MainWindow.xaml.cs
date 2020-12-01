using System;
using System.Collections.Generic;
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
using UnitMeasurementProject.Classes;

namespace UnitMeasurementProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Unit> lstUnits = new List<Unit>();
        private List<ListBox> lstListBoxs = new List<ListBox>();


        internal List<Unit> LstUnits { get => lstUnits; set => lstUnits = value; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeUnits();

            txtInputValue.Text = "0";

            txtInputValue.TextChanged += txtInputValue_TextChanged;
            fromTabControl.SelectionChanged += TabControl_SelectionChanged;
            toTabControl.SelectionChanged += TabControl_SelectionChanged;
            InitalizeLists();

        }

        //Main Functionnality based on the Scale information located in each unit object
        public void ConvertUnit(Unit? unitFrom, Unit? unitTo, double value)
        {
            if(unitFrom.HasValue && unitTo.HasValue)
            {
                if(unitFrom.Value.Type == unitTo.Value.Type)
                {
                    double result = 0;
                    result = (value / unitFrom.Value.Scale) * unitTo.Value.Scale;

                    txtOutputValue.Text = result.ToString();
                }
            }
           else
                MessageBox.Show("Two units must be selected", "Alert", MessageBoxButton.OK, MessageBoxImage.Error); 
        }

        //Should be replaced by a proper database
        public void InitializeUnits()
        {
            //Initializing distances
            Unit meter = new Unit()
            {
                Type = UnitType.Distance,
                Name = "meter",
                Abbreviation = "m",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 1
            };
            Unit centimeter = new Unit()
            {
                Type = UnitType.Distance,
                Name = "centimeter",
                Abbreviation = "cm",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 100
            };
            Unit kilometer = new Unit()
            {
                Type = UnitType.Distance,
                Name = "kilometer",
                Abbreviation = "km",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 0.001
            };
            Unit foot = new Unit()
            {
                Type = UnitType.Distance,
                Name = "foot",
                Abbreviation = "ft",
                Localisation = Localisation.US,
                Scale = 3.281
            };
            Unit inch = new Unit()
            {
                Type = UnitType.Distance,
                Name = "inch",
                Abbreviation = "in",
                Localisation = Localisation.US,
                Scale = 39.37
            };

            // Initializing Volumes
            Unit liter = new Unit()
            {
                Type = UnitType.Volume,
                Name = "liter",
                Abbreviation = "L",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 1
            }; 
            Unit milliliter = new Unit()
            {
                Type = UnitType.Volume,
                Name = "milliliter",
                Abbreviation = "mL",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 1000
            };
            Unit centiliter = new Unit()
            {
                Type = UnitType.Volume,
                Name = "centiliter",
                Abbreviation = "cL",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 100
            };
            Unit cup = new Unit()
            {
                Type = UnitType.Volume,
                Name = "cup",
                Abbreviation = "cup",
                Localisation = Localisation.US,
                Scale = 4.167
            };
            Unit pint = new Unit()
            {
                Type = UnitType.Volume,
                Name = "pint",
                Abbreviation = "pt",
                Localisation = Localisation.US,
                Scale = 2.113
            };

            // Initializing Weight
            Unit gram = new Unit()
            {
                Type = UnitType.Weight,
                Name = "gram",
                Abbreviation = "g",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 1
            };
            Unit kilogram = new Unit()
            {
                Type = UnitType.Weight,
                Name = "kilogram",
                Abbreviation = "kg",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 0.001
            };
            Unit ton = new Unit()
            {
                Type = UnitType.Weight,
                Name = "ton",
                Abbreviation = "t",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 0.000001
            };
            Unit carat = new Unit()
            {
                Type = UnitType.Weight,
                Name = "carat",
                Abbreviation = "ct",
                Localisation = Localisation.RestOfTheWorld,
                Scale = 5
            };

            lstUnits.AddRange(new List<Unit>() { meter, centimeter, kilometer, foot, inch, liter, centiliter, milliliter, cup, pint, gram, kilogram, ton, carat });
        }

        //Trigger the Converter if a Unit is selected in a list
        private void ListUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertUnit();
        }

        //Trigger the converter if the input text has changed
        private void txtInputValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertUnit();
        }

        //Outside of the main functionnality we can implement more security before starting the calculus
        private void ConvertUnit()
        {
            try 
            {
                if(txtInputValue.Text != String.Empty && IsDigitsOnly(txtInputValue.Text) && selectedUnit(fromTabControl).HasValue && selectedUnit(toTabControl).HasValue)
                    ConvertUnit(selectedUnit(fromTabControl), selectedUnit(toTabControl), double.Parse(txtInputValue.Text));
            }
            catch(NullReferenceException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private Unit? selectedUnit(TabControl tc)
        {
            TabItem ti = tc.SelectedItem as TabItem;
            ListBox listBox = ti.Content as ListBox;
            return listBox.SelectedItem as Unit?;
        }

        //cheap double regex
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9' || c == '.' || c == ',' )
                    return false;
            }

            return true;
        }

        // UI control to smoothly switch from UnitType to avoid "mixed Unit Type" problems
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tc = sender as TabControl;
            TabItem ti = tc.SelectedItem as TabItem;
            ListBox listBox = ti.Content as ListBox;
            if (tc == fromTabControl)
            {
                toTabControl.SelectionChanged -= TabControl_SelectionChanged;
                toTabControl.SelectedIndex = tc.SelectedIndex;
                toTabControl.SelectionChanged += TabControl_SelectionChanged;
            }
            else
            {
                fromTabControl.SelectionChanged -= TabControl_SelectionChanged;
                fromTabControl.SelectedIndex = tc.SelectedIndex;
                fromTabControl.SelectionChanged += TabControl_SelectionChanged;
            }
        }

        private void ListListener(bool add)
        {
            foreach(ListBox lstBox in lstListBoxs)
            {
                if (add)
                    lstBox.SelectionChanged += ListUnits_SelectionChanged;
                else
                    lstBox.SelectionChanged -= ListUnits_SelectionChanged;
            }
        }

        private void InitalizeLists()
        {
            listUnitsFromVolume.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Volume);
            listUnitsFromDistance.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Distance);
            listUnitsFromWeight.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Weight);
            listUnitsToVolume.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Volume);
            listUnitsToDistance.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Distance);
            listUnitsToWeight.ItemsSource = LstUnits.Where(x => x.Type == UnitType.Weight);

            lstListBoxs.AddRange(new List<ListBox>() { listUnitsFromVolume, listUnitsFromDistance, listUnitsFromWeight, listUnitsToVolume, listUnitsToDistance, listUnitsToWeight });
            
            ListListener(true);
        }

    }
}
