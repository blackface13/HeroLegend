﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Assets.Code._4.CORE;

public class TestMoveObject : MonoBehaviour
{
    //SQLiteCore LiteCore;
    //IDataReader reader;
    //private void Start()
    //{
    //    LiteCore = new SQLiteCore();
    //    reader = LiteCore._getEnemy();
    //    DatabaseEnemy enedb = new DatabaseEnemy();
    //    while (reader.Read())
    //    {
    //        SystemProperties prop = new SystemProperties();
    //        prop.ID = Convert.ToInt32(reader[0]);
    //        prop.Name = reader[1].ToString();
    //        prop.Description = reader[2].ToString();
    //        prop.Level = Convert.ToInt32(reader[3]);
    //        prop.EXP = Convert.ToSingle(reader[4]);
    //        prop.HP_total = Convert.ToSingle(reader[5]);
    //        prop.HP_curent = Convert.ToSingle(reader[5]);
    //        prop.MP_total = Convert.ToSingle(reader[6]);
    //        prop.Atk_physic = Convert.ToSingle(reader[7]);
    //        prop.Atk_magic = Convert.ToSingle(reader[8]);
    //        prop.Def_physic = Convert.ToSingle(reader[9]);
    //        prop.Def_magic = Convert.ToSingle(reader[10]);
    //        prop.Get_hp_physic = Convert.ToSingle(reader[11]);
    //        prop.Get_hp_magic = Convert.ToSingle(reader[12]);
    //        prop.Pass_def_physic = Convert.ToSingle(reader[13]);
    //        prop.Pass_def_magic = Convert.ToSingle(reader[14]);
    //        prop.Re_hp = Convert.ToSingle(reader[15]);
    //        prop.Re_mp = Convert.ToSingle(reader[16]);
    //        prop.Crit = Convert.ToSingle(reader[17]);
    //        prop.Alow_effect = Convert.ToSingle(reader[18]);
    //        prop.Buff_effect = Convert.ToSingle(reader[19]);
    //        prop.Speed_atk_player = Convert.ToSingle(reader[20]);
    //        prop.Time_respawn_skill = reader[21].ToString().Split(';');//This is string, convert to float late
    //        prop.Mana_skill = Convert.ToSingle(reader[22]);
    //        prop.HP_per_level = Convert.ToSingle(reader[23]);
    //        prop.ATK_physic_per_level = Convert.ToSingle(reader[24]);
    //        prop.ATK_magic_per_level = Convert.ToSingle(reader[25]);
    //        prop.Def_physic_per_level = Convert.ToSingle(reader[26]);
    //        prop.Def_magic_per_level = Convert.ToSingle(reader[27]);
    //        prop.Re_HP_per_level = Convert.ToSingle(reader[28]);
    //        prop.Re_MP_per_level = Convert.ToSingle(reader[29]);
    //        prop.ATK_speed_per_level = Convert.ToSingle(reader[30]);
    //        prop.Time_respawn_skill_per_level = Convert.ToSingle(reader[31]);
    //        if ((int)(SystemProperties.player_type.canchien) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.canchien;
    //        if ((int)(SystemProperties.player_type.danhxa) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.danhxa;
    //        if ((int)(SystemProperties.player_type.dodon) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.dodon;
    //        if ((int)(SystemProperties.player_type.hotro) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.hotro;
    //        if ((int)(SystemProperties.player_type.phapsu) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.phapsu;
    //        if ((int)(SystemProperties.player_type.satthu) == Convert.ToInt32(reader[32]))
    //            prop.Type = SystemProperties.player_type.satthu;
    //        if ((int)(SystemProperties.player_sect.earth) == Convert.ToInt32(reader[33]))
    //            prop.Sect = SystemProperties.player_sect.earth;
    //        if ((int)(SystemProperties.player_sect.fire) == Convert.ToInt32(reader[33]))
    //            prop.Sect = SystemProperties.player_sect.fire;
    //        if ((int)(SystemProperties.player_sect.water) == Convert.ToInt32(reader[33]))
    //            prop.Sect = SystemProperties.player_sect.water;
    //        if ((int)(SystemProperties.player_sect.wind) == Convert.ToInt32(reader[33]))
    //            prop.Sect = SystemProperties.player_sect.wind;
    //        prop.Live = true;
    //        enedb.DBEnemy.Add(prop);
    //    }
    //    GUIUtility.systemCopyBuffer = Securitys.Encrypt(JsonUtility.ToJson(enedb));
    //}
}
