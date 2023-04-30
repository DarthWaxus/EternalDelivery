using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public float score;
    public List<Transform> spawnPoints;
    public List<Transform> spawnCopterPoints;
    public float spawnBoxTimer = 1;
    public float spawnCopterTimer = 1;
    public bool playing = false;
    public bool gameOver = false;
    public List<GameObject> boxes;
    public List<GameObject> copters;
    public Transform boxesHolder;
    public Transform coptersHolder;
    public GameObject logo;
    public GameObject startBtn;
    public TMP_Text scoreText;
    private int coptersNum = 0;
    private int maxCoptersNum = 10;
    private int boxesNum = 0;
    private int maxBoxesNum = 20;
    public int undeliveredNum = 0;
    public int undeliveredMaxNum = 5;
    public TMP_Text undeliveredText;
    public TMP_Text gameOverText;
    public GameObject boxesBeware;
    public GameObject deliveredPanel;
    public GameObject undeliveredPanel;
    public Camera cam;
    public CameraShake cameraShake;
    public float shakeDurationOnBoxInBot;
    public float shakeIntensityOnBoxInBot;
    public Toggle toggleMute;

    private void Awake()
    {
        EventManager.BoxRemoved.AddListener(BoxRemoved);
        EventManager.CopterRemoved.AddListener(CopterRemoved);
        EventManager.BoxInBot.AddListener(BoxInBot);
    }
    public void BoxInBot(Box box)
    {
        cameraShake.Shake(shakeDurationOnBoxInBot, shakeIntensityOnBoxInBot);
    }
    public void CopterRemoved(Copter copter)
    {
        coptersNum--;
        if (copter.box == null)
        {
            undeliveredNum++;
            UpdateUndeliveredText();
            if (undeliveredNum >= undeliveredMaxNum)
            {
                GameOver();
            }
        }
    }
    public void BoxRemoved(Box box)
    {
        if (box.captured)
        {
            score++;
            scoreText.text = score.ToString();
        }
        boxesNum--;
        boxesBeware?.SetActive(false);
    }
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        logo.SetActive(true);
        boxesBeware?.SetActive(false);
        deliveredPanel?.SetActive(false);
        undeliveredPanel?.SetActive(false);
    }

    public void StartGame()
    {
        score = 0;
        boxesNum = 0;
        coptersNum = 0;
        scoreText.text = score.ToString();
        gameOver = false;
        gameOverText.gameObject.SetActive(false);
        logo?.SetActive(false);
        startBtn?.SetActive(false);
        undeliveredText.gameObject.SetActive(true);
        playing = true;
        StartCoroutine(SpawnBoxesRoutine());
        StartCoroutine(SpawnCoptersRoutine());
        deliveredPanel?.SetActive(true);
        undeliveredPanel?.SetActive(true);
    }
    public void GameOver()
    {
        deliveredPanel?.SetActive(false);
        undeliveredPanel?.SetActive(false);
        playing = false;
        gameOver = true;
        undeliveredNum = 0;
        UpdateUndeliveredText();
        startBtn.SetActive(true);
        undeliveredText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Nice job! You've delivered " + score.ToString() + " packages. Try again?";
        GameObject[] copters = GameObject.FindGameObjectsWithTag("Copter");
        foreach (var item in copters)
            Destroy(item);
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var item in boxes)
            Destroy(item);
    }
    void UpdateUndeliveredText()
    {
        undeliveredText.text = undeliveredNum.ToString() + "/" + undeliveredMaxNum.ToString();
    }
    IEnumerator SpawnBoxesRoutine()
    {
        while (playing)
        {
            yield return new WaitForSeconds(spawnBoxTimer);
            if (boxesNum >= maxBoxesNum)
            {
                boxesBeware?.SetActive(true);
                continue;
            }
            Spawn(spawnPoints[Random.Range(0, spawnPoints.Count)].position, boxes[Random.Range(0, boxes.Count)], boxesHolder);
            boxesNum++;
        }
    }
    IEnumerator SpawnCoptersRoutine()
    {
        while (playing)
        {
            yield return new WaitForSeconds(spawnCopterTimer);
            if (coptersNum >= maxCoptersNum)
                continue;
            Spawn(spawnCopterPoints[Random.Range(0, spawnCopterPoints.Count)].position, copters[Random.Range(0, copters.Count)], coptersHolder);
            coptersNum++;
        }
    }
    void Spawn(Vector3 pos, GameObject obj, Transform holder)
    {
        Instantiate(obj, pos, Quaternion.identity, holder);
    }
    public void ToggleMute()
    {
        SoundManager.instance.mute = !toggleMute.isOn;
    }
}
