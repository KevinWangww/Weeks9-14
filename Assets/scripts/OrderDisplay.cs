using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDisplay : MonoBehaviour
{
    public Sprite[] productSprites;         // Array of product images (corresponding numbers)
    private SpriteRenderer spriteRenderer;  // Renderer for displaying images

    // For displaying orders
    // CustomerPosition is the customer's position, on top of which the order is displayed a bit higher up
    // ProductIndex is the customer's order index.
    public void Initialize(Vector2 customerPosition, int productIndex)
    {
        // Setting the display position above the customer.
        transform.position = customerPosition + new Vector2(0, 3f);

        // Set the image to the corresponding product.
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = productSprites[productIndex];
        
    }
}
