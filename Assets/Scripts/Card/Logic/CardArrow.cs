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
    /// ����ָ��λ��
    /// </summary>
    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position; // ����λ��
        Vector3 direction = mousePos - cardPosition; // �ӿ���ָ�����ķ���
        Vector3 normalizedDirection = direction.normalized; // ��һ������

        // ���㴹ֱ�ڿ��Ƶ���귽�������
        Vector3 perpendicular = new(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);

        // ���ÿ��Ƶ��ƫ�������������ֵ���ı����ߵ���״
        Vector3 offset = perpendicular * arcModifier;

        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; // ���Ƶ�

        Arrow.positionCount = pointsCount; // ���� LineRenderer �ĵ������

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (float)(pointsCount - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
            Arrow.SetPosition(i, point);
        }
    }

    /// <summary>
    /// ������α��������ߵ�
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

        Vector3 p = uu * p0; // ��һ��
        p += 2 * u * t * p1; // �ڶ���
        p += tt * p2; // ������

        return p;
    }
}
