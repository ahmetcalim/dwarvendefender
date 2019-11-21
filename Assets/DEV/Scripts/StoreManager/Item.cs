using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int wIndex;
    public string name;
    public GameObject prefab;
    public int cost;
    public enum StoreState {AVAILABLE, NOT_AVAILABLE, PURCHASED}
    public StoreState storeState;
    public Text nameText;
    public Text costText;
    private CurrentPlayer currentPlayer;
    public Button purchaseBtn;
    private void Start()
    {
        currentPlayer = FindObjectOfType<CurrentPlayer>();
        if (nameText)
        {
            nameText.text = name;
        }
    }
    private void LateUpdate()
    {
        if (storeState == StoreState.PURCHASED)
        {
            LogState(Color.blue);
            costText.text = "Purchased";
            purchaseBtn.enabled = false;
            if (costText.gameObject.activeSelf)
            {
                costText.gameObject.SetActive(false);
            }
        }
        else
        {
            switch (storeState)
            {
                case StoreState.AVAILABLE:
                    LogState(Color.green);
                    purchaseBtn.enabled = true;
                    costText.text = cost.ToString();
                    CheckCost();
                    break;
                case StoreState.NOT_AVAILABLE:
                    LogState(Color.red);
                    purchaseBtn.enabled = false;
                    costText.text = cost + " Not Enough Resource";
                    CheckCost();
                    break;
                default:
                    break;
            }
        }
       
       
    }
    private void LogState(Color color)
    {
        GetComponent<Image>().color = color;
    }
    private void CheckCost()
    {
        if (currentPlayer.resource >= cost)
        {
            storeState = StoreState.AVAILABLE;
        }
        else
        {
            storeState = StoreState.NOT_AVAILABLE;
        }
    }
}
