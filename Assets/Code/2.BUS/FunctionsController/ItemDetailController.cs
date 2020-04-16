using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetailController : MonoBehaviour {

    public GameObject[] ObjectController;
    private List<GameObject> EffectCraftSuccess;

    public Text[] TextUI;

    // Start is called before the first frame update
    void Start () {
        CraftCoreSetting.Initialize ();
        GlobalVariables.ItemDetailAction = -1; //Chờ thực hiện thao tác
        //Đóng UI luôn nếu gặp lỗi itemviewing rỗng
        if (GlobalVariables.ItemViewing == null)
            ButtonFunctions (0); //

        //Setup hiệu ứng craft thành công
        EffectCraftSuccess = new List<GameObject> ();
        EffectCraftSuccess.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/EffectCraft"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
        EffectCraftSuccess[0].transform.SetParent (ObjectController[11].transform, false);

        SetupText ();
        ValidViewType ();
        ShowDetailItem ();
    }

    /// <summary>
    /// Check kiểu xem để config cho phù hợp
    /// </summary>
    private void ValidViewType () {

        //Ẩn 3 button nâng cấp, nâng phẩm, phân giải với item ko phải tran bị
        if (GlobalVariables.ItemViewing.ItemTypeMode != global::ItemModel.TypeMode.Equip) {
            ObjectController[6].SetActive (false); //Button uplevel
            ObjectController[7].SetActive (false); //Button up color
            ObjectController[8].SetActive (false); //Button disassemble
            ObjectController[12].SetActive (false); //Button select
        } else {
            ObjectController[6].SetActive (true); //Button uplevel
            ObjectController[7].SetActive (true); //Button up color
            ObjectController[8].SetActive (true); //Button disassemble
            ObjectController[12].SetActive (false); //Button select
        }

        //Tùy chọn hiển thị
        switch (GlobalVariables.ItemViewingType) {
            case -1: //Chỉ xem, không cho thao tác
                ObjectController[3].SetActive (false); //Button sell
                ObjectController[4].SetActive (false); //Button equip
                ObjectController[5].SetActive (false); //Button remove
                ObjectController[6].SetActive (false); //Button uplevel
                ObjectController[7].SetActive (false); //Button up color
                ObjectController[8].SetActive (false); //Button disassemble
                ObjectController[12].SetActive (false); //Button select
                break;
            case 0: //Normal (ko trang bị, show nút bán)
                ObjectController[3].SetActive (true); //Button sell
                ObjectController[4].SetActive (false); //Button equip
                ObjectController[5].SetActive (false); //Button remove
                ObjectController[12].SetActive (false); //Button select
                break;
            case 1: //Trang bị
                ObjectController[3].SetActive (false); //Button sell
                ObjectController[4].SetActive (true); //Button equip
                ObjectController[5].SetActive (false); //Button remove
                ObjectController[12].SetActive (false); //Button select
                break;
            case 2: //Gỡ trang bị
                ObjectController[3].SetActive (false); //Button sell
                ObjectController[4].SetActive (false); //Button equip
                ObjectController[5].SetActive (true); //Button remove
                ObjectController[12].SetActive (false); //Button select
                break;
            case 3: //Select
                ObjectController[3].SetActive (false); //Button sell
                ObjectController[4].SetActive (false); //Button equip
                ObjectController[5].SetActive (false); //Button remove
                ObjectController[6].SetActive (false); //Button uplevel
                ObjectController[7].SetActive (false); //Button up color
                ObjectController[8].SetActive (false); //Button disassemble
                ObjectController[12].SetActive (true); //Button select
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Gán text cho UI
    /// </summary>
    private void SetupText () {
        TextUI[4].text = Languages.lang[138]; // = "Bán";
        TextUI[5].text = Languages.lang[134]; // = "Trang bị";
        TextUI[6].text = Languages.lang[48]; // = "Gỡ bỏ";
        TextUI[7].text = Languages.lang[154]; // = "Nâng cấp";
        TextUI[8].text = Languages.lang[155]; // = "Nâng phẩm";
        TextUI[9].text = Languages.lang[156]; // = "Phân giải";
        TextUI[11].text = Languages.lang[148]; // = "Yêu cầu";
        TextUI[12].text = Languages.lang[137]; // = "Lựa chọn";
    }

    /// <summary>
    /// Setup các thành phần chi tiết item
    /// </summary>
    private void ShowDetailItem () {
        //Gán hình ảnh viền item và hình item
        ObjectController[0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + GlobalVariables.ItemViewing.ItemColor.ToString ());
        ObjectController[1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + GlobalVariables.ItemViewing.ItemType + @"/" + GlobalVariables.ItemViewing.ItemID);

        //Các text liên quan tới item
        TextUI[0].text = ItemSystem.GetItemName (GlobalVariables.ItemViewing.ItemType, GlobalVariables.ItemViewing.ItemID); //Tên item
        TextUI[1].text = GlobalVariables.ItemViewing.ItemTypeMode != global::ItemModel.TypeMode.Equip ? Languages.lang[75] + GlobalVariables.ItemViewing.Quantity : Languages.lang[23] + GlobalVariables.ItemViewing.ItemLevel; //Số lượng hoặc level
        TextUI[2].text = Languages.lang[76] + ItemSystem.GetItemPrice (GlobalVariables.ItemViewing); //Giá bán
        TextUI[3].text = ItemSystem.GetItemDescription (GlobalVariables.ItemViewing); //Thông tin item

        //Resize lại thông tin item
        var lineCount = TextUI[3].text.Split ('\n');
        var sizeHeight = 68f * lineCount.Length;
        ObjectController[2].GetComponent<RectTransform> ().sizeDelta = new Vector2 (TextUI[3].GetComponent<RectTransform> ().sizeDelta.x, sizeHeight > 760f ? sizeHeight : 760f); //760 là fix cứng khi thiết kế
    }

    /// <summary>
    /// Chức năng các functions
    /// </summary>
    public void ButtonFunctions (int type) {
        switch (type) {
            case -1: //Xóa thông tin các biến
                GlobalVariables.ItemViewing = null;
                break;
            case 0: //Đóng UI chi tiết item
                GlobalVariables.ItemDetailAction = -2;
                ButtonFunctions (-1);
                GameSystem.DisposePrefabUI (6);
                break;
            case 1: //Bán item
                //Show dialog xác nhận bán item
                GameSystem.ShowConfirmDialog (string.Format (Languages.lang[131], GlobalVariables.ItemViewing.ItemPrice));
                StartCoroutine (WaitingForActions (0));
                break;
            case 2: //Trang bị item
                GlobalVariables.ItemDetailAction = 1;
                GameSystem.DisposePrefabUI (6); //Đóng UI
                break;
            case 3: //Gỡ trang bị
                GlobalVariables.ItemDetailAction = 2;
                GameSystem.DisposePrefabUI (6); //Đóng UI
                break;
            case 4: //Nâng cấp
                GlobalVariables.ItemDetailAction = 3;

                //Check điều kiện
                if (GlobalVariables.ItemViewing.ItemLevel < ItemCoreSetting.ItemLevelMax) {
                    if (ValidModifyItem (0, GlobalVariables.ItemViewing.ItemLevel, 1)) //Check điều kiện nâng cấp
                    {
                        GlobalVariables.ItemViewing.ItemLevel++; //Tăng level
                        GlobalVariables.ItemViewing.ItemPrice = ItemSystem.GetItemPrice (GlobalVariables.ItemViewing); //Update lại giá bán
                        //DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (GlobalVariables.ItemViewing.ItemGuidID))].ItemLevel = GlobalVariables.ItemViewing.ItemLevel;
                        ShowDetailItem ();
                        ShowEffectCraftSuccess ();
                        DataUserController.SaveInventory ();
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[157])); //Nâng cấp thành công
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                } else
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[158])); //Đã nâng cấp tối đa

                //Ẩn thông tin required nâng cấp
                ButtonHideRequiredModifyItem (null);

                //GameSystem.DisposePrefabUI (6); //Đóng UI
                break;
            case 5: //Nâng phẩm
                GlobalVariables.ItemDetailAction = 4;

                if (GlobalVariables.ItemViewing.ItemColor < 6) {
                    if (ValidModifyItem (1, GlobalVariables.ItemViewing.ItemColor, 1)) //Check điều kiện nâng cấp
                    {
                        GlobalVariables.ItemViewing.ItemColor++;
                        GlobalVariables.ItemViewing.ItemPrice = ItemSystem.GetItemPrice (GlobalVariables.ItemViewing); //Update lại giá bán
                        //DataUserController.Inventory.DBItems[DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID.Equals (GlobalVariables.ItemViewing.ItemGuidID))].ItemColor = GlobalVariables.ItemViewing.ItemColor;
                        ShowDetailItem ();
                        ShowEffectCraftSuccess ();
                        DataUserController.SaveInventory ();
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[159])); //Nâng cấp thành công
                    } else
                        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[149])); //Không đủ tài nguyên
                } else
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[160])); //Đã nâng cấp tối đa

                //Ẩn thông tin required nâng cấp
                ButtonHideRequiredModifyItem (null);
                //GameSystem.DisposePrefabUI (6); //Đóng UI
                break;
            case 6: //Phân giải chỉ đối với item trang bị
                if (DataUserController.User.InventorySlot - DataUserController.Inventory.DBItems.Count < 3) {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[140])); // "Không đủ chỗ trống trong thùng đồ";
                    break;
                }

                //Show dialog xác nhận phân giải item
                GameSystem.ShowConfirmDialog (Languages.lang[278]);
                StartCoroutine (WaitingForActions (1));
                break;
            case 7: //Chọn item này (custom)
                GlobalVariables.ItemDetailAction = 6; //Selected
                GameSystem.DisposePrefabUI (6); //Đóng UI
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
            case 0: //Bán item
                //Chờ thao tác từ use
                yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);

                //Xác nhận đồng ý
                if (GameSystem.ConfirmBoxResult == 1) {
                    UserSystem.IncreaseGolds (GlobalVariables.ItemViewing.ItemPrice); //Cộng tiền
                    GameSystem.ControlFunctions.ShowMessage (Languages.lang[275] + GlobalVariables.ItemViewing.ItemPrice + Languages.lang[276]); // "Nhận dc ... vàng";
                    InventorySystem.RemoveItem (GlobalVariables.ItemViewing); //Xóa item khỏi inventory
                    DataUserController.SaveUserInfor ();
                    DataUserController.SaveInventory ();
                    ButtonFunctions (-1);
                    GlobalVariables.ItemDetailAction = 0;
                    GameSystem.DisposePrefabUI (6); //Đóng UI
                }
                break;
            case 1: //Phân giải item
                //Chờ thao tác từ use
                yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);

                //Xác nhận đồng ý
                if (GameSystem.ConfirmBoxResult == 1) {
                    //Kiểm tra item là trang bị
                    if (GlobalVariables.ItemViewing.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
                        if (ItemSystem.BreakItem (GlobalVariables.ItemViewing)) //Nếu phân giải thành công
                        {
                            GlobalVariables.ItemDetailAction = 5;
                            DataUserController.SaveInventory ();
                            GameSystem.DisposePrefabUI (6); //Đóng UI
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

    /// <summary>
    /// Check xem đủ điều kiện nâng cấp item hay không
    /// </summary>
    /// <param name="typeModify">Kiểu nâng cấp: 0: nâng cấp, 1: nâng phẩm</param>
    /// <param name="currentLevel">Level hiện tại hoặc màu sắc hiện tại</param>
    /// <param name="typeCheck">0: show thông tin, 1: check để trừ item</param>
    private bool ValidModifyItem (int typeModify, int currentLevel, int typeCheck) {
        var slotItemRequerid = -1;

        //Tìm id có trong list
        var count = DataUserController.Inventory.DBItems.Count;
        var itemInforUpgrade = typeModify.Equals (0) ? ItemCoreSetting.ItemUpgradeLevel[currentLevel].Split (';') [0].Split (',') : ItemCoreSetting.ItemUpgradeColor[currentLevel].Split (';') [0].Split (','); //Lấy thông tin đá cường hóa cho vào mảng, sau này sẽ update nhiều loại sau
        var itemForUpgrade = DataUserController.Inventory.DBItems.Find (x => x.ItemType == Convert.ToSByte (itemInforUpgrade[0]) && x.ItemID == Convert.ToByte (itemInforUpgrade[1])); //Lấy thông tin đá cường hóa nếu có trong inventory
        if (itemForUpgrade != null) //Nếu tồn tại item cần để nâng cấp trong inventory => lấy index của nó
            slotItemRequerid = DataUserController.Inventory.DBItems.FindIndex (x => x.Equals (itemForUpgrade));

        ObjectController[9].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + itemInforUpgrade[0] + @"/" + itemInforUpgrade[1]); //Gán hình item
        TextUI[10].text = (itemForUpgrade != null?itemForUpgrade.Quantity.ToString (): "0") + "/" + itemInforUpgrade[2]; //Gán text số lượng item cần thiết/hiện có

        //Nếu có item trong danh sách thùng đồ thì mới xử lý tiếp
        if (itemForUpgrade != null) {
            if (itemForUpgrade.Quantity >= Convert.ToInt32 (itemInforUpgrade[2])) //Check đủ số lượng
            {
                if (typeCheck.Equals (1)) //Nếu kiểu check để thực hiện nâng cấp, thì mới trừ item
                {
                    InventorySystem.ReduceItemQuantityInventory (Convert.ToSByte (itemInforUpgrade[0]), Convert.ToByte (itemInforUpgrade[1]), Convert.ToInt32 (itemInforUpgrade[2]));
                    DataUserController.SaveInventory ();
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyUpgradeLevel (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (GlobalVariables.ItemViewing.ItemLevel < ItemCoreSetting.ItemLevelMax) {
            ValidModifyItem (0, GlobalVariables.ItemViewing.ItemLevel, 0);
            ObjectController[10].SetActive (true);
        }
    }

    /// <summary>
    /// Nhấn giữ để xem yêu cầu nâng phẩm item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonShowRequiredModifyUpgradeColor (BaseEventData eventData) {
        //Xem item trong chi tiết nhân vật, chưa được trang bị, hoặc xem trong inventory
        if (GlobalVariables.ItemViewing.ItemColor < ItemCoreSetting.ItemLevelMax - 1) {
            ValidModifyItem (1, GlobalVariables.ItemViewing.ItemColor, 0);
            ObjectController[10].SetActive (true);
        }
    }

    /// <summary>
    /// Nhả nút xem yêu cầu nâng cấp item
    /// </summary>
    /// <param name="eventData"></param>
    public void ButtonHideRequiredModifyItem (BaseEventData eventData) {
        ObjectController[10].SetActive (false);
    }

    /// <summary>
    /// Hiển thị hiệu ứng chế tạo thành công
    /// </summary>
    private void ShowEffectCraftSuccess () {
        var count = EffectCraftSuccess.Count;
        for (int i = 0; i < count; i++) {
            if (!EffectCraftSuccess[i].activeSelf) {
                EffectCraftSuccess[i].SetActive (true);
                EffectCraftSuccess[i].transform.position = new Vector3 (ObjectController[1].transform.position.x, ObjectController[1].transform.position.y, 0);
                break;
            }
            if (i == count - 1) {
                EffectCraftSuccess.Add ((GameObject) Instantiate (EffectCraftSuccess[0], new Vector3 (ObjectController[1].transform.position.x, ObjectController[1].transform.position.y, 0), Quaternion.identity));
                EffectCraftSuccess[EffectCraftSuccess.Count - 1].transform.SetParent (ObjectController[11].transform, false);
                EffectCraftSuccess[EffectCraftSuccess.Count - 1].transform.position = new Vector3 (ObjectController[1].transform.position.x, ObjectController[1].transform.position.y, 0);
            }
        }
    }
}