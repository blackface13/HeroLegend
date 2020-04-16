using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProperties : MonoBehaviour
{
    //[Header("Map ID")]
    public int MapID;
    private GameObject BattleControl;
    //Các thuộc tính object của map, để disable khi tùy chọn cấu hình, tất cả set ở interface
    [Header("Các object hiệu ứng")]
    public GameObject[] ParticlesObject;//Các object hiệu ứng
    [Header("Các object phía sau, ko phải hình nền")]
    public GameObject[] BackgroundObject;//Các object phía sau, ko phải hình nền
    [Header("Các object detail để control")]
    public GameObject[] Detail1;//Set ở interface
    public GameObject[] Detail2;
    public GameObject[] Detail3;
    public GameObject[] Detail4;
    //Thời tiết (ngẫu nhiên trong game)
    [Header("Weather. 0: Rain, 1: Sun, 2: Leaf")]
    public GameObject[] Weather;
    private int WeatherID;
    //-----------------------------------------
    private float[] IntNumber;
    private Vector3[] Vec;
    private void Awake()
    {
        BattleControl = GameObject.FindGameObjectWithTag("ControlScene");
        WeatherID = UnityEngine.Random.Range(0, 3);
        switch (MapID)
        {
            case 2:
                Vec = new Vector3[2];
                IntNumber = new float[2];
                IntNumber[0] = 3f;//Bird fly speed
                IntNumber[1] = Detail1[0].transform.position.x;
                Vec[0] = Detail1[0].transform.position;
                Vec[1] = Detail1[1].transform.position;
                break;
            default: break;
        }
    }
    private void Start()
    {
        if (Weather.Length >= WeatherID - 1)
        {
            if (WeatherID.Equals(Weather.Length))//Nothing
            { }
            else if (WeatherID.Equals(0))//ID = 0
                Weather[WeatherID].SetActive(true);
            else if (Weather[WeatherID] != null)//Còn lại
                Weather[WeatherID].SetActive(true);
                if (!BattleControl.GetComponent<System_Battle>().BolNumber[2])
                    Weather[1].SetActive(false);
        }
    }
    private void Update()
    {
        switch (MapID)
        {
            case 2:
                M2BirdFly();
                break;
            default: break;
        }
    }
    #region Map 2
    /// <summary>
    /// Bird fly control
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    private void M2BirdFly()
    {
        if (Vec[0].x <= -75f)
            Vec[0].x = IntNumber[1];
        else Vec[0].x -= IntNumber[0] * Time.deltaTime;
        if (Vec[1].x <= -75f)
            Vec[1].x = IntNumber[1];
        else Vec[1].x -= IntNumber[0] * Time.deltaTime;
        Detail1[0].transform.position = Vec[0];
        Detail1[1].transform.position = Vec[1];
    }
    #endregion
}
