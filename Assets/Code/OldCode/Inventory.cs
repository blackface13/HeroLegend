using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    #region Inventory System 
    private double Slot = 0; //Slot tối đa
    private int ItemSlotViewing = -1; //Slot item đang được xem, dùng để bán hoặc xem item tiếp theo
    private int HorizontalQuantity = 13; //Số slot trên 1 hàng ngang (được tính toán lại ở hàm SetupInventory phía dưới, vì nó scale theo tỉ lệ màn hình mỗi thiết bị)
    public GameObject[] Obj; //Set in interface
    public Text[] ItemInforText; //Set in interface
    private GameObject[] ItemSlot;
    private Image[] ItemSlotImg; //Hình nền phía sau của item, thể hiện màu của item
    private Image ItemDetailColor; //Hình nền phía sau của item, thể hiện màu item, chi tiết item
    public GameObject[] ItemBag1Slot; //Set in interface
    private List<GameObject> Items = new List<GameObject> ();
    private List<GameObject> ItemBag1 = new List<GameObject> ();
    private List<ItemModel> InventoryTemplate = new List<ItemModel> ();
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
    private GameObject ObjectEffectSelected;
    #endregion

    #region Craft System 

    public GameObject[] ObjCraft;
    public List<GameObject> ButtonListCraftItemUse;
    public List<GameObject> ButtonListCraftItemWeaponPhysic;
    public List<GameObject> ButtonListCraftItemWeaponMagic;
    public List<GameObject> ButtonListCraftItemDefense;
    public List<GameObject> ButtonListCraftItemSets;
    private Image ItemToCraft;
    private int TypeCraftSelected = -1; //Kiểu craft đang chọn. 0 = equip, 1 = use, 2 = quest
    private int SlotItemCraftSelected = -1; //Slot item đang lựa chọn để craft
    private List<GameObject> EffectCraftSuccess;
    private int LevelCraft = 1; //Cấp độ để chế tạo item
    public AnimationCurve moveDrawnCrafting; //Hiệu ứng đường đi của hiệu ứng craft
    private Vector2[] PositionMoveEffectCrafting; //Danh sách tọa độ các object item (0 = tọa độ object item sẽ được craft)
    private GameObject[] EffectCrafting = new GameObject[3]; //Tối đa 1 item được craft từ 3 nguyên liệu khác nhau
    #endregion

    #region Hero System 
    public GameObject[] ObjHeroSystem;
    private List<GameObject> Hero;
    private List<Image> HeroAvt;
    List<HeroesProperties> HeroListTemplate = new List<HeroesProperties> ();
    private int HorizontalQuantityEquip = 6; //Số slot trên 1 hàng ngang của hệ thống trang bị vật phẩm cho hero
    private List<GameObject> ListBackgroundItemEquip = new List<GameObject> ();
    private List<GameObject> ListItemEquip = new List<GameObject> ();
    private ItemModel ItemViewing;
    private int SlotHeroViewing; //Lưu slot hero đang xem
    private List<GameObject> HeroObject; //Danh sách animation hero chuyển động
    #endregion

    // Use this for initialization
    void Start () {
        #region Khởi tạo hoặc set Canvas thông báo cho Scene 

        try {
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
        } catch {
            GameSystem.Initialize (); //Khởi tạo này dành cho scene nào test ngay
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
            GameSystem.MessageCanvas.GetComponent<Canvas> ().planeDistance = 1;
        }
        #endregion
        ObjectEffectSelected = Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ParticleEffectSelected"), Obj[17].transform.position, Quaternion.identity); //Hiệu ứng selected nằm ở button Heroes
        Action = ActionType.ViewInfor;
        //Languages.SetupLanguage (1); //Set tạm
        SetText ();
        DataUserController.LoadUserInfor ();
        DataUserController.LoadHeroDefault ();
        DataUserController.LoadHeroes ();
        DataUserController.LoadInventory (); //Set tạm
        Slot = DataUserController.User.InventorySlot; //Gán tổng số slot bằng slot inventory của user
        ItemDropController.Initialize (); //Khởi tạo item drop
        SetupInventory (); //Khởi tạo inventory
        SetupHeroList (); //Khởi tạo hero
        SetupAddItems (); //Vẽ item lên màn hình
        //print(string.Format("fjdlsa {0} and {1}", 123, 234));
        SetupCraftSystem ();
        ButtonRefreshInventory (null);
    }

    #region Inventory Functions 

    /// <summary>
    /// Gán ngôn ngữ vào các text
    /// </summary>
    private void SetText () {
        //ItemInforText[4].text = string.Format(Languages.lang[131], Languages.lang[132], Languages.lang[133], "");//Gán text kiểu xem (default = all)
        ItemInforText[5].text = Languages.lang[134];
        ItemInforText[6].text = Languages.lang[135];
        ItemInforText[7].text = Languages.lang[136];
        ItemInforText[8].text = Languages.lang[137];
        ItemInforText[9].text = ItemInforText[12].text = Languages.lang[138];
        ItemInforText[11].text = Languages.lang[139];
        ItemInforText[15].text = Languages.lang[134];
        ItemInforText[16].text = Languages.lang[135];
        ItemInforText[17].text = Languages.lang[136];
        ItemInforText[18].text = Languages.lang[144];
        ItemInforText[19].text = Languages.lang[145];
        ItemInforText[20].text = Languages.lang[146];
        ItemInforText[21].text = Languages.lang[145];
        ItemInforText[22].text = Languages.lang[147];
        ItemInforText[23].text = Languages.lang[150];
        ItemInforText[24].text = Languages.lang[151];
        ItemInforText[25].text = Languages.lang[152];
        ItemInforText[26].text = Languages.lang[153];
        //Thông tin chi tiết nhân vật
        ItemInforText[34].text = Languages.lang[11]; //Thông tin
        ItemInforText[35].text = Languages.lang[12]; //Trang bị
        ItemInforText[36].text = Languages.lang[13]; //Kỹ năng
        ItemInforText[37].text = Languages.lang[15]; //Tiểu sử
        ItemInforText[38].text = Languages.lang[14]; //Đặc biệt
        ItemInforText[39].text = Languages.lang[12]; //Trang bị
        ItemInforText[40].text = Languages.lang[48]; //Gỡ bỏ
        ItemInforText[41].text = Languages.lang[79]; //Trang bị nhanh
        ItemInforText[42].text = Languages.lang[78]; //Gỡ trang bị nhanh
        ItemInforText[43].text = Languages.lang[154]; //Nâng cấp
        ItemInforText[44].text = Languages.lang[155]; //Nâng phẩm
        ItemInforText[45].text = Languages.lang[156]; //Phân giải
        ItemInforText[46].text = Languages.lang[148]; //Yêu cầu

        ItemInforText[48].text = Languages.lang[165]; //Nội tại và kỹ năng
        ItemInforText[50].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
        ItemInforText[51].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
        ItemInforText[52].text = Languages.lang[265]; //Lựa chọn trang bị
        ItemInforText[32].text = Languages.lang[22]; //Text chỉ số
        ItemInforText[27].text = "";
        for (int i = 200; i < 238; i++) {
            ItemInforText[27].text += Languages.lang[i] + "\n";
        }
    }

    #region Khởi tạo 
    /// <summary>
    /// Khởi tạo số lượng, sắp xếp vị trí slot inventory
    /// </summary>
    /// <returns></returns>
    private void SetupInventory () {
        ItemDetailColor = Obj[25].GetComponent<Image> ();
        HorizontalQuantity = Convert.ToInt32 (Obj[11].GetComponent<RectTransform> ().sizeDelta.x + Obj[10].GetComponent<RectTransform> ().sizeDelta.x) / 208; //Tính lại số lượng item trên 1 hàng theo scale của màn hình
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        ItemSlot = new GameObject[(int) Slot];
        ItemSlotImg = new Image[(int) Slot];
        float regionSpace = 210f; //Khoảng cách giữa các object
        Obj[0].GetComponent<RectTransform> ().sizeDelta = Slot % HorizontalQuantity == 0 ? new Vector2 (0, (float)(Slot / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((float)(Slot / HorizontalQuantity) + 1) * regionSpace);
        float verticalcounttemp = Obj[0].GetComponent<RectTransform> ().sizeDelta.y / 2 - 105;
        //Khởi tạo button inventory
        for (int i = 0; i < Slot; i++) {
            ItemSlot[i] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 102 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity);
            ItemSlot[i].transform.SetParent (Obj[0].transform, false);
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
        PositionButtonItemFilter[0] = Obj[12].GetComponent<RectTransform> ().localPosition.x; //Set tọa độ ban đầu
        PositionButtonItemFilter[1] = 230f; //Khoảng cách sẽ dịch chuyển
        PositionButtonItemFilter[2] = 3000f; //Tốc độ dịch chuyển
        RectTransformButtonItemFilter = new RectTransform[3];
        RectTransformButtonItemFilter[0] = Obj[12].GetComponent<RectTransform> ();
        RectTransformButtonItemFilter[1] = Obj[13].GetComponent<RectTransform> ();
        RectTransformButtonItemFilter[2] = Obj[14].GetComponent<RectTransform> ();

        #endregion
    }

    /// <summary>
    /// Thêm item vào thùng đồ
    /// </summary>
    public void SetupAddItems () {
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
    }

    /// <summary>
    /// Click vào item trong thùng đồ
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="inventory"></param>
    /// <param name="type">Kiểu xem: null = xem từ inventory, 0 = xem từ danh sách trang bị, 1 = xem trang bị đã được trang bị</param>
    public void ItemClick (int slot, List<ItemModel> inventory, int? type) {
        ViewingItemType = type;
        ItemDetailColor.sprite = Resources.Load<Sprite> ("Images/BorderItem/" + inventory[slot].ItemColor.ToString ());
        switch (Action) {
            case ActionType.ViewInfor: //Chế độ xem
                ItemSlotViewing = slot; //Gán slot item đang được click vào
                Obj[1].SetActive (true);
                Obj[2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + inventory[slot].ItemType + @"/" + inventory[slot].ItemID);
                ItemInforText[0].text = ItemSystem.GetItemName (inventory[slot].ItemType, inventory[slot].ItemID); //Tên item
                if (inventory[slot].ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip))
                    ItemInforText[1].text = Languages.lang[23] + inventory[slot].ItemLevel;
                else
                    ItemInforText[1].text = Languages.lang[75] + inventory[slot].Quantity;
                switch (inventory[slot].ItemTypeMode) {
                    case global::ItemModel.TypeMode.Equip: //Hiển thị 3 nút nâng cấp, nâng phẩm và phân giải đối với item equip
                        Obj[26].SetActive (true);
                        Obj[27].SetActive (true);
                        ItemInforText[2].text = ItemSystem.GetItemDescription (inventory[slot]); //Description
                        break;
                    default:
                        //Ẩn thị 3 nút nâng cấp, nâng phẩm và phân giải
                        Obj[26].SetActive (false);
                        Obj[27].SetActive (false);
                        //Obj[28].SetActive (false);
                        ItemInforText[2].text = ItemSystem.GetItemDescription (inventory[slot]); // Description
                        break;
                }
                var lineCount = ItemInforText[2].text.Split ('\n');
                var sizeHeight = 66f * lineCount.Length;
                Obj[34].GetComponent<RectTransform> ().sizeDelta = new Vector2 (ItemInforText[2].GetComponent<RectTransform> ().sizeDelta.x, sizeHeight > 760f?sizeHeight : 760f); //760 là fix cứng khi thiết kế
                //print (lineCount.Length);
                ItemInforText[3].text = Languages.lang[76] + inventory[slot].ItemPrice; //ItemSystem.GetItemPrice (inventory[slot]); //Giá bán
                // Obj[20].SetActive (Obj[10].activeSelf); //Chỉ hiển thị 2 nút next và prev khi xem trang bị trong inventory
                // Obj[21].SetActive (Obj[10].activeSelf);
                Obj[22].SetActive (Obj[10].activeSelf); //Ẩn button bán item đơn lẻ
                if (!Obj[10].activeSelf) //Nếu inventory ko active (tức là đang ở hệ thống trang bị item cho nhân vật)
                {
                    if (type.Equals (0)) { //Xem trang bị chưa dc trang bị
                        Obj[23].SetActive (true); // button trang bị
                        Obj[24].SetActive (false); // button gỡ trang bị
                    }
                    if (type.Equals (1)) { //Xem trang bị đã được trang bị
                        Obj[23].SetActive (false); // button trang bị
                        Obj[24].SetActive (true); // button gỡ trang bị
                    }
                    ItemViewing = inventory[slot]; //Gán tạm item để thao tác
                } else {
                    Obj[23].SetActive (false); // button trang bị
                    Obj[24].SetActive (false); // button gỡ trang bị
                }
                break;
            case ActionType.SelectItems: //Chế độ lựa chọn item
                Items[slot].transform.GetChild (1).GetComponent<Image> ().gameObject.SetActive (!Items[slot].transform.GetChild (1).GetComponent<Image> ().gameObject.activeSelf);

                // Obj[1].SetActive(true);
                // Obj[2].GetComponent<Image>().sprite = inventory[slot].Icon;
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
    private void AddItems (ItemModel item) {
        DataUserController.Inventory.DBItems.Add (item);
    }

    /// <summary>
    /// Giảm trừ số lượng của item trong thùng đồ (Dành cho item use hoặc quest)
    /// </summary>
    /// <param name="slotItem">Slot trong danh sách</param>
    /// <param name="itemList">list item truyền vào</param>
    /// <param name="quantityRemove">số lượng giảm đi</param>
    // private bool ReduceItemQuantity (int slotItem, List<ItemModel> itemList, int quantityRemove) {
    //     if (itemList[slotItem].Quantity >= quantityRemove) //Nếu số lượng muốn giảm >= số lượng còn lại trong thùng
    //     {
    //         itemList[slotItem].Quantity -= quantityRemove;
    //         if (itemList[slotItem].Quantity <= 0) {
    //             itemList.RemoveAt (slotItem);
    //         }
    //         if (ItemsFilter[1])
    //             ReSortItems (itemList);
    //         //InventoryTemplate = itemList;
    //         DataUserController.SaveInventory ();
    //         //ButtonRefreshInventory(null);
    //         return true;
    //     }
    //     return false;
    // }

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
    //Xóa mảng item trong thùng đồ
    private void RemoveItems (List<int> slotdestroy) {
        for (int i = slotdestroy.Count - 1; i >= 0; i--) {
            InventoryTemplate.RemoveAt (slotdestroy[i]);
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
            Obj[1].SetActive (false);
            if (ViewingItemType.Equals (null)) //Nếu là kiểu xem item trong inventory
            {
                ReSortItems (DataUserController.Inventory.DBItems); //Resort để thay đổi các chỉ số sau khi nâng cấp
                ActionViewFilterItems ();
            }
            if (ViewingItemType.Equals (0)) //Nếu là kiểu xem item trong chi tiết nhân vật, chưa trang bị
                SetupItemInEquip ();
            if (ViewingItemType.Equals (1)) //Nếu là kiểu xem item trong chi tiết nhân vật, đã trang bị
                SetupItemEquiped ();
        }
        if (type.Equals (1))
            ObjCraft[13].SetActive (false);
        //refresh lại số tiền 
        ItemInforText[50].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
        ItemInforText[51].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
    }
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
            var amounttotal = InventoryTemplate[ItemSlotViewing].ItemPrice; //ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Tổng số tiền
            //Show dialog xác nhận bán item
            GameSystem.ShowConfirmDialog (string.Format (Languages.lang[131], amounttotal));
            //Chờ lệnh confirm
            StartCoroutine (ActionRemoveItems (1));
        }
        End:
            ReSortItems (InventoryTemplate);
        ItemInforText[50].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
        ItemInforText[51].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
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
                if (ViewingItemType.Equals (null)) {
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
                }
                //Xem item trong chi tiết nhân vật, chưa được trang bị
                if (ViewingItemType.Equals (0)) {
                    if (InventoryTemplate[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
                        if (ValidModifyItem (type, InventoryTemplate[ItemSlotViewing].ItemLevel, 1)) //Check điều kiện nâng cấp
                        {
                            InventoryTemplate[ItemSlotViewing].ItemLevel++;
                            InventoryTemplate[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Update lại giá bán
                            DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (InventoryTemplate[ItemSlotViewing].ItemGuidID))].ItemLevel = InventoryTemplate[ItemSlotViewing].ItemLevel;
                            var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();
                            ItemClick (ItemSlotViewing, listTempItemEquip, 0);
                            DataUserController.SaveInventory ();
                            ShowEffectCraftSuccess (1);
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[157])); //Nâng cấp thành công
                        } else
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[158])); //Đã nâng cấp tối đa
                }
                //Xem item trong chi tiết nhân vật, đang trang bị cho hero
                if (ViewingItemType.Equals (1)) {
                    if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
                        if (ValidModifyItem (type, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemLevel, 1)) //Check điều kiện nâng cấp
                        {
                            //InventoryTemplate[ItemSlotViewing].ItemLevel++;
                            DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemLevel++;
                            DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing]); //Update lại giá bán
                            ItemClick (ItemSlotViewing, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1);
                            DataUserController.SaveHeroes ();
                            ShowEffectCraftSuccess (1);
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[157])); //Nâng cấp thành công
                        } else
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[158])); //Đã nâng cấp tối đa
                }
                break;

                #endregion

                #region Nâng phẩm item 

            case 1: //Nâng phẩm item
                //Xem item trong inventory
                if (ViewingItemType.Equals (null)) {
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
                }
                //Xem item trong chi tiết nhân vật, chưa được trang bị
                if (ViewingItemType.Equals (0)) {
                    if (InventoryTemplate[ItemSlotViewing].ItemColor < 6) {
                        if (ValidModifyItem (type, InventoryTemplate[ItemSlotViewing].ItemColor, 1)) //Check điều kiện nâng cấp
                        {
                            InventoryTemplate[ItemSlotViewing].ItemColor++;
                            InventoryTemplate[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]); //Update lại giá bán
                            DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (InventoryTemplate[ItemSlotViewing].ItemGuidID))].ItemColor = InventoryTemplate[ItemSlotViewing].ItemColor;
                            var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();
                            ItemClick (ItemSlotViewing, listTempItemEquip, 0);
                            DataUserController.SaveInventory ();
                            ShowEffectCraftSuccess (1);
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[159])); //Nâng cấp thành công
                        } else
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[160])); //Đã nâng cấp tối đa
                }
                //Xem item trong chi tiết nhân vật, đang trang bị cho hero
                if (ViewingItemType.Equals (1)) {
                    if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemColor < 6) {
                        if (ValidModifyItem (type, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemColor, 1)) //Check điều kiện nâng cấp
                        {
                            DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemColor++;
                            DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemPrice = ItemSystem.GetItemPrice (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing]); //Update lại giá bán
                            ItemClick (ItemSlotViewing, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1);
                            DataUserController.SaveHeroes ();
                            ShowEffectCraftSuccess (1);
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[159])); //Nâng cấp thành công
                        } else
                            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[160])); //Đã nâng cấp tối đa
                }
                break;

                #endregion
            default:
                break;
        }
        Obj[29].SetActive (false);
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyItem (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (ViewingItemType.Equals (null) || ViewingItemType.Equals (0)) {
            if (InventoryTemplate[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
                ValidModifyItem (0, InventoryTemplate[ItemSlotViewing].ItemLevel, 0);
                Obj[29].SetActive (true);
            }
        }
        //Xem item trong chi tiết nhân vật, đang trang bị cho hero
        if (ViewingItemType.Equals (1)) {
            if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemLevel < ItemCoreSetting.ItemLevelMax) {
                ValidModifyItem (0, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemLevel, 0);
                Obj[29].SetActive (true);
            }
        }
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng phẩm item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyItem2 (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (ViewingItemType.Equals (null) || ViewingItemType.Equals (0)) {
            if (InventoryTemplate[ItemSlotViewing].ItemColor < 6) {
                ValidModifyItem (1, InventoryTemplate[ItemSlotViewing].ItemColor, 0);
                Obj[29].SetActive (true);
            }
        }
        //Xem item trong chi tiết nhân vật, đang trang bị cho hero
        if (ViewingItemType.Equals (1)) {
            if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemColor < 6) {
                ValidModifyItem (1, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[ItemSlotViewing].ItemColor, 0);
                Obj[29].SetActive (true);
            }
        }
    }

    /// <summary>
    /// Nhả nút xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonHideRequiredModifyItem (BaseEventData eventData) {
        Obj[29].SetActive (false);
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
        Obj[30].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemInforUpgrade[0] + @"/" + itemInforUpgrade[1]); //Gán hình item
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
            List<int> arrayindex = new List<int> ();
            if (type.Equals (0)) //Bán multi
            {
                for (int i = 0; i < Items.Count; i++) {
                    if (Items[i].transform.GetChild (1).GetComponent<Image> ().gameObject.activeSelf) {
                        arrayindex.Add (i);
                        price += InventoryTemplate[i].ItemPrice; //ItemSystem.GetItemPrice (InventoryTemplate[i]);
                    }
                }
            } else //Bán đơn item
            {
                arrayindex.Add (ItemSlotViewing);
                price += InventoryTemplate[ItemSlotViewing].ItemPrice; //ItemSystem.GetItemPrice (InventoryTemplate[ItemSlotViewing]);
            }
            //SafeValues price_temp = new SafeValues (price);
            //print(DataUserController.User.Golds);
            DataUserController.User.Golds += price; //Cộng vàng sau khi bán
            //print(price_temp);
            //print(DataUserController.User.Golds);
            DataUserController.SaveUserInfor ();
            RemoveCheckItem ();
            //RemoveItems(-1);
            RemoveItems (arrayindex);
            ReSortItems (InventoryTemplate);
            if (type.Equals (0))
                ButtonSelectItems (null);
            ReGenInventoryAfterSell ();
            if (type.Equals (1)) //Trường hợp bán 1 item khi đang xem item đó
            {
                //ReSortItems (InventoryTemplate);
                ReSortItems (DataUserController.Inventory.DBItems);
                var count = InventoryTemplate.Count;
                if (count.Equals (0)) {
                    ButtonHideItemsInformations (0);
                    ButtonRefreshInventory (null);
                } else if (ItemSlotViewing > count - 1) {
                    ItemSlotViewing = count - 1;
                    ItemClick (ItemSlotViewing, InventoryTemplate, null);
                } else {
                    ItemClick (ItemSlotViewing, InventoryTemplate, null);
                }
            }
        }
    }
    //Nút select item
    public void ButtonSelectItems (BaseEventData eventData) {
        if (!Action.Equals (ActionType.EquipToBag)) //Không phải chế độ trang bị item vào bag thì mới cho phép select item
        {
            Action = Action == ActionType.SelectItems ? ActionType.ViewInfor : ActionType.SelectItems;
            if (Action.Equals (ActionType.SelectItems)) {
                ItemInforText[8].text = Languages.lang[67];
                ItemInforText[4].text = string.Format (Languages.lang[131], Languages.lang[137], "", "");
                Obj[4].SetActive (true);
                Obj[3].GetComponent<Image> ().color = new Color (255, 255, 255, 1f);
            } else {
                ItemInforText[8].text = Languages.lang[137];
                ItemInforText[4].text = string.Format (Languages.lang[131], Languages.lang[132], Languages.lang[133], "");
                Obj[4].SetActive (false);
                Obj[3].GetComponent<Image> ().color = new Color (255, 255, 255, 0.5f);
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
        RectTransformButtonItemFilter[0].localPosition = !ItemsFilter[0] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[0].localPosition.y, RectTransformButtonItemFilter[0].localPosition.z) :
            new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[0].localPosition.y, RectTransformButtonItemFilter[0].localPosition.z);
        RectTransformButtonItemFilter[1].localPosition = !ItemsFilter[1] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[1].localPosition.y, RectTransformButtonItemFilter[1].localPosition.z) :
            new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[1].localPosition.y, RectTransformButtonItemFilter[1].localPosition.z);
        RectTransformButtonItemFilter[2].localPosition = !ItemsFilter[2] ? new Vector3 (PositionButtonItemFilter[0] - PositionButtonItemFilter[1], RectTransformButtonItemFilter[2].localPosition.y, RectTransformButtonItemFilter[2].localPosition.z) :
            new Vector3 (PositionButtonItemFilter[0], RectTransformButtonItemFilter[2].localPosition.y, RectTransformButtonItemFilter[2].localPosition.z);
        InventoryTemplate.RemoveRange (0, InventoryTemplate.Count);
        if (ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.ToList ();
        } else if (ItemsFilter[0] && !ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).ToList ();
        } else if (!ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (!ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).ToList ();
        } else if (ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (!ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).ToList ();
        } else if (ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) {
            InventoryTemplate = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) || x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).ToList ();
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
            var itemtquest = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemID).ToList ();
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
        switch (ViewingItemType) {
            case 0: //Xem trong phần trang bị (chưa trang bị)
                var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();

                ItemSlotViewing += type.Equals (0) ? -1 : 1;
                if (ItemSlotViewing > listTempItemEquip.Count - 1)
                    ItemClick (0, listTempItemEquip, 0);
                else if (ItemSlotViewing < 0)
                    ItemClick (listTempItemEquip.Count - 1, listTempItemEquip, 0);
                else ItemClick (ItemSlotViewing, listTempItemEquip, 0);

                break;
            case 1: //Xem item đã trang bị cho hero
                ItemSlotViewing += type.Equals (0) ? -1 : 1;
                if (ItemSlotViewing > DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count - 1)
                    ItemClick (0, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1);
                else if (ItemSlotViewing < 0)
                    ItemClick (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count - 1, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1);
                else ItemClick (ItemSlotViewing, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1);
                break;
            default:
                ItemSlotViewing += type.Equals (0) ? -1 : 1;
                if (ItemSlotViewing > InventoryTemplate.Count - 1)
                    ItemClick (0, InventoryTemplate, null);
                else if (ItemSlotViewing < 0)
                    ItemClick (InventoryTemplate.Count - 1, InventoryTemplate, null);
                else ItemClick (ItemSlotViewing, InventoryTemplate, null);
                break;
        }
    }

    /// <summary>
    /// Gen ra inventory mới sau khi bán item
    /// </summary>
    private void ReGenInventoryAfterSell () {
        if (ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) //Trường hợp đang view all
        {
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems = InventoryTemplate.ToList ();
        } else if (ItemsFilter[0] && !ItemsFilter[1] && !ItemsFilter[2]) //Đang xem mỗi trang bị
        {
            var itemtuse = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).OrderByDescending (x => x.ItemPrice).ToList ();
            var itemtquest = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
            DataUserController.Inventory.DBItems.AddRange (itemtuse);
            DataUserController.Inventory.DBItems.AddRange (itemtquest);
        } else if (!ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) //Đang xem mỗi item use
        {
            var itemtequip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).OrderByDescending (x => x.ItemPrice).ToList ();
            var itemtquest = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (itemtequip);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
            DataUserController.Inventory.DBItems.AddRange (itemtquest);
        } else if (!ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) //Đang xem mỗi item quest
        {
            var itemtequip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).OrderByDescending (x => x.ItemPrice).ToList ();
            var itemtuse = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).OrderBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (itemtequip);
            DataUserController.Inventory.DBItems.AddRange (itemtuse);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
        } else if (ItemsFilter[0] && ItemsFilter[1] && !ItemsFilter[2]) //Xem trang bị và vật phẩm
        {
            var itemtquest = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Quest)).OrderBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
            DataUserController.Inventory.DBItems.AddRange (itemtquest);
        } else if (!ItemsFilter[0] && ItemsFilter[1] && ItemsFilter[2]) //Xem vật phẩm và nhiệm vụ
        {
            var itemtequip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)).OrderByDescending (x => x.ItemPrice).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (itemtequip);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
        } else if (ItemsFilter[0] && !ItemsFilter[1] && ItemsFilter[2]) //Xem trang bị và nhiệm vụ
        {
            var itemtuse = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode.Equals (global::ItemModel.TypeMode.Use)).OrderBy (x => x.ItemID).ToList ();
            DataUserController.Inventory.DBItems.RemoveRange (0, DataUserController.Inventory.DBItems.Count);
            DataUserController.Inventory.DBItems.AddRange (InventoryTemplate);
            DataUserController.Inventory.DBItems.AddRange (itemtuse);
        }
        DataUserController.SaveInventory ();
        ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ
    }

    /// <summary>
    /// Button function lẻ tẻ
    /// </summary>
    /// <param name="type">0: hiển thị UI xem video nhận vàng, gem. 1: refresh vàng và gem</param>
    public void ButtonFunction (int type) {
        switch (type) {
            case 0:
                Obj[31].SetActive (true);
                break;
            case 1:
                ItemInforText[50].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                ItemInforText[51].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            default:
                break;
        }
    }
    #endregion

    #endregion

    #region Craft Functions 

    /// <summary>
    /// Sự kiện chọn dropdown
    /// </summary>
    /// <param name="target"></param>
    private void myDropdownValueChangedHandler (Dropdown target) {
        if (target.value.Equals (0)) {
            ObjCraft[14].SetActive (true);
            ObjCraft[15].SetActive (false);
            ObjCraft[16].SetActive (false);
            ObjCraft[21].SetActive (false);
            ObjCraft[22].SetActive (false);
        }
        if (target.value.Equals (1)) {
            ObjCraft[14].SetActive (false);
            ObjCraft[15].SetActive (false);
            ObjCraft[16].SetActive (true);
            ObjCraft[21].SetActive (false);
            ObjCraft[22].SetActive (false);
        }
        if (target.value.Equals (2)) {
            ObjCraft[14].SetActive (false);
            ObjCraft[15].SetActive (false);
            ObjCraft[16].SetActive (false);
            ObjCraft[21].SetActive (true);
            ObjCraft[22].SetActive (false);
        }
        if (target.value.Equals (4)) {
            ObjCraft[14].SetActive (false);
            ObjCraft[15].SetActive (false);
            ObjCraft[16].SetActive (false);
            ObjCraft[21].SetActive (false);
            ObjCraft[22].SetActive (true);
        }
        if (target.value.Equals (3)) {
            ObjCraft[14].SetActive (false);
            ObjCraft[15].SetActive (true);
            ObjCraft[16].SetActive (false);
            ObjCraft[21].SetActive (false);
            ObjCraft[22].SetActive (false);
        }
    }
    /// <summary>
    /// Khởi tạo ban đầu cho hệ thống craft
    /// </summary>
    private void SetupCraftSystem () {

        #region Setup tọa độ các object item craft để di chuyển hiệu ứng crafting 
        PositionMoveEffectCrafting = new Vector2[7]; //0 = tọa độ object item sẽ được craft
        PositionMoveEffectCrafting[0] = ObjCraft[6].transform.position;
        PositionMoveEffectCrafting[1] = ObjCraft[7].transform.position;
        PositionMoveEffectCrafting[2] = ObjCraft[8].transform.position;
        PositionMoveEffectCrafting[3] = ObjCraft[9].transform.position;
        PositionMoveEffectCrafting[4] = ObjCraft[10].transform.position;
        PositionMoveEffectCrafting[5] = ObjCraft[11].transform.position;
        PositionMoveEffectCrafting[6] = ObjCraft[12].transform.position;
        #endregion

        #region Khởi tạo hiệu ứng crafting 
        EffectCrafting[0] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[1] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[2] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[0].SetActive (false);
        EffectCrafting[1].SetActive (false);
        EffectCrafting[2].SetActive (false);
        #endregion

        #region Khởi tạo dropdown list để lựa chọn loại item chế tạo 

        var dropdown = ObjCraft[20].GetComponent<Dropdown> ();
        dropdown.options.Clear ();
        dropdown.options.Add (new Dropdown.OptionData () { text = Languages.lang[185] });
        dropdown.options.Add (new Dropdown.OptionData () { text = Languages.lang[186] });
        dropdown.options.Add (new Dropdown.OptionData () { text = Languages.lang[187] });
        dropdown.options.Add (new Dropdown.OptionData () { text = Languages.lang[188] });
        dropdown.options.Add (new Dropdown.OptionData () { text = Languages.lang[189] });
        dropdown.onValueChanged.AddListener (delegate {
            myDropdownValueChangedHandler (dropdown);
        });

        #endregion

        CraftTemplate.Initialize ();
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        ItemToCraft = ObjCraft[6].transform.GetChild (0).GetComponent<Image> ();
        ButtonListCraftItemUse = new List<GameObject> ();
        ButtonListCraftItemWeaponPhysic = new List<GameObject> ();
        ButtonListCraftItemWeaponMagic = new List<GameObject> ();
        float regionSpace = 150f; //Khoảng cách giữa các object

        #region Khởi tạo các object cho list item weapon physic 
        var countEquip = CraftTemplate.CraftItemWeaponPhysic.Count;
        ObjCraft[1].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemWeaponPhysic.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemWeaponPhysic[i].transform.SetParent (ObjCraft[1].transform, false);
            ButtonListCraftItemWeaponPhysic[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType0[int.Parse (CraftTemplate.CraftItemWeaponPhysic[i].Split (';') [0].Split (',') [1])];
            ButtonListCraftItemWeaponPhysic[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftTemplate.CraftItemWeaponPhysic[i].Split (';') [0].Split (',') [0] + "/" + CraftTemplate.CraftItemWeaponPhysic[i].Split (';') [0].Split (',') [1]);
            ButtonListCraftItemWeaponPhysic[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (0, temp));
        }
        #endregion

        #region Khởi tạo các object cho list item weapon magic 
        countEquip = CraftTemplate.CraftItemWeaponMagic.Count;
        ObjCraft[2].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemWeaponMagic.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemWeaponMagic[i].transform.SetParent (ObjCraft[2].transform, false);
            switch (CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [0]) {
                case "1":
                    ButtonListCraftItemWeaponMagic[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType1[int.Parse (CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [1])];
                    break;
                case "2":
                    ButtonListCraftItemWeaponMagic[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType2[int.Parse (CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [1])];
                    break;
                case "3":
                    ButtonListCraftItemWeaponMagic[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType3[int.Parse (CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [1])];
                    break;
            }
            ButtonListCraftItemWeaponMagic[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [0] + "/" + CraftTemplate.CraftItemWeaponMagic[i].Split (';') [0].Split (',') [1]);
            ButtonListCraftItemWeaponMagic[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (1, temp));
        }
        #endregion

        #region Khởi tạo các object cho list item Defense 
        countEquip = CraftTemplate.CraftItemDefense.Count;
        ObjCraft[18].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemDefense.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemDefense[i].transform.SetParent (ObjCraft[18].transform, false);
            switch (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [0]) {
                case "4":
                    ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType4[int.Parse (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1])];
                    break;
                case "5":
                    ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType5[int.Parse (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1])];
                    break;
                case "6":
                    ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType6[int.Parse (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1])];
                    break;
                case "7":
                    ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType7[int.Parse (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1])];
                    break;
                case "8":
                    ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType8[int.Parse (CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1])];
                    break;
            }
            ButtonListCraftItemDefense[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [0] + "/" + CraftTemplate.CraftItemDefense[i].Split (';') [0].Split (',') [1]);
            ButtonListCraftItemDefense[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (2, temp));
        }
        #endregion

        // #region Khởi tạo các object cho list item Sets 
        // countEquip = CraftTemplate.CraftItemSets.Count;
        // ObjCraft[19].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        // for (int i = 0; i < countEquip; i++) {
        //     var temp = i;
        //     ButtonListCraftItemSets.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
        //     ButtonListCraftItemSets[i].transform.SetParent (ObjCraft[19].transform, false);
        //     ButtonListCraftItemSets[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemName[int.Parse (CraftTemplate.CraftItemSets[i].Split (';') [0])];
        //     ButtonListCraftItemSets[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftTemplate.CraftItemSets[i].Split (';') [0]);
        //     ButtonListCraftItemSets[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (4, temp));
        // }
        // #endregion

        #region Khởi tạo các object cho list item Use 
        var countUse = CraftTemplate.CraftItemUse.Count;
        ObjCraft[0].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countUse * regionSpace);
        for (int i = 0; i < countUse; i++) {
            var temp = i;
            ButtonListCraftItemUse.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countUse * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemUse[i].transform.SetParent (ObjCraft[0].transform, false);
            ButtonListCraftItemUse[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType10[int.Parse (CraftTemplate.CraftItemUse[i].Split (';') [0].Split (',') [1])];
            ButtonListCraftItemUse[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftTemplate.CraftItemUse[i].Split (';') [0].Split (',') [0] + "/" + CraftTemplate.CraftItemUse[i].Split (';') [0].Split (',') [1]);
            ButtonListCraftItemUse[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (3, temp));
        }
        #endregion
    }

    /// <summary>
    /// Hàm khi click vào 1 item trong danh sách craft
    /// </summary>
    /// <param name="typeItem">0: item equip, 1: item use, 2: item quest</param>
    /// <param name="slotItem">slot trong danh sách item</param>
    private void ClickItemInListCraft (int typeItem, int slotItem) {
        if (GameSystem.ControlActive) {
            List<string> listCraftTemp = null;
            TypeCraftSelected = typeItem;
            SlotItemCraftSelected = slotItem;
            switch (typeItem) {
                case 0: //item equip
                    ObjCraft[17].SetActive (false); //Buton craft all
                    listCraftTemp = CraftTemplate.CraftItemWeaponPhysic;
                    break;
                case 1: //item equip
                    ObjCraft[17].SetActive (false); //Buton craft all
                    listCraftTemp = CraftTemplate.CraftItemWeaponMagic;
                    break;
                case 2: //item equip
                    ObjCraft[17].SetActive (false); //Buton craft all
                    listCraftTemp = CraftTemplate.CraftItemDefense;
                    break;
                case 3: //item equip
                    ObjCraft[17].SetActive (true); //Buton craft all
                    listCraftTemp = CraftTemplate.CraftItemUse;
                    break;
                case 4: //item use
                    ObjCraft[17].SetActive (false); //Buton craft all
                    listCraftTemp = CraftTemplate.CraftItemSets;
                    break;
                default:
                    break;
            }

            var slotQuantity = listCraftTemp[slotItem].Split (';');
            for (int i = 0; i < slotQuantity.Length; i++) //Cắt các khoảng trắng dư thừa nếu có
            {
                slotQuantity[i] = slotQuantity[i].Trim ();
            }
            LevelCraft = Convert.ToInt32 (slotQuantity[1]);
            ItemToCraft.sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[0].Split (',') [0] + "/" + slotQuantity[0].Split (',') [1]);
            ShowUIResourceCraft (slotQuantity.Length - 2);
            ObjCraft[6].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ObjCraft[6].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[0].Split (',') [0]), int.Parse (slotQuantity[0].Split (',') [1])));

            var idItemResource1 = slotQuantity[2].Split (',') [1];
            var idItemResource2 = slotQuantity.Length > 3 ? slotQuantity[3].Split (',') [1] : null;
            var idItemResource3 = slotQuantity.Length > 4 ? slotQuantity[4].Split (',') [1] : null;
            var quantityItemResource1 = slotQuantity[2].Split (',') [2];
            var quantityItemResource2 = slotQuantity.Length > 3 ? slotQuantity[3].Split (',') [2] : null;
            var quantityItemResource3 = slotQuantity.Length > 4 ? slotQuantity[4].Split (',') [2] : null;
            var itemresource1 = DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource1) && x.ItemType == int.Parse (slotQuantity[2].Split (',') [0]));
            var itemresource2 = slotQuantity.Length > 3 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource2) && x.ItemType == int.Parse (slotQuantity[3].Split (',') [0])) : null;
            var itemresource3 = slotQuantity.Length > 4 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource3) && x.ItemType == int.Parse (slotQuantity[4].Split (',') [0])) : null;
            var quantityItemCurrent1 = itemresource1 != null ? itemresource1.Quantity : 0;
            var quantityItemCurrent2 = itemresource2 != null ? itemresource2.Quantity : 0;
            var quantityItemCurrent3 = itemresource3 != null ? itemresource3.Quantity : 0;
            var calculator1 = itemresource1 != null ? quantityItemCurrent1 / int.Parse (quantityItemResource1) : 0;
            var calculator2 = itemresource2 != null ? quantityItemCurrent2 / int.Parse (quantityItemResource2) : 0;
            var calculator3 = itemresource3 != null ? quantityItemCurrent3 / int.Parse (quantityItemResource3) : 0;
            //Craft from 1 item
            if (slotQuantity.Length == 3) {
                ObjCraft[7].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[2].Split (',') [0] + "/" + slotQuantity[2].Split (',') [1]);
                ObjCraft[7].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjCraft[7].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[7].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[2].Split (',') [0]), int.Parse (slotQuantity[2].Split (',') [1])));
                ObjCraft[6].transform.GetChild (1).GetComponent<Text> ().text = calculator1.ToString ();
            }
            //Craft from 2 item
            if (slotQuantity.Length == 4) {
                ObjCraft[8].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[2].Split (',') [0] + "/" + slotQuantity[2].Split (',') [1]);
                ObjCraft[8].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjCraft[8].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[8].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[2].Split (',') [0]), int.Parse (slotQuantity[2].Split (',') [1])));

                ObjCraft[9].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[3].Split (',') [0] + "/" + slotQuantity[3].Split (',') [1]);
                ObjCraft[9].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource2) >= quantityItemCurrent2? "white": "white") + ">" + quantityItemCurrent2.ToString () + "/" + quantityItemResource2 + "</color>";
                ObjCraft[9].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[9].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[3].Split (',') [0]), int.Parse (slotQuantity[3].Split (',') [1])));

                ObjCraft[6].transform.GetChild (1).GetComponent<Text> ().text = calculator1 < calculator2 ? calculator1.ToString () : calculator2.ToString ();
            }
            //Craft from 3 item
            if (slotQuantity.Length == 5) {
                ObjCraft[10].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[2].Split (',') [0] + "/" + slotQuantity[2].Split (',') [1]);
                ObjCraft[10].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjCraft[10].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[10].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[2].Split (',') [0]), int.Parse (slotQuantity[2].Split (',') [1])));

                ObjCraft[11].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[3].Split (',') [0] + "/" + slotQuantity[3].Split (',') [1]);
                ObjCraft[11].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource2) >= quantityItemCurrent2? "white": "white") + ">" + quantityItemCurrent2.ToString () + "/" + quantityItemResource2 + "</color>";
                ObjCraft[11].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[11].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[3].Split (',') [0]), int.Parse (slotQuantity[3].Split (',') [1])));

                ObjCraft[12].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + slotQuantity[4].Split (',') [0] + "/" + slotQuantity[4].Split (',') [1]);
                ObjCraft[12].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource3) >= quantityItemCurrent3? "white": "white") + ">" + quantityItemCurrent3.ToString () + "/" + quantityItemResource3 + "</color>";
                ObjCraft[12].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjCraft[12].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (int.Parse (slotQuantity[4].Split (',') [0]), int.Parse (slotQuantity[4].Split (',') [1])));

                ObjCraft[6].transform.GetChild (1).GetComponent<Text> ().text = (calculator1 < calculator2) ? (calculator1 < calculator3) ? calculator1.ToString () : calculator3.ToString () : calculator2 < calculator3 ? calculator2.ToString () : calculator3.ToString ();
            }
        }
    }

    /// <summary>
    /// Hiển thị thông tin các item khi bấm vào
    /// </summary>
    /// <param name="idItem"></param>
    private void ShowInforItemOnCraft (int idType, int idItem) {
        if (GameSystem.ControlActive) {
            ObjCraft[13].transform.GetChild (3).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + idType.ToString () + "/" + idItem.ToString ());
            ItemInforText[13].text = Languages.lang[196];
            switch (idType) {
                case 0:
                    ItemInforText[14].text = Languages.ItemNameType0[idItem];
                    break;
                case 1:
                    ItemInforText[14].text = Languages.ItemNameType1[idItem];
                    break;
                case 2:
                    ItemInforText[14].text = Languages.ItemNameType2[idItem];
                    break;
                case 3:
                    ItemInforText[14].text = Languages.ItemNameType3[idItem];
                    break;
                case 4:
                    ItemInforText[14].text = Languages.ItemNameType4[idItem];
                    break;
                case 5:
                    ItemInforText[14].text = Languages.ItemNameType5[idItem];
                    break;
                case 6:
                    ItemInforText[14].text = Languages.ItemNameType6[idItem];
                    break;
                case 7:
                    ItemInforText[14].text = Languages.ItemNameType7[idItem];
                    break;
                case 8:
                    ItemInforText[14].text = Languages.ItemNameType8[idItem];
                    break;
                case 10:
                    ItemInforText[14].text = Languages.ItemNameType10[idItem];
                    ItemInforText[13].text = Languages.ItemInforType10[idItem];
                    break;
                case 11:
                    ItemInforText[14].text = Languages.ItemNameType11[idItem];
                    ItemInforText[13].text = Languages.ItemInforType11[idItem];
                    break;
                default:
                    break;
            }
            ObjCraft[13].SetActive (true);
        }
    }

    /// <summary>
    /// Hiển thị giao diện craft 1, 2 hay 3 item
    /// </summary>
    private void ShowUIResourceCraft (int slot) {
        for (int i = 0; i < 3; i++) {
            if (i + 1 == slot)
                ObjCraft[i + 3].SetActive (true);
            else
                ObjCraft[i + 3].SetActive (false);
        }
    }

    /// <summary>
    /// Button thực hiện chế tạo item
    /// 0 = craft đơn lẻ, 1 = craft toàn bộ
    /// </summary>
    public void ButtonCraft (int type) {
        if (GameSystem.ControlActive) {
            if (DataUserController.Inventory.DBItems.Count >= Slot) //Check chỗ trống và show ra thông báo
                GameSystem.ControlFunctions.ShowMessage ((Languages.lang[140]));
            else {
                try {
                    List<string> listCraftTemp = null;
                    global::ItemModel.TypeMode _itemtype = global::ItemModel.TypeMode.Use;
                    switch (TypeCraftSelected) {
                        case 0: //item equip
                            listCraftTemp = CraftTemplate.CraftItemWeaponPhysic;
                            _itemtype = global::ItemModel.TypeMode.Equip;
                            break;
                        case 1: //item use
                            listCraftTemp = CraftTemplate.CraftItemWeaponMagic;
                            _itemtype = global::ItemModel.TypeMode.Equip;
                            break;
                        case 2: //item use
                            listCraftTemp = CraftTemplate.CraftItemDefense;
                            _itemtype = global::ItemModel.TypeMode.Equip;
                            break;
                        case 3: //item use
                            listCraftTemp = CraftTemplate.CraftItemUse;
                            _itemtype = global::ItemModel.TypeMode.Use;
                            break;
                        case 4: //item quest
                            listCraftTemp = CraftTemplate.CraftItemSets;
                            _itemtype = global::ItemModel.TypeMode.Equip;
                            break;
                        default:
                            _itemtype = global::ItemModel.TypeMode.Equip;
                            break;
                    }

                    #region Tính toán craft 

                    var slotQuantity = listCraftTemp[SlotItemCraftSelected].Split (';');
                    var idItemResource1 = slotQuantity[2].Split (',') [1];
                    var idItemResource2 = slotQuantity.Length > 3 ? slotQuantity[3].Split (',') [1] : null;
                    var idItemResource3 = slotQuantity.Length > 4 ? slotQuantity[4].Split (',') [1] : null;
                    var quantityItemResource1 = slotQuantity[2].Split (',') [2];
                    var quantityItemResource2 = slotQuantity.Length > 3 ? slotQuantity[3].Split (',') [2] : null;
                    var quantityItemResource3 = slotQuantity.Length > 4 ? slotQuantity[4].Split (',') [2] : null;
                    var itemresource1 = DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource1) && x.ItemType == int.Parse (slotQuantity[2].Split (',') [0]));
                    var itemresource2 = slotQuantity.Length > 3 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource2) && x.ItemType == int.Parse (slotQuantity[3].Split (',') [0])) : null;
                    var itemresource3 = slotQuantity.Length > 4 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == int.Parse (idItemResource3) && x.ItemType == int.Parse (slotQuantity[4].Split (',') [0])) : null;
                    var quantityItemCurrent1 = itemresource1 != null ? itemresource1.Quantity : 0;
                    var quantityItemCurrent2 = itemresource2 != null ? itemresource2.Quantity : 0;
                    var quantityItemCurrent3 = itemresource3 != null ? itemresource3.Quantity : 0;
                    var calculator1 = itemresource1 != null ? quantityItemCurrent1 / int.Parse (quantityItemResource1) : 0;
                    var calculator2 = itemresource2 != null ? quantityItemCurrent2 / int.Parse (quantityItemResource2) : 0;
                    var calculator3 = itemresource3 != null ? quantityItemCurrent3 / int.Parse (quantityItemResource3) : 0;
                    var quantityMaxCraft = 0;

                    if (slotQuantity.Length == 3) {
                        quantityMaxCraft = calculator1;
                    }
                    //Craft from 2 item
                    if (slotQuantity.Length == 4) {
                        quantityMaxCraft = calculator1 < calculator2 ? calculator1 : calculator2;
                    }
                    //Craft from 3 item
                    if (slotQuantity.Length == 5) {
                        quantityMaxCraft = (calculator1 < calculator2) ? (calculator1 < calculator3) ? calculator1 : calculator3 : calculator2 < calculator3 ? calculator2 : calculator3;
                    }
                    #endregion

                    if (quantityMaxCraft > 0) {
                        StartCoroutine (EffectCraftingMove (Convert.ToSByte (slotQuantity.Length - 2), .6f));
                        #region Trừ item 

                        if (slotQuantity.Length == 3) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? int.Parse (quantityItemResource1) : int.Parse (quantityItemResource1) * quantityMaxCraft);
                        }
                        if (slotQuantity.Length == 4) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? int.Parse (quantityItemResource1) : int.Parse (quantityItemResource1) * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource2.ItemType, itemresource2.ItemID, type.Equals (0) ? int.Parse (quantityItemResource2) : int.Parse (quantityItemResource2) * quantityMaxCraft);
                        }
                        if (slotQuantity.Length == 5) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? int.Parse (quantityItemResource1) : int.Parse (quantityItemResource1) * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource2.ItemType, itemresource2.ItemID, type.Equals (0) ? int.Parse (quantityItemResource2) : int.Parse (quantityItemResource2) * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource3.ItemType, itemresource3.ItemID, type.Equals (0) ? int.Parse (quantityItemResource3) : int.Parse (quantityItemResource3) * quantityMaxCraft);
                        }

                        #endregion
                        //AddItemCraftToInventory (Convert.ToSByte (listCraftTemp[SlotItemCraftSelected].Split (';') [0]), Convert.ToByte (listCraftTemp[SlotItemCraftSelected].Split (';') [1]), quantityMaxCraft);
                        InventorySystem.AddItemToInventory (ItemSystem.CreateItem (false, false, Convert.ToByte (slotQuantity[1]), Convert.ToSByte (slotQuantity[0].Split (',') [0]), Convert.ToByte (slotQuantity[0].Split (',') [1]), quantityMaxCraft));
                        ButtonRefreshInventory (null);
                    } else {
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[143])); //Không đủ nguyên liệu để chế tạo
                    }
                } catch {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[142])); //Lỗi - Không thể chế tạo đồ
                }
            }
            // TypeCraftSelected = typeItem;
            // SlotItemCraftSelected = slotItem;
        }
    }

    /// <summary>
    /// Điều khiển di chuyển của hiệu ứng crafting 
    /// </summary>
    /// <param name="resourceCount">Số lượng tài nguyên cần để craft</param>
    /// <param name="duration">Thời gian để hiệu ứng di chuyển thành công - Cũng là thời gian chờ từ khi click button craft</param>
    /// <returns></returns>
    private IEnumerator EffectCraftingMove (sbyte resourceCount, float duration) {
        GameSystem.ControlActive = false;
        var targetPos = new Vector2 (100, 100);
        float time = 0;
        float rate = 1 / duration;
        Vector2[] startPos = new Vector2[EffectCrafting.Length];
        startPos[0] = EffectCrafting[0].transform.position = resourceCount.Equals (1) ? PositionMoveEffectCrafting[1] : resourceCount.Equals (2) ? PositionMoveEffectCrafting[2] : PositionMoveEffectCrafting[4];
        startPos[1] = EffectCrafting[1].transform.position = resourceCount.Equals (2) ? PositionMoveEffectCrafting[3] : PositionMoveEffectCrafting[5];
        startPos[2] = EffectCrafting[2].transform.position = PositionMoveEffectCrafting[6];
        for (sbyte i = (sbyte) (resourceCount - 1); i >= 0; i--)
            EffectCrafting[i].SetActive (true);
        //Vector2 startPos = ObjHome[17].GetComponent<RectTransform> ().anchoredPosition;
        var pos1 = EffectCrafting[0].GetComponent<Transform> ();
        var pos2 = EffectCrafting[1].GetComponent<Transform> ();
        var pos3 = EffectCrafting[2].GetComponent<Transform> ();
        while (time < 1) {
            time += rate * Time.deltaTime;
            pos1.transform.position = Vector2.Lerp (startPos[0], PositionMoveEffectCrafting[0], moveDrawnCrafting.Evaluate (time));
            pos2.transform.position = Vector2.Lerp (startPos[1], PositionMoveEffectCrafting[0], moveDrawnCrafting.Evaluate (time));
            pos3.transform.position = Vector2.Lerp (startPos[2], PositionMoveEffectCrafting[0], moveDrawnCrafting.Evaluate (time));
            yield return 0;
        }
        EffectCrafting[0].GetComponent<ParticleSystem> ().Stop ();
        EffectCrafting[1].GetComponent<ParticleSystem> ().Stop ();
        EffectCrafting[2].GetComponent<ParticleSystem> ().Stop ();
        ShowEffectCraftSuccess (0);
        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[141])); //Chế tạo thành công
        GameSystem.ControlActive = true;
        //Refresh lại khung craft để update số lượng item
        if (TypeCraftSelected != -1)
            ClickItemInListCraft (TypeCraftSelected, SlotItemCraftSelected);
    }

    /// <summary>
    /// Hiển thị hiệu ứng chế tạo thành công
    /// </summary>
    /// <param name="type">0: craft item, 1: cường hóa hoặc nâng phẩm item</param>
    private void ShowEffectCraftSuccess (int type) {
        var count = EffectCraftSuccess.Count;
        for (int i = 0; i < count; i++) {
            if (!EffectCraftSuccess[i].activeSelf) {
                EffectCraftSuccess[i].SetActive (true);
                EffectCraftSuccess[i].transform.position = new Vector3 ((type.Equals (0) ? ObjCraft[6] : Obj[2]).transform.position.x, (type.Equals (0) ? ObjCraft[6] : Obj[2]).transform.position.y, 0);
                break;
            }
            if (i == count - 1)
                EffectCraftSuccess.Add ((GameObject) Instantiate (EffectCraftSuccess[0], new Vector3 ((type.Equals (0) ? ObjCraft[6] : Obj[2]).transform.position.x, (type.Equals (0) ? ObjCraft[6] : Obj[2]).transform.position.y, 0), Quaternion.identity));

        }
    }

    /// <summary>
    /// Chuyển đổi giữa các danh sách craft
    /// </summary>
    /// <param name="type">0 = equip, 1 = use, 2 = quest</param>
    public void ButtonChangeItemType (int type) {
        if (type.Equals (0)) {
            //ObjCraft[17].SetActive (false); //Buton craft all
            ObjCraft[14].SetActive (true);
            ObjCraft[15].SetActive (false);
            ObjCraft[16].SetActive (false);
        }
        if (type.Equals (1)) {
            //ObjCraft[17].SetActive (true); //Buton craft all
            ObjCraft[14].SetActive (false);
            ObjCraft[15].SetActive (true);
            ObjCraft[16].SetActive (false);
        }
        if (type.Equals (2))
            GameSystem.ControlFunctions.ShowMessage (("Comming Soon..."));
    }
    #endregion

    #region Heroes Functions 

    /// <summary>
    /// Khởi tạo mảng danh sách hero đã sở hữu, và đưa lên UI
    /// </summary>
    private void SetupHeroList () {
        var totalHeroesInList = DataUserController.Heroes.DBHeroes.Count;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 210f; //Khoảng cách giữa các object
        ObjHeroSystem[0].GetComponent<RectTransform> ().sizeDelta = totalHeroesInList % HorizontalQuantity == 0 ? new Vector2 (0, (totalHeroesInList / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((totalHeroesInList / HorizontalQuantity) + 1) * regionSpace);
        float verticalcounttemp = ObjHeroSystem[0].GetComponent<RectTransform> ().sizeDelta.y / 2 - 105;
        //Khởi tạo button inventory
        Hero = new List<GameObject> ();
        HeroAvt = new List<Image> ();
        HeroObject = new List<GameObject> ();
        for (int i = 0; i < totalHeroesInList; i++) {
            var temp = i;
            Hero.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/HeroItemSmall"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 102 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity));
            Hero[i].transform.SetParent (ObjHeroSystem[0].transform, false); //Đẩy prefab vào scroll
            HeroAvt.Add (Hero[i].transform.GetChild (0).GetComponent<Image> ()); //Lấy biến avt
            HeroAvt[i].sprite = Resources.Load<Sprite> ("HeroAvt/" + DataUserController.Heroes.DBHeroes[i].ID.ToString ()); //Set avt theo ID hero
            HeroAvt[i].GetComponent<Button> ().onClick.AddListener (() => ShowHeroInforDetail (temp));
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
            HeroObject.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/HeroAvt/Hero" + DataUserController.Heroes.DBHeroes[i].ID.ToString ()), new Vector3 (-11.5f, -3f, 0), Quaternion.identity));
            HeroObject[i].SetActive (false);
        }
    }

    /// <summary>
    /// Click vào nhân vật và hiển thị chi tiết thông tin nhân vật
    /// </summary>
    private void ShowHeroInforDetail (int slotHero) {
        CaculatorValueHeroes (slotHero);
        HideAllHeroAvt ();
        HeroObject[slotHero].SetActive (true);
        SlotHeroViewing = slotHero;
        SetupItemInEquip ();
        SetupItemEquiped ();
        ObjHeroSystem[1].SetActive (true); //Show UI
        //Text nội tại và kỹ năng
        ItemInforText[49].text = Languages.lang[166] + "\n" + Languages.HeroIntrinsic[DataUserController.Heroes.DBHeroes[slotHero].ID - 1] + "\n\n\n" +
            Languages.lang[167] + "\n" + Languages.HeroSkillDescription[DataUserController.Heroes.DBHeroes[slotHero].ID - 1];
        ObjectEffectSelected.SetActive (false);
    }

    /// <summary>
    /// Ẩn tất cả các hero đứng làm mẫu
    /// </summary>
    private void HideAllHeroAvt () {
        ObjectEffectSelected.SetActive (true);
        var count = HeroObject.Count;
        for (int i = 0; i < count; i++)
            HeroObject[i].SetActive (false);
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

        ItemInforText[28].text = ItemInforText[29].text = DataUserController.Heroes.DBHeroes[slotHero].Name; //Tên hero
        //ItemInforText[30].text = DataUserController.Heroes.DBHeroes[slotHero].HType.Equals (global::HeroesProperties.HeroType.far) ? Languages.lang[17] : Languages.lang[20]; //Class nhân vật
        ItemInforText[31].text = Languages.lang[23] + DataUserController.Heroes.DBHeroes[slotHero].Level.ToString (); //Cấp độ nhân vật
        ObjHeroSystem[20].transform.localScale = new Vector3 (DataUserController.Heroes.DBHeroes[slotHero].EXP / Module.NextExp (DataUserController.Heroes.DBHeroes[slotHero].Level), 1, 1); //Thanh exp
        var heroOriginal = DataUserController.HeroesDefault.DBHeroesDefault.Find (x => x.ID == DataUserController.Heroes.DBHeroes[slotHero].ID);
        ItemInforText[30].text = Languages.lang[(int) heroOriginal.Type + 15]; //Class nhân vật

        ItemInforText[33].text = String.Format ("{0:0.#}", (heroOriginal.vHealth + vHealthPlus + (heroOriginal.vHealthPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
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
    //Khởi tạo các item đã được trang bị cho hero
    private void SetupItemEquiped () {
        var count = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;
        for (int i = 0; i < 6; i++) {
            ObjHeroSystem[i + 8].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/none");
            ObjHeroSystem[i + 8].GetComponent<Button> ().onClick.RemoveAllListeners ();
        }
        for (int i = 0; i < 6; i++) {
            if (i < count) {
                var temp = i;
                ObjHeroSystem[i + 8].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemType + @"/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemID);
                ObjHeroSystem[i + 8].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjHeroSystem[i + 8].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1));
                if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6 (xem trong module phần inventory)
                    ObjHeroSystem[i + 14].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemColor);
                ObjHeroSystem[i + 8].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemLevel.ToString ();

            } else {
                ObjHeroSystem[i + 14].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0");
                ObjHeroSystem[i + 8].transform.GetChild (0).GetComponent<Text> ().text = "";
            }
        }
    }

    /// <summary>
    /// Đưa dữ liệu các item trang bị lên view trong hệ thống trang bị item cho nhân vật
    /// </summary>
    private void SetupItemInEquip () {
        var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();
        var totalItemEquip = listTempItemEquip.Count;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 160f; //Khoảng cách giữa các object
        ObjHeroSystem[7].GetComponent<RectTransform> ().sizeDelta = totalItemEquip % HorizontalQuantityEquip == 0 ? new Vector2 (0, (totalItemEquip / HorizontalQuantityEquip) * regionSpace) : new Vector2 (0, ((totalItemEquip / HorizontalQuantityEquip) + 1) * regionSpace);
        float verticalcounttemp = totalItemEquip <= HorizontalQuantityEquip ? 0 : ((totalItemEquip % HorizontalQuantityEquip).Equals (0) ? (totalItemEquip - 1) / HorizontalQuantityEquip * 80 : (totalItemEquip / HorizontalQuantityEquip) * 80);
        float horizonXOriginal = -400;
        //Thêm mới các object ban đầu nếu có sự chênh lệch số lượng
        if (ListBackgroundItemEquip.Count < totalItemEquip) {
            var count = totalItemEquip - ListBackgroundItemEquip.Count;
            for (int i = 0; i < count; i++) {
                ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (0, 0, 0), Quaternion.identity));
                ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            }
        }
        //Set các object
        for (int i = 0; i < totalItemEquip; i++) {
            var temp = i;
            //ListBackgroundItemEquip[i].transform.parent = null;
            ListBackgroundItemEquip[i].transform.SetParent (ObjHeroSystem[7].transform, false); //Đẩy prefab vào scroll
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (horizonXOriginal + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0);
            //ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 160 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity));
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (150, 150);
            ListBackgroundItemEquip[i].transform.localScale = new Vector3 (1, 1, 1);
            if (listTempItemEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6
                ListBackgroundItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listTempItemEquip[i].ItemColor);
            //ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListItemEquip[i].transform.SetParent (ListBackgroundItemEquip[i].transform, false);
            ListItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listTempItemEquip[i].ItemType + @"/" + listTempItemEquip[i].ItemID);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = "";
            ListItemEquip[i].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ListItemEquip[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, listTempItemEquip, 0));
            ListItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (130, 130);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + listTempItemEquip[i].ItemLevel.ToString ();
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantityEquip == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        //Xóa object nếu dư
        if (ListBackgroundItemEquip.Count > totalItemEquip) {
            for (int i = ListBackgroundItemEquip.Count - 1; i >= totalItemEquip; i--) {
                Destroy (ListBackgroundItemEquip[i]);
                Destroy (ListItemEquip[i]);
                ListBackgroundItemEquip.RemoveAt (i);
                ListItemEquip.RemoveAt (i);
            }
        }
    }

    /// <summary>
    /// Nút thực hiện các chức năng của hệ thống nhân vật
    /// 0: Đóng thông tin chi tiết
    /// 1: Trang bị item
    /// 2: Gỡ bỏ item
    /// 3: Hero trước đó
    /// 4: Hero tiếp theo
    /// 5: Trang bị nhanh
    /// 6: Gỡ nhanh
    /// </summary>
    /// <param name="type"></param>
    public void HeroSystemButtonFunctions (int type) {
        switch (type) {
            case 0: //Đóng thông tin chi tiết
                ObjHeroSystem[1].SetActive (false);
                break;
            case 1: //Trang bị item cho hero
                if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count >= 6) //Check slot full
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[128])); //Thông báo full slot
                else {
                    DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Add (ItemViewing); //Thêm trang bị đang xem vào hero
                    RemoveItems (ItemSlotViewing);
                    DataUserController.SaveInventory ();
                    ButtonRefreshInventory (null);
                    SetupItemInEquip ();
                    SetupItemEquiped ();
                    Obj[1].SetActive (false);
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[129])); //Thông báo trang bị thành công
                    DataUserController.SaveHeroes ();
                }
                break;
            case 2: //Gỡ trang bị
                RemoveItemFromHero (false);
                break;
            case 3: //Prev nhân vật
                ShowHeroInforDetail (SlotHeroViewing <= 0 ? DataUserController.Heroes.DBHeroes.Count - 1 : SlotHeroViewing - 1);
                break;
            case 4: //Next nhân vật
                ShowHeroInforDetail (SlotHeroViewing >= DataUserController.Heroes.DBHeroes.Count - 1 ? 0 : SlotHeroViewing + 1);
                break;
            case 5: //Trang bị nhanh
                GameSystem.ControlFunctions.ShowMessage (("Comming Soon...")); //Thông báo 
                break;
            case 6: //Gỡ trang bị nhanh
                RemoveItemFromHero (true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Gỡ trang bị từ nhân vật
    /// </summary>
    /// <param name="isRemoveAll">false: gỡ 1 item, true: gỡ toàn bộ</param>
    private void RemoveItemFromHero (bool isRemoveAll) {
        var count = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;
        if (count > 0) //Nếu có trang bị thì mới gỡ
        {
            if (isRemoveAll) { //Nếu là chế độ gỡ toàn bộ
                if (DataUserController.User.InventorySlot - DataUserController.Inventory.DBItems.Count >= DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count) {
                    for (int i = 0; i < count; i++) {
                        var itemTemp = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[0];
                        AddItemToInventory (itemTemp);
                        DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.RemoveAt (0);
                    }
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                } else {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                }

            } else //Gỡ 1 item
            {
                if (DataUserController.Inventory.DBItems.Count < DataUserController.User.InventorySlot) {
                    AddItemToInventory (ItemViewing);
                    DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.RemoveAt (ItemSlotViewing);
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                } else {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                }
            }
            ButtonRefreshInventory (null);
            SetupItemInEquip ();
            SetupItemEquiped ();
            Obj[1].SetActive (false);
            DataUserController.SaveInventory ();
            InventoryTemplate = DataUserController.Inventory.DBItems.ToList ();
            ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ
            DataUserController.SaveHeroes ();
        }
    }

    /// <summary>
    /// Các button trong chi tiết nhân vật
    /// </summary>
    /// <param name="type">-1: thoát chi tiết nhân vật, 0: thông tin nhân vật, 1 Trang bị nhân vật, 2 Kỹ năng, 3 Tiểu sử, 4 đặc biệt</param>
    public void DetailHeroButtonFunctions (int type) {
        if (type != -1) {
            ObjHeroSystem[2].SetActive (false);
            ObjHeroSystem[3].SetActive (false);
            ObjHeroSystem[4].SetActive (false);
            ObjHeroSystem[5].SetActive (false);
        }
        switch (type) {
            case -1:
                HideAllHeroAvt ();
                ObjHeroSystem[6].SetActive (false);
                break;
            case 0:
                CaculatorValueHeroes (SlotHeroViewing);
                ObjHeroSystem[2].SetActive (true);
                break;
            case 1:
                ObjHeroSystem[3].SetActive (true);
                break;
            case 2:
                ObjHeroSystem[4].SetActive (true);
                break;
            case 3:
                ObjHeroSystem[5].SetActive (true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chức năng trang bị nhanh cho nhân vật
    /// </summary>
    // private void FastEquip () {
    //     if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count >= 6) //Check slot full
    //         GameSystem.ControlFunctions.ShowMessage( (Languages.lang[128])); //Thông báo full slot
    //     else {
    //         var slotEmpty = 6 - DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;//Lấy số slot trống còn lại
    //         ItemModel[] itemWillEquip = new ItemModel[slotEmpty];//Khởi tạo list item sẽ trang bị
    //         var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();//Lấy danh sách item trang bị trong inventory
    //         //bool isMagic = DataUserController.Heroes.DBHeroes[SlotHeroViewing].vMagic > 0;//Có phải là tướng sử dụng sức mạnh phép thuật hay ko

    //     }

    // }

    #endregion

    /// <summary>
    /// Button chuyển đổi giữa các chức năng
    /// 0 = Heroes, 1 = Inventory, 2 = Craft
    /// </summary>
    public void ButtonChangeFunctions (int typeFunction) {
        if (GameSystem.ControlActive) {
            //Hero function
            if (typeFunction.Equals (0)) {
                Obj[10].SetActive (false);
                Obj[18].SetActive (false);
                Obj[19].SetActive (true);
                ButtonRefreshInventory (null);
                if (Action.Equals (ActionType.SelectItems))
                    ButtonSelectItems (null);
                ObjectEffectSelected.transform.position = Obj[17].transform.position;
            }
            //Inventory function
            if (typeFunction.Equals (1) && !Obj[10].activeSelf) {
                Obj[10].SetActive (true);
                Obj[18].SetActive (false);
                Obj[19].SetActive (false);
                //ButtonRefreshInventory (null);
                ObjectEffectSelected.transform.position = Obj[15].transform.position;
            }
            //Craft function
            if (typeFunction.Equals (2) && !Obj[18].activeSelf) {
                //ButtonRefreshInventory (null);
                if (Action.Equals (ActionType.SelectItems))
                    ButtonSelectItems (null);
                //Refresh lại khung craft để update số lượng item
                if (TypeCraftSelected != -1)
                    ClickItemInListCraft (TypeCraftSelected, SlotItemCraftSelected);
                Obj[10].SetActive (false);
                Obj[19].SetActive (false);
                Obj[18].SetActive (true);
                ObjectEffectSelected.transform.position = Obj[16].transform.position;
            }
        }
    }
    //Test thêm đá cường hóa để test nâng cấp
    public void TestAddItemGemUpgrade () {
        ButtonRefreshInventory (null);
        for (byte i = 0; i < 45; i++) {
            InventorySystem.AddItemToInventory (ItemSystem.CreateItem (false, false, 0, 11, i, 10000));
        }
        for (byte i = 0; i < 27; i++) {
            InventorySystem.AddItemToInventory (ItemSystem.CreateItem (false, false, 0, 10, i, 10000));
        }
        for (byte i = 0; i < 34; i++) {
            InventorySystem.AddItemToInventory (ItemSystem.CreateItem (false, false, 0, 12, i, 10000));
        }
        ButtonRefreshInventory (null);
    }

    public void ButtonCreateRandomItems (int type) {
        ButtonRefreshInventory (null);
        switch (type) {
            case 0: //Item weapon
                //AddItemsRandom (1, global::ItemModel.TypeMode.Equip, (byte) UnityEngine.Random.Range (1, 4));
                InventorySystem.AddItemToInventory (ItemSystem.CreateItem (true, true, (byte) UnityEngine.Random.Range (0, 8), 0, 0, 1));
                GameSystem.ControlFunctions.ShowMessage (("Thêm vũ khí ngẫu nhiên"));
                break;
            case 1: //Item use
                //AddItemsRandom (1, global::ItemModel.TypeMode.Use, (byte) UnityEngine.Random.Range (1, 4));
                GameSystem.ControlFunctions.ShowMessage (("Thêm vật phẩm tiêu dùng ngẫu nhiên"));
                break;
            case 2: //Item quest
                //AddItemsRandom (1, global::ItemModel.TypeMode.Quest, UnityEngine.Random.Range (1, 4));
                // for (int i = 147; i < 193; i++) {
                //     AddItemUserOrQuestToInventory (Module.ListItemActive[i], global::ItemModel.TypeMode.Quest, 1000);
                // }
                DataUserController.Inventory.DBItems.Clear ();
                GameSystem.ControlFunctions.ShowMessage (("Xóa toàn bộ item"));
                break;
            default:
                break;
        }
        DataUserController.User.Gems += 10000;
        DataUserController.SaveUserInfor ();
        //item = DataUserController.Inventory;
        InventoryTemplate = DataUserController.Inventory.DBItems.ToList ();
        ReSortItems (InventoryTemplate);
        DataUserController.SaveInventory ();
        ItemInforText[10].text = DataUserController.Inventory.DBItems.Count.ToString () + "/" + Slot.ToString (); //Count số lượng có trong thùng đồ
    }
}