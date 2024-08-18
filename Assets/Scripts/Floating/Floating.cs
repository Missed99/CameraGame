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
    public float floatingHeight = 0.25f;//悬浮高度
    public CurveType curveType;//曲线类型
    public float time = 90 * Mathf.Rad2Deg;
    public Vector3 originalPosition;//原始位置

    private void Start()
    {
        originalPosition = transform.position;
        curveType = RandomSinOrCos();//获得随机一种曲线
    }

    private void Update()
    {
        time += Time.deltaTime;//时间不断增加
        FloatingByCurve(curveType);
    }

    //随机一种曲线
    public CurveType RandomSinOrCos()
    {
        var tmp = Random.Range(0, 2);
        return tmp == 0 ? CurveType.Sin : CurveType.Cos;
    }

    //按曲线运行
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

    //正弦运行方式
    public void SinFloating()
    {
        var y = Mathf.Sin(time) * floatingHeight + originalPosition.y;
        this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }

    //余弦运行方式
    public void CosFloating()
    {
        var y = Mathf.Cos(time) * floatingHeight + originalPosition.y;
        this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }
}
