using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftController : MonoBehaviour {

    #region Variables 

    public GameObject[] ObjectController;
    private List<GameObject> ButtonListCraftItemUse;
    private List<GameObject> ButtonListCraftItemWeaponPhysic;
    private List<GameObject> ButtonListCraftItemWeaponMagic;
    private List<GameObject> ButtonListCraftItemDefense;
    private List<GameObject> ButtonListCraftItemSets;
    private Image ItemToCraft;
    private List<GameObject> EffectCraftSuccess;
    public AnimationCurve moveDrawnCrafting; //Hiệu ứng đường đi của hiệu ứng craft
    private Vector2[] PositionMoveEffectCrafting; //Danh sách tọa độ các object item (0 = tọa độ object item sẽ được craft)
    private GameObject[] EffectCrafting = new GameObject[3]; //Tối đa 1 item được craft từ 3 nguyên liệu khác nhau
    public Text[] TextUI; //Set in interface
    private CraftModel ItemCraftSelected;
    #endregion

    #region Initialize 

    // Start is called before the first frame update
    void Start () {
        SetupCraftSystem ();
        TextUI[2].text = Languages.lang[145]; // = "Chế tạo";
        TextUI[3].text = Languages.lang[147]; // = "Chế tạo all";
        TextUI[7].text = Languages.lang[282]; // "Lựa chọn loại vật phẩm chế tạo";
        TextUI[8].text = Languages.lang[283]; // "Lựa chọn vật phẩm chế tạo";
        TextUI[9].text = Languages.lang[284]; // "Đây là những nguyên liệu cần thiết để chế tạo";
        TextUI[10].text = Languages.lang[285]; // "Với các vật phẩm sử dụng, bạn có thể chế tạo toàn bộ nếu đủ nguyên liệu";
        ButtonFunctions (2); //Update text tiền
    }

    /// <summary>
    /// Sự kiện chọn dropdown
    /// </summary>
    /// <param name="target"></param>
    private void myDropdownValueChangedHandler (Dropdown target) {
        if (target.value.Equals (0)) {
            ObjectController[14].SetActive (true);
            ObjectController[15].SetActive (false);
            ObjectController[16].SetActive (false);
            ObjectController[21].SetActive (false);
            ObjectController[22].SetActive (false);
        }
        if (target.value.Equals (1)) {
            ObjectController[14].SetActive (false);
            ObjectController[15].SetActive (false);
            ObjectController[16].SetActive (true);
            ObjectController[21].SetActive (false);
            ObjectController[22].SetActive (false);
        }
        if (target.value.Equals (2)) {
            ObjectController[14].SetActive (false);
            ObjectController[15].SetActive (false);
            ObjectController[16].SetActive (false);
            ObjectController[21].SetActive (true);
            ObjectController[22].SetActive (false);
        }
        if (target.value.Equals (4)) {
            ObjectController[14].SetActive (false);
            ObjectController[15].SetActive (false);
            ObjectController[16].SetActive (false);
            ObjectController[21].SetActive (false);
            ObjectController[22].SetActive (true);
        }
        if (target.value.Equals (3)) {
            ObjectController[14].SetActive (false);
            ObjectController[15].SetActive (true);
            ObjectController[16].SetActive (false);
            ObjectController[21].SetActive (false);
            ObjectController[22].SetActive (false);
        }
    }
    /// <summary>
    /// Khởi tạo ban đầu cho hệ thống craft
    /// </summary>
    private void SetupCraftSystem () {

        #region Setup tọa độ các object item craft để di chuyển hiệu ứng crafting 
        PositionMoveEffectCrafting = new Vector2[7]; //0 = tọa độ object item sẽ được craft
        PositionMoveEffectCrafting[0] = ObjectController[6].transform.position;
        PositionMoveEffectCrafting[1] = ObjectController[7].transform.position;
        PositionMoveEffectCrafting[2] = ObjectController[8].transform.position;
        PositionMoveEffectCrafting[3] = ObjectController[9].transform.position;
        PositionMoveEffectCrafting[4] = ObjectController[10].transform.position;
        PositionMoveEffectCrafting[5] = ObjectController[11].transform.position;
        PositionMoveEffectCrafting[6] = ObjectController[12].transform.position;
        #endregion

        #region Khởi tạo hiệu ứng crafting 
        EffectCrafting[0] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[1] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[2] = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCrafting"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectCrafting[0].SetActive (false);
        EffectCrafting[1].SetActive (false);
        EffectCrafting[2].SetActive (false);
        EffectCrafting[0].transform.SetParent (ObjectController[24].transform, false);
        EffectCrafting[1].transform.SetParent (ObjectController[24].transform, false);
        EffectCrafting[2].transform.SetParent (ObjectController[24].transform, false);
        #endregion

        #region Khởi tạo dropdown list để lựa chọn loại item chế tạo 

        var dropdown = ObjectController[20].GetComponent<Dropdown> ();
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

        CraftCoreSetting.Initialize ();
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        EffectCraftSuccess[0].transform.SetParent (ObjectController[24].transform, false);
        ItemToCraft = ObjectController[6].transform.GetChild (0).GetComponent<Image> ();
        ButtonListCraftItemUse = new List<GameObject> ();
        ButtonListCraftItemWeaponPhysic = new List<GameObject> ();
        ButtonListCraftItemWeaponMagic = new List<GameObject> ();
        ButtonListCraftItemDefense = new List<GameObject> ();
        float regionSpace = 150f; //Khoảng cách giữa các object

        #region Khởi tạo các object cho list item weapon physic 
        var countEquip = CraftCoreSetting.CraftItemWeaponPhysic.Count;
        ObjectController[1].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemWeaponPhysic.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemWeaponPhysic[i].transform.SetParent (ObjectController[1].transform, false);
            ButtonListCraftItemWeaponPhysic[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemNameType0[CraftCoreSetting.CraftItemWeaponPhysic[i].ItemID];
            ButtonListCraftItemWeaponPhysic[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftCoreSetting.CraftItemWeaponPhysic[i].ItemType + "/" + CraftCoreSetting.CraftItemWeaponPhysic[i].ItemID);
            ButtonListCraftItemWeaponPhysic[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (CraftCoreSetting.CraftItemWeaponPhysic[temp], 0));
        }
        #endregion

        #region Khởi tạo các object cho list item weapon magic 
        countEquip = CraftCoreSetting.CraftItemWeaponMagic.Count;
        ObjectController[2].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemWeaponMagic.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemWeaponMagic[i].transform.SetParent (ObjectController[2].transform, false);
            ButtonListCraftItemWeaponMagic[i].transform.GetChild (0).GetComponent<Text> ().text = ItemSystem.GetItemName ((sbyte) CraftCoreSetting.CraftItemWeaponMagic[i].ItemType, (byte) CraftCoreSetting.CraftItemWeaponMagic[i].ItemID);
            ButtonListCraftItemWeaponMagic[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftCoreSetting.CraftItemWeaponMagic[i].ItemType + "/" + CraftCoreSetting.CraftItemWeaponMagic[i].ItemID);
            ButtonListCraftItemWeaponMagic[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (CraftCoreSetting.CraftItemWeaponMagic[temp], 0));
        }
        #endregion

        #region Khởi tạo các object cho list item Defense 
        countEquip = CraftCoreSetting.CraftItemDefense.Count;
        ObjectController[18].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        for (int i = 0; i < countEquip; i++) {
            var temp = i;
            ButtonListCraftItemDefense.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemDefense[i].transform.SetParent (ObjectController[18].transform, false);
            ButtonListCraftItemDefense[i].transform.GetChild (0).GetComponent<Text> ().text = ItemSystem.GetItemName ((sbyte) CraftCoreSetting.CraftItemDefense[i].ItemType, (byte) CraftCoreSetting.CraftItemDefense[i].ItemID);
            ButtonListCraftItemDefense[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftCoreSetting.CraftItemDefense[i].ItemType + "/" + CraftCoreSetting.CraftItemDefense[i].ItemID);
            ButtonListCraftItemDefense[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (CraftCoreSetting.CraftItemDefense[temp], 0));
        }
        #endregion

        #region Khởi tạo các object cho list item Use 
        var countUse = CraftCoreSetting.CraftItemUse.Count;
        ObjectController[0].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countUse * regionSpace);
        for (int i = 0; i < countUse; i++) {
            var temp = i;
            ButtonListCraftItemUse.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countUse * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
            ButtonListCraftItemUse[i].transform.SetParent (ObjectController[0].transform, false);
            ButtonListCraftItemUse[i].transform.GetChild (0).GetComponent<Text> ().text = ItemSystem.GetItemName ((sbyte) CraftCoreSetting.CraftItemUse[i].ItemType, (byte) CraftCoreSetting.CraftItemUse[i].ItemID);
            ButtonListCraftItemUse[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftCoreSetting.CraftItemUse[i].ItemType + "/" + CraftCoreSetting.CraftItemUse[i].ItemID);
            ButtonListCraftItemUse[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (CraftCoreSetting.CraftItemUse[temp], 1));
        }
        #endregion

        // #region Khởi tạo các object cho list item Sets 
        // countEquip = CraftCoreSetting.CraftItemSets.Count;
        // ObjectController[19].GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, countEquip * regionSpace);
        // for (int i = 0; i < countEquip; i++) {
        //     var temp = i;
        //     ButtonListCraftItemSets.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ButtonListCraftItem"), new Vector3 (270, ((countEquip * regionSpace) / 2) - 75 - regionSpace * i, 0), Quaternion.identity));
        //     ButtonListCraftItemSets[i].transform.SetParent (ObjectController[19].transform, false);
        //     ButtonListCraftItemSets[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.ItemName[int.Parse (CraftCoreSetting.CraftItemSets[i].Split (';') [0])];
        //     ButtonListCraftItemSets[i].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + CraftCoreSetting.CraftItemSets[i].Split (';') [0]);
        //     ButtonListCraftItemSets[i].GetComponent<Button> ().onClick.AddListener (() => ClickItemInListCraft (4, temp));
        // }
        // #endregion
    }

    #endregion

    #region Functions 

    /// <summary>
    /// Hàm khi click vào 1 item trong danh sách craft
    /// </summary>
    /// <param name="itemCraft">item craft</param>
    /// <param name="isShowButtonCraftAll">0 = false, 1 = true, -1 = none</param>
    private void ClickItemInListCraft (CraftModel itemCraft, int isShowButtonCraftAll) {
        if (GameSystem.ControlActive) {

            //Ẩn hiên nút chế tạo tất cả
            if (isShowButtonCraftAll.Equals (0))
                ObjectController[17].SetActive (false); //Buton craft all
            if (isShowButtonCraftAll.Equals (1))
                ObjectController[17].SetActive (true); //Buton craft all

            ItemCraftSelected = itemCraft; //Gán cho biến global của class này
            ItemToCraft.sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemType + "/" + itemCraft.ItemID); //Gán hình ảnh
            TextUI[6].text = itemCraft.MoneyForCraft.ToString ();
            ShowUIResourceCraft (itemCraft.ItemResourceID.Length); //hiển thị giao diện chế tạo
            ObjectController[6].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ObjectController[6].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemType, itemCraft.ItemID));

            //Tạo các chỉ số để tính toán
            var quantityItemResource1 = itemCraft.ItemResourceQuantity[0];
            var quantityItemResource2 = itemCraft.ItemResourceQuantity.Length > 1 ? itemCraft.ItemResourceQuantity[1] : -1;
            var quantityItemResource3 = itemCraft.ItemResourceQuantity.Length > 2 ? itemCraft.ItemResourceQuantity[2] : -1;
            var itemresource1 = DataUserController.Inventory.DBItems.Find (x => x.ItemID == itemCraft.ItemResourceID[0] && x.ItemType == itemCraft.ItemResourceType[0]);
            var itemresource2 = itemCraft.ItemResourceID.Length > 1 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == itemCraft.ItemResourceID[1] && x.ItemType == itemCraft.ItemResourceType[1]) : null;
            var itemresource3 = itemCraft.ItemResourceID.Length > 2 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == itemCraft.ItemResourceID[2] && x.ItemType == itemCraft.ItemResourceType[2]) : null;
            var quantityItemCurrent1 = itemresource1 != null ? itemresource1.Quantity : 0;
            var quantityItemCurrent2 = itemresource2 != null ? itemresource2.Quantity : 0;
            var quantityItemCurrent3 = itemresource3 != null ? itemresource3.Quantity : 0;
            var calculator1 = itemresource1 != null ? quantityItemCurrent1 / quantityItemResource1 : 0;
            var calculator2 = itemresource2 != null ? quantityItemCurrent2 / quantityItemResource2 : 0;
            var calculator3 = itemresource3 != null ? quantityItemCurrent3 / quantityItemResource3 : 0;

            //Craft from 1 item
            if (itemCraft.ItemResourceID.Length == 1) {
                ObjectController[7].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[0] + "/" + itemCraft.ItemResourceID[0]);
                ObjectController[7].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjectController[7].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[7].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[0], itemCraft.ItemResourceID[0]));
                ObjectController[6].transform.GetChild (1).GetComponent<Text> ().text = calculator1.ToString ();
            }

            //Craft from 2 item
            if (itemCraft.ItemResourceID.Length == 2) {
                ObjectController[8].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[0] + "/" + itemCraft.ItemResourceID[0]);
                ObjectController[8].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjectController[8].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[8].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[0], itemCraft.ItemResourceID[0]));

                ObjectController[9].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[1] + "/" + itemCraft.ItemResourceID[1]);
                ObjectController[9].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource2) >= quantityItemCurrent2? "white": "white") + ">" + quantityItemCurrent2.ToString () + "/" + quantityItemResource2 + "</color>";
                ObjectController[9].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[9].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[1], itemCraft.ItemResourceID[1]));

                ObjectController[6].transform.GetChild (1).GetComponent<Text> ().text = calculator1 < calculator2 ? calculator1.ToString () : calculator2.ToString ();
            }

            //Craft from 3 item
            if (itemCraft.ItemResourceID.Length == 3) {
                ObjectController[10].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[0] + "/" + itemCraft.ItemResourceID[0]);
                ObjectController[10].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource1) >= quantityItemCurrent1? "white": "white") + ">" + quantityItemCurrent1.ToString () + "/" + quantityItemResource1 + "</color>";
                ObjectController[10].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[10].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[0], itemCraft.ItemResourceID[0]));

                ObjectController[11].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[1] + "/" + itemCraft.ItemResourceID[1]);
                ObjectController[11].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource2) >= quantityItemCurrent2? "white": "white") + ">" + quantityItemCurrent2.ToString () + "/" + quantityItemResource2 + "</color>";
                ObjectController[11].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[11].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[1], itemCraft.ItemResourceID[1]));

                ObjectController[12].transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemCraft.ItemResourceType[2] + "/" + itemCraft.ItemResourceID[2]);
                ObjectController[12].transform.GetChild (1).GetComponent<Text> ().text = "<color=" + (Convert.ToInt32 (quantityItemResource3) >= quantityItemCurrent3? "white": "white") + ">" + quantityItemCurrent3.ToString () + "/" + quantityItemResource3 + "</color>";
                ObjectController[12].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[12].GetComponent<Button> ().onClick.AddListener (() => ShowInforItemOnCraft (itemCraft.ItemResourceType[2], itemCraft.ItemResourceID[2]));

                ObjectController[6].transform.GetChild (1).GetComponent<Text> ().text = (calculator1 < calculator2) ? (calculator1 < calculator3) ? calculator1.ToString () : calculator3.ToString () : calculator2 < calculator3 ? calculator2.ToString () : calculator3.ToString ();
            }
        }
    }

    /// <summary>
    /// Hiển thị thông tin các item khi bấm vào, hoặc item đã chế tạo được
    /// </summary>
    /// <param name="idItem"></param>
    private void ShowInforItemOnCraft (int itemType, int itemID) {
        if (GameSystem.ControlActive) {
            ObjectController[23].SetActive (true); //Hiển thị nền background chi tiết item
            ObjectController[13].transform.GetChild (2).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemType.ToString () + "/" + itemID.ToString ());
            TextUI[1].text = ItemSystem.GetItemName ((sbyte) itemType, (byte) itemID); //Tên item
            TextUI[0].text = itemType < 10 ? Languages.lang[196] : ItemSystem.GetItemDescription ((sbyte) itemType, (byte) itemID); //Thông tin item
            ObjectController[13].SetActive (true);
        }
    }

    /// <summary>
    /// Hiển thị giao diện craft 1, 2 hay 3 item
    /// </summary>
    private void ShowUIResourceCraft (int slot) {
        for (int i = 0; i < 3; i++) {
            if (i + 1 == slot)
                ObjectController[i + 3].SetActive (true);
            else
                ObjectController[i + 3].SetActive (false);
        }
    }

    /// <summary>
    /// Button thực hiện chế tạo item
    /// 0 = craft đơn lẻ, 1 = craft toàn bộ
    /// </summary>
    public void ButtonCraft (int type) {
        if (GameSystem.ControlActive) {
            if (DataUserController.Inventory.DBItems.Count >= DataUserController.User.InventorySlot) //Check chỗ trống và show ra thông báo
                GameSystem.ControlFunctions.ShowMessage ((Languages.lang[140]));
            else {
                try {
                    #region Tính toán craft 

                    //Tạo các chỉ số để tính toán
                    var quantityItemResource1 = ItemCraftSelected.ItemResourceQuantity[0];
                    var quantityItemResource2 = ItemCraftSelected.ItemResourceQuantity.Length > 1 ? ItemCraftSelected.ItemResourceQuantity[1] : -1;
                    var quantityItemResource3 = ItemCraftSelected.ItemResourceQuantity.Length > 2 ? ItemCraftSelected.ItemResourceQuantity[2] : -1;
                    var itemresource1 = DataUserController.Inventory.DBItems.Find (x => x.ItemID == ItemCraftSelected.ItemResourceID[0] && x.ItemType == ItemCraftSelected.ItemResourceType[0]);
                    var itemresource2 = ItemCraftSelected.ItemResourceID.Length > 1 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == ItemCraftSelected.ItemResourceID[1] && x.ItemType == ItemCraftSelected.ItemResourceType[1]) : null;
                    var itemresource3 = ItemCraftSelected.ItemResourceID.Length > 2 ? DataUserController.Inventory.DBItems.Find (x => x.ItemID == ItemCraftSelected.ItemResourceID[2] && x.ItemType == ItemCraftSelected.ItemResourceType[2]) : null;
                    var quantityItemCurrent1 = itemresource1 != null ? itemresource1.Quantity : 0;
                    var quantityItemCurrent2 = itemresource2 != null ? itemresource2.Quantity : 0;
                    var quantityItemCurrent3 = itemresource3 != null ? itemresource3.Quantity : 0;
                    var calculator1 = itemresource1 != null ? quantityItemCurrent1 / quantityItemResource1 : 0;
                    var calculator2 = itemresource2 != null ? quantityItemCurrent2 / quantityItemResource2 : 0;
                    var calculator3 = itemresource3 != null ? quantityItemCurrent3 / quantityItemResource3 : 0;
                    var quantityMaxCraft = 0;

                    if (ItemCraftSelected.ItemResourceID.Length == 1) {
                        quantityMaxCraft = calculator1;
                    }
                    //Craft from 2 item
                    if (ItemCraftSelected.ItemResourceID.Length == 2) {
                        quantityMaxCraft = calculator1 < calculator2 ? calculator1 : calculator2;
                    }
                    //Craft from 3 item
                    if (ItemCraftSelected.ItemResourceID.Length == 3) {
                        quantityMaxCraft = (calculator1 < calculator2) ? (calculator1 < calculator3) ? calculator1 : calculator3 : calculator2 < calculator3 ? calculator2 : calculator3;
                    }
                    #endregion
                    var MoneyNeedForCraft = type.Equals (0) ? ItemCraftSelected.MoneyForCraft : (ItemCraftSelected.MoneyForCraft * quantityMaxCraft);
                    if (quantityMaxCraft > 0 && UserSystem.CheckGolds (MoneyNeedForCraft)) {
                        StartCoroutine (EffectCraftingMove (ItemCraftSelected.ItemResourceID.Length, .6f));
                        #region Trừ item 

                        if (ItemCraftSelected.ItemResourceID.Length == 1) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? quantityItemResource1 : quantityItemResource1 * quantityMaxCraft);
                        }
                        if (ItemCraftSelected.ItemResourceID.Length == 2) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? quantityItemResource1 : quantityItemResource1 * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource2.ItemType, itemresource2.ItemID, type.Equals (0) ? quantityItemResource2 : quantityItemResource2 * quantityMaxCraft);
                        }
                        if (ItemCraftSelected.ItemResourceID.Length == 3) {
                            InventorySystem.ReduceItemQuantityInventory (itemresource1.ItemType, itemresource1.ItemID, type.Equals (0) ? quantityItemResource1 : quantityItemResource1 * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource2.ItemType, itemresource2.ItemID, type.Equals (0) ? quantityItemResource2 : quantityItemResource2 * quantityMaxCraft);
                            InventorySystem.ReduceItemQuantityInventory (itemresource3.ItemType, itemresource3.ItemID, type.Equals (0) ? quantityItemResource3 : quantityItemResource3 * quantityMaxCraft);
                        }
                        #endregion

                        //Tạo item và gán vào biến tổng để hiển thị
                        GlobalVariables.ItemViewing = ItemSystem.CreateItem (false, false, ItemCraftSelected.LevelCrafted, (sbyte) ItemCraftSelected.ItemType, (byte) ItemCraftSelected.ItemID, type.Equals (0) ? 1 : quantityMaxCraft);
                        GlobalVariables.ItemViewingType = 0;

                        //Đẩy item đã tạo vào inventory
                        InventorySystem.AddItemToInventory (GlobalVariables.ItemViewing);

                        //Trừ tiền
                        UserSystem.DecreaseGolds (MoneyNeedForCraft, true);

                        //Lưu dữ liệu
                        DataUserController.SaveUserInfor ();
                        DataUserController.SaveInventory ();
                        GameSystem.ControlActive = false;
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
    private IEnumerator EffectCraftingMove (int resourceCount, float duration) {
        GameSystem.ControlActive = false;
        var targetPos = new Vector2 (100, 100);
        float time = 0;
        float rate = 1 / duration;
        Vector2[] startPos = new Vector2[EffectCrafting.Length];
        startPos[0] = EffectCrafting[0].transform.position = resourceCount.Equals (1) ? PositionMoveEffectCrafting[1] : resourceCount.Equals (2) ? PositionMoveEffectCrafting[2] : PositionMoveEffectCrafting[4];
        startPos[1] = EffectCrafting[1].transform.position = resourceCount.Equals (2) ? PositionMoveEffectCrafting[3] : PositionMoveEffectCrafting[5];
        startPos[2] = EffectCrafting[2].transform.position = PositionMoveEffectCrafting[6];
        for (int i = (resourceCount - 1); i >= 0; i--)
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
        ShowEffectCraftSuccess (); //Hiệu ứng chế tạo thành công
        GameSystem.ControlFunctions.ShowMessage (Languages.lang[141]); //Chế tạo thành công
        //GameSystem.ControlFunctions.ShowMessage( (Languages.lang[141])); //Chế tạo thành công
        GameSystem.ControlActive = true;

        ButtonFunctions (2); //Refresh lại giá trị tiền tệ

        //Nếu item được tạo ra là item trang bị => show thông tin chi tiết
        if (GlobalVariables.ItemViewing.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
            GameSystem.InitializePrefabUI (6); //Hiển thị thông tin item đã chế tạo được
            StartCoroutine (WaitingCloseItemDetailUI ());
        }

        //Refresh lại khung craft để update số lượng item
        ClickItemInListCraft (ItemCraftSelected, -1);

        GameSystem.ControlActive = true;
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
                case 5: //Phân giải trang bị thành công
                    //GameSystem.ControlFunctions.ShowMessage (GlobalVariables.NotificationText);
                    StartCoroutine (GameSystem.ControlFunctions.ShowMessagecontinuity (GlobalVariables.NotificationText)); //GameSystem.ControlFunctions.ShowMessage( (GlobalVariables.NotificationText));//Thông báo đã nhận dc những gì
                    break;
            }
            ButtonFunctions (2); //Refresh lại giá trị tiền tệ
        }
    }

    /// <summary>
    /// Hiển thị hiệu ứng chế tạo thành công
    /// </summary>
    private void ShowEffectCraftSuccess () {
        var count = EffectCraftSuccess.Count;
        for (int i = 0; i < count; i++) {
            if (!EffectCraftSuccess[i].activeSelf) {
                EffectCraftSuccess[i].SetActive (true);
                EffectCraftSuccess[i].transform.position = new Vector3 (ObjectController[6].transform.position.x, ObjectController[6].transform.position.y, 0);
                break;
            }
            if (i == count - 1)
                EffectCraftSuccess.Add ((GameObject) Instantiate (EffectCraftSuccess[0], new Vector3 (ObjectController[6].transform.position.x, ObjectController[6].transform.position.y, 0), Quaternion.identity));

        }
    }

    /// <summary>
    /// Chuyển đổi giữa các danh sách craft
    /// </summary>
    /// <param name="type">0 = equip, 1 = use, 2 = quest</param>
    public void ButtonChangeItemType (int type) {
        if (GameSystem.ControlActive) {
            if (type.Equals (0)) {
                //ObjectController[17].SetActive (false); //Buton craft all
                ObjectController[14].SetActive (true);
                ObjectController[15].SetActive (false);
                ObjectController[16].SetActive (false);
            }
            if (type.Equals (1)) {
                //ObjectController[17].SetActive (true); //Buton craft all
                ObjectController[14].SetActive (false);
                ObjectController[15].SetActive (true);
                ObjectController[16].SetActive (false);
            }
            if (type.Equals (2))
                GameSystem.ControlFunctions.ShowMessage (("Comming Soon..."));
        }
    }

    /// <summary>
    /// Chức năng các button
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunctions (int type) {
        if (GameSystem.ControlActive) {
            switch (type) {
                case 0: //Đóng UI craft
                    GameSystem.DisposePrefabUI (5);
                    break;
                case 1: //Đóng UI chi tiết item
                    ObjectController[13].SetActive (false);
                    ObjectController[23].SetActive (false);

                    //Refresh lại khung craft để update số lượng item
                    ClickItemInListCraft (ItemCraftSelected, -1);
                    //ItemCrafted = null; //Clear thông tin item đã chế tạo được
                    break;
                case 2: //update lại text tiền tệ
                    TextUI[4].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                    TextUI[5].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                    break;
                case 3: //Mở UI hướng dẫn
                    ObjectController[25].SetActive (true);
                    break;
                case 4: //Đóng UI hướng dẫn
                    ObjectController[25].SetActive (false);
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
}