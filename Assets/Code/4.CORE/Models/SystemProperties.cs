using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SystemProperties {

    public int ID;
    public string Name;
    public string Description;
    public int Level;
    public double EXP;
    public float HP_total;
    public float HP_curent;
    public float MP_total;
    public float MP_curent;
    public float Atk_physic;
    public float Atk_magic;
    public float Def_physic;
    public float Def_magic;
    public float Get_hp_physic;
    public float Get_hp_magic;
    public float Pass_def_physic;
    public float Pass_def_magic;
    public float Re_hp;
    public float Re_mp;
    public float Crit;
    public float Alow_effect;
    public float Buff_effect;
    public float Speed_atk_player;//This is speed anim. Default = 1.0f
    public string[] Time_respawn_skill;//Thời gian hồi chiêu
    public float[] Time_respawn_temp;
    public float Mana_skill;
    public float HP_per_level;
    public float ATK_physic_per_level;
    public float ATK_magic_per_level;
    public float Def_physic_per_level;
    public float Def_magic_per_level;
    public float Re_HP_per_level;
    public float Re_MP_per_level;
    public float ATK_speed_per_level;
    public float Time_respawn_skill_per_level;
    public bool Live = true;
    public bool Re_HPMP = false;
    public bool[] State = new bool[16];
    public float[] MPSkill;//Thể lực tiêu tốn của skill
    public player_sect Sect;
    public enum player_sect
    {
        wind = 1,//gió
        earth = 2,//đất
        water = 3,//nước
        fire = 4//lửa
    }
    public player_type Type;
    public enum player_type
    {
        canchien = 1,
        satthu = 2,
        hotro = 3,
        dodon = 4,
        danhxa = 5,
        phapsu = 6
    }
    public SystemProperties(int id, string name, string description, int level, double exp, float hp, float mp, float atk_physic, float atk_magic, float def_physic, float def_magic,
        float get_hp_physic, float get_hp_magic, float pass_def_physic, float pass_def_magic, float re_hp, float re_mp, float crit, float alow_effect, float buff_effect,
        float speed_atk, string[] time_respawn_skill, float mana_skill, //Time skill
        float hp_per_level, float atk_physic_per_level, float atk_magic_per_level, float def_physic_per_level, float def_magic_per_level,
        float re_hp_per_level, float re_mp_per_level, float atk_speed_per_level, float time_respawn_skill_per_level,
        player_type type, player_sect sect)
    {
        ID = id;
        Name = name;//Tên
        Description = description;//Thông tin
        Level = level;
        EXP = exp;
        HP_total = hp;//Máu
        HP_curent = hp;
        MP_total = mp;//Mana
        MP_curent = mp;
        Atk_physic = atk_physic;//Sát thương vật lý
        Atk_magic = atk_magic;//Sát thương phép
        Def_physic = def_physic;//Thủ vật lý
        Def_magic = def_magic;//Thủ phép
        Get_hp_physic = get_hp_physic;//Hút máu
        Get_hp_magic = get_hp_magic;//Hut máu phép
        Pass_def_physic = pass_def_physic;//Xuyên giáp
        Pass_def_magic = pass_def_magic;//Xuyên phép
        Re_hp = re_hp;//Hồi máu
        Re_mp = re_mp;//Hồi mana
        Crit = crit;
        Alow_effect = alow_effect;
        Buff_effect = buff_effect;
        Speed_atk_player = speed_atk;//Tốc độ đánh
        Time_respawn_skill = time_respawn_skill;//Giảm time hồi chiêu
        Mana_skill = mana_skill;
        //Values per level (Tăng chỉ số theo mỗi cấp độ)
        HP_per_level = hp_per_level;
        ATK_physic_per_level = atk_physic_per_level;
        ATK_magic_per_level = atk_magic_per_level;
        Def_physic_per_level = def_physic_per_level;
        Def_magic_per_level = def_magic_per_level;
        Re_HP_per_level = re_hp_per_level;
        Re_MP_per_level = re_mp_per_level;
        ATK_speed_per_level = atk_speed_per_level;
        Time_respawn_skill_per_level = time_respawn_skill_per_level;//Giảm time hồi chiêu mỗi cấp, tính theo %
        Type = type;
        Sect = sect;
    }
    public SystemProperties()
    { }
}
