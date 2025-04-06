using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// 顾客行为脚本
// 功能：计时、处理玩家点击上菜、生成订单显示对象。
// 使用者：CustomerManager.cs（调用 StartTimer）、玩家点击（OnMouseDown）
[System.Serializable]
public class CustomerClickEvent : UnityEvent<Customer> { }
public class Customer : MonoBehaviour
{
    public int orderType;                             // 顾客所点商品编号（0,1,2）
    public GameObject orderDisplayPrefab;             // 显示订单用的UI预制体

    private GameObject orderDisplayInstance;          // 实例化出来的订单UI对象
    private float timeElapsed = 0f;                   // 顾客已等待的时间
    private bool served = false;                      // 顾客是否已被服务

    private CustomerManager customerManager;          // 顾客管理器，用于上报点击事件

    // Unity Start生命周期函数
    // 在顾客生成后启动计时器并显示订单图标
    // 调用：Unity引擎自动调用
    void Start()
    {
        StartCoroutine(WaitForTimeout());
        CreateOrderDisplay();

        // 自动查找场景中的 CustomerManager（也可以手动拖入）
        if (customerManager == null)
        {
            customerManager = FindObjectOfType<CustomerManager>();
        }
    }

    // 顾客等待协程：10秒未被服务就离开
    // 调用：Start() 内部调用
    IEnumerator WaitForTimeout()
    {
        while (timeElapsed < 10f && !served)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (!served)
        {
            Destroy(gameObject); // 超时离开
            Debug.Log("顾客超时离开");
        }
    }

    // 在顾客上方生成订单显示对象，并显示对应图片
    // 使用者：StartTimer()
    private void CreateOrderDisplay()
    {
        // 生成订单显示对象
        orderDisplayInstance = Instantiate(orderDisplayPrefab);
        orderDisplayInstance.GetComponent<SpriteRenderer>().enabled = true;

        // 初始化位置和图片（传入顾客位置和订单编号）
        OrderDisplay display = orderDisplayInstance.GetComponent<OrderDisplay>();
        display.Initialize(transform.position, orderType);
    }

    private void Update()
    {
        // 当玩家点击左键
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标点击在世界空间中的位置（z = 0 用于2D）
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 计算鼠标与顾客的距离
            float distance = Vector2.Distance(mouseWorldPos, transform.position);

            // 如果在点击半径范围内，视为选中顾客
            float clickRadius = 0.5f;
            if (distance < clickRadius)
            {
                OnMouseDown();
            }
        }
    }


    void OnMouseDown()
    {
        if (!served && customerManager != null)
        {
            // 向 CustomerManager 上报：我被点击了
            customerManager.OnCustomerClicked(this); // 使用 CustomerManager 的方法
        }
    }


    // 被 CustomerManager 调用，当顾客服务成功后执行销毁
    // 调用者：CustomerManager
    public void MarkServed()
    {
        served = true;
        Destroy(gameObject); // 顾客离开
    }

    // 获取顾客当前的订单编号
    // 调用者：CustomerManager
    public int GetOrderType()
    {
        return orderType;
    }

    // 可选方法：外部在生成顾客后手动设置管理器引用
    // 调用者：CustomerManager
    public void SetCustomerManager(CustomerManager manager)
    {
        customerManager = manager;
    }

    // 顾客销毁时一并销毁订单图标
    void OnDestroy()
    {
        if (orderDisplayInstance != null)
        {
            Destroy(orderDisplayInstance);
        }
    }
}
