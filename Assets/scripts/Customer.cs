using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// The script is responsible for timing, handling player clicks, and generating order display objects.
public class Customer : MonoBehaviour
{
    public int orderType;                             // Customer's order index number
    public GameObject orderDisplayPrefab;             // The preform use to showing order icons

    private GameObject orderDisplayInstance;          // This customer's product display object
    private float timeElapsed = 0f;                   // Customer waiting time

    private CustomerManager customerManager;          // Reference to CustomerManager for report on clicks
    public Customer customer;                         // Use to store itself
    void Start()
    {
        orderType = Random.Range(0, 3);               // Random order numbers from 0 to 2

        StartCoroutine(WaitForTimeout());             // Start calculating wait time
        CreateOrderDisplay();                         // Display The order

        customer = GetComponent<Customer>();          // Get your own customer component
    }

    // Use to allow manager to send itself to a new customer prefab as soon as it is generated.
    public void SetCustomerManager(CustomerManager manager)
    {
        customerManager = manager; // Assignment to local variable
    }

    // Timer, customers leave when time expires
    IEnumerator WaitForTimeout()
    {
        while (timeElapsed < 8f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        customerManager.RemoveCustomerFromListeners(this);  // Calling the Remove Listener method in Manager
        StartCoroutine(ShrinkAndDestroy());                 // Make the customer object smaller and then destroy it
    }

    // An animation of the object gradually shrinking and then destroying the
    IEnumerator ShrinkAndDestroy()
    {
        float i = 1f;
        float process = 0f;
        Vector3 originalScale = transform.localScale;

        // Progress is not yet complete.
        while (process < i)
        {
            float t = process / i;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            process += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Destroy the object
    }

    // Generate an order display object above the customer and display the corresponding picture
    private void CreateOrderDisplay()
    {
        // Generate order display object
        orderDisplayInstance = Instantiate(orderDisplayPrefab);

        // Import customer location and order number
        OrderDisplay display = orderDisplayInstance.GetComponent<OrderDisplay>();
        display.Initialize(transform.position, orderType);
    }

    // Trigger serving logic when player clicks on customer
    // Determine whether a customer has been clicked by calculating the distance from the customer object at the time of the mouse click.
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the position of the mouse click in world space
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the distance between the mouse and the customer
            float distance = Vector2.Distance(mouseWorldPos, transform.position);

            // Customer is considered to be selected if they are within the click radius.
            float clickRadius = 1f;
            if (distance < clickRadius)
            {
                // Handle clicks in a unified way through the Manager and return the results
                bool success = customerManager.NotifyCustomerClicked(customer);

                if (success)
                {
                    
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("product error");
                }
            }
        }
    }

    // Returns the customer's order to the customer manager
    public int GetOrderType()
    {
        return orderType;
    }

    // Provide wait time to external (used to determine if the order is completed quickly)
    public float GetElapsedTime()
    {
        return timeElapsed;
    }

    // Objects dedicated to deleting display orders
    void OnDestroy()
    {
        Destroy(orderDisplayInstance);
    }
}
