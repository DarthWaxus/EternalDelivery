using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBox : MonoBehaviour
{
    public float loadedPower;
    public float maxLoadPower;
    public float minLoadPower;
    public float loadPowerStep;
    public Transform flag;
    public Rigidbody loadedBody;
    public Transform lastLoadedBody;
    public float minFlagRot;
    public float maxFlagRot;
    public Sound boxInBotSound;
    public Sound fireSound;
    public Animator anim;
    public AudioSource loadingAudio;
    public void Fire()
    {
        if (!loadedBody)
        {
            return;
        }
        loadedBody.AddForce(Vector3.up * loadedPower);
        loadedBody = null;
        anim?.SetBool("BoxIn", false);
        loadedPower = 0;
        flag.rotation = Quaternion.Euler(minFlagRot, -90, 0);
        SoundManager.instance.PlaySound(fireSound);
    }
    private void Update()
    {
        if (loadedBody && loadedPower < maxLoadPower)
        {
            loadedPower += Time.deltaTime * loadPowerStep;
            float powerPercent = loadedPower / maxLoadPower * 100;
            flag.rotation = Quaternion.Euler(-powerPercent + minFlagRot, -90, 0);
            if (!loadingAudio.isPlaying && !SoundManager.instance.mute)
                loadingAudio.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        AddBox(other);
    }
    private void OnTriggerStay(Collider other)
    {
        AddBox(other);
    }
    private void AddBox(Collider other)
    {
        if (loadedBody == null && other.CompareTag("Box"))
        {
            Rigidbody otherBody = other.GetComponent<Rigidbody>();
            if (otherBody)
            {
                anim?.SetBool("BoxIn", true);
                loadedBody = otherBody;
                EventManager.SendBoxInBot(other.GetComponent<Box>());
                SoundManager.instance.PlaySound(boxInBotSound);
            }
        }
    }
}
