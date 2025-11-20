using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<CarController> cars = new List<CarController>();
    public float positionUpdateRate = 0.1f;
    public Transform[] spawnPoints; 
    private float lastPositionUpdateTime;
    public int playersToBegin = 2;
    public bool gameStarted = false;
    public int lapsToWin = 3;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Time.time - lastPositionUpdateTime > positionUpdateRate)
        {
            lastPositionUpdateTime = Time.time;
            UpdateCarRacePositions();
        }

        if(!gameStarted && cars.Count == playersToBegin)
        {
            gameStarted = true;
            StartCountdown();
        }
    }

    void StartCountdown () 
    {
        PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

        for (int x = 0; x < uis.Length; ++x)
            uis[x].StartCountDownDisplay();

        Invoke("BeginGame", 3.0f);
    }

    void BeginGame ()
    {
        for (int x = 0; x < cars.Count ; ++x)
        {
            cars[x].canControl = true;
        }
    }

    void UpdateCarRacePositions ()
    {
        cars.Sort(SortPosition);

        for (int x = 0; x < cars.Count; x++)
        {
            cars[x].racePosition = cars.Count - x;
        }
    }

    int SortPosition (CarController a, CarController b)
    {
        if (a.zonesPassed > b.zonesPassed)
            return 1;
        else if (b.zonesPassed > a.zonesPassed)
            return -1;

        float aDist = Vector3.Distance(a.transform.position, a.curTrackZone.transform.position);
        float bDist = Vector3.Distance(b.transform.position, b.curTrackZone.transform.position);

        return aDist > bDist ? 1 : -1;
    }

    public void CheckIsWinner (CarController car)
    {
        if (car.curLap == lapsToWin + 1) 
        {
            for (int x = 0; x < cars.Count; ++x )
            {
                cars[x].canControl = false;
            }

        PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

        for (int x = 0; x < uis.Length; ++x)
            uis[x].GameOver(uis[x].car == car);

        }
    }
}
