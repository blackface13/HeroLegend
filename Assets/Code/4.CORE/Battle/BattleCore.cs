using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackCore {
    public static class BattleCore {
        #region Misc (Một số nội tại của hero cần sử dụng tới các biến tĩnh)
        //Nội tại hero 5: Khi hỗ trợ hoặc hạ gục đối phương, hồi lại 20% máu đã tổn thất
        public static bool Hero5IntrinsicEnable = false;
        public static int? Hero5IntrinsicTeam = null;
        //--------------------------------------------------------------
        //Nội tại hero 8: Khi hỗ trợ hoặc hạ gục đối phương, tốc độ đánh sẽ được tăng gấp đôi trong 5 giây
        public static bool Hero8IntrinsicEnable = false;
        public static int? Hero8IntrinsicTeam = null;
        //--------------------------------------------------------------
        #endregion
        public static float[] BattleSpeed = new float[] { 1f, 1.5f, 2f };
        public static readonly float DefaultAnAnimationTime = 0.5f; //chỉ số này phải < 1. Thời gian thực hiện 1 đòn đánh thường của hero, mọi hero đều phải thiết kế theo thời gian này
        public static int DamageObjectQuantity = 50; //Số lượng object damage được tạo khi vào game
        public static float TimeDelayComboNormalAtk = 5f; //Thời gian ngắt combo của normal atk khi không đánh trong 1 khoảng
        public static string HeroObjectLink = "Prefabs/Hero/"; //Đường dẫn tới object hero
        public static string HeroObjectAvtLink = "Prefabs/HeroAvt/"; //Đường dẫn tới object hero
        public static string HeroSkillObjectLink = "Prefabs/HeroSkill/"; //Đường dẫn tới object hero
        public static string StatusObjectLink = "Prefabs/Status/"; //Đường dẫn tới object hero
        public static string MapObjectLink = "Prefabs/Maps/"; //Đường dẫn tới object hero
        public static float SpeedMove = 50f; //Tốc độ di chuyển của hero nhảy tới đối phương và di chuyển về
        public static float DefaultVectorY = -3f; //Vị trí Y mặc định của các hero
        public static bool UserVitualCamera; //Cho phép sử dụng vitual camera hay ko
        public static float MaxEnergyTeam1 = 1000f; //Chỉ số thể lực tối đa của team. = tổng số mp của các hero hiện tại xuất hiện trong battle
        public static float MaxEnergyTeam2 = 1000f; //Chỉ số thể lực tối đa của team. = tổng số mp của các hero hiện tại xuất hiện trong battle
        public static float SpeedRespawnEnergyTeam1 = 80f; //Tốc độ hồi thể lực mặc định 80f
        public static float SpeedRespawnEnergyTeam2 = 80f; //Tốc độ hồi thể lực mặc định 80f
        public static int MaxHeroInTeam = 3; //Tối đa bao nhiêu hero 1 team
        public static float TimePerEffectToHero = .25f; //Thời gian giãn cách giữa 2 lần gây dame hiệu ứng bất lợi cho hero (thiêu đốt, trúng độc)
        public static string TagKillReHP = "SkillReHP"; //Tên tag skill hồi máu cho đồng đội
        public static string TagKillShield = "SkillShield"; //Tên tag skill hồi máu cho đồng đội
        public static string TagEffectFireBurn = "FireBurn"; //Tên tag hiệu ứng thiêu đốt
        public static int DifficultPerRound = 1; //Level sau mỗi lần win map
        public static int ItemPerRound = 1; //Khoảng số lượng item nhận được sau mỗi trận đấu
        public static int MaxItemReward = 15; //Số lượng tối đa loại item có thể nhận trong 1 trận đấu
        public static readonly sbyte GemRewardEndBattleRate = 50; //Tỉ lệ nhận dc gem sau mỗi trận đấu
        public static readonly sbyte MaxGemRewardEndBattle = 5; //Gem tối đa sau mỗi trận đấu có thể nhận đc
        public static readonly List<byte> HeroIDLine1 = new List<byte> { 4, 5, 10 };
        public static readonly List<byte> HeroIDLine2 = new List<byte> { 2, 3, 7 };
        public static readonly List<byte> HeroIDLine3 = new List<byte> { 1, 6, 8, 9 };

        /// <summary>
        /// Đặt lại các biến nội tại về ban đầu (gọi trước khi vào trận)
        /// </summary>
        public static void ResetIntrisic () {
            BattleCore.Hero5IntrinsicEnable = false;
            BattleCore.Hero5IntrinsicTeam = null;
        }

        /// <summary>
        /// Gây sát thương
        /// </summary>
        /// <param name="Hero"></param>
        /// <param name="Enemy"></param>
        /// <param name="vec">Vị trí để showdame</param>
        /// <param name="Dameper">Phần trăm dame</param>
        /// <param name="type">Kiểu dame, physic hay magic</param>
        /// <param name="damefrom">Bên nào tấn công</param>
        public static float Damage (HeroesProperties hero1, HeroesProperties hero2, int Dameper, int type) {
            var defExellent = hero2.vDefenseExcellent > 0 && UnityEngine.Random.Range (0, 100) <= hero2.vDefenseExcellent? true : false; //Phòng thủ hoàn hảo
            var dgmExellent = !defExellent && hero1.vDamageExcellent > 0 && UnityEngine.Random.Range (0, 100) <= hero1.vDamageExcellent? true : false; //Sát thương hoàn hảo
            var dmgEarth = DamageCaculator (hero1.vDamageEarth, hero1.vDamageEarth, hero2.vDefenseEarth, hero2.vDefenseEarth, 0, 0, 100, 0, 0); //Sát thương hệ đất
            var dmgWater = DamageCaculator (hero1.vDamageWater, hero1.vDamageWater, hero2.vDefenseWater, hero2.vDefenseWater, 0, 0, 100, 0, 0); //Sát thương hệ nước
            var dmgFire = DamageCaculator (hero1.vDamageFire, hero1.vDamageFire, hero2.vDefenseFire, hero2.vDefenseFire, 0, 0, 100, 0, 0); //Sát thương hệ lửa
            return (dmgEarth + dmgWater + dmgFire) + DamageCaculator (defExellent?0 : hero1.vAtk, defExellent?0 : hero1.vMagic, dgmExellent? 0 : hero2.vArmor, dgmExellent?0 : hero2.vMagicResist, hero1.vLethality, hero1.vMagicPenetration, Dameper, type, hero1.vCritical);
        }

        //Tính toán sát thương
        /// <summary>
        /// Caculator damage
        /// </summary>
        /// <param name="dame_physic">Dame gốc vật lý</param>
        /// <param name="dame_magic">Dame phép</param>
        /// <param name="def_physic">Thủ vật lý của đối phương</param>
        /// <param name="def_magic">Thủ phép của đối phương</param>
        /// <param name="pass_def_physic">Xuyên giáp</param>
        /// <param name="pass_def_magic">Xuyên phép</param>
        /// <param name="dameper">Số phần trăm tính toán</param>
        /// <param name="type">Kiểu sát thương. 0: Vật lý, 1: phép</param>
        /// <returns></returns>
        public static float DamageCaculator (float dame_physic, float dame_magic, float def_physic, float def_magic, float pass_def_physic, float pass_def_magic, int dameper, int type, float crit) {
            float dame_end = 0f;
            if (type == 0) //Sát thương vật lý
            {
                dame_end = def_physic >= 0 ? dame_physic * (100 / (100 + (def_physic - (def_physic * pass_def_physic / 100)))) : dame_end = dame_physic * (2 - 100 / (100 - def_physic));
                dame_end = UnityEngine.Random.Range (0, 100) <= crit ? dame_end * 2 : dame_end; //Gấp đôi sát thương với đòn chí mạng
            }
            if (type == 1) //Sát thương phép
                dame_end = def_magic >= 0 ? dame_end = dame_magic * (100 / (100 + (def_magic - (def_magic * pass_def_magic / 100)))) : dame_end = dame_magic * (2 - 100 / (100 - def_magic));
            dame_end = dame_end * dameper / 100f; //Tính toán số phần trăm sát thương
            return UnityEngine.Random.Range (dame_end - (dame_end * 10 / 100f), dame_end + (dame_end * 10 / 100f)); //Dame sẽ chênh lệch 10%
        }

        /// <summary>
        /// Hồi máu cho đồng đội với skill của mình
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="dameper">Phần trăm dame</param>
        /// <param name="type">Kiểu dame, magic hay physic</param>
        /// <returns></returns>
        public static float RespawnHPValues (HeroesProperties prop, int dameper, int type) {
            float dame_end = 0f;
            if (type == 0) //Sát thương vật lý
                dame_end = prop.vAtk * dameper / 100f;
            if (type == 1) //Sát thương phép
                dame_end = prop.vMagic * dameper / 100f;
            return dame_end;
        }

        /// <summary>
        /// Tính toán thời gian giảm trừ để ra thời gian cuối cùng khi đã tính % kháng hiệu ứng
        /// </summary>
        /// <param name="prop">DataValues của hero</param>
        /// <param name="timeEffect">Thời gian gây ra hiệu ứng của skill đối phương</param>
        /// <returns></returns>
        public static float TimeCauseEffect (HeroesProperties prop, float timeEffect) {
            return timeEffect - (timeEffect * prop.vTenacity / 100f);
        }

        /// <summary>
        /// Điều chỉnh độ khó của map
        /// </summary>
        /// <param name="type">0: tăng dần độ khó, 1 -> trừ 5 level cho vùng đất đó</param>
        /// <param name="mapID">id của loại map (xem trong module)</param>
        public static void ChangeDifficult (int type, int mapID) {
            if (type.Equals(0))
                DataUserController.User.LevelMap[mapID] += BattleCore.DifficultPerRound;
            else
            {
                DataUserController.User.LevelMap[mapID] -= GlobalVariables.MapLevelDeclareHireAssassin;
                if (DataUserController.User.LevelMap[mapID] < 1)
                    DataUserController.User.LevelMap[mapID] = 1;
            }
        }

        /// <summary>
        /// Trả về thời gian chờ giữa 2 đòn đánh
        /// Tốc độ đánh < 2.0f thì gọi hàm này
        /// </summary>
        /// /// <param name="value">Tốc độ đánh</param>
        /// <returns></returns>
        public static float GetTimePendingNormalAtk (float value) {
            if (value >= (1 / DefaultAnAnimationTime))
                return 0;
            else
                return ((1 / DefaultAnAnimationTime) / value * DefaultAnAnimationTime - DefaultAnAnimationTime); //0.5f là nửa giây, thời gian animation normal atk của các hero được thiết kế.
        }

        /// <summary>
        /// Trả về tốc độ của animation nếu tốc độ đánh > 2.0f
        /// /// Tốc độ đánh > 2.0f thì gọi hàm này
        /// </summary>
        /// <param name="value">Tốc độ đánh</param>
        /// <returns></returns>
        public static float GetAnimSpeed (float value) {
            return DefaultAnAnimationTime / (1 / value);
        }

        /// <summary>
        /// Tạo danh sách item có thể nhận được sau trận đấu
        /// </summary>
        public static void CreateItemRewardList () {

        }
    }
}