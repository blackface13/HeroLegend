public static class SocketCoreSetting {

    #region Variables 

    public static int QuantityCreateSocket = 1; //Số lượng khuôn cần thiết mỗi lần tạo socket
    static bool CreateSocketQuantityPlus = true; //Số lượng có tăng dần mỗi lần tạo socket hay ko
    static int CreateSocketQuantity = 2; //Số lượng tăng dần mỗi lần tạo socket

    #endregion

    /// <summary>
    /// Trả về số gems cần thiết để đục lỗ
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static float GetGemsRequiredCreateSocket (ItemModel item) {
        return item != null?(item.vSocketSlot + 1) * 100 : 0;
    }

    /// <summary>
    /// Trả về số gold cần thiết để khảm socket
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static float GetGoldsRequiredInsertSocket (ItemModel item) {
        return item != null?(int) (ItemSystem.GetItemPrice (item) * 1.5f) : 0;
    }

    /// <summary>
    /// Trả về số lượng khuôn cần thiết để đục lỗ
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static int GetQuantityItemCreateSocket (ItemModel item) {
        return QuantityCreateSocket + (item.vSocketSlot) * (CreateSocketQuantityPlus?CreateSocketQuantity : 0);
    }

}