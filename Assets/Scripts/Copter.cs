using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copter : MonoBehaviour
{
    public Vector3 posToMove;
    public Box box;
    public float waitTime;
    float maxWaitTime;
    public float idleTime;
    public float speed;
    public float leaveSpeed;
    public Rect gameField;
    public Rect leaveField;
    public bool waitStarted = false;
    public CopterState state;
    public Rigidbody rbody;
    public float step = 0.1f;
    public Transform boxHolder;
    public float backPos = 5;
    public Sound captureSound;
    public Sound leaveWithoutBoxSound;
    public Sound leaveWithBoxSound;
    public GameObject waitTimer;
    public Transform waitTimerSlider;


    void Start()
    {
        posToMove = GetRandomPos(gameField);
        state = CopterState.Move;
        maxWaitTime = waitTime;
        // StartCoroutine(MainRoutine());
    }
    public Vector3 GetRandomPos(Rect rect, float z = 0)
    {
        Vector3 res = new Vector3();
        res.x = Random.Range(rect.xMin, rect.xMax);
        res.y = Random.Range(rect.yMin, rect.yMax);
        res.z = z;
        return res;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitStarted && waitTime >= 0 && state != CopterState.Leave && state != CopterState.MoveToBack)
        {
            waitTime -= Time.deltaTime;
            float percent = waitTime / maxWaitTime;
            waitTimerSlider.localScale = new Vector3(percent, 1, 1);
            if (waitTime <= 0)
                MoveToBack();
        }
        bool reachedMovePos = Vector3.Distance(transform.position, posToMove) <= step;
        if (state != CopterState.Idle)
        {
            if (reachedMovePos)
            {
                if (state == CopterState.Leave)
                    Remove();
                else if (state == CopterState.MoveToBack)
                    Leave();
                else if (waitTime > 0)
                    Idle();
            }
            else
            {
                Move(posToMove);
            }
        }
    }
    private void Idle()
    {
        if (!waitStarted)
            waitStarted = true;
        state = CopterState.Idle;
        StartCoroutine(IdleRoutine());
    }
    private void Move()
    {
        state = CopterState.Move;
        posToMove = GetRandomPos(gameField);
    }
    private void MoveToBack()
    {
        waitTime = 0;
        posToMove = transform.position;
        posToMove.z = backPos;
        state = CopterState.MoveToBack;
        rbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
    private void Leave()
    {
        posToMove = GetRandomPos(leaveField, backPos);
        state = CopterState.Leave;
    }
    private void FixedUpdate()
    {
        if (box != null)
            box.transform.position = boxHolder.position;
    }
    void Remove()
    {
        if (box != null)
        {
            EventManager.SendBoxRemoved(box);
            SoundManager.instance.PlaySound(leaveWithBoxSound);
        }
        else
        {
            SoundManager.instance.PlaySound(leaveWithoutBoxSound);
        }
        EventManager.SendCopterRemoved(this);

        Destroy(gameObject);
    }
    void Move(Vector3 pos)
    {
        Vector3 vel = new Vector3();
        if (transform.position.x < pos.x)
        {
            vel.x = step;
        }
        else if (transform.position.x > pos.x)
        {
            vel.x = -step;
        }
        if (transform.position.y < pos.y)
        {
            vel.y = step;
        }
        else if (transform.position.y > pos.y)
        {
            vel.y = -step;
        }
        if (transform.position.z < pos.z)
        {
            vel.z = step;
        }
        else if (transform.position.z > pos.z)
        {
            vel.z = -step;
        }
        float s = (state == CopterState.Leave || state == CopterState.MoveToBack) ? leaveSpeed : speed;
        rbody.AddForce(vel * s);
    }
    IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(idleTime);
        if (state == CopterState.Idle)
            Move();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (box == null && other.transform.CompareTag("Box"))
        {
            box = other.gameObject.GetComponent<Box>();
            MoveToBack();
            EventManager.SendBoxCaptured(box, this);
            SoundManager.instance.PlaySound(captureSound);
        }
    }
}
public enum CopterState
{
    Idle,
    Move,
    Leave,
    MoveToBack
}
