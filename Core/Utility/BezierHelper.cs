using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������߹�����
/// </summary>
public class BezierHelper
{
    /// <summary>
    /// ���Ա��������ߣ�����Tֵ�����㱴���������������Ӧ�ĵ�
    /// </summary>
    /// <param name="t"></param>Tֵ
    /// <param name="p0"></param>��ʼ��
    /// <param name="p1"></param>���Ƶ�
    /// <returns></returns>����Tֵ��������ı��������ߵ�
    public static Vector3 CalculateLineBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;

        Vector3 p = u * p0;
        p += t * p1;


        return p;
    }

    /// <summary>
    /// ���α��������ߣ�����Tֵ�����㱴���������������Ӧ�ĵ�
    /// </summary>
    /// <param name="t"></param>Tֵ
    /// <param name="p0"></param>��ʼ��
    /// <param name="p1"></param>���Ƶ�
    /// <param name="p2"></param>Ŀ���
    /// <returns></returns>����Tֵ��������ı��������ߵ�
    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    /// <summary>
    /// ���α��������ߣ�����Tֵ�����㱴���������������Ӧ�ĵ�
    /// </summary>
    /// <param name="t">����ֵ</param>
    /// <param name="p0">���</param>
    /// <param name="p1">���Ƶ�1</param>
    /// <param name="p2">���Ƶ�2</param>
    /// <param name="p3">β��</param>
    /// <returns></returns>
    public static Vector3 CalculateThreePowerBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;

        Vector3 p = uuu * p0;
        p += 3 * t * uu * p1;
        p += 3 * tt * u * p2;
        p += ttt * p3;

        return p;
    }

    /// <summary>
    /// ������ĵ�
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <returns></returns>
    public static Vector3 FuckYou(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p =3*uu * p0;
        p += 3 *( t * 2*u + uu) * p1;
        p += 3 * (u * 2 * t + t) * p2;
        p += 3*t * p3;

        return p;
    }



    /// <summary>
    /// ��ȡ�洢���������ߵ������
    /// </summary>
    /// <param name="startPoint"></param>��ʼ��
    /// <param name="controlPoint"></param>���Ƶ�
    /// <param name="endPoint"></param>Ŀ���
    /// <param name="segmentNum">���������������������յ�</param>
    /// <returns></returns>�洢���������ߵ������
    public static Vector3[] GetLineBeizerList(Vector3 startPoint, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        segmentNum--;
        for (int i = 0; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateLineBezierPoint(t, startPoint, endPoint);
            path[i ] = pixel;
        }
        return path;

    }

    /// <summary>
    /// ��ȡ�洢�Ķ��α��������ߵ������
    /// </summary>
    /// <param name="startPoint"></param>��ʼ��
    /// <param name="controlPoint"></param>���Ƶ�
    /// <param name="endPoint"></param>Ŀ���
    /// <param name="segmentNum"></param>�����������
    /// <returns></returns>�洢���������ߵ������
    public static Vector3[] GetCubicBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        segmentNum--;
        for (int i = 0; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);
            path[i] = pixel;
        }
        return path;

    }

    /// <summary>
    /// ��ȡ�洢�����α��������ߵ������
    /// </summary>
    /// <param name="startPoint"></param>��ʼ��
    /// <param name="controlPoint1"></param>���Ƶ�1
    /// <param name="controlPoint2"></param>���Ƶ�2
    /// <param name="endPoint"></param>Ŀ���
    /// <param name="segmentNum"></param>�����������
    /// <returns></returns>�洢���������ߵ������
    public static Vector3[] GetThreePowerBeizerList(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        segmentNum--;
        for (int i = 0; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateThreePowerBezierPoint(t, startPoint,
                controlPoint1, controlPoint2, endPoint);
            path[i ] = pixel;
        }
        return path;
    }
    public static Vector3[][] GetThreePowerBeizerListWithSlope(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        Vector3[] slopes = new Vector3[segmentNum];
        segmentNum--;
        for (int i = 0; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateThreePowerBezierPoint(t, startPoint,
                controlPoint1, controlPoint2, endPoint);
            Vector3 slope = FuckYou(t, startPoint,
                controlPoint1, controlPoint2, endPoint);
            path[i] = pixel;
            slopes[i] = slope;
        }
        return new Vector3[][] { path,slopes};
    }
}
