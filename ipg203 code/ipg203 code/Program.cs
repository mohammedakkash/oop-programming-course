
using System;
using System.Collections.Generic;

namespace AssetManagementSystem
{

   //============================================================
    // 1. ABSTRACTION: Interface Definition
    //============================================================

    /// <summary>
    /// Defines the essential contract for any asset.
    /// An interface ensures that any class implementing it must provide this functionality.
    /// </summary>
    public interface IAsset
    {
        string GetAssetSummary();
    }

    //============================================================
    // 2. ABSTRACTION & ENCAPSULATION: Abstract Base Class
    //============================================================

    /// <summary>
    /// Represents a general asset with common properties and behaviors.
    /// It's abstract, so it cannot be instantiated directly.
    /// It implements the IAsset interface.
    /// </summary>
    public abstract class Asset : IAsset
    {
        // Static property to count total number of assets created.
        // It belongs to the class itself, not to any single instance.
        public static int TotalAssets { get; private set; }

        // Private field for the ID to enforce read-only behavior after creation.
        // This is a core part of encapsulation.
        private readonly string _assetId;

        // Public property for AssetID (read-only after creation).
        // Provides controlled access to the private field.
        public string AssetId => _assetId;

        // Public property for PurchaseDate.
        public DateTime PurchaseDate { get; set; }

        // Public property for Name.
        public string Name { get; set; }

        /// <summary>
        /// Constructor for the base class. It's 'protected' so only derived classes can call it.
        /// </summary>
        protected Asset(string name, DateTime purchaseDate)
        {
            // A unique ID is generated upon creation and cannot be changed.
            this._assetId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            this.Name = name;
            this.PurchaseDate = purchaseDate;

            // Increment the static counter each time a new asset is created.
            TotalAssets++;
        }

        // An abstract method. It has no implementation here.
        // Derived classes MUST provide their own implementation.
        public abstract string GetMaintenanceDetails();

        // A virtual method from the IAsset interface.
        // It has a default implementation that can be overridden by derived classes.
        public virtual string GetAssetSummary()
        {
            return $"ID: {AssetId}, Name: {Name}, Purchased: {PurchaseDate.ToShortDateString()}";
        }
    }

    //============================================================
    // 3. INHERITANCE & POLYMORPHISM: Concrete Derived Classes
    //============================================================

    /// <summary>
    /// Represents a specific type of asset: Laptop. Inherits from Asset.
    /// </summary>
    public class Laptop : Asset
    {
        public string CpuType { get; set; }
        public int RamInGB { get; set; }

        public Laptop(string name, DateTime purchaseDate, string cpuType, int ramInGB)
            : base(name, purchaseDate) // Call the base class constructor.
        {
            this.CpuType = cpuType;
            this.RamInGB = ramInGB;
        }

        // Overriding the abstract method to provide a specific implementation for Laptop.
        public override string GetMaintenanceDetails()
        {
            return "Maintenance: Check battery health and update software annually.";
        }

        // Overriding the virtual method to add more specific details.
        public override string GetAssetSummary()
        {
            return $"{base.GetAssetSummary()}, CPU: {CpuType}, RAM: {RamInGB}GB";
        }
    }

    /// <summary>
    /// Represents a specific type of asset: Monitor. Inherits from Asset.
    /// </summary>
    public class Monitor : Asset
    {
        public int ScreenSizeInches { get; set; }
        public string Resolution { get; set; }

        public Monitor(string name, DateTime purchaseDate, int screenSize, string resolution)
            : base(name, purchaseDate)
        {
            this.ScreenSizeInches = screenSize;
            this.Resolution = resolution;
        }

        // Overriding the abstract method.
        public override string GetMaintenanceDetails()
        {
            return "Maintenance: Clean the screen and check for dead pixels every 6 months.";
        }

        // Overriding the virtual method.
        public override string GetAssetSummary()
        {
            return $"{base.GetAssetSummary()}, Size: {ScreenSizeInches}\", Resolution: {Resolution}";
        }
    }

    /// <summary>
    /// Represents another specific type of asset: OfficeChair. Inherits from Asset.
    /// </summary>
    public class OfficeChair : Asset
    {
        public string Material { get; set; }

        public OfficeChair(string name, DateTime purchaseDate, string material)
            : base(name, purchaseDate)
        {
            this.Material = material;
        }

        // Overriding the abstract method.
        public override string GetMaintenanceDetails()
        {
            return "Maintenance: Check mechanical parts and clean fabric quarterly.";
        }

        // Overriding the virtual method.
        public override string GetAssetSummary()
        {
            return $"{base.GetAssetSummary()}, Material: {Material}";
        }
    }

    //============================================================
    // 4. DELEGATES & EVENTS: The Asset Manager
    //============================================================

    /// <summary>
    /// A delegate defines the signature for a method that can handle notifications.
    /// </summary>
    public delegate void AssetNotificationHandler(string message);

    /// <summary>
    /// Manages a collection of assets and demonstrates polymorphism and events.
    /// </summary>
    public class AssetManager
    {
        // The event that other classes can subscribe to.
        public event AssetNotificationHandler AssetNearingEOL;

        // A list to hold various types of assets, demonstrating polymorphism.
        private readonly List<Asset> _assets;

        public AssetManager()
        {
            _assets = new List<Asset>();
        }

        public void AddAsset(Asset asset)
        {
            if (asset != null)
            {
                _assets.Add(asset);
                Console.WriteLine($"-> Added Asset: {asset.Name}");
            }
        }

        /// <summary>
        /// This method demonstrates polymorphism. It calls methods on each asset,
        /// and the correct version of the method is executed at runtime based on the object's actual type.
        /// </summary>
        public void PrintAllAssetSummaries()
        {
            Console.WriteLine("\n--- All Company Assets ---");
            foreach (var asset in _assets)
            {
                // The correct GetAssetSummary() override is called here.
                Console.WriteLine(asset.GetAssetSummary());
            }
            Console.WriteLine("--------------------------\n");
        }

        public void PrintAllMaintenanceDetails()
        {
            Console.WriteLine("\n--- Asset Maintenance Schedule ---");
            foreach (var asset in _assets)
            {
                // The correct GetMaintenanceDetails() override is called here.
                Console.WriteLine($"Asset: {asset.Name} ({asset.AssetId}) -> {asset.GetMaintenanceDetails()}");
            }
            Console.WriteLine("----------------------------------\n");
        }

        /// <summary>
        /// A method that checks a condition and raises an event.
        /// </summary>
        public void CheckAssetsEOL()
        {
            Console.WriteLine("--- Checking Assets for End-of-Life (EOL) ---");
            foreach (var asset in _assets)
            {
                // Condition: If asset is older than 3 years.
                if (asset.PurchaseDate < DateTime.Now.AddYears(-3))
                {
                    // Raise the event if there are subscribers.
                    OnAssetNearingEOL($"Warning: Asset '{asset.Name}' (ID: {asset.AssetId}) has passed its 3-year EOL mark.");
                }
            }
            Console.WriteLine("---------------------------------------------\n");
        }

        /// <summary>
        /// Protected virtual method to raise the event. This is a common pattern.
        /// </summary>
        protected virtual void OnAssetNearingEOL(string message)
        {
            // Check if there are any subscribers to the event before invoking it.
            AssetNearingEOL?.Invoke(message);
        }
    }
    

    //============================================================
    // 5. STATIC CLASS & MEMBERS
    //============================================================

    /// <summary>
    /// A static class contains only static members and cannot be instantiated.
    /// It's useful for utility functions.
    /// </summary>
    public static class AssetValidator
    {
        // A static method to validate an asset's name.
        public static bool IsValidAssetName(string name)
        {
            // Simple validation: name should not be null or whitespace.
            return !string.IsNullOrWhiteSpace(name);
        }
    }

    //============================================================
    // Main Program Entry Point
    //============================================================

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Asset Management System...\n");

            // 1. Create an AssetManager instance.
            var manager = new AssetManager();

            // 2. Subscribe a method to the manager's event.
            // Now, HandleEolNotification will be called when the event is raised.
            manager.AssetNearingEOL += HandleEolNotification;

            // 3. Create different types of assets.
            // One asset is old (purchased in 2020) to trigger the EOL event.
            var laptop1 = new Laptop("Dev Laptop", new DateTime(2024, 1, 15), "Intel i7", 16);
            var monitor1 = new Monitor("Main Monitor", new DateTime(2023, 5, 20), 27, "4K");
            var chair1 = new OfficeChair("Ergo Chair", new DateTime(2024, 2, 10), "Mesh");
            var oldLaptop = new Laptop("Old HP", new DateTime(2020, 10, 1), "Intel i5", 8);

            // Use the static validation class before adding an asset.
            if (AssetValidator.IsValidAssetName(laptop1.Name))
            {
                manager.AddAsset(laptop1);
            }
            manager.AddAsset(monitor1);
            manager.AddAsset(chair1);
            manager.AddAsset(oldLaptop);

            Console.WriteLine();

            // 4. Demonstrate Polymorphism by calling methods on the list of assets.
            manager.PrintAllAssetSummaries();
            manager.PrintAllMaintenanceDetails();

            // 5. Trigger the condition that raises the event.
            manager.CheckAssetsEOL();

            // 6. Display the static property value from the base Asset class.
            Console.WriteLine($"Total number of assets created: {Asset.TotalAssets}");

            Console.WriteLine("\nSystem finished.");
        }

        /// <summary>
        /// This is the event handler method. It matches the signature of the AssetNotificationHandler delegate.
        /// </summary>
        public static void HandleEolNotification(string message)
        {
            // Change console color to highlight the event notification.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[EVENT RECEIVED] {message}");
            Console.ResetColor();
        }
    }
}




