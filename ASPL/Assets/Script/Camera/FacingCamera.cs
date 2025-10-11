using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    Transform[] childs;

    void Start()
    {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].rotation = Camera.main.transform.rotation;
        }
    }


    void Update()
    {
        //childs = new Transform[transform.childCount];
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i] = transform.GetChild(i);
        //}
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i].rotation = Camera.main.transform.rotation;
        //}

        // 更安全的写法（显式保留Y/Z轴）
        //Vector3 cameraEuler = Camera.main.transform.eulerAngles;

        //for (int i = 0; i < childs.Length; i++)
        //{
        //    // 使用当前物体的Y/Z值
        //    childs[i].eulerAngles = new Vector3(
        //        cameraEuler.x,
        //        childs[i].eulerAngles.y,  // 保留原有Y
        //        childs[i].eulerAngles.z   // 保留原有Z
        //    );
        //}
    }
}
