using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using BlackCore;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Room : MonoBehaviour {
    private Thread[] thread;
    public Text[] TextLanguage; //Setup in interface
    public GameObject[] Obj; //Setup in interface
    private Sprite[] Img = new Sprite[2]; //Hình ảnh xám cho button ko thể nhấn
    private GameObject[] ObjChar = new GameObject[2];
    private int CurentUserInRoom;
    private float HeightContentMessenger;

    #region HeroController 
    private int SlotTeamSelected = -1; //Slot trong team đang selected
    private int SlotHeroSelected = -1; //Slot trong list đang selected
    private int HorizontalQuantity = 5; //Số slot trên 1 hàng ngang
    private List<GameObject> HeroList; //Danh sách hero
    private List<GameObject> HeroListAvatar; //Các object hero dành cho hiển thị bên ngoài
    private List<GameObject> EnemyListAvatar; //Các object enemy dành cho hiển thị bên ngoài
    private Image[] ItemUseImg; //Hình ảnh item đã được trang bị trước khi vào trận
    private List<GameObject> ListBackgroundItemUse = new List<GameObject> (); //Danh sách bg item use
    private List<GameObject> ListItemUse = new List<GameObject> (); //Danh sách item use
    private int SlotItemUseSelected; //Slot đc chọn để trang bị item
    private List<GameObject> ListBackgroundItemEquip = new List<GameObject> (); //Danh sách bg item use
    private List<GameObject> ListItemEquip = new List<GameObject> (); //Danh sách item use
    public GameObject[] ListItemEquiped; //Danh sách item đã được trang bị, set ở interface
    private ItemModel ItemViewing; //Item đang được xem
    private List<GameObject> ListHeroChangeEquip = new List<GameObject> (); //Danh sách hero dành cho đổi trang bị nhanh

    #endregion

    #region Initialize 

    private void Awake () {
        if (Module.IsLimitFPS)
            Application.targetFrameRate = GameSystem.Settings.FPSLimit;
        // if (SystemInfo.deviceUniqueIdentifier.Equals ("f60b101527283e3173368eb154253cb3a3643352") ||
        //     SystemInfo.deviceUniqueIdentifier.Equals ("b529ddc529207d131d57ee255fe9c084"))
        //     Obj[10].SetActive (true);
        // thread = new Thread[2];
        // Img[0] = Resources.Load<Sprite>("Images/13");
        // Img[1] = Resources.Load<Sprite>("Images/20");
        // ObjChar[0] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/AvtH" + Module.GameLoad("CharID")), new Vector3(0, 2, 0), Quaternion.identity);
        // ObjChar[0].transform.SetParent(Obj[7].transform, false);//Set parent
        // //thread[0] = new Thread(GetDataRoom);
        // TCPNetBase.IsOurRoom = true;
        // HeightContentMessenger = Obj[11].GetComponent<RectTransform>().sizeDelta.y;
    }

    void Start () {
        ItemUseImg = new Image[3];
        //Tọa độ backgound nền hero team 1 đứng
        Obj[5].transform.position = new Vector3 (0 - Camera.main.aspect * 8f, Obj[5].transform.position.y, Obj[5].transform.position.z);
        Obj[6].transform.position = new Vector3 (0 - Camera.main.aspect * 5f, Obj[6].transform.position.y, Obj[6].transform.position.z);
        Obj[7].transform.position = new Vector3 (0 - Camera.main.aspect * 2f, Obj[7].transform.position.y, Obj[7].transform.position.z);
        //Tọa độ backgound nền hero team 2 đứng
        Obj[18].transform.position = new Vector3 (0 + Camera.main.aspect * 8f, Obj[18].transform.position.y, Obj[18].transform.position.z);
        Obj[19].transform.position = new Vector3 (0 + Camera.main.aspect * 5f, Obj[19].transform.position.y, Obj[19].transform.position.z);
        Obj[20].transform.position = new Vector3 (0 + Camera.main.aspect * 2f, Obj[20].transform.position.y, Obj[20].transform.position.z);
        //Tọa độ button swap
        Obj[13].transform.position = new Vector3 (0 - Camera.main.aspect * 6.5f, Obj[13].transform.position.y, Obj[13].transform.position.z);
        Obj[17].transform.position = new Vector3 (0 - Camera.main.aspect * 3.5f, Obj[17].transform.position.y, Obj[17].transform.position.z);
        //Tọa độ button ẩn remove hero khỏi đội hình
        Obj[14].transform.position = Obj[5].transform.position + new Vector3 (0, 1.5f, 0);
        Obj[15].transform.position = Obj[6].transform.position + new Vector3 (0, 1.5f, 0);
        Obj[16].transform.position = Obj[7].transform.position + new Vector3 (0, 1.5f, 0);
        //Tọa độ các slot item trang bị trước khi vào trận
        Obj[22].transform.position = new Vector3 (Obj[6].transform.position.x, Obj[22].transform.position.y, Obj[22].transform.position.z);
        Obj[21].transform.position = Obj[22].transform.position - new Vector3 (3f, 0, 0);
        Obj[23].transform.position = Obj[22].transform.position + new Vector3 (3f, 0, 0);
        //Object hình ảnh item trang bị trước khi vào trận
        ItemUseImg[0] = Obj[21].transform.GetChild (0).GetComponent<Image> ();
        ItemUseImg[1] = Obj[22].transform.GetChild (0).GetComponent<Image> ();
        ItemUseImg[2] = Obj[23].transform.GetChild (0).GetComponent<Image> ();
        Module.PrevScene = "Room";
        //ADS.RequestBanner(0);
        #region Khởi tạo hoặc set Canvas thông báo cho Scene 
        try {
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
        } catch {
            GameSystem.Initialize (); //Khởi tạo này dành cho scene nào test ngay
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
            GameSystem.MessageCanvas.GetComponent<Canvas> ().planeDistance = 1;
        }
        #endregion
        GameSystem.RunBGM (0);
        SetupHeroController ();
        SetupTeam ();
        SetupLanguage ();
        SetupItemUseFromInventory ();
        SetupItemEquipFromInventory ();
        SetupItemUseEquiped ();
        SetupHeroFromList ();
        Obj[28].SetActive (false); //Đóng popup
        Obj[24].SetActive (false); //Ẩn canvas UI
    }

    private void SetupLanguage () {
        TextLanguage[0].text = Languages.lang[118];
        TextLanguage[1].text = Languages.lang[144];
        TextLanguage[2].text = Languages.lang[100];
        TextLanguage[3].text = Languages.lang[62];
        TextLanguage[5].text = Languages.lang[261]; //"Remove";
        TextLanguage[6].text = Languages.lang[262]; //"Information";
        TextLanguage[7].text = Languages.lang[263]; //"Equip";
        TextLanguage[8].text = Languages.lang[264]; //"Change equip";
        TextLanguage[9].text = Languages.lang[79]; //"gỡ nhanh";
        TextLanguage[13].text = Languages.lang[22]; // = "Chỉ số";
        TextLanguage[14].text = "";
        for (int i = 200; i < 238; i++) {
            TextLanguage[14].text += Languages.lang[i] + "\n";
        }
    }

    private void SetupHeroController () {
        DataUserController.LoadHeroes ();
        HeroList = new List<GameObject> ();
        HeroListAvatar = new List<GameObject> ();
        EnemyListAvatar = new List<GameObject> ();
        float regionSpace = 250f; //Khoảng cách giữa các object
        var HeroCount = DataUserController.Heroes.DBHeroes.Count; //Biến tạm, số lượng hero đang sở hữu
        Obj[2].GetComponent<RectTransform> ().sizeDelta = new Vector2 (HeroCount * regionSpace, Obj[2].GetComponent<RectTransform> ().sizeDelta.y);
        float verticalcounttemp = Obj[2].GetComponent<RectTransform> ().sizeDelta.y / 2 - 125;
        var vecXTemp = 0 - Camera.main.aspect * 5.5f;
        //khởi tạo các object hero
        for (int i = 0; i < HeroCount; i++) {
            var temp = i;
            HeroList.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/HeroItemBig"), new Vector3 (-((HeroCount * regionSpace) / 2) + 140 + regionSpace * i, 50, 0), Quaternion.identity));
            HeroListAvatar.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroObjectAvtLink + "Hero" + (DataUserController.Heroes.DBHeroes[i].ID).ToString ()), new Vector3 (-1000, -1000, 0), Quaternion.identity));
            HeroListAvatar[i].transform.localScale = new Vector3 (.8f, .8f, .8f);
            HeroListAvatar[i].SetActive (false);
            HeroList[i].transform.SetParent (Obj[2].transform, false);
            HeroList[i].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (Module.AvatarHeroLink + DataUserController.Heroes.DBHeroes[i].ID);
            HeroList[i].transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => PustHeroToTeam (temp));

            //EnemyListAvatar.Add
        }

        if (Module.BattleModeSelected.Equals (0)) { //Nếu là chế độ chơi preview
            //Lấy danh sách enemy
            var listEnemy = DataUserController.User.EnemyFutureMap[Module.WorldMapRegionSelected].Split (';');
            //Khởi tạo enemy
            for (sbyte i = 0; i < listEnemy.Length; i++) {
                EnemyListAvatar.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroObjectAvtLink + "Hero" + listEnemy[i].ToString ()), new Vector3 (-1000, -1000, 0), Quaternion.identity));
                EnemyListAvatar[i].transform.localScale = new Vector3 (-.8f, .8f, .8f);
                EnemyListAvatar[i].transform.position = Obj[i + 18].transform.position + new Vector3 (listEnemy[i].Equals ("7") ? -.8f : 0, 1.5f, 0);
            }
        }
    }

    /// <summary>
    /// Hiển thị hero có trong team
    /// </summary>
    private void SetupTeam () {
        DataUserController.LoadTeam ();
        string[] temp = DataUserController.Team.Split (';');
        var count = DataUserController.Heroes.DBHeroes.Count;
        if (temp[0] != "0") {
            for (int i = 0; i < count; i++) {
                if (DataUserController.Heroes.DBHeroes[i].ID == int.Parse (temp[0])) {
                    HeroList[i].transform.GetChild (2).gameObject.SetActive (true);
                    HeroListAvatar[i].SetActive (true);
                    HeroListAvatar[i].transform.position = Obj[5].transform.position + new Vector3 (temp[0].Equals (7)?.8f : 0, 1.5f, 0);
                    break;
                }
            }
        }
        if (temp[1] != "0") {
            for (int i = 0; i < count; i++) {
                if (DataUserController.Heroes.DBHeroes[i].ID == int.Parse (temp[1])) {
                    HeroList[i].transform.GetChild (2).gameObject.SetActive (true);
                    HeroListAvatar[i].SetActive (true);
                    HeroListAvatar[i].transform.position = Obj[6].transform.position + new Vector3 (temp[1].Equals (7)?.8f : 0, 1.5f, 0);
                    break;
                }
            }
        }
        if (temp[2] != "0") {
            for (int i = 0; i < count; i++) {
                if (DataUserController.Heroes.DBHeroes[i].ID == int.Parse (temp[2])) {
                    HeroList[i].transform.GetChild (2).gameObject.SetActive (true);
                    HeroListAvatar[i].SetActive (true);
                    HeroListAvatar[i].transform.position = Obj[7].transform.position + new Vector3 (temp[2].Equals (7)?.8f : 0, 1.5f, 0);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Hiển thị các item use đã được trang bị trước khi vào trận
    /// </summary>
    private void SetupItemUseEquiped () {
        var itemUseEquip = DataUserController.User.ItemUseForBattle.Split (';'); //Lấy danh sách item use đã dc mang
        for (sbyte i = 0; i < itemUseEquip.Length; i++) {
            if (!string.IsNullOrEmpty (itemUseEquip[i])) //Nếu slot đang chọn đã trang bị item rồi
            {
                var thisItem = DataUserController.Inventory.DBItems.Find (x => x.ItemType.ToString () == itemUseEquip[i].Split (',') [0] && x.ItemID.ToString () == itemUseEquip[i].Split (',') [1]);
                if (thisItem != null) {
                    ItemUseImg[i].sprite = Resources.Load<Sprite> ("Images/Items/" + thisItem.ItemType + @"/" + thisItem.ItemID);
                    Obj[21 + i].transform.GetChild (2).GetComponent<Text> ().text = thisItem.Quantity.ToString ();
                } else {
                    itemUseEquip[i] = "";
                    ItemUseImg[i].sprite = Resources.Load<Sprite> ("Images/none");
                    Obj[21 + i].transform.GetChild (2).GetComponent<Text> ().text = "";
                }
            } else {
                ItemUseImg[i].sprite = Resources.Load<Sprite> ("Images/none");
                Obj[21 + i].transform.GetChild (2).GetComponent<Text> ().text = "";
            }
        }
        DataUserController.User.ItemUseForBattle = itemUseEquip[0] + ";" + itemUseEquip[1] + ";" + itemUseEquip[2];
        DataUserController.SaveUserInfor ();
    }

    /// <summary>
    /// Khởi tạo danh sách item use có trong inventory
    /// </summary>
    private void SetupItemUseFromInventory () {
        var listItemUse = DataUserController.Inventory.DBItems.Where (x => x.ItemType == 10 && x.ItemID >= 0 && x.ItemID <= 8).ToList (); //Lấy danh sách các bình máu để trang bị
        var totalItemEquip = listItemUse.Count;
        var HorizontalQuantityEquip = 8;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 145f; //Khoảng cách giữa các object
        float verticalcounttemp = totalItemEquip <= HorizontalQuantityEquip ? 0 : ((totalItemEquip % HorizontalQuantityEquip).Equals (0) ? (totalItemEquip - 1) / HorizontalQuantityEquip * 80 : (totalItemEquip / HorizontalQuantityEquip) * 80);
        float horizonXOriginal = -515;
        Obj[25].GetComponent<RectTransform> ().sizeDelta = totalItemEquip % HorizontalQuantityEquip == 0 ? new Vector2 (0, (totalItemEquip / HorizontalQuantityEquip) * regionSpace) : new Vector2 (0, ((totalItemEquip / HorizontalQuantityEquip) + 1) * regionSpace);
        for (sbyte i = 0; i < totalItemEquip; i++) {
            var temp = i;
            ListBackgroundItemUse.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListItemUse.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListBackgroundItemUse[i].transform.SetParent (Obj[25].transform, false); //Đẩy prefab vào scroll
            ListBackgroundItemUse[i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (horizonXOriginal + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0);
            ListBackgroundItemUse[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (130, 130);
            ListBackgroundItemUse[i].transform.localScale = new Vector3 (1, 1, 1);
            ListItemUse[i].transform.SetParent (ListBackgroundItemUse[i].transform, false);
            ListItemUse[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listItemUse[i].ItemType + @"/" + listItemUse[i].ItemID);
            ListItemUse[i].transform.GetChild (0).GetComponent<Text> ().text = "";
            ListItemUse[i].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ListItemUse[i].GetComponent<Button> ().onClick.AddListener (() => SelectItemToUse (listItemUse[temp]));
            ListItemUse[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (115, 115);
            ListItemUse[i].transform.GetChild (0).GetComponent<Text> ().text = listItemUse[i].Quantity.ToString ();
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantityEquip == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
    }

    /// <summary>
    /// Khởi tạo danh sách hero dành cho đổi trang bị nhanh
    /// </summary>
    private void SetupHeroFromList () {
        var listItemUse = DataUserController.Heroes.DBHeroes.ToList (); //Lấy danh sách các hero
        var totalItemEquip = listItemUse.Count;
        var HorizontalQuantityEquip = 8;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 145f; //Khoảng cách giữa các object
        float verticalcounttemp = totalItemEquip <= HorizontalQuantityEquip ? 0 : ((totalItemEquip % HorizontalQuantityEquip).Equals (0) ? (totalItemEquip - 1) / HorizontalQuantityEquip * 80 : (totalItemEquip / HorizontalQuantityEquip) * 80);
        float horizonXOriginal = -515;
        Obj[33].GetComponent<RectTransform> ().sizeDelta = totalItemEquip % HorizontalQuantityEquip == 0 ? new Vector2 (0, (totalItemEquip / HorizontalQuantityEquip) * regionSpace) : new Vector2 (0, ((totalItemEquip / HorizontalQuantityEquip) + 1) * regionSpace);
        for (sbyte i = 0; i < totalItemEquip; i++) {
            var temp = i;
            ListHeroChangeEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/HeroItemSmall"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListHeroChangeEquip[i].transform.SetParent (Obj[33].transform, false); //Đẩy prefab vào scroll
            ListHeroChangeEquip[i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (horizonXOriginal + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0);
            ListHeroChangeEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (130, 130);
            ListHeroChangeEquip[i].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("HeroAvt/" + DataUserController.Heroes.DBHeroes[i].ID);
            ListHeroChangeEquip[i].transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => FastChangeEquip (DataUserController.Heroes.DBHeroes[temp].ID));
            ListHeroChangeEquip[i].transform.localScale = new Vector3 (1, 1, 1);
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantityEquip == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
    }

    /// <summary>
    /// Khởi tạo danh sách item use có trong inventory
    /// </summary>
    private void SetupItemEquipFromInventory () {
        var listItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList (); //Lấy danh sách các item trang bị
        var totalItemEquip = listItemEquip.Count;
        var HorizontalQuantityEquip = 6;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 145f; //Khoảng cách giữa các object
        float verticalcounttemp = totalItemEquip <= HorizontalQuantityEquip ? 0 : ((totalItemEquip % HorizontalQuantityEquip).Equals (0) ? (totalItemEquip - 1) / HorizontalQuantityEquip * 72 : (totalItemEquip / HorizontalQuantityEquip) * 72);
        float horizonXOriginal = -360;
        //Thêm mới các object ban đầu nếu có sự chênh lệch số lượng
        if (ListBackgroundItemEquip.Count < totalItemEquip) {
            var count = totalItemEquip - ListBackgroundItemEquip.Count;
            for (int i = 0; i < count; i++) {
                ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (0, 0, 0), Quaternion.identity));
                ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            }
        }
        Obj[29].GetComponent<RectTransform> ().sizeDelta = totalItemEquip % HorizontalQuantityEquip == 0 ? new Vector2 (0, (totalItemEquip / HorizontalQuantityEquip) * regionSpace) : new Vector2 (0, ((totalItemEquip / HorizontalQuantityEquip) + 1) * regionSpace);
        //Set các object
        for (sbyte i = 0; i < totalItemEquip; i++) {
            var temp = i;
            ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListBackgroundItemEquip[i].transform.SetParent (Obj[29].transform, false); //Đẩy prefab vào scroll
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (horizonXOriginal + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0);
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (130, 130);
            ListBackgroundItemEquip[i].transform.localScale = new Vector3 (1, 1, 1);
            ListItemEquip[i].transform.SetParent (ListBackgroundItemEquip[i].transform, false);
            ListItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listItemEquip[i].ItemType + @"/" + listItemEquip[i].ItemID);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = "";
            ListItemEquip[i].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ListItemEquip[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (listItemEquip[temp], 1, 0));
            ListItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (115, 115);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + listItemEquip[i].ItemLevel.ToString ();
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantityEquip == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        Obj[29].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, Obj[29].GetComponent<RectTransform> ().anchoredPosition.y, 0);
        //Xóa object nếu dư
        if (ListBackgroundItemEquip.Count > totalItemEquip) {
            for (int i = ListBackgroundItemEquip.Count - 1; i >= totalItemEquip; i--) {
                Destroy (ListBackgroundItemEquip[i]);
                Destroy (ListItemEquip[i]);
                ListBackgroundItemEquip.RemoveAt (i);
                ListItemEquip.RemoveAt (i);
            }
        }
        
        //Đưa màu item về chuẩn
        for (int i = 0; i < totalItemEquip; i++) {
            if (i < totalItemEquip) {
                if (listItemEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6
                    ListBackgroundItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listItemEquip[i].ItemColor);
            } else {
                ListBackgroundItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0");
            }
        }
    }

    #endregion

    #region Functions 

    /// <summary>
    /// Đẩy hero có trong danh sách vào team
    /// </summary>
    /// <param name="slotHero"></param>
    private void PustHeroToTeam (int slotHero) {
        string[] temp = DataUserController.Team.Split (';');
        //Nếu đang tồn tại hero trong team
        if (HeroList[slotHero].transform.GetChild (2).gameObject.activeSelf) {
            for (int i = 0; i < temp.Length; i++) {
                if (temp[i].Equals (DataUserController.Heroes.DBHeroes[slotHero].ID.ToString ())) {
                    RemoveHeroFromTeam (i);
                    break;
                }
            }
        } else //Ko thì đẩy vào team
        {
            for (int i = 0; i < temp.Length; i++) {
                if (!temp[i].Equals ("0") && i.Equals (temp.Length - 1)) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[281]); //Team is full
                }
                if (temp[i].Equals ("0")) {
                    temp[i] = DataUserController.Heroes.DBHeroes[slotHero].ID.ToString ();
                    DataUserController.Team = temp[0] + ";" + temp[1] + ";" + temp[2];
                    DataUserController.SaveTeam ();
                    break;
                }
            }
        }
        SetupTeam ();
    }

    /// <summary>
    /// Ẩn object hero trong team
    /// </summary>
    /// <param name="idHero"></param>
    private void HiddenHeroAvt (int idHero) {
        var count = HeroList.Count;
        for (int i = 0; i < count; i++) {
            if (DataUserController.Heroes.DBHeroes[i].ID.Equals (idHero)) {
                HeroListAvatar[i].SetActive (false);
                HeroListAvatar[i].transform.position = new Vector3 (-1000, -1000, 0);
                break;
            }
        }
    }

    /// <summary>
    /// remove nhân vật cho slot 1,2,3
    /// </summary>
    public void RemoveHeroFromTeam (int slot) {
        string[] temp = DataUserController.Team.Split (';');
        if (temp[slot] != "0") {
            var count = HeroList.Count;
            for (int i = 0; i < count; i++) {
                if (temp[slot].Equals (DataUserController.Heroes.DBHeroes[i].ID.ToString ())) {
                    HeroList[i].transform.GetChild (2).gameObject.SetActive (false);
                    break;
                }
            }
            HiddenHeroAvt (int.Parse (temp[slot]));
            temp[slot] = "0";
            DataUserController.Team = temp[0] + ";" + temp[1] + ";" + temp[2];
            DataUserController.SaveTeam ();
            SetupTeam ();
        }
    }
    /// <summary>
    /// Swap nhân vật trong team
    /// </summary>
    /// <param name="order"></param>
    public void ButtonSwap (int order) {
        var count = HeroListAvatar.Count;
        for (int i = 0; i < count; i++) {
            HeroListAvatar[i].SetActive (false);
        }
        string[] temp = DataUserController.Team.Split (';');
        switch (order) {
            case 0:
                var swapstr = temp[0];
                temp[0] = temp[1];
                temp[1] = swapstr;
                DataUserController.Team = temp[0] + ";" + temp[1] + ";" + temp[2];
                break;
            case 1:
                swapstr = temp[1];
                temp[1] = temp[2];
                temp[2] = swapstr;
                DataUserController.Team = temp[0] + ";" + temp[1] + ";" + temp[2];
                break;
            default:
                break;
        }
        DataUserController.SaveTeam ();
        SetupTeam ();
    }

    /// <summary>
    /// Show các chức năng
    /// </summary>
    /// <param name="type"></param>
    public void ButtonShowFunctions (int type) {
        switch (type) {
            case 0:
                Obj[8].SetActive (true);
                break; //setting
            case 1:
                Obj[9].SetActive (true);
                break; //chatboxd
            case 2: //Đóng canvas UI
                Obj[27].SetActive (false); //Đóng form item use
                Obj[28].SetActive (false); //Đóng popup
                Obj[30].SetActive (false); //Đóng trang bị
                Obj[34].SetActive (false); //Đóng danh sách hero cho trang bị nhanh
                Obj[35].SetActive (false); //Đóng thông tin chi tiết nhân vật
                Obj[24].SetActive (false); //Ẩn canvas UI
                break;
                #region 3 nút trang bị item cho trận đấu 

            case 3:
                SlotItemUseSelected = type - 3;
                Obj[27].SetActive (true); //Show list item use
                Obj[24].SetActive (true); //Show canvas UI
                break;

            case 4:
                SlotItemUseSelected = type - 3;
                Obj[27].SetActive (true); //Show list item use
                Obj[24].SetActive (true); //Show canvas UI
                break;

            case 5:
                SlotItemUseSelected = type - 3;
                Obj[27].SetActive (true); //Show list item use
                Obj[24].SetActive (true); //Show canvas UI
                break;

                #endregion

                #region 3 nút Show popup khi click vào nhân vật 

            case 6:
                Obj[28].transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x + 6f, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
                SlotTeamSelected = type - 6;
                var listHeroTeam1 = DataUserController.Team.Split (';');
                SlotHeroSelected = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == Convert.ToInt32 (listHeroTeam1[SlotTeamSelected]));
                Obj[28].SetActive (true); //Show popup
                Obj[24].SetActive (true); //Show canvas UI
                break;
            case 7:
                Obj[28].transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x + 6f, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
                SlotTeamSelected = type - 6;
                var listHeroTeam2 = DataUserController.Team.Split (';');
                SlotHeroSelected = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == Convert.ToInt32 (listHeroTeam2[SlotTeamSelected]));
                Obj[28].SetActive (true); //Show popup
                Obj[24].SetActive (true); //Show canvas UI
                break;
            case 8:
                Obj[28].transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x + 6f, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
                SlotTeamSelected = type - 6;
                var listHeroTeam3 = DataUserController.Team.Split (';');
                SlotHeroSelected = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == Convert.ToInt32 (listHeroTeam3[SlotTeamSelected]));
                Obj[28].SetActive (true); //Show popup
                Obj[24].SetActive (true); //Show canvas UI
                break;

                #endregion
            case 9: //Xóa nhân vật khỏi đội hình
                RemoveHeroFromTeam (SlotTeamSelected);
                ButtonShowFunctions (2);
                break;
            case 10: //Trang bị cho nhân vật
                Obj[30].SetActive (true);
                Obj[28].SetActive (false); //Ẩn popup
                ShowItemEquiped ();
                break;
            case 11: //Đóng form chi tiết item
                Obj[31].SetActive (false);
                break;
            case 12: //Gỡ trang bị nhanh
                if (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count > 0) { //Nếu có trang bị thì mới gỡ
                    if (DataUserController.User.InventorySlot - DataUserController.Inventory.DBItems.Count >= DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count) {
                        var count = DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count;
                        for (int i = 0; i < count; i++) {
                            var itemTemp = DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip[0];
                            InventorySystem.AddItemToInventory (itemTemp);
                            DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.RemoveAt (0);
                        }
                        DataUserController.SaveInventory ();
                        DataUserController.SaveHeroes ();
                        SetupItemEquipFromInventory ();
                        ShowItemEquiped ();
                        ItemViewing = null;
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                    } else {
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                    }
                }
                break;
            case 13: //Show form danh sách hero để đổi trang bị nhanh
                Obj[28].SetActive (false); //Ẩn popup
                var listHeroTeam = DataUserController.Team.Split (';');
                if (DataUserController.Heroes.DBHeroes.Find (x => x.ID == Convert.ToInt32 (listHeroTeam[SlotTeamSelected])).ItemsEquip.Count > 0) { //Nếu có item đang được trang bị
                    Obj[34].SetActive (true); //Show form
                    Obj[24].SetActive (true); //Show canvas UI
                } else {
                    Obj[24].SetActive (false); //Show canvas UI
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[267])); //Nhân vật chưa được trang bị
                }
                break;
            case 14: //Thông tin chi tiết nhân vật
                CaculatorValueHeroes (SlotHeroSelected);
                Obj[28].SetActive (false); //Ẩn popup
                Obj[35].SetActive (true);
                break;
            case 15: //Hiển thị UI Inventory
              GameSystem.InitializePrefabUI(1);
               // StartCoroutine(WaitingCloseUI(1)); //Chờ đóng UI
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chờ giao diện Inventory đóng lại
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator WaitingCloseUI(int slot)
    {
        switch (slot)
        {
            case 1:
                yield return new WaitUntil(() => GameSystem.InventoryUI == null);
                //Success
                if (GameSystem.InventoryUI == null)
                {
                    //ButtonFunctions(8); //Refresh lại giá trị tiền tệ
                }
                break;
            default:break;
        }
    }

    /// <summary>
    /// Tính toán lại chỉ số của heroes
    /// </summary>
    private void CaculatorValueHeroes (int slotHero) {

        #region Tính toán chỉ số lúc đeo trang bị 

        var vAtkPlus = 0f; //Sát thương vật lý
        var vMagicPlus = 0f; //Sát thương phép thuật
        var vHealthPlus = 0f; //Máu
        var vManaPlus = 0f; //Mana
        var vArmorPlus = 0f; //Giáp
        var vMagicResistPlus = 0f; //Kháng phép
        var vHealthRegenPlus = 0f; //Chỉ số hồi máu mỗi giây
        var vManaRegenPlus = 0f; //Chỉ số hồi mana mỗi giây
        var vDamageEarthPlus = 0f; //Sát thương hệ đất
        var vDamageWaterPlus = 0f; //Sát thương hệ nước
        var vDamageFirePlus = 0f; //Sát thương hệ lửa
        var vDefenseEarthPlus = 0f; //Kháng hệ đất
        var vDefenseWaterPlus = 0f; //Kháng hệ nước
        var vDefenseFirePlus = 0f; //Kháng hệ hỏa
        var vAtkSpeedPlus = 0f; //% Tốc độ tấn công cơ bản tăng thêm
        var vLifeStealPhysicPlus = 0f; //% hút máu
        var vLifeStealMagicPlus = 0f; //% hút máu phép
        var vLethalityPlus = 0f; //% Xuyên giáp
        var vMagicPenetrationPlus = 0f; //% Xuyên phép
        var vCriticalPlus = 0f; //% chí mạng
        var vTenacityPlus = 0f; //% kháng hiệu ứng
        var vCooldownReductionPlus = 0f; //% Giảm tgian hồi chiêu
        var vDamageExcellentPlus = 0f; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
        var vDefenseExcellentPlus = 0f; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
        var vDoubleDamagePlus = 0f; //Tỉ lệ x2 đòn đánh max = 10%
        var vTripleDamagePlus = 0f; //Tỉ lệ x3 đòn đánh max = 10%
        var vDamageReflectPlus = 0f; //Phản hồi % sát thương. max = 5%
        var vRewardPlusPlus = 0f; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

        var countItemEquip = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip.Count > 0 ? DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip.Count : 0;
        if (countItemEquip > 0) {
            for (int i = 0; i < countItemEquip; i++) {
                //Sát thương vật lý
                var valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtk; //Chỉ số gốc
                var valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                var valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                var valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkPlus / 100f : 0; //% tăng thêm
                vAtkPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Sát thương phép
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagic; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPlus / 100f : 0; //% tăng thêm
                vMagicPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Máu
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealth; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthPlus / 100f : 0; //% tăng thêm
                vHealthPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Năng lượng
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMana; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaPlus / 100f : 0; //% tăng thêm
                vManaPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Giáp
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmor; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmorPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmorPlus / 100f : 0; //% tăng thêm
                vArmorPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Kháng phép
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResist; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResistPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResistPlus / 100f : 0; //% tăng thêm
                vMagicResistPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);

                vHealthRegenPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthRegen; //Chỉ số hồi máu mỗi giây
                vManaRegenPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaRegen; //Chỉ số hồi mana mỗi giây
                vDamageEarthPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageEarth; //Sát thương hệ đất
                vDamageWaterPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageWater; //Sát thương hệ nước
                vDamageFirePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageFire; //Sát thương hệ lửa
                vDefenseEarthPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseEarth; //Kháng hệ đất
                vDefenseWaterPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseWater; //Kháng hệ nước
                vDefenseFirePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseFire; //Kháng hệ hỏa
                vAtkSpeedPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkSpeed; //% Tốc độ tấn công cơ bản tăng thêm
                vLifeStealPhysicPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLifeStealPhysic; //% hút máu
                vLifeStealMagicPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLifeStealMagic; //% hút máu phép
                vLethalityPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLethality; //% Xuyên giáp
                vMagicPenetrationPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPenetration; //% Xuyên phép
                vCriticalPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vCritical; //% chí mạng
                vTenacityPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vTenacity; //% kháng hiệu ứng
                vCooldownReductionPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vCooldownReduction; //% Giảm tgian hồi chiêu
                vDamageExcellentPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageExcellent; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
                vDefenseExcellentPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseExcellent; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
                vDoubleDamagePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDoubleDamage; //Tỉ lệ x2 đòn đánh max = 10%
                vTripleDamagePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vTripleDamage; //Tỉ lệ x3 đòn đánh max = 10%
                vDamageReflectPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageReflect; //Phản hồi % sát thương. max = 5%
                vRewardPlusPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vRewardPlus; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

            }
        }

        TextLanguage[10].text = DataUserController.Heroes.DBHeroes[slotHero].Name; //Tên hero
        TextLanguage[12].text = Languages.lang[23] + DataUserController.Heroes.DBHeroes[slotHero].Level.ToString (); //Cấp độ nhân vật
        Obj[36].transform.localScale = new Vector3 (DataUserController.Heroes.DBHeroes[slotHero].EXP / Module.NextExp (DataUserController.Heroes.DBHeroes[slotHero].Level), 1, 1); //Thanh exp
        var heroOriginal = DataUserController.HeroesDefault.DBHeroesDefault.Find (x => x.ID == DataUserController.Heroes.DBHeroes[slotHero].ID);
        TextLanguage[11].text = Languages.lang[(int) heroOriginal.Type + 15]; //Class nhân vật

        TextLanguage[15].text = String.Format ("{0:0.#}", (heroOriginal.vHealth + vHealthPlus + (heroOriginal.vHealthPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMana) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vAtk + vAtkPlus + (heroOriginal.vAtkPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagic + vMagicPlus + (heroOriginal.vMagicPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vArmor + vArmorPlus + (heroOriginal.vArmorPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagicResist + vMagicResistPlus + (heroOriginal.vMagicResistPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vHealthRegen + vHealthRegenPlus + (heroOriginal.vHealthRegenPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vManaRegen + vManaRegenPlus + (heroOriginal.vManaRegenPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageEarth + vDamageEarthPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageWater + vDamageWaterPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageFire + vDamageFirePlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseEarth + vDefenseEarthPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseWater + vDefenseWaterPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseFire + vDefenseFirePlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vAtkSpeed + (heroOriginal.vAtkSpeed * vAtkSpeedPlus / 100f))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vCooldown[1])) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLifeStealPhysic + vLifeStealPhysicPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLifeStealMagic + vLifeStealMagicPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLethality + vLethalityPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagicPenetration + vMagicPenetrationPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vCritical + vCriticalPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vTenacity + vTenacityPlus)) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vCooldownReduction + vCooldownReductionPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDamageExcellent + vDamageExcellentPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDefenseExcellent + vDefenseExcellentPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDoubleDamage + vDoubleDamagePlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vTripleDamage + vTripleDamagePlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDamageReflect + vDamageReflectPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vRewardPlus + vRewardPlusPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.Mana_skill[0]) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.Mana_skill[1]) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vHealthPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vAtkPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMagicPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vArmorPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMagicResistPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vHealthRegenPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vManaRegenPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vCooldownReductionPerLevel);
        #endregion
    }

    /// <summary>
    /// Lựa chọn item use để trang bị cho trận đấu
    /// </summary>
    /// <param name="guidID"></param>
    private void SelectItemToUse (ItemModel item) {
        var itemUseEquip = DataUserController.User.ItemUseForBattle.Split (';'); //Lấy danh sách item use đã dc mang
        for (sbyte i = 0; i < itemUseEquip.Length; i++) {
            if (!string.IsNullOrEmpty (itemUseEquip[i])) //Nếu slot đang chọn đã trang bị item rồi
            {
                if (itemUseEquip[i].Split (',') [0] == item.ItemType.ToString () && itemUseEquip[i].Split (',') [1] == item.ItemID.ToString ()) //Check trùng với item truyền vào
                { //Nếu trùng thì bỏ slot trùng
                    itemUseEquip[i] = "";
                    ItemUseImg[i].sprite = Resources.Load<Sprite> ("Images/none");
                }
            }
        }
        itemUseEquip[SlotItemUseSelected] = item.ItemType + "," + item.ItemID;
        DataUserController.User.ItemUseForBattle = itemUseEquip[0] + ";" + itemUseEquip[1] + ";" + itemUseEquip[2];
        ItemUseImg[SlotItemUseSelected].sprite = Resources.Load<Sprite> ("Images/Items/" + item.ItemType + @"/" + item.ItemID);
        Obj[27].SetActive (false); //Ẩn danh sách item
        Obj[24].SetActive (false); //Ẩn canvas
        DataUserController.SaveUserInfor ();
    }

    /// <summary>
    /// Hiển thị các item đã được trang bị cho hero
    /// </summary>
    private void ShowItemEquiped () {
        var listHeroTeam = DataUserController.Team.Split (';');
        if (listHeroTeam[SlotTeamSelected] != "0") {
            var thisHero = DataUserController.Heroes.DBHeroes.Find (x => x.ID == Convert.ToInt32 (listHeroTeam[SlotTeamSelected])); //Lấy hero đang xem trong team
            SlotHeroSelected = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == Convert.ToInt32 (listHeroTeam[SlotTeamSelected])); //Lấy slot hero trong toàn danh sách
            var count = thisHero.ItemsEquip.Count;

            for (int i = 0; i < 6; i++) {
                ListItemEquiped[i].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/none");
                ListItemEquiped[i].transform.GetChild (0).GetComponent<Button> ().onClick.RemoveAllListeners ();
            }
            for (int i = 0; i < 6; i++) {
                if (i < count) {
                    var temp = i;
                    ListItemEquiped[i].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + thisHero.ItemsEquip[i].ItemType + @"/" + thisHero.ItemsEquip[i].ItemID);
                    ListItemEquiped[i].transform.GetChild (0).GetComponent<Button> ().onClick.RemoveAllListeners ();
                    ListItemEquiped[i].transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => ItemClick (thisHero.ItemsEquip[temp], 2, 1));
                    if (thisHero.ItemsEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6 (xem trong module phần inventory)
                        ListItemEquiped[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + thisHero.ItemsEquip[i].ItemColor);
                    ListItemEquiped[i].transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + thisHero.ItemsEquip[i].ItemLevel.ToString ();

                } else {
                    ListItemEquiped[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0");
                    ListItemEquiped[i].transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "";
                }
            }
        }
    }

    /// <summary>
    /// Hiển thị thông tin item khi click vào
    /// </summary>
    /// <param name="item">Item</param>
    /// <param name="actionType">Kiểu thao tác: 0-Bán trang bị, 1-Trang bị, 2-Gỡ trang bị</param>
    /// <param name="type">0= xem trong inventory, 1 = xem trong hero</param>
    private void ItemClick (ItemModel item, int actionType, int type) {
        GlobalVariables.ItemViewingType = actionType;
        GlobalVariables.ItemViewing = item;
        GameSystem.InitializePrefabUI (6); //Hiển thị thông tin item click vào
        StartCoroutine (WaitingCloseItemDetailUI (type));
        // GameSystem.ShowItemIformation (item, actionType, isShowUpgrade, isShowUpColor, isShowSeparate, isShowNextItem); //hiển thị item
        // ItemViewing = item; //Gán item đang xem để thao tác nếu có lệnh
        // StartCoroutine (WaitActionFromItemInformationBox ()); //Chờ thao tác từ box information
    }

    /// <summary>
    /// Chờ đóng UI chi tiết item 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitingCloseItemDetailUI (int? type) {
        yield return new WaitUntil (() => GameSystem.ItemDetailCanvasUI == null);
        //Success
        if (GameSystem.ItemDetailCanvasUI == null) {
            switch (GlobalVariables.ItemDetailAction) {
                case 1: //Trang bị item
                    if (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count < 6) { //Check slot equip hero
                        DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Add (GlobalVariables.ItemViewing);
                        InventorySystem.RemoveItem (GlobalVariables.ItemViewing);
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[129])); //Trang bị thành công
                    } else {
                        GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[128])); //Không thể trang bị thêm
                    }
                    break;
                case 2: //Gỡ trang bị item
                    if (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count > 0) { //Nếu có trang bị thì mới gỡ
                        if (DataUserController.Inventory.DBItems.Count < DataUserController.User.InventorySlot) { //Check slot inventory
                            DataUserController.Inventory.DBItems.Add (GlobalVariables.ItemViewing);
                            DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.RemoveAt (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.FindIndex (x => x.ItemGuidID == GlobalVariables.ItemViewing.ItemGuidID));
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                        } else {
                            GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                        }
                    }
                    break;
                case 5: //Phân giải trang bị thành công
                    if (type.Equals (1)) //Nếu phân giải item từ trang bị nhân vật => remove item khỏi nhân vật
                        StartCoroutine (BreakItemFromHero ());
                    StartCoroutine (GameSystem.ControlFunctions.ShowMessagecontinuity (GlobalVariables.NotificationText)); //GameSystem.ControlFunctions.ShowMessage( (GlobalVariables.NotificationText));//Thông báo đã nhận dc những gì
                    break;
            }
            //ButtonFunctions (2); //Refresh lại giá trị tiền tệ

            //Thiết lập lại các giá trị                        GlobalVariables.ItemViewing = null;
            GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
            SetupItemEquipFromInventory ();
            ShowItemEquiped ();
            DataUserController.SaveInventory ();
            DataUserController.SaveHeroes ();
        }
    }
    
    /// <summary>
    /// Phân giải item trang bị từ nhân vật
    /// </summary>
    /// <returns></returns>
    private IEnumerator BreakItemFromHero () {
        try {
            var count = DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count;
            if (count > 0) //Nếu có trang bị thì mới break
            {
                //Xóa item trang bị khỏi nhân vật
                DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.RemoveAt (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.FindIndex(x => x.ItemGuidID == GlobalVariables.ItemViewing.ItemGuidID));
            }
        } catch { }
        yield return null;
    }

    /// <summary>
    /// Chờ thao tác từ box item information. -2 = đóng form, -1 = wait, 0 = sell, 1 = equip, 2 = remove, 3 = up level, 4 = up color, 5 = disassemble
    /// </summary>
    private IEnumerator WaitActionFromItemInformationBox () {
        yield return new WaitUntil (() => GameSystem.ActionItemInformation != -1);
        //Actions
        switch (GameSystem.ActionItemInformation) {
            case 0: //sell
                break;
            case 1: //equip
                if (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count < 6) { //Check slot equip hero
                    DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Add (ItemViewing);
                    InventorySystem.RemoveItem (ItemViewing);
                    DataUserController.SaveInventory ();
                    DataUserController.SaveHeroes ();
                    SetupItemEquipFromInventory ();
                    ShowItemEquiped ();
                    ItemViewing = null;
                    GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[129])); //Trang bị thành công
                } else {
                    GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[128])); //Không thể trang bị thêm
                }
                break;
            case 2: //remove
                if (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.Count > 0) { //Nếu có trang bị thì mới gỡ
                    if (DataUserController.Inventory.DBItems.Count < DataUserController.User.InventorySlot) { //Check slot inventory
                        DataUserController.Inventory.DBItems.Add (ItemViewing);
                        DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.RemoveAt (DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip.FindIndex (x => x.ItemGuidID == ItemViewing.ItemGuidID));
                        DataUserController.SaveInventory ();
                        DataUserController.SaveHeroes ();
                        SetupItemEquipFromInventory ();
                        ShowItemEquiped ();
                        ItemViewing = null;
                        GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                    } else {
                        GameSystem.ItemInformation.SetActive (false); //Ẩn box item information
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                    }
                }
                break;
            case 3: //up level
                break;
            case 4: //up color
                break;
            case 5: //disassemble - phân giải
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Thực hiện thay đổi nhanh trang bị giữa 2 nhân vật
    /// </summary>
    private void FastChangeEquip (int heroID) {
        // var listHeroTeam = DataUserController.Team.Split (';');
        // SlotHeroSelected = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == Convert.ToInt32 (listHeroTeam[SlotTeamSelected]));
        int slotHeroChange = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == heroID);
        if (!DataUserController.Heroes.DBHeroes[SlotHeroSelected].ID.Equals (heroID)) //Nếu chọn khác với chính hero đó => thực hiện đổi
        {
            List<ItemModel> equipTemp = new List<ItemModel> ();
            equipTemp = DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip;
            DataUserController.Heroes.DBHeroes[SlotHeroSelected].ItemsEquip = DataUserController.Heroes.DBHeroes[slotHeroChange].ItemsEquip;
            DataUserController.Heroes.DBHeroes[slotHeroChange].ItemsEquip = equipTemp;
            DataUserController.SaveHeroes ();
            ButtonShowFunctions (2); //Đóng UI
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[268])); //Đổi trang bị thành công
        } else {
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[266])); //Không thể đổi trang bị cho chính mình
        }
    }
    #endregion

    // /// <summary>
    // /// Chờ xác nhận có update phiên bản mới hay ko
    // /// </summary>
    // /// <returns></returns>
    // private IEnumerator ActionWaitForConfirmUpdateNewVersion () {
    //     yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
    //     //Accept
    //     if (GameSystem.ConfirmBoxResult == 1) {
    //         StartCoroutine (DownloadNewVersion (SyncData.ServerString.Split (';') [1]));
    //     }
    // }

    // /// <summary>
    // /// Download file
    // /// </summary>
    // /// <param name="URL"></param>
    // /// <returns></returns>
    // private IEnumerator DownloadNewVersion (string URL) {
    //     WWW www = new WWW (URL);
    //     //	yield return www;
    //     GameSystem.ShowConfirmDialog ("Downloading\n");

    //     while (!www.isDone) {
    //         //Debug.Log( www.progress * 100 ) ;
    //         GameSystem.ConfirmBoxText.text = string.Format ("Downloading\n {0:#,###.##}  Mb", www.bytesDownloaded / 1000000f);
    //         yield return null;
    //     }
    //     string savePath = Application.persistentDataPath + "/Update.apk";
    //     byte[] bytes = www.bytes;
    //     File.WriteAllBytes (savePath, bytes);
    //     //PlayerPrefs.SetInt ("Version", Convert.ToInt32(version [0]));
    //     Application.OpenURL (savePath);
    //     //        AndroidContentOpenerWrapper.OpenContent (savePath);
    // }

    public void ButtonTest () {
        StartCoroutine (GameSystem.FadeOut (GameSystem.BGM, 1f)); //Nhỏ dần nhạc nền rồi tắt

    }

}