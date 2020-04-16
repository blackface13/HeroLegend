using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemSystem {

    public static bool IsCalculatorItemPrice;
    static byte[] ItemArray;
    static int[, ] ItemPrice;
    //static List<ItemBreakModel> ListItemBreak; //Danh sách item có thể nhận dc khi phân giải

    #region Initialize 

    /// <summary>
    /// Khởi tạo phân vùng cho item
    /// </summary>
    static void InitializeItemArray () {
        if (ItemArray == null || ItemArray.Length <= 0) {
            ItemArray = new byte[13];
            ItemArray[0] = 67;
            ItemArray[1] = 8;
            ItemArray[2] = 6;
            ItemArray[3] = 5;
            ItemArray[4] = 10;
            ItemArray[5] = 6;
            ItemArray[6] = 10;
            ItemArray[7] = 5;
            ItemArray[8] = 5;
            ItemArray[9] = 0;
            ItemArray[10] = 26;
            ItemArray[11] = 45;
            ItemArray[12] = 34;
        }
    }

    /// <summary>
    /// Setup giá bán item
    /// </summary>
    private static void ItemPriceSetup () {
        if (ItemPrice == null) {
            ItemPrice = new int[3, 50];
            ItemPrice[0, 0] = 50; //Bình máu 1
            ItemPrice[0, 1] = 60; //Bình máu 2
            ItemPrice[0, 2] = 70; //Bình máu 3
            ItemPrice[0, 3] = 80; //Bình máu 4
            ItemPrice[0, 4] = 90; //Bình máu 5
            ItemPrice[0, 5] = 100; //Bình máu 6
            ItemPrice[0, 6] = 110; //Bình máu 7
            ItemPrice[0, 7] = 120; //Bình máu 8
            ItemPrice[0, 8] = 130; //Bình máu 9
            ItemPrice[0, 9] = 230; //Đá cường hóa cấp 1";
            ItemPrice[0, 10] = 240; //Đá cường hóa cấp 2";
            ItemPrice[0, 11] = 250; //Đá cường hóa cấp 3";
            ItemPrice[0, 12] = 260; //Đá cường hóa cấp 4";
            ItemPrice[0, 13] = 270; //Đá cường hóa cấp 5";
            ItemPrice[0, 14] = 280; //Đá cường hóa cấp 6";
            ItemPrice[0, 15] = 290; //Đá cường hóa cấp 7";
            ItemPrice[0, 16] = 300; //Đá cường hóa cấp 8";
            ItemPrice[0, 17] = 310; //Đá cường hóa cấp 9";
            ItemPrice[0, 18] = 320; //Đá cường hóa cấp 10";
            ItemPrice[0, 19] = 350; //Đá nâng phẩm cấp 1";
            ItemPrice[0, 20] = 360; //Đá nâng phẩm cấp 2";
            ItemPrice[0, 21] = 370; //Đá nâng phẩm cấp 3";
            ItemPrice[0, 22] = 380; //Đá nâng phẩm cấp 4";
            ItemPrice[0, 23] = 390; //Đá nâng phẩm cấp 5";
            ItemPrice[0, 24] = 400; //Đá nâng phẩm cấp 6";
            ItemPrice[0, 25] = 320; //Lông phượng
            //Item nguyên liệu
            ItemPrice[1, 0] = 6; //Lá ngón
            ItemPrice[1, 1] = 7; //Hoa cúc trắng
            ItemPrice[1, 2] = 8; //Cỏ gai
            ItemPrice[1, 3] = 9; //Quả mã tiền
            ItemPrice[1, 4] = 10; //Hoa súng
            ItemPrice[1, 5] = 11; //Nhựa cây
            ItemPrice[1, 6] = 12; //Hoa cúc xanh
            ItemPrice[1, 7] = 13; //Quả cherry
            ItemPrice[1, 8] = 14; //Hoa trúc đào
            ItemPrice[1, 9] = 15; //Gai xương rồng
            ItemPrice[1, 10] = 16; //Lá gai
            ItemPrice[1, 11] = 17; //Ớt đỏ
            ItemPrice[1, 12] = 18; //Lá con khỉ
            ItemPrice[1, 13] = 19; //Cỏ dại
            ItemPrice[1, 14] = 20; //Vải thô
            ItemPrice[1, 15] = 25; //Vải len
            ItemPrice[1, 16] = 30; //Vải lanh
            ItemPrice[1, 17] = 35; //Vải jeans
            ItemPrice[1, 18] = 40; //Vải cotton
            ItemPrice[1, 19] = 45; //Vải lụa
            ItemPrice[1, 20] = 27; //Quặng kim loại
            ItemPrice[1, 21] = 32; //Quặng đá
            ItemPrice[1, 22] = 39; //Quặng crom
            ItemPrice[1, 23] = 45; //Quặng đồng
            ItemPrice[1, 24] = 51; //Quặng vàng
            ItemPrice[1, 25] = 63; //Quặng hồng ngọc
            ItemPrice[1, 26] = 75; //Quặng ngọc lục bảo
            ItemPrice[1, 27] = 89; //Quặng thạch anh
            ItemPrice[1, 28] = 99; //Quặng đá mắt hổ
            ItemPrice[1, 29] = 120; //Quặng sapphire
            ItemPrice[1, 30] = 200; //Quặng kim cương
            ItemPrice[1, 31] = 10; //Nhánh cây
            ItemPrice[1, 32] = 13; //Tấm gỗ nhỏ
            ItemPrice[1, 33] = 17; //Tấm gỗ lớn
            ItemPrice[1, 34] = 25; //Gỗ thông
            ItemPrice[1, 35] = 29; //Gỗ trầm hương
            ItemPrice[1, 36] = 33; //Gỗ xoan
            ItemPrice[1, 37] = 42; //Gỗ lim
            ItemPrice[1, 38] = 56; //Cuốn thư cổ
            ItemPrice[1, 39] = 61; //Cuốn sách cổ
            ItemPrice[1, 40] = 760; //Pha lê lốc xoáy
            ItemPrice[1, 41] = 770; //Pha lê rực lửa
            ItemPrice[1, 42] = 780; //Pha lê thủy triều
            ItemPrice[1, 43] = 790; //Pha lê địa đàng
            ItemPrice[1, 44] = 800; //Lông vũ
            //Item socket
            ItemPrice[2, 0] = 1300;
            ItemPrice[2, 1] = 1300;
            ItemPrice[2, 2] = 1300;
            ItemPrice[2, 3] = 1300;
            ItemPrice[2, 4] = 1300;
            ItemPrice[2, 5] = 1300;
            ItemPrice[2, 6] = 1300;
            ItemPrice[2, 7] = 1300;
            ItemPrice[2, 8] = 1300;
            ItemPrice[2, 9] = 1300;
            ItemPrice[2, 10] = 1300;
            ItemPrice[2, 11] = 1300;
            ItemPrice[2, 12] = 1300;
            ItemPrice[2, 13] = 1300;
            ItemPrice[2, 14] = 1300;
            ItemPrice[2, 15] = 1300;
            ItemPrice[2, 16] = 1300;
            ItemPrice[2, 17] = 1300;
            ItemPrice[2, 18] = 1300;
            ItemPrice[2, 19] = 1300;
            ItemPrice[2, 20] = 1300;
            ItemPrice[2, 21] = 1300;
            ItemPrice[2, 22] = 1300;
            ItemPrice[2, 23] = 1300;
            ItemPrice[2, 24] = 1300;
            ItemPrice[2, 25] = 1300;
            ItemPrice[2, 26] = 1300;
            ItemPrice[2, 27] = 1300;
            ItemPrice[2, 28] = 1300;
            ItemPrice[2, 29] = 1300;
            ItemPrice[2, 30] = 1300;
            ItemPrice[2, 31] = 1300;
            ItemPrice[2, 32] = 1300;
            ItemPrice[2, 33] = 1300;
        }
    }

    #endregion

    #region Functions 

    /// <summary>
    /// Tạo item, nếu isRandomItemType = true -> random các item trang bị 
    /// </summary>
    public static ItemModel CreateItem (bool isRandomItemType, bool isRandomItemID, float levelCreate, sbyte itemType, byte itemID, int quantity) {
        InitializeItemArray ();
        ItemModel item = new ItemModel ();
        item.vIsLock = false; //Cho phép giao dịch khi tạo mới 1 item
        item.ItemType = isRandomItemType ? (sbyte) UnityEngine.Random.Range (0, 9) : itemType;
        item.ItemTypeMode = GetItemModeByItemType (item.ItemType);
        item.ItemGuidID = Guid.NewGuid ().ToString ();
        item.ItemID = isRandomItemID?(byte) UnityEngine.Random.Range (0, ItemArray[item.ItemType]) : itemID;
        item.ItemLevel = 0;
        item.Quantity = item.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip) ? 1 : quantity;
        switch (item.ItemTypeMode) {
            #region Item trang bị 
            case global::ItemModel.TypeMode.Equip:
                if (item.ItemType.Equals (0)) {
                    item.vAtk = RandomValuesItems (levelCreate, "vAtk"); //Sát thương vật lý
                    item.vDamageEarth = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageEarthRate? RandomValuesItems (levelCreate, "vDamageEarth") : 0; //Sát thương hệ đất
                    item.vDamageWater = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageWaterRate? RandomValuesItems (levelCreate, "vDamageWater") : 0; //Sát thương hệ nước
                    item.vDamageFire = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageFireRate? RandomValuesItems (levelCreate, "vDamageFire") : 0; //Sát thương hệ lửa
                    item.vAtkSpeed = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vAtkSpeedRate?(sbyte) RandomValuesItems (levelCreate, "vAtkSpeed"): (sbyte) 0; //% Tốc độ tấn công cơ bản tăng thêm
                    item.vLifeStealPhysic = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vLifeStealPhysicRate?(sbyte) RandomValuesItems (levelCreate, "vLifeStealPhysic"): (sbyte) 0; //% hút máu
                    item.vLethality = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vLethalityRate?(sbyte) RandomValuesItems (levelCreate, "vLethality"): (sbyte) 0; //% Xuyên giáp
                    item.vCritical = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vCriticalRate?(sbyte) RandomValuesItems (levelCreate, "vCritical"): (sbyte) 0; //% chí mạng
                    item.vAtkPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vAtkPlusRate?(short) RandomValuesItems (levelCreate, "vAtkPlus"): (short) 0; //Gia tăng % sát thương vật lý (max = 32767)
                    item.vDamageExcellent = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageExcellentRate?(sbyte) RandomValuesItems (levelCreate, "vDamageExcellent"): (sbyte) 0; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
                    item.vDoubleDamage = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDoubleDamageRate?(sbyte) RandomValuesItems (levelCreate, "vDoubleDamage"): (sbyte) 0; //Tỉ lệ x2 đòn đánh max = 10%
                    item.vTripleDamage = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vTripleDamageRate?(sbyte) RandomValuesItems (levelCreate, "vTripleDamage"): (sbyte) 0; //Tỉ lệ x3 đòn đánh max = 10%
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                if (item.ItemType.Equals (1) || item.ItemType.Equals (2) || item.ItemType.Equals (3)) {
                    item.vMagic = RandomValuesItems (levelCreate, "vMagic");; //Sát thương phép thuật
                    item.vDamageEarth = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageEarthRate? RandomValuesItems (levelCreate, "vDamageEarth") : 0; //Sát thương hệ đất
                    item.vDamageWater = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageWaterRate? RandomValuesItems (levelCreate, "vDamageWater") : 0; //Sát thương hệ nước
                    item.vDamageFire = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageFireRate? RandomValuesItems (levelCreate, "vDamageFire") : 0; //Sát thương hệ lửa
                    item.vLifeStealMagic = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vLifeStealMagicRate?(sbyte) RandomValuesItems (levelCreate, "vLifeStealMagic"): (sbyte) 0; //% hút máu phép
                    item.vMagicPenetration = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vMagicPenetrationRate?(sbyte) RandomValuesItems (levelCreate, "vMagicPenetration"): (sbyte) 0; //% Xuyên phép
                    item.vMagicPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vMagicPlusRate?(short) RandomValuesItems (levelCreate, "vMagicPlus"): (short) 0; //Gia tăng % sát thương phép (max = 32767)
                    item.vDamageExcellent = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageExcellentRate?(sbyte) RandomValuesItems (levelCreate, "vDamageExcellent"): (sbyte) 0; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
                    item.vDoubleDamage = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDoubleDamageRate?(sbyte) RandomValuesItems (levelCreate, "vDoubleDamage"): (sbyte) 0; //Tỉ lệ x2 đòn đánh max = 10%
                    item.vTripleDamage = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vTripleDamageRate?(sbyte) RandomValuesItems (levelCreate, "vTripleDamage"): (sbyte) 0; //Tỉ lệ x3 đòn đánh max = 10%
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                if (item.ItemType.Equals (4)) //Giáp
                {
                    item.vArmor = RandomValuesItems (levelCreate, "vArmor"); //Giáp
                    item.vMagicResist = RandomValuesItems (levelCreate, "vMagicResist"); //Kháng phép
                    item.vDefenseEarth = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseEarthRate? RandomValuesItems (levelCreate, "vDefenseEarth") : 0; //Kháng hệ đất
                    item.vDefenseWater = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseWaterRate? RandomValuesItems (levelCreate, "vDefenseWater") : 0; //Kháng hệ nước
                    item.vDefenseFire = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseFireRate? RandomValuesItems (levelCreate, "vDefenseFire") : 0; //Kháng hệ hỏa
                    item.vTenacity = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vTenacityRate?(sbyte) RandomValuesItems (levelCreate, "vTenacity"): (sbyte) 0; //% kháng hiệu ứng
                    item.vArmorPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vArmorPlusRate?(short) RandomValuesItems (levelCreate, "vArmorPlus"): (short) 0; //Gia tăng % Giáp
                    item.vMagicResistPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vMagicResistPlusRate?(short) RandomValuesItems (levelCreate, "vMagicResistPlus"): (short) 0; //Gia tăng % Kháng phép
                    item.vDefenseExcellent = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseExcellentRate?(sbyte) RandomValuesItems (levelCreate, "vDefenseExcellent"): (sbyte) 0; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
                    item.vDamageReflect = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageReflectRate?(sbyte) RandomValuesItems (levelCreate, "vDamageReflect"): (sbyte) 0; //Phản hồi % sát thương. max = 5%
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }

                if (item.ItemType.Equals (5)) //Đai lưng
                {
                    item.vHealth = RandomValuesItems (levelCreate, "vHealth"); //Máu
                    item.vHealthRegen = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vHealthRegenRate? RandomValuesItems (levelCreate, "vHealthRegen") : 0; //Chỉ số hồi máu mỗi giây
                    item.vDefenseEarth = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseEarthRate? RandomValuesItems (levelCreate, "vDefenseEarth") : 0; //Kháng hệ đất
                    item.vTenacity = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vTenacityRate?(sbyte) RandomValuesItems (levelCreate, "vTenacity"): (sbyte) 0; //% kháng hiệu ứng
                    item.vCooldownReduction = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vCooldownReductionRate?(sbyte) RandomValuesItems (levelCreate, "vCooldownReduction"): (sbyte) 0; //% Giảm tgian hồi chiêu
                    item.vHealthPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vHealthPlusRate?(short) RandomValuesItems (levelCreate, "vHealthPlus"): (short) 0; //Gia tăng % máu (max = 32767)
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                if (item.ItemType.Equals (6)) { //Giáp tay
                    item.vMana = RandomValuesItems (levelCreate, "vMana"); //Mana
                    item.vArmor = RandomValuesItems (levelCreate, "vArmor"); //Giáp
                    item.vDefenseWater = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseWaterRate? RandomValuesItems (levelCreate, "vDefenseWater") : 0; //Kháng hệ nước
                    item.vManaPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vManaPlusRate?(short) RandomValuesItems (levelCreate, "vManaPlus"): (short) 0; //Gia tăng % mana (max = 32767)
                    item.vArmorPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vArmorPlusRate?(short) RandomValuesItems (levelCreate, "vArmorPlus"): (short) 0; //Gia tăng % Giáp
                    item.vDamageReflect = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageReflectRate?(sbyte) RandomValuesItems (levelCreate, "vDamageReflect"): (sbyte) 0; //Phản hồi % sát thương. max = 5%
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                if (item.ItemType.Equals (7)) { //Găng tay
                    item.vHealth = RandomValuesItems (levelCreate, "vHealth"); //Máu
                    item.vMagicResist = RandomValuesItems (levelCreate, "vMagicResist"); //Kháng phép
                    item.vHealthRegen = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vHealthRegenRate? RandomValuesItems (levelCreate, "vHealthRegen") : 0; //Chỉ số hồi máu mỗi giây
                    item.vDefenseFire = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseFireRate? RandomValuesItems (levelCreate, "vDefenseFire") : 0; //Kháng hệ hỏa
                    item.vHealthPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vHealthPlusRate?(short) RandomValuesItems (levelCreate, "vHealthPlus"): (short) 0; //Gia tăng % máu (max = 32767)
                    item.vMagicResistPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vMagicResistPlusRate?(short) RandomValuesItems (levelCreate, "vMagicResistPlus"): (short) 0; //Gia tăng % Kháng phép
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                if (item.ItemType.Equals (8)) { //Khiên
                    item.vArmor = RandomValuesItems (levelCreate, "vArmor"); //Giáp
                    item.vMagicResist = RandomValuesItems (levelCreate, "vMagicResist"); //Kháng phép
                    item.vDefenseEarth = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseEarthRate? RandomValuesItems (levelCreate, "vDefenseEarth") : 0; //Kháng hệ đất
                    item.vDefenseWater = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseWaterRate? RandomValuesItems (levelCreate, "vDefenseWater") : 0; //Kháng hệ nước
                    item.vDefenseFire = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDefenseFireRate? RandomValuesItems (levelCreate, "vDefenseFire") : 0; //Kháng hệ hỏa
                    item.vTenacity = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vTenacityRate?(sbyte) RandomValuesItems (levelCreate, "vTenacity"): (sbyte) 0; //% kháng hiệu ứng
                    item.vArmorPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vArmorPlusRate?(short) RandomValuesItems (levelCreate, "vArmorPlus"): (short) 0; //Gia tăng % Giáp
                    item.vMagicResistPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vMagicResistPlusRate?(short) RandomValuesItems (levelCreate, "vMagicResistPlus"): (short) 0; //Gia tăng % Kháng phép
                    item.vDamageReflect = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vDamageReflectRate?(sbyte) RandomValuesItems (levelCreate, "vDamageReflect"): (sbyte) 0; //Phản hồi % sát thương. max = 5%
                    item.vRewardPlus = UnityEngine.Random.Range (0, 100) < ItemCoreSetting.vRewardPlusRate?(sbyte) RandomValuesItems (levelCreate, "vRewardPlus"): (sbyte) 0; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
                }
                break;
                #endregion
            default:
                break;
        }
        item.vSocketSlot = 0;
        #region Tạo socket cho item trang bị 

        if (item.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
            //Tạo socket theo tỉ lệ
            var haveSocketSlotRate = UnityEngine.Random.Range (0, 100);
            for (int i = ItemCoreSetting.CreateHaveSocketRate.Length - 1; i >= 0; i--) {
                if (haveSocketSlotRate < ItemCoreSetting.CreateHaveSocketRate[i]) {
                    item.vSocketSlot = (sbyte) (i + 1);
                    break;
                }
            }
            item.Sockets = item.vSocketSlot > 0 ? new List<SocketModel> () : null;
        }

        #endregion
        item.ItemPrice = GetItemPrice (item); //Tạo giá bán cho item
        return item;
    }

    /// <summary>
    /// Trả về loại item dựa trên itemType truyền vào
    /// </summary>
    private static global::ItemModel.TypeMode GetItemModeByItemType (sbyte itemType) {
        if (itemType >= 0 && itemType <= 8) //Item trang bị
            return global::ItemModel.TypeMode.Equip;
        if (itemType == 10) //Item sử dụng
            return global::ItemModel.TypeMode.Use;
        if (itemType == 11) //Item quest
            return global::ItemModel.TypeMode.Quest;
        return global::ItemModel.TypeMode.Quest;
    }

    /// <summary>
    /// Khởi tạo giá trị ngẫu nhiên cho item
    /// </summary>
    /// <returns></returns>
    private static float RandomValuesItems (float levelCreate, string properties) {
        switch (properties) {
            case "vAtk": //Tấn công
                return UnityEngine.Random.Range ((float) (10 * levelCreate), (float) (10 * (levelCreate + 1)));
            case "vMagic": //Phép thuật
                return UnityEngine.Random.Range ((float) (10 * levelCreate), (float) (10 * (levelCreate + 1)));
            case "vHealth": //Máu
                return UnityEngine.Random.Range ((float) (100 * levelCreate), (float) (100 * (levelCreate + 1)));
            case "vMana": //Mana
                return UnityEngine.Random.Range ((float) (100 * levelCreate), (float) (100 * (levelCreate + 1)));
            case "vArmor": //Giáp
                return UnityEngine.Random.Range ((float) (10 * levelCreate), (float) (10 * (levelCreate + 1)));
            case "vMagicResist": //Kháng phép
                return UnityEngine.Random.Range ((float) (8 * levelCreate), (float) (8 * (levelCreate + 1)));
            case "vHealthRegen": //Hồi máu
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (2 * levelCreate));
            case "vManaRegen": //Hồi mana
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (2 * levelCreate));
            case "vDamageEarth": //Sát thương hệ đất
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vDamageWater": //Sát thương hệ nước
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vDamageFire": //Sát thương hệ lửa
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vDefenseEarth": //Kháng Sát thương hệ đất
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vDefenseWater": //Kháng Sát thương hệ nước
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vDefenseFire": //Kháng Sát thương hệ lửa
                return UnityEngine.Random.Range ((float) (0 * levelCreate) + 0.1f, (float) (5 * levelCreate));
            case "vAtkSpeed": //Tốc độ tấn công cơ bản tăng thêm
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vLifeStealPhysic": //Hút máu
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vLifeStealMagic": //Hút máu phép
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vLethality": //Xuyên giáp
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vMagicPenetration": //Xuyên phép
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vCritical": //Chí mạng
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vTenacity": //Kháng hiệu ứng
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vCooldownReduction": //Giảm tgian hồi chiêu
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vAtkPlus": //Gia tăng % sát thương vật lý
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vMagicPlus": //Gia tăng % sát thương phép
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vHealthPlus": //Gia tăng % máu
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vManaPlus": //Gia tăng % mana
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vDamageExcellent": //Sát thương hoàn hảo
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vDefenseExcellent": //phong thu hoàn hảo
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vDoubleDamage": //Tỉ lệ x2 đòn đánh max 
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vTripleDamage": //Tỉ lệ x3 đòn đánh
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (2 * levelCreate));
            case "vDamageReflect": //Phản hồi % sát thương
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (1 * levelCreate));
            case "vRewardPlus": //Tăng lượng vàng rơi ra vào cuối trận
                return (int) UnityEngine.Random.Range ((0 * levelCreate), (4 * levelCreate));

            default:
                break;
        }
        return 0;
    }

    /// <summary>
    /// Trả về tên item thông qua type và ID
    /// </summary>
    public static string GetItemName (sbyte itemType, byte itemID) {
        switch (itemType) {
            case 0:
                return Languages.ItemNameType0[itemID];
            case 1:
                return Languages.ItemNameType1[itemID];
            case 2:
                return Languages.ItemNameType2[itemID];
            case 3:
                return Languages.ItemNameType3[itemID];
            case 4:
                return Languages.ItemNameType4[itemID];
            case 5:
                return Languages.ItemNameType5[itemID];
            case 6:
                return Languages.ItemNameType6[itemID];
            case 7:
                return Languages.ItemNameType7[itemID];
            case 8:
                return Languages.ItemNameType8[itemID];
            case 10:
                return Languages.ItemNameType10[itemID];
            case 11:
                return Languages.ItemNameType11[itemID];
            case 12:
                return Languages.ItemNameType12[itemID];
            default:
                break;
        }
        return "";
    }

    /// <summary>
    /// Trả về thông tin nội dung của socket
    /// </summary>
    public static string GetItemSocketDescription (SocketModel socket) {
        var descriptions = "";
        if (socket != null) {
            descriptions += socket.vAtk > 0 ? string.Format (Languages.lang[700], socket.vAtk) + "\n" : "";
            descriptions += socket.vMagic > 0 ? string.Format (Languages.lang[701], socket.vMagic) + "\n" : "";
            descriptions += socket.vHealth > 0 ? string.Format (Languages.lang[702], socket.vHealth) + "\n" : "";
            descriptions += socket.vMana > 0 ? string.Format (Languages.lang[703], socket.vMana) + "\n" : "";
            descriptions += socket.vArmor > 0 ? string.Format (Languages.lang[704], socket.vArmor) + "\n" : "";
            descriptions += socket.vMagicResist > 0 ? string.Format (Languages.lang[705], socket.vMagicResist) + "\n" : "";
            descriptions += socket.vHealthRegen > 0 ? string.Format (Languages.lang[706], socket.vHealthRegen) + "\n" : "";
            descriptions += socket.vManaRegen > 0 ? string.Format (Languages.lang[707], socket.vManaRegen) + "\n" : "";
            descriptions += socket.vDamageEarth > 0 ? string.Format (Languages.lang[708], socket.vDamageEarth) + "\n" : "";
            descriptions += socket.vDamageWater > 0 ? string.Format (Languages.lang[709], socket.vDamageWater) + "\n" : "";
            descriptions += socket.vDamageFire > 0 ? string.Format (Languages.lang[710], socket.vDamageFire) + "\n" : "";
            descriptions += socket.vDefenseEarth > 0 ? string.Format (Languages.lang[711], socket.vDefenseWater) + "\n" : "";
            descriptions += socket.vDefenseWater > 0 ? string.Format (Languages.lang[712], socket.vDefenseWater) + "\n" : "";
            descriptions += socket.vDefenseFire > 0 ? string.Format (Languages.lang[713], socket.vDefenseFire) + "\n" : "";
            descriptions += socket.vAtkSpeed > 0 ? string.Format (Languages.lang[714], socket.vAtkSpeed) + "\n" : "";
            descriptions += socket.vLifeStealPhysic > 0 ? string.Format (Languages.lang[715], socket.vLifeStealPhysic) + "\n" : "";
            descriptions += socket.vLifeStealMagic > 0 ? string.Format (Languages.lang[716], socket.vLifeStealMagic) + "\n" : "";
            descriptions += socket.vLethality > 0 ? string.Format (Languages.lang[717], socket.vLethality) + "\n" : "";
            descriptions += socket.vMagicPenetration > 0 ? string.Format (Languages.lang[718], socket.vMagicPenetration) + "\n" : "";
            descriptions += socket.vCritical > 0 ? string.Format (Languages.lang[719], socket.vCritical) + "\n" : "";
            descriptions += socket.vTenacity > 0 ? string.Format (Languages.lang[720], socket.vTenacity) + "\n" : "";
            descriptions += socket.vCooldownReduction > 0 ? string.Format (Languages.lang[721], socket.vCooldownReduction) + "\n" : "";
            descriptions += socket.vAtkPlus > 0 ? string.Format (Languages.lang[722], socket.vAtkPlus) + "\n" : "";
            descriptions += socket.vMagicPlus > 0 ? string.Format (Languages.lang[723], socket.vMagicPlus) + "\n" : "";
            descriptions += socket.vHealthPlus > 0 ? string.Format (Languages.lang[724], socket.vHealthPlus) + "\n" : "";
            descriptions += socket.vManaPlus > 0 ? string.Format (Languages.lang[725], socket.vManaPlus) + "\n" : "";
            descriptions += socket.vArmorPlus > 0 ? string.Format (Languages.lang[734], socket.vArmorPlus) + "\n" : "";
            descriptions += socket.vMagicResistPlus > 0 ? string.Format (Languages.lang[735], socket.vMagicResistPlus) + "\n" : "";
            descriptions += socket.vDamageExcellent > 0 ? string.Format (Languages.lang[726], socket.vDamageExcellent) + "\n" : "";
            descriptions += socket.vDefenseExcellent > 0 ? string.Format (Languages.lang[727], socket.vDefenseExcellent) + "\n" : "";
            descriptions += socket.vDoubleDamage > 0 ? string.Format (Languages.lang[728], socket.vDoubleDamage) + "\n" : "";
            descriptions += socket.vTripleDamage > 0 ? string.Format (Languages.lang[729], socket.vTripleDamage) + "\n" : "";
            descriptions += socket.vDamageReflect > 0 ? string.Format (Languages.lang[730], socket.vDamageReflect) + "\n" : "";
            descriptions += socket.vRewardPlus > 0 ? string.Format (Languages.lang[731], socket.vRewardPlus) + "\n" : "";
        }
        return descriptions;
    }

    /// <summary>
    /// Trả về giá bán của item
    /// </summary>
    public static int GetItemPrice (ItemModel item) {
        var itemPrice = 0;
        if (item.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
            itemPrice += item.ItemLevel * ItemCoreSetting.ItemLevelPrice;
            itemPrice += item.ItemColor * ItemCoreSetting.ItemColorPrice;
            itemPrice += (int) (item.vAtk * ItemCoreSetting.vAtkPrice);
            itemPrice += (int) (item.vMagic * ItemCoreSetting.vMagicPrice);
            itemPrice += (int) (item.vHealth * ItemCoreSetting.vHealthPrice);
            itemPrice += (int) (item.vMana * ItemCoreSetting.vManaPrice);
            itemPrice += (int) (item.vArmor * ItemCoreSetting.vArmorPrice);
            itemPrice += (int) (item.vMagicResist * ItemCoreSetting.vMagicResistPrice);
            itemPrice += (int) (item.vHealthRegen * ItemCoreSetting.vHealthRegenPrice);
            itemPrice += (int) (item.vManaRegen * ItemCoreSetting.vManaRegenPrice);
            itemPrice += (int) (item.vDamageEarth * ItemCoreSetting.vDamageEarthPrice);
            itemPrice += (int) (item.vDamageWater * ItemCoreSetting.vDamageWaterPrice);
            itemPrice += (int) (item.vDamageFire * ItemCoreSetting.vDamageFirePrice);
            itemPrice += (int) (item.vDefenseEarth * ItemCoreSetting.vDefenseEarthPrice);
            itemPrice += (int) (item.vDefenseWater * ItemCoreSetting.vDefenseWaterPrice);
            itemPrice += (int) (item.vDefenseFire * ItemCoreSetting.vDefenseFirePrice);
            itemPrice +=(int) (item.vAtkSpeed * ItemCoreSetting.vAtkSpeedPrice);
            itemPrice +=(int) (item.vLifeStealPhysic * ItemCoreSetting.vLifeStealPhysicPrice);
            itemPrice +=(int) (item.vLifeStealMagic * ItemCoreSetting.vLifeStealMagicPrice);
            itemPrice +=(int) (item.vLethality * ItemCoreSetting.vLethalityPrice);
            itemPrice +=(int) (item.vMagicPenetration * ItemCoreSetting.vMagicPenetrationPrice);
            itemPrice +=(int) (item.vCritical * ItemCoreSetting.vCriticalPrice);
            itemPrice +=(int) (item.vTenacity * ItemCoreSetting.vTenacityPrice);
            itemPrice +=(int) (item.vCooldownReduction * ItemCoreSetting.vCooldownReductionPrice);
            itemPrice +=(int) (item.vAtkPlus * ItemCoreSetting.vAtkPlusPrice);
            itemPrice +=(int) (item.vMagicPlus * ItemCoreSetting.vMagicPlusPrice);
            itemPrice +=(int) (item.vHealthPlus * ItemCoreSetting.vHealthPlusPrice);
            itemPrice +=(int) (item.vManaPlus * ItemCoreSetting.vManaPlusPrice);
            itemPrice +=(int) (item.vArmorPlus * ItemCoreSetting.vArmorPlusPrice);
            itemPrice +=(int) (item.vMagicResistPlus * ItemCoreSetting.vMagicResistPlusPrice);
            itemPrice +=(int) (item.vDamageExcellent * ItemCoreSetting.vDamageExcellentPrice);
            itemPrice +=(int) (item.vDefenseExcellent * ItemCoreSetting.vDefenseExcellentPrice);
            itemPrice +=(int) (item.vDoubleDamage * ItemCoreSetting.vDoubleDamagePrice);
            itemPrice +=(int) (item.vTripleDamage * ItemCoreSetting.vTripleDamagePrice);
            itemPrice +=(int) (item.vDamageReflect * ItemCoreSetting.vDamageReflectPrice);
            itemPrice +=(int) (item.vRewardPlus * ItemCoreSetting.vRewardPlusPrice);
            itemPrice +=(int) (item.vSocketSlot * ItemCoreSetting.vSocketSlotPrice);
        } else {
            ItemPriceSetup ();
            itemPrice = ItemPrice[item.ItemType - 10, item.ItemID] * item.Quantity;
        }
        return itemPrice;
    }

    /// <summary>
    /// Trả về thông tin item thông qua type và ID, dành cho item khác item trang bị
    /// </summary>
    public static string GetItemDescription (sbyte itemType, byte itemID) {
        switch (itemType) {
            case 10:
                return Languages.ItemInforType10[itemID] + ItemSystem.GetRegionItemDrop (itemType, itemID);
            case 11:
                return Languages.ItemInforType11[itemID] + GetCraftedItem (itemType, itemID) + ItemSystem.GetRegionItemDrop (itemType, itemID);
            case 12:
                return Languages.ItemInforType12[itemID] + ItemSystem.GetRegionItemDrop (itemType, itemID);
            default:
                break;
        }
        return "";
    }

    /// <summary>
    /// Trả về item descriptions
    /// </summary>
    public static string GetItemDescription (ItemModel item) {
        var descriptions = "";
        if (item.ItemTypeMode.Equals (global::ItemModel.TypeMode.Equip)) {
            descriptions += item.vAtk > 0 ? string.Format (Languages.lang[700] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vAtk * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vAtk + (item.vAtk * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vAtk)) : "";
            descriptions += item.vMagic > 0 ? string.Format (Languages.lang[701] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vMagic * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vMagic + (item.vMagic * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vMagic)) : "";
            descriptions += item.vHealth > 0 ? string.Format (Languages.lang[702] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vHealth * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vHealth + (item.vHealth * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vHealth)) : "";
            descriptions += item.vMana > 0 ? string.Format (Languages.lang[703] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vMana * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vMana + (item.vMana * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vMana)) : "";
            descriptions += item.vArmor > 0 ? string.Format (Languages.lang[704] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vArmor * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vArmor + (item.vArmor * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vArmor)) : "";
            descriptions += item.vMagicResist > 0 ? string.Format (Languages.lang[705] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vMagicResist * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vMagicResist + (item.vMagicResist * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vMagicResist)) : "";
            descriptions += item.vHealthRegen > 0 ? string.Format (Languages.lang[706] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vHealthRegen * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vHealthRegen + (item.vHealthRegen * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vHealthRegen)) : "";
            descriptions += item.vManaRegen > 0 ? string.Format (Languages.lang[707] +
                (item.ItemLevel > 0 ? " <color=green>(+" + String.Format ("{0:0.#}", item.vManaRegen * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) + ")</color>" : "") //Cộng chỉ số theo level
                +
                (item.ItemColor > 0 ? " <color=orange>(+" + String.Format ("{0:0.#}", (item.vManaRegen + (item.vManaRegen * ((item.ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f))) * ((item.ItemColor * ItemCoreSetting.UpgradePerColor) / 100f)) + ")</color>" : "") + "\n", //Cộng chỉ số theo màu sắc
                String.Format ("{0:0.#}", item.vManaRegen)) : "";
            descriptions += item.vDamageEarth > 0 ? string.Format (Languages.lang[708], String.Format ("{0:0.#}", item.vDamageEarth)) + "\n" : "";
            descriptions += item.vDamageWater > 0 ? string.Format (Languages.lang[709], String.Format ("{0:0.#}", item.vDamageWater)) + "\n" : "";
            descriptions += item.vDamageFire > 0 ? string.Format (Languages.lang[710], String.Format ("{0:0.#}", item.vDamageFire)) + "\n" : "";
            descriptions += item.vDefenseEarth > 0 ? string.Format (Languages.lang[711], String.Format ("{0:0.#}", item.vDefenseEarth)) + "\n" : "";
            descriptions += item.vDefenseWater > 0 ? string.Format (Languages.lang[712], String.Format ("{0:0.#}", item.vDefenseWater)) + "\n" : "";
            descriptions += item.vDefenseFire > 0 ? string.Format (Languages.lang[713], String.Format ("{0:0.#}", item.vDefenseFire)) + "\n" : "";
            descriptions += item.vAtkSpeed > 0 ? string.Format (Languages.lang[714], item.vAtkSpeed) + "\n" : "";
            descriptions += item.vLifeStealPhysic > 0 ? string.Format (Languages.lang[715], item.vLifeStealPhysic) + "\n" : "";
            descriptions += item.vLifeStealMagic > 0 ? string.Format (Languages.lang[716], item.vLifeStealMagic) + "\n" : "";
            descriptions += item.vLethality > 0 ? string.Format (Languages.lang[717], item.vLethality) + "\n" : "";
            descriptions += item.vMagicPenetration > 0 ? string.Format (Languages.lang[718], item.vMagicPenetration) + "\n" : "";
            descriptions += item.vCritical > 0 ? string.Format (Languages.lang[719], item.vCritical) + "\n" : "";
            descriptions += item.vTenacity > 0 ? string.Format (Languages.lang[720], item.vTenacity) + "\n" : "";
            descriptions += item.vCooldownReduction > 0 ? string.Format (Languages.lang[721], item.vCooldownReduction) + "\n" : "";
            descriptions += item.vAtkPlus > 0 ? string.Format (Languages.lang[722], item.vAtkPlus) + "\n" : "";
            descriptions += item.vMagicPlus > 0 ? string.Format (Languages.lang[723], item.vMagicPlus) + "\n" : "";
            descriptions += item.vHealthPlus > 0 ? string.Format (Languages.lang[724], item.vHealthPlus) + "\n" : "";
            descriptions += item.vManaPlus > 0 ? string.Format (Languages.lang[725], item.vManaPlus) + "\n" : "";
            descriptions += item.vArmorPlus > 0 ? string.Format (Languages.lang[734], item.vArmorPlus) + "\n" : "";
            descriptions += item.vMagicResistPlus > 0 ? string.Format (Languages.lang[735], item.vMagicResistPlus) + "\n" : "";
            descriptions += item.vDamageExcellent > 0 ? string.Format (Languages.lang[726], item.vDamageExcellent) + "\n" : "";
            descriptions += item.vDefenseExcellent > 0 ? string.Format (Languages.lang[727], item.vDefenseExcellent) + "\n" : "";
            descriptions += item.vDoubleDamage > 0 ? string.Format (Languages.lang[728], item.vDoubleDamage) + "\n" : "";
            descriptions += item.vTripleDamage > 0 ? string.Format (Languages.lang[729], item.vTripleDamage) + "\n" : "";
            descriptions += item.vDamageReflect > 0 ? string.Format (Languages.lang[730], item.vDamageReflect) + "\n" : "";
            descriptions += item.vRewardPlus > 0 ? string.Format (Languages.lang[731], item.vRewardPlus) + "\n" : "";
            descriptions += item.vSocketSlot > 0 ? string.Format (Languages.lang[732], item.vSocketSlot) + "\n" : "";
            //Thông tin socket
            if (item.vSocketSlot > 0) {
                if (item.Sockets != null) {
                    for (sbyte i = 0; i < item.vSocketSlot; i++) {
                        if (item.Sockets.Count > i)
                            descriptions += "     " + ItemSystem.GetItemSocketDescription (item.Sockets[i]);
                        else
                            descriptions += "     " + Languages.lang[733] + "\n";
                    }
                } else {
                    for (sbyte i = 0; i < item.vSocketSlot; i++)
                        descriptions += "     " + Languages.lang[733] + "\n";
                }
            }
        } else {
            switch (item.ItemType) {
                case 10:
                    descriptions = Languages.ItemInforType10[item.ItemID] + ItemSystem.GetRegionItemDrop (item.ItemType, item.ItemID);
                    break;
                case 11:
                    descriptions = Languages.ItemInforType11[item.ItemID] + GetCraftedItem (item.ItemType, item.ItemID) + ItemSystem.GetRegionItemDrop (item.ItemType, item.ItemID);
                    break;
                case 12:
                    descriptions = Languages.ItemInforType12[item.ItemID] + ItemSystem.GetRegionItemDrop (item.ItemType, item.ItemID);
                    break;
                default:
                    break;
            }
        }
        return descriptions;
    }

    /// <summary>
    /// Trả về những nơi item có thể rơi ra
    /// </summary>
    /// <param name="item">truyền item đó vào</param>
    /// <returns></returns>
    public static string GetRegionItemDrop (sbyte itemType, byte itemID) {
        var listItemDrop = ItemDropController.ItemsDrop.Where (x => x.ItemType == itemType && x.ItemID == itemID).ToList ();
        if (listItemDrop != null && listItemDrop.Count > 0) {
            string result = Languages.lang[260];
            var count = listItemDrop.Count;
            for (sbyte i = 0; i < count; i++) {
                result += Languages.lang[listItemDrop[i].DropOnMap + 168] + "(" + listItemDrop[i].Ratio / 100f + "%)" + (i != count - 1 ? ", " : "");
            }
            return result;
        } else {
            return "";
        }
    }

    /// <summary>
    /// Trả về string chế tạo ra những gì dành cho các item nguyên liệu chế tạo
    /// Đang tạm dừng, chưa giải quyết dc FirstOrDefault, nó đang trả về 0, 0 là = itemID truyền vào => trả về kết quả sai của item có ID = 0
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public static string GetCraftedItem (sbyte itemType, byte itemID) {
        // string result = "<color=#3F42AF> ";
        // //Lấy danh sách các item mà item này có thể chế tạo
        // var listCraft = CraftCoreSetting.CraftItemUse.Where (x => x.ItemResourceType.Where (m => m == itemType).FirstOrDefault () == itemType && x.ItemResourceID.Where (m => m == itemID).FirstOrDefault () == itemID).ToList ();
        // listCraft.AddRange (CraftCoreSetting.CraftItemWeaponPhysic.Where (x => x.ItemResourceType.Where (m => m == itemType).FirstOrDefault () == itemType && x.ItemResourceID.Where (m => m == itemID).FirstOrDefault () == itemID).ToList ());
        // listCraft.AddRange (CraftCoreSetting.CraftItemWeaponMagic.Where (x => x.ItemResourceType.Where (m => m == itemType).FirstOrDefault () == itemType && x.ItemResourceID.Where (m => m == itemID).FirstOrDefault () == itemID).ToList ());
        // listCraft.AddRange (CraftCoreSetting.CraftItemDefense.Where (x => x.ItemResourceType.Where (m => m == itemType).FirstOrDefault () == itemType && x.ItemResourceID.Where (m => m == itemID).FirstOrDefault () == itemID).ToList ());
        // //listCraft.AddRange (CraftCoreSetting.CraftItemSets.Where (x => x.ItemResourceType.Where (m => m == itemType).FirstOrDefault () == itemType && x.ItemResourceID.Where (m => m == itemID).FirstOrDefault () == itemID).ToList ());

        // //Trả về text các item có thể chế tạo được
        // var count = listCraft.Count;
        // for (int i = 0; i < count; i++) {
        //     result += GetItemName ((sbyte) listCraft[i].ItemType, (byte) listCraft[i].ItemID) + (i != count - 1 ? ", " : "");
        // }
        // result += "</color>";
        // return result;
        return "";
    }

    /// <summary>
    /// Phân giải item - Dành cho item trang bị
    /// Trả về thông tin các tài nguyên nhận được sau khi phân giải
    /// </summary>
    /// <param name="item"></param>
    public static bool BreakItem (ItemModel item) {
        try {
            CraftModel findItem = null;

            //Vũ khí vật lý
            if (item.ItemType.Equals (0)) {
                findItem = CraftCoreSetting.CraftItemWeaponPhysic.Find (x => x.ItemType == item.ItemType && x.ItemID == item.ItemID);
            }

            //Vũ khí phép thuật
            if (item.ItemType.Equals (1) || item.ItemType.Equals (2) || item.ItemType.Equals (3)) {
                findItem = CraftCoreSetting.CraftItemWeaponMagic.Find (x => x.ItemType == item.ItemType && x.ItemID == item.ItemID);
            }

            //Đồ phòng thủ
            if (item.ItemType >= 4 && item.ItemType <= 8) {
                findItem = CraftCoreSetting.CraftItemDefense.Find (x => x.ItemType == item.ItemType && x.ItemID == item.ItemID);
            }

            //Đạt điều kiện => thực hiện phân giải
            if (findItem != null) {

                GlobalVariables.NotificationText = ""; //String trả về sau phân giải
                int[] itemResource = new int[findItem.ItemResourceID.Length];

                //Tính số lượng tài nguyên nhận dc khi phân giải
                for (int i = 0; i < itemResource.Length; i++) {
                    itemResource[i] = findItem.ItemResourceQuantity[i] * ItemCoreSetting.ItemBreakRate / 100;

                    //Đưa item vào inventory
                    if (itemResource[i] > 0) {
                        GlobalVariables.NotificationText += GetItemName ((sbyte) findItem.ItemResourceType[i], (byte) findItem.ItemResourceID[i]) + " <color=green>x" + itemResource[i] + "</color>;"; //Cộng chuỗi thông báo tên item và số lượng
                        InventorySystem.AddItemToInventory (CreateItem (false, false, 0, (sbyte) findItem.ItemResourceType[i], (byte) findItem.ItemResourceID[i], itemResource[i]));
                    }
                }

                //Xóa item trong inventory nếu có
                InventorySystem.RemoveItem (item); //Xóa khỏi inventory

                return true;
            }

            //Trả về nếu ko thể phân giải
            return false; // "Không thể phân giải vật phẩm này";
        } catch {
            return false; // "Không thể phân giải vật phẩm này";
        }
    }

    #endregion
}