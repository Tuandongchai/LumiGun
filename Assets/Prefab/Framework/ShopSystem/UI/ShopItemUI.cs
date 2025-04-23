using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI TitleText, PriceText, DescriptionText;

    [SerializeField] Button button;
    [SerializeField] Image GrayOutCover;

    ShopItem item;

    [SerializeField] Color InEfficientCreditColor;
    [SerializeField] Color SurffiicentCreditCOlor;


    public delegate void OnItemSelected(ShopItemUI selectedItem);

    public event OnItemSelected onItemSelected;

    private void Start()
    {
        button.onClick.AddListener(ItemSelected);
        //DescriptionText.autoSizeTextContainer = true;
    }

    private void ItemSelected()
    {
        onItemSelected?.Invoke(this);
    }

    public void Init(ShopItem item, int AvaliableCredits)
    {
        this.item = item;

        Icon.sprite = item.ItemIcon;
        TitleText.text = item.Title;
        PriceText.text = "$" + item.Price.ToString();
        DescriptionText.text = item.Description;

        Refresh(AvaliableCredits);
    }

    public void Refresh(int avaliableCredits)
    {
        if(avaliableCredits < item.Price)
        {
            GrayOutCover.enabled = true;
            PriceText.color = InEfficientCreditColor;
        }
        else
        {
            GrayOutCover.enabled =false;
            PriceText.color = SurffiicentCreditCOlor;
        }
    }

    internal ShopItem GetItem()
    {
        return item;
    }
}
