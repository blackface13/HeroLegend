using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpinController : MonoBehaviour {
    #region Variables

    public GameObject[] ObjSpin;
    private List<string> ItemSpinList; //Danh sách item được phép xuất hiện trong spin
    public Text[] TextLanguage;
    private float[] RotationPoints = new float[8] { 0f, -44.8f, -90.3f, -135.8f, -180f, 135f, 90f, 45.3f };
    private bool IsSpining = false; //Check xem có đang quay hay ko
    private bool IsStartNewSpin = true; //Cho phép bắt đầu quay lượt mới hay chưa
    private bool IsAutoSpin = false;
    private float SpeedRotationZ = -50f; //Tốc độ quay, giá trị này sẽ được gán lại phía dưới
    private RectTransform RectArrowSpin;
    Vector3 rotationEuler = Vector3.zero;
    private Image[] ItemImage;
    public GameObject[] ItemSpin; //Object các item sẽ xuất hiện, Set ở interface
    private Button[] ItemSpinButton;
    private List<GameObject> EffectCraftSuccess;
    private string[] ItemIDSpin; //Danh sách ID item được đưa lên spin

    #endregion

    #region Initialize 

    void Start () {
        DataUserController.LoadInventory ();
        DataUserController.LoadUserInfor ();
        ItemImage = new Image[ItemSpin.Length];
        ItemSpinButton = new Button[ItemSpin.Length];
        ItemIDSpin = new string[ItemSpin.Length];
        for (int i = 0; i < ItemSpin.Length; i++) //Gán image item
        {
            ItemImage[i] = ItemSpin[i].transform.GetComponent<Image> ();
            ItemSpinButton[i] = ItemSpin[i].transform.GetComponent<Button> ();
        }
        CreateNewListItems (); //Tạo mới danh sách item
        RectArrowSpin = ObjSpin[1].GetComponent<RectTransform> ();
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        SetupTextQuantityItemSpin ();
        TextLanguage[3].text = Languages.lang[192]; //Auto spin
        TextLanguage[4].text = Languages.lang[194]; //Introducton
        TextLanguage[6].text = Languages.lang[194]; //Introducton
        TextLanguage[5].text = Languages.lang[195]; //Introducton descriptions
    }

    /// <summary>
    /// Tạo danh sách item có thể xuất hiện trong quay thưởng
    /// </summary>
    private void CreateItemSpinActiveList () {
        //Cấu trúc: A,B,C
        //A: item type
        //B: item ID
        //C: show rate (tỉ lệ xuất hiện)
        if (ItemSpinList == null) {
            ItemSpinList = new List<string> ();
            //Item vật phẩm sử dụng
            ItemSpinList.Add ("10,0,100");
            ItemSpinList.Add ("10,1,100");
            ItemSpinList.Add ("10,2,100");
            ItemSpinList.Add ("10,3,100");
            ItemSpinList.Add ("10,4,100");
            ItemSpinList.Add ("10,5,100");
            ItemSpinList.Add ("10,6,100");
            ItemSpinList.Add ("10,7,100");
            ItemSpinList.Add ("10,8,100");
            ItemSpinList.Add ("10,9,100");
            ItemSpinList.Add ("10,10,100");
            ItemSpinList.Add ("10,11,100");
            ItemSpinList.Add ("10,12,100");
            ItemSpinList.Add ("10,13,100");
            ItemSpinList.Add ("10,14,100");
            ItemSpinList.Add ("10,15,100");
            ItemSpinList.Add ("10,16,100");
            ItemSpinList.Add ("10,17,100");
            ItemSpinList.Add ("10,18,100");
            ItemSpinList.Add ("10,19,100");
            ItemSpinList.Add ("10,20,100");
            ItemSpinList.Add ("10,21,100");
            ItemSpinList.Add ("10,22,100");
            ItemSpinList.Add ("10,23,100");
            ItemSpinList.Add ("10,24,100");
            ItemSpinList.Add ("10,25,100");
            //Item nguyên liệu
            ItemSpinList.Add ("11,0,100");
            ItemSpinList.Add ("11,1,100");
            ItemSpinList.Add ("11,2,100");
            ItemSpinList.Add ("11,3,100");
            ItemSpinList.Add ("11,4,100");
            ItemSpinList.Add ("11,5,100");
            ItemSpinList.Add ("11,6,100");
            ItemSpinList.Add ("11,7,100");
            ItemSpinList.Add ("11,8,100");
            ItemSpinList.Add ("11,9,100");
            ItemSpinList.Add ("11,10,100");
            ItemSpinList.Add ("11,11,100");
            ItemSpinList.Add ("11,12,100");
            ItemSpinList.Add ("11,13,100");
            ItemSpinList.Add ("11,14,100");
            ItemSpinList.Add ("11,15,100");
            ItemSpinList.Add ("11,16,100");
            ItemSpinList.Add ("11,17,100");
            ItemSpinList.Add ("11,18,100");
            ItemSpinList.Add ("11,19,100");
            ItemSpinList.Add ("11,20,100");
            ItemSpinList.Add ("11,21,100");
            ItemSpinList.Add ("11,22,100");
            ItemSpinList.Add ("11,23,100");
            ItemSpinList.Add ("11,24,100");
            ItemSpinList.Add ("11,25,100");
            ItemSpinList.Add ("11,26,100");
            ItemSpinList.Add ("11,27,100");
            ItemSpinList.Add ("11,28,100");
            ItemSpinList.Add ("11,29,100");
            ItemSpinList.Add ("11,30,100");
            ItemSpinList.Add ("11,31,100");
            ItemSpinList.Add ("11,32,100");
            ItemSpinList.Add ("11,33,100");
            ItemSpinList.Add ("11,34,100");
            ItemSpinList.Add ("11,35,100");
            ItemSpinList.Add ("11,36,100");
            ItemSpinList.Add ("11,37,100");
            ItemSpinList.Add ("11,38,100");
            ItemSpinList.Add ("11,39,100");
            ItemSpinList.Add ("11,40,100");
            ItemSpinList.Add ("11,41,100");
            ItemSpinList.Add ("11,42,100");
            ItemSpinList.Add ("11,43,100");
            ItemSpinList.Add ("11,44,100");
            //Item socket
            ItemSpinList.Add ("12,0,100");
            ItemSpinList.Add ("12,1,100");
            ItemSpinList.Add ("12,2,100");
            ItemSpinList.Add ("12,3,100");
            ItemSpinList.Add ("12,4,100");
            ItemSpinList.Add ("12,5,100");
            ItemSpinList.Add ("12,6,100");
            ItemSpinList.Add ("12,7,100");
            ItemSpinList.Add ("12,8,100");
            ItemSpinList.Add ("12,9,100");
            ItemSpinList.Add ("12,10,100");
            ItemSpinList.Add ("12,11,100");
            ItemSpinList.Add ("12,12,100");
            ItemSpinList.Add ("12,13,100");
            ItemSpinList.Add ("12,14,100");
            ItemSpinList.Add ("12,15,100");
            ItemSpinList.Add ("12,16,100");
            ItemSpinList.Add ("12,17,100");
            ItemSpinList.Add ("12,18,100");
            ItemSpinList.Add ("12,19,100");
            ItemSpinList.Add ("12,20,100");
            ItemSpinList.Add ("12,21,100");
            ItemSpinList.Add ("12,22,100");
            ItemSpinList.Add ("12,23,100");
            ItemSpinList.Add ("12,24,100");
            ItemSpinList.Add ("12,25,100");
            ItemSpinList.Add ("12,26,100");
            ItemSpinList.Add ("12,27,100");
            ItemSpinList.Add ("12,28,100");
            ItemSpinList.Add ("12,29,100");
            ItemSpinList.Add ("12,30,100");
            ItemSpinList.Add ("12,31,100");
            ItemSpinList.Add ("12,32,100");
            ItemSpinList.Add ("12,33,100");
        }
    }

    /// <summary>
    /// Hiển thị số lượng item cần thiết để quay spin
    /// </summary>
    private void SetupTextQuantityItemSpin () {
        var featherQuantity = DataUserController.Inventory.DBItems.Find (x => x.ItemID == Module.SpinMainItemID); //230 là id lông phượng
        ObjSpin[5].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + Module.SpinMainItemType + @"/" + Module.SpinMainItemID.ToString ()); //Gán hình ảnh item cần thiết để sử dụng spin
        TextLanguage[2].text = featherQuantity != null?featherQuantity.Quantity.ToString (): "0"; //Số lượng lông phượng
    }

    #endregion

    #region Functions 

    private void Update () {
        if (IsSpining) {
            rotationEuler -= Vector3.forward * SpeedRotationZ * Time.deltaTime; //increment 30 degrees every second
            RectArrowSpin.rotation = Quaternion.Euler (rotationEuler);
        }
    }

    /// <summary>
    /// Check dữ liệu trước khi quay
    /// </summary>
    /// <returns></returns>
    private bool Valid () {
        var featherQuantity = DataUserController.Inventory.DBItems.Find (x => x.ItemID == Module.SpinMainItemID); //230 là id lông phượng
        if (featherQuantity == null || featherQuantity.Quantity <= 0) {
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[191])); //Không đủ Lông phượng để thực hiện
            IsAutoSpin = false;
            AutoSpin (IsAutoSpin);
            return false;
        }
        if (DataUserController.Inventory.DBItems.Count >= DataUserController.User.InventorySlot) { //Check slot trống trong thùng đồ
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Thông báo giới hạn slot inventory
            IsAutoSpin = false;
            AutoSpin (IsAutoSpin);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Lật tất cả item về scale 0 và tạo list item mới
    /// </summary>
    private void ResetScaleImageItems () {
        for (int i = 0; i < ItemSpin.Length; i++)
            ItemImage[i].rectTransform.localScale = new Vector3 (0, 1, 1);
        CreateNewListItems (); //Tạo mới danh sách item
    }

    /// <summary>
    /// Tạo list item mới để quay
    /// </summary>
    private void CreateNewListItems () {
        CreateItemSpinActiveList ();
        for (int i = 0; i < ItemSpin.Length; i++) {
            var temp = ItemSpinList[UnityEngine.Random.Range (0, ItemSpinList.Count)];
            ItemIDSpin[i] = temp;
            ItemImage[i].sprite = Resources.Load<Sprite> ("Images/Items/" + temp.Split (',') [0] + @"/" + temp.Split (',') [1]);
            ItemSpinButton[i].onClick.RemoveAllListeners ();
            ItemSpinButton[i].onClick.AddListener (() => ItemClick (temp));
        }
    }

    /// <summary>
    /// Tạo item mới để quay
    /// </summary>
    private void CreateNewImageItem (int slotItem) {
        CreateItemSpinActiveList ();
        var temp = ItemSpinList[UnityEngine.Random.Range (0, ItemSpinList.Count)];
        ItemIDSpin[slotItem] = temp;
        ItemImage[slotItem].sprite = Resources.Load<Sprite> ("Images/Items/" + temp.Split (',') [0] + @"/" + temp.Split (',') [1]);
        ItemSpinButton[slotItem].onClick.RemoveAllListeners ();
        ItemSpinButton[slotItem].onClick.AddListener (() => ItemClick (temp));
    }

    /// <summary>
    /// Hiệu ứng lật item
    /// </summary>
    /// <param name="type">0 = show, 1 = hide</param>
    /// <param name="timeDelay"></param>
    /// <param name="FadeTime"></param>
    /// <returns></returns>
    private IEnumerator ShowingOrClosingItemsSpin (int type, float timeDelay, float FadeTime) {
        yield return new WaitForSeconds (type.Equals (0) ? (FadeTime + .1f) : 0f);
        float scaleX = type.Equals (0) ? 0 : 1f;
        for (int i = 0; i < ItemImage.Length; i++) {
            scaleX = type.Equals (0) ? 0 : 1f;
            while (type.Equals (0) ? ItemImage[i].transform.localScale.x<1f : ItemImage[i].transform.localScale.x> 0) {
                scaleX += (type.Equals (0) ? 1f : -1f) * Time.deltaTime / FadeTime;
                ItemImage[i].transform.localScale = new Vector3 (scaleX, 1, 1);
                if (type.Equals (0) ? ItemImage[i].transform.localScale.x > 1f : ItemImage[i].transform.localScale.x < 0) //Không cho vượt quá giới hạn scale
                    ItemImage[i].transform.localScale = new Vector3 (type.Equals (0) ? 1 : 0, 1, 1);
                if (ItemImage[i].transform.localScale.x <= 0) { //Tạo image item mới khi item đã lật về scale = 0
                    if (type.Equals (1))
                        CreateNewImageItem (i);
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// Show thông tin chi tiết từng item
    /// </summary>
    /// <param name="itemInfor"></param>
    private void ItemClick (string itemInfor) {
        var itemType = Convert.ToSByte (itemInfor.Split (',') [0]);
        var itemID = Convert.ToByte (itemInfor.Split (',') [1]);
        ObjSpin[2].SetActive (true);

        TextLanguage[0].text = ItemSystem.GetItemName (itemType, itemID); //Tên item
        TextLanguage[1].text = ItemSystem.GetItemDescription (itemType, itemID); // Description
        ObjSpin[3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemType.ToString () + @"/" + itemID.ToString ());
    }

    /// <summary>
    /// Dừng spin sau khoảng time
    /// </summary>
    /// <param name="speedCurrent"></param>
    /// <param name="FadeTime"></param>
    /// <returns></returns>
    private IEnumerator StopSpin (float speedCurrent, float FadeTime) {
        yield return new WaitForSeconds (UnityEngine.Random.Range (0f, 1f));
        while (SpeedRotationZ > 0f) {
            SpeedRotationZ -= speedCurrent * Time.deltaTime / FadeTime;
            yield return null;
        }
        var result = 0;
        if (RectArrowSpin.eulerAngles.z <= 360f && RectArrowSpin.eulerAngles.z >= 315.2f)
            result = 0;
        if (RectArrowSpin.eulerAngles.z < 315.2f && RectArrowSpin.eulerAngles.z >= 270f)
            result = 1;
        if (RectArrowSpin.eulerAngles.z < 270f && RectArrowSpin.eulerAngles.z >= 225f)
            result = 2;
        if (RectArrowSpin.eulerAngles.z < 225f && RectArrowSpin.eulerAngles.z >= 180f)
            result = 3;
        if (RectArrowSpin.eulerAngles.z < 180f && RectArrowSpin.eulerAngles.z >= 135f)
            result = 4;
        if (RectArrowSpin.eulerAngles.z < 135f && RectArrowSpin.eulerAngles.z >= 90f)
            result = 5;
        if (RectArrowSpin.eulerAngles.z < 90f && RectArrowSpin.eulerAngles.z >= 45.3f)
            result = 6;
        if (RectArrowSpin.eulerAngles.z < 45.3f && RectArrowSpin.eulerAngles.z >= 0f)
            result = 7;

        var itemRewardType = Convert.ToInt16 (ItemIDSpin[result].Split (',') [0]); //Type của item nhận đc
        var itemRewardID = Convert.ToInt16 (ItemIDSpin[result].Split (',') [1]); //ID của item nhận dc
        var itemRewardName = itemRewardType.Equals (10) ? Languages.ItemNameType10[itemRewardID] : itemRewardType.Equals (11) ? Languages.ItemNameType11[itemRewardID] : Languages.ItemNameType12[itemRewardID];
        GameSystem.ControlFunctions.ShowMessage ((string.Format (Languages.lang[190], itemRewardName)));
        StartCoroutine (GameSystem.PlaySound ((AudioClip) Resources.Load<AudioClip> ("Audio/SE/SpinSuccess"), 0));
        ShowEffectSuccess (ItemSpin[result].transform.position);
        InventorySystem.AddItemToInventory (ItemSystem.CreateItem (false, false, 0, (sbyte) itemRewardType, (byte) itemRewardID, 1));
        DataUserController.SaveInventory ();
        IsSpining = false; //Dừng quay hẳn
        IsStartNewSpin = true; //Cho phép bắt đầu lượt quay mới
        SetupTextQuantityItemSpin ();
        if (IsAutoSpin) //Thực hiện gọi hàm quay nếu bật tự động
        {
            AutoSpin (true);
        }
    }
    /// <summary>
    /// Thực hiện các nút ấn
    /// </summary>
    /// <param name="type">0: đóng cửa sổ spin</param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0:
                if (!IsSpining && !IsAutoSpin) //Nếu đang quay thì ko cho đóng cửa sổ
                {
                    ObjSpin[4].GetComponent<Home> ().ButtonFunctions (1);
                    ObjSpin[0].SetActive (false);
                }
                break;
            case 1: //Ẩn thông tin chi tiết item
                ObjSpin[2].SetActive (false);
                break;
            case 2: //Tự động quay
                if (!IsAutoSpin && !IsSpining)
                    AutoSpin (true);
                else if (IsSpining && !IsAutoSpin) {
                    IsAutoSpin = true;
                    TextLanguage[3].text = IsAutoSpin? Languages.lang[193] : Languages.lang[192]; //Auto spin
                } else if (IsAutoSpin)
                    AutoSpin (false);
                break;
            case 3: //Bật/Đóng khung hướng dẫn khung hướng dẫn
                ObjSpin[6].SetActive (!ObjSpin[6].activeSelf);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Hiệu ứng kết thúc spin
    /// </summary>
    /// <param name="vec"></param>
    private void ShowEffectSuccess (Vector3 vec) {
        var count = EffectCraftSuccess.Count;
        for (int i = 0; i < count; i++) {
            if (!EffectCraftSuccess[i].activeSelf) {
                EffectCraftSuccess[i].SetActive (true);
                EffectCraftSuccess[i].transform.position = vec;
                break;
            }
            if (i == count - 1)
                EffectCraftSuccess.Add ((GameObject) Instantiate (EffectCraftSuccess[0], vec, Quaternion.identity));

        }
    }

    /// <summary>
    /// Thực hiện nhả nút quay
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonSpinKeyUp (BaseEventData eventData) {
        if (IsStartNewSpin && IsSpining && !IsAutoSpin) {
            StartCoroutine (StopSpin (SpeedRotationZ, Module.SpinTimeStop));
            IsStartNewSpin = false;
        }
    }

    /// <summary>
    /// Thực hiện nhấn nút quay
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonSpinKeyDown (BaseEventData eventData) {
        if (!IsAutoSpin)
            StartSpin ();
    }

    /// <summary>
    /// Thực hiện quay
    /// </summary>
    private void StartSpin () {
        if (!IsSpining && Valid ()) //Nếu đang ko quay và valid true thì thực hiện
        {
            //ResetScaleImageItem ();
            DataUserController.User.NumberSpined++;
            InventorySystem.ReduceItemQuantityInventory (Module.SpinMainItemType, Module.SpinMainItemID, Module.SpinItemQuantity);
            StartCoroutine (ShowingOrClosingItemsSpin (1, 0, 0.1f)); //Lật item về 0
            StartCoroutine (ShowingOrClosingItemsSpin (0, 0, 0.1f)); //Lật item về 1
            SpeedRotationZ = Module.SpinRotateDefault + UnityEngine.Random.Range (100f, 3000f); //Cộng thêm ngẫu nhiên tốc độ quay
            SetupTextQuantityItemSpin ();
            IsSpining = true; //Đang quay
            if (IsAutoSpin) { //Nếu bật tự động quay thì dừng lại ngay, đỡ tốn nhiều time
                StartCoroutine (StopSpin (SpeedRotationZ, Module.SpinTimeStop));
                IsStartNewSpin = false;
            }
        }
    }

    /// <summary>
    /// Thực hiện tự động quay thưởng
    /// </summary>
    /// <param name="isAuto"></param>
    public void AutoSpin (bool isAuto) {
        IsAutoSpin = isAuto;
        TextLanguage[3].text = IsAutoSpin? Languages.lang[193] : Languages.lang[192]; //Auto spin
        if (isAuto) {
            StartSpin ();
        }
    }
    #endregion
}