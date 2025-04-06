using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerPrefabs;               // 顾客预制体数组（每种类型一个）
    public float spawnInterval = 2.0f;                 // 每隔多少秒生成一个顾客

    private ScoreManager scoreManager;                 // 分数系统引用（用于加分）
    private ProductManager productManager;             // 商品系统引用（用于判断是否选对）
    private List<Customer> activeCustomers = new List<Customer>();  // 当前在场顾客列表

    // Unity生命周期函数，初始化并启动生成顾客协程
    // 调用者：Unity引擎
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();         // 获取 ScoreManager
        productManager = FindObjectOfType<ProductManager>();     // 获取 ProductManager
        StartCoroutine(SpawnCustomers());
    }

    // 每隔一段时间生成一个顾客
    // 调用者：Start() 协程
    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            GenerateCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 实际生成顾客的方法
    // 调用者：SpawnCustomers()
    void GenerateCustomer()
    {
        int index = Random.Range(0, customerPrefabs.Length);
        Vector3 position = GetRandomPosition();

        GameObject customerObj = Instantiate(customerPrefabs[index], position, Quaternion.identity);
        Customer customer = customerObj.GetComponent<Customer>();

        customer.SetCustomerManager(this); // 顾客内部会保存引用
        activeCustomers.Add(customer);
    }

    // 由顾客调用（被点击时），统一判断是否选对商品
    // 调用者：Customer.OnMouseDown()
    public void OnCustomerClicked(Customer customer)
    {
        int selected = productManager.GetCurrentProduct();  // 当前玩家选择的商品
        int order = customer.GetOrderType();                // 顾客的订单编号

        if (selected == order)
        {
            Debug.Log("订单正确，加分");
            scoreManager.AddScore(1);                       // 调用 ScoreManager
            customer.MarkServed();                          // 顾客离开
        }
        else
        {
            Debug.Log("订单错误");
        }

        activeCustomers.Remove(customer);
    }

    // 顾客生成位置的随机函数
    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-4f, 4f);
        float y = Random.Range(-2f, 2f);
        return new Vector3(x, y, 0);
    }
}
