using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class TowerController : MonoBehaviour {
    [Header ("Draw Curve")]
    public AnimationCurve moveCurve;
    public GameObject[] ObjectController;
    private Vector3[] TowerPosition;
    private int ThisTower = 0, TowerLeft = 1, TowerRight = 2; //3 tòa tháp
    private bool IsMoving = false; //Biến xác định có đang thực hiện hiệu ứng di chuyển hay ko
    public Text[] TextUI;
    void Start () {
        TowerPosition = new Vector3[3]; //3 Tower
        for (int i = 0; i < TowerPosition.Length; i++)
            TowerPosition[i] = ObjectController[i].transform.position;
        ButtonFunctions (-1); //Update giá trị tiền tệ
    }

    private void SetupTextUI () {
        TextUI[1].text = ""; //Title
        TextUI[5].text = Languages.lang[294]; //Start
    }

    /// <summary>
    /// Chức năng
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case -1: //update lại text tiền tệ
                TextUI[2].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                TextUI[3].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            case 0: //Đóng UI
                GameSystem.DisposePrefabUI (7);
                break;
            case 1: //Show hướng dẫn
                break;
            case 2: //Move left
                if (!IsMoving) {
                    IsMoving = true; //Đang thực hiện move

                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[ThisTower], TowerPosition[0], TowerPosition[1], .3f, moveCurve));
                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[TowerLeft], TowerPosition[1], TowerPosition[2], .3f, moveCurve));
                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[TowerRight], TowerPosition[2], TowerPosition[0], .3f, moveCurve));

                    StartCoroutine (MoveActions (false));

                    //Đoạn này set soringlayer của group nhưng ko thực hiện dc với UI trong canvans
                    // ObjectController[ThisTower].GetComponent<SortingGroup> ().sortingOrder = 1;
                    // ObjectController[TowerLeft].GetComponent<SortingGroup> ().sortingOrder = 0;
                    // ObjectController[TowerRight].GetComponent<SortingGroup> ().sortingOrder = 2;
                }
                break;
            case 3: //Move right
                if (!IsMoving) {
                    IsMoving = true; //Đang thực hiện move

                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[ThisTower], TowerPosition[0], TowerPosition[2], .3f, moveCurve));
                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[TowerLeft], TowerPosition[1], TowerPosition[0], .3f, moveCurve));
                    StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[TowerRight], TowerPosition[2], TowerPosition[1], .3f, moveCurve));

                    StartCoroutine (MoveActions (true));
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Thực hiện hiệu ứng nút bên trái
    /// </summary>
    /// <param name="isLeft">true = phải, false = trái</param>
    /// <returns></returns>
    private IEnumerator MoveActions (bool isRight) {
        var delaytime = 0.15f;
        if (isRight) {
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerRight], new Vector3 (0f, .5f, .5f), delaytime));
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[ThisTower], new Vector3 (.5f, .5f, .5f), delaytime * 2));
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerLeft], new Vector3 (1f, 1f, 1f), delaytime * 2));
            yield return new WaitForSeconds (delaytime);
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerRight], new Vector3 (.5f, .5f, .5f), delaytime));
            CalculatorThisTower (isRight);
        } else {
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerLeft], new Vector3 (0f, .5f, .5f), delaytime));
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[ThisTower], new Vector3 (.5f, .5f, .5f), delaytime * 2));
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerRight], new Vector3 (1f, 1f, 1f), delaytime * 2));
            yield return new WaitForSeconds (delaytime);
            StartCoroutine (GameSystem.ScaleUI (0, ObjectController[TowerLeft], new Vector3 (.5f, .5f, .5f), delaytime));
            CalculatorThisTower (isRight);
        }
        IsMoving = false; //Đang thực hiện move
    }

    /// <summary>
    /// Tính toán tòa tháp hiện tại
    /// </summary>
    /// <param name="isIncrease">true = +, false = -</param>
    private void CalculatorThisTower (bool isIncrease) {
        var totalTower = TowerPosition.Length; //Gán tổng các tòa tháp
        if (isIncrease) //Next tháp
        {
            if (ThisTower >= totalTower - 1)
                ThisTower = 0;
            else ThisTower++;
        } else {
            if (ThisTower <= 0)
                ThisTower = totalTower - 1;
            else ThisTower--;
        }
        TowerLeft = ThisTower == 2 ? 0 : ThisTower + 1;
        TowerRight = ThisTower == 0 ? 2 : ThisTower - 1;
    }
}