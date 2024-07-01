using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArrow : MonoBehaviour
{
    private LineRenderer Arrow;

    private Vector3 mousePos;
    public int pointsCount;
    public float arcModifier;
    private void Awake()
    {
        Arrow = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3( Input.mousePosition.x, Input.mousePosition.y, 10));
        SetArrowPosition();


    }
    /// <summary>
    /// 设置指针位置
    /// </summary>
    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position; // 卡牌位置
        Vector3 direction = mousePos - cardPosition; // 从卡牌指向鼠标的方向
        Vector3 normalizedDirection = direction.normalized; // 归一化方向

        // 计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular = new(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);

        // 设置控制点的偏移量，调整这个值来改变曲线的形状
        Vector3 offset = perpendicular * arcModifier;

        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; // 控制点

        Arrow.positionCount = pointsCount; // 设置 LineRenderer 的点的数量

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (float)(pointsCount - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
            Arrow.SetPosition(i, point);
        }
    }

    /// <summary>
    /// 计算二次贝塞尔曲线点
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // 第一项
        p += 2 * u * t * p1; // 第二项
        p += tt * p2; // 第三项

        return p;
    }
}
