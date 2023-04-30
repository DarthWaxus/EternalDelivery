using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Rigidbody rbody;
    public BoxCollider col;
    public bool captured = false;

    private void Awake()
    {
        EventManager.BoxCaptured.AddListener(BoxCaptured);
    }
    private void Update()
    {
        if (transform.position.y < -10)
        {
            EventManager.SendBoxRemoved(this);
            Destroy(gameObject);
        }
    }
    public void BoxCaptured(Box box, Copter copter)
    {
        if (box != this)
            return;
        rbody.isKinematic = true;
        col.enabled = false;
        transform.position = copter.boxHolder.position;
        transform.SetParent(copter.transform);
        captured = true;
    }
}
