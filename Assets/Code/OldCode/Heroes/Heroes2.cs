using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Heroes2 : BaseHeroes
{

    #region Variables
    private GameObject[] EffAtk = new GameObject[3];
    private GameObject EffAtkUp;
    private GameObject[] EffSkill1 = new GameObject[3];
    private GameObject[] EffSkill2 = new GameObject[2];
    private GameObject[] EffSkill3 = new GameObject[3];
    private GameObject[] EffSkill4 = new GameObject[6];

    #endregion
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(GetValuesBase());//Setup values for this Enemy
        StartCoroutine(CreateObjectSkill());//Setup object skill
        StartCoroutine(CreateAudio());//Setup audio (khởi tạo âm thanh cho các skill)
        //======================================================================================
        FloatNumber[2] = Module.BASESKILL1SPEEDDEFAULT;//Skill 1 - Tốc độ lướt
        TimeRespawnSkill = new float[5];//Khởi tạo list time hồi chiêu
        TimeRespawnSkill[0] = 1500;//Skill 
        TimeRespawnSkill[1] = 2000;//Skill 
        TimeRespawnSkill[2] = 2000;//Skill 
        TimeRespawnSkill[3] = 2000;//Skill 
        TimeRespawnSkill[4] = 500;// Skill 
    }
    #region Create setups
    #region Create Object Skill
    private IEnumerator CreateObjectSkill()
    {
        //Normal atk
        for (int i = 0; i < EffAtk.Length; i++)
        {
            EffAtk[i] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Atk" + (i + 1).ToString()), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            EffAtk[i].SetActive(false);
        }
        //Skill 1
        EffSkill1[0] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill1R"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffSkill1[1] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill1L"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffSkill1[2] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill1Eff1"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffSkill1[0].SetActive(false);
        EffSkill1[1].SetActive(false);
        EffSkill1[2].SetActive(false);
        //Skill 2
        EffSkill2[0] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill2"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffSkill2[1] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill2Eff1"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffSkill2[0].SetActive(false);
        EffSkill2[1].SetActive(false);
        //Skill 3
        for (int i = 0; i < EffSkill3.Length; i++)
        {
            EffSkill3[i] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill3-" + (i + 1).ToString()), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            EffSkill3[i].SetActive(false);
        }
        //Skill 4. 0-2: object hiệu ứng. 3-5: object sát thương
        for (int i = 0; i < EffSkill4.Length / 2; i++)
        {
            EffSkill4[i] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill4Eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            EffSkill4[i].SetActive(false);
        }
        for (int i = 3; i < EffSkill4.Length; i++)
        {
            EffSkill4[i] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Skill4"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            EffSkill4[i].SetActive(false);
        }
        //Atk up 
        EffAtkUp = new GameObject();
        EffAtkUp = Instantiate(Resources.Load<GameObject>("Prefabs/H2AtkUp"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffAtkUp.SetActive(false);
        yield return new WaitForSeconds(0);
    }
    #endregion
    #region Create Object Autio
    private IEnumerator CreateAudio()
    {
        Sounds = GetComponent<AudioSource>();
        AudioNormalAtk = new AudioClip[9];
        AudioSkill1 = new AudioClip[1];
        AudioSkill2 = new AudioClip[1];
        for (int i = 0; i < AudioNormalAtk.Length; i++)
            AudioNormalAtk[i] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/Sword" + (i + 1).ToString()));
        AudioSkill1[0] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H2Skill1"));
        AudioSkill2[0] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H2Skill2"));
        yield return new WaitForSeconds(0);
    }
    #endregion
    #endregion
    private IEnumerator GetValuesBase()
    {
        // Prop = new List<SystemProperties>();
        // for (int i = 0; i < DBHeroes.DBHeroes.Count; i++)
        //     foreach (SystemProperties basehero in DBHeroes.DBHeroes)
        //     {
        //         if (basehero.ID.Equals(IDHeroes))
        //         {
        //             Prop.Add(basehero);
        //             Prop[0].MP_total = Prop[0].MP_curent = 9000;//Test
        //             //Tạo thể lực cho skill
        //             Prop[0].MPSkill = new float[5];
        //             Prop[0].MPSkill[0] = 10f;//Normal atk 1
        //             Prop[0].MPSkill[1] = 20f;//Thể lực tiêu hao của skill lướt né đòn
        //             Prop[0].MPSkill[2] = 25f;//Skill 1
        //             Prop[0].MPSkill[3] = 25f;//Skill 2
        //             Prop[0].MPSkill[4] = 25f;//Skill 3
        //             break;
        //         }
        //     }
        yield return new WaitForSeconds(0);
    }

    public override void Start()
    {
        base.Start();
        //Khắc phục tình trạng sai hướng của skill 4
        for (int i = 0; i < EffSkill4.Length / 2; i++)
        {
            EffSkill4[i].SetActive(true);
        }
    }
    void Update()
    {
        //print(BolNumber[6]);
        if (!Module.PAUSEGAME)
        {
            //Skill 1
            if (BolNumber[8])//L­ướt của skill 3
            {
                if (BolNumber[5])//Check hướng trái phải
                {

                    if (Player.transform.position.x >= Module.LIMITMAPMOVE[0] - Module.RANGEMOVELIMIT)
                        HeroControl.HeroPos.x -= FloatNumber[2] * Time.deltaTime * GameSystem.Settings.FPSLimit;
                }
                else
                {
                    if (Player.transform.position.x <= Module.LIMITMAPMOVE[1] + Module.RANGEMOVELIMIT)
                        HeroControl.HeroPos.x += FloatNumber[2] * Time.deltaTime * GameSystem.Settings.FPSLimit;
                }
            }
            ReturnComboAtk();//Điều khiển combo normal atk
            RespawnSkillControl();//Điều khiển time hồi chiêu
        }
    }

    /// <summary>
    /// Gọi hàm thực thi skill trong Animation
    /// </summary>
    /// <param name="SkillID"></param>
    public void SkillAction(int SkillID)
    {
        switch (SkillID)
        {
            case 1://Normal attack
                ShowSkill(EffAtk[0], new Vector3(Player.transform.position.x + 1.45f, Player.transform.position.y + 0.71f, Module.BASELAYER[2]), Quaternion.Euler(36f, 48f, -94.59f));
                break;
            case 2://Normal attack
                ShowSkill(EffAtk[1], new Vector3(Player.transform.position.x + 0.4f, Player.transform.position.y + 0.71f, Module.BASELAYER[2]), Quaternion.Euler(-63.16f, -34.7f, -14.05f));
                break;
            case 3://Normal attack
                ShowSkill(EffAtk[2], new Vector3(Player.transform.position.x + 0.4f, Player.transform.position.y - 0.43f, Module.BASELAYER[2]), Quaternion.Euler(73.35f, 0f, 0f));
                break;
            case 4://Skill 1, lướt liên tục trên không
                if (GameSystem.Settings.SoundEnable)
                {
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0.1f));//Play random sound
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0.2f));//Play random sound
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0.3f));//Play random sound
                }
                if (BolNumber[5])
                    ShowSkill(EffSkill1[1], new Vector3(Player.transform.position.x - 8f, Player.transform.position.y + 3.5f, Module.BASELAYER[2]), Quaternion.identity);
                else
                    ShowSkill(EffSkill1[0], new Vector3(Player.transform.position.x + 8f, Player.transform.position.y + 3.5f, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 5://Skill 1: hiệu ứng tiếp đất
                if (GameSystem.Settings.SoundEnable)
                    StartCoroutine(PlaySound(AudioSkill1[0], 0));//Play random sound
                ShowSkill(EffSkill1[2], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 6://Skill 2: hiệu ứng tiếp đất và hiệu ứng skill
                if (GameSystem.Settings.SoundEnable)
                    StartCoroutine(PlaySound(AudioSkill2[0], 0));//Play random sound
                ShowSkill(EffSkill2[0], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                ShowSkill(EffSkill2[1], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 7://Skill 3: hiệu ứng chém lướt
                //StartCoroutine(PlaySound(AudioSkill2[0], 0));//Play random sound
                if (!BolNumber[5])//Nhìn bên phải
                    ShowSkill(EffSkill3[0], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                else
                    ShowSkill(EffSkill3[1], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 8://Skill 3: hiệu ứng lốc
                //StartCoroutine(PlaySound(AudioSkill2[0], 0));//Play random sound
                ShowSkill(EffSkill3[2], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 9://Skill 4: Quét kiếm 3 lần
                   //StartCoroutine(PlaySound(AudioSkill2[0], 0));//Play random sound
                ShowSkill(EffSkill4[0], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                ShowSkill(EffSkill4[3], new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 10:
                ShowSkill(EffSkill4[1], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                ShowSkill(EffSkill4[4], new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 11:
                ShowSkill(EffSkill4[2], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), Quaternion.identity);
                ShowSkill(EffSkill4[5], new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Module.BASELAYER[2]), Quaternion.identity);
                break;
            case 12://Chém thường hất tung enemy khi bấm UP + Atk (Copy của atk1)
                ShowSkill(EffAtkUp, new Vector3(Player.transform.position.x + 1.45f, Player.transform.position.y + 0.71f, Module.BASELAYER[2]), Quaternion.Euler(36f, 48f, -94.59f));
                break;
            default: break;
        }
    }

    /// <summary>
    /// Normal Attack
    /// </summary>
    /// <returns></returns>
    public override void Attack(bool pressup, bool pressdown)
    {
        if (BolNumber[6] && !BolNumber[4])
        {
            for (int i = 0; i < EffAtk.Length; i++)//Set hướng nhìn cho normal atk
            {
                if (!EffAtk[i].activeSelf)
                {
                    EffAtk[i].GetComponent<H2_Normalatk>().herosee = Module.CURRENSEE;
                }
            }
            if (Prop[0].MP_curent >= Prop[0].MPSkill[0])//Nếu đủ thể lực để tung đòn đánh
            {
                if (!pressup && !pressdown)//Nếu không nhấn up hoặc down khi atk
                {
                    #region Đánh thường combo 3 đòn
                    if (!BolNumber[1])
                    {
                        BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                        Prop[0].MP_curent -= Prop[0].MPSkill[0];//Trừ thể lực
                        ControlGravity(false);
                        BolNumber[4] = true; //Start run anim
                        BolNumber[0] = true;
                        if (GameSystem.Settings.SoundEnable)
                            StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                        if (BolNumber[7])//Nếu đang nhảy lên
                            HeroControl.Anim.SetTrigger("Atk1Fly");
                        else
                            HeroControl.Anim.SetTrigger("Atk1");
                        BolNumber[1] = true;
                        FloatNumber[0] = FloatNumber[1];
                        return;
                    }
                    if (!BolNumber[2])
                    {
                        BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                        Prop[0].MP_curent -= Prop[0].MPSkill[0];//Trừ thể lực
                        ControlGravity(false);
                        BolNumber[4] = true; //Start run anim
                        BolNumber[0] = true;
                        if (GameSystem.Settings.SoundEnable)
                            StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                        if (BolNumber[7])//Nếu đang nhảy lên
                            HeroControl.Anim.SetTrigger("Atk2Fly");
                        else
                            HeroControl.Anim.SetTrigger("Atk2");
                        BolNumber[2] = true;
                        FloatNumber[0] = FloatNumber[1];
                        return;
                    }
                    if (!BolNumber[3])
                    {
                        BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                        Prop[0].MP_curent -= Prop[0].MPSkill[0];//Trừ thể lực
                        ControlGravity(false);
                        BolNumber[4] = true; //Start run anim
                        BolNumber[0] = true;
                        if (GameSystem.Settings.SoundEnable)
                            StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                        if (BolNumber[7])//Nếu đang nhảy lên
                            HeroControl.Anim.SetTrigger("Atk3Fly");
                        else
                            HeroControl.Anim.SetTrigger("Atk3");
                        BolNumber[3] = true;
                        FloatNumber[0] = FloatNumber[1];
                        return;
                    }
                    #endregion
                }
                else if (pressup && !BolNumber[0])//Nếu nhấn up khi atk
                {
                    EffAtkUp.GetComponent<H2_Normalatk>().herosee = Module.CURRENSEE;
                    BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                    Prop[0].MP_curent -= Prop[0].MPSkill[0];//Trừ thể lực
                    ControlGravity(false);
                    BolNumber[4] = true; //Start run anim
                    BolNumber[0] = true;
                    if (GameSystem.Settings.SoundEnable)
                        StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                    if (BolNumber[7])//Nếu đang nhảy lên
                        HeroControl.Anim.SetTrigger("AtkUpFly");
                    else
                        HeroControl.Anim.SetTrigger("AtkUp");
                    return;
                }
                //StartCoroutine(CountNormalAtk(1));
            }
        }

    }

    /// <summary>
    /// Skill khi tap
    /// </summary>
    /// <param name="SkillID"></param>
    /// <returns></returns>
    public override IEnumerator Skill(int SkillID)
    {
        switch (SkillID)
        {
            case 1:
                if (!BolNumber[0] && !BolSkill[1] && Prop[0].MP_curent >= Prop[0].MPSkill[SkillID])//Nếu ko còn skill nào đang hoạt động, và skill 1 đã hồi chiêu
                {
                    Prop[0].MP_curent -= Prop[0].MPSkill[SkillID];//Trừ thể lực
                    StartCoroutine(StarRespawn(1));//Khởi động hồi chiêu
                    BolNumber[6] = false;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill                
                    BolNumber[4] = true; //Start run anim
                    BolNumber[5] = HeroControl.HeroSee;
                    HeroControl.Anim.SetTrigger("Skill1");
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            case 2:
                if (!BolNumber[0] && !BolSkill[2] && Prop[0].MP_curent >= Prop[0].MPSkill[SkillID])//Nếu ko còn skill nào đang hoạt động, và skill 3 đã hồi chiêu
                {
                    Prop[0].MP_curent -= Prop[0].MPSkill[SkillID];//Trừ thể lực
                    StartCoroutine(StarRespawn(2));//Khởi động hồi chiêu
                    //BolNumber[6] = false;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill                
                    BolNumber[4] = true; //Start run anim
                    BolNumber[5] = HeroControl.HeroSee;
                    HeroControl.Anim.SetTrigger("Skill2");
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            case 3:
                if (!BolNumber[0] && !BolSkill[3] && !BolNumber[7] && Prop[0].MP_curent >= Prop[0].MPSkill[SkillID])//Nếu ko còn skill nào đang hoạt động, và skill 3 đã hồi chiêu, và đang ko nhảy lên
                {
                    Prop[0].MP_curent -= Prop[0].MPSkill[SkillID];//Trừ thể lực
                    BolNumber[8] = true;//Bắt đầu lướt skill 3
                    StartCoroutine(StarRespawn(3));//Khởi động hồi chiêu
                    BolNumber[6] = false;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill                
                    BolNumber[4] = true; //Start run anim
                    BolNumber[5] = HeroControl.HeroSee;
                    HeroControl.Anim.SetTrigger("Skill3");
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0.3f);
                    BolNumber[8] = false;//Ngừng lướt
                }
                break;
            case 4:
                if (!BolNumber[0] && !BolSkill[4] && Prop[0].MP_curent >= Prop[0].MPSkill[SkillID])//Nếu ko còn skill nào đang hoạt động, và skill 4 đã hồi chiêu
                {
                    ControlGravity(false);
                    Prop[0].MP_curent -= Prop[0].MPSkill[SkillID];//Trừ thể lực
                    StartCoroutine(StarRespawn(4));//Khởi động hồi chiêu
                    BolNumber[6] = false;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill                
                    BolNumber[4] = true; //Start run anim
                    BolNumber[5] = HeroControl.HeroSee;
                    HeroControl.Anim.SetTrigger("Skill4");
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            default: break;
        }
    }
}
