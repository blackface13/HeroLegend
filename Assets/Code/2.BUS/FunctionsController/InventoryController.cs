using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    #region Inventory System 
    private double Slot = 0; //Slot tối đa
    private int ItemSlotViewing = -1; //Slot item đang được xem, dùng để bán hoặc xem item tiếp theo
    private int HorizontalQuantity = 13; //Số slot trên 1 hàng ngang (được tính toán lại ở hàm SetupInventory phía dưới, vì nó scale theo tỉ lệ màn hình mỗi thiết bị)
    public GameObject[] ObjectController; //Set in interface
    public Text[] ItemInforText; //Set in interface
    private GameObject[] ItemSlot;
    private Image[] ItemSlotImg; //Hình nền phía sau của item, thể hiện màu của item
    private Image ItemDetailColor; //Hình nền phía sau của item, thể hiện màu item, chi tiết item
    private List<GameObject> Items = new List<GameObject> ();
    private List<GameObject> ItemBag1 = new List<GameObject> ();
    private List<ItemModel> InventoryTemplate;
    public DataInventory item = new DataInventory ();
    private bool[] ItemsFilter; //Lọc xem trang bị theo kiểu trang bị. 0 = Equip, 1 = Use, 2 = Quest
    public enum ActionType //Chế độ view inventory
    {
        SelectItems,
        ViewInfor,
        EquipToBag
    }
    public ActionType Action;
    private int BagInSelecting;
    private float[] PositionButtonItemFilter; //Tọa độ x của thanh kéo ra vào lọc item xem theo loại, mảng thứ 0 = tọa độ ban đầu, mảng thứ 1 là khoảng cách sẽ dịch chuyển, mảng 2 là tốc độ dịch chuyển
    private RectTransform[] RectTransformButtonItemFilter; //Gán rect để dịch chuyển thanh button
    private int? ViewingItemType; //Kiểu xem item trang bị: null: xem trong inventory, 0: xem trong chi tiết nhân vật, item chưa trang bị, 1: xem trong chi tiết nhân vật, item đã được trang bị cho hero
    private bool EnterButtonModifyItem; //Check xem có đang nhấn upgrade item để xem yêu cầu nâng cấp hay ko
    private int InventoryFilterType; //Kiểu lọc trang bị. 0 = xem all
    private ItemModel ItemViewing;
    private List<GameObject> EffectCraftSuccess;
    [Header ("Draw Curve")]
    public AnimationCurve moveCurve;
    public Slider SliderItemSell;
    #endregion

    // Start is called before the first frame update
    void Start () {
        Action = ActionType.ViewInfor;
        //Languages.SetupLanguage (1); //Set tạm
        StartCoroutine (SetupLanguage ());
        DataUserController.LoadUserInfor ();
        DataUserController.LoadHeroDefault ();
        DataUserController.LoadHeroes ();
        DataUserController.LoadInventory (); //Set tạm
        Slot = DataUserController.User.InventorySlot; //Gán tổng số slot bằng slot inventory của user
        ItemDropController.Initialize (); //Khởi tạo item drop
        StartCoroutine (SetupInventory ()); //Khởi tạo inventory
        StartCoroutine (SetupAddItems ()); //Vẽ item lên màn hình
        ButtonRefreshInventory (null);
    }

    #region Inventory Functions 

    #region Khởi tạo 

    /// <summary>
    /// Gán ngôn ngữ vào các text
    /// </summary>
    private IEnumerator SetupLanguage () {
        ItemInforText[4].text = Languages.lang[138]; //Bán
        ItemInforText[16].text = Languages.lang[67]; //Hủy bỏ
        ItemInforText[5].text = Languages.lang[134];
        ItemInforText[6].text = Languages.lang[135];
        ItemInforText[7].text = Languages.lang[136];
        ItemInforText[8].text = Languages.lang[137];
        ItemInforText[9].text = ItemInforText[12].text = Languages.lang[138];
        ItemInforText[11].text = Languages.lang[139];
        ItemInforText[43].text = Languages.lang[154]; //Nâng cấp
        ItemInforText[44].text = Languages.lang[155]; //Nâng phẩm
        ItemInforText[45].text = Languages.lang[156]; //Phân giải
        ItemInforText[46].text = Languages.lang[148]; //Yêu cầu
        ButtonFunction (1);
        yield return null;
    }

    /// <summary>
    /// Khởi tạo số lượng, sắp xếp vị trí slot inventory
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupInventory () {
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        EffectCraftSuccess[0].transform.SetParent (ObjectController[11].transform, false);
        ItemDetailColor = ObjectController[18].GetComponent<Image> ();
        HorizontalQuantity = Convert.ToInt32 (ObjectController[11].GetComponent<RectTransform> ().sizeDelta.x + ObjectController[10].GetComponent<RectTransform> ().sizeDelta.x) / 208; //Tính lại số lượng item trên 1 hàng theo scale của màn hình
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        ItemSlot = new GameObject[(int) Slot];
        ItemSlotImg = new Image[(int) Slot];
        float regionSpace = 210f; //Khoảng cách giữa các object
        ObjectController[0].GetComponent<RectTransform> ().sizeDelta = Slot % HorizontalQuantity == 0 ? new Vector2 (0, (float)(Slot / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((float)(Slot / HorizontalQuantity) + 1) * regionSpace);
        float verticalcounttemp = ObjectController[0].GetComponent<RectTransform> ().sizeDelta.y / 2 - 105;
        //Khởi tạo button inventory
        for (int i = 0; i < Slot; i++) {
            ItemSlot[i] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 102 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity);
            ItemSlot[i].transform.SetParent (ObjectController[0].transform, false);
            ItemSlotImg[i] = ItemSlot[i].GetComponent<Image> ();
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        ItemsFilter = new bool[3]; //Lọc xem trang bị theo kiểu trang bị. 0 = Equip, 1 = Use, 2 = Quest
        ItemsFilter[0] = ItemsFilter[1] = ItemsFilter[2] = true;
        ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ

        #region Setup các giá trị của hiệu ứng kéo ra vào lọc item 

        PositionButtonItemFilter = new float[3];
        PositionButtonItemFilter[0] = ObjectController[12].GetComponent<RectTransform> ().localPosition.x; //Set tọa độ ban đầu
        PositionButtonItemFilter[1] = 230f; //Khoảng cách sẽ dịch chuyển
        PositionButtonItemFilter[2] = 3000f; //Tốc độ dịch chuyển
        RectTransformButtonItemFilter = new RectTransform[3];
        RectTransformButtonItemFilter[0] = ObjectController[12].GetComponent<RectTransform> ();
        RectTransformButtonItemFilter[1] = ObjectController[13].GetComponent<RectTransform> ();
        RectTransformButtonItemFilter[2] = ObjectController[14].GetComponent<RectTransform> ();

        #endregion
        yield return null;
    }

    /// <summary>
    /// Thêm item vào thùng đồ
    /// </summary>
    public IEnumerator SetupAddItems () {
        //RemoveItems(-1);
        //Items = new GameObject[item.Count];
        try {
            for (int i = 0; i < DataUserController.Inventory.DBItems.Count; i++) {
                var temp = i;
                Items.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
                Items[i].transform.SetParent (ItemSlot[i].transform, false);
                Items[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Inventory.DBItems[i].ItemType + @"/" + DataUserController.Inventory.DBItems[i].ItemID);
                if (DataUserController.Inventory.DBItems[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6
                    ItemSlotImg[i].sprite = Resources.Load<Sprite> ("Images/BorderItem/" + DataUserController.Inventory.DBItems[i].ItemColor);
                if (DataUserController.Inventory.DBItems[i].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    Items[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + DataUserController.Inventory.DBItems[i].ItemLevel.ToString ();
                else
                    Items[i].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[i].Quantity.ToString ();
                Items[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Inventory.DBItems, null));
            }
        } catch {
            RemoveItems (-1);
            ReSortItems (DataUserController.Inventory.DBItems);
        }
        InventoryTemplate = DataUserController.Inventory.DBItems.ToList (); //Set inventory tạm thời bằng inventory chính
        yield return null;
    }

    /// <summary>
    /// Click vào item trong thùng đồ
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="inventory"></param>
    /// <param name="type">Kiểu xem: null = xem từ inventory, 0 = xem từ danh sách trang bị, 1 = xem trang bị đã được trang bị</param>
    public void ItemClick (int slot, List<ItemModel> inventory, int? type) {
        ItemViewing = inventory[slot]; //Gán tạm item để thao tác
        ViewingItemType = type;
        ItemDetailColor.sprite = Resources.Load<Sprite> ("Images/BorderItem/" + inventory[slot].ItemColor.ToString ());
        switch (Action) {
            case ActionType.ViewInfor: //Chế độ xem
                ItemSlotViewing = slot; //Gán slot item đang được click vào
                ObjectController[1].SetActive (true);
                ObjectController[2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + inventory[slot].ItemType + @"/" + inventory[slot].ItemID);
                ItemInforText[0].text = ItemSystem.GetItemName (inventory[slot].ItemType, inventory[slot].ItemID); //Tên item
                if (inventory[slot].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    ItemInforText[1].text = Languages.lang[23] + inventory[slot].ItemLevel;
                else
                    ItemInforText[1].text = Languages.lang[75] + inventory[slot].Quantity;
                switch (inventory[slot].ItemTypeMode) {
                    case global::ItemModel.TypeMode.Equip: //Hiển thị 3 nút nâng cấp, nâng phẩm và phân giải đối với item equip
                        ObjectController[17].SetActive (true);
                        ObjectController[16].SetActive (true);
                        ObjectController[15].SetActive (true);
                        ItemInforText[2].text = ItemSystem.GetItemDescription (inventory[slot]); //Description
                        break;
                    default:
                        //Ẩn 3 nút nâng cấp, nâng phẩm và phân giải
                        ObjectController[17].SetActive (false);
                        ObjectController[16].SetActive (false);
                        ObjectController[15].SetActive (false);
                        ItemInforText[2].text = ItemSystem.GetItemDescription (inventory[slot]); // Description
                        break;
                }
                var lineCount = ItemInforText[2].text.Split ('\n');
                var sizeHeight = 68f * lineCount.Length;
                ObjectController[7].GetComponent<RectTransform> ().sizeDelta = new Vector2 (ItemInforText[2].GetComponent<RectTransform> ().sizeDelta.x, sizeHeight > 760f?sizeHeight : 760f); //760 là fix cứng khi thiết kế
                //print (lineCount.Length);
                ItemInforText[3].text = Languages.lang[76] + ItemSystem.GetItemPrice (inventory[slot]); //ItemSystem.GetItemPrice (inventory[slot]); //Giá bán
                // ObjectController[20].SetActive (ObjectController[10].activeSelf); //Chỉ hiển thị 2 nút next và prev khi xem trang bị trong inventory
                // ObjectController[21].SetActive (ObjectController[10].activeSelf);
                ObjectController[22].SetActive (ObjectController[10].activeSelf); //Ẩn button bán item đơn lẻ
                if (!ObjectController[10].activeSelf) //Nếu inventory ko active (tức là đang ở hệ thống trang bị item cho nhân vật)
                {
                    if (type.Equals (0)) { //Xem trang bị chưa dc trang bị
                        ObjectController[23].SetActive (true); // button trang bị
                        ObjectController[19].SetActive (false); // button gỡ trang bị
                    }
                    if (type.Equals (1)) { //Xem trang bị đã được trang bị
                        ObjectController[23].SetActive (false); // button trang bị
                        ObjectController[19].SetActive (true); // button gỡ trang bị
                    }
                    ItemViewing = inventory[slot]; //Gán tạm item để thao tác
                } else {
                    ObjectController[23].SetActive (false); // button trang bị
                    ObjectController[19].SetActive (false); // button gỡ trang bị
                }
                break;
            case ActionType.SelectItems: //Chế độ lựa chọn item
                Items[slot].transform.GetChild (1).GetComponent<Image> ().gameObject.SetActive (!Items[slot].transform.GetChild (1).GetComponent<Image> ().gameObject.activeSelf);

                // ObjectController[1].SetActive(true);
                // ObjectController[2].GetComponent<Image>().sprite = inventory[slot].Icon;
                // ItemInforText[0].text = inventory[slot].Name;
                // if (inventory[slot].ItemTypeMode.Equals(global::ItemModel.TypeMode.Equip))
                //     ItemInforText[1].text = "";
                // else
                //     ItemInforText[1].text = Languages.lang[75] + inventory[slot].Quantity;
                // ItemInforText[2].text = inventory[slot].Descriptions;//Description
                // ItemInforText[3].text = Languages.lang[76] + inventory[slot].Price;
                ItemInforText[3].text = Languages.lang[76] + inventory[slot].ItemPrice; //ItemSystem.GetItemPrice (inventory[slot]); //Giá bán
                break;
            default:
                break;
        }
    }
    #endregion

    #region Functions 

    #region Thêm, xóa, sắp xếp items 

    //Thêm item ngẫu nhiên vào thùng đồ
    private void AddItemsRandom (int quantity, global::ItemModel.TypeMode ItemType, byte trapvalue) {
        for (int i = 0; i < quantity; i++)
            if (Items.Count < Slot) {
                bool haveInventory = false; //Xác định item có trong thùng đồ hay chưa với loại item use hoặc quest
                int slotExists = -1;
                var itemcreated = ItemSystem.CreateItem (true, true, (byte) trapvalue, 0, 0, 1);
                //Nếu là item khác loại trang bị thì mới check cộng dồn
                if (!itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
                    var count = DataUserController.Inventory.DBItems.Count;
                    for (int y = 0; y < count; y++) {
                        if (DataUserController.Inventory.DBItems[y].ItemID.Equals (itemcreated.ItemID)) {
                            haveInventory = true;
                            slotExists = y;
                            break;
                        }
                    }
                }
                if (itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || !haveInventory) {
                    DataUserController.Inventory.DBItems.Add (itemcreated);
                    var temp = DataUserController.Inventory.DBItems.Count - 1;
                    Items.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
                    Items[DataUserController.Inventory.DBItems.Count - 1].transform.SetParent (ItemSlot[DataUserController.Inventory.DBItems.Count - 1].transform, false);
                    Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemID + @"/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemID);
                    if (DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                        Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
                    else
                        Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].Quantity.ToString ();
                    Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Inventory.DBItems, null));
                }
                if (haveInventory) {
                    DataUserController.Inventory.DBItems[slotExists].Quantity += itemcreated.Quantity;
                    Items[slotExists].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[slotExists].Quantity.ToString ();
                }
            }
    }

    /// <summary>
    /// Thêm item vào thùng đồ
    /// </summary>
    /// <param name="item"></param>
    private void AddItemToInventory (ItemModel item) {

        if (Items.Count < Slot) {
            bool haveInventory = false; //Xác định item có trong thùng đồ hay chưa với loại item use hoặc quest
            int slotExists = -1;
            var itemcreated = item;
            //Nếu là item khác loại trang bị thì mới check cộng dồn
            if (!itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
                var count = DataUserController.Inventory.DBItems.Count;
                for (int y = 0; y < count; y++) {
                    if (DataUserController.Inventory.DBItems[y].ItemID.Equals (itemcreated.ItemID)) {
                        haveInventory = true;
                        slotExists = y;
                        break;
                    }
                }
            }
            if (itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || !haveInventory) {
                DataUserController.Inventory.DBItems.Add (itemcreated);
                var temp = DataUserController.Inventory.DBItems.Count - 1;
                Items.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
                Items[DataUserController.Inventory.DBItems.Count - 1].transform.SetParent (ItemSlot[DataUserController.Inventory.DBItems.Count - 1].transform, false);
                Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemID + @"/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemID);
                if (DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
                else
                    Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].Quantity.ToString ();
                Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Inventory.DBItems, null));
            }
            if (haveInventory) {
                DataUserController.Inventory.DBItems[slotExists].Quantity += itemcreated.Quantity;
                Items[slotExists].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[slotExists].Quantity.ToString ();
            }
        }
    }

    /// <summary>
    /// Thêm item vào thùng đồ
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="ItemType"></param>
    /// <param name="trapvalue"></param>
    private void AddItemCraftToInventory (sbyte itemType, byte itemID, int itemQuantity) {
        if (Items.Count < Slot) {
            bool haveInventory = false; //Xác định item có trong thùng đồ hay chưa với loại item use hoặc quest
            int slotExists = -1;
            var itemcreated = ItemSystem.CreateItem (false, false, 1, itemType, itemID, itemQuantity);
            //Nếu là item khác loại trang bị thì mới check cộng dồn
            if (!itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
                var count = DataUserController.Inventory.DBItems.Count;
                for (int y = 0; y < count; y++) {
                    if (DataUserController.Inventory.DBItems[y].ItemID.Equals (itemcreated.ItemID)) {
                        haveInventory = true;
                        slotExists = y;
                        break;
                    }
                }
            }
            if (itemcreated.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || !haveInventory) {
                DataUserController.Inventory.DBItems.Add (itemcreated);
                var temp = DataUserController.Inventory.DBItems.Count - 1;
                Items.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
                Items[DataUserController.Inventory.DBItems.Count - 1].transform.SetParent (ItemSlot[DataUserController.Inventory.DBItems.Count - 1].transform, false);
                Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemType + @"/" + DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemID);
                if (DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
                else
                    Items[DataUserController.Inventory.DBItems.Count - 1].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.Count - 1].Quantity.ToString ();
                Items[DataUserController.Inventory.DBItems.Count - 1].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Inventory.DBItems, null));
            }
            if (haveInventory) {
                DataUserController.Inventory.DBItems[slotExists].Quantity += itemcreated.Quantity;
                Items[slotExists].transform.GetChild (0).GetComponent<Text> ().text = DataUserController.Inventory.DBItems[slotExists].Quantity.ToString ();
            }
        }
    }

    //Xóa 1 item trong thùng đồ (-1 = xóa all)
    private void RemoveItems (int slotdestroy) {
        switch (slotdestroy) {
            case -1: //Xóa all item
                DataUserController.Inventory.DBItems.Clear ();
                break;
            default:
                DataUserController.Inventory.DBItems.RemoveAt (slotdestroy);
                break;
        }
    }

    //Sắp xếp lại item trong thùng đồ
    private void ReSortItems (List<ItemModel> inventory) {
        //Xóa item với những item số lượng = 0
        var countTotalQuantity = inventory.Count;
        for (int i = countTotalQuantity - 1; i >= 0; i--)
            if (inventory[i].Quantity <= 0)
                inventory.RemoveAt (i);
        //----------------------------------------
        if (Items.Count < inventory.Count) {
            var count = inventory.Count - Items.Count;
            for (int i = 0; i < count; i++)
                Items.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
        }
        var count3 = inventory.Count;
        for (int i = 0; i < count3; i++) {
            var temp = i;
            Items[i].transform.SetParent (ItemSlot[i].transform, false);
            Items[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + inventory[i].ItemType + @"/" + inventory[i].ItemID);
            if (inventory[i].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                Items[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + inventory[i].ItemLevel.ToString ();
            else
                Items[i].transform.GetChild (0).GetComponent<Text> ().text = inventory[i].Quantity.ToString ();
            Items[i].GetComponent<Button> ().onClick.RemoveAllListeners ();
            Items[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, inventory, null));
        }
        if (Items.Count > inventory.Count) {
            var count = Items.Count - 1;
            for (int i = count; i >= inventory.Count; i--) {
                Destroy (Items[i]);
                Items.RemoveAt (i);
            }
        }
        //Đưa màu item về chuẩn
        var count2 = inventory.Count;
        for (int i = 0; i < Slot; i++) {
            if (i < count2) {
                if (inventory[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6
                    ItemSlotImg[i].sprite = Resources.Load<Sprite> ("Images/BorderItem/" + inventory[i].ItemColor);
            } else {
                ItemSlotImg[i].sprite = Resources.Load<Sprite> ("Images/BorderItem/0");
            }
        }
        ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ
    }
    #endregion

    #endregion

    #region Button Click 

    /// <summary>
    /// Ẩn cửa sổ thông tin chi tiết item
    /// </summary>
    /// <param name="type">0: cửa sổ của inventory, 1: cửa sổ của craft</param>
    public void ButtonHideItemsInformations (int type) {
        if (type.Equals (0)) {
            ObjectController[1].SetActive (false);
            if (ViewingItemType.Equals (null)) //Nếu là kiểu xem item trong inventory
            {
                ReSortItems (DataUserController.Inventory.DBItems); //Resort để thay đổi các chỉ số sau khi nâng cấp
                ActionViewFilterItems ();
            }
            // if (ViewingItemType.Equals (0)) //Nếu là kiểu xem item trong chi tiết nhân vật, chưa trang bị
            //     SetupItemInEquip ();
            // if (ViewingItemType.Equals (1)) //Nếu là kiểu xem item trong chi tiết nhân vật, đã trang bị
            //     SetupItemEquiped ();
        }
        // if (type.Equals (1))
        //     ObjCraft[13].SetActive (false);
        ButtonFunction (1); //refresh lại tiền tệ 
    }

    /// <summary>
    /// Nút bán item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonSellItems (BaseEventData eventData) {
        //Trường hợp bán nhiều item
        if (Action == ActionType.SelectItems) {
            //Tính tổng số tiền
            var quantitytotal = 0; //Tổng số item được tick
            var amounttotal = 0; //Tổng số tiền
            for (int i = 0; i < Items.Count; i++) {
                if (Items[i].transform.GetChild (1).GetComponent<Image> ().gameObject.activeSelf) {
                    quantitytotal++;
                    amounttotal += (int) InventoryTemplate[i].ItemPrice; //ItemSystem.GetItemPrice (InventoryTemplate[i]);
                }
            }
            if (quantitytotal > 0) //Có hơn 1 item được check thì mới show dialog bán
            {
                //Show dialog xác nhận bán item
                GameSystem.ShowConfirmDialog (string.Format (Languages.lang[130], quantitytotal, amounttotal));
                //Chờ lệnh confirm
                StartCoroutine (ActionRemoveItems (0));
            } else goto End;
        }
        //Bán 1 item khi đang xem item đó
        if (Action == ActionType.ViewInfor) {
            ObjectController[26].GetComponent<Slider> ().maxValue = InventoryTemplate[ItemSlotViewing].Quantity;
            if (InventoryTemplate[ItemSlotViewing].Quantity > 1) {
                ObjectController[25].SetActive (true); //Hiển thị UI kéo thả item
                StartCoroutine (ShowInforSelling ());
            } else {
                var amounttotal = InventoryTemplate[ItemSlotViewing].ItemPrice; //ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Tổng số tiền
                //Show dialog xác nhận bán item
                GameSystem.ShowConfirmDialog (string.Format (Languages.lang[131], amounttotal));
                StartCoroutine (ActionRemoveItems (1));
            }

            //Chờ lệnh confirm
        }
        End:
            ReSortItems (InventoryTemplate);
        ButtonFunction (1); //refresh lại tiền tệ 
        return;
    }
    /// <summary>
    /// Action show dialog chờ confirm bán items, 0: bán nhiều item, 1: bán từng item
    /// </summary>
    /// <param name="type">0: bán nhiều item, 1: bán từng item</param>
    /// <returns></returns>

    /// <summary>
    /// Thao tác với item
    /// 0: nâng cấp
    /// 1: nâng phẩm
    /// 2: phân giải
    /// </summary>
    /// <param name="type"></param>
    public void ButtonModifyItem (int type) {
        switch (type) {

            #region Nâng cấp item 

            case 0: //Nâng cấp item
                //Xem item trong inventory
                if (InventoryTemplate[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
                    if (ValidModifyItem (type, InventoryTemplate[ItemSlotViewing].ItemLevel, 1)) //Check điều kiện nâng cấp
                    {
                        InventoryTemplate[ItemSlotViewing].ItemLevel++; //Tăng level
                        InventoryTemplate[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Update lại giá bán
                        DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (InventoryTemplate[ItemSlotViewing].ItemGuidID))].ItemLevel = InventoryTemplate[ItemSlotViewing].ItemLevel;
                        ItemClick (ItemSlotViewing, InventoryTemplate, null);
                        DataUserController.SaveInventory ();
                        ShowEffectCraftSuccess (1);
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[157])); //Nâng cấp thành công
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                } else
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[158])); //Đã nâng cấp tối đa

                break;

                #endregion

                #region Nâng phẩm item 

            case 1: //Nâng phẩm item
                //Xem item trong inventory
                if (InventoryTemplate[ItemSlotViewing].ItemColor < 6) {
                    if (ValidModifyItem (type, InventoryTemplate[ItemSlotViewing].ItemColor, 1)) //Check điều kiện nâng cấp
                    {
                        InventoryTemplate[ItemSlotViewing].ItemColor++;
                        InventoryTemplate[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Update lại giá bán
                        DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (InventoryTemplate[ItemSlotViewing].ItemGuidID))].ItemColor = InventoryTemplate[ItemSlotViewing].ItemColor;
                        ItemClick (ItemSlotViewing, InventoryTemplate, null);
                        DataUserController.SaveInventory ();
                        ShowEffectCraftSuccess (1);
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[159])); //Nâng cấp thành công
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                } else
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[160])); //Đã nâng cấp tối đa

                break;

                #endregion
            default:
                break;
        }
        ObjectController[8].SetActive (false);
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyItem (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (InventoryTemplate[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
            ValidModifyItem (0, InventoryTemplate[ItemSlotViewing].ItemLevel, 0);
            ObjectController[8].SetActive (true);
        }
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng phẩm item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyItem2 (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (InventoryTemplate[ItemSlotViewing].ItemColor < 6) {
            ValidModifyItem (1, InventoryTemplate[ItemSlotViewing].ItemColor, 0);
            ObjectController[8].SetActive (true);
        }
    }

    /// <summary>
    /// Nhả nút xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonHideRequiredModifyItem (BaseEventData eventData) {
        ObjectController[8].SetActive (false);
    }

    /// <summary>
    /// Check xem đủ điều kiện nâng cấp item hay không
    /// </summary>
    /// <param name="typeModify">Kiểu nâng cấp: 0: nâng cấp, 1: nâng phẩm</param>
    /// <param name="currentLevel">Level hiện tại hoặc màu sắc hiện tại</param>
    /// <param name="typeCheck">0: show thông tin, 1: check để trừ item</param>
    private bool ValidModifyItem (int typeModify, int currentLevel, int typeCheck) {
        var slotItemRequerid = -1;
        #region Tìm id có trong list 

        var count = DataUserController.Inventory.DBItems.Count;
        var itemInforUpgrade = typeModify.Equals (0) ? ItemCoreSetting.ItemUpgradeLevel[currentLevel].Split (';') [0].Split (',') : ItemCoreSetting.ItemUpgradeColor[currentLevel].Split (';') [0].Split (','); //Lấy thông tin đá cường hóa cho vào mảng, sau này sẽ update nhiều loại sau
        var itemForUpgrade = DataUserController.Inventory.DBItems.Find (x => x.ItemType == Convert.ToSByte (itemInforUpgrade[0]) && x.ItemID == Convert.ToByte (itemInforUpgrade[1])); //Lấy thông tin đá cường hóa nếu có trong inventory
        if (itemForUpgrade != null) //Nếu tồn tại item cần để nâng cấp trong inventory => lấy index của nó
            slotItemRequerid = DataUserController.Inventory.DBItems.FindIndex (x => x.Equals (itemForUpgrade));

        #endregion
        ObjectController[9].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemInforUpgrade[0] + @"/" + itemInforUpgrade[1]); //Gán hình item
        ItemInforText[47].text = (itemForUpgrade != null?itemForUpgrade.Quantity.ToString (): "0") + "/" + itemInforUpgrade[2]; //Gán text số lượng item cần thiết/hiện có
        //Nếu có item trong danh sách thùng đồ thì mới xử lý tiếp
        if (itemForUpgrade != null) {
            if (itemForUpgrade.Quantity >= Convert.ToInt32 (itemInforUpgrade[2])) //Check đủ số lượng
            {
                if (typeCheck.Equals (1)) //Nếu kiểu check để thực hiện nâng cấp, thì mới trừ item
                {
                    InventorySystem.ReduceItemQuantityInventory (Convert.ToSByte (itemInforUpgrade[0]), Convert.ToByte (itemInforUpgrade[1]), Convert.ToInt32 (itemInforUpgrade[2]));
                    if (ItemsFilter[1])
                        ReSortItems (DataUserController.Inventory.DBItems);
                    DataUserController.SaveInventory ();
                }
                //ReduceItemQuantity (slotItemRequerid, DataUserController.Inventory.DBItems, typeModify.Equals (0) ? quantityItemForUpgrade : quantityItemForUpQuality); //Trừ item trong thùng đồ
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Thực hiện xóa item khỏi thùng đồ
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator ActionRemoveItems (int type) {
        yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
        //Accept
        if (GameSystem.ConfirmBoxResult == 1) {
            float price = 0; //Giá sẽ bán dc
            // List<int> arrayindex = new List<int> ();
            if (type.Equals (0)) //Bán multi
            {
                var count = Items.Count - 1;
                for (int i = count; i >= 0; i--) {
                    if (Items[i].transform.GetChild (1).GetComponent<Image> ().gameObject.activeSelf) {
                        //arrayindex.Add (i);
                        price += InventoryTemplate[i].ItemPrice;
                        InventorySystem.RemoveItem (InventoryTemplate[i]);
                    }
                }
            } else //Bán đơn item
            {
                var quantitySell = (int) SliderItemSell.value;
                price += ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]) / InventoryTemplate[ItemSlotViewing].Quantity * quantitySell;
                if (!InventoryTemplate[ItemSlotViewing].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    InventorySystem.ReduceItemQuantityInventory (InventoryTemplate[ItemSlotViewing].ItemType, InventoryTemplate[ItemSlotViewing].ItemID, quantitySell);
                else
                    InventorySystem.RemoveItem (InventoryTemplate[ItemSlotViewing]);
            }
            UserSystem.IncreaseGolds (price); //Cộng vàng sau khi bán
            DataUserController.SaveUserInfor ();
            RemoveCheckItem ();
            if (type.Equals (0))
                ButtonSelectItems (null);
            ReGenInventoryAfterSell ();
            ReSortItems (InventoryTemplate);
            if (type.Equals (1)) //Trường hợp bán 1 item khi đang xem item đó
            {
                //ReSortItems (InventoryTemplate);
                var count = InventoryTemplate.Count;
                if (count.Equals (0)) {
                    ButtonHideItemsInformations (0);
                } else if (ItemSlotViewing > count - 1) {
                    ItemSlotViewing = count - 1;
                    ItemClick (ItemSlotViewing, InventoryTemplate, null);
                } else {
                    ItemClick (ItemSlotViewing, InventoryTemplate, null);
                }
            }
            ButtonFunction (1); //Gán lại giá trị tiền tệ
            ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ
            DataUserController.SaveInventory ();
        }
    }
    //Nút select item
    public void ButtonSelectItems (BaseEventData eventData) {
        if (!Action.Equals (ActionType.EquipToBag)) //Không phải chế độ trang bị item vào bag thì mới cho phép select item
        {
            Action = Action == ActionType.SelectItems ? ActionType.ViewInfor : ActionType.SelectItems;
            if (Action.Equals (ActionType.SelectItems)) {
                ItemInforText[8].text = Languages.lang[67];
                //ItemInforText[4].text = string.Format (Languages.lang[131], Languages.lang[137], "", "");
                ObjectController[24].SetActive (true);
                ObjectController[3].GetComponent<Image> ().color = new Color (255, 255, 255, 1f);
            } else {
                ItemInforText[8].text = Languages.lang[137];
                //ItemInforText[4].text = string.Format (Languages.lang[131], Languages.lang[132], Languages.lang[133], "");
                ObjectController[24].SetActive (false);
                ObjectController[3].GetComponent<Image> ().color = new Color (255, 255, 255, 0.5f);
            }
            RemoveCheckItem ();
        }
        //ButtonHideItemsInformations();
    }
    //Bỏ check all item đang được check
    private void RemoveCheckItem () {
        for (int i = 0; i < Items.Count; i++)
            Items[i].transform.GetChild (1).GetComponent<Image> ().gameObject.SetActive (false);
    }
    #region Lọc item theo loại 

    /// <summary>
    /// Lọc item trong thùng đồ theo loại
    /// 0 = trang bị
    /// 1 = sử dụng
    /// 2 = nhiệm vụ
    /// </summary>
    /// <param name="typeItem"></param>
    public void ButtonFilterItem (int typeItem) {
        if (Action.Equals (ActionType.ViewInfor)) {
            ItemsFilter[typeItem] = !ItemsFilter[typeItem];
            ActionViewFilterItems ();
        }
    }
    /// <summary>
    /// Xem inventory theo tùy chọn của player
    /// </summary>
    private void ActionViewFilterItems () {
        if (!ItemsFilter[0] && !ItemsFilter[1] && !ItemsFilter[2]) {
            ItemsFilter[0] = ItemsFilter[1] = ItemsFilter[2] = true;
        }
        if (!ItemsFilter[0])
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[12], RectTransformButtonItemFilter[0].localPosition, new Vector2 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[0].localPosition.y), .2f, moveCurve));
        else
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[12], RectTransformButtonItemFilter[0].localPosition, new Vector2 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[0].localPosition.y), .2f, moveCurve));
        if (!ItemsFilter[1])
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[13], RectTransformButtonItemFilter[1].localPosition, new Vector2 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[1].localPosition.y), .2f, moveCurve));
        else
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[13], RectTransformButtonItemFilter[1].localPosition, new Vector2 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[1].localPosition.y), .2f, moveCurve));
        if (!ItemsFilter[2])
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[14], RectTransformButtonItemFilter[2].localPosition, new Vector2 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[2].localPosition.y), .2f, moveCurve));
        else
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[14], RectTransformButtonItemFilter[2].localPosition, new Vector2 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[2].localPosition.y), .2f, moveCurve));
        InventoryTemplate.RemoveRange (0, InventoryTemplate.Count);
        if (ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.ToList ();
        } else if (ItemsFilter[0] && !ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).ToList ();
        } else if (!ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (!ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).ToList ();
        } else if (ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (!ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.FindAll (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).ToList ();
        }
        ReSortItems (InventoryTemplate);
    }

    #endregion
    /// <summary>
    /// Sắp xếp lại item trong thùng đồ
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonRefreshInventory (BaseEventData eventData) {
        if (Action.Equals (ActionType.ViewInfor)) {
            ItemsFilter[0] = ItemsFilter[1] = ItemsFilter[2] = true;
            RectTransformButtonItemFilter[0].localPosition = !ItemsFilter[0] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[0].localPosition.y, RectTransformButtonItemFilter[0].localPosition.z) :
                new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[0].localPosition.y, RectTransformButtonItemFilter[0].localPosition.z);
            RectTransformButtonItemFilter[1].localPosition = !ItemsFilter[1] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[1].localPosition.y, RectTransformButtonItemFilter[1].localPosition.z) :
                new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[1].localPosition.y, RectTransformButtonItemFilter[1].localPosition.z);
            RectTransformButtonItemFilter[2].localPosition = !ItemsFilter[2] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[2].localPosition.y, RectTransformButtonItemFilter[2].localPosition.z) :
                new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[2].localPosition.y, RectTransformButtonItemFilter[2].localPosition.z);
            var itemtequip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).OrderByDescending (x => x.ItemPrice).ToList ();
            var itemtuse = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).OrderBy (x => x.ItemID).ToList ();
            var itemtquest = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemType).ThenBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (itemtequip);
            DataUserController.Inventory.DBItems.AddRange (itemtuse);
            DataUserController.Inventory.DBItems.AddRange (itemtquest);
            InventoryTemplate.RemoveRange (0, InventoryTemplate.Count);
            InventoryTemplate = DataUserController.Inventory.DBItems.ToList (); //Set lại inventory tạm thời
            ReSortItems (DataUserController.Inventory.DBItems);
            DataUserController.SaveInventory ();
        }
    }

    /// <summary>
    /// Xem item tiếp theo hoặc trước đó trong thùng đồ
    /// </summary>
    /// <param name="type">0: prev, 1: next</param>
    public void ButtonNextOrPrevItem (int type) {

        ItemSlotViewing += type.Equals (0) ? -1 : 1;
        if (ItemSlotViewing > InventoryTemplate.Count - 1)
            ItemClick (0, InventoryTemplate, null);
        else if (ItemSlotViewing < 0)
            ItemClick (InventoryTemplate.Count - 1, InventoryTemplate, null);
        else ItemClick (ItemSlotViewing, InventoryTemplate, null);
    }

    /// <summary>
    /// Gen ra inventory mới sau khi bán item
    /// </summary>
    private void ReGenInventoryAfterSell () {
        InventoryTemplate.Clear ();
        if (ItemsFilter[0]) //Item equip
            InventoryTemplate.AddRange (DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).OrderByDescending (x => x.ItemPrice));
        if (ItemsFilter[1]) //Item use
            InventoryTemplate.AddRange (DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).OrderBy (x => x.ItemID));
        if (ItemsFilter[2]) //Item quest
            InventoryTemplate.AddRange (DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemID));
    }

    /// <summary>
    /// Button mở rộng thêm thùng đồ bằng việc xem video
    /// </summary>
    public void WatchADSForSlotInventory () {
        if (ADS.rewardBasedVideoSlotInventory.IsLoaded ()) {
            GameSystem.ShowConfirmDialog (Languages.lang[180]); //Xác nhận xem video
            StartCoroutine (WaitConfirmWatchVideo ());
        } else
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[181])); //Chờ video tiếp theo dc tải
    }

    /// <summary>
    /// Chờ thực hiện chọn xem video
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitConfirmWatchVideo () {
        yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
        //Accept
        if (GameSystem.ConfirmBoxResult == 1) {
            ADS.rewardBasedVideoSlotInventory.Show (); //Hiển thị video quảng cáo
        }
    }

    /// <summary>
    /// Button function lẻ tẻ
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunction (int type) {
        switch (type) {
            case 0: //hiển thị UI xem video nhận vàng
                //ObjectController[31].SetActive (true);
                break;
            case 1: //refresh vàng và gem
                ItemInforText[50].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                ItemInforText[51].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            case 2: //Đóng UI inventory
                GameSystem.DisposePrefabUI (1);
                break;
            case 3: //Đóng UI kéo thả lựa chọn item
                ObjectController[25].SetActive (false);
                break;
            case 4: //Chọn bán item từ UI kéo thả lựa chọn item
                var amounttotal = ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]) / InventoryTemplate[ItemSlotViewing].Quantity * (int) SliderItemSell.value;
                GameSystem.ShowConfirmDialog (string.Format (Languages.lang[131], amounttotal));
                StartCoroutine (ActionRemoveItems (1));
                ObjectController[25].SetActive (false);
                break;
            case 5: //Trừ 1 số lượng item để bán
                SliderItemSell.value = SliderItemSell.value > SliderItemSell.minValue?SliderItemSell.value - 1 : SliderItemSell.minValue;
                break;
            case 6: //Cộng 1 số lượng item để bán
                SliderItemSell.value = SliderItemSell.value < SliderItemSell.maxValue?SliderItemSell.value + 1 : SliderItemSell.maxValue;
                break;
            case 7: //Phân giải item
                if (DataUserController.User.InventorySlot - DataUserController.Inventory.DBItems.Count < 3) {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[140])); // "Không đủ chỗ trống trong thùng đồ";
                    break;
                }

                //Show dialog xác nhận phân giải item
                GameSystem.ShowConfirmDialog (Languages.lang[278]);
                StartCoroutine (WaitingForActions (1));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chờ thực hiện thao tác từ use
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator WaitingForActions (int type) {
        switch (type) {
            case 0:
                break;
            case 1: //Phân giải item
                //Chờ thao tác từ use
                yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);

                //Xác nhận đồng ý
                if (GameSystem.ConfirmBoxResult == 1) {
                    //Kiểm tra item là trang bị
                    if (ItemViewing.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
                        if (ItemSystem.BreakItem (ItemViewing)) //Nếu phân giải thành công
                        {
                            GlobalVariables.ItemDetailAction = 5;
                            DataUserController.SaveInventory ();
                            ReGenInventoryAfterSell ();
                            ReSortItems (InventoryTemplate);
                            //GameSystem.DisposePrefabUI (6); //Đóng UI
                            ButtonHideItemsInformations (0);
                            StartCoroutine (GameSystem.ControlFunctions.ShowMessagecontinuity (GlobalVariables.NotificationText)); //GameSystem.ControlFunctions.ShowMessage( (GlobalVariables.NotificationText));//Thông báo đã nhận dc những gì
                        } else {
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[273])); // "Không thể phân giải vật phẩm này";
                        }
                    } else {
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[273])); // "Không thể phân giải vật phẩm này";
                    }
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #endregion

    /// <summary>
    /// Hiển thị hiệu ứng chế tạo thành công
    /// </summary>
    /// <param name="type">0: craft item, 1: cường hóa hoặc nâng phẩm item</param>
    private void ShowEffectCraftSuccess (int type) {
        var count = EffectCraftSuccess.Count;
        for (int i = 0; i < count; i++) {
            if (!EffectCraftSuccess[i].activeSelf) {
                EffectCraftSuccess[i].SetActive (true);
                EffectCraftSuccess[i].transform.position = new Vector3 (ObjectController[2].transform.position.x, ObjectController[2].transform.position.y, 0);
                break;
            }
            if (i == count - 1) {
                EffectCraftSuccess.Add ((GameObject) Instantiate (EffectCraftSuccess[0], new Vector3 (ObjectController[2].transform.position.x, ObjectController[2].transform.position.y, 0), Quaternion.identity));
                EffectCraftSuccess[EffectCraftSuccess.Count - 1].transform.SetParent (ObjectController[11].transform, false);
                EffectCraftSuccess[EffectCraftSuccess.Count - 1].transform.position = new Vector3 (ObjectController[2].transform.position.x, ObjectController[2].transform.position.y, 0);
            }
        }
    }

    /// <summary>
    /// Hiển thị số lượng và giá bán realtime
    /// </summary>
    private IEnumerator ShowInforSelling () {
        Begin : ItemInforText[14].text = Languages.lang[75] + SliderItemSell.value.ToString (); //So luong
        ItemInforText[15].text = Languages.lang[76] + (ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]) / InventoryTemplate[ItemSlotViewing].Quantity * SliderItemSell.value).ToString (); //Gia ban theo so luong
        yield return new WaitForSeconds (0);
        if (ObjectController[25].activeSelf)
            goto Begin;
    }
}