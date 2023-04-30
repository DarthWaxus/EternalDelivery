using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 0;
    public Vector3 angle;
    void Update()
    {
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
