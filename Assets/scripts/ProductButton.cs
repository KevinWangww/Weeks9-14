using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    public int productIndex;                  // 当前按钮代表的商品编号
    public ProductManager productManager;     // 商品管理器引用
    private Button button;                    // Unity UI 按钮组件

    public UnityEvent<int> onProductSelected; // 商品选择事件，传递商品编号

    // 初始化按钮事件监听（Unity生命周期方法）
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick); // 绑定点击事件
    }

    // 当按钮被点击时触发
    // 使用者：ProductManager（监听此事件）
    void OnButtonClick()
    {
        onProductSelected.Invoke(productIndex); // 发出商品编号事件
        Debug.Log("选择商品：" + productIndex);
    }
}
