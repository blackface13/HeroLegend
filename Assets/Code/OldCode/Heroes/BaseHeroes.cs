using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Assets.Code._4.CORE;

public class BaseHeroes : OverridableMonoBehaviour
{
    #region Variables
    public DatabaseHeroes DBHeroes;
    public List<SystemProperties> Prop;
    public int IDHeroes;
    public float[] TimeRespawnSkill;
    public GameObject MainHeroes;//Object Character Control
    public CharacterControl HeroControl;
    public GameObject Player;//Object MainHero
    /// <summary>
    /// 0, 1: Đếm ngược time combo
    /// 2: Skill 1 - Tốc độ lướt
    /// </summary>
    public float[] FloatNumber = new float[10];

    /// <summary>
    /// 0: Đang attack hay ko
    /// 1, 2, 3: Normal attack
    /// 4: Cho phép anim khác chạy
    /// 5: Set hero see temp
    /// 6: Cho phép di chuyển trong khi atk hay ko
    /// 7: Đang jump hay ko
    /// 8: Đang lướt hay ko
    /// </summary>
    public bool[] BolNumber = new bool[10];

    /// <summary>
    /// 0: Time hoạt động skill 1
    /// 1-5: Kích hoạt hồi chiêu skill 1-5
    /// </summary>
    public bool[] BolSkill = new bool[10];
    public GameObject Ctrl; // Tạo biến control để điều khiển var của Battle System
    public System_Battle SystemBattle;
    public Rigidbody2D HeroRigidBody2D;
    public AudioSource Sounds;
    public AudioClip[] AudioNormalAtk;
    public AudioClip[] AudioHit;
    public AudioClip[] AudioSkill1;
    public AudioClip[] AudioSkill2;
    public AudioClip[] AudioSkill3;
    public AudioClip[] AudioMisc;
    public Guid GuidCode;
    #endregion

    #region Function

    protected override void Awake()
    {
        StartCoroutine(SetupBaseDatabase());//Giải mã database
        BolNumber[5] = true;//Set hướng mặc định khi start game
        MainHeroes = GameObject.FindGameObjectWithTag("MainHeroes");
        HeroControl = MainHeroes.GetComponent<CharacterControl>();
        BolNumber[0] = false;//Attacking = off
        BolNumber[6] = true;//Giữ chân = off

        Ctrl = GameObject.FindGameObjectWithTag("ControlScene");
        SystemBattle = Ctrl.GetComponent<System_Battle>();
    }

    /// <summary>
    /// Decrypt database
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupBaseDatabase()
    {
        DBHeroes = new DatabaseHeroes();
        DBHeroes = JsonUtility.FromJson<DatabaseHeroes>(Securitys.Decrypt(Module.DBHEROES).ToString());
        // DataUserController.Heroes = DBHeroes;
        // DataUserController.SaveHeroes();
        yield return new WaitForSeconds(0);
    }
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        HeroRigidBody2D = SystemBattle.HeroObject[0].GetComponent<Rigidbody2D>();
    }
    public override void UpdateMe()
    {
    }
    /// <summary>
    /// Bắt đầu thực hiện đánh hoặc skill
    /// </summary>
    public IEnumerator StartAtk(float time)
    {
        //BolNumber[0] = true;
        BolNumber[4] = true; //Start run anim
        Module.GUILDBASE = new Guid();
        GuidCode = Module.GUILDBASE;
        yield return new WaitForSeconds(time);
        if (GuidCode.Equals(Module.GUILDBASE))
            EndAtk();
    }

    /// <summary>
    /// Kết thúc đòn đánh hoặc skill
    /// </summary>
    public void EndAtk()
    {
        if (BolNumber[8])//Dành cho trường hợp lướt
        {
            if (!BolNumber[0])
            {
                if (!HeroControl.HeroComponent.BolNumber[7])//Nếu đang không nhảy
                {
                    //Đưa hero về trạng thái animation mặc định
                    if (HeroControl.MoveLeft || HeroControl.MoveRight)
                        HeroControl.Anim.SetTrigger("Move");
                    else HeroControl.Anim.SetTrigger("Idie");
                }
                //Reset combo
                if (BolNumber[3])
                {
                    for (int i = 0; i < 3; i++)
                        BolNumber[i + 1] = false;
                }
                BolNumber[0] = false;
            }
            if (Module.CURRENSEE)//Chỉnh lại hướng nhìn sau khi kết thúc animation
                HeroControl.Flip(2);//Right
            else
                HeroControl.Flip(1);//Left
            BolNumber[8] = false;
            BolNumber[6] = true;//Cho phép di chuyển
            BolNumber[4] = false;//Cho phép thực hiện anim khác (dùng cho normal atk)
            ControlGravity(true);
        }
        else if (BolNumber[0])//Kết thúc đòn đánh
        {
            if (!HeroControl.HeroComponent.BolNumber[7] && !HeroControl.HeroComponent.BolNumber[4])//Nếu đang không nhảy
            {
                //Đưa hero về trạng thái animation mặc định
                if (HeroControl.MoveLeft || HeroControl.MoveRight)
                    HeroControl.Anim.SetTrigger("Move");
                else HeroControl.Anim.SetTrigger("Idie");
            }
            //Reset combo
            if (BolNumber[3])
            {
                for (int i = 0; i < 3; i++)
                    BolNumber[i + 1] = false;
            }
            BolNumber[0] = false;
            if (BolNumber[8])
                BolNumber[8] = false;
            BolNumber[6] = true;//Cho phép di chuyển
            BolNumber[4] = false;//Cho phép thực hiện anim khác (dùng cho normal atk)
            ControlGravity(true);
            if (Module.CURRENSEE)//Chỉnh lại hướng nhìn sau khi kết thúc animation
                HeroControl.Flip(2);//Right
            else
                HeroControl.Flip(1);//Left
        }
        //CheckSee();
    }

    //private void CheckSee()
    //{
    //    if(BolNumber[5] != Module.CURRENSEE)
    //    {
    //        if (Module.CURRENSEE)
    //            MainHeroes.GetComponent<CharacterControl>().Flip(1);
    //        else
    //            MainHeroes.GetComponent<CharacterControl>().Flip(2);
    //        BolNumber[5] = Module.CURRENSEE;
    //    }
    //}
    /// <summary>
    /// Điều khiển combo normal atk
    /// </summary>
    public void ReturnComboAtk()
    {
        FloatNumber[1] += 1 * Time.deltaTime;
        if (BolNumber[1] || BolNumber[2] || BolNumber[3])
        {
            if (FloatNumber[1] - FloatNumber[0] >= Module.TIMEDELAYCOMBO)
                for (int i = 0; i < 3; i++)
                    BolNumber[i + 1] = false;
        }
    }

    /// <summary>
    /// Điều khiển time hồi chiêu của Hero
    /// </summary>
    public void RespawnSkillControl()
    {
        if (BolSkill[1])
        {
            if (HeroControl.ObjButtonSkill[1].transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
            {
                BolSkill[1] = false;
            }
            else
                HeroControl.ObjButtonSkill[1].transform.GetChild(0).GetComponent<Image>().fillAmount -= 1000f / TimeRespawnSkill[0] * Time.deltaTime;
        }
        if (BolSkill[2])
        {
            if (HeroControl.ObjButtonSkill[2].transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
            {
                BolSkill[2] = false;
            }
            else
                HeroControl.ObjButtonSkill[2].transform.GetChild(0).GetComponent<Image>().fillAmount -= 1000f / TimeRespawnSkill[1] * Time.deltaTime;
        }
        if (BolSkill[3])
        {
            if (HeroControl.ObjButtonSkill[3].transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
            {
                BolSkill[3] = false;
            }
            else
                HeroControl.ObjButtonSkill[3].transform.GetChild(0).GetComponent<Image>().fillAmount -= 1000f / TimeRespawnSkill[2] * Time.deltaTime;
        }
        if (BolSkill[4])
        {
            if (HeroControl.ObjButtonSkill[4].transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
            {
                BolSkill[4] = false;
            }
            else
                HeroControl.ObjButtonSkill[4].transform.GetChild(0).GetComponent<Image>().fillAmount -= 1000f / TimeRespawnSkill[3] * Time.deltaTime;
        }
    }

    /// <summary>
    /// Cài đặt thời gian hồi chiêu cho skill
    /// </summary>
    /// <param name="SkillID"></param>
    /// <returns></returns>
    public IEnumerator StarRespawn(int SkillID)
    {
        BolSkill[SkillID] = true;//Kích hoạt respawn skill
        HeroControl.ObjButtonSkill[SkillID].transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Normal Attack for all Hero
    /// </summary>
    public virtual void Attack(bool pressup, bool pressdown)
    { }

    /// <summary>
    /// Đưa về trạng thái cho phép di chuyển sau khi normal atk
    /// </summary>
    public virtual void EnableMove()
    {
        BolNumber[6] = true;//Cho phép di chuyển
        BolNumber[4] = false;//Cho phép thực hiện anim khác (dùng cho normal atk)
    }

    /// <summary>
    /// Skill 1
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator Skill(int SkillID)
    {
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Show object skill khi xuất chiêu
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="vec"></param>
    /// <param name="quater"></param>
    public virtual void ShowSkill(GameObject obj, Vector3 vec, Quaternion quater)
    {
        obj.transform.position = vec;
        obj.transform.rotation = quater;
        obj.SetActive(true);
    }
    /// <summary>
    /// Check collision
    /// </summary>
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[0]))//Chạm đất
        {
            if (BolNumber[7])//Nếu đang nhảy
            {
                if (!BolNumber[0])//Nếu đang ko dùng skill 
                {
                    if (HeroControl.MoveLeft || HeroControl.MoveRight)//Chuyển anim về trạng thái hiện tại
                        HeroControl.Anim.SetTrigger("Move");
                    else
                        HeroControl.Anim.SetTrigger("Idie");
                }
                BolNumber[7] = false;//Disable nhảy
            }
        }
    }

    /// <summary>
    /// Control gravity Hero (Điều khiển trọng lực, dành cho nhảy lên và lướt)
    /// </summary>
    /// <param name="state"></param>
    public void ControlGravity(bool state)
    {
        HeroRigidBody2D.gravityScale = !state ? 0 : Module.GRAVITYCHARACTER;
        if (!state)
        {
            HeroRigidBody2D.velocity = Vector3.zero;
            HeroRigidBody2D.angularVelocity = 0f;
        }
    }

    /// <summary>
    /// Play sound effect (Chạy âm thanh hiệu ứng skill)
    /// </summary>
    /// <param name="_clip"></param>
    /// <returns></returns>
    public virtual IEnumerator PlaySound(AudioClip _clip, float _time)
    {
        yield return new WaitForSeconds(_time);
        Sounds.PlayOneShot(_clip);
    }
    #endregion
}
