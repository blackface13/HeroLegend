using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class SocketSystem {
    public static readonly float vAtkMax; //Sát thương vật lý
    public static readonly float vMagicMax; //Sát thương phép thuật
    public static readonly float vHealthMax; //Máu
    public static readonly float vManaMax; //Máu
    public static readonly float vArmorMax; //Giáp
    public static readonly float vMagicResistMax; //Kháng phép
    public static readonly float vHealthRegenMax; //Chỉ số hồi máu mỗi giây
    public static readonly float vManaRegenMax; //Chỉ số hồi mana mỗi giây
    public static readonly float vDamageEarthMax; //Sát thương hệ đất
    public static readonly float vDamageWaterMax; //Sát thương hệ nước
    public static readonly float vDamageFireMax; //Sát thương hệ lửa
    public static readonly float vDefenseEarthMax; //Kháng hệ đất
    public static readonly float vDefenseWaterMax; //Kháng hệ nước
    public static readonly float vDefenseFireMax; //Kháng hệ hỏa
    public static readonly sbyte vAtkSpeedMax = 20; //% Tốc độ tấn công cơ bản tăng thêm
    public static readonly sbyte vLifeStealPhysicMax = 10; //% hút máu
    public static readonly sbyte vLifeStealMagicMax = 10; //% hút máu phép
    public static readonly sbyte vLethalityMax = 20; //% Xuyên giáp
    public static readonly sbyte vMagicPenetrationMax = 20; //% Xuyên phép
    public static readonly sbyte vCriticalMax = 20; //% chí mạng
    public static readonly sbyte vTenacityMax = 10; //% kháng hiệu ứng
    public static readonly sbyte vCooldownReductionMax = 10; //% Giảm tgian hồi chiêu
    public static readonly short vAtkPlusMax = 32000; //Gia tăng % sát thương vật lý (max = 32767)
    public static readonly short vMagicPlusMax = 32000; //Gia tăng % sát thương phép (max = 32767)
    public static readonly short vHealthPlusMax = 32000; //Gia tăng % máu (max = 32767)
    public static readonly short vManaPlusMax = 32000; //Gia tăng % mana (max = 32767)
    public static readonly sbyte vDamageExcellentMax = 5; //% Sát thương hoàn hảo (bỏ qua giáp). max = 10%
    public static readonly sbyte vDefenseExcellentMax = 5; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
    public static readonly sbyte vDoubleDamageMax = 5; //Tỉ lệ x2 đòn đánh max = 10%
    public static readonly sbyte vTripleDamageMax = 5; //Tỉ lệ x3 đòn đánh max = 10%
    public static readonly sbyte vDamageReflectMax = 5; //Phản hồi % sát thương. max = 5%
    public static readonly sbyte vRewardPlusMax = 100; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

    //==================================================
    public static List<string> JewelImgCracked;
    /// <summary>
    /// Tạo socket 
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SocketModel CreateSocket (int itemID, float value) {
        //Khởi tạo socket mới
        var model = new SocketModel ();
        model.SocketGuidID = Guid.NewGuid ().ToString ();
        model.SocketID = (sbyte) itemID;
        model.SocketLevel = 1;
        //Lấy tên field
        var fieldName = GetPropertyName (itemID);

        //Set giá trị truyền vào
        if (!string.IsNullOrEmpty (fieldName)) {
            var propInfo = model.GetType ().GetField (fieldName);
            if (propInfo != null) {
                Type t = Nullable.GetUnderlyingType (propInfo.FieldType) ?? propInfo.FieldType;
                object safeValue = Convert.ChangeType (value, t);
                propInfo.SetValue (model, safeValue);
                return model;
            }
        }
        return null;
    }

    /// <summary>
    /// Get tên field khi truyền vào socket item id
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public static string GetPropertyName (int itemID) {
        switch (itemID) {
            case 0:
                return "vAtk";
            case 9:
                return "vMagic";
            case 27:
                return "vAtkSpeed";
            case 15:
                return "vHealth";
            case 6:
                return "vMana";
            case 18:
                return "vArmor";
            case 11:
                return "vMagicResist";
            case 13:
                return "vHealthRegen";
            case 5:
                return "vManaRegen";
            case 31:
                return "vDamageEarth";
            case 26:
                return "vDamageWater";
            case 30:
                return "vDamageFire";
            case 23:
                return "vDefenseEarth";
            case 20:
                return "vDefenseWater";
            case 22:
                return "vDefenseFire";
            case 28:
                return "vLifeStealPhysic";
            case 7:
                return "vLifeStealMagic";
            case 2:
                return "vLethality";
            case 1:
                return "vMagicPenetration";
            case 32:
                return "vCritical";
            case 12:
                return "vTenacity";
            case 19:
                return "vCooldownReduction";
            case 33:
                return "vAtkPlus";
            case 8:
                return "vMagicPlus";
            case 14:
                return "vHealthPlus";
            case 4:
                return "vManaPlus";
            case 17:
                return "vArmorPlus";
            case 10:
                return "vMagicResistPlus";
            case 29:
                return "vDamageExcellent";
            case 21:
                return "vDefenseExcellent";
            case 25:
                return "vDoubleDamage";
            case 24:
                return "vTripleDamage";
            case 3:
                return "vDamageReflect";
            case 16:
                return "vRewardPlus";
            default:
                return "";
        }
    }

    /// <summary>
    /// Trả về tên giá trị khi truyền id item socket
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public static string GetNameValue (int itemID) {
        switch (itemID) {
            case 0:
                return Languages.lang[700];
            case 9:
                return Languages.lang[701];
            case 27:
                return Languages.lang[714];
            case 15:
                return Languages.lang[702];
            case 6:
                return Languages.lang[703];
            case 18:
                return Languages.lang[704];
            case 11:
                return Languages.lang[705];
            case 13:
                return Languages.lang[706];
            case 5:
                return Languages.lang[707];
            case 31:
                return Languages.lang[708];
            case 26:
                return Languages.lang[709];
            case 30:
                return Languages.lang[710];
            case 23:
                return Languages.lang[711];
            case 20:
                return Languages.lang[712];
            case 22:
                return Languages.lang[713];
            case 28:
                return Languages.lang[715];
            case 7:
                return Languages.lang[716];
            case 2:
                return Languages.lang[717];
            case 1:
                return Languages.lang[718];
            case 32:
                return Languages.lang[719];
            case 12:
                return Languages.lang[720];
            case 19:
                return Languages.lang[721];
            case 33:
                return Languages.lang[722];
            case 8:
                return Languages.lang[723];
            case 14:
                return Languages.lang[724];
            case 4:
                return Languages.lang[725];
            case 17:
                return Languages.lang[734];
            case 10:
                return Languages.lang[735];
            case 29:
                return Languages.lang[726];
            case 21:
                return Languages.lang[727];
            case 25:
                return Languages.lang[728];
            case 24:
                return Languages.lang[729];
            case 3:
                return Languages.lang[730];
            case 16:
                return Languages.lang[731];
            default:
                return "";
        }
    }

    /// <summary>
    /// Lấy giá trị khảm ngọc
    /// </summary>
    /// <param name="item">Item truyền vào</param>
    /// <param name="obj">Biến giá trị cần lấy</param>
    /// <returns></returns>
    public static float GetValueSocket (ItemModel item, ItemModel itemSocket) {
        switch (GetPropertyName(itemSocket.ItemID)) {
            case "vAtk": //Tấn công
                return UnityEngine.Random.Range ((float) (10 * (item.ItemLevel + 1)), (float) (10 * ((item.ItemLevel + 1) + 1)));
            case "vMagic": //Phép thuật
                return UnityEngine.Random.Range ((float) (10 * (item.ItemLevel + 1)), (float) (10 * ((item.ItemLevel + 1) + 1)));
            case "vHealth": //Máu
                return UnityEngine.Random.Range ((float) (100 * (item.ItemLevel + 1)), (float) (100 * ((item.ItemLevel + 1) + 1)));
            case "vMana": //Mana
                return UnityEngine.Random.Range ((float) (100 * (item.ItemLevel + 1)), (float) (100 * ((item.ItemLevel + 1) + 1)));
            case "vArmor": //Giáp
                return UnityEngine.Random.Range ((float) (10 * (item.ItemLevel + 1)), (float) (10 * ((item.ItemLevel + 1) + 1)));
            case "vMagicResist": //Kháng phép
                return UnityEngine.Random.Range ((float) (8 * (item.ItemLevel + 1)), (float) (8 * ((item.ItemLevel + 1) + 1)));
            case "vHealthRegen": //Hồi máu
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (2 * (item.ItemLevel + 1)));
            case "vManaRegen": //Hồi mana
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (2 * (item.ItemLevel + 1)));
            case "vDamageEarth": //Sát thương hệ đất
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vDamageWater": //Sát thương hệ nước
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vDamageFire": //Sát thương hệ lửa
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vDefenseEarth": //Kháng Sát thương hệ đất
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vDefenseWater": //Kháng Sát thương hệ nước
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vDefenseFire": //Kháng Sát thương hệ lửa
                return UnityEngine.Random.Range ((float) (0 * (item.ItemLevel + 1)) + 0.1f, (float) (5 * (item.ItemLevel + 1)));
            case "vAtkSpeed": //Tốc độ tấn công cơ bản tăng thêm
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vLifeStealPhysic": //Hút máu(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vLifeStealMagic": //Hút máu phép(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vLethality": //Xuyên giáp(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vMagicPenetration": //Xuyên phép(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vCritical": //Chí mạng(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vTenacity": //Kháng hiệu ứng(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vCooldownReduction": //Giảm tgia(float)n hồi chiêu(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vAtkPlus": //Gia tăng % sát thươ(float)ng vật lý(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vMagicPlus": //Gia tăng % sát th(float)ương phép(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vHealthPlus": //Gia tăng % máu(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vManaPlus": //Gia tăng % mana(float)(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vDamageExcellent": //Sát thương (float)hoàn hảo(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vDefenseExcellent": //phong thu (float)hoàn hảo(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vDoubleDamage": //Tỉ lệ x2 đòn đ(float)ánh max (float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vTripleDamage": //Tỉ lệ x3 đòn đ(float)ánh(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(2 * (item.ItemLevel + 1)));
            case "vDamageReflect": //Phản hồi % sá(float)t thương(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(1 * (item.ItemLevel + 1)));
            case "vRewardPlus": //Tăng lượng vàng (float)rơi ra vào cuối trận(float)
                return  UnityEngine.Random.Range ((float)(0 * (item.ItemLevel + 1)), (float)(4 * (item.ItemLevel + 1)));

            default:
                break;
        }
        return 0;
    }

    /// <summary>
    /// Trả về hình ảnh bị vỡ của loại ngọc
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public static string GetImgJewelCracked (int itemID) {
        if (JewelImgCracked == null) {
            CreateListJewelImgCracked ();
        }
        if (JewelImgCracked.Count <= 0) {
            CreateListJewelImgCracked ();
        }
        return JewelImgCracked[itemID];
    }

    /// <summary>
    /// Khởi tạo danh sách hình ảnh nứt vỡ ngọc
    /// </summary>
    static void CreateListJewelImgCracked () {

        JewelImgCracked = new List<string> ();
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("2-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("3-cracked");
        JewelImgCracked.Add ("4-cracked");
        JewelImgCracked.Add ("4-cracked");
        JewelImgCracked.Add ("4-cracked");
        JewelImgCracked.Add ("3-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("3-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("1-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("5-cracked");
        JewelImgCracked.Add ("0-cracked");
        JewelImgCracked.Add ("0-cracked");
    }
}