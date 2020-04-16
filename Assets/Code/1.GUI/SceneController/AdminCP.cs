using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackCore;
using Assets.Code._4.CORE;
//Admin CP.
public class AdminCP : MonoBehaviour
{

    #region HeroController 
    /// <summary>
    /// 0: Content chứa danh sách hero
	/// 1: Khung viền phải của khung thông tin
	/// 2: Khung viền giữa của khung thông tin
	/// 3: Khung viền trái của khung thông tin
    /// </summary>
    public GameObject[] HeroControllerUIObject;
    private int SlotHeroSelected = -1;
    private int HorizontalQuantity = 5;//Số slot trên 1 hàng ngang
    private List<GameObject> HeroList;//Danh sách hero
    private List<GameObject> HeroListAvatar;//Các object hero
    public InputField[] ValuesHero;
    #endregion

    /// <summary>
    /// Hàm start
    /// </summary>
    void Start()
    {
        #region Khởi tạo hoặc set Canvas thông báo cho Scene 
        try
        {
            GameSystem.MessageCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            GameSystem.DisableAllMessenger();
        }
        catch
        {
            GameSystem.Initialize();//Khởi tạo này dành cho scene nào test ngay
            GameSystem.MessageCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            GameSystem.MessageCanvas.GetComponent<Canvas>().planeDistance = 1;
        }
        #endregion
        HeroController();
    }

    /// <summary>
    /// Khởi tạo giao diện quản lý hero
    /// </summary>
    private void HeroController()
    {
        //Set kích thước UI theo tỉ lệ màn hình
        //Màn 16:9 chuẩn
        if (string.Format("{0:#,###.##}", Camera.main.aspect).Equals("1.78"))
        {
            HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta = new Vector2(873.3f, 841.21f);
            HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition = new Vector2(-19.36f, -53.2f);
            HeroControllerUIObject[3].GetComponent<RectTransform>().localPosition = new Vector2(-473.6f, -53.2f);
            HeroControllerUIObject[4].GetComponent<RectTransform>().sizeDelta = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.y + 80, HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.x - 70);
            HeroControllerUIObject[4].GetComponent<RectTransform>().localPosition = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.x, HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.y);
        }
        //Màn s8, mi9se
        if (string.Format("{0:#,###.##}", Camera.main.aspect).Equals("2.06") || string.Format("{0:#,###.##}", Camera.main.aspect).Equals("2.17"))
        {
            HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta = new Vector2(873.3f, 1139f);
            HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition = new Vector2(-168f, -53.2f);
            HeroControllerUIObject[3].GetComponent<RectTransform>().localPosition = new Vector2(-771.09f, -53.2f);
            HeroControllerUIObject[4].GetComponent<RectTransform>().sizeDelta = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.y + 80, HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.x - 70);
            HeroControllerUIObject[4].GetComponent<RectTransform>().localPosition = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.x, HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.y);
        }
        //Màn mi9se
        if (string.Format("{0:#,###.##}", Camera.main.aspect).Equals("2.17"))
        {
            HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta = new Vector2(873.3f, 1256.71f);//Mid UI - Edit
            HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition = new Vector2(-227.16f, -53.2f);//- Edit
            HeroControllerUIObject[3].GetComponent<RectTransform>().localPosition = new Vector2(-889.8f, -53.2f);//Left UI - Edit
            HeroControllerUIObject[4].GetComponent<RectTransform>().sizeDelta = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.y + 80, HeroControllerUIObject[2].GetComponent<RectTransform>().sizeDelta.x - 70);
            HeroControllerUIObject[4].GetComponent<RectTransform>().localPosition = new Vector2(HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.x, HeroControllerUIObject[2].GetComponent<RectTransform>().localPosition.y);
        }
        //Load Hero
        DataUserController.LoadHeroDefault();//Load các Hero mặc định
        HeroList = new List<GameObject>();
        HeroListAvatar = new List<GameObject>();
        float regionSpace = 170f;//Khoảng cách giữa các object
        int i_temp_x = 0;//Biến tạm hàng ngang
        int i_temp_y = 0;//Biến tạm hàng dọc
        var HeroCount = DataUserController.HeroesDefault.DBHeroesDefault.Count;//Biến tạm, số lượng hero đang sở hữu
        HeroControllerUIObject[0].GetComponent<RectTransform>().sizeDelta = HeroCount % HorizontalQuantity == 0 ? new Vector2(0, (HeroCount / HorizontalQuantity) * regionSpace) : new Vector2(0, ((HeroCount / HorizontalQuantity) + 1) * regionSpace);
        float verticalcounttemp = HeroControllerUIObject[0].GetComponent<RectTransform>().sizeDelta.y / 2 - 75;
        var vecXTemp = 0 - Camera.main.aspect * 5.5f;
        for (int i = 0; i < HeroCount; i++)
        {
            var temp = i;
            HeroList.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/HeroItem"), new Vector3(-340 + regionSpace * i_temp_x, -10 + verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity));
            HeroListAvatar.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectAvtLink + "Hero" + (i + 1).ToString()), new Vector3(vecXTemp, -7.6f, 0), Quaternion.identity));
            Destroy(HeroListAvatar[i].GetComponent<Rigidbody2D>());
            Destroy(HeroListAvatar[i].GetComponent<Collider2D>());
            HeroListAvatar[i].SetActive(false);
            // foreach (Component c in HeroListAvatar[i].GetComponents(typeof(Component)))
            // {
            //     if (c != null)
            //     {
            //         if(c.GetType().ToString().StartsWith("Hero"))
            //         Destroy(c);
            //     }
            // }
            HeroList[i].transform.SetParent(HeroControllerUIObject[0].transform, false); i_temp_x++;
            HeroList[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(Module.AvatarHeroLink + DataUserController.HeroesDefault.DBHeroesDefault[i].ID);
            HeroList[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DetailValuesHero(temp));
            //Space line
            if ((i + 1) % HorizontalQuantity == 0 && i != 0)
            {
                i_temp_y++;
                i_temp_x = 0;
            }
        }
    }

    /// <summary>
    /// Ẩn hiện hero
    /// </summary>
    /// <param name="slotID"></param>
    private void ShowAvtHero(int slotID)
    {
        var count = HeroListAvatar.Count;
        for (int i = 0; i < count; i++)
        {
            if (i.Equals(slotID))
                HeroListAvatar[i].SetActive(true);
            else
                HeroListAvatar[i].SetActive(false);
        }
    }

    /// <summary>
    /// Show chi tiết hero khi click vào
    /// </summary>
    /// <param name="slotHero"></param>
    private void DetailValuesHero(int slotHero)
    {
        ShowAvtHero(slotHero);
        if (SlotHeroSelected != -1)
        {
            MappingData(SlotHeroSelected, 1);
            DataUserController.SaveHeroBase();
        }
        SlotHeroSelected = slotHero;
        HeroControllerUIObject[5].SetActive(true);
        MappingData(slotHero, 0);
    }

    /// <summary>
    /// Map và map ngược dữ liệu để save hoặc show
    /// </summary>
    /// <param name="type">0 = đẩy data ra view, 1= save</param>
    private void MappingData(int slotHero, int type)
    {
        switch (type)
        {
            case 0://Đẩy data lên view
                ValuesHero[0].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Name;
                ValuesHero[1].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Description;
                ValuesHero[2].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Intrinsic;
                ValuesHero[3].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Level.ToString();
                ValuesHero[4].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vHealth.ToString();
                ValuesHero[5].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMana.ToString();
                ValuesHero[6].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vAtk.ToString();
                ValuesHero[7].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMagic.ToString();
                ValuesHero[8].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vArmor.ToString();
                ValuesHero[9].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMagicResist.ToString();
                ValuesHero[10].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vLifeStealPhysic.ToString();
                ValuesHero[11].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vLifeStealMagic.ToString();
                ValuesHero[12].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vLethality.ToString();
                ValuesHero[13].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMagicPenetration.ToString();
                ValuesHero[14].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vHealthRegen.ToString();
                ValuesHero[15].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vManaRegen.ToString();
                ValuesHero[16].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vCritical.ToString();
                ValuesHero[17].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vTenacity.ToString();
                ValuesHero[18].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vAtkSpeed.ToString();
                ValuesHero[19].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vCooldownReduction.ToString();
                ValuesHero[20].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Mana_skill[0].ToString();
                ValuesHero[21].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Mana_skill[1].ToString();
                ValuesHero[22].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vHealthPerLevel.ToString();
                ValuesHero[23].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vAtkPerLevel.ToString();
                ValuesHero[24].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMagicPerLevel.ToString();
                ValuesHero[25].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vArmorPerLevel.ToString();
                ValuesHero[26].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vMagicResistPerLevel.ToString();
                ValuesHero[27].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vHealthRegenPerLevel.ToString();
                ValuesHero[28].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vManaRegenPerLevel.ToString();
                ValuesHero[29].text = DataUserController.HeroesDefault.DBHeroesDefault[slotHero].vCooldownReductionPerLevel.ToString();
                HeroControllerUIObject[8].GetComponent<Dropdown>().value = (int)DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Type-1;
                break;
            case 1:

                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Name = ValuesHero[0].text;
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Description = ValuesHero[1].text;
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Intrinsic = ValuesHero[2].text;
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Level = int.Parse(ValuesHero[3].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vHealth = float.Parse(ValuesHero[4].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMana = float.Parse(ValuesHero[5].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vAtk = float.Parse(ValuesHero[6].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMagic = float.Parse(ValuesHero[7].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vArmor = float.Parse(ValuesHero[8].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMagicResist = float.Parse(ValuesHero[9].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vLifeStealPhysic = float.Parse(ValuesHero[10].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vLifeStealMagic = float.Parse(ValuesHero[11].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vLethality = float.Parse(ValuesHero[12].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMagicPenetration = float.Parse(ValuesHero[13].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vHealthRegen = float.Parse(ValuesHero[14].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vManaRegen = float.Parse(ValuesHero[15].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vCritical = float.Parse(ValuesHero[16].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vTenacity = float.Parse(ValuesHero[17].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vAtkSpeed = float.Parse(ValuesHero[18].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vCooldownReduction = float.Parse(ValuesHero[19].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Mana_skill[0] = float.Parse(ValuesHero[20].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].Mana_skill[1] = float.Parse(ValuesHero[21].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vHealthPerLevel = float.Parse(ValuesHero[22].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vAtkPerLevel = float.Parse(ValuesHero[23].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMagicPerLevel = float.Parse(ValuesHero[24].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vArmorPerLevel = float.Parse(ValuesHero[25].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vMagicResistPerLevel = float.Parse(ValuesHero[26].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vHealthRegenPerLevel = float.Parse(ValuesHero[27].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vManaRegenPerLevel = float.Parse(ValuesHero[28].text);
                DataUserController.HeroesDefault.DBHeroesDefault[SlotHeroSelected].vCooldownReductionPerLevel = float.Parse(ValuesHero[29].text);
                global::HeroesProperties.player_type _type = global::HeroesProperties.player_type.canchien;
                switch(HeroControllerUIObject[8].GetComponent<Dropdown>().value)
                {
                    case 0:
                    _type = global::HeroesProperties.player_type.canchien;
                    break;
                    case 1:
                    _type = global::HeroesProperties.player_type.satthu;
                    break;
                    case 2:
                    _type = global::HeroesProperties.player_type.hotro;
                    break;
                    case 3:
                    _type = global::HeroesProperties.player_type.dodon;
                    break;
                    case 4:
                    _type = global::HeroesProperties.player_type.xathu;
                    break;
                    case 5:
                    _type = global::HeroesProperties.player_type.phapsu;
                    break;
                }
                DataUserController.HeroesDefault.DBHeroesDefault[slotHero].Type = _type;
                break;
            default: break;
        }
    }

    /// <summary>
    /// Messenger box chờ đợi người chơi xác nhận
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitConfirmCommitData()
    {
        yield return new WaitUntil(() => GameSystem.ConfirmBoxResult != 0);//Chờ confirm
        //Accept
        if (GameSystem.ConfirmBoxResult == 1)
        {
            StartCoroutine(SyncData.HeroBlanceDefault(Securitys.Encrypt(JsonUtility.ToJson(DataUserController.HeroesDefault)).ToString()));
        }
        else
        GUIUtility.systemCopyBuffer = Securitys.Encrypt(JsonUtility.ToJson(DataUserController.HeroesDefault)).ToString();
    }

    /// <summary>
    /// Lưu dữ liệu đã chỉnh sửa
    /// </summary>
    /// <param name="saveType">0 = lưu hero</param>
    public void ButtonSave()
    {
        int saveType = 0;
        switch (saveType)
        {
            case 0://Lưu dữ liệu hero
                MappingData(SlotHeroSelected, 1);
                DataUserController.SaveHeroBase();
                GameSystem.ShowConfirmDialog("Dữ liệu đã được lưu, bạn có muốn đẩy dữ liệu lên server không?");
                StartCoroutine(WaitConfirmCommitData());
                //StartCoroutine(SyncData.Feedback("tank", "jlsdkfjds"));
                break;
            default: break;
        }
    }

}
