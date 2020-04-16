using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HeroesProperties {

    public int ID;
    public string Name;
    public string Description;
    public string Intrinsic; //Nội tại
    public string SkillDescription; //
    public int Level;
    public float EXP;
    public float vHealth; //Máu
    public float vHealthCurrent;
    public float vMana; //Mana
    public float vManaCurrent;
    public float vAtk; //Sát thương vật lý
    public float vMagic; //Sát thương phép thuật
    public float vArmor; //Giáp
    public float vMagicResist; //kháng phép
    public float vHealthRegen; //Hồi máu mỗi giây
    public float vManaRegen; //Hồi năng lượng mỗi giây
    public float vDamageEarth; //Sát thương hệ đất
    public float vDamageWater; //Sát thương hệ nước
    public float vDamageFire; //Sát thương hệ lửa
    public float vDefenseEarth; //Kháng hệ đất
    public float vDefenseWater; //Kháng hệ nước
    public float vDefenseFire; //Kháng hệ hỏa
    public float vAtkSpeed; //This is speed anim. Default = 1.0f
    public float vLifeStealPhysic; //% hút máu
    public float vLifeStealMagic; //% hút máu phép
    public float vLethality; //% Xuyên giáp
    public float vMagicPenetration; //% Xuyên phép
    public float vCritical; //% chí mạng
    public float vTenacity; //% kháng hiệu ứng
    public float vCooldownReduction; //Thời gian hồi chiêu
    public float vDamageExcellent; //Sát thương hoàn hảo
    public float vDefenseExcellent; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
    public float vDoubleDamage; //Tỉ lệ x2 đòn đánh max = 10%
    public float vTripleDamage; //Tỉ lệ x3 đòn đánh max = 10%
    public float vDamageReflect; //Phản hồi % sát thương. max = 5%
    public float vRewardPlus; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%
    public float[] vCooldown;//Thời gian hồi đánh thường và chiêu thức
    public float[] Mana_skill;
    public float vHealthPerLevel;
    public float vAtkPerLevel;
    public float vMagicPerLevel;
    public float vArmorPerLevel;
    public float vMagicResistPerLevel;
    public float vHealthRegenPerLevel;
    public float vManaRegenPerLevel;
    public float vCooldownReductionPerLevel;
    public List<ItemModel> ItemsEquip;
    public enum HeroType {
        near, //Cận chiến
        far //Tầm xa
    }
    public HeroType HType;
    public player_type Type;
    public enum player_type {
        canchien = 1,
        satthu = 2,
        hotro = 3,
        dodon = 4,
        xathu = 5,
        phapsu = 6
    }
    public HeroesProperties (int id,
        string name,
        string description,
        string intrinsic,
        string skilldescription,
        int level,
        float exp,
        float vhealth,
        float vmana,
        float vatk,
        float vmagic,
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
        sbyte vdamageexcellent,
        sbyte vdefenseexcellent,
        sbyte vdoubledamage,
        sbyte vtripledamage,
        sbyte vdamagereflect,
        sbyte vrewardplus,
        float[] vcooldown,
        float[] mana_skill,
        float vhealthperlevel,
        float vatkperlevel,
        float vmagicperlevel,
        float varmorperlevel,
        float vmagicresistperlevel,
        float vhealthregenperlevel,
        float vmanaregenperlevel,
        float vcooldownreductionperlevel,
        player_type type, 
        HeroType htype) {
        ID = id;
        Name = name;
        Description = description;
        Intrinsic = intrinsic;
        SkillDescription = skilldescription;
        Level = level;
        EXP = exp;
        vHealth = vhealth;
        vMana = vmana;
        vAtk = vatk;
        vMagic = vmagic;
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
        vAtkSpeed = vatkspeed;
        vLifeStealPhysic = vlifestealphysic;
        vLifeStealMagic = vlifestealmagic;
        vLethality = vlethality;
        vMagicPenetration = vmagicpenetration;
        vCritical = vcritical;
        vTenacity = vtenacity;
        vCooldownReduction = vcooldownreduction;
        vDamageExcellent = vdamageexcellent;
        vDefenseExcellent = vdefenseexcellent;
        vDoubleDamage = vdoubledamage;
        vTripleDamage = vtripledamage;
        vDamageReflect = vdamagereflect;
        vRewardPlus = vrewardplus;
        vCooldown = vcooldown;
        Mana_skill = mana_skill;
        vHealthPerLevel = vhealthperlevel;
        vAtkPerLevel = vatkperlevel;
        vMagicPerLevel = vmagicperlevel;
        vArmorPerLevel = varmorperlevel;
        vMagicResistPerLevel = vmagicresistperlevel;
        vHealthRegenPerLevel = vhealthregenperlevel;
        vManaRegenPerLevel = vmanaregenperlevel;
        vCooldownReductionPerLevel = vcooldownreductionperlevel;

        Type = type;
        HType = htype;
    }
    public HeroesProperties () { }
    public HeroesProperties Clone () {
        return (HeroesProperties) this.MemberwiseClone ();
    }
}