using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataHandler : MonoBehaviour
{
    private GameObject furniture;
    private double price;
    private Item i;
    [SerializeField] private ButtonManager buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] public List<Item> items;

    private int current_id = 0;

    private static DataHandler instance;
    public static DataHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
    }

    private void Start()
    {
        LoadItems();
        CreateButtons();
        
    }

    void LoadItems()
    {
        var items_obj = Resources.LoadAll("Items", typeof(Item));
        foreach (var item in items_obj)
        {
            items.Add(item as Item);
        }
    }

    void CreateButtons()
    {
        foreach (Item i in items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.ItemID = current_id;
            b.ButtonTexture = i.itemImage;
            items[current_id].id = current_id;
            current_id++;
        }

        buttonContainer.GetComponent<UIContentFitter>().Fit();
    }

    public void SetFurniture(int id)
    {
        furniture = items[id].itemPrefab;
        
    }

    public void SetPrice(int id)
    {
        price = items[id].price;
    }

    public GameObject GetFurniture()
    {
        return furniture;
    }

    public double GetPrice()
    {
        return price;
    }

    public double GetPrice(int id)
    {
        return items[id].price;
    }

}
