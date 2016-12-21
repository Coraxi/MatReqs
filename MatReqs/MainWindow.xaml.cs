using System.Collections.Generic;
using System.Windows;

namespace MatReqs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CalculatedItem> _resultingItems = new List<CalculatedItem>();
        private List<RecipeItem> _recipes = new List<RecipeItem>();
        private List<BasicItem> _basicItems = new List<BasicItem>();
        private List<Machine> _machines = new List<Machine>();

        public MainWindow()
        {
            InitializeComponent();

            FillUpLists();
        }

        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            _resultingItems.AddRange(_recipes.Find(reci => reci.Title == "Redstone Chipset").ReturnListOfItems());
            txtBlock_MiscValues.Text = _recipes.Find(reci => reci.Title == "Redstone Chipset").ReturnMiscInfo();

            listBox_CalculatedResult.ItemsSource = _resultingItems;
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

        public string ReturnMiscInfo()
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
                
                resultingString += string.Format("\n\nTotal RF required: {0:0,0}", RFRequired);
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