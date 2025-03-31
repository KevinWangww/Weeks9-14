using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerPrefabs; // 可生成的顾客预制体数组
    public float spawnInterval = 2.0f;   // 生成顾客的间隔时间（秒）

    // 游戏开始时启动协程持续生成顾客
    private void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    // 协程：每隔 spawnInterval 秒生成一个顾客
    // 使用者：自身 Start()
    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 随机选择一个顾客类型并生成到场景
    // 使用者：SpawnCustomers()
    void SpawnCustomer()
    {
        int index = Random.Range(0, customerPrefabs.Length);
        int order = Random.Range(0, 3);
        Vector2 position = GetRandomPosition();
        GameObject customer = Instantiate(customerPrefabs[index], position, default);
        customer.GetComponent<SpriteRenderer>().enabled = true;

        Customer customerScript = customer.GetComponent<Customer>();
        customerScript.StartTimer(); // 启动该顾客的计时器（来自 Customer.cs）
    }

    // 为顾客生成一个随机位置（静止）
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(-4f, 4f);
        float y = Random.Range(-2f, 2f);
        return new Vector2(x, y);
    }
}
