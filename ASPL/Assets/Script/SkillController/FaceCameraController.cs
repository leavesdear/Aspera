using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class FaceCameraController : MonoBehaviour
{
    protected virtual void Start()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
