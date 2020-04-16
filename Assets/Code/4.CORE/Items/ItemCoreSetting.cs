using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCoreSetting {

    #region Một số item đặc biệt 

    //Khuôn socket
    public static int CreateSocketItemType = 10;
    public static int CreateSocketItemID = 26;

    #endregion
    public static int ItemBreakRate = 30; //Tỉ lệ tài nguyên nhận đc sau phân giải. 1 = 1%

    #region Setting giới hạn cho các chỉ số item 
    public static readonly sbyte SecondHeathRegen = 5; //Số giây hồi máu (tức là giá trị máu sẽ hồi trong bao nhiêu giây, mặc định = 5)
    public static sbyte UpgradePerLevel = 10; //% Sức mạnh giá trị tăng thêm sau mỗi lần cường hóa
    public static sbyte UpgradePerColor = 10; //% Sức mạnh giá trị tăng thêm sau mỗi lần nâng phẩm
    public static sbyte ItemTypeMax = 127; //Loại item (giới hạn -128 -> 127)
    public static byte ItemIDMax = 255; //ID item (giới hạn 0 -> 255)
    public static sbyte ItemLevelMax = 10; //Cấp độ item
    public static sbyte ItemColorMax = 7; //Màu sắc item
    public static int QuantityMax = 2000000000;
    // public static float vAtkMax; //Sát thương vật lý
    // public static float vMagicMax; //Sát thương phép thuật
    // public static float vHealthMax; //Máu
    // public static float vManaMax; //Máu
    // public static float vArmorMax; //Giáp
    // public static float vMagicResistMax; //Kháng phép
    // public static float vHealthRegenMax; //Chỉ số hồi máu mỗi giây
    // public static float vManaRegenMax; //Chỉ số hồi mana mỗi giây
    // public static float vDamageEarthMax; //Sát thương hệ đất
    // public static float vDamageWaterMax; //Sát thương hệ nước
    // public static float vDamageFireMax; //Sát thương hệ lửa
    // public static float vDefenseEarthMax;//Kháng hệ đất
    // public static float vDefenseWaterMax;//Kháng hệ nước
    // public static float vDefenseFireMax;//Kháng hệ hỏa
    public static sbyte vAtkSpeedMax = 20; //% Tốc độ tấn công cơ bản tăng thêm
    public static sbyte vLifeStealPhysicMax = 10; //% hút máu
    public static sbyte vLifeStealMagicMax = 10; //% hút máu phép
    public static sbyte vLethalityMax = 20; //% Xuyên giáp
    public static sbyte vMagicPenetrationMax = 20; //% Xuyên phép
    public static sbyte vCriticalMax = 20; //% chí mạng
    public static sbyte vTenacityMax = 10; //% kháng hiệu ứng
    public static sbyte vCooldownReductionMax = 10; //% Giảm tgian hồi chiêu
    public static short vAtkPlusMax = 32000; //Gia tăng % sát thương vật lý (max = 32767)
    public static short vMagicPlusMax = 32000; //Gia tăng % sát thương phép (max = 32767)
    public static short vHealthPlusMax = 32000; //Gia tăng % máu (max = 32767)
    public static short vManaPlusMax = 32000; //Gia tăng % mana (max = 32767)
    public static short vArmorPlusMax = 32000; //Gia tăng % Giáp (max = 32767)
    public static short vMagicResistPlus = 32000; //Gia tăng % Kháng phép (max = 32767)
    public static sbyte vDamageExcellentMax = 5; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
    public static sbyte vDefenseExcellentMax = 5; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
    public static sbyte vDoubleDamageMax = 5; //Tỉ lệ x2 đòn đánh max = 10%
    public static sbyte vTripleDamageMax = 5; //Tỉ lệ x3 đòn đánh max = 10%
    public static sbyte vDamageReflectMax = 5; //Phản hồi % sát thương. max = 5%
    public static sbyte vRewardPlusMax = 100; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
    public static sbyte vSocketSlotMax = 6; //Số lượng slot socket
    public static string[] ItemUpgradeLevel = new string[10] { "10,9,5;", "10,10,5;", "10,11,5;", "10,12,5;", "10,13,5;", "10,14,5;", "10,15,5;", "10,16,5;", "10,17,5;", "10,18,5;" }; //Nâng cấp item cần gì. A,B,C. A: item type, B: itemID, C: số lượng
    public static string[] ItemUpgradeColor = new string[6] { "10,19,1;", "10,20,1;", "10,21,1;", "10,22,1;", "10,23,1;", "10,24,1;" }; //Nâng phẩm item cần gì. A,B,C. A: item type, B: itemID, C: số lượng

    #endregion
    //public static List<SocketModel> SocketsMax;
    #region Tỉ lệ chế tạo đồ xuất hiện dòng thuộc tính 

    public static sbyte[] CreateHaveSocketRate = new sbyte[6] { 30, 25, 20, 15, 10, 5 }; //Tỉ lệ chế tạo item có socket 
    public static sbyte vHealthRegenRate = 30; //Chỉ số hồi máu
    public static sbyte vManaRegenRate = 30; //Chỉ số hồi mana
    public static sbyte vDamageEarthRate = 30; //Sát thương hệ đất
    public static sbyte vDamageWaterRate = 30; //Sát thương hệ nước
    public static sbyte vDamageFireRate = 30; //Sát thương hệ lửa
    public static sbyte vDefenseEarthRate = 30; //Kháng Sát thương hệ đất
    public static sbyte vDefenseWaterRate = 30; //Kháng Sát thương hệ nước
    public static sbyte vDefenseFireRate = 30; //Kháng Sát thương hệ lửa
    public static sbyte vAtkSpeedRate = 20; //% Tốc độ tấn công cơ bản tăng thêm
    public static sbyte vLifeStealPhysicRate = 30; //% hút máu
    public static sbyte vLifeStealMagicRate = 30; //% hút máu phép
    public static sbyte vLethalityRate = 20; //% Xuyên giáp
    public static sbyte vMagicPenetrationRate = 20; //% Xuyên phép
    public static sbyte vCriticalRate = 30; //% chí mạng
    public static sbyte vTenacityRate = 30; //% kháng hiệu ứng
    public static sbyte vCooldownReductionRate = 20; //% Giảm tgian hồi chiêu
    public static sbyte vAtkPlusRate = 20; //Gia tăng % sát thương vật lý (max = 32767)
    public static sbyte vMagicPlusRate = 20; //Gia tăng % sát thương phép (max = 32767)
    public static sbyte vHealthPlusRate = 30; //Gia tăng % máu (max = 32767)
    public static sbyte vManaPlusRate = 30; //Gia tăng % mana (max = 32767)
    public static sbyte vArmorPlusRate = 30;
    public static sbyte vMagicResistPlusRate = 30;
    public static sbyte vDamageExcellentRate = 10; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
    public static sbyte vDefenseExcellentRate = 10; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
    public static sbyte vDoubleDamageRate = 10; //Tỉ lệ x2 đòn đánh max = 10%
    public static sbyte vTripleDamageRate = 10; //Tỉ lệ x3 đòn đánh max = 10%
    public static sbyte vDamageReflectRate = 10; //Phản hồi % sát thương. max = 5%
    public static sbyte vRewardPlusRate = 30; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

    #endregion

    #region Giá bán cho mỗi chỉ số 

    public static short ItemLevelPrice = 234;
    public static short ItemColorPrice = 567;
    public static short vAtkPrice = 6;
    public static short vMagicPrice = 7;
    public static short vAtkSpeedPrice = 10;
    public static short vHealthPrice = 3;
    public static short vManaPrice = 2;
    public static short vArmorPrice = 4;
    public static short vMagicResistPrice = 5;
    public static short vHealthRegenPrice = 11;
    public static short vManaRegenPrice = 9;
    public static short vDamageEarthPrice = 12;
    public static short vDamageWaterPrice = 12;
    public static short vDamageFirePrice = 12;
    public static short vDefenseEarthPrice = 12;
    public static short vDefenseWaterPrice = 12;
    public static short vDefenseFirePrice = 12;
    public static short vLifeStealPhysicPrice = 16;
    public static short vLifeStealMagicPrice = 18;
    public static short vLethalityPrice = 21;
    public static short vMagicPenetrationPrice = 22;
    public static short vCriticalPrice = 17;
    public static short vTenacityPrice = 15;
    public static short vCooldownReductionPrice = 12;
    public static short vAtkPlusPrice = 22;
    public static short vMagicPlusPrice = 23;
    public static short vHealthPlusPrice = 16;
    public static short vManaPlusPrice = 15;
    public static short vArmorPlusPrice = 19;
    public static short vMagicResistPlusPrice = 20;
    public static short vDamageExcellentPrice = 34;
    public static short vDefenseExcellentPrice = 39;
    public static short vDoubleDamagePrice = 45;
    public static short vTripleDamagePrice = 91;
    public static short vDamageReflectPrice = 125;
    public static short vRewardPlusPrice = 33;
    public static short vSocketSlotPrice = 159;

    #endregion
}