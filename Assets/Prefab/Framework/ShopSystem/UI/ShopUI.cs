using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] ShopItemUI shopItemUIPrefab;
    [SerializeField] RectTransform shopList;
    [SerializeField] CreditComponent creditComp;
    [SerializeField] UIManager uiManager;

    [SerializeField] TextMeshProUGUI creditText;

    [SerializeField] Button BackBtn;
    [SerializeField] Button BuyBtn;

    List<ShopItemUI> shopItems = new List<ShopItemUI>();

    ShopItemUI selectedItem;

    private void Start()
    {
        InitShopItem();
        BackBtn.onClick.AddListener(uiManager.SwithToGameplayUI);
        BuyBtn.onClick.AddListener(TryPurchaseItem);
        creditComp.onCreditChanged += UpdateCredit;
        UpdateCredit(creditComp.Credit());
    }

    private void TryPurchaseItem()
    {
        if (!selectedItem || !shopSystem.TryPurchase(selectedItem.GetItem(), creditComp))
            return;
        RemoveItem(selectedItem); 
    }

    private void RemoveItem(ShopItemUI itemToRemove)
    {
        shopItems.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }

    private void UpdateCredit(int newCredit)
    {
        creditText.SetText(newCredit.ToString());
        RefreshItems();
    }

    private void RefreshItems()
    {
        foreach(ShopItemUI shopItemUI in shopItems)
        {
            shopItemUI.Refresh(creditComp.Credit());
        }
    }

    private void InitShopItem()
    {
        ShopItem[] shopItems = shopSystem.GetShopItems();
        foreach(ShopItem item in shopItems)
        {
            AddShopItem(item);
        }
    }

    private void AddShopItem(ShopItem item)
    {
        ShopItemUI newItemUI = Instantiate(shopItemUIPrefab, shopList);
        newItemUI.Init(item, creditComp.Credit());
        newItemUI.onItemSelected += ItemSelected;
        shopItems.Add(newItemUI);
    }

    private void ItemSelected(ShopItemUI Item)
    {
        selectedItem = Item;
    }
}
