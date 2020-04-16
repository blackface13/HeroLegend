using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InsertSocketController : MonoBehaviour {

    #region Variables 

    private bool IsRefreshList = false; //Có refresh lại danh sách hay ko
    private int FunctionMode = 0; //0 = insert, 1 = gỡ socket, 2 = đục lỗ
    public GameObject[] ObjectController;
    public Text[] TextUI;
    private int HorizontalQuantity = 13; //Số slot trên 1 hàng ngang (được tính toán lại ở phía dưới, vì nó scale theo tỉ lệ màn hình mỗi thiết bị)
    private GameObject[] ListItemSlot;
    private GameObject[] ListItemEquipHaveSocket;
    private GameObject[] ListItemNotSocketSlot;
    private GameObject[] ListItemEquipHantSocket;
    private GameObject[] ListItemSocketSlot;
    private GameObject[] ListItemSocketInventory;
    private bool BtnFunctionIsLeft = true;
    [Header ("Draw Curve")]
    public AnimationCurve moveCurve;
    Vector3 PositionSelecting; //Tọa độ item được chọn
    ItemModel ItemSelecting; //Item đang dc lựa chọn, biến tạm
    ItemModel ItemEquipSelected; //Item equip đang dc lựa chọn, biến tạm
    ItemModel ItemSocketSelected; //Item equip đang dc lựa chọn, biến tạm
    AudioClip[] SoundClip; //Âm thanh của skill
    bool IsShowItemEquip = true; //Show danh sách item trang bị
    List<GameObject> EffectCraftSuccess;
    private float[] RotatePosButtonChangeMode = new float[] { 30, -90, -210 }; //Tọa độ Z của button chuyển chế độ

    #endregion

    #region Initialize 

    void Start () {
        SoundClip = new AudioClip[3];
        TextUI[4].text = TextUI[2].text = Languages.lang[297]; //Kham ngoc
        TextUI[5].text = TextUI[6].text = "0";

        SoundClip[0] = Resources.Load<AudioClip> ("Audio/SE/InsertItem"); //Insert item to slot
        SoundClip[1] = Resources.Load<AudioClip> ("Audio/SE/ThrowItem"); //Thow item out slot
        SoundClip[2] = Resources.Load<AudioClip> ("Audio/SE/CraftSuccess"); //CraftSuccess

        ObjectController[25].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + ItemCoreSetting.CreateSocketItemType + @"/" + ItemCoreSetting.CreateSocketItemID); //Hình item khuôn tạo socket
        SetupItemEquipHaveSocket (false, false);
        SetupItemEquipHantSocket (false, true);
        SetupItemSocket (false, true);
        GeneralFunctions (6); //Update tiền
        //Khởi tạo list object effect ban đầu
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        EffectCraftSuccess[0].transform.SetParent (ObjectController[2].transform, false);
    }

    /// <summary>
    /// Khởi tạo danh sách item có lỗ
    /// </summary>
    private void SetupItemEquipHaveSocket (bool isDestroy, bool isHidden) {

        //Xóa toàn bộ object
        if (isDestroy) {
            for (int i = 0; i < ListItemEquipHaveSocket.Length; i++) {
                Destroy (ListItemEquipHaveSocket[i]);
                Destroy (ListItemSlot[i]);
            }
        }

        var listItemHaveSocket = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == ItemModel.TypeMode.Equip && x.vSocketSlot > 0).ToList (); //Lấy danh sách các trang bị có thể khảm socket

        //Khởi tạo khung, tính giá trị hàng ngang
        HorizontalQuantity = Convert.ToInt32 (ObjectController[0].GetComponent<RectTransform> ().rect.width) / 234; //Tính lại số lượng item trên 1 hàng theo scale của màn hình
        float regionSpace = (ObjectController[0].GetComponent<RectTransform> ().rect.width - 70) / HorizontalQuantity; //Khoảng cách giữa các object
        ListItemEquipHaveSocket = new GameObject[listItemHaveSocket.Count]; //Khởi tạo mảng danh sách item
        ListItemSlot = new GameObject[ListItemEquipHaveSocket.Length]; //Khởi tạo mảng danh sách slot item
        ObjectController[1].GetComponent<RectTransform> ().sizeDelta = ListItemEquipHaveSocket.Length % HorizontalQuantity == 0 ? new Vector2 (0, (ListItemEquipHaveSocket.Length / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((ListItemEquipHaveSocket.Length / HorizontalQuantity) + 1) * regionSpace + 10);
        var layoutRect = new Vector2 (ObjectController[0].GetComponent<RectTransform> ().rect.width, ObjectController[0].GetComponent<RectTransform> ().rect.height); //Lấy kích thước khung layout
        var startPosition = new Vector3 (0 - layoutRect.x / 2 + 150, 0 + ObjectController[1].GetComponent<RectTransform> ().rect.height / 2 - 115, 0); //Tính điểm bắt đầu cho item thứ 1
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc

        //Fill object lên màn hình
        for (int i = 0; i < ListItemEquipHaveSocket.Length; i++) {

            //Khởi tạo item
            ListItemSlot[i] = Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (startPosition.x + regionSpace * i_temp_x, startPosition.y + regionSpace * -i_temp_y, 0), Quaternion.identity);
            ListItemEquipHaveSocket[i] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity);
            ListItemEquipHaveSocket[i].transform.SetParent (ListItemSlot[i].transform, false);
            ListItemSlot[i].transform.SetParent (ObjectController[1].transform, false);

            //Gán hình ảnh, thông số của item
            var temp = i;
            ListItemSlot[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listItemHaveSocket[i].ItemColor); //Phẩm chất item
            ListItemEquipHaveSocket[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + listItemHaveSocket[i].ItemLevel.ToString (); //Level item
            ListItemEquipHaveSocket[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listItemHaveSocket[i].ItemType + @"/" + listItemHaveSocket[i].ItemID); //Hình item
            ListItemEquipHaveSocket[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, listItemHaveSocket[temp], null, ListItemSlot[temp].transform.position));

            //Xuống dòng
            i_temp_x++;
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        ObjectController[7].SetActive (!isHidden);
    }

    /// <summary>
    /// Khởi tạo danh sách item ko có lỗ
    /// </summary>
    private void SetupItemEquipHantSocket (bool isDestroy, bool isHidden) {

        //Xóa toàn bộ object
        if (isDestroy) {
            for (int i = 0; i < ListItemEquipHantSocket.Length; i++) {
                Destroy (ListItemEquipHantSocket[i]);
                Destroy (ListItemNotSocketSlot[i]);
            }
        }

        var listItemHaveSocket = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == ItemModel.TypeMode.Equip && x.vSocketSlot < ItemCoreSetting.vSocketSlotMax).ToList (); //Lấy danh sách các trang bị có lỗ khảm < 6

        //Khởi tạo khung, tính giá trị hàng ngang
        HorizontalQuantity = Convert.ToInt32 (ObjectController[0].GetComponent<RectTransform> ().rect.width) / 234; //Tính lại số lượng item trên 1 hàng theo scale của màn hình
        float regionSpace = (ObjectController[0].GetComponent<RectTransform> ().rect.width - 70) / HorizontalQuantity; //Khoảng cách giữa các object
        ListItemEquipHantSocket = new GameObject[listItemHaveSocket.Count]; //Khởi tạo mảng danh sách item
        ListItemNotSocketSlot = new GameObject[ListItemEquipHantSocket.Length]; //Khởi tạo mảng danh sách slot item
        ObjectController[21].GetComponent<RectTransform> ().sizeDelta = ListItemEquipHantSocket.Length % HorizontalQuantity == 0 ? new Vector2 (0, (ListItemEquipHantSocket.Length / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((ListItemEquipHantSocket.Length / HorizontalQuantity) + 1) * regionSpace + 10);
        var layoutRect = new Vector2 (ObjectController[0].GetComponent<RectTransform> ().rect.width, ObjectController[0].GetComponent<RectTransform> ().rect.height); //Lấy kích thước khung layout
        var startPosition = new Vector3 (0 - layoutRect.x / 2 + 150, 0 + ObjectController[21].GetComponent<RectTransform> ().rect.height / 2 - 115, 0); //Tính điểm bắt đầu cho item thứ 1
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc

        //Fill object lên màn hình
        for (int i = 0; i < ListItemEquipHantSocket.Length; i++) {

            //Khởi tạo item
            ListItemNotSocketSlot[i] = Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (startPosition.x + regionSpace * i_temp_x, startPosition.y + regionSpace * -i_temp_y, 0), Quaternion.identity);
            ListItemEquipHantSocket[i] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity);
            ListItemEquipHantSocket[i].transform.SetParent (ListItemNotSocketSlot[i].transform, false);
            ListItemNotSocketSlot[i].transform.SetParent (ObjectController[21].transform, false);

            //Gán hình ảnh, thông số của item
            var temp = i;
            ListItemNotSocketSlot[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listItemHaveSocket[i].ItemColor); //Phẩm chất item
            ListItemEquipHantSocket[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + listItemHaveSocket[i].ItemLevel.ToString (); //Level item
            ListItemEquipHantSocket[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listItemHaveSocket[i].ItemType + @"/" + listItemHaveSocket[i].ItemID); //Hình item
            ListItemEquipHantSocket[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, listItemHaveSocket[temp], null, ListItemNotSocketSlot[temp].transform.position));

            //Xuống dòng
            i_temp_x++;
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }

        ObjectController[20].SetActive (!isHidden);
    }

    /// <summary>
    /// Khởi tạo danh sách item socket
    /// </summary>
    /// <param name="isDestroy">= true khi refresh sau khi khảm ngọc</param>
    private void SetupItemSocket (bool isDestroy, bool isHidden) {

        //Xóa toàn bộ object
        if (isDestroy) {
            for (int i = 0; i < ListItemSocketInventory.Length; i++) {
                Destroy (ListItemSocketInventory[i]);
                Destroy (ListItemSocketSlot[i]);
            }
        }

        //Khởi tạo các object
        var listItemSocket = DataUserController.Inventory.DBItems.Where (x => x.ItemType == 12).ToList (); //Lấy danh sách các socket

        //Khởi tạo khung, tính giá trị hàng ngang
        float regionSpace = (ObjectController[0].GetComponent<RectTransform> ().rect.width - 70) / HorizontalQuantity; //Khoảng cách giữa các object
        ListItemSocketInventory = new GameObject[listItemSocket.Count]; //Khởi tạo mảng danh sách item
        ListItemSocketSlot = new GameObject[ListItemSocketInventory.Length]; //Khởi tạo mảng danh sách slot item
        ObjectController[6].GetComponent<RectTransform> ().sizeDelta = ListItemSocketInventory.Length % HorizontalQuantity == 0 ? new Vector2 (0, (ListItemSocketInventory.Length / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((ListItemSocketInventory.Length / HorizontalQuantity) + 1) * regionSpace + 10);
        var layoutRect = new Vector2 (ObjectController[0].GetComponent<RectTransform> ().rect.width, ObjectController[0].GetComponent<RectTransform> ().rect.height); //Lấy kích thước khung layout
        var startPosition = new Vector3 (0 - layoutRect.x / 2 + 150, 0 + ObjectController[6].GetComponent<RectTransform> ().rect.height / 2 - 115, 0); //Tính điểm bắt đầu cho item thứ 1
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc

        //Fill object lên màn hình
        for (int i = 0; i < ListItemSocketInventory.Length; i++) {

            //Khởi tạo item
            ListItemSocketSlot[i] = Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (startPosition.x + regionSpace * i_temp_x, startPosition.y + regionSpace * -i_temp_y, 0), Quaternion.identity);
            ListItemSocketInventory[i] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity);
            ListItemSocketInventory[i].transform.SetParent (ListItemSocketSlot[i].transform, false);
            ListItemSocketSlot[i].transform.SetParent (ObjectController[6].transform, false);

            //Gán hình ảnh, thông số của item
            var temp = i;
            ListItemSocketSlot[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listItemSocket[i].ItemColor); //Phẩm chất item
            ListItemSocketInventory[i].transform.GetChild (0).GetComponent<Text> ().text = listItemSocket[i].Quantity.ToString (); //Số lượng item
            ListItemSocketInventory[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listItemSocket[i].ItemType + @"/" + listItemSocket[i].ItemID); //Hình item
            ListItemSocketInventory[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, listItemSocket[temp], null, ListItemSocketSlot[temp].transform.position));

            //Xuống dòng
            i_temp_x++;
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        ObjectController[8].SetActive (!isHidden); //Ẩn/hiện danh sách item socket sau khi khởi tạo xong
    }
    #endregion

    #region Functions 

    /// <summary>
    /// Click vào item 
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="inventory"></param>
    /// <param name="type">Kiểu xem: null = xem từ inventory, -1: chỉ xem, 0 = xem từ danh sách trang bị, 1 = xem trang bị đã được trang bị</param>
    public void ItemClick (int slot, ItemModel item, int? type, Vector3 posItem) {
        PositionSelecting = posItem;
        GlobalVariables.ItemViewing = item;
        GlobalVariables.ItemViewingType = 3; //Kiểu xem item
        GameSystem.InitializePrefabUI (6); //Hiển thị thông tin item click vào
        ItemSelecting = item; //Gán tạm item để thao tác
        StartCoroutine (WaitingCloseItemDetailUI ());
    }

    /// <summary>
    /// Chờ đóng UI chi tiết item 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitingCloseItemDetailUI () {
        yield return new WaitUntil (() => GameSystem.ItemDetailCanvasUI == null);
        //Success
        if (GameSystem.ItemDetailCanvasUI == null) {
            switch (GlobalVariables.ItemDetailAction) {
                case 6: //Lựa chọn item (custom)
                    if (IsShowItemEquip) {
                        ObjectController[2].SetActive (true);
                        ObjectController[2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + ItemSelecting.ItemType + @"/" + ItemSelecting.ItemID); //Hình item;
                        StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[2], PositionSelecting, ObjectController[4].transform.position, .3f, moveCurve));
                        ObjectController[7].SetActive (false); //Ẩn/hiện danh sách item equip
                        ObjectController[8].SetActive (true); //Ẩn/hiện danh sách item socket

                        //Gán item equip nếu lựa chọn item trang bị
                        if (ItemSelecting.ItemTypeMode.Equals (ItemModel.TypeMode.Equip))
                            ItemEquipSelected = ItemSelecting;
                        if (FunctionMode.Equals (1) || FunctionMode.Equals (2)) {
                            LoadMode (FunctionMode);
                            goto PassSocket;
                        }
                        IsShowItemEquip = false;
                    } else {

                        //Check full slot
                        if (ItemEquipSelected.Sockets.Count >= ItemEquipSelected.vSocketSlot) {
                            GameSystem.ControlFunctions.ShowMessage (Languages.lang[295]); //Limit slot socket
                            goto End;
                        }
                        ItemSocketSelected = ItemSelecting;
                        ObjectController[3].SetActive (true);
                        ObjectController[3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + ItemSelecting.ItemType + @"/" + ItemSelecting.ItemID); //Hình item;
                        StartCoroutine (GameSystem.MoveObjectCurve (false, ObjectController[3], PositionSelecting, ObjectController[5].transform.position, .3f, moveCurve));
                    }
                    PassSocket: {
                        GlobalVariables.ObjectIsMoving = true; //Bắt đầu di chuyển object
                        StartCoroutine (WaitingObjectMoveCompleted ());
                    }
                    End:
                        break;
            }

            //Thiết lập lại các giá trị
            DataUserController.SaveInventory ();
            DataUserController.SaveHeroes ();
        }
    }

    /// <summary>
    /// Chờ object di chuyển xong
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitingObjectMoveCompleted () {
        yield return new WaitUntil (() => !GlobalVariables.ObjectIsMoving);
        //Success
        if (!GlobalVariables.ObjectIsMoving) {
            StartCoroutine (GameSystem.ControlFunctions.PlaySound (SoundClip[0], 0f));
            SetInforItemSelected (0, ItemSelecting);

            //Set text giá
            TextUI[5].text = ItemEquipSelected != null? SocketCoreSetting.GetGoldsRequiredInsertSocket (ItemEquipSelected).ToString (): "0";
        }
    }

    /// <summary>
    /// Gán thông tin item đã dc chọn
    /// </summary>
    /// <param name="item">truyền null nếu clear thông tin</param>
    private void SetInforItemSelected (int slot, ItemModel item) {
        switch (slot) {
            case 0: //Item đầu vào: equip item
                ObjectController[4].GetComponent<Image> ().sprite = item != null? Resources.Load<Sprite> ("Images/BorderItem/" + item.ItemColor) : null; //Phẩm chất item
                ObjectController[4].transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = item != null?Languages.lang[23] + item.ItemLevel.ToString (): ""; //Level item
                ObjectController[4].transform.GetChild (0).GetComponent<Image> ().sprite = item != null?Resources.Load<Sprite> ("Images/Items/" + item.ItemType + @"/" + item.ItemID) : null; //Hình item
                break;
            case 1: //Item nguyên liệu: socket item
                ObjectController[5].transform.GetChild (0).GetComponent<Image> ().sprite = item != null?Resources.Load<Sprite> ("Images/Items/" + item.ItemType + @"/" + item.ItemID) : null; //Hình item
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chuyển chế độ: 0-Khảm socket, 1-Gỡ socket, 2-Đục lỗ
    /// </summary>
    /// <param name="typeMode"></param>
    private void LoadMode (int typeMode) {
        if (IsRefreshList) {
            SetupItemEquipHantSocket (true, typeMode.Equals (2) ? false : true);
            SetupItemEquipHaveSocket (true, typeMode.Equals (0) ? false : true);
            //SetupItemSocket (true, typeMode.Equals (0) ? true : false);
            IsRefreshList = false;
        }
        switch (typeMode) {
            case 0: //Khảm socket
                TextUI[4].text = TextUI[2].text = Languages.lang[297]; //Kham ngoc
                ObjectController[5].SetActive (true); //Slot socket để khảm
                ObjectController[15].SetActive (true); //Hình ảnh thanh bar
                ObjectController[22].SetActive (false); //Button add socket
                ObjectController[17].SetActive (true); //Gold required
                ObjectController[23].SetActive (false); //Gems required
                ObjectController[18].SetActive (true); //Button kham
                ObjectController[24].SetActive (false); //UI create socket

                //Ẩn hết các slot socket của item đưa vào
                for (int i = 0; i < ItemCoreSetting.vSocketSlotMax; i++) {
                    ObjectController[9 + i].SetActive (false);
                }

                if (ItemEquipSelected != null) {
                    if (ItemEquipSelected.vSocketSlot <= 0) {
                        ItemEquipSelected = null;
                        ObjectController[2].SetActive (false); //Item selected
                        ObjectController[4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0"); //Phẩm chất item
                        ObjectController[7].SetActive (true); //Ẩn/hiện danh sách item equip
                        ObjectController[8].SetActive (false); //Ẩn/hiện danh sách item socket
                        ObjectController[20].SetActive (false); //Ẩn/hiện danh sách item equip not socket
                        IsShowItemEquip = true;
                    } else {
                        ObjectController[7].SetActive (false); //Ẩn/hiện danh sách item equip
                        ObjectController[8].SetActive (true); //Ẩn/hiện danh sách item socket
                        ObjectController[20].SetActive (false); //Ẩn/hiện danh sách item equip not socket
                        IsShowItemEquip = false;
                    }
                } else {
                    ObjectController[7].SetActive (true); //Ẩn/hiện danh sách item equip
                    ObjectController[8].SetActive (false); //Ẩn/hiện danh sách item socket
                    ObjectController[20].SetActive (false); //Ẩn/hiện danh sách item equip not socket
                    IsShowItemEquip = true;
                }

                //Set text giá
                TextUI[5].text = ItemEquipSelected != null? SocketCoreSetting.GetGoldsRequiredInsertSocket (ItemEquipSelected).ToString (): "0";
                break;
            case 1: //Gỡ socket
                TextUI[2].text = Languages.lang[298]; //go kham
                ObjectController[5].SetActive (false); //Slot socket để khảm
                ObjectController[15].SetActive (false); //Hình ảnh thanh bar
                ObjectController[3].SetActive (false); //Hình ảnh tạm itemsocket
                ObjectController[17].SetActive (false); //Gold required
                ObjectController[23].SetActive (false); //Gems required
                ObjectController[18].SetActive (false); //Button kham
                ObjectController[22].SetActive (false); //Button add socket

                IsShowItemEquip = true;
                ObjectController[7].SetActive (true); //Ẩn/hiện danh sách item equip
                ObjectController[8].SetActive (false); //Ẩn/hiện danh sách item socket
                ObjectController[20].SetActive (false); //Ẩn/hiện danh sách item equip not socket
                ItemSocketSelected = null;

                if (ItemEquipSelected != null) {

                    //Ẩn hết các slot socket của item đưa vào
                    for (int i = 0; i < ItemCoreSetting.vSocketSlotMax; i++) {
                        ObjectController[9 + i].SetActive (false);
                        ObjectController[9 + i].transform.GetChild (0).gameObject.SetActive (false);
                    }

                    //Hiển thị socket của item
                    if (ItemEquipSelected.SocketIDs != null) {
                        for (int i = 0; i < ItemEquipSelected.vSocketSlot; i++) {
                            ObjectController[9 + i].SetActive (true);
                            if (i < ItemEquipSelected.SocketIDs.Count) {
                                ObjectController[9 + i].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/12/" + ItemEquipSelected.SocketIDs[i]); //Hình socket item
                                ObjectController[9 + i].transform.GetChild (0).gameObject.SetActive (true);
                            }
                        }
                    }
                } else { //Hiện hết các slot socket trống khi ko có item nào
                    for (int i = 0; i < ItemCoreSetting.vSocketSlotMax; i++) {
                        ObjectController[9 + i].SetActive (true);
                        ObjectController[9 + i].transform.GetChild (0).gameObject.SetActive (false);
                    }
                }
                break;
            case 2: //Đục lỗ
                if (ItemEquipSelected != null) {
                    if (ItemEquipSelected.vSocketSlot >= 6) {
                        ItemEquipSelected = null;
                        ObjectController[2].SetActive (false);
                        ObjectController[4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0"); //Phẩm chất item
                        ObjectController[24].SetActive (false); //UI create socket
                        ObjectController[18].SetActive (false); //Button kham
                        ObjectController[23].SetActive (false); //Gems required
                    } else {

                        //Hiển thị UI
                        ObjectController[5].SetActive (false); //Slot socket để khảm
                        ObjectController[15].SetActive (false); //Hình ảnh thanh bar
                        ObjectController[22].SetActive (true); //Button add socket
                        ObjectController[24].SetActive (true); //UI create socket
                        ObjectController[17].SetActive (false); //Gold required
                        ObjectController[23].SetActive (true); //Gems required
                        ObjectController[18].SetActive (true); //Button kham

                        //Set text lỗ
                        TextUI[3].text = ItemEquipSelected.vSocketSlot.ToString () + @"/" + ItemCoreSetting.vSocketSlotMax.ToString ();

                        //Set text giá
                        TextUI[6].text = SocketCoreSetting.GetGemsRequiredCreateSocket (ItemEquipSelected).ToString ();

                        //Set text số lượng khuôn
                        TextUI[7].text = SocketCoreSetting.GetQuantityItemCreateSocket (ItemEquipSelected) + @"/" + InventorySystem.GetQuantityItem (ItemCoreSetting.CreateSocketItemType, ItemCoreSetting.CreateSocketItemID).ToString ();

                    }
                }

                //Ẩn hết các slot socket của item đưa vào
                for (int i = 0; i < ItemCoreSetting.vSocketSlotMax; i++) {
                    ObjectController[9 + i].SetActive (false);
                    ObjectController[9 + i].transform.GetChild (0).gameObject.SetActive (false);
                }

                ObjectController[7].SetActive (false); //Ẩn/hiện danh sách item equip
                ObjectController[8].SetActive (false); //Ẩn/hiện danh sách item socket
                ObjectController[20].SetActive (true); //Ẩn/hiện danh sách item equip not socket
                TextUI[4].text = TextUI[2].text = Languages.lang[299]; //Đục lỗ
                IsShowItemEquip = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Function chung
    /// </summary>
    /// <param name="type"></param>
    public void GeneralFunctions (int type) {
        switch (type) {
            case 0: //Đóng chức năng
                if (!GlobalVariables.ActionLock)
                    GameSystem.DisposePrefabUI (8);
                break;
            case 1: //Introductions
                //Debug.Log(nameof(ItemEquipSelected.vArmorPlus));
                break;
            case 2: //Chuyển đổi mode khảm <=> gỡ
                if (!GlobalVariables.ActionLock) {
                    if (FunctionMode.Equals (2)) { //0 = insert, 1 = gỡ socket, 2 = đục lỗ
                        FunctionMode = 0;
                    } else {
                        FunctionMode++;
                    }
                    LoadMode (FunctionMode);
                    StartCoroutine (GameSystem.RotationObject (true, ObjectController[16], ObjectController[16].transform.eulerAngles, new Vector3 (0, 0, RotatePosButtonChangeMode[FunctionMode]), .1f));
                }
                break;
            case 3: //Gỡ item ra khỏi slot
                if (!GlobalVariables.ActionLock) {
                   GameSystem.DisableObjectFromList(EffectCraftSuccess);
                    StartCoroutine (GameSystem.ControlFunctions.PlaySound (SoundClip[1], 0f));
                    ObjectController[2].SetActive (false); //Item equip
                    ObjectController[3].SetActive (false); //Item socket
                    ObjectController[7].SetActive (true); //Ẩn/hiện danh sách item equip
                    ObjectController[8].SetActive (false); //Ẩn/hiện danh sách item socket
                    ObjectController[4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0"); //Phẩm chất item
                    IsShowItemEquip = true;
                    ItemEquipSelected = null;
                    ItemSocketSelected = null;
                    ItemSelecting = null;
                    switch (FunctionMode) {
                        case 0:
                            break;
                        case 1: //chế độ gỡ socket
                            LoadMode (FunctionMode);
                            break;
                        case 2: //Chế độ đục lỗ
                            ObjectController[15].SetActive (false); //Hình ảnh thanh bar
                            ObjectController[22].SetActive (false); //Button add socket
                            ObjectController[17].SetActive (false); //Gold required
                            ObjectController[23].SetActive (false); //Gems required
                            ObjectController[18].SetActive (false); //Button kham
                            ObjectController[24].SetActive (false); //UI đục lỗ
                            break;
                        default:
                            break;
                    }
                    //Set text giá
                    TextUI[5].text = "0";
                }
                break;
            case 4: //Gỡ ngọc ra khỏi slot
                if (!GlobalVariables.ActionLock) {
                    ObjectController[3].SetActive (false);
                    StartCoroutine (GameSystem.ControlFunctions.PlaySound (SoundClip[1], 0f));
                    ItemSocketSelected = null;
                }
                break;
            case 5: //Thực hiện khảm ngọc vào item
                if (!GlobalVariables.ActionLock) {
                    switch (FunctionMode) {

                        #region Khảm ngọc 

                        case 0: //Khảm
                            var goldRequired = SocketCoreSetting.GetGoldsRequiredInsertSocket (ItemEquipSelected);
                            if (ItemEquipSelected != null && ItemSocketSelected != null && UserSystem.CheckGolds (goldRequired)) { //Check điều kiện
                                //Khởi tạo mảng socket nếu chưa có
                                if (ItemEquipSelected.Sockets == null) {
                                    ItemEquipSelected.Sockets = new List<SocketModel> ();
                                    ItemEquipSelected.SocketIDs = new List<int> ();
                                }

                                //Check full slot
                                if (ItemEquipSelected.Sockets.Count >= ItemEquipSelected.vSocketSlot) {
                                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[295]); //Limit slot socket
                                    return;
                                }

                                //Khởi tạo mảng socket nếu chưa có
                                ItemEquipSelected.Sockets = ItemEquipSelected.Sockets == null ? new List<SocketModel>() : ItemEquipSelected.Sockets;
                                ItemEquipSelected.SocketIDs = ItemEquipSelected.SocketIDs == null ? new List<int>() : ItemEquipSelected.SocketIDs;

                                //Khảm socket vào item
                                var randomValue =(float) (Math.Round(SocketSystem.GetValueSocket (ItemEquipSelected, ItemSocketSelected), 2)); //Giá trị gán vào socket
                                ItemEquipSelected.Sockets.Add (SocketSystem.CreateSocket (ItemSocketSelected.ItemID, randomValue)); //Thêm socket vào item
                                ItemEquipSelected.SocketIDs.Add (ItemSocketSelected.ItemID); //Thêm ID của socket vào item
                                ItemEquipSelected.vIsLock = true; //Khóa item ko cho giao dịch online
                                GameSystem.ControlFunctions.ShowMessage (string.Format (SocketSystem.GetNameValue (ItemSocketSelected.ItemID), randomValue)); //Tên giá trị + giá trị

                                //Play sound success
                                StartCoroutine (GameSystem.ControlFunctions.PlaySound (SoundClip[2], 0f));

                                //Lưu dữ liệu
                                InventorySystem.UpdateItemEquip (ItemEquipSelected);
                                ObjectController[3].SetActive (false); //Item socket
                                GlobalVariables.IsReduceItemToEmpty = false;
                                InventorySystem.ReduceItemQuantityInventory (ItemSocketSelected.ItemType, ItemSocketSelected.ItemID, 1);
                                DataUserController.SaveInventory ();
                                ItemSocketSelected = null;

                                //Show hiệu ứng success
                                GameSystem.CheckExistAndCreateObjectFromList (EffectCraftSuccess, ObjectController[2].transform.position, true, ObjectController[2]);

                                //Setup lại UI item socket khi trừ item về 0
                                GeneralFunctions (6); //Update tiền tệ
                                //if (GlobalVariables.IsReduceItemToEmpty) {
                                SetupItemSocket (true, false);
                                //}

                                //Trừ vàng, save data
                                UserSystem.DecreaseGolds (goldRequired);
                                DataUserController.SaveUserInfor ();
                            } else {
                                GameSystem.ControlFunctions.ShowMessage (Languages.lang[296]); //Không thể khảm
                            }
                            break;

                            #endregion

                            #region Đục lỗ 
                        case 2: //Đục lỗ
                            if (ValidData (2)) { //Check điều kiện

                                //Đục lỗ
                                var gemsRequired = SocketCoreSetting.GetGemsRequiredCreateSocket (ItemEquipSelected);
                                var quantityRequired = SocketCoreSetting.GetQuantityItemCreateSocket (ItemEquipSelected);
                                ItemEquipSelected.vSocketSlot++;

                                //Trừ item
                                InventorySystem.ReduceItemQuantityInventory ((sbyte) ItemCoreSetting.CreateSocketItemType, (byte) ItemCoreSetting.CreateSocketItemID, quantityRequired);
                                UserSystem.DecreaseGems (gemsRequired);

                                //Lưu dữ liệu
                                DataUserController.SaveInventory ();
                                DataUserController.SaveUserInfor ();

                                //Play sound success
                                StartCoroutine (GameSystem.ControlFunctions.PlaySound (SoundClip[2], 0f));

                                //Show hiệu ứng success
                                GameSystem.CheckExistAndCreateObjectFromList (EffectCraftSuccess, ObjectController[2].transform.position, true, ObjectController[2]);

                                //Show thông báo
                                GameSystem.ControlFunctions.ShowMessage (Languages.lang[157]); //Update thành công

                                //Refresh dữ liệu trên UI
                                GeneralFunctions (6); //Update tiền
                                IsRefreshList = true; //Refresh lại danh sách
                                LoadMode (FunctionMode);
                            }
                            break;

                            #endregion

                        default:
                            break;
                    }
                }
                break;
            case 6: //Update tiền tệ
                TextUI[0].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                TextUI[1].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Check dữ liệu
    /// </summary>
    /// <returns></returns>
    private bool ValidData (int typeMode) {

        switch (typeMode) {
            case 1:
                //Check null item truyền vào
                if (ItemEquipSelected == null && ItemSocketSelected == null) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[296]); // = "Không thể khảm ngọc";
                    return false;
                }

                //Check vàng
                var goldRequired = SocketCoreSetting.GetGoldsRequiredInsertSocket (ItemEquipSelected);
                if (!UserSystem.CheckGolds (goldRequired)) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[304]); //Không đủ vàng
                    return false;
                }
                break;

            case 2: //Đục lỗ
                //Check null item truyền vào
                if (ItemEquipSelected == null) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[303]); //Không thể đục lỗ
                    return false;
                }

                //Check gem
                var gemsRequired = SocketCoreSetting.GetGemsRequiredCreateSocket (ItemEquipSelected);
                if (!UserSystem.CheckGems (gemsRequired)) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[250]); //Không đủ đá quý
                    return false;
                }

                //Check số lượng khuôn
                var quantityRequired = SocketCoreSetting.GetQuantityItemCreateSocket (ItemEquipSelected);
                if (!InventorySystem.CheckQuantityItem (ItemCoreSetting.CreateSocketItemType, ItemCoreSetting.CreateSocketItemID, quantityRequired)) {
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[149]); //Không đủ tài nguyên
                    return false;
                }

                break;
            default:
                break;
        }
        return true;
    }

    /// <summary>
    /// Hiển thị thông báo phá hủy ngọc
    /// </summary>
    /// <param name="slot"></param>
    public void ButtonShowConfirmBreakJewel (int slot) {
        GameSystem.ShowConfirmDialog (ItemSystem.GetItemSocketDescription (ItemEquipSelected.Sockets[slot]) + Languages.lang[300]); //Show xác nhận phá ngọc
        StartCoroutine (WaitingForBreakJewel (slot)); //Chờ confiem
    }

    /// <summary>
    /// Đợi xác nhận phá hủy ngọc
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator WaitingForBreakJewel (int slot) {
        yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
        //Accept
        if (GameSystem.ConfirmBoxResult == 1) {
            StartCoroutine (RunEffectJewelCracked (slot));
            ItemEquipSelected.Sockets.RemoveAt (slot);
            ItemEquipSelected.SocketIDs.RemoveAt (slot);
            DataUserController.SaveInventory ();
        }
    }

    /// <summary>
    /// Hình ảnh ngọc bị nứt vỡ
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    private IEnumerator RunEffectJewelCracked (int slot) {
        GlobalVariables.ActionLock = true;
        ObjectController[19].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/12/" + SocketSystem.GetImgJewelCracked (ItemEquipSelected.Sockets[slot].SocketID)); //Hình socket item
        ObjectController[19].transform.position = ObjectController[slot + 9].transform.position;
        ObjectController[19].SetActive (true);
        yield return new WaitForSeconds (1f);
        //GameSystem.CheckExistAndCreateObjectFromList (EffectCraftSuccess, ObjectController[19].transform.position, true, ObjectController[19]); //Show hiệu ứng success
        ObjectController[19].SetActive (false);
        LoadMode (FunctionMode);
        GameSystem.ControlFunctions.ShowMessage (Languages.lang[301]); //Jewel destroyed
        GlobalVariables.ActionLock = false;
    }
    #endregion
}