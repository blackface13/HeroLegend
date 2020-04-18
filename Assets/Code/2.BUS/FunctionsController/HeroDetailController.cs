using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HeroDetailController : MonoBehaviour {

    #region Hero System 
    public GameObject[] ObjectController;
    private GameObject HeroAvt;
    private int HorizontalQuantityEquip = 7; //Số slot trên 1 hàng ngang của hệ thống trang bị vật phẩm cho hero
    private List<GameObject> ListBackgroundItemEquip = new List<GameObject> ();
    private List<GameObject> ListItemEquip = new List<GameObject> ();
    private ItemModel ItemViewing;
    private int SlotHeroViewing; //Lưu slot hero đang xem
    public Text[] ItemInforText; //Set in interface
    private int ItemSlotViewing = -1; //Slot item đang được xem, dùng để bán hoặc xem item tiếp theo
    public Image ItemDetailColor; //Hình nền phía sau của item, thể hiện màu item, chi tiết item
    public Image ItemDetailImg; //Hình của item
    #endregion

    // Start is called before the first frame update
    void Start () {
        SetupLanguage ();
        ObjectController[0].transform.position = ObjectController[21].transform.position; //Set tọa độ icon loading tại button thông tin
        Destroy (HeroAvt);
        SetupItemInEquip ();
        ShowHeroInforDetail (GlobalVariables.HeroSlotSelected);
    }

    private void SetupLanguage () {
        ItemInforText[16].text = Languages.lang[165]; // = "Nội tại và kỹ năng";
        ItemInforText[4].text = Languages.lang[262]; // = "Thông tin";
        ItemInforText[5].text = ItemInforText[21].text = Languages.lang[263]; // = "Trang bị";
        ItemInforText[6].text = Languages.lang[13]; // = "Kỹ năng";
        ItemInforText[7].text = ItemInforText[13].text = Languages.lang[15]; // = "Tiểu sử";
        ItemInforText[8].text = Languages.lang[14]; // = "Đặc biệt";
        ItemInforText[9].text = Languages.lang[23]; // = "Level: ";
        ItemInforText[10].text = Languages.lang[22]; // = "Chỉ số";
        ItemInforText[11].text = Languages.lang[79]; // = "Gỡ nhanh";
        ItemInforText[12].text = Languages.lang[265]; // = "Lựa chọn trang bị";
        ItemInforText[22].text = Languages.lang[48]; // = "Gỡ bỏ";
        ItemInforText[23].text = Languages.lang[154]; // = "Nâng cấp";
        ItemInforText[24].text = Languages.lang[155]; // = "Nâng phẩm";
        ItemInforText[14].text = "";
        for (int i = 200; i < 238; i++) {
            ItemInforText[14].text += Languages.lang[i] + "\n";
        }
        for (int i = 25; i < 31; i++) {
            ItemInforText[i].text = Languages.lang[i + 263];
        }
    }

    #region Heroes Functions 

    /// <summary>
    /// Click vào nhân vật và hiển thị chi tiết thông tin nhân vật
    /// </summary>
    private void ShowHeroInforDetail (int slotHero) {
        GlobalVariables.HeroSlotSelected = slotHero;
        if (HeroAvt != null)
            Destroy (HeroAvt);
        HeroAvt = (GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/HeroAvt/Hero" + DataUserController.Heroes.DBHeroes[GlobalVariables.HeroSlotSelected].ID.ToString ()), new Vector3 (-11.5f, -3f, 0), Quaternion.identity);
        HeroAvt.GetComponent<SortingGroup> ().sortingOrder = 302;
        HeroAvt.transform.SetParent (ObjectController[1].transform, true);
        HeroAvt.transform.localPosition = new Vector3(HeroAvt.transform.localPosition.x, HeroAvt.transform.localPosition.y, 0);
        CaculatorValueHeroes (GlobalVariables.HeroSlotSelected);
        SlotHeroViewing = GlobalVariables.HeroSlotSelected;
        SetupItemEquiped ();
        //ObjectController[1].SetActive (true); //Show UI
        //Text nội tại và kỹ năng
        ItemInforText[3].text = Languages.lang[166] + "\n" + Languages.HeroIntrinsic[DataUserController.Heroes.DBHeroes[GlobalVariables.HeroSlotSelected].ID - 1] + "\n\n\n" +
            Languages.lang[167] + "\n" + Languages.HeroSkillDescription[DataUserController.Heroes.DBHeroes[GlobalVariables.HeroSlotSelected].ID - 1];
    }

    /// <summary>
    /// Tính toán lại chỉ số của heroes
    /// </summary>
    private void CaculatorValueHeroes (int slotHero) {

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

        var countItemEquip = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip.Count > 0 ? DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip.Count : 0;
        if (countItemEquip > 0) {
            for (int i = 0; i < countItemEquip; i++) {
                //Sát thương vật lý
                var valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtk; //Chỉ số gốc
                var valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                var valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                var valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkPlus / 100f : 0; //% tăng thêm
                vAtkPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Sát thương phép
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagic; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPlus / 100f : 0; //% tăng thêm
                vMagicPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Máu
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealth; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthPlus / 100f : 0; //% tăng thêm
                vHealthPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Năng lượng
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMana; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaPlus / 100f : 0; //% tăng thêm
                vManaPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Giáp
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmor; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmorPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vArmorPlus / 100f : 0; //% tăng thêm
                vArmorPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);
                //Kháng phép
                valueOriginal = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResist; //Chỉ số gốc
                valueBonusLevel = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel > 0 ? valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f : 0); //Cộng chỉ số theo level
                valueBonusColor = (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor > 0 ? (valueOriginal + (valueOriginal * (DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemLevel * ItemCoreSetting.UpgradePerLevel) / 100f)) * ((DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].ItemColor * ItemCoreSetting.UpgradePerColor) / 100f) : 0); //Cộng chỉ số theo màu sắc
                valueBonusPlus = DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResistPlus > 0 ? (valueOriginal + valueBonusLevel + valueBonusColor) * DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicResistPlus / 100f : 0; //% tăng thêm
                vMagicResistPlus += (valueOriginal + valueBonusLevel + valueBonusColor + valueBonusPlus);

                vHealthRegenPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vHealthRegen; //Chỉ số hồi máu mỗi giây
                vManaRegenPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vManaRegen; //Chỉ số hồi mana mỗi giây
                vDamageEarthPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageEarth; //Sát thương hệ đất
                vDamageWaterPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageWater; //Sát thương hệ nước
                vDamageFirePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageFire; //Sát thương hệ lửa
                vDefenseEarthPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseEarth; //Kháng hệ đất
                vDefenseWaterPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseWater; //Kháng hệ nước
                vDefenseFirePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseFire; //Kháng hệ hỏa
                vAtkSpeedPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vAtkSpeed; //% Tốc độ tấn công cơ bản tăng thêm
                vLifeStealPhysicPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLifeStealPhysic; //% hút máu
                vLifeStealMagicPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLifeStealMagic; //% hút máu phép
                vLethalityPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vLethality; //% Xuyên giáp
                vMagicPenetrationPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vMagicPenetration; //% Xuyên phép
                vCriticalPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vCritical; //% chí mạng
                vTenacityPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vTenacity; //% kháng hiệu ứng
                vCooldownReductionPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vCooldownReduction; //% Giảm tgian hồi chiêu
                vDamageExcellentPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageExcellent; //% Sát thương hoàn hảo (bỏ qua giáp hoặc kháng phép). max = 10%
                vDefenseExcellentPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDefenseExcellent; //% phong thu hoàn hảo (ko bị đánh trúng). max = 10%
                vDoubleDamagePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDoubleDamage; //Tỉ lệ x2 đòn đánh max = 10%
                vTripleDamagePlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vTripleDamage; //Tỉ lệ x3 đòn đánh max = 10%
                vDamageReflectPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vDamageReflect; //Phản hồi % sát thương. max = 5%
                vRewardPlusPlus += DataUserController.Heroes.DBHeroes[slotHero].ItemsEquip[i].vRewardPlus; //Tăng lượng vàng rơi ra vào cuối trận. max = 100%

            }
        }

        ItemInforText[15].text = DataUserController.Heroes.DBHeroes[slotHero].Name; //Tên hero
        ItemInforText[1].text = Languages.lang[23] + DataUserController.Heroes.DBHeroes[slotHero].Level.ToString (); //Cấp độ nhân vật
        ObjectController[20].transform.localScale = new Vector3 (DataUserController.Heroes.DBHeroes[slotHero].EXP / Module.NextExp (DataUserController.Heroes.DBHeroes[slotHero].Level), 1, 1); //Thanh exp
        var heroOriginal = DataUserController.HeroesDefault.DBHeroesDefault.Find (x => x.ID == DataUserController.Heroes.DBHeroes[slotHero].ID);
        ItemInforText[0].text = Languages.lang[(int) heroOriginal.Type + 15]; //Class nhân vật

        ItemInforText[2].text = String.Format ("{0:0.#}", (heroOriginal.vHealth + vHealthPlus + (heroOriginal.vHealthPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMana) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vAtk + vAtkPlus + (heroOriginal.vAtkPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagic + vMagicPlus + (heroOriginal.vMagicPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vArmor + vArmorPlus + (heroOriginal.vArmorPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagicResist + vMagicResistPlus + (heroOriginal.vMagicResistPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vHealthRegen + vHealthRegenPlus + (heroOriginal.vHealthRegenPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vManaRegen + vManaRegenPlus + (heroOriginal.vManaRegenPerLevel * DataUserController.Heroes.DBHeroes[slotHero].Level))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageEarth + vDamageEarthPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageWater + vDamageWaterPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDamageFire + vDamageFirePlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseEarth + vDefenseEarthPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseWater + vDefenseWaterPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vDefenseFire + vDefenseFirePlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vAtkSpeed + (heroOriginal.vAtkSpeed * vAtkSpeedPlus / 100f))) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vCooldown[1])) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLifeStealPhysic + vLifeStealPhysicPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLifeStealMagic + vLifeStealMagicPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vLethality + vLethalityPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vMagicPenetration + vMagicPenetrationPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vCritical + vCriticalPlus)) + "\n" +
            String.Format ("{0:0.#}", (heroOriginal.vTenacity + vTenacityPlus)) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vCooldownReduction + vCooldownReductionPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDamageExcellent + vDamageExcellentPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDefenseExcellent + vDefenseExcellentPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDoubleDamage + vDoubleDamagePlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vTripleDamage + vTripleDamagePlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vDamageReflect + vDamageReflectPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vRewardPlus + vRewardPlusPlus) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.Mana_skill[0]) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.Mana_skill[1]) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vHealthPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vAtkPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMagicPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vArmorPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vMagicResistPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vHealthRegenPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vManaRegenPerLevel) + "\n" +
            String.Format ("{0:0.#}", heroOriginal.vCooldownReductionPerLevel);
        #endregion
    }

    //Khởi tạo các item đã được trang bị cho hero
    private void SetupItemEquiped () {
        var count = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;
        for (int i = 0; i < 6; i++) {
            ObjectController[i + 8].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/none");
            ObjectController[i + 8].GetComponent<Button> ().onClick.RemoveAllListeners ();
        }
        for (int i = 0; i < 6; i++) {
            if (i < count) {
                var temp = i;
                ObjectController[i + 8].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemType + @"/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemID);
                ObjectController[i + 8].GetComponent<Button> ().onClick.RemoveAllListeners ();
                ObjectController[i + 8].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip, 1));
                if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6 (xem trong module phần inventory)
                    ObjectController[i + 14].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemColor);
                ObjectController[i + 8].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[i].ItemLevel.ToString ();

            } else {
                ObjectController[i + 14].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/0");
                ObjectController[i + 8].transform.GetChild (0).GetComponent<Text> ().text = "";
            }
        }
    }

    /// <summary>
    /// Đưa dữ liệu các item trang bị lên view trong hệ thống trang bị item cho nhân vật
    /// </summary>
    private void SetupItemInEquip () {
        var listTempItemEquip = DataUserController.Inventory.DBItems.Where (x => x.ItemTypeMode == global::ItemModel.TypeMode.Equip).ToList ();
        var totalItemEquip = listTempItemEquip.Count;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 165f; //Khoảng cách giữa các object
        ObjectController[7].GetComponent<RectTransform> ().sizeDelta = totalItemEquip % HorizontalQuantityEquip == 0 ? new Vector2 (0, (totalItemEquip / HorizontalQuantityEquip) * regionSpace) : new Vector2 (0, ((totalItemEquip / HorizontalQuantityEquip) + 1) * regionSpace);
        float verticalcounttemp = totalItemEquip <= HorizontalQuantityEquip ? 0 : ((totalItemEquip % HorizontalQuantityEquip).Equals (0) ? (totalItemEquip - 1) / HorizontalQuantityEquip * 80 : (totalItemEquip / HorizontalQuantityEquip) * 80);
        float horizonXOriginal = -493;
        //Thêm mới các object ban đầu nếu có sự chênh lệch số lượng
        if (ListBackgroundItemEquip.Count < totalItemEquip) {
            var count = totalItemEquip - ListBackgroundItemEquip.Count;
            for (int i = 0; i < count; i++) {
                ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (0, 0, 0), Quaternion.identity));
                ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            }
        }
        //Set các object
        for (int i = 0; i < totalItemEquip; i++) {
            var temp = i;
            //ListBackgroundItemEquip[i].transform.parent = null;
            ListBackgroundItemEquip[i].transform.SetParent (ObjectController[7].transform, false); //Đẩy prefab vào scroll
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (horizonXOriginal + regionSpace * i_temp_x, 10 + verticalcounttemp + regionSpace * -i_temp_y, 0);
            //ListBackgroundItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/ItemBody"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 160 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity));
            ListBackgroundItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (150, 150);
            ListBackgroundItemEquip[i].transform.localScale = new Vector3 (1, 1, 1);
            if (listTempItemEquip[i].ItemColor < ItemCoreSetting.ItemColorMax) //Giới hạn màu sắc từ 0-6
                ListBackgroundItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/BorderItem/" + listTempItemEquip[i].ItemColor);
            //ListItemEquip.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Items"), new Vector3 (0, 0, 0), Quaternion.identity));
            ListItemEquip[i].transform.SetParent (ListBackgroundItemEquip[i].transform, false);
            ListItemEquip[i].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images/Items/" + listTempItemEquip[i].ItemType + @"/" + listTempItemEquip[i].ItemID);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = "";
            ListItemEquip[i].GetComponent<Button> ().onClick.RemoveAllListeners ();
            ListItemEquip[i].GetComponent<Button> ().onClick.AddListener (() => ItemClick (temp, listTempItemEquip, 0));
            ListItemEquip[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (130, 130);
            ListItemEquip[i].transform.GetChild (0).GetComponent<Text> ().text = Languages.lang[23] + listTempItemEquip[i].ItemLevel.ToString ();
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantityEquip == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
        //Xóa object nếu dư
        if (ListBackgroundItemEquip.Count > totalItemEquip) {
            for (int i = ListBackgroundItemEquip.Count - 1; i >= totalItemEquip; i--) {
                Destroy (ListBackgroundItemEquip[i]);
                Destroy (ListItemEquip[i]);
                ListBackgroundItemEquip.RemoveAt (i);
                ListItemEquip.RemoveAt (i);
            }
        }
    }

    /// <summary>
    /// Gỡ trang bị từ nhân vật
    /// </summary>
    /// <param name="isRemoveAll">false: gỡ 1 item, true: gỡ toàn bộ</param>
    private void RemoveItemFromHero (bool isRemoveAll) {
        var count = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;
        if (count > 0) //Nếu có trang bị thì mới gỡ
        {
            if (isRemoveAll) { //Nếu là chế độ gỡ toàn bộ
                if (DataUserController.User.InventorySlot - DataUserController.Inventory.DBItems.Count >= DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count) {
                    for (int i = 0; i < count; i++) {
                        var itemTemp = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip[0];
                        InventorySystem.AddItemToInventory (itemTemp);
                        DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.RemoveAt (0);
                    }
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                } else {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                }
            } else //Gỡ 1 item
            {
                if (DataUserController.Inventory.DBItems.Count < DataUserController.User.InventorySlot) {
                    InventorySystem.AddItemToInventory (ItemViewing);
                    DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.RemoveAt (ItemSlotViewing);
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[80])); //Thông báo gỡ trang bị thành công
                } else {
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[175])); //Vượt quá giới hạn rương đồ
                }
            }
            SetupItemInEquip ();
            SetupItemEquiped ();
            ButtonFunctions (7);
            DataUserController.SaveInventory ();
            DataUserController.SaveHeroes ();
        }
    }

    /// <summary>
    /// Phân giải item trang bị từ nhân vật
    /// </summary>
    /// <returns></returns>
    private IEnumerator BreakItemFromHero () {
        try {
            var count = DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count;
            if (count > 0) //Nếu có trang bị thì mới break
            {
                //Xóa item trang bị khỏi nhân vật
                DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.RemoveAt (ItemSlotViewing);
            }
        } catch { }
        yield return null;
    }

    /// <summary>
    /// Các button trong chi tiết nhân vật
    /// </summary>
    /// <param name="type">-1: thoát chi tiết nhân vật, 0: thông tin nhân vật, 1 Trang bị nhân vật, 2 Kỹ năng, 3 Tiểu sử, 4 đặc biệt</param>
    public void DetailHeroButtonFunctions (int type) {
        if (type != -1) {
            ObjectController[2].SetActive (false);
            ObjectController[3].SetActive (false);
            ObjectController[4].SetActive (false);
            ObjectController[5].SetActive (false);
            ObjectController[0].transform.position = ObjectController[21 + type].transform.position;
        }
        switch (type) {
            case -1:
                ObjectController[6].SetActive (false);
                break;
            case 0:
                CaculatorValueHeroes (SlotHeroViewing);
                ObjectController[2].SetActive (true);
                break;
            case 1:
                ObjectController[3].SetActive (true);
                break;
            case 2:
                ObjectController[4].SetActive (true);
                break;
            case 3:
                ObjectController[5].SetActive (true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Click vào item trong thùng đồ
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="inventory"></param>
    /// <param name="type">Kiểu xem: null = xem từ inventory, 0 = xem từ danh sách trang bị, 1 = xem trang bị đã được trang bị</param>
    public void ItemClick (int slot, List<ItemModel> inventory, int? type) {
        GlobalVariables.ItemViewing = inventory[slot];
        GlobalVariables.ItemViewingType = type.Equals (0) ? 1 : 2; //Kiểu xem item
        GameSystem.InitializePrefabUI (6); //Hiển thị thông tin item click vào
        ItemSlotViewing = slot; //Gán slot item đang được click vào
        ItemViewing = inventory[slot]; //Gán tạm item để thao tác
        StartCoroutine (WaitingCloseItemDetailUI (type));

        HeroAvt.SetActive (false); //Ẩn Hình ảnh hero chuyển động
    }

    /// <summary>
    /// Chờ đóng UI chi tiết item 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitingCloseItemDetailUI (int? type) {
        yield return new WaitUntil (() => GameSystem.ItemDetailCanvasUI == null);
        //Success
        if (GameSystem.ItemDetailCanvasUI == null) {
            switch (GlobalVariables.ItemDetailAction) {
                case 1: //Trang bị item
                    ButtonFunctions (1);
                    break;
                case 2: //Gỡ trang bị item
                    ButtonFunctions (2);
                    break;
                case 5: //Phân giải trang bị thành công
                    if (type.Equals (1)) //Nếu phân giải item từ trang bị nhân vật => remove item khỏi nhân vật
                        StartCoroutine (BreakItemFromHero ());
                    StartCoroutine (GameSystem.ControlFunctions.ShowMessagecontinuity (GlobalVariables.NotificationText)); //GameSystem.ControlFunctions.ShowMessage( (GlobalVariables.NotificationText));//Thông báo đã nhận dc những gì
                    break;
            }
            //ButtonFunctions (2); //Refresh lại giá trị tiền tệ

            //Thiết lập lại các giá trị
            SetupItemInEquip ();
            SetupItemEquiped ();
            ButtonFunctions (7);
            DataUserController.SaveInventory ();
            DataUserController.SaveHeroes ();
            HeroAvt.SetActive (true); //Hiển thị ảnh hero chuyển động
        }
    }

    /// <summary>
    /// Nút thực hiện các chức năng của hệ thống nhân vật
    /// 0: Đóng thông tin chi tiết
    /// 1: Trang bị item
    /// 2: Gỡ bỏ item
    /// 3: Hero trước đó
    /// 4: Hero tiếp theo
    /// 5: Trang bị nhanh
    /// 6: Gỡ nhanh
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0: //Đóng thông tin chi tiết
                GameSystem.DisposePrefabUI (4); //
                // if (GlobalVariables.HeroSlotListSelected != null)
                //     GlobalVariables.HeroSlotListSelected.Clear (); //Clear mảng tạm dữ liệu hero 
                //ObjectController[1].SetActive (false);
                break;
            case 1: //Trang bị item cho hero
                if (DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Count >= 6) //Check slot full
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[128])); //Thông báo full slot
                else {
                    DataUserController.Heroes.DBHeroes[SlotHeroViewing].ItemsEquip.Add (ItemViewing); //Thêm trang bị đang xem vào hero
                    InventorySystem.RemoveItem (ItemViewing);
                    GameSystem.ControlFunctions.ShowMessage ((Languages.lang[129])); //Thông báo trang bị thành công
                }
                break;
            case 2: //Gỡ trang bị
                RemoveItemFromHero (false);
                break;
            case 3: //Prev nhân vật
                if (GlobalVariables.HeroSlotListSelected.Count > 1)
                    ShowHeroInforDetail (GlobalVariables.HeroSlotListSelected.FindIndex (x => x == GlobalVariables.HeroSlotSelected) <= 0 ? GlobalVariables.HeroSlotListSelected[GlobalVariables.HeroSlotListSelected.Count - 1] : GlobalVariables.HeroSlotListSelected[GlobalVariables.HeroSlotListSelected.FindIndex (x => x == GlobalVariables.HeroSlotSelected) - 1]);
                break;
            case 4: //Next nhân vật
                if (GlobalVariables.HeroSlotListSelected.Count > 1)
                    ShowHeroInforDetail (GlobalVariables.HeroSlotListSelected.FindIndex (x => x == GlobalVariables.HeroSlotSelected) >= GlobalVariables.HeroSlotListSelected.Count - 1 ? GlobalVariables.HeroSlotListSelected[0] : GlobalVariables.HeroSlotListSelected[GlobalVariables.HeroSlotListSelected.FindIndex (x => x == GlobalVariables.HeroSlotSelected) + 1]);
                break;
            case 5: //Trang bị nhanh
                GameSystem.ControlFunctions.ShowMessage (("Comming Soon...")); //Thông báo 
                break;
            case 6: //Gỡ trang bị nhanh
                RemoveItemFromHero (true);
                break;
            case 7: //Đóng cửa sổ item detail
                ObjectController[26].SetActive (false);
                HeroAvt.SetActive (true); //Hiển thị Hình ảnh hero chuyển động
                break;
            case 8: //Mở UI hướng dẫn
                HeroAvt.SetActive (false); //Hình ảnh hero chuyển động
                ObjectController[32].SetActive (true);
                break;
            case 9: //Đóng UI hướng dẫn
                HeroAvt.SetActive (true); //Hình ảnh hero chuyển động
                ObjectController[32].SetActive (false);
                break;
            default:
                break;
        }
    }
    #endregion

}