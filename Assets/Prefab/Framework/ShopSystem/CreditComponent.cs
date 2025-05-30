using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    public bool HandlePurchase(Object newPurchase);
}
public class CreditComponent : MonoBehaviour, IRewardListener
{
    [SerializeField] int credits;
    [SerializeField] Component[] PurchaseListeners;

    List<IPurchaseListener> purchaseListenerInterfaces = new List<IPurchaseListener>();

    private void Start()
    {
        CollectPurchaseListeners();

    }
     
    private void CollectPurchaseListeners()
    {
        foreach(Component listener  in PurchaseListeners)
        {
            IPurchaseListener listenerInterface = listener as IPurchaseListener;
            if(listenerInterface != null)
            {
                purchaseListenerInterfaces.Add(listenerInterface);
            }
        }
    }
    private void BroadCasePurchase(Object item)
    {
        foreach(IPurchaseListener purchaseListener in purchaseListenerInterfaces)
        {
            if (purchaseListener.HandlePurchase(item))
                return;
        }
    }

    /*public int Credit { get; private set; }*/
    public int Credit()
    {
        return credits;
    }
    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;

    public bool Purchase(int price, Object item)
    {
        if(credits<price) return false;
        credits -= price;
        onCreditChanged?.Invoke(credits);

        BroadCasePurchase(item); 

        return true;
    }

    public void Reward(Reward reward)
    {
        credits += reward.creditReward;
        onCreditChanged.Invoke(credits);
    }

    
}
