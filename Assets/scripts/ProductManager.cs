using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductManager : MonoBehaviour
{
    public UnityEvent<int> onProductSwitch; // 商品切换事件（可供UI动画等扩展）
    private int currentProduct = 0;         // 当前选择商品编号

    private void Start()
    {
    }

    // 接收商品按钮点击事件并更新当前商品编号
    // 触发商品切换事件
    // 使用者：ProductButton.cs（按钮点击后触发）
    public void OnProductSelected(int productIndex)
    {
        currentProduct = productIndex;
        onProductSwitch.Invoke(currentProduct);
        Debug.Log("当前选择商品：" + currentProduct);
    }

    // 提供当前商品编号
    // 使用者：Customer.cs（点击顾客时判断是否选对商品）
    public int GetCurrentProduct()
    {
        return currentProduct;
    }
}
