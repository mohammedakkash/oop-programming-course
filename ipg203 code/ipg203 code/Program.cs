
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
