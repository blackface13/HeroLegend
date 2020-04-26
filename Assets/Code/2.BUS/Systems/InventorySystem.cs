using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Class dành để thực thi các chức năng được gọi từ các class khác class Inventory
/// </summary>
public static class InventorySystem {

    public static readonly int SlotInventoryDefault = 50;//Slot inventory khi khởi tạo acc mới
    public static readonly int SlotInventoryFirstPrice = 10;//
    public static readonly bool IsUpGemWhenUpSlot = true;//Giá trị sau mỗi lần + 1 ô có tăng lên hay không
    public static readonly int PricePerSlot = 10;//Giá trị tăng lên sau mỗi lần +1 ô

    /// <summary>
    /// Thêm item use hoặc quest vào thùng đồ, không check vượt slot
    /// </summary>
    /// <param name="itemID">ID item</param>
    /// <param name="quantity">Số lượng cần thêm</param>
    /// <param name="item">truyền vào null nếu như item muốn thêm không phải là item trang bị</param>
    public static void AddItemToInventory (ItemModel item) {
        // DataUserController.LoadInventory ();
        // DataUserController.LoadUserInfor ();
        var countItemInventory = DataUserController.Inventory.DBItems.Count; //Biến tạm tổng số slot của inventory
        if (countItemInventory < DataUserController.User.InventorySlot) { //Nếu còn slot trống
            if (countItemInventory > 0) {
                if (!item.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) { //Nếu loại item ko phải equip
                    var itemExistIndex = DataUserController.Inventory.DBItems.FindIndex (x => x.ItemType == item.ItemType && x.ItemID == item.ItemID); //= -1 Nếu chưa có item trong inventory
                    if (itemExistIndex != -1) //Nếu item thêm vào đã có trong inventory
                        DataUserController.Inventory.DBItems[itemExistIndex].Quantity += item.Quantity; //Cộng thêm số lượng
                    else
                        DataUserController.Inventory.DBItems.Add (item); //Thêm chính item truyển vào
                } else { //Nếu là item trang bị
                    DataUserController.Inventory.DBItems.Add (item); //Thêm chính item truyển vào
                }
            } else { //Thêm vào luôn khi inventory trống không
                DataUserController.Inventory.DBItems.Add (item);
            }
            // DataUserController.SaveInventory ();
            // DataUserController.SaveUserInfor ();
        }
    }

    /// <summary>
    /// Giảm số lượng item trong inventory
    /// </summary>
    /// <param name="itemID">ID item</param>
    /// <param name="quantity">Số lượng giảm trừ</param>
    public static void ReduceItemQuantityInventory (sbyte itemType, byte itemID, int quantity) {
        //DataUserController.LoadInventory ();
        var itemFinding = DataUserController.Inventory.DBItems.Find (x => x.ItemID == itemID && x.ItemType == itemType); //Gán item cần tìm
        if (itemFinding != null) { //Nếu tìm thấy item đó trong thùng đồ thì mới thực hiện tiếp
            if (itemFinding.Quantity >= quantity) //Trừ nếu đủ số lượng
                itemFinding.Quantity -= quantity;
            if (itemFinding.Quantity <= 0) { //Nếu trừ hết, thì remove nó khỏi list
                DataUserController.Inventory.DBItems.RemoveAt (DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID == itemFinding.ItemGuidID));
                GlobalVariables.IsReduceItemToEmpty = true; //Giảm trừ item trong inventory mà item đó = 0
            }
        }
        //DataUserController.SaveInventory ();
    }

    /// <summary>
    /// Xóa item khỏi thùng đồ
    /// </summary>
    public static void RemoveItem (ItemModel item) {
        try {
            DataUserController.Inventory.DBItems.RemoveAt (DataUserController.Inventory.DBItems.FindIndex (x => x.ItemGuidID == item.ItemGuidID));
        } catch { }
    }

    /// <summary>
    /// Trả về loại item gì theo ItemID
    /// </summary>
    /// <param name="itemID">ID có thể xem trong thư mục Items</param>
    /// <returns></returns>
    private static global::Items.ItemType GetItemTypeByItemID (int itemID) {
        if (itemID >= 1 && itemID <= 199) //Item trang bị
            return global::Items.ItemType.equip;
        if (itemID >= 200 && itemID <= 299) //Item sử dụng
            return global::Items.ItemType.use;
        if (itemID >= 300 && itemID <= 499) //Item quest
            return global::Items.ItemType.quest;
        return global::Items.ItemType.quest;
    }

    /// <summary>
    /// Update trang bị
    /// </summary>
    /// <returns></returns>
    public static bool UpdateItemEquip (ItemModel item) {
        var count = DataUserController.Inventory.DBItems.Count;
        //DataUserController.Inventory.DBItems.Select (x => { x = item; return true; });//Hàm update cũ, code nhanh hơn nhưng hiệu năng kém hơn
        for (int i = 0; i < count; i++) { //Hàm update mới, code lâu hơn nhưng hiệu năng ok hơn
            if (DataUserController.Inventory.DBItems[i].ItemGuidID.Equals (item.ItemGuidID)) {
                DataUserController.Inventory.DBItems[i] = item;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Trả về số lượng của item 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static int GetQuantityItem (int itemType, int itemID) {
        var item = DataUserController.Inventory.DBItems.Find (x => x.ItemType == itemType && x.ItemID == itemID);
        return item != null? item.Quantity : 0;
    }

    /// <summary>
    /// Kiểm tra số lượng item
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="itemID"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static bool CheckQuantityItem (int itemType, int itemID, int quantity) {
        var item = DataUserController.Inventory.DBItems.Find (x => x.ItemType == itemType && x.ItemID == itemID);

        //Trả về false nếu ko có item
        if (item == null)
            return false;

        if (item.Quantity >= quantity)
            return true;

        return false;
    }

    /// <summary>
    /// Tính giá gem khi mua slot inventory
    /// </summary>
    public static double GetPriceBuySlotInventory()
    {
        return (DataUserController.User.InventorySlot - SlotInventoryDefault) * (IsUpGemWhenUpSlot ? PricePerSlot : 0) + SlotInventoryFirstPrice;
    }
}