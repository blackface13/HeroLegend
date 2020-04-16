using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class lưu trữ thông tin người dùng
//Các giá trị được set bên dưới là các giá trị mặc định nếu như user lần đầu chơi game
public class AccountInformations {
    public List<string> SecurityCode; //Mã guid lưu ID lần cuối cùng thao tác để chống hack
    public string UserID = Guid.NewGuid ().ToString ();
    public string UserName;
    public float UserGold = 10000; //Vàng sở hữu
    public float UserGems = 100; //Đá quý sở hữu
    public float UserSlotInventory = 100; //Số slot inventory ban đầu
    public float UserWinBattle = 0; //Thắng bao nhiêu trận
    public float UserLoseBatte = 0; //Thua bao nhiêu trận
    public float NumberSpin = 0; //Số lượt quay vòng quay may mắn
    public string CurrentVersion; //Application.version;
    public string[] EnemyFutureMap = new string[7];
    public string ItemUseForBattle = ";;"; //Danh sách item dc trang bị cho battle, 3 trang bị ngăn cách bởi 2 dấu ; - cấu trúc: "A,B;A,B". A = item type, B = item ID
    public bool IsAutoBattle = false; //Tự động đánh trong battle
    #region Độ khó của các map 
    public float[] DifficultMap = new float[7] { 1, 1, 1, 1, 1, 1, 1 };
    //Thứ tự: 0 - Độ khó của map rừng
    //Thứ tự: 1 - Độ khó của map đòng bằng
    //Thứ tự: 2 - Độ khó của map núi lửa
    //Thứ tự: 3 - Độ khó của map tuyết
    //Thứ tự: 4 - Độ khó của map địa ngục
    //Thứ tự: 5 - Độ khó của map hang độc
    //Thứ tự: 6 - Độ khó của map hang ma
    #endregion
}
