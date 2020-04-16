using UnityEngine;
[System.Serializable]

public class SocketModel {
    public string SocketGuidID; //Mã socket
    //public sbyte SocketType; //Loại socket, đất lửa nước...
    public sbyte SocketID; //ID socket
    public sbyte SocketLevel; //Cấp độ socket
    public float vAtk; //Sát thương vật lý
    public float vMagic; //Sát thương phép thuật
    public float vHealth; //Máu
    public float vMana; //Mana
    public float vArmor; //Giáp
    public float vMagicResist; //Kháng phép
    public float vHealthRegen; //Chỉ số hồi máu mỗi giây
    public float vManaRegen; //Chỉ số hồi mana mỗi giây
    public float vDamageEarth; //Sát thương hệ đất
    public float vDamageWater; //Sát thương hệ nước
    public float vDamageFire; //Sát thương hệ lửa
    public float vDefenseEarth; //Kháng hệ đất
    public float vDefenseWater; //Kháng hệ nước
    public float vDefenseFire; //Kháng hệ hỏa
    public float vAtkSpeed; //% Tốc độ tấn công cơ bản tăng thêm
    public float vLifeStealPhysic; //% hút máu
    public float vLifeStealMagic; //% hút máu phép
    public float vLethality; //% Xuyên giáp
    public float vMagicPenetration; //% Xuyên phép
    public float vCritical; //% chí mạng
    public float vTenacity; //% kháng hiệu ứng
    public float vCooldownReduction; //% Giảm tgian hồi chiêu
    public float vAtkPlus; //Gia tăng % sát thương vật lý (max = 32767)
    public float vMagicPlus; //Gia tăng % sát thương phép (max = 32767)
    public float vHealthPlus; //Gia tăng % máu (max = 32767)
    public float vManaPlus; //Gia tăng % mana (max = 32767)
    public float vArmorPlus; //Gia tăng % Giáp
    public float vMagicResistPlus; //Gia tăng % Kháng phép
    public float vDamageExcellent; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
    public float vDefenseExcellent; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
    public float vDoubleDamage; //Tỉ lệ x2 đòn đánh max = 10%
    public float vTripleDamage; //Tỉ lệ x3 đòn đánh max = 10%
    public float vDamageReflect; //Phản hồi % sát thương. max = 5%
    public float vRewardPlus; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
    public SocketModel () { }
    public SocketModel (string socketguidid, 
    //sbyte sockettype, 
    sbyte socketid, sbyte socketlevel, float vatk,
        float vmagic,
        float vhealth,
        float vmana,
        float varmor,
        float vmagicresist,
        float vhealthregen,
        float vmanaregen,
        float vdamageearth,
        float vdamagewater,
        float vdamagefire,
        float vdefenseearth,
        float vdefensewater,
        float vdefensefire,
        float vatkspeed,
        float vlifestealphysic,
        float vlifestealmagic,
        float vlethality,
        float vmagicpenetration,
        float vcritical,
        float vtenacity,
        float vcooldownreduction,
        float vatkplus,
        float vmagicplus,
        float vhealthplus,
        float vmanaplus,
        float varmorplus,
        float vmagicresistplus,
        float vdamageexcellent,
        float vdefenseexcellent,
        float vdoubledamage,
        float vtripledamage,
        float vdamagereflect,
        float vrewardplus) {
        SocketGuidID = socketguidid;
        //SocketType = sockettype;
        SocketID = socketid;
        SocketLevel = socketlevel;
        vAtk = vatk;
        vMagic = vmagic;
        vAtkSpeed = vatkspeed;
        vHealth = vhealth;
        vMana = vmana;
        vArmor = varmor;
        vMagicResist = vmagicresist;
        vHealthRegen = vhealthregen;
        vManaRegen = vmanaregen;
        vDamageEarth = vdamageearth;
        vDamageWater = vdamagewater;
        vDamageFire = vdamagefire;
        vDefenseEarth = vdefenseearth;
        vDefenseWater = vdefensewater;
        vDefenseFire = vdefensefire;
        vLifeStealPhysic = vlifestealphysic;
        vLifeStealMagic = vlifestealmagic;
        vLethality = vlethality;
        vMagicPenetration = vmagicpenetration;
        vCritical = vcritical;
        vTenacity = vtenacity;
        vCooldownReduction = vcooldownreduction;
        vAtkPlus = vatkplus;
        vMagicPlus = vmagicplus;
        vHealthPlus = vhealthplus;
        vManaPlus = vmanaplus;
        vArmorPlus = varmorplus;
        vMagicResistPlus = vmagicresistplus;
        vDamageExcellent = vdamageexcellent;
        vDefenseExcellent = vdefenseexcellent;
        vDoubleDamage = vdoubledamage;
        vTripleDamage = vtripledamage;
        vDamageReflect = vdamagereflect;
        vRewardPlus = vrewardplus;
    }
    
    
    /// <summary>
    /// Hàm clone một item
    /// </summary>
    /// <returns></returns>
    public SocketModel Clone()
    {
        return (SocketModel)this.MemberwiseClone();
    }
}