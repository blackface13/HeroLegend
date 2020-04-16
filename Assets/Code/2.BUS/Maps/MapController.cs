using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Điều khiển các object chi tiết trong map cho sinh động hơn
public class MapController : MonoBehaviour
{
    [Header("Map ID")]
    public int MapID;
    public float[] floatnumber;
    public GameObject[] obj;//Các object phía sau, ko phải hình nền

    private void Awake()
    {
        SetupVariable();
    }

    private void SetupVariable()
    {
        switch (MapID)
        {
            #region Map 1 

            case 1:
                floatnumber = new float[3];//Tọa độ X của 2 đám mây và bầy chim
                floatnumber[0] = obj[0].transform.position.x;
                floatnumber[1] = obj[1].transform.position.x;
                floatnumber[2] = obj[2].transform.position.x;
                break;

            #endregion 
            #region Map 2 

            case 2:
                floatnumber = new float[2];//Tọa độ X của 2 đám mây và bầy chim
                floatnumber[0] = obj[0].transform.position.x;
                floatnumber[1] = obj[1].transform.position.x;
                break;

            #endregion 
            default: break;
        }
    }
    // private void Start()
    // {

    // }
    private void Update()
    {
        switch (MapID)
        {
            #region Map 1 

            case 1:
                //Lặp lại tọa độ nếu vượt quá màn hình
                if (floatnumber[0] <= -35f)
                    floatnumber[0] = -floatnumber[0];
                else floatnumber[0] -= 0.1f * Time.deltaTime;
                if (floatnumber[1] <= -35f)
                    floatnumber[1] = -floatnumber[1];
                else floatnumber[1] -= 0.2f * Time.deltaTime;
                if (floatnumber[2] <= -35f)
                    floatnumber[2] = -floatnumber[2];
                else floatnumber[2] -= 0.5f * Time.deltaTime;//Tốc độ bay của chim

                //0, 1: 2 đám mây
                obj[0].transform.position = new Vector3(floatnumber[0], obj[0].transform.position.y, obj[0].transform.position.z);
                obj[1].transform.position = new Vector3(floatnumber[1], obj[1].transform.position.y, obj[1].transform.position.z);
                //2: bầy chim
                obj[2].transform.position = new Vector3(floatnumber[2], obj[2].transform.position.y, obj[2].transform.position.z);
                break;
            #endregion
            #region Map 2 

            case 2:
                //Lặp lại tọa độ nếu vượt quá màn hình
                if (floatnumber[0] <= -35f)
                    floatnumber[0] = -floatnumber[0];
                else floatnumber[0] -= 0.1f * Time.deltaTime;
                if (floatnumber[1] <= -35f)
                    floatnumber[1] = -floatnumber[1];
                else floatnumber[1] -= 0.2f * Time.deltaTime;

                //0, 1: 2 đám mây
                obj[0].transform.position = new Vector3(floatnumber[0], obj[0].transform.position.y, obj[0].transform.position.z);
                obj[1].transform.position = new Vector3(floatnumber[1], obj[1].transform.position.y, obj[1].transform.position.z);
                break;
            #endregion
            default: break;
        }
    }
}