using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace MatReqs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<RecipeItem> _recipes = new List<RecipeItem>();
        private List<BasicItem> _basicItems = new List<BasicItem>();
        private List<Machine> _machines = new List<Machine>();
        private int _amountOfItemToCalculate = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitMatReqs();

            FillUpLists();
        }

        private void InitMatReqs()
        {
            txt_InputAmountOfItems.Text = _amountOfItemToCalculate.ToString();
        }

        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            List<CalculatedItem> returnedItems = new List<CalculatedItem>();

            // Grab input value
            _amountOfItemToCalculate = int.Parse(txt_InputAmountOfItems.Text);
            
            // Fill our list with the correct items for exactly one recipe
            returnedItems.AddRange(_recipes.Find(recipe => recipe.Title == "Redstone Chipset").ReturnListOfItems());

            // Calculate resulting amounts based on multiplier _amountOfItemToCalculate
            if (_amountOfItemToCalculate > 0)
            {
                foreach(CalculatedItem recipe in returnedItems)
                {
                    recipe.Amount = recipe.Amount * _amountOfItemToCalculate;
                }
            }

            txtBlock_MiscValues.Text = _recipes.Find(reci => reci.Title == "Redstone Chipset").ReturnMiscInfo(_amountOfItemToCalculate);

            listBox_CalculatedResult.ItemsSource = returnedItems;
        }

        private void FillUpLists()
        {
            FillUpBasicItems();
            FillUpMachineList();
            FillUpRecipes();
        }

        private void FillUpRecipes()
        {
            RecipeItem newRecipe = new RecipeItem();
            newRecipe.Title = "Redstone Chipset";
            newRecipe.AddItem(_basicItems.Find(item => item.ItemId == ItemIDs.RedstoneDust), 1);
            newRecipe.RequiresMachines = true;
            newRecipe.AddMachine(_machines.Find(item => item.Id == MachineIds.AssemblyTable));
            newRecipe.AddRF(100000);
            _recipes.Add(newRecipe);
        }

        private void FillUpBasicItems()
        {
            _basicItems.Add(new BasicItem("Redstone Dust", ItemIDs.RedstoneDust, GameDimension.Overworld));
        }

        private void FillUpMachineList()
        {
            _machines.Add(new Machine("Assembly Table", MachineIds.AssemblyTable, Mod.BCSilicon));
        }

        private void txt_InputAmountOfItems_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Makes sure only integers are accepted
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    #region Item Classes
    public class RecipeItem
    {
        public string Title { get; set; }
        public bool RequiresMachines { get; set; }
        public int RFRequired { get; set; }
        public Dictionary<BasicItem, int> ItemsRequired;
        public List<Machine> MachinesRequired;

        public RecipeItem()
        {
            ItemsRequired = new Dictionary<BasicItem, int>();
            MachinesRequired = new List<Machine>();
        }

        public void AddItem(BasicItem basicItem, int amount)
        {
            ItemsRequired.Add(basicItem, amount);
        }

        public void AddMachine(Machine machine)
        {
            MachinesRequired.Add(machine);
        }

        public void AddRF(int rfRequired)
        {
            RFRequired += rfRequired;
        }

        public List<CalculatedItem> ReturnListOfItems()
        {
            List<CalculatedItem> resultingList = new List<CalculatedItem>();
            
            foreach (KeyValuePair<BasicItem, int> pair in ItemsRequired)
            {
                BasicItem item = pair.Key;
                int amount = pair.Value;
                CalculatedItem newItem = new CalculatedItem();

                newItem.Title = item.Title;
                newItem.Amount = amount;
                newItem.InInventory = false;

                resultingList.Add(newItem);
            }
            
            return resultingList;
        }

        public string ReturnMiscInfo(int amountOfItems)
        {
            string resultingString = "";
            
            resultingString += string.Format("Machines Required: " + (RequiresMachines ? "Yes" : "No"));

            if (RequiresMachines)
            {
                resultingString += string.Format("\n\nMachines Required:");

                foreach (Machine m in MachinesRequired)
                {
                    resultingString += string.Format("\n" + m.Title);
                }
                
                resultingString += string.Format("\n\nTotal RF required: {0:0,0}", amountOfItems > 1 ? RFRequired * amountOfItems : RFRequired);
            }
            
            return resultingString;
        }
    }

    public class BasicItem
    {
        public string Title { get; set; }
        public ItemIDs ItemId { get; set; }
        public GameDimension DimensionToGetItem { get; set; }

        public BasicItem(string title, ItemIDs newID, GameDimension dimension)
        {
            Title = title;
            DimensionToGetItem = dimension;
            ItemId = newID;
        }
    }
    
    public class CalculatedItem
    {
        public string Title { get; set; }
        public int Amount { get; set; }
        public bool InInventory { get; set; }
    }

    public class Machine
    {
        public string Title { get; set; }
        public MachineIds Id { get; set; }
        public Mod BelongingToMod { get; set; }

        public Machine(string title, MachineIds id, Mod mod)
        {
            Title = title;
            Id = id;
            BelongingToMod = mod;
        }
    }
    #endregion

    #region Data classes
    public enum Mod
    {
        EnderIO,
        ThermalExpansion,
        IndustrialCraft,
        AE2,
        ImmersiveEngineering,
        Forestry,
        BCSilicon,
        Railcraft
    }
    public enum GameDimension
    {
        Overworld,
        Nether,
        End,
        TwilightForest
    }
    public enum ItemIDs
    {
        RedstoneDust,
        GlowstoneDust
    }
    public enum MachineIds
    {
        AssemblyTable,          // BC Silicon
        AlloySmelter,           // EnderIO
        SAGMill,                // EnderIO
        Pulverizer,             // TE
        ElectricFurnace,        // IC2
        RedstoneFurnace,        // TE
        MetalFormer,            // IC2 
        Compressor,             // IC2
        TheVAT,                 // EnderIO
        RollingMachine,         // Railcraft
        ThermionicFabricator,   // Forestry
        MagmaCrucible,          // TE
        FluidTransposer,        // TE
        InductionSmelter,       // TE
        Carpenter               // Forestry
    }
    #endregion
}