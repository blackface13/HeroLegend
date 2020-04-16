using UnityEngine;
/// <summary>
/// Model tỉ lệ phân giải item (Đang tạm chưa dùng)
/// </summary>
public class ItemBreakModel {
    public int ItemType;
    public int ItemID;
    public int BreakRate;
    Random RangeQuantity;
    public ItemBreakModel () { }
    public ItemBreakModel Clone () {
        return (ItemBreakModel) this.MemberwiseClone ();
    }
    public ItemBreakModel (int itemType, int itemID, int breakRate, Random rangeQuantity) {
        ItemType = itemType;
        ItemID = itemID;
        BreakRate = breakRate;
        RangeQuantity = rangeQuantity;
    }
}