using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void MouseOn()
    {
        buttonImage.color = Color.gray;    // Mouse over buttons with grayed out colors.
    }

    public void MouseExit()
    {
        buttonImage.color = Color.white;   // Mouse over button to restore original color
    }
}
