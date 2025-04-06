using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 顾客行为脚本
// 功能：计时、处理玩家点击上菜、生成订单显示对象。
// 使用者：CustomerManager.cs（调用 StartTimer）、玩家点击（OnMouseDown）
[System.Serializable]
public class BonusScoreEvent : UnityEvent<int> { }

public class Customer : MonoBehaviour
{
    public int orderType;                     // 顾客要求的商品编号
    public UnityEvent onServeSuccess;         // 上菜成功事件（如加分）
    public BonusScoreEvent onQuickServe;      // 快速上菜事件（<5秒加额外分）

    public GameObject orderDisplayPrefab;     // 订单显示对象的预制体
    private GameObject orderDisplayInstance;  // 订单显示对象实例

    public ProductManager productManager;

    private float timeElapsed = 0;            // 用于计时

    // 启动计时器，并创建订单显示对象
    // 使用者：CustomerManager.cs（在 SpawnCustomer 中调用）
    public void StartTimer()
    {
        orderType = Random.Range(0, 3); // 0 到 2

        StartCoroutine(WaitForTimeout());
        CreateOrderDisplay();
    }

    // 每帧计时，超过10秒未上菜则顾客离开
    IEnumerator WaitForTimeout()
    {
        while (timeElapsed < 10f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject); // 超时销毁顾客
        Debug.Log("顾客超时离开");
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

    // 玩家点击顾客时触发上菜逻辑
    // 使用者：Unity 引擎自动调用（OnMouseDown）
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
                TryServe();
            }
        }
    }

    // 尝试上菜
    private void TryServe()
    {
        int currentProduct = productManager.GetCurrentProduct();

        if (currentProduct == orderType)
        {
            onServeSuccess.Invoke(); // 正确上菜事件
            if (timeElapsed < 5f)
            {
                onQuickServe.Invoke(1); // 快速上菜事件
            }

            Destroy(gameObject);
            Debug.Log("成功上菜，订单：" + orderType);
        }
        else
        {
            Debug.Log("商品不匹配！");
        }
    }

    // 当顾客被销毁时销毁其订单显示对象
    private void OnDestroy()
    {
        if (orderDisplayInstance != null)
        {
            Destroy(orderDisplayInstance);
        }
    }
}
