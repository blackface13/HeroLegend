using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Controller.SceneController;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HeroBase : MonoBehaviour
{

    #region Variables 

    public bool IntrinsicEnable = true; //Biến check xem có dùng nội tại hay ko, dành cho 1 số nhân vật sử dụng nội tại real time
    public List<GameObject> Skill1; //Skill1 (đánh thường)
    public List<GameObject> Skill2; //Skill2
    public GameObject[] Skill3; //Skill3
    public int HeroID; //Set ở Editor
    public HeroType HType;
    public enum HeroType
    {
        near, //Cận chiến
        far //Tầm xa
    }
    public Actions HeroAction; //Action mà nhân vật đang thực hiện
    public enum Actions
    {
        Normal, //Đang đứng ở vị trí của mình
        MoveToEnemy, //Đang di chuyển tới vị trí của đối phương
        MoveToMyPosition, //Đang di chuyển về vị trí của mình
        AttackAction, //Đang đánh
    }
    public Vector3 VectorOriginal; //Vị trí ban đầu của nhân vật
    public Vector3 VectorCurren; //Vị trí hiện tại, dùng để di chuyển
    private bool SetVector; //Check set vector hay chưa, chỉ gán 1 lần duy nhất
    public int Team; //0: team trái, 1 team phải
    private DatabaseHero DBHeroes;
    private List<HeroesProperties> Prop;
    public HeroesProperties DataValues = new HeroesProperties(); //Các chỉ số của nhân vật
    private Animator Anim; //Component animation của hero
    public BattleSystem Battle;
    public bool HeroAlive; //Hero còn sống hay đã tạch
    private GameObject[] StatusObject; //Object hiệu ứng của các status
    private GameObject StatusObjectTime; //Object thời gian còn lại của hiệu ứng
    public status Status;
    public enum status
    {
        Normal, //Bình thường
        Silent, //Câm lặng
        Stun, //Choáng
        Root, //Giữ chân
        Ice, //Đóng băng
        Static, //Tĩnh, không thể bị chọn làm mục tiêu
        Blind, //Mù
        Slow, //Làm chậm
    }
    public Effect Eff;
    public enum Effect
    {
        Normal, //Bình thường
        Fire, //Thiêu đốt
        Poison, //Trúng độc
    }
    public float EffectSpaceY = 7f; //Khoảng cách Y hiệu ứng từ nhân vật tới hiệu ứng, set lại ở các class hero lớn hơn, còn các hero chibi thì để nguyên
    public int[] SkillType; //Kiểu ra đòn của skill, đứng tại chỗ hoặc tới gần. 0 = tại chỗ, 1 = tới gần, set ở Class hero được kế thừa
    public List<int> SkillListAciton; //Danh sách skill sẽ thực hiện 0 = normalatk, 1 = skill 1, 2 = skill 2
    private bool SkillListAcitoning; //Biến check xem có đang thực hiện skill trong list hay ko
    private bool StayingCollision; //Check xem có đang giữ va chạm hay ko
    public int ComboNormalAtk; //Đếm combo normal atk, 0 1 2
    public float[] ControlTimeComboNormalAtk; //Điều khiển thời gian combo
    public float SpeedAnimationValue = 1f;
    public bool SilentStatus; //Hiệu ứng câm lặng, check xem hiệu ứng có đang hoạt động hay ko
    public bool IsAttacked; //Check xem đòn đánh thường đã gây sát thương hay chưa (dành cho đang thực thi đòn đánh thường mà bị ngắt bởi dùng skill)
    public bool IsNoDame; //Biến này = true thì sẽ miễn nhiễm sát thương từ đối phương
    public bool IsRunAnimDie; //check xem đã chạy animation die hay chưa
    public bool EndBattle; //= true thì ko update nhân vật nữa, kết thúc trận đấu
    public bool LockAction = false; //= true thì ko di chuyển nhân vật nữa
    public float DamageValue = 0; //Sát thương sẽ gây ra cho đối thủ (để cộng điểm kinh nghiệm cuối trận)
    public float DefenseValue = 0; //Sát thương phải gánh chịu (để cộng điểm kinh nghiệm cuối trận)
    public float AnimNormalAtkSpeed; //Tốc độ của animation khi atk speed > 2.0f
    public List<ItemModel> ItemsEnemy = new List<ItemModel>();
    #endregion

    #region Initialize 

    public virtual void Awake()
    {
        SilentStatus = false; //Hiệu ứng câm lặng ko được kích hoạt
        Eff = Effect.Normal; //Không bị dính hiệu ứng gì khi setup
        Status = status.Normal; //Trạng thái bình thường
        HeroAlive = true; //Set hero còn sống
        Battle = GameObject.Find("SceneControl").GetComponent<BattleSystem>();
        Status = status.Normal; //Set trạng thái bình thường trước khi vào trận
        HeroAction = Actions.Normal; //Set action hiện tại đang thực hiện
        ControlTimeComboNormalAtk = new float[2];
        CreateObjectStatus();
    }

    //Khởi tạo
    public virtual void Start()
    {
        StartCoroutine(HPController()); //Khởi tạo việc hồi máu suốt quá trình chơi game
        VectorOriginal = VectorCurren = transform.position; //Gán vị trí ban đầu để thực hiện di chuyển về vị trí cũ với hero cận chiến
    }

    /// <summary>
    /// Khởi tạo các giá trị ban đầu cho hero
    /// </summary>
    /// <param name="obj"></param>
    public void FirstSetupHero(GameObject obj)
    {

        Anim = obj.GetComponent<Animator>();
        obj.gameObject.layer = Team.Equals(0) ? 9 : 10; //Chọn team trái hay phải và setup layer
        obj.transform.localScale = Team.Equals(0) ? obj.transform.localScale : new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
    }

    /// <summary>
    /// Tính toán lại các chỉ số nhân vật
    /// </summary>
    private void CaculatorDataValue()
    {
        DataUserController.LoadHeroDefault();
        DataValues = DataUserController.HeroesDefault.DBHeroesDefault.Find(x => x.ID == HeroID).Clone(); //Clone dữ liệu nv default
        // for (int i = 0; i < DataValues.Time_respawn_skill.Length; i++)//clone mảng thời gian hồi chiêu
        //     DataValues.Time_respawn_skill = (float[])(DataValues.Time_respawn_skill.Clone());
        for (int i = 0; i < DataValues.Mana_skill.Length; i++) //clone mảng mana sử dụng cho skill
            DataValues.Mana_skill = (float[])(DataValues.Mana_skill.Clone());
        var thisHero = Team.Equals(0) ? DataUserController.Heroes.DBHeroes.Find(x => x.ID == HeroID).Clone() : DataValues;
        if (!Team.Equals(0)) //Nếu không phải nhân vật của user -> gán bộ trang bị cho enemy để tăng sức mạnh
            thisHero.ItemsEquip = ItemsEnemy;
        #region Tính toán lại các chỉ số dựa trên Level nhân vật 
        #region Tính toán chỉ số lúc đeo trang bị 

        var vAtkPlus = 0f; //Sát thương vật lý
        var vMagicPlus = 0f; //Sát thương phép thuật
        var vHealthPlus = 0f; //Máu
        var vManaPlus = 0f; //Mana
        var vArmorPlus = 0f; //Giáp
        var vMagicResistPlus = 0f; //Kháng phép
        var vHealthRegenPlus = 0f; //Chỉ số hồi máu mỗi giây
        var vManaRegenPlus = 0f; //Chỉ số hồi mana mỗi giây
        var vDamageEarthPlus = 0f; //Sát thương hệ đất
        var vDamageWaterPlus = 0f; //Sát thương hệ nước
        var vDamageFirePlus = 0f; //Sát thương hệ lửa
        var vDefenseEarthPlus = 0f; //Kháng hệ đất
        var vDefenseWaterPlus = 0f; //Kháng hệ nước
        var vDefenseFirePlus = 0f; //Kháng hệ hỏa
        var vAtkSpeedPlus = 0f; //% Tốc độ tấn công cơ bản tăng thêm
        var vLifeStealPhysicPlus = 0f; //% hút máu
        var vLifeStealMagicPlus = 0f; //% hút máu phép
        var vLethalityPlus = 0f; //% Xuyên giáp
        var vMagicPenetrationPlus = 0f; //% Xuyên phép
        var vCriticalPlus = 0f; //% chí mạng
        var vTenacityPlus = 0f; //% kháng hiệu ứng
        var vCooldownReductionPlus = 0f; //% Giảm tgian hồi chiêu
        var vDamageExcellentPlus = 0f; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
        var vDefenseExcellentPlus = 0f; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
        var vDoubleDamagePlus = 0f; //Tỉ lệ x2 đòn đánh max = 10%
        var vTripleDamagePlus = 0f; //Tỉ lệ x3 đòn đánh max = 10%
        var vDamageReflectPlus = 0f; //Phản hồi % sát thương. max = 5%
        var vRewardPlusPlus = 0f; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

        var countItemEquip = thisHero.ItemsEquip.Count > 0 ? thisHero.ItemsEquip.Count : 0;
        if (countItemEquip > 0)
        {
            for (int i = 0; i < countItemEquip; i++)
            {
                //Sát thương vật lý
                var valueOriginal = thisHero.ItemsEquip[i].vAtk; //Chỉ số gốc
                var valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                var valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                var valueBonusPlus = thisHero.ItemsEquip[i].vAtkPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vAtkPlus / 100f : 0; //% tăng thêm
                vAtkPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Sát thương phép
                valueOriginal = thisHero.ItemsEquip[i].vMagic; //Chỉ số gốc
                valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = thisHero.ItemsEquip[i].vMagicPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vMagicPlus / 100f : 0; //% tăng thêm
                vMagicPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Máu
                valueOriginal = thisHero.ItemsEquip[i].vHealth; //Chỉ số gốc
                valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = thisHero.ItemsEquip[i].vHealthPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vHealthPlus / 100f : 0; //% tăng thêm
                vHealthPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Năng lượng
                valueOriginal = thisHero.ItemsEquip[i].vMana; //Chỉ số gốc
                valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = thisHero.ItemsEquip[i].vManaPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vManaPlus / 100f : 0; //% tăng thêm
                vManaPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Giáp
                valueOriginal = thisHero.ItemsEquip[i].vArmor; //Chỉ số gốc
                valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = thisHero.ItemsEquip[i].vArmorPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vArmorPlus / 100f : 0; //% tăng thêm
                vArmorPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Kháng phép
                valueOriginal = thisHero.ItemsEquip[i].vMagicResist; //Chỉ số gốc
                valueBonusLevel = (thisHero.ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (thisHero.ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (thisHero.ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((thisHero.ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = thisHero.ItemsEquip[i].vMagicResistPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * thisHero.ItemsEquip[i].vMagicResistPlus / 100f : 0; //% tăng thêm
                vMagicResistPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);

                vHealthRegenPlus += thisHero.ItemsEquip[i].vHealthRegen; //Chỉ số hồi máu mỗi giây
                vManaRegenPlus += thisHero.ItemsEquip[i].vManaRegen; //Chỉ số hồi mana mỗi giây
                vDamageEarthPlus += thisHero.ItemsEquip[i].vDamageEarth; //Sát thương hệ đất
                vDamageWaterPlus += thisHero.ItemsEquip[i].vDamageWater; //Sát thương hệ nước
                vDamageFirePlus += thisHero.ItemsEquip[i].vDamageFire; //Sát thương hệ lửa
                vDefenseEarthPlus += thisHero.ItemsEquip[i].vDefenseEarth; //Kháng hệ đất
                vDefenseWaterPlus += thisHero.ItemsEquip[i].vDefenseWater; //Kháng hệ nước
                vDefenseFirePlus += thisHero.ItemsEquip[i].vDefenseFire; //Kháng hệ hỏa
                vAtkSpeedPlus += thisHero.ItemsEquip[i].vAtkSpeed; //% Tốc độ tấn công cơ bản tăng thêm
                vLifeStealPhysicPlus += thisHero.ItemsEquip[i].vLifeStealPhysic; //% hút máu
                vLifeStealMagicPlus += thisHero.ItemsEquip[i].vLifeStealMagic; //% hút máu phép
                vLethalityPlus += thisHero.ItemsEquip[i].vLethality; //% Xuyên giáp
                vMagicPenetrationPlus += thisHero.ItemsEquip[i].vMagicPenetration; //% Xuyên phép
                vCriticalPlus += thisHero.ItemsEquip[i].vCritical; //% chí mạng
                vTenacityPlus += thisHero.ItemsEquip[i].vTenacity; //% kháng hiệu ứng
                vCooldownReductionPlus += thisHero.ItemsEquip[i].vCooldownReduction; //% Giảm tgian hồi chiêu
                vDamageExcellentPlus += thisHero.ItemsEquip[i].vDamageExcellent; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
                vDefenseExcellentPlus += thisHero.ItemsEquip[i].vDefenseExcellent; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
                vDoubleDamagePlus += thisHero.ItemsEquip[i].vDoubleDamage; //Tỉ lệ x2 đòn đánh max = 10%
                vTripleDamagePlus += thisHero.ItemsEquip[i].vTripleDamage; //Tỉ lệ x3 đòn đánh max = 10%
                vDamageReflectPlus += thisHero.ItemsEquip[i].vDamageReflect; //Phản hồi % sát thương. max = 5%
                vRewardPlusPlus += thisHero.ItemsEquip[i].vRewardPlus; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%}
            }
        }

        #endregion
        DataValues.vAtk += DataValues.vAtkPerLevel * thisHero.Level + vAtkPlus; //Sát thương vật lý
        DataValues.vMagic += DataValues.vMagicPerLevel * thisHero.Level + vMagicPlus; //Sát thương phép thuật
        DataValues.vHealth += DataValues.vHealthPerLevel * thisHero.Level + vHealthPlus; //Máu
        DataValues.vHealthCurrent = DataValues.vHealth;
        DataValues.vMana += vManaPlus; //Mana
        DataValues.vArmor += DataValues.vArmorPerLevel * thisHero.Level + vArmorPlus; //Giáp
        DataValues.vMagicResist += DataValues.vMagicResistPerLevel * thisHero.Level + vMagicResistPlus; //Kháng phép
        DataValues.vHealthRegen += DataValues.vHealthRegenPerLevel * thisHero.Level + vHealthRegenPlus; //Chỉ số hồi máu mỗi giây
        DataValues.vManaRegen += DataValues.vManaRegenPerLevel * thisHero.Level + vManaRegenPlus; //Chỉ số hồi mana mỗi giây
        DataValues.vDamageEarth += vDamageEarthPlus; //Sát thương hệ đất
        DataValues.vDamageWater += vDamageWaterPlus; //Sát thương hệ nước
        DataValues.vDamageFire += vDamageFirePlus; //Sát thương hệ lửa
        DataValues.vDefenseEarth += vDefenseEarthPlus; //Kháng hệ đất
        DataValues.vDefenseWater += vDefenseWaterPlus; //Kháng hệ nước
        DataValues.vDefenseFire += vDefenseFirePlus; //Kháng hệ hỏa
        DataValues.vAtkSpeed += DataValues.vAtkSpeed * vAtkSpeedPlus / 100f; //% Tốc độ tấn công cơ bản tăng thêm
        DataValues.vLifeStealPhysic += vLifeStealPhysicPlus; //% hút máu
        DataValues.vLifeStealMagic += vLifeStealMagicPlus; //% hút máu phép
        DataValues.vLethality += vLethalityPlus; //% Xuyên giáp
        DataValues.vMagicPenetration += vMagicPenetrationPlus; //% Xuyên phép
        DataValues.vCritical += vCriticalPlus; //% chí mạng
        DataValues.vTenacity += vTenacityPlus; //% kháng hiệu ứng
        DataValues.vCooldownReduction += vCooldownReductionPlus + ((DataValues.vCooldownReductionPerLevel * thisHero.Level) / 100f); //% Giảm tgian hồi chiêu
        DataValues.vDamageExcellent += vDamageExcellentPlus; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép)
        DataValues.vDefenseExcellent += vDefenseExcellentPlus; //% phong thu hoàn hảo (ko bị đánh trúng).
        DataValues.vDoubleDamage += vDoubleDamagePlus; //Tỉ lệ x2 đòn đánh
        DataValues.vTripleDamage += vTripleDamagePlus; //Tỉ lệ x3 đòn đánh
        DataValues.vDamageReflect += vDamageReflectPlus; //Phản hồi % sát thương
        DataValues.vRewardPlus += vRewardPlusPlus; //Tăng lượng vàng rơi ra vào cuối trận
        DataValues.vCooldown[1] -= DataValues.vCooldown[1] * DataValues.vCooldownReduction / 100f; //Thời gian hồi chiêu
        //Nếu vCooldown[0] được set = -1, thì đó là enemy không thể tấn công, chỉ đứng 1 chỗ, dành cho boss kiếm vàng, kim cương....
        if (DataValues.vCooldown[0] != -1)
        {
            if (DataValues.vAtkSpeed <= (1 / BattleCore.DefaultAnAnimationTime))
                DataValues.vCooldown[0] = BattleCore.GetTimePendingNormalAtk(DataValues.vAtkSpeed);
            else
            {
                DataValues.vCooldown[0] = 0;
                AnimNormalAtkSpeed = BattleCore.GetAnimSpeed(DataValues.vAtkSpeed);
            }
        }
        #endregion
    }

    /// <summary>
    /// Refresh lại đội hình cho 2 bên vì biến team sẽ được set sau khi object được tạo
    /// </summary>
    /// <param name="obj"></param>
    public virtual void RefreshTeam(GameObject obj)
    {
        CaculatorDataValue();
        if (DataValues.vCooldown[0] != -1) //Nếu thời gian hồi đánh thường khác 1- (-1 là nhân vật đó ko có đánh thường, dành cho monster không đánh thường, ko skill)
            StartCoroutine(AutoNormalAtk());
        if (Team.Equals(1))
        { //Nếu là team địch
            if (DataValues.vCooldown[1] != -1) //Nếu thời gian hồi chiêu skill khác 1- (-1 là nhân vật đó ko có skill, dành cho monster cỏ)
                StartCoroutine(AutoSkill()); //Thực hiện tự động ra skill đối với team địch - AI viết trong hàm này
            obj.gameObject.layer = Team.Equals(0) ? 9 : 10; //Chọn team trái hay phải và setup layer
            obj.transform.localScale = Team.Equals(0) ? obj.transform.localScale : new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            VectorOriginal = VectorCurren = obj.transform.position; //Gán vị trí ban đầu để thực hiện di chuyển về vị trí cũ với hero cận chiến
            if (Skill1 != null)
            {
                var count = Skill1.Count;
                for (int i = 0; i < count; i++) //Set lại layer của skill khi đổi team của nhân vật
                {
                    Skill1[i].GetComponent<SkillCore>().ReSetupLayer(Team);
                }
            }
            try
            {
                if (Skill2 != null)
                {
                    var count = Skill2.Count;
                    for (int i = 0; i < count; i++) //Set lại layer của skill khi đổi team của nhân vật
                    {
                        Skill2[i].GetComponent<SkillCore>().ReSetupLayer(Team);
                    }
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// Tạo các Object Status
    /// </summary>
    private void CreateObjectStatus()
    {
        StatusObject = new GameObject[5];
        StatusObject[0] = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "Stun"), new Vector3(0, gameObject.transform.position.y + EffectSpaceY, Module.BASELAYER[2]), Quaternion.Euler(75f, 0, 0));
        StatusObject[1] = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "Slow"), new Vector3(0, gameObject.transform.position.y + EffectSpaceY, Module.BASELAYER[2]), Quaternion.Euler(75f, 0, 0));
        StatusObject[2] = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "Root"), new Vector3(0, gameObject.transform.position.y + EffectSpaceY, Module.BASELAYER[2]), Quaternion.Euler(75f, 0, 0));
        StatusObject[3] = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "Silent"), new Vector3(0, gameObject.transform.position.y + EffectSpaceY, Module.BASELAYER[2]), Quaternion.Euler(75f, 0, 0));
        StatusObject[4] = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "Fire"), new Vector3(0, gameObject.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
        StatusObjectTime = Instantiate(Resources.Load<GameObject>(BattleCore.StatusObjectLink + "StatusTime"), new Vector3(0, gameObject.transform.position.y + EffectSpaceY - 0.5f, Module.BASELAYER[2]), Quaternion.identity);

        StatusObjectTime.transform.SetParent(transform, false);
        StatusObjectTime.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            StatusObject[i].SetActive(false);
            StatusObject[i].transform.SetParent(transform, false);
        }
        StatusObject[4].transform.localPosition = new Vector3(0, 0, 0); //Set lại hiệu ứng thiêu dốt vào giữa nhân vật
    }
    #endregion

    #region Functions 

    /// <summary>
    /// Thực hiện đòn đánh thường tự động
    /// </summary>
    public IEnumerator AutoNormalAtk()
    {
    Begin: yield return new WaitForSeconds(DataValues.vCooldown[0]); //Chờ time hồi đánh thường
        SkillListAciton.Add(0);
        yield return new WaitUntil(() => Status == status.Normal && !SkillListAcitoning);
        goto Begin;
        //StartCoroutine (AutoNormalAtk ());
    }

    /// <summary>
    /// Thực hiện tung skill
    /// </summary>
    public IEnumerator AutoSkill()
    {
    Begin: yield return new WaitForSeconds(DataValues.vCooldown[1]); //Chờ time hồi skill
        SkillListAciton.Add(1);
        //yield return new WaitUntil (() => Status == status.Normal && !SkillListAcitoning);
        goto Begin;
        //StartCoroutine (AutoNormalAtk ());
    }

    /// <summary>
    /// Gọi trong hàm va chạm với skill của đối thủ, thay đổi trạng thái như choáng, câm lặng... nếu có
    /// </summary>
    /// <param name="skillcore"></param>
    private void AssignStatus(SkillCore skillcore)
    {
        if (skillcore.Status.ToString() != status.Normal.ToString())
        {
            foreach (var stt in status.GetValues(typeof(status)))
            {
                if (skillcore.Status.ToString().Equals(stt.ToString()))
                {
                    var timestatusaction = skillcore.TimeStatus - (skillcore.TimeStatus * DataValues.vTenacity / 100f);
                    SetStatus((status)stt, timestatusaction);
                    //StartCoroutine(AutoHidenStatus(timestatusaction, StatusObject[0]));
                    //print(skillcore.Status);
                    break;
                }
            }
        }
    }

    // Hàm valid trạng thái hero trước khi thay đổi (stt = loại trạng thái, times = thời gian hiệu lực)
    // Chức năng: không thể nhận status tiếp theo giống status trước đó khi status trước chưa hết hiệu lực
    // Nếu status tiếp theo được nhận vào, thì status trước đó sẽ mất hiệu lực
    public void SetStatus(status stt, float times)
    {
        switch (stt)
        {
            case status.Normal: //Trạng thái bình thường
                Status = status.Normal;
                break;
            case status.Silent: //Trạng thái câm lặng
                if (Status != status.Silent && Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Stun: //Trạng thái choáng
                if (Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Root: //Trạng thái giữ chân
                if (Status != status.Root && Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Ice: //Trạng thái đóng băng
                if (Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Static: //Trạng thái không thể chọn làm mục tiêu
                if (Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Blind: //Trạng thái mù
                if (Status != status.Blind && Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            case status.Slow: //Trạng thái làm chậm
                if (Status != status.Slow && Status != status.Stun && Status != status.Ice && Status != status.Static)
                    StartCoroutine(StatusAction(stt, times));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Cài đặt effect cho trạng thái
    /// </summary>
    /// <param name="slot"></param>
    private void SetEffectAction(int slot)
    {
        for (int i = 0; i < StatusObject.Length; i++)
        {
            if (i.Equals(slot))
            {
                StatusObject[i].SetActive(true);
                if (i.Equals(3)) //Hiệu ứng câm lặng
                    SilentStatus = true; //Kích hoạt bị câm lặng
            }
            else
            {
                StatusObject[i].SetActive(false);
                SilentStatus = false; //Loại bỏ câm lặng
            }
        }
    }

    /// <summary>
    /// Thực hiện set trạng thái cho nhân vật
    /// </summary>
    /// <param name="stt"></param>
    /// <param name="times">Thời gian bị hiệu ứng</param>
    /// <returns></returns>
    private IEnumerator StatusAction(status stt, float times)
    {
        Status = stt;
        switch (Status)
        {
            case status.Stun: //Hiệu ứng choáng
                SetEffectAction(0);
                StatusObjectTime.SetActive(true);
                StartCoroutine(GameSystem.ScaleUI(1, StatusObjectTime.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), new Vector3(0, 1, 1), times));
                Anim.Rebind(); //Stop animation
                SkillListAcitoning = false; //Dừng đòn đánh hiện tại
                SpeedAnimationValue = 1f; //Tốc độ di chuyển bình thường
                break;
            case status.Slow: //Hiệu ứng làm chậm
                SetEffectAction(1);
                StatusObjectTime.SetActive(true);
                StartCoroutine(GameSystem.ScaleUI(1, StatusObjectTime.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), new Vector3(0, 1, 1), times));
                SpeedAnimationValue = 5f;
                break;
            case status.Root: //Hiệu ứng giữ chân
                SetEffectAction(2);
                StatusObjectTime.SetActive(true);
                StartCoroutine(GameSystem.ScaleUI(1, StatusObjectTime.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), new Vector3(0, 1, 1), times));
                SpeedAnimationValue = 1f;
                break;
            case status.Silent: //Hiệu ứng câm lặng
                SetEffectAction(3);
                StatusObjectTime.SetActive(true);
                StartCoroutine(GameSystem.ScaleUI(1, StatusObjectTime.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), new Vector3(0, 1, 1), times));
                SpeedAnimationValue = 1f;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(times);
        if (Status.Equals(stt)) //Nếu vẫn chịu trạng thái cũ => trở về trạng thái bình thường sau khi hết hiệu lực
        {
            SpeedAnimationValue = 1f; //Tốc độ di chuyển bình thường
            Status = status.Normal; //Về trạng thái bình thường
            StatusObjectTime.SetActive(false);
            SetEffectAction(-1); //Xóa hết các hiệu ứng của trạng thái
            if (Status.Equals(status.Stun))
                ChangeAnim("Idie");
            if (SkillListAciton.Count <= 0 && transform.position != VectorOriginal && !SkillListAcitoning) //Danh sách skill có skill cần hoạt động
            {
                Battle.UpdatePisitionCenterCombat(0); //Đưa camera về vị trí 0
                HeroAction = Actions.MoveToMyPosition; //Di chuyển về vị trí của mình
                ChangeAnim("Goback"); //Animation lùi về
                SkillListAcitoning = false; //Dừng đòn đánh hiện tại
            }
        }
    }

    /// <summary>
    /// Thay đổi animations cho nhân vật
    /// </summary>
    /// <param name="trigger"></param>
    public void ChangeAnim(string trigger)
    {
        Anim.Rebind();
        Anim.speed = trigger.StartsWith("Atk") && AnimNormalAtkSpeed > 0 ? AnimNormalAtkSpeed : 1.0f; //Set tốc độ animation khi đánh thường và tốc độ anim > 0
        Anim.SetTrigger(trigger);
    }

    //Hàm này gọi ở animation, gọi các object skill xuất hiện
    public virtual void ActionSkill(int skillnumber)
    {
        IsAttacked = true; //Đòn đánh thường đã gây sát thương
        if (GameSystem.Settings.SkillSlowMotion) //Nếu user setting bật hiệu ứng làm chậm khi dùng skill
            if (skillnumber.Equals(1)) //Nếu dùng skill
            {
                StartCoroutine(SlowMotionForSkill()); //Làm chậm
            }
    }

    /// <summary>
    /// Làm chậm hoạt ảnh khi dùng skill
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowMotionForSkill()
    {
        Time.timeScale = .2f;
        yield return new WaitForSeconds(.05f);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Gọi ở các object kế thừa, enable skill của hero
    /// </summary>
    /// <param name="obj">Skill object</param>
    /// <param name="vec">Tọa độ xuât hiện</param>
    /// <param name="quater">Độ nghiêng, xoay tròn</param>
    public void ShowSkill(GameObject obj, Vector3 vec, Quaternion quater)
    {
        obj.transform.position = vec;
        obj.transform.rotation = quater;
        obj.SetActive(true);
    }

    /// <summary>
    /// Kết thúc tấn công cho hero (gọi ở cuối animations)
    /// </summary>
    public virtual void EndAtk()
    {
        switch (HType)
        {
            case HeroType.near: //Hero cận chiến
                if (HeroAction.Equals(Actions.AttackAction)) //Nếu đánh xong
                {
                    if (SkillListAciton.Count <= 0)
                    {
                        Battle.UpdatePisitionCenterCombat(0); //Đưa camera về vị trí 0
                        HeroAction = Actions.MoveToMyPosition; //Di chuyển về vị trí của mình
                        ChangeAnim("Goback"); //Animation lùi về
                        SkillListAcitoning = false; //Kết thúc thực hiện skill
                    }
                    //Nếu đang giữ action đánh mà trong list action vẫn còn action cần thực hiện và vẫn đang va chạm với đối phương
                    if (SkillListAciton.Count > 0 && StayingCollision)
                    {
                        HeroAction = Actions.AttackAction;
                        ChangeAnim(GetAnimationName(SkillListAciton[0]));
                        ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
                    }
                    else
                    {
                        Battle.UpdatePisitionCenterCombat(0); //Đưa camera về vị trí 0
                        HeroAction = Actions.MoveToMyPosition; //Di chuyển về vị trí của mình
                        ChangeAnim("Goback"); //Animation lùi về
                        SkillListAcitoning = false; //Kết thúc thực hiện skill
                    }
                    // if ((Team.Equals(0) && transform.position.x <= VectorOriginal.x) || (Team.Equals(1) && transform.position.x >= VectorOriginal.x))//Nếu về tới vị trí
                    // {
                    //     transform.position = VectorOriginal;//Gán lại vị trí
                    //     VectorCurren = VectorOriginal;
                    //     ChangeAnim("Idie");//Animation bình thường
                    //     Battle.UpdatePisitionCenterCombat(0);
                    //     HeroAction = Actions.Normal;//Đưa về trạng thái bình thường
                    // }
                }
                break;
            case HeroType.far: //Hero tầm xa
                {
                    if (SkillListAciton.Count > 0)
                    {
                        ChangeAnim(GetAnimationName(SkillListAciton[0]));
                        ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
                    }
                    else
                    {
                        ChangeAnim("Idie");
                        SkillListAcitoning = false; //Kết thúc thực hiện skill
                    }
                }
                break;
            default:
                break;
        }
    }
    // public virtual void RunSkill1()
    // {
    //     RunSkill(SkillType[0], 0);
    // }
    /// <summary>
    /// Trả về object skill đang ko hoạt động để xuất hiện
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public GameObject GetObjectDontActive(List<GameObject> obj)
    {
        int count = obj.Count;
        for (int i = 0; i < count; i++)
        {
            if (!obj[i].activeSelf)
                return obj[i];
        }
        return null;
    }
    //Thực hiện skill. Skilltype = 0, đứng tại chỗ tung skill, 1 = nhảy lại gần tung skill (1 dành cho các skill buff của nhân vật cận chiến, hoặc skill đứng tại chỗ ra đòn)
    // skillnumber = thứ tự skill. 0 = đánh thường, 1 = skill1, 2 = skill2
    public void RunSkill(int skilltype, int skillnumber)
    {
        if (!SetVector)
        {
            VectorCurren = VectorOriginal = transform.position;
            SetVector = true;
        }
        IsAttacked = false; //Đưa đòn đánh thường trở về trạng thái chờ
        switch (skilltype) //Kiểu skill
        {
            case 0: //Skill tầm xa, đứng tại chỗ ra đòn
                HeroAction = Actions.AttackAction;
                ChangeAnim(GetAnimationName(skillnumber));
                break;
            case 1: //Skill cận chiến, nhảy tới mới ra đòn
                //if (HeroAction.Equals(Actions.Normal))
                //Nếu đang giữ action đánh mà trong list action vẫn còn action cần thực hiện và vẫn đang va chạm với đối phương
                if (SkillListAciton.Count > 0 && StayingCollision)
                {
                    HeroAction = Actions.AttackAction;
                    ChangeAnim(GetAnimationName(SkillListAciton[0]));
                    ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
                }
                else
                {
                    HeroAction = !HeroAction.Equals(Actions.MoveToEnemy) ? Actions.MoveToEnemy : Actions.MoveToEnemy;
                    ChangeAnim("Goto"); //Animation nhảy tới
                }

                break;
            default:
                break;
        }
        ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
    }
    /// <summary>
    /// Điều khiển combo của normal atk, ngắt combo sau 1 giây
    /// </summary>
    public void ReturnComboAtk()
    {
        if (ComboNormalAtk >= 3)
            ComboNormalAtk = 0;
        ControlTimeComboNormalAtk[1] += 1 * Time.deltaTime;
        if (ComboNormalAtk > 0)
        {
            if (ControlTimeComboNormalAtk[1] - ControlTimeComboNormalAtk[0] >= BattleCore.TimeDelayComboNormalAtk)
                ComboNormalAtk = 0;
        }
    }
    /// <summary>
    /// Truyền vào số thứ tự skill và trả về tên animation cần thực hiện
    /// </summary>
    /// <param name="skillnumber">0: đánh thường, 1: skill</param>
    /// <returns></returns>
    private string GetAnimationName(int skillnumber)
    {
        switch (skillnumber)
        {
            case 0:
                SkillListAciton.RemoveAt(0); //Xóa action vừa thực hiện
                return ComboNormalAtk == 0 ? "Atk1" : ComboNormalAtk == 1 ? "Atk2" : "Atk3";
            case 1:
                SkillListAciton.RemoveAt(0); //Xóa action vừa thực hiện
                //ComboNormalAtk = 0; //Ngắt combo của normal atk (Disable dòng này thì combo đánh thường không bị ngắt khi dùng skill)
                return "Skill1";
            default:
                break;
        }
        return "";
    }

    /// <summary>
    /// Thực hiện skill
    /// </summary>
    /// <param name="skillnumber"></param>
    public void AddSkillToListAction(int skillnumber)
    {
        SkillListAciton.Add(skillnumber);
        if (SkillListAcitoning) //Nếu nhân vật đang thực hiện đòn đánh
        {
            Anim.Rebind(); //Dừng đòn đánh thường hiện tại
            if (!IsAttacked) //Nếu đòn đánh thường chưa gây dame
            {
                SkillListAciton.Add(0); //Thêm lại đòn đánh thường sau khi dùng skill, 0 là đòn đánh thường
            }
            SkillListAcitoning = false;
        }
    }

    //Điều khiển hành động của hero realtime
    private void SkillController()
    {
        if (SkillListAciton.Count > 0) //Danh sách skill có skill cần hoạt động
        {
            if (!SkillListAcitoning) //Nếu đang chưa thực hiện skill trong danh sách
            {
                RunSkill(SkillType[SkillListAciton[0]], SkillListAciton[0]);
                SkillListAcitoning = true; //Chờ thực hiện xong skill
            }
        }
        if (HeroAction.Equals(Actions.MoveToMyPosition)) //Di chuyển về vị trí của mình
            if ((Team.Equals(0) && transform.position.x <= VectorOriginal.x) || (Team.Equals(1) && transform.position.x >= VectorOriginal.x)) //Nếu về tới vị trí
            {
                transform.position = VectorOriginal; //Gán lại vị trí
                VectorCurren = VectorOriginal;
                ChangeAnim("Idie"); //Animation bình thường
                                    //Battle.UpdatePisitionCenterCombat(0);
                HeroAction = Actions.Normal; //Đưa về trạng thái bình thường
            }
            else
            {
                if (!SkillListAcitoning)
                    if (!Status.Equals(status.Root)) //Nếu ko dính trạng thái bị giữ chân thì cho di chuyển
                        VectorCurren.x -= Team.Equals(0) ? (BattleCore.SpeedMove * (Time.deltaTime / SpeedAnimationValue)) : -(BattleCore.SpeedMove * (Time.deltaTime / SpeedAnimationValue));
                transform.position = VectorCurren;
            }
        if (HeroAction.Equals(Actions.MoveToEnemy)) //Di chuyển tới vị trí đối phương
        {
            if (!Status.Equals(status.Root)) //Nếu ko dính trạng thái bị giữ chân thì cho di chuyển
                VectorCurren.x += Team.Equals(0) ? (BattleCore.SpeedMove * (Time.deltaTime / SpeedAnimationValue)) : -(BattleCore.SpeedMove * (Time.deltaTime / SpeedAnimationValue));
            transform.position = VectorCurren;
        }
    }

    //Update
    public virtual void Update()
    {
        if (!EndBattle)
        {
            //Nếu hero die thì disable va chạm
            if (DataValues.vHealthCurrent <= 0) //Ko cho phép trừ máu khi máu đã < 0
            {
                GameSystem.HideObject(this.gameObject, 1f); //Ẩn sau 1s nếu như bị lỗi không chạy animation die
                DataValues.vHealthCurrent = 0;
                HeroAlive = false; //Hero đã chết
                GetComponent<Collider2D>().enabled = false;
                if (!IsRunAnimDie)
                {
                    HeroAction = Actions.Normal; //Đưa về trạng thái bình thường
                    ChangeAnim("Die");
                    Battle.UpdatePisitionCenterCombat(0); //Đưa camera về vị trí 0
                    IsRunAnimDie = true;
                }
            }
            else
            {
                if (DataValues.vHealthCurrent >= DataValues.vHealth) //Ko cho phép hồi máu khi máu đã max
                    DataValues.vHealthCurrent = DataValues.vHealth;
                //Nếu còn sống thì mới update
                if (HeroAlive)
                {
                    if (Status != status.Stun) //Không bị choáng thì sẽ update
                    {
                        if (!LockAction)
                            SkillController(); //Điều khiển các action của hero cận chiến, nhảy tới hoặc nhảy về hoặc đánh
                    }
                    ReturnComboAtk(); //Hàm này không nằm trong các hiệu ứng
                }
                //Chống bug bỏ qua đối phương trước mặt (dành cho hero cận chiến)
                if ((Team.Equals(0) && transform.position.x >= Camera.main.aspect * 11f) || (Team.Equals(1) && transform.position.x <= 0 - Camera.main.aspect * 11f))
                {
                    transform.position = VectorOriginal; //Gán lại vị trí
                    VectorCurren = VectorOriginal;
                    ChangeAnim("Idie"); //Animation bình thường
                    HeroAction = Actions.Normal; //Đưa về trạng thái bình thường
                    SkillListAcitoning = false;
                }
            }
        }
    }

    /// <summary>
    /// Sản sinh ra liên tục nhiều object skill
    /// </summary>
    /// <param name="objlist">List object skill</param>
    /// <param name="position">vị trí sinh ra object</param>
    /// <param name="delaytime">khoảng cách time giữa 2 lần sản sinh object</param>
    /// <param name="remaining">số object sẽ được sinh ra</param>
    /// <returns></returns>
    public virtual IEnumerator MultiAtk(List<GameObject> objlist, Vector3 position, float delaytime, int remaining, Quaternion quater)
    {
        int count = 0;
    Begin:
        CheckExistAndCreateEffectExtension(position, objlist, quater);
        count++;
        yield return new WaitForSeconds(delaytime);
        if (count < remaining)
            goto Begin;
    }
    /// <summary>
    /// Điều khiển hồi hp realtime mỗi giây
    /// </summary>
    private IEnumerator HPController()
    {
    Respawn: yield return new WaitForSeconds(1);
        DataValues.vHealthCurrent += DataValues.vHealthCurrent > 0 ? DataValues.vHealthRegen / ItemCoreSetting.SecondHeathRegen : 0; //Chỉ hồi máu khi số lượng máu > 0 (nếu nhỏ hơn hoặc = 0 tức là đã chết)
        if (DataValues.vHealthCurrent > 0) //HP > 0 => tiếp tục hồi máu mỗi giây
        {
            goto Respawn;
        }
        else
        {
            yield return new WaitForSeconds(1);
            this.gameObject.SetActive(false);//Khắc phục bug chết nhưng vẫn hiển thị hình ảnh
        }
    }

    /// <summary>
    /// Tự động ẩn gameobject sau 1 khoảng thời gian
    /// </summary>
    /// <param name="time"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    // public virtual IEnumerator AutoHidenStatus(float time, GameObject obj)
    // {
    //     yield return new WaitForSeconds(time);
    //     obj.SetActive(false);
    //     //obj.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, obj.transform.position.z);
    //     obj.transform.localEulerAngles = new Vector3();
    // }

    /// <summary>
    /// Ẩn object ngay lập tức, gọi ở cuối anim die
    /// </summary>
    /// <param name="obj"></param>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYERRIGID2D[1]);
        gameObject.transform.localEulerAngles = new Vector3();
    }
    /// <summary>
    /// Check khởi tạo và hiển thị hiệu ứng trúng đòn lên đối phương
    /// </summary>
    /// <param name="col"></param>
    public bool CheckExistAndCreateEffectExtension(Vector3 col, List<GameObject> gobject, Quaternion quater)
    {
        var a = GetObjectDontActive(gobject);
        if (a == null)
        {
            gobject.Add(Instantiate(gobject[0], new Vector3(col.x, col.y, Module.BASELAYER[2]), quater));
            return true;
        }
        else
            ShowSkill(a, new Vector3(col.x, col.y, Module.BASELAYER[2]), quater);
        return false;
    }

    /// <summary>
    /// Hiệu ứng hồi máu
    /// </summary>
    /// <param name="hpRegen">Số máu hồi lại mỗi lần</param>
    /// <param name="timeRespawn">Khoảng thời gian giữa 2 lần hồi máu</param>
    /// <param name="timesRegen">Số lần sẽ hồi máu</param>
    /// <returns></returns>
    public IEnumerator ActionHealthRegen(float hpRegen, float timeRespawn, int timesRegen)
    {
        var count = 0; //Biến tạm đếm số lần máu đã hồi lại
    Begin:
        if (DataValues.vHealthCurrent <= 0)
            goto End;
        DataValues.vHealthCurrent += DataValues.vHealthCurrent >= DataValues.vHealth ? 0 : hpRegen; //Hồi máu
        Battle.DamageShow(this.transform.position, 2, Team, hpRegen); //Gọi hàm show chỉ số từ battle system
        yield return new WaitForSeconds(timeRespawn);
        count++; //Tăng số lần đếm hồi máu
        if (count >= timesRegen)
            goto End;
        else goto Begin;
        End: yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Hiệu ứng thiêu đốt
    /// </summary>
    /// <param name="damage">Số dame gây ra mỗi lần</param>
    /// <param name="timeTotal">Lượng thời gian bị hiệu ứng</param>
    /// <param name="timeRespawn">Khoảng thời gian giữa 2 lần gây dame</param>
    /// <returns></returns>
    public IEnumerator ActionFireBurn(float damage, float timeTotal, float timeRespawn)
    {
        StatusObjectTime.SetActive(true); //Enable đếm time hiệu ứng
        StartCoroutine(GameSystem.ScaleUI(1, StatusObjectTime.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), new Vector3(0, 1, 1), timeTotal));
        StatusObject[4].SetActive(true);
        var count = 0; //Biến tạm đếm số lần thực hiện
        var time = 0f;
    Begin:
        DataValues.vHealthCurrent -= DataValues.vHealthCurrent <= 0 ? 0 : damage; //Trừ máu
        DefenseValue += damage; //Cộng điểm sát thương phải nhận trong trận, để tính điểm kinh nghiệm cuối trận
        if (DataValues.vHealthCurrent <= 0) //Chạy anim die khi hết máu
        {
            goto End;
        }
        Battle.DamageShow(this.transform.position, 1, Team, damage); //Gọi hàm show chỉ số từ battle system
        yield return new WaitForSeconds(timeRespawn);
        time += timeRespawn;
        count++; //Tăng số lần đếm 
        if (time > timeTotal)
            goto End;
        else goto Begin;
        End: yield return new WaitForSeconds(0);
        StatusObject[4].SetActive(false);
        StatusObjectTime.SetActive(false); //Disable đếm time hiệu ứng
    }

    /// <summary>
    /// Tăng chỉ số cho hero trong 1 khoảng thời gian, dành cho các hiệu ứng buff
    /// </summary>
    /// <param name="valueName">Tên giá trị cần tăng</param>
    /// <param name="value">giá trị cần tăng</param>
    /// <param name="timeAction">thời gian hiệu lực</param>
    /// <returns></returns>
    public IEnumerator ActionBuffValues(string valueName, float value, float timeAction)
    {

        #region Tăng chỉ số tạm thời 

        switch (valueName)
        {
            case "vLifeSteal": //hút máu thích ứng, tăng cả hút máu phép và hút máu vật lý
                DataValues.vLifeStealMagic += value;
                DataValues.vLifeStealPhysic += value;
                break;
            case "vArmor": //Giáp
                DataValues.vArmor += value;
                break;
            case "vMagicResist": //Kháng phép
                DataValues.vMagicResist += value;
                break;
            case "vAtk": //Sát thương vly
                DataValues.vAtk += value;
                break;
            case "vMagic": //Phép thuật
                DataValues.vMagic += value;
                break;
            case "vAtkSpeed": //Tốc độ đánh
                DataValues.vAtkSpeed += value;
                break;
            default:
                break;
        }

        #endregion

        yield return new WaitForSeconds(timeAction);

        #region Giảm sau khoảng thời gian 
        try
        {
            switch (valueName)
            {
                case "vLifeSteal": //hút máu thích ứng, tăng cả hút máu phép và hút máu vật lý
                    DataValues.vLifeStealMagic -= value;
                    DataValues.vLifeStealPhysic -= value;
                    break;
                case "vArmor": //Giáp
                    DataValues.vArmor -= value;
                    break;
                case "vMagicResist": //Kháng phép
                    DataValues.vMagicResist -= value;
                    break;
                case "vAtk": //Sát thương vly 
                    DataValues.vAtk -= value;
                    break;
                case "vMagic": //Phép thuật
                    DataValues.vMagic -= value;
                    break;
                case "vAtkSpeed": //Tốc độ đánh
                    DataValues.vAtkSpeed -= value;
                    break;
                default:
                    break;
            }
        }
        catch { }
        #endregion
    }

    #endregion

    #region Xử lý va chạm 

    /// <summary>
    /// Xử lý va chạm với kill đối thủ
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        //Tương tác với skill đối phương
        if (!IsNoDame) //Nếu không miễn nhiễm sát thương thì mới tính dame
            if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[4])) || (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[3])))
            {
                try
                {
                    var skillcore = col.transform.GetComponent<SkillCore>(); //Get component skill để lấy thông tin chủ sở hữu skill
                    var x3Dame = DataValues.vTripleDamage > 0 && UnityEngine.Random.Range(0, 100) <= DataValues.vTripleDamage ? true : false; //Tỉ lệ x3 đòn đánh
                    var x2Dame = !x3Dame && DataValues.vDoubleDamage > 0 && UnityEngine.Random.Range(0, 100) <= DataValues.vDoubleDamage ? true : false; //Tỉ lệ x2 đòn đánh
                    for (int i = 0; i < (x3Dame ? 3 : x3Dame ? 2 : 1); i++)
                    {

                        //Tạo số máu bị trừ theo % nếu có
                        var dmgPercentHealth = skillcore.PercentHealthAtkEnable ? DataValues.vHealth * (skillcore.PercentHealthCreated / 100f) : 0;
                        skillcore.PercentHealthAtkEnable = false; //Đưa về trạng thái ko trừ máu theo %
                        skillcore.PercentHealthCreated = 0; //% máu bị trừ = 0

                        //Tính sát thương gây ra của skill đối với hero
                        var damageresult = dmgPercentHealth + BattleCore.Damage(skillcore.DataValues, DataValues, skillcore.DamagePercent, skillcore.SkillType); //Sát thương mà đối phương gây ra cho mình
                        var dmgReflect = damageresult > 0 && DataValues.vDamageReflect > 0 ? BattleCore.DamageCaculator(damageresult * DataValues.vDamageReflect / 100f, damageresult * DataValues.vDamageReflect / 100f, DataValues.vArmor, DataValues.vMagicResist, DataValues.vLethality, DataValues.vMagicPenetration, 100, skillcore.SkillType, 0) : 0; //Sát thương phản lại đối phương nếu có phản dmg
                        var dmgHeathRegen = skillcore.SkillType.Equals(0) ? (skillcore.Hero.DataValues.vLifeStealPhysic > 0 ? damageresult * skillcore.Hero.DataValues.vLifeStealPhysic / 100f : 0) : (skillcore.Hero.DataValues.vLifeStealMagic > 0 ? damageresult * skillcore.Hero.DataValues.vLifeStealMagic / 100f : 0); //Hút máu/hút máu phép
                        dmgReflect -= dmgHeathRegen;
                        dmgReflect = dmgReflect > 0 && ((int)dmgReflect).Equals(0) ? 1 : dmgReflect;

                        //Giảm máu của hero
                        DataValues.vHealthCurrent -= DataValues.vHealthCurrent > 0 ? damageresult : 0;
                        skillcore.Hero.DataValues.vHealthCurrent -= skillcore.Hero.DataValues.vHealthCurrent > 0 ? dmgReflect : 0; //Giảm máu của đối phương nếu bị phản dmg hoặc tăng máu nếu có hút máu
                        DefenseValue += damageresult; //Cộng điểm sát thương phải nhận trong trận, để tính điểm kinh nghiệm cuối trận
                        skillcore.Hero.DamageValue += damageresult; //Cộng điểm sát gây ra cho đối phương, để tính điểm kinh nghiệm cuối trận
                        if (skillcore.RatioStatus >= UnityEngine.Random.Range(1, 101)) //Tính toán tỉ lệ skill gây ra hiệu ứng
                            AssignStatus(skillcore); //Thay đổi trạng thái và hiệu ứng nếu có
                        //Gọi hàm show damage từ battle system
                        Battle.DamageShow(this.transform.position, skillcore.SkillType, Team, damageresult);
                        if (dmgReflect > 0) //Show dmg bị phản lại cho đối phương
                            Battle.DamageShow(skillcore.Hero.transform.position, 3, skillcore.Hero.Team, dmgReflect);

                        #region Nếu skill của đối phương có hiệu ứng thiêu đốt
                        if (col.gameObject.tag.ToString().Equals(BattleCore.TagEffectFireBurn))
                        {
                            StartCoroutine(ActionFireBurn(skillcore.SkillType.Equals(0) ? skillcore.DataValues.vAtk * (skillcore.PercentDamageFireBurn / 100f) * BattleCore.TimePerEffectToHero : skillcore.DataValues.vMagic * (skillcore.PercentDamageFireBurn / 100f) * BattleCore.TimePerEffectToHero,
                                BattleCore.TimeCauseEffect(DataValues, skillcore.TimeStatus), BattleCore.TimePerEffectToHero));
                        }
                        #endregion

                        #region Khi bị hạ gục
                        if (DataValues.vHealthCurrent <= 0)
                        {
                            //Nội tại hero 5
                            BattleCore.Hero5IntrinsicEnable = true;
                            BattleCore.Hero5IntrinsicTeam = Team;
                            //Nội tại hero 8
                            BattleCore.Hero8IntrinsicEnable = true;
                            BattleCore.Hero8IntrinsicTeam = Team;
                        }
                        #endregion
                    }
                }
                catch { }
            }
        //Tương tác với skill team mình 
        if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[3])) ||
            (Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[7])) ||
            (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[4])) ||
            (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[8])))
        {
            try
            {
                #region Hồi máu khi va chạm với skill team mình 
                if (col.tag.ToString().Equals(BattleCore.TagKillReHP))
                {
                    var skillcore = col.transform.GetComponent<SkillCore>(); //Get component skill để lấy thông tin chủ sở hữu skill

                    //Tính lượng máu sẽ đc hồi lại
                    var valuesresult = BattleCore.RespawnHPValues(skillcore.DataValues, skillcore.DamagePercent, skillcore.SkillType);
                    DataValues.vHealthCurrent += DataValues.vHealthCurrent >= DataValues.vHealth ? 0 : valuesresult;

                    //Gọi hàm show chỉ số từ battle system
                    Battle.DamageShow(this.transform.position, 2, Team, valuesresult);
                }

                #endregion
            }
            catch { }
        }
    }
    /// <summary>
    /// Xử lý va chạm với đối thủ
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        //Va chạm với đối phương - Dành cho hero cận chiến lao tới đánh gần
        if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
        {

            //Nếu đang lao tới
            if (HeroAction.Equals(Actions.MoveToEnemy))
            {
                Battle.UpdatePisitionCenterCombat(col.transform.position.x);
                HeroAction = Actions.AttackAction;

                //Ưu tiên skill trước đánh thường
                if (SkillListAciton.Count > 1)
                    if (SkillListAciton[1].Equals(1))
                    {
                        var temp = SkillListAciton[0];
                        SkillListAciton[0] = SkillListAciton[1];
                        SkillListAciton[1] = temp;
                    }

                ChangeAnim(GetAnimationName(SkillListAciton[0]));
                ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
            }
        }

        //Va chạm với skill đối phương (những skill va chạm)
        if (!IsNoDame) //Nếu không miễn nhiễm sát thương thì mới tính dame
            if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[6])) || (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[5])))
            {
                try
                {
                    var skillcore = col.transform.GetComponent<SkillCore>(); //Get component skill để lấy thông tin chủ sở hữu skill

                    var x3Dame = DataValues.vTripleDamage > 0 && UnityEngine.Random.Range(0, 100) <= DataValues.vTripleDamage ? true : false; //Tỉ lệ x3 đòn đánh
                    var x2Dame = !x3Dame && DataValues.vDoubleDamage > 0 && UnityEngine.Random.Range(0, 100) <= DataValues.vDoubleDamage ? true : false; //Tỉ lệ x2 đòn đánh
                    for (int i = 0; i < (x3Dame ? 3 : x3Dame ? 2 : 1); i++)
                    {

                        //Tạo số máu bị trừ theo % nếu có
                        var dmgPercentHealth = skillcore.PercentHealthAtkEnable ? DataValues.vHealth * (skillcore.PercentHealthCreated / 100f) : 0;
                        skillcore.PercentHealthAtkEnable = false; //Đưa về trạng thái ko trừ máu theo %
                        skillcore.PercentHealthCreated = 0; //% máu bị trừ = 0

                        //Tính sát thương gây ra của skill đối với hero
                        var damageresult = dmgPercentHealth + BattleCore.Damage(skillcore.DataValues, DataValues, skillcore.DamagePercent, skillcore.SkillType); //Sát thương mà đối phương gây ra cho mình
                        var dmgReflect = damageresult > 0 && DataValues.vDamageReflect > 0 ? BattleCore.DamageCaculator(damageresult * DataValues.vDamageReflect / 100f, damageresult * DataValues.vDamageReflect / 100f, DataValues.vArmor, DataValues.vMagicResist, DataValues.vLethality, DataValues.vMagicPenetration, 100, skillcore.SkillType, 0) : 0; //Sát thương phản lại đối phương nếu có phản dmg
                        var dmgHeathRegen = skillcore.SkillType.Equals(0) ? (skillcore.Hero.DataValues.vLifeStealPhysic > 0 ? damageresult * skillcore.Hero.DataValues.vLifeStealPhysic / 100f : 0) : (skillcore.Hero.DataValues.vLifeStealMagic > 0 ? damageresult * skillcore.Hero.DataValues.vLifeStealMagic / 100f : 0); //Hút máu/hút máu phép
                        dmgReflect -= dmgHeathRegen;
                        dmgReflect = dmgReflect > 0 && ((int)dmgReflect).Equals(0) ? 1 : dmgReflect;

                        //Giảm máu của hero
                        DataValues.vHealthCurrent -= DataValues.vHealthCurrent > 0 ? damageresult : 0;
                        skillcore.Hero.DataValues.vHealthCurrent -= skillcore.Hero.DataValues.vHealthCurrent > 0 ? dmgReflect : 0; //Giảm máu của đối phương nếu bị phản dmg hoặc tăng máu nếu có hút máu
                        DefenseValue += damageresult; //Cộng điểm sát thương phải nhận trong trận, để tính điểm kinh nghiệm cuối trận
                        skillcore.Hero.DamageValue += damageresult; //Cộng điểm sát gây ra cho đối phương, để tính điểm kinh nghiệm cuối trận
                        if (skillcore.RatioStatus >= UnityEngine.Random.Range(1, 101)) //Tính toán tỉ lệ skill gây ra hiệu ứng
                            AssignStatus(skillcore); //Thay đổi trạng thái và hiệu ứng nếu có
                        //Gọi hàm show damage từ battle system
                        Battle.DamageShow(this.transform.position, skillcore.SkillType, Team, damageresult);
                        if (dmgReflect > 0) //Show dmg bị phản lại cho đối phương
                            Battle.DamageShow(skillcore.Hero.transform.position, 3, skillcore.Hero.Team, dmgReflect);

                        #region Nếu skill của đối phương có hiệu ứng thiêu đốt
                        if (col.gameObject.tag.ToString().Equals(BattleCore.TagEffectFireBurn))
                        {
                            StartCoroutine(ActionFireBurn(skillcore.SkillType.Equals(0) ? skillcore.DataValues.vAtk * (skillcore.PercentDamageFireBurn / 100f) * BattleCore.TimePerEffectToHero : skillcore.DataValues.vMagic * (skillcore.PercentDamageFireBurn / 100f) * BattleCore.TimePerEffectToHero,
                                BattleCore.TimeCauseEffect(DataValues, skillcore.TimeStatus), BattleCore.TimePerEffectToHero));
                        }
                        #endregion

                        #region Khi bị hạ gục
                        if (DataValues.vHealthCurrent <= 0)
                        {
                            //Nội tại hero 5
                            BattleCore.Hero5IntrinsicEnable = true;
                            BattleCore.Hero5IntrinsicTeam = Team;
                            //Nội tại hero 8
                            BattleCore.Hero8IntrinsicEnable = true;
                            BattleCore.Hero8IntrinsicTeam = Team;
                        }
                        #endregion
                    }
                }
                catch { }
            }
    }
    /// <summary>
    /// /// Kiểm tra xem có đang chạm đối thủ hay ko
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnCollisionStay2D(Collision2D col)
    {
        if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
        {
            StayingCollision = true;
            if (HeroAction.Equals(Actions.MoveToEnemy)) //Hàm này chặn hero cận chiến bỏ qua địch đằng trước để đánh địch phía sau gây bug
            {
                Battle.UpdatePisitionCenterCombat(col.transform.position.x);
                HeroAction = Actions.AttackAction;
                ChangeAnim(GetAnimationName(SkillListAciton[0]));
                ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
            }
        }
    }
    public virtual void OnCollisionExit2D(Collision2D col)
    {
        //if ((Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
        StayingCollision = false;
    }
    #endregion
}