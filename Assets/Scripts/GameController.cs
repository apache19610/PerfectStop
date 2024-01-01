using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;


public class GameController : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void AddCoinsExtern();

    //[DllImport("__Internal")]
    //private static extern void SetToLeaderBoard(int value);

    public Text coinsText, nowScore, topScore;
    public GameObject[] canvasButtons;
    public Transform mainCam;
    public GameObject[] roads;
    public GameObject barrier, blinker;
    public GameObject[] cars;
    public float cameraMoveSpeed = 10f;
    public List<GameObject> roadOnScreen = new List<GameObject>();
    private float _prevXCamPos, nextRoadXPos = 150f, _distanceForNewRoad = 60f, barrierXPos = -23f, rotateSpeed;
    public int countRoads = 3, lastBarrierRoad, getListSideElement, parkedCars, nowCoins, topScoreSave;
    private Rigidbody createdCar;
    private List<bool> turnSides = new List<bool>();
    private bool needToTurnCarLeft;
    private GameObject playerCar;
    [NonSerialized] public bool needToTurnCarRight;
    public AudioClip carNoise, brakes, correctPark;
    private AudioSource _audioSource;

    private void Start() {     
        _audioSource = GetComponent<AudioSource>();
        
        if (!PlayerPrefs.HasKey("NowCar")) {
            playerCar = cars[0];
        }
        else {
            foreach (GameObject car in cars) {
                if (car.name == PlayerPrefs.GetString("NowCar")) {
                    playerCar = car;
                    break;
                }
            }
        }

        PlayCarNoise();

        Barrier.isLose = false;
        rotateSpeed = cameraMoveSpeed * 5f;
        
        createdCar = Instantiate(
            playerCar, 
            new Vector3(-30, -0.24f, -9.8f), 
            Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>();
        createdCar.transform.SetParent(transform);
        
        _prevXCamPos = mainCam.position.x;

        CreateBarrier();
        topScoreSave = PlayerPrefs.GetInt("Score");
        topScore.text = "Лучший счёт: " + topScoreSave;
        //SetToLeaderBoard(topScoreSave);
        ShowAdv();
    }

    private void Update() {
        mainCam.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.World);

        if (createdCar.transform.localPosition.x < -15f) {
            Vector3 pos = createdCar.transform.localPosition;
            createdCar.transform.localPosition = Vector3.MoveTowards(
                pos, 
                new Vector3(-15, pos.y, pos.z), 
                10f * Time.deltaTime);
        }
        
        if (mainCam.position.x > _prevXCamPos + _distanceForNewRoad) {
            _prevXCamPos = mainCam.position.x;

            GameObject nextRoad = Instantiate(roads[Random.Range(0, roads.Length)], new Vector3(nextRoadXPos, 0, 0), Quaternion.identity);
            nextRoadXPos += 75f;

            nextRoad.name = "Road - " + countRoads;
            countRoads++;

            bool? leftSide = null;
            int whichBarrier = -2;
            if (nextRoad.name != "Road - " + lastBarrierRoad) {
                leftSide = Random.Range(0, 2) == 0;
                whichBarrier = Random.Range(1, 13);
            }

            bool isAddSideToList = false;
            for (short i = 0; i < 15; i++) {
                if (leftSide == true && (i == whichBarrier || i == whichBarrier + 1)) {
                    lastBarrierRoad = countRoads;

                    if (!isAddSideToList) {
                        turnSides.Add(true);
                        isAddSideToList = true;
                    }

                    GameObject newBlikner = Instantiate(blinker, new Vector3(barrierXPos, -0.2f, -3.05f), Quaternion.identity);
                    newBlikner.transform.SetParent(nextRoad.transform);
                }
                else
                    InstatiateBarrier(barrierXPos, nextRoad.transform);

                if (leftSide == false && (i == whichBarrier || i == whichBarrier + 1)) {
                    lastBarrierRoad = countRoads;
                    
                    if (!isAddSideToList) {
                        turnSides.Add(false);
                        isAddSideToList = true;
                    }
                    
                    GameObject newBlikner = Instantiate(blinker, new Vector3(barrierXPos, -0.2f, -16.99f), Quaternion.identity);
                    newBlikner.transform.SetParent(nextRoad.transform);
                }
                else
                    InstatiateBarrier(barrierXPos, nextRoad.transform, true);
                
                barrierXPos += 5f;
            }
            
            roadOnScreen.Add(nextRoad);

            foreach (GameObject road in roadOnScreen) {
                if (road.transform.position.x < mainCam.position.x - 75f) {
                    Destroy(road);
                    roadOnScreen.Remove(road);
                    break;
                }
            }
        }

        if (Barrier.isLose && !canvasButtons[0].activeSelf) {
            foreach (GameObject btn in canvasButtons)
                btn.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
#if !UNITY_WEBGL
           if (Input.GetTouch(0).phase != TouchPhase.Began)
                return; 
#endif

            if (createdCar.transform.localPosition.x < -15f)
                return;
            
            if (turnSides.Count == 0 || turnSides.Count - 1 < getListSideElement)
                return;

            if (needToTurnCarLeft || needToTurnCarRight)
                return;
            
            if (PlayerPrefs.GetString("music") != "No" && _audioSource.clip != brakes) {
                _audioSource.clip = brakes;
                _audioSource.loop = false;
                _audioSource.Play();
            }

            if (turnSides[getListSideElement])
                needToTurnCarLeft = true;
            else
                needToTurnCarRight = true;
            
            getListSideElement++;
        }
    }

    private void FixedUpdate() {
        if ((needToTurnCarLeft || needToTurnCarRight) && !Barrier.isLose)
            TurnCar(rotateSpeed);

        if (Barrier.isLose && cameraMoveSpeed > 0)
            cameraMoveSpeed = 0;
    }

    void TurnCar(float tSpeed) {
        if (needToTurnCarLeft)
            tSpeed *= -1;

        cameraMoveSpeed -= cameraMoveSpeed * 1.5f * Time.deltaTime;
        if (cameraMoveSpeed < 3f)
            cameraMoveSpeed = 0;

        if (createdCar.transform.eulerAngles.y > 90f && needToTurnCarLeft
            || needToTurnCarRight && createdCar.transform.eulerAngles.y < 88f ||
            createdCar.transform.eulerAngles.y > 93f) {
            createdCar.MoveRotation(
                createdCar.rotation * Quaternion.Euler(0, tSpeed * Time.fixedDeltaTime, 0));

            float moveSpeed = 6f * Time.deltaTime;
            if (needToTurnCarLeft)
                moveSpeed = 5f * Time.deltaTime * -1;

            createdCar.transform.localPosition -= new Vector3(0,0,moveSpeed);
        }
        else {
            cameraMoveSpeed = 0;
            createdCar.transform.localEulerAngles = new Vector3(0, 90,0);
            
            Invoke("CreateNewCar", 1f);
            
            if (PlayerPrefs.GetString("music") != "No" && _audioSource.clip != correctPark) {
                _audioSource.clip = correctPark;
                _audioSource.loop = false;
                _audioSource.Play();
            }
        }
    }

    void CreateNewCar() {
        if (Barrier.isLose || (!needToTurnCarLeft && !needToTurnCarRight))
            return;
        
        Destroy(createdCar);
        createdCar.transform.SetParent(null);
        Destroy(createdCar.gameObject, 3f);
        needToTurnCarLeft = false;
        needToTurnCarRight = false;
        cameraMoveSpeed = Random.Range(40, 55);
        createdCar = Instantiate(
            playerCar, 
            new Vector3(mainCam.transform.position.x - 2f, -0.24f, -9.8f), 
            Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>();
        createdCar.transform.SetParent(transform);

        PlayCarNoise();
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString();

        parkedCars++;
        nowScore.text = "<color=#FF6464>Счёт:</color> " + parkedCars;

        if (PlayerPrefs.GetInt("Score") < parkedCars) {
            PlayerPrefs.SetInt("Score", parkedCars);
            topScore.text = "Лучший счёт: " + parkedCars;
        }
    }

    private void PlayCarNoise() {
        if (PlayerPrefs.GetString("music") != "No") {
            _audioSource.clip = carNoise;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    private void CreateBarrier() {
        for (short i = 0; i < 15; i++) {
            InstatiateBarrier(barrierXPos, roadOnScreen[0].transform);
            InstatiateBarrier(barrierXPos, roadOnScreen[0].transform, true);
            barrierXPos += 5f;
        }
        
        for (short i = 0; i < 15; i++) {
            InstatiateBarrier(barrierXPos, roadOnScreen[1].transform);
            InstatiateBarrier(barrierXPos, roadOnScreen[1].transform, true);
            barrierXPos += 5f;
        }
    }

    private void InstatiateBarrier(float xPos, Transform roadParent, bool rightPos = false) {
        float zPos = rightPos ? -16.5f : -3.5f;
        GameObject newObj = Instantiate(barrier, new Vector3(xPos, 0.3f, zPos), Quaternion.identity) as GameObject;
        newObj.transform.SetParent(roadParent);
    }

    public void BoznagrazdenieZaProsmotrReklami()
    {
        if (parkedCars > 0)
        {
            nowCoins = PlayerPrefs.GetInt("Coins") + (parkedCars * 2);
            coinsText.text = nowCoins.ToString();
            PlayerPrefs.SetInt("Coins", nowCoins);
        }
        AddCoinsExtern();
    }

}
