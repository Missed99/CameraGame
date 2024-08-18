using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CurveType
{
    Sin,
    Cos
}

public class Floating : MonoBehaviour
{
    public float floatingHeight = 0.25f;//�����߶�
    public CurveType curveType;//��������
    public float time = 90 * Mathf.Rad2Deg;
    public Vector3 originalPosition;//ԭʼλ��

    private void Start()
    {
        originalPosition = transform.position;
        curveType = RandomSinOrCos();//������һ������
    }

    private void Update()
    {
        time += Time.deltaTime;//ʱ�䲻������
        FloatingByCurve(curveType);
    }

    //���һ������
    public CurveType RandomSinOrCos()
    {
        var tmp = Random.Range(0, 2);
        return tmp == 0 ? CurveType.Sin : CurveType.Cos;
    }

    //����������
    public void FloatingByCurve(CurveType _curveType)
    {
        switch (_curveType)
        {
            case CurveType.Sin:
                SinFloating();
                break;
            case CurveType.Cos:
                CosFloating();
                break;
            default:
                break;
        }
    }

    //�������з�ʽ
    public void SinFloating()
    {
        var y = Mathf.Sin(time) * floatingHeight + originalPosition.y;
        this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }

    //�������з�ʽ
    public void CosFloating()
    {
        var y = Mathf.Cos(time) * floatingHeight + originalPosition.y;
        this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }
}
