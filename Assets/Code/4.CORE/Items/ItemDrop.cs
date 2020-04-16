using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Cài đặt tỉ lệ rơi đồ của các item
//Các item rơi ra không thuộc loại item trang bị, item trang bị chỉ có thể mở rương hoặc chế tạo thì mới có
public class ItemDrop {
    public sbyte ItemType; //Loại item
    public byte ItemID; //ID của item
    public sbyte DropOnMap; //Rơi tại vùng map nào
    public short Ratio; //Tỉ lệ rơi ra là bao nhiêu. 1% = 100. 100% = 10000
    public ItemDrop (sbyte itemType, byte itemID, sbyte dropOnMap, short ratio) {
        ItemType = itemType;
        ItemID = itemID;
        DropOnMap = dropOnMap;
        Ratio = ratio;
    }
}

//Model danh sách item đã rơi ra
public class ItemDroped {
    public sbyte ItemType; //Loại item
    public byte ItemID; //ID của item
    public short Quantity; //Số lượng rơi ra
    public ItemDroped (sbyte itemType, byte itemID, short quantity) {
        ItemType = itemType;
        ItemID = itemID;
        Quantity = quantity;
    }
}
public static class ItemDropController {
    public static List<ItemDrop> ItemsDrop = new List<ItemDrop> (); //Danh sách item rơi ra tại map nào

    /// <summary>
    /// Khởi tạo
    /// </summary>
    public static void Initialize () {
        if (ItemsDrop.Count <= 0) //Check điều kiện nếu đã khởi tạo rồi thì ko tạo lại nữa, tốn hiệu năng
        {
            //0: rừng
            //1: đồng bằng
            //2: núi lửa
            //3: núi tuyết
            //4: địa ngục
            //5: hang độc
            //6: hang ma
            //Cấu trúc: itemType, itemID, rơi tại map nào, tỉ lệ rơi (1 = 0.01%, 10000 = 100%)

            #region Map rừng 

            ItemsDrop.Add (new ItemDrop (11, 0, 0, 7500)); //Lá ngón
            ItemsDrop.Add (new ItemDrop (11, 1, 0, 7000)); //Hoa cúc trắng
            ItemsDrop.Add (new ItemDrop (11, 2, 0, 6500)); //Cỏ gai
            ItemsDrop.Add (new ItemDrop (11, 3, 0, 6000)); //Quả mã tiền
            ItemsDrop.Add (new ItemDrop (11, 4, 0, 5500)); //Hoa súng
            ItemsDrop.Add (new ItemDrop (11, 5, 0, 5000)); //Nhựa cây
            ItemsDrop.Add (new ItemDrop (11, 6, 0, 4500)); //Hoa cúc xanh
            ItemsDrop.Add (new ItemDrop (11, 7, 0, 4000)); //Quả cherry
            ItemsDrop.Add (new ItemDrop (11, 8, 0, 3500)); //Hoa trúc đào
            ItemsDrop.Add (new ItemDrop (11, 9, 0, 3000)); //Gai xương rồng
            ItemsDrop.Add (new ItemDrop (11, 10, 0, 2500)); //Lá gai
            ItemsDrop.Add (new ItemDrop (11, 11, 0, 2000)); //Ớt đỏ
            ItemsDrop.Add (new ItemDrop (11, 12, 0, 1500)); //Lá con khỉ
            ItemsDrop.Add (new ItemDrop (11, 13, 0, 1000)); //Cỏ dại

            ItemsDrop.Add (new ItemDrop (11, 14, 0, 8000)); //Vải thô
            ItemsDrop.Add (new ItemDrop (11, 15, 0, 7000)); //Vải len
            ItemsDrop.Add (new ItemDrop (11, 16, 0, 6000)); //Vải lanh
            ItemsDrop.Add (new ItemDrop (11, 17, 0, 4000)); //Vải jeans
            ItemsDrop.Add (new ItemDrop (11, 18, 0, 3000)); //Vải cotton
            ItemsDrop.Add (new ItemDrop (11, 19, 0, 2000)); //Vải lụa

            ItemsDrop.Add (new ItemDrop (11, 31, 0, 8000)); //Nhánh cây
            ItemsDrop.Add (new ItemDrop (11, 32, 0, 7000)); //Tấm gỗ nhỏ
            ItemsDrop.Add (new ItemDrop (11, 33, 0, 6000)); //Tấm gỗ lớn
            ItemsDrop.Add (new ItemDrop (11, 34, 0, 5000)); //Gỗ thông
            ItemsDrop.Add (new ItemDrop (11, 35, 0, 4000)); //Gỗ trầm hương
            ItemsDrop.Add (new ItemDrop (11, 36, 0, 3000)); //Gỗ xoan
            ItemsDrop.Add (new ItemDrop (11, 37, 0, 2000)); //Gỗ lim

            ItemsDrop.Add (new ItemDrop (10, 25, 0, 2000)); //Lông phượng

            #endregion

            #region Map đồng bằng 

            ItemsDrop.Add (new ItemDrop (11, 0, 1, 7500)); //Lá ngón
            ItemsDrop.Add (new ItemDrop (11, 1, 1, 7000)); //Hoa cúc trắng
            ItemsDrop.Add (new ItemDrop (11, 2, 1, 6500)); //Cỏ gai
            ItemsDrop.Add (new ItemDrop (11, 3, 1, 6000)); //Quả mã tiền
            ItemsDrop.Add (new ItemDrop (11, 4, 1, 5500)); //Hoa súng
            ItemsDrop.Add (new ItemDrop (11, 5, 1, 5000)); //Nhựa cây
            ItemsDrop.Add (new ItemDrop (11, 6, 1, 4500)); //Hoa cúc xanh
            ItemsDrop.Add (new ItemDrop (11, 7, 1, 4000)); //Quả cherry
            ItemsDrop.Add (new ItemDrop (11, 8, 1, 3500)); //Hoa trúc đào
            ItemsDrop.Add (new ItemDrop (11, 9, 1, 3000)); //Gai xương rồng
            ItemsDrop.Add (new ItemDrop (11, 10, 1, 2500)); //Lá gai
            ItemsDrop.Add (new ItemDrop (11, 11, 1, 2000)); //Ớt đỏ
            ItemsDrop.Add (new ItemDrop (11, 12, 1, 1500)); //Lá con khỉ
            ItemsDrop.Add (new ItemDrop (11, 13, 1, 1000)); //Cỏ dại

            ItemsDrop.Add (new ItemDrop (11, 14, 1, 8000)); //Vải thô
            ItemsDrop.Add (new ItemDrop (11, 15, 1, 7000)); //Vải len
            ItemsDrop.Add (new ItemDrop (11, 16, 1, 6000)); //Vải lanh
            ItemsDrop.Add (new ItemDrop (11, 17, 1, 4000)); //Vải jeans
            ItemsDrop.Add (new ItemDrop (11, 18, 1, 3000)); //Vải cotton
            ItemsDrop.Add (new ItemDrop (11, 19, 1, 2000)); //Vải lụa

            ItemsDrop.Add (new ItemDrop (11, 31, 1, 8000)); //Nhánh cây
            ItemsDrop.Add (new ItemDrop (11, 32, 1, 7000)); //Tấm gỗ nhỏ
            ItemsDrop.Add (new ItemDrop (11, 33, 1, 6000)); //Tấm gỗ lớn
            ItemsDrop.Add (new ItemDrop (11, 34, 1, 5000)); //Gỗ thông
            ItemsDrop.Add (new ItemDrop (11, 35, 1, 4000)); //Gỗ trầm hương
            ItemsDrop.Add (new ItemDrop (11, 36, 1, 3000)); //Gỗ xoan
            ItemsDrop.Add (new ItemDrop (11, 37, 1, 2000)); //Gỗ lim

            ItemsDrop.Add (new ItemDrop (10, 25, 1, 2000)); //Lông phượng

            #endregion

            #region Map núi lửa 

            ItemsDrop.Add (new ItemDrop (11, 20, 2, 10000)); //Quặng kim loại
            ItemsDrop.Add (new ItemDrop (11, 21, 2, 9000)); //Quặng đá
            ItemsDrop.Add (new ItemDrop (11, 22, 2, 9000)); //Quặng crom
            ItemsDrop.Add (new ItemDrop (11, 23, 2, 8000)); //Quặng đồng
            ItemsDrop.Add (new ItemDrop (11, 24, 2, 7000)); //Quặng vàng
            ItemsDrop.Add (new ItemDrop (11, 25, 2, 6000)); //Quặng hồng ngọc
            ItemsDrop.Add (new ItemDrop (11, 26, 2, 5000)); //Quặng ngọc lục bảo
            ItemsDrop.Add (new ItemDrop (11, 27, 2, 4000)); //Quặng thạch anh
            ItemsDrop.Add (new ItemDrop (11, 28, 2, 3000)); //Quặng đá mắt hổ
            ItemsDrop.Add (new ItemDrop (11, 29, 2, 2000)); //Quặng sapphire
            ItemsDrop.Add (new ItemDrop (11, 30, 2, 1000)); //Quặng kim cương

            ItemsDrop.Add (new ItemDrop (11, 41, 2, 500)); //Pha lê rực lửa
            ItemsDrop.Add (new ItemDrop (11, 44, 2, 100)); //Lông vũ

            ItemsDrop.Add (new ItemDrop (10, 25, 2, 2000)); //Lông phượng

            #endregion

            #region Map núi tuyết 

            ItemsDrop.Add (new ItemDrop (11, 0, 3, 7500)); //Lá ngón
            ItemsDrop.Add (new ItemDrop (11, 1, 3, 7000)); //Hoa cúc trắng
            ItemsDrop.Add (new ItemDrop (11, 2, 3, 6500)); //Cỏ gai
            ItemsDrop.Add (new ItemDrop (11, 3, 3, 6000)); //Quả mã tiền
            ItemsDrop.Add (new ItemDrop (11, 4, 3, 5500)); //Hoa súng
            ItemsDrop.Add (new ItemDrop (11, 5, 3, 5000)); //Nhựa cây
            ItemsDrop.Add (new ItemDrop (11, 6, 3, 4500)); //Hoa cúc xanh
            ItemsDrop.Add (new ItemDrop (11, 7, 3, 4000)); //Quả cherry
            ItemsDrop.Add (new ItemDrop (11, 8, 3, 3500)); //Hoa trúc đào
            ItemsDrop.Add (new ItemDrop (11, 9, 3, 3000)); //Gai xương rồng
            ItemsDrop.Add (new ItemDrop (11, 10, 3, 2500)); //Lá gai
            ItemsDrop.Add (new ItemDrop (11, 11, 3, 2000)); //Ớt đỏ
            ItemsDrop.Add (new ItemDrop (11, 12, 3, 1500)); //Lá con khỉ
            ItemsDrop.Add (new ItemDrop (11, 13, 3, 1000)); //Cỏ dại

            ItemsDrop.Add (new ItemDrop (11, 42, 3, 500)); //Pha lê thủy triều

            ItemsDrop.Add (new ItemDrop (10, 25, 3, 2000)); //Lông phượng

            #endregion

            #region Map địa ngục 

            ItemsDrop.Add (new ItemDrop (11, 42, 4, 500)); //Pha lê thủy triều
            ItemsDrop.Add (new ItemDrop (11, 43, 4, 500)); //Pha lê địa đàng
            ItemsDrop.Add (new ItemDrop (11, 41, 4, 500)); //Pha lê rực lửa
            ItemsDrop.Add (new ItemDrop (11, 40, 4, 500)); //Pha lê lốc xoáy
            ItemsDrop.Add (new ItemDrop (10, 25, 4, 2000)); //Lông phượng

            #endregion

            #region Map hang độc 

            ItemsDrop.Add (new ItemDrop (10, 25, 5, 2000)); //Lông phượng
            ItemsDrop.Add (new ItemDrop (11, 43, 5, 500)); //Pha lê địa đàng

            #endregion

            #region Map Hang ma 

            ItemsDrop.Add (new ItemDrop (11, 20, 6, 10000)); //Quặng kim loại
            ItemsDrop.Add (new ItemDrop (11, 21, 6, 9000)); //Quặng đá
            ItemsDrop.Add (new ItemDrop (11, 22, 6, 9000)); //Quặng crom
            ItemsDrop.Add (new ItemDrop (11, 23, 6, 8000)); //Quặng đồng
            ItemsDrop.Add (new ItemDrop (11, 24, 6, 7000)); //Quặng vàng
            ItemsDrop.Add (new ItemDrop (11, 25, 6, 6000)); //Quặng hồng ngọc
            ItemsDrop.Add (new ItemDrop (11, 26, 6, 5000)); //Quặng ngọc lục bảo
            ItemsDrop.Add (new ItemDrop (11, 27, 6, 4000)); //Quặng thạch anh
            ItemsDrop.Add (new ItemDrop (11, 28, 6, 3000)); //Quặng đá mắt hổ
            ItemsDrop.Add (new ItemDrop (11, 29, 6, 2000)); //Quặng sapphire
            ItemsDrop.Add (new ItemDrop (11, 30, 6, 1000)); //Quặng kim cương

            ItemsDrop.Add (new ItemDrop (11, 38, 6, 2000)); //Cuốn thư cổ
            ItemsDrop.Add (new ItemDrop (11, 39, 6, 2000)); //Cuốn sách cổ

            ItemsDrop.Add (new ItemDrop (11, 40, 6, 500)); //Pha lê lốc xoáy

            ItemsDrop.Add (new ItemDrop (10, 25, 6, 2000)); //Lông phượng

            #endregion

            #region Ngọc khảm rơi full map 

            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 0,  500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 0, 500)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 0, 500)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 1,  700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 1, 700)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 1, 700)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 2, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 2, 1000)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 3, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 3, 1000)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 4, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 4, 1000)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 5, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 5, 1000)); //Ngọc socket
            //Socket jewel
            ItemsDrop.Add (new ItemDrop (12, 0, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 1, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 2, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 3, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 4, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 5, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 6, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 7, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 8, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 9, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 10, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 11, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 12, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 13, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 14, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 15, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 16, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 17, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 18, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 19, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 20, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 21, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 22, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 23, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 24, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 25, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 26, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 27, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 28, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 29, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 30, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 31, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 32, 6, 1000)); //Ngọc socket
            ItemsDrop.Add (new ItemDrop (12, 33, 6, 1000)); //Ngọc socket

            #endregion

            #region Khuôn socket, rơi full map 

            ItemsDrop.Add (new ItemDrop (10, 26, 0, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 1, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 2, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 3, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 4, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 5, 1000)); //Khuôn socket
            ItemsDrop.Add (new ItemDrop (10, 26, 6, 1000)); //Khuôn socket

            #endregion
        }
    }

    /// <summary>
    /// Rớt item dành cho battle, do code cũ nên phải viết thêm hàm này
    /// </summary>
    /// <param name="mapRegionID"></param>
    /// <returns></returns>
    public static ItemDrop DropedForBattle (int mapRegionID) {
        var ratioDrop = UnityEngine.Random.Range (0, 10000); //Tính tỉ lệ rớt đồ
        var itemDroped = ItemsDrop.Where (x => x.DropOnMap == mapRegionID && x.Ratio >= ratioDrop).ToList (); //Lấy danh sách các item trong map có thể rớt ra

        var randomSlot = itemDroped.Count > 0 ? UnityEngine.Random.Range (0, itemDroped.Count) : -1; //Lấy ngẫu nhiên dòng
        var itemReward = itemDroped.Count > 0 ? itemDroped.Skip (randomSlot).Take (1).First () : null;
        return itemReward;
    }
}