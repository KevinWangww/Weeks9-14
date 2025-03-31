using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDisplay : MonoBehaviour
{
    public Sprite[] productSprites;         // 商品图片数组（对应编号）
    private SpriteRenderer spriteRenderer;  // 渲染器用于显示图像

    // 初始化订单显示
    // 参数：
    // position：顾客的位置（自动加偏移）
    // productIndex：订单编号（商品类型）
    public void Initialize(Vector3 customerPosition, int productIndex)
    {
        // 设置显示位置（顾客正上方）
        transform.position = customerPosition + new Vector3(0, 1.5f, 0);

        // 获取 SpriteRenderer 组件
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 设置图像
        if (productIndex >= 0 && productIndex < productSprites.Length)
        {
            spriteRenderer.sprite = productSprites[productIndex];
        }
        else
        {
            Debug.LogWarning("订单编号超出范围：" + productIndex);
        }
    }
}
