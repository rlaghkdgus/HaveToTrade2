using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utills : MonoBehaviour
{
    
}
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour   // �̱����� �����ų Ŭ������
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                    Debug.LogError("Not Activated : " + typeof(T));
            }
            return instance;
        }
    }
}
public class Data<T>//������ ����, Value�� ���Ҷ����� �޼ҵ� ȣ��
{
    private T v;//�̰� ���Ҷ����� �޼ҵ� ȣ��
    public T Value
    {
        get { return v; }//����� �޾ƿ�
        set//����� ���Ҷ�����
        {
            v = value;
            onChange?.Invoke(value);//�����Ϳ� ������ �Լ��� ȣ��
        }
    }
    public Action<T> onChange;//onChange�� ������ �ȿ� �޼ҵ� �߰� �� ����(����)
}
static class YieldCache 
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)//�ε� �Ҽ��� �������̸� �����ϱ����� ����. ex) 0.1f�� 0.10000f�� �ٸ��� ��޵Ǵ°� ����.
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();//�ؽð� ��ȯ���� Ű�� ȿ�������� �˻��ϱ�����
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    //��� : ������ ���ð��� ����Ұ�� ��ü�� ���� ������ �ʰ� �̹� ��������ִ� ��ü���� ȣ��, ��û�� �ð��� ���ٸ� ���ο� ��ü ���� �� ��ȯ.
    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());//���ð��� Ű�� �� waitforseconds ��ü����(Dictionary���)
    private static readonly Dictionary<float, WaitForSecondsRealtime> _timeIntervalReal = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)//�Ķ���� seconds ���� Ű�� ���� �� �̹� ĳ�̵� ��ü Ž��
    {
        WaitForSeconds wfs;
        if (!_timeInterval.TryGetValue(seconds, out wfs))//��������������
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));//��ųʸ��� ����
        return wfs;//��ȯ
    }

    public static WaitForSecondsRealtime WaitForSecondsRealTime(float seconds)
    {
        WaitForSecondsRealtime wfsReal;
        if (!_timeIntervalReal.TryGetValue(seconds, out wfsReal))
            _timeIntervalReal.Add(seconds, wfsReal = new WaitForSecondsRealtime(seconds));
        return wfsReal;
    }
}
