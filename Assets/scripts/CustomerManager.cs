using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerPrefabs;                       // Customer prefabs
    public float spawnInterval = 2.0f;                         // Waiting time to spawn new customer

    private List<Customer> listeners = new List<Customer>();   // Customer listener list
    public ScoreManager scoreManager;                          // Script that displays scores, use to invoke adding score when completing an order.
    public ProductManager productManager;                      // Use to get the item currently selected by the player.
    public CustomerManager customerManager;                    // itself, use to assign itself to the new generated customer.

    void Start()
    {
        StartCoroutine(SpawnCustomers()); // Start spawning customers
        customerManager = GetComponent<CustomerManager>(); 
    }

    IEnumerator SpawnCustomers() // Generate customer coroutine
    {
        while (true)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnInterval); // Wait sometime and continue
        }
    }

    void SpawnCustomer() // Spawn a customer
    {
        int index = Random.Range(0, customerPrefabs.Length);                                 // Randomly pick a customer's prefab

        float x = Random.Range(-4f, 4f);
        float y = Random.Range(-1f, 1f);
        Vector2 position = new Vector2(x, y);                                                // Get a random location

        GameObject obj = Instantiate(customerPrefabs[index], position, Quaternion.identity); // Instantiate a new customer
        Customer customer = obj.GetComponent<Customer>();                                    // Find the script for this customer

        customer.SetCustomerManager(customerManager);                                        // Transfers itself to this new customer so that the customer can invoke the methods of the script.
        listeners.Add(customer);                                                             // Adding a customer to the listener list
    }

    // Called when a customer is clicked, it determines whether it is correct and returns whether the service was successful.
    public bool NotifyCustomerClicked(Customer customer)
    {
        // Check if it is in the list of listeners.
        // Used to avoid a situation where a customer continues to be clicked after being removed from the listener.
        if (!listeners.Contains(customer))
        {
            return false;
        }

        int selected = productManager.GetCurrentProduct();    // See which item is now selected by the player.
        int order = customer.GetOrderType();                  // Get the items ordered by the customer.

        float elapsed = customer.GetElapsedTime();            // Getting the waiting time

        // If the selection matches the order.
        if (selected == order)
        {
            if (elapsed < 2f)         // If the time is within 2 seconds
            {
                scoreManager.AddScore(1);
            }
            scoreManager.AddScore(1); // Add 1 score
        }

        // Removes the customer from the list of listeners and returns the result.
        listeners.Remove(customer);
        return (selected == order); 
    }

    // Used to remove yourself from the listener when the wait time has expired, called by the customer.
    public void RemoveCustomerFromListeners(Customer customer)
    {
        listeners.Remove(customer);
    }
}