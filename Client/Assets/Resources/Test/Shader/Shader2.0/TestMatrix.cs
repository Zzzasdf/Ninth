using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatrix : MonoBehaviour
{
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 posWorld = transform.parent.localToWorldMatrix.MultiplyPoint(transform.localPosition);
        Debug.Log(posWorld);
        // 表示 从 世界 转换到 对应的父类下的 物体坐标系
        Vector3 targetPos = target.transform.worldToLocalMatrix.MultiplyPoint(posWorld);
        Debug.Log(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
