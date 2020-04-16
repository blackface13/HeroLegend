using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Items
{
    public ItemType Type;
    public int ID;
    public string Name;
    public string Descriptions;
    public Sprite Icon;
    public int Level;//Cấp độ trang bị
    public int Color;//Chỉ số màu sắc của trang bị (chỉ dành cho trang bị)
    public int Quantity;//Số lượng item
    public int Price;//Giá bán
    public float Atk;//Chỉ số tấn công vật lý
    public float Magic;//Sức mạnh phép thuật
    public float BuffAtk;//Phần trăm xuyên giáp
    public float BuffMagic;//Phần trăm xuyên phép
    public float GetHPAtk;//Hút máu vật lý
    public float GetHPMagic;//Hút máu phép
    public float Critical;//Chí mạng
    public float HP;//Máu
    public float ReHP;//Chỉ số hồi máu mỗi 5s
    public float DefP;//Giáp
    public float DefM;//Kháng phép
    public float DefState;//Kháng hiệu ứng
    public int Special;//Hiệu ứng đặc biệt (thiết kế riêng)
    public enum ItemType
    {
        equip,//Item trang bị
        use,//Item sử dụng
        quest,//Item nhiệm vụ
        inschar//Mảnh ghép tướng
    }
    public Items(ItemType type, int id, string name, string descriptions, int level, int color, int quantity, int price, float atk, float magic, float buffatk, float buffmagic, float gethpatk, float gethpmagic,
        float crit, float hp, float rehp, float defp, float defm, float defstate, int special)
    {
        Type = type;
        ID = id;
        //Icon = Resources.Load<Sprite>("Images/Items/" + ID);
        Name = name;
        Descriptions = descriptions;
        Level = level;
        Color = color;
        Quantity = quantity;
        Price = price;
        Atk = atk;
        Magic = magic;
        BuffAtk = buffatk;
        BuffMagic = buffmagic;
        GetHPMagic = gethpmagic;
        Critical = crit;
        HP = hp;
        ReHP = rehp;
        DefP = defp;
        DefM = defm;
        DefState = defstate;
        Special = special;
    }
    public Items(){}
    
    /// <summary>
    /// Hàm clone một item
    /// </summary>
    /// <returns></returns>
    public Items Clone()
    {
        return (Items)this.MemberwiseClone();
    }
}
