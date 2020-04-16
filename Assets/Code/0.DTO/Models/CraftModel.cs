public class CraftModel {
    public int ItemType; //Loại item sẽ dc tạo
    public int ItemID; //ID item sẽ dc tạo
    public int LevelCrafted; //Cấp độ chế tạo của đồ, càng cao thì đồ càng mạnh
    public int MoneyForCraft; //Số tiền cần thiết để tạo
    public int[] ItemResourceType; //List loại item cần thiết
    public int[] ItemResourceID; //List id item cần thiết
    public int[] ItemResourceQuantity; //List số lượng item cần thiết

    public CraftModel () { }
    public CraftModel Clone () {
        return (CraftModel) this.MemberwiseClone ();
    }
    public CraftModel (int itemType, int itemID, int levelCrafted, int moneyForCraft, int[] itemResourceType, int[] itemResourceID, int[] itemResourceQuantity) {
        ItemType = itemType;
        ItemID = itemID;
        LevelCrafted = levelCrafted;
        MoneyForCraft = moneyForCraft;
        ItemResourceType = itemResourceType;
        ItemResourceID = itemResourceID;
        ItemResourceQuantity = itemResourceQuantity;
    }
}