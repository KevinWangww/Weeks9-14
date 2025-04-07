using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductManager : MonoBehaviour
{
    public List<Button> productButtons = new List<Button>();  // Button list

    private int currentProduct = 0;                           // Currently selected item number

    void Start()
    {
        // Add listener to each button (using the index as the item number)
        for (int i = 0; i < productButtons.Count; i++)
        {
            int index = i;
            productButtons[index].onClick.AddListener(() => OnProductButtonClicked(index)); // Buttons add listener
        }
    }

    // Call when the button is clicked, updating the current item number
    public void OnProductButtonClicked(int index)
    {
        currentProduct = index;
    }

    // Provide the current item number to the CustomerManager.
    public int GetCurrentProduct()
    {
        return currentProduct;
    }
}
