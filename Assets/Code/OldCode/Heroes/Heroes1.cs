using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Heroes1 : BaseHeroes
{
    #region Variables
    private GameObject[] SkillObject01 = new GameObject[4];//Object skill 1 - Số lượng object phải là chẵn (Nếu điều chỉnh số lượng này, thì sửa ở hàm SkillAction())
    private GameObject[] SkillObject02 = new GameObject[6];//Object skill 2 - Số lượng object phải là chẵn - Skill nhảy lên tung phi tiêu 2 bên
    private Heroes1_skill2[] SkillObject02Component = new Heroes1_skill2[6];//Object skill 2 - Số lượng object phải là chẵn - Skill nhảy lên tung phi tiêu 2 bên
    private GameObject[] SkillObject03 = new GameObject[5];//Object skill 3 - Skill đặt bẫy
    private GameObject[] SkillObject04 = new GameObject[3];//Object skill 4 - Tung ra 3 phi tiêu về hướng nhìn và giật về
    private GameObject[] EffectSkillObject02 = new GameObject[2];//Hiệu ứng khi tung đòn, ko phải hiệu ứng của skill
    #endregion

    /// <summary>
    /// Khởi tạo ban đầu
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(GetValuesBase());//Setup values for this Enemy
        StartCoroutine(CreateAudio());//Setup audio (khởi tạo âm thanh cho các skill)
        //Skill 1============Khởi tạo Object phi tiêu, 5 phi tiêu nhỏ và 5 phi tiêu lớn============
        for (int i = 0; i < SkillObject01.Length; i++)
            SkillObject01[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk1"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        //Điều chỉnh kích thước của 1 nửa phi tiêu lớn phía sau
        for (int i = SkillObject01.Length / 2; i < SkillObject01.Length; i++)
            SkillObject01[i].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        //Ẩn các object này ngay sau khi được khởi tạo và gán giá trị
        for (int i = 0; i < SkillObject01.Length; i++)
            SkillObject01[i].SetActive(false);
        //======================================================================================

        //Skill 2============Khởi tạo Object phi tiêu============
        for (int i = 0; i < SkillObject02.Length; i++)
            SkillObject02[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk2"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        //Ẩn các object này ngay sau khi được khởi tạo và gán giá trị
        for (int i = 0; i < SkillObject02.Length; i++)
        {
            SkillObject02Component[i] = SkillObject02[i].GetComponent<Heroes1_skill2>();
            SkillObject02[i].SetActive(false);
        }
        //======================================================================================

        //Skill 3============Khởi tạo Object bẫy============
        for (int i = 0; i < SkillObject03.Length; i++)
            SkillObject03[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk3"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        //Ẩn các object này ngay sau khi được khởi tạo và gán giá trị
        for (int i = 0; i < SkillObject03.Length; i++)
        {
            SkillObject03[i].SetActive(false);
        }
        //======================================================================================

        //Skill 4============Khởi tạo Object============
        for (int i = 0; i < SkillObject04.Length; i++)
            SkillObject04[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk4"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        //Ẩn các object này ngay sau khi được khởi tạo và gán giá trị
        for (int i = 0; i < SkillObject04.Length; i++)
        {
            SkillObject04[i].GetComponent<Heroes1_skill4>().ObjectType = i + 1;//Set kiểu object type để định hướng skill bay chéo lên hay ngang hay chéo xuống
            SkillObject04[i].SetActive(false);
        }
        //======================================================================================
        FloatNumber[2] = Module.BASESKILL1SPEEDDEFAULT;//Skill 1 - Tốc độ lướt
        TimeRespawnSkill = new float[5];//Khởi tạo list time hồi chiêu
        TimeRespawnSkill[0] = 500;//Skill lướt
        TimeRespawnSkill[1] = 2000;//Skill nhảy lên bắn phi tiêu
        TimeRespawnSkill[2] = 1000;//Skill đặt bẫy
        TimeRespawnSkill[3] = 1000;//Skill phóng và rút phi tiêu về
        TimeRespawnSkill[4] = 500;
        //Khởi tạo effect cho skill
        EffectSkillObject02[0] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk2_Eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectSkillObject02[1] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk2_Eff2"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(80f, 0f, 0f));
        EffectSkillObject02[0].SetActive(false);
        EffectSkillObject02[1].SetActive(false);
    }

    /// <summary>
    /// Setup values for this Enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetValuesBase()
    {
        // Prop = new List<SystemProperties>();
        // for (int i = 0; i < DBHeroes.DBHeroes.Count; i++)
        //     foreach (SystemProperties basehero in DBHeroes.DBHeroes)
        //     {
        //         if (basehero.ID.Equals(IDHeroes))
        //         {
        //             Prop.Add(basehero);
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

    #region Create Object Autio
    private IEnumerator CreateAudio()
    {
        Sounds = GetComponent<AudioSource>();
        AudioNormalAtk = new AudioClip[3];
        AudioHit = new AudioClip[4];
        AudioSkill1 = new AudioClip[1];//Skill 1
        AudioSkill2 = new AudioClip[1];//Skill 1
        AudioSkill3 = new AudioClip[1];//Skill 1
        for (int i = 0; i < AudioNormalAtk.Length; i++)//Normal atk
            AudioNormalAtk[i] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H1Nor" + (i + 1).ToString()));
        for (int i = 0; i < AudioHit.Length; i++)//Hit
            AudioHit[i] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H1Hit" + (i + 1).ToString()));
        AudioSkill1[0] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H1Skill1"));
        AudioSkill2[0] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H1Skill2"));
        AudioSkill3[0] = Instantiate(Resources.Load<AudioClip>("Sounds/Effects/H1Skill3"));
        yield return new WaitForSeconds(0);
    }
    #endregion
    /// <summary>
    /// Update for this Scene
    /// </summary>
    void Update()
    {
        if (!Module.PAUSEGAME)
        {
            //Skill 1
            if (BolSkill[1])//Nếu đang true, thì lướt (thay đổi vị trí)
            {
                if (BolNumber[8])//Nếu đang lướt
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
            }
            ReturnComboAtk();//Điều khiển combo normal atk
            RespawnSkillControl();//Điều khiển time hồi chiêu
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
            for (int i = 0; i < SkillObject01.Length; i++)
            {
                if (!SkillObject01[i].activeSelf)
                {
                    SkillObject01[i].GetComponent<Heroes1_normalatk>().herosee = Module.CURRENSEE;
                }
            }
            if (!BolNumber[1])
            {
            BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                BolNumber[4] = true; //Start run anim
                BolNumber[0] = true;
                if (GameSystem.Settings.SoundEnable)
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                HeroControl.Anim.SetTrigger("Atk1");
                BolNumber[1] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            if (!BolNumber[2])
            {
            BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                BolNumber[4] = true; //Start run anim
                BolNumber[0] = true;
                if (GameSystem.Settings.SoundEnable)
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0));//Play random sound
                HeroControl.Anim.SetTrigger("Atk2");
                BolNumber[2] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            if (!BolNumber[3])
            {
            BolNumber[6] = false;//Không cho di chuyển khi đang xài chiêu
                BolNumber[4] = true; //Start run anim
                BolNumber[0] = true;
                if (GameSystem.Settings.SoundEnable)
                    StartCoroutine(PlaySound(AudioNormalAtk[Random.Range(0, AudioNormalAtk.Length)], 0.2f));//Play random sound
                HeroControl.Anim.SetTrigger("Atk3");
                BolNumber[3] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            //StartCoroutine(CountNormalAtk(1));
        }
    }

    /// <summary>
    /// Skill lướt Hero 1
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Skill(int SkillID)
    {
        switch (SkillID)
        {
            case 1:
                if (!BolNumber[0] && !BolSkill[1])//Nếu ko còn skill nào đang hoạt động, và skill 1 đã hồi chiêu
                {
                    StartCoroutine(StarRespawn(1));//Khởi động hồi chiêu
                    BolNumber[5] = HeroControl.HeroSee;
                    HeroControl.Anim.SetTrigger("Skill1");
                    StartCoroutine(SkillStart(0.33f, false, 1));//Thực hiện lướt
                    //BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            case 2:
                if (!BolNumber[0] && !BolSkill[2])//Nếu ko còn skill nào đang hoạt động, và skill 3 đã hồi chiêu
                {
                    StartCoroutine(StarRespawn(2));//Khởi động hồi chiêu
                    BolNumber[5] = HeroControl.HeroSee;
                    if (GameSystem.Settings.SoundEnable)
                        StartCoroutine(PlaySound(AudioSkill1[0], 0));//Play sound
                    HeroControl.Anim.SetTrigger("Skill2");
                    StartCoroutine(SkillStart(0.83f, true, 2));//Thực hiện skill
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            case 3:
                if (!BolNumber[0] && !BolSkill[3] && !BolNumber[7])//Nếu ko còn skill nào đang hoạt động, và skill 3 đã hồi chiêu, và đang ko nhảy lên
                {
                    StartCoroutine(StarRespawn(3));//Khởi động hồi chiêu
                    BolNumber[5] = HeroControl.HeroSee;
                    if (GameSystem.Settings.SoundEnable)
                        StartCoroutine(PlaySound(AudioSkill2[0], 0));//Play sound
                    HeroControl.Anim.SetTrigger("Skill3");
                    StartCoroutine(SkillStart(0.5f, false, 3));//Thực hiện skill, ko cho di chuyển
                    BolNumber[0] = true;//Start Atk
                    yield return new WaitForSeconds(0);
                }
                break;
            case 4:
                if (!BolNumber[0] && !BolSkill[4])//Nếu ko còn skill nào đang hoạt động, và skill 4 đã hồi chiêu
                {
                    StartCoroutine(StarRespawn(4));//Khởi động hồi chiêu
                    BolNumber[5] = HeroControl.HeroSee;
                    if (GameSystem.Settings.SoundEnable)
                        StartCoroutine(PlaySound(AudioSkill3[0], 0));//Play sound
                    HeroControl.Anim.SetTrigger("Skill4");
                    StartCoroutine(SkillStart(0.5f, true, 4));//Thực hiện skill, cho di chuyển
                    BolNumber[0] = true;//Start Atk
                    for (int i = 0; i < SkillObject04.Length; i++)
                    {
                        SkillObject04[i].GetComponent<Heroes1_skill4>().herosee = Module.CURRENSEE;
                    }
                    yield return new WaitForSeconds(0);
                }
                break;
            default: break;
        }
    }

    /// <summary>
    /// Quản lý time giữ chân, time sử dụng skill
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator SkillStart(float time, bool limitmove, int skillID)
    {
        switch (skillID)
        {
            case 1:
                ControlGravity(false);
                BolNumber[8] = true;//Bắt đầu lướt
                BolNumber[6] = limitmove;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill
                yield return new WaitForSeconds(time);
                BolNumber[6] = true;
                //BolNumber[8] = false;//Dừng lướt    
                if (BolNumber[7])//Nếu đang nhảy
                {
                    if (!BolNumber[0])
                        HeroControl.Anim.SetTrigger("Jump");
                }
                ControlGravity(true);
                break;
            case 2:
                BolNumber[6] = limitmove;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill
                yield return new WaitForSeconds(time);
                BolNumber[6] = true;
                break;
            case 3:
                BolNumber[6] = limitmove;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill
                yield return new WaitForSeconds(time);
                BolNumber[6] = true;
                break;
            case 4:
                BolNumber[6] = limitmove;//Cho phép di chuyển khi đang dùng skill hay ko, tùy vào từng skill
                yield return new WaitForSeconds(time);
                BolNumber[6] = true;
                break;
            default:
                yield return new WaitForSeconds(0);
                break;
        }
    }

    /// <summary>
    /// Bật hiệu ứng khi sử dụng
    /// </summary>
    /// <param name="SkillID"></param>
    public void EnableEffect(int SkillID)
    {
        //Bật hiệu ứng khí bay khi sử dụng skill
        switch (SkillID)
        {
            case 2:
                EffectSkillObject02[0].transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3f, Module.BASELAYER[2]);
                EffectSkillObject02[1].transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5f, Module.BASELAYER[2]);
                EffectSkillObject02[0].SetActive(true);
                EffectSkillObject02[1].SetActive(true);
                SystemBattle.Shake(0.05f);
                break;
            default: break;
        }
    }

    /// <summary>
    /// Gọi hàm thực thi skill trong Animation
    /// </summary>
    /// <param name="SkillID"></param>
    public void SkillAction(int SkillID)
    {
        //if (Prop[0].MP_curent >= Prop[0].MPSkill[SkillID])
        //    Prop[0].MP_curent -= Prop[0].MPSkill[SkillID];
        switch (SkillID)
        {
            case 1://Normal attack
                if (SkillObject01[0].activeSelf)
                    ShowSkill(SkillObject01[1], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                else
                    ShowSkill(SkillObject01[0], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                break;
            case 2://Normal attack
                if (SkillObject01[2].activeSelf)
                    ShowSkill(SkillObject01[3], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                else
                    ShowSkill(SkillObject01[2], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                break;
            case 3://Normal attack
                if (SkillObject01[0].activeSelf)
                    ShowSkill(SkillObject01[1], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                else
                    ShowSkill(SkillObject01[0], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                if (SkillObject01[2].activeSelf)
                    ShowSkill(SkillObject01[3], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                else
                    ShowSkill(SkillObject01[2], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                break;
            case 4://Skill nhảy lên tung phi tiêu
                for (int i = 0; i < SkillObject02.Length; i++)//Duyệt all object của skill2
                {
                    if (!SkillObject02[i].activeSelf)//Nếu object nào đang hide
                    {
                        if (i % 2 != 0)//Nếu thứ tự object đó là chẵn (Mục đích để show object trái hoặc phải Hero)
                        {
                            SkillObject02Component[i].herosee = false;
                            ShowSkill(SkillObject02[i], new Vector3(Player.transform.position.x - 1f, Player.transform.position.y + 4f, Module.BASELAYER[2]), new Quaternion());
                        }
                        else
                        {
                            SkillObject02Component[i].herosee = true;
                            ShowSkill(SkillObject02[i], new Vector3(Player.transform.position.x + 1f, Player.transform.position.y + 4f, Module.BASELAYER[2]), new Quaternion());
                        }
                        break;
                    }
                }
                break;
            case 5://Skill đặt bẫy
                for (int i = 0; i < SkillObject03.Length; i++)//Duyệt all object của skill3
                {
                    if (!SkillObject03[i].activeSelf)//Nếu object nào đang hide
                    {
                        ShowSkill(SkillObject03[i], new Vector3(Player.transform.position.x, Player.transform.position.y - 1.5f, Module.BASELAYER[2]), new Quaternion());
                        break;
                    }
                }
                break;
            case 6://Skill phóng và kéo về 3 phi tiêu
                for (int i = 0; i < SkillObject04.Length; i++)//Duyệt all object của skill4
                {
                    if (!SkillObject04[i].activeSelf)//Nếu object nào đang hide
                        ShowSkill(SkillObject04[i], new Vector3(Player.transform.position.x, Player.transform.position.y, Module.BASELAYER[2]), new Quaternion());
                }
                break;
            default: break;
        }
    }


}
