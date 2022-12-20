using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace IAP
{
    public class InAppManager : MonoBehaviour, IStoreListener
    {
        [SerializeField] private Renderer _planetRenderer;
        [SerializeField] private SpaceshipController _ship;
        public List<PurchaseItem> PurchaseItems;

        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;

        public const string greenShip = "greenShip";
        public const string redShip = "redShip";
        public const string blueShip = "blueShip";
        public const string purpleShip = "purpleShip";
        public const string lightBlueShip = "lightBlueShip";
        public const string goldShip = "goldShip";

        public const string greenShigp = "greenShipgp";
        public const string redShipgp = "redShipgp";
        public const string blueShipgp = "blueShipgp";
        public const string purpleShipgp = "purpleShipgp";
        public const string lightBlueShipgp = "lightBlueShipgp";
        public const string goldShipgp = "goldShigp";

        public const string greenPlanet = "greenPlanet";
        public const string redPlanet = "redPlanet";
        public const string bluePlanet = "bluePlanet";
        public const string purplePlanet = "purplePlanet";
        public const string lightBluePlanet = "lightBluePlanet";
        public const string goldPlanet = "goldPlanet";

        public const string greenPlanetgp = "greenPlanetgp";
        public const string redPlanetgp = "redPlanetgp";
        public const string bluePlanetgp = "bluePlanetgp";
        public const string purplePlanetgp = "purplePlanetgp";
        public const string lightBluePlanetgp = "lightBluePlanetgp";
        public const string goldPlanetgp = "goldPlanetgp";
        
        public static InAppManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }
            void Start()
        {
            if (m_StoreController == null)
            {
                InitializePurchasing();
            }


        }

        public void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());            
            builder.AddProduct(greenShip, ProductType.NonConsumable, new IDs() { { greenShigp, GooglePlay.Name } });
            builder.AddProduct(redShip, ProductType.NonConsumable, new IDs() { { redShipgp, GooglePlay.Name } });
            builder.AddProduct(blueShip, ProductType.NonConsumable, new IDs() { { blueShipgp, GooglePlay.Name } });
            builder.AddProduct(purpleShip, ProductType.NonConsumable, new IDs() { { purpleShipgp, GooglePlay.Name } });
            builder.AddProduct(lightBlueShip, ProductType.NonConsumable, new IDs() { { lightBlueShipgp, GooglePlay.Name } });
            builder.AddProduct(goldShip, ProductType.NonConsumable, new IDs() { { goldShipgp, GooglePlay.Name } });

            builder.AddProduct(greenPlanet, ProductType.NonConsumable, new IDs() { { greenPlanetgp, GooglePlay.Name } });
            builder.AddProduct(redPlanet, ProductType.NonConsumable, new IDs() { { redPlanetgp, GooglePlay.Name } });
            builder.AddProduct(bluePlanet, ProductType.NonConsumable, new IDs() { { bluePlanetgp, GooglePlay.Name } });
            builder.AddProduct(purplePlanet, ProductType.NonConsumable, new IDs() { { purplePlanetgp, GooglePlay.Name } });
            builder.AddProduct(lightBluePlanet, ProductType.NonConsumable, new IDs() { { lightBluePlanetgp, GooglePlay.Name } });
            builder.AddProduct(goldPlanet, ProductType.NonConsumable, new IDs() { { goldPlanetgp, GooglePlay.Name } });

            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyProductID(string productId)
        {
            try
            {
                if (IsInitialized())
                {
                    Product product = m_StoreController.products.WithID(productId);

                    if (product != null && product.availableToPurchase)
                    {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        m_StoreController.InitiatePurchase(product);
                    }
                    else
                    {
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                else
                {
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            }
            catch (Exception e)
            {
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }

        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("RestorePurchases started ...");

                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result) =>
                {
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            else
            {
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: Completed!");

            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {

            return PurchaseProcessingResult.Complete;
        }

        public void ProcessPurchase(PurchaseItem item)   
        {
            var args = item.ID;

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args));
            if (String.Equals(args, greenShip, StringComparison.Ordinal))
            {
                //Action for money
                _ship.ChangeColor(Color.green);
            }
            else if (String.Equals(args, redShip, StringComparison.Ordinal))
            {
                _ship.ChangeColor(Color.red);
                //Action for no ads
            }
            else if (String.Equals(args, blueShip, StringComparison.Ordinal))
            {
                _ship.ChangeColor(Color.blue);
                //Action for no ads
            }
            else if (String.Equals(args, purpleShip, StringComparison.Ordinal))
            {
                _ship.ChangeColor(new Color(0.5f, 0, 0.5f));
                //Action for no ads
            }
            else if (String.Equals(args, lightBlueShip, StringComparison.Ordinal))
            {
                _ship.ChangeColor(new Color(85 / 255, 170 / 255, 255 / 255));
                //Action for no ads
            }
            else if (String.Equals(args, goldShip, StringComparison.Ordinal))
            {
                //Action for no ads
                _ship.ChangeColor(Color.yellow);
            }
            else if (String.Equals(args, greenPlanet, StringComparison.Ordinal))
            {
                //Action for money
                ChangePlanetColor(Color.green);
            }
            else if (String.Equals(args, redPlanet, StringComparison.Ordinal))
            {
                ChangePlanetColor(Color.red);
                //Action for no ads
            }
            else if (String.Equals(args, bluePlanet, StringComparison.Ordinal))
            {
                ChangePlanetColor(Color.blue);
                //Action for no ads
            }
            else if (String.Equals(args, purplePlanet, StringComparison.Ordinal))
            {
                ChangePlanetColor(new Color(0.5f, 0, 0.5f));
                //Action for no ads
            }
            else if (String.Equals(args, lightBluePlanet, StringComparison.Ordinal))
            {
                ChangePlanetColor(new Color(85 / 255, 170 / 255, 255 / 255));
                //Action for no ads
            }
            else if (String.Equals(args, goldPlanet, StringComparison.Ordinal))
            {
                //Action for no ads
                ChangePlanetColor(Color.yellow);
            }


            //return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }


        private void ChangePlanetColor(Color color)
        {
            _planetRenderer.material.color = color;

        }
    }

}