using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Vector3 maxSpeed;
    Rigidbody rbody;
    public Rigidbody captureBoxRbody;
    public CaptureBox captureBox;
    public float minPosX;
    public float maxPosX;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    private void FixedUpdate()
    {
        Vector3 vel = rbody.velocity;
        vel.x = Mathf.Clamp(vel.x, -maxSpeed.x, maxSpeed.x);
        vel.y = Mathf.Clamp(vel.y, -maxSpeed.y, maxSpeed.y);
        vel.z = Mathf.Clamp(vel.z, -maxSpeed.z, maxSpeed.z);
        rbody.velocity = vel;
        Vector3 pos = rbody.position;
        pos.x = Mathf.Clamp(pos.x, minPosX, maxPosX);
        rbody.position = pos;
    }
    void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        if (x != 0)
        {
            rbody.AddForce(speed * new Vector3(x, 0, 0));
        }
        if (Input.GetButton("Fire1") || Input.GetButton("Jump"))
        {
            captureBox.Fire();
        }
    }
}
