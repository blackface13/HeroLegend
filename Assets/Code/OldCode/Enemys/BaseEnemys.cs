using Assets.Code._4.CORE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemys : MonoBehaviour
{
    #region Variables
    public DatabaseEnemy DBEnemy;
    public List<SystemProperties> Prop;
    public Vector2 Rectangle;
    public GameObject Player;
    public int EnemyID;//ID của enemy
    public GameObject[] Gobject;//Gameobject để control - Setup trong Hàm Awake() của script kế thừa script này
    public GameObject MainHeroes;
    public Animator Anim;//Animator controller của Enemy
    public Vector3 ThisVec;//Vector tạm dành cho việc điều khiển enemy
    public GameObject CheckRangeLeft, CheckRangeRight;
    public GameObject Ctrl; // Tạo biến control để điều khiển var của Battle System
    public System_Battle SystemBattle;
    //Detect collision
    public bool CollisionEnter, CollisionTrigger, CollisionExit;
    public GameObject ObjectCollision;
    public BaseHeroes BaseHero;
    public int DamePer = 100;
    public Guid GuidForThrow;//Guid cho event đang ở trên ko hay ko
    //=========Bar state===========
    public GameObject BarState;
    private Transform HPBar;
    private Transform MPBar;
    //=============================
    public AnimatorStateInfo AnimState;//Dùng để check xem anim nào đang hoạt động
    //----------------
    /// <summary>
    /// 0: Check đổi anim cho enemy.
    /// 1: Đang trong time respawn normal attack
    /// 2: On/Off gây sát thương của đòn đánh
    /// 3: Đang trong tầm có thể gây sát thương hay ko
    /// 4: Đang trong tầm phát hiện hay ko
    /// </summary>
    public bool[] BaseFloatNumber = new bool[5];
    /// <summary>
    /// 0: Chỉ số thông minh
    /// 1: Tầm xa phát hiện ra Hero
    /// 2: Khoảng cách đứng đánh
    /// 3: Tốc độ thể hiện anim khi chạy
    /// 4: Tốc độ chạy
    /// 5: Khoảng cách bị đẩy lùi (Càng lớn thì càng bị đẩy xa)
    /// 6: Thời gian delay normal attack
    /// 7: Thời gian bị hiệu ứng khống chế
    /// </summary>
    public float[] BaseValues = new float[8];
    public enum Status { Idie, Move, Attack, Hited, Die, Keep, Throw, Def };
    public Status EneStatus;
    public Rigidbody2D ThisRigidBody2D;
    public Vector3 VecTemp;
    #endregion

    #region Functions
    public virtual void Awake()
    {
        //base.Awake();
        StartCoroutine(SetupBaseDatabase());//Giải mã database
        MainHeroes = GameObject.FindGameObjectWithTag("MainHeroes");
        Anim = gameObject.GetComponent<Animator>();
        EneStatus = Status.Idie;
        BaseFloatNumber[1] = false;//Kết thúc delay normal atk
        Ctrl = GameObject.FindGameObjectWithTag("ControlScene");
        SystemBattle = Ctrl.GetComponent<System_Battle>();
        SetupBarState();
    }
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        BaseHero = Player.GetComponent<BaseHeroes>();
        ThisRigidBody2D = GetComponent<Rigidbody2D>();
        AnimState = Anim.GetCurrentAnimatorStateInfo(0);
    }

    /// <summary>
    /// Thiết lập HP bar và MP bar
    /// </summary>
    private void SetupBarState()
    {
        BarState = Instantiate(Resources.Load<GameObject>("Prefabs/StateBarEnemy"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f));
        HPBar = BarState.transform.GetChild(1).GetComponent<Transform>();
        MPBar = BarState.transform.GetChild(3).GetComponent<Transform>();
    }

    /// <summary>
    /// Decrypt database
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupBaseDatabase()
    {
        DBEnemy = new DatabaseEnemy();
        DBEnemy = JsonUtility.FromJson<DatabaseEnemy>(Securitys.Decrypt(Module.DBENEMY).ToString());
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Start delay normal atk
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator StartDelayNormalAtk()
    {
        BaseFloatNumber[1] = true;//Đang trong time respawn normal attack
        yield return new WaitForSeconds(BaseValues[6]);
        BaseFloatNumber[1] = false;//Kết thúc respawn atk, cho phép normal atk
    }

    /// <summary>
    /// Bật variable có thể gây sát thương
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public virtual IEnumerator EnableHit(float time)
    {
        BaseFloatNumber[2] = true;//Bật gây sát thương
        yield return new WaitForSeconds(time);
        BaseFloatNumber[2] = false;//Tắt gây sát thương
    }

    /// <summary>
    /// Bắt đầu thực hiện đòn đánh và tự động kết thúc đòn đánh với time được set
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public virtual IEnumerator StartAtk(float time)
    {
        EneStatus = Status.Attack;
        yield return new WaitForSeconds(time);
        EneStatus = Status.Idie;
        BaseFloatNumber[0] = true;//Cho phép thay đổi anim
        ChangeAnim("Idie", 1f);
    }

    public virtual void Update()
    {
        //print(EneStatus);
        if (!EneStatus.Equals(Status.Die))
        {
            EnemyMove();
        }
        if(!Prop[0].Live)
            UpdateBarState();
    }

    /// <summary>
    /// Update liên tục vị trí và trạng thái của các thanh HP, MP
    /// </summary>
    private void UpdateBarState()
    {
        BarState.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2.5f, this.transform.position.z);
        HPBar.localScale = new Vector3(((Prop[0].HP_curent / Prop[0].HP_total) * HPBar.localScale.y), HPBar.localScale.y, HPBar.localScale.z);
        MPBar.localScale = new Vector3(((Prop[0].MP_curent / Prop[0].MP_total) * MPBar.localScale.y), MPBar.localScale.y, MPBar.localScale.z);
        Prop[0].MP_curent += Prop[0].MP_curent >= Prop[0].MP_total || EneStatus.Equals(Status.Attack) ? 0 : Module.ReMP * Time.deltaTime;
        if (Prop[0].HP_curent <= 0)
        {
            EneStatus = Status.Die;
            BaseFloatNumber[0] = true;//Cho phép thay đổi anim
            ChangeAnim("Die", BaseValues[3]);
        }
    }

    /// <summary>
    /// Bắt đầu hiệu ứng khống chế
    /// </summary>
    /// <param name="statusinput"></param>
    /// <returns></returns>
    public IEnumerator StarEffect(Status statusinput)
    {
        if (!EneStatus.Equals(statusinput))//Không cộng dồn hiệu ứng 
        {
            EneStatus = statusinput;
            switch (statusinput)
            {
                case Status.Keep:
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Idie", 1f);
                    break;
                default: break;
            }
            yield return new WaitForSeconds(BaseValues[7]);//Thời gian hiệu ứng khống chế
            if (EneStatus.Equals(Status.Keep))//Nếu đang bị giữ giân
                EneStatus = Status.Idie;
        }
    }

    /// <summary>
    /// Check ở cuối anim của di chuyển (fix lỗi bị đánh chạy thụt lùi)
    /// </summary>
    public void CheckEndHit()
    {
        if(EneStatus.Equals(Status.Hited))
        {
            BaseFloatNumber[0] = true;//Cho phép thay đổi anim
            ChangeAnim("Idie", 1f);
            EneStatus = Status.Idie;
        }

    }
    /// <summary>
    /// Auto move của enemy - Tùy chỉnh lại theo độ thông minh, phát hiện xa gần của enemy
    /// </summary>
    public void EnemyMove()
    {
           // print(AnimState.IsName("Idie"));
        if (!EneStatus.Equals(Status.Die))
        {
            gameObject.transform.position = new Vector3(ThisVec.x, gameObject.transform.position.y, ThisVec.z);
            if (EneStatus.Equals(Status.Hited))//Bị đẩy lùi nếu trúng skill
            {
                if (Player.transform.position.x < gameObject.transform.position.x)
                    ThisVec.x += BaseValues[5] * Time.deltaTime * GameSystem.Settings.FPSLimit;//Bị đẩy lùi
                else
                    ThisVec.x -= BaseValues[5] * Time.deltaTime * GameSystem.Settings.FPSLimit;//Bị đẩy lùi
                //if (AnimState.IsName("Move"))
                //    EneStatus = Status.Idie;
            }
            //=========Flip nhân vật========
            if (Player.transform.position.x < gameObject.transform.position.x && gameObject.transform.position.x - Player.transform.position.x <= BaseValues[1])
            {
                if (!EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Throw))//Nếu đang không bị hiệu ứng đánh và đang đánh
                    Flip(2);//Lật nhân vật
            }
            if (Player.transform.position.x > gameObject.transform.position.x && Player.transform.position.x - gameObject.transform.position.x <= BaseValues[1])
            {
                if (!EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Throw))//Nếu đang không bị hiệu ứng đánh và đang đánh
                    Flip(1);//Lật nhân vật
            }
            //==============================
            //Trong tầm nhìn, và Hero đứng bên trái Enemy
            if (Player.transform.position.x < gameObject.transform.position.x && gameObject.transform.position.x - Player.transform.position.x <= BaseValues[1] && gameObject.transform.position.x - Player.transform.position.x >= BaseValues[2])
            {
                BaseFloatNumber[4] = true;//Trong tầm phát hiện
                BaseFloatNumber[3] = false;//Đang ngoài tầm gây sát thương
                if (!EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Keep) && !EneStatus.Equals(Status.Throw))//Nếu đang không bị hiệu ứng đánh và đang ko atk, ko giữ chân, ko hất tung
                    ThisVec.x -= BaseValues[4] * Time.deltaTime * GameSystem.Settings.FPSLimit;//Chạy về bên trái
                if (EneStatus.Equals(Status.Idie) && !EneStatus.Equals(Status.Keep))//Nếu đang đứng yên
                {
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Move", BaseValues[3]);
                    EneStatus = Status.Move;
                }
            }
            //Trong tầm nhìn, và Hero đứng bên phải Enemy
            else if (Player.transform.position.x > gameObject.transform.position.x && Player.transform.position.x - gameObject.transform.position.x <= BaseValues[1] && Player.transform.position.x - gameObject.transform.position.x >= BaseValues[2])
            {
                BaseFloatNumber[4] = true;//Trong tầm phát hiện
                BaseFloatNumber[3] = false;//Đang ngoài tầm gây sát thương
                if (!EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Keep) && !EneStatus.Equals(Status.Throw))//Nếu đang không bị hiệu ứng đánh và đang ko atk, ko giữ chân
                    ThisVec.x += BaseValues[4] * Time.deltaTime * GameSystem.Settings.FPSLimit;//Chạy về bên phải
                if (EneStatus.Equals(Status.Idie) && !EneStatus.Equals(Status.Keep))//Nếu đang đứng yên và ko bị trói chân
                {
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Move", BaseValues[3]);
                    EneStatus = Status.Move;
                }
            }
            //Ngoài tầm di chuyển, trong tầm gây sát thương
            else if ((Player.transform.position.x > gameObject.transform.position.x && Player.transform.position.x - gameObject.transform.position.x <= BaseValues[2]) || (Player.transform.position.x < gameObject.transform.position.x && gameObject.transform.position.x - Player.transform.position.x <= BaseValues[2]))
            {
                BaseFloatNumber[4] = false;//Ngoài tầm phát hiện
                BaseFloatNumber[3] = true;//Đang trong tầm gây sát thương
                if (!BaseFloatNumber[1])//Nếu kết thúc trạng thái chờ normal atk
                {
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    Anim.SetTrigger("Atk1");
                    StartCoroutine(StartDelayNormalAtk());
                }
                if (!EneStatus.Equals(Status.Idie) && !EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Throw))
                {
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Idie", 1f);
                    EneStatus = Status.Idie;
                }
            }
            else
            {
                BaseFloatNumber[4] = false;//Ngoài tầm phát hiện
                BaseFloatNumber[3] = false;//Đang ngoài tầm gây sát thương
                if (!EneStatus.Equals(Status.Idie) && !EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack) && !EneStatus.Equals(Status.Throw))
                {
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Idie", 1f);
                    EneStatus = Status.Idie;
                }
            }
        }
    }

    public void Flip(int x)
    {
        Vector3 theScale = gameObject.transform.localScale;
        switch (x)
        {
            case 1:
                if (theScale.x > 0)
                    theScale.x *= -1;
                break;
            case 2:
                if (theScale.x < 0)
                    theScale.x *= -1;
                break;
            default: break;
        }
        gameObject.transform.localScale = theScale;
    }

    public void ChangeAnim(string AnimName, float speed)
    {
        if (BaseFloatNumber[0])
        {
            Anim.SetTrigger(AnimName);
            Anim.speed = speed;
            BaseFloatNumber[0] = false;
        }
    }

    public virtual IEnumerator Hited(float time)//Đang tạm ko dùng
    {
        yield return new WaitForSeconds(time);
        if (!EneStatus.Equals(Status.Throw))//Nếu trạng thái đang ko bị hất tung
        {
            EneStatus = Status.Move;
            BaseFloatNumber[0] = true;//Cho phép thay đổi anim
            ChangeAnim("Move", 1f);
        }
    }

    /// <summary>
    /// Hàm này dc gọi khi hiệu ứng hited kết thúc
    /// </summary>
    public void EndHit()
    {
        if (!EneStatus.Equals(Status.Throw))//Nếu trạng thái đang ko bị hất tung
        {
            EneStatus = Status.Idie;
            BaseFloatNumber[0] = true;//Cho phép thay đổi anim
            ChangeAnim("Idie", 1f);
        }
        //BaseFloatNumber[0] = true;//Cho phép thay đổi anim
        //ChangeAnim("Idie", 1f);
        //print(EneStatus);
    }

    /// <summary>
    /// Hàm này được gọi khi hiệu ứng Die kết thúc
    /// </summary>
    public void Die()
    {
        Prop[0].Live = false;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, gameObject.transform.position.z);
        BarState.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, gameObject.transform.position.z);
        BarState.SetActive(false);
    }

    /// <summary>
    /// Xử lý va chạm với skill của hero
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        //Va chạm với skill của hero
        if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[3]) && !EneStatus.Equals(Status.Die))//BASELAYERRIGID2D xem trong Module
        {
            if (EneStatus.Equals(Status.Throw))//Nếu trạng thái đang bị hất tung
            {
                GuidForThrow = Guid.NewGuid();
                StartCoroutine(KeepInAir());//Giữ trên không trong 1 khoảng time khi bị trúng dame nếu đang bị hất tung
                //EneStatus = Status.Hited;
                //ChangeAnim("Hit", 1f);
                //StartCoroutine(Hited(Random.Range(0.25f, 0.35f)));
            }
            if (col.gameObject.tag.Equals("Repel"))//Nếu là skill đẩy lùi
            {
                if (!EneStatus.Equals(Status.Attack))//!EneStatus.Equals(Status.Hited) &&
                {
                    if (!EneStatus.Equals(Status.Throw))//Nếu trạng thái đang ko bị hất tung
                        EneStatus = Status.Hited;
                    else
                    {
                        GuidForThrow = Guid.NewGuid();
                        StartCoroutine(KeepInAir());//Giữ trên không trong 1 khoảng time khi bị trúng dame nếu đang bị hất tung
                    }

                    //    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    ChangeAnim("Hit", 1f);
                    //StartCoroutine(Hited(Random.Range(0.25f, 0.35f)));
                }
            }
            if (col.gameObject.tag.Equals("Keep"))//Nếu là skill giữ chân
            {
                if (!EneStatus.Equals(Status.Hited) && !EneStatus.Equals(Status.Attack))
                {
                    StartCoroutine(StarEffect(Status.Keep));
                }
            }
            if (col.gameObject.tag.Equals("Throw"))//Nếu là skill hất tung
            {
                if (!EneStatus.Equals(Status.Attack))
                {
                    if (EneStatus.Equals(Status.Throw))//Nếu trạng thái đang bị hất tung
                    {
                        //StartCoroutine(KeepInAir(0.1f));//Giữ trên không trong 1 khoảng time khi bị trúng dame nếu đang bị hất tung
                        EneStatus = Status.Throw;
                        BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                        Anim.SetTrigger("Hit");
                    }
                    else
                    {
                        //print(EneStatus);
                        BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                        Anim.SetTrigger("Hit");
                        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50), ForceMode2D.Impulse);
                        EneStatus = Status.Throw;
                        BaseFloatNumber[0] = false;//Cho phép thay đổi anim
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check collision khi bị hất tung và chạm đất
    /// </summary>
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[0]))//Chạm đất
        {
            if (EneStatus.Equals(Status.Throw))//Nếu trạng thái đang bị hất tung
            {
                if (BaseFloatNumber[4])//Trong tầm phát hiện
                {
                    EneStatus = Status.Move;
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    Anim.SetTrigger("Move");
                }
                else
                {
                    EneStatus = Status.Idie;
                    BaseFloatNumber[0] = true;//Cho phép thay đổi anim
                    Anim.SetTrigger("Idie");
                }
                BaseFloatNumber[0] = false;
            }
        }
    }

    /// <summary>
    /// Control gravity enemy (Điều khiển trọng lực, dành cho bị đánh trên không)
    /// </summary>
    /// <param name="state"></param>
    public void ControlGravity(bool state)
    {
        ThisRigidBody2D.gravityScale = !state ? 0 : Module.GRAVITYCHARACTER;
        if (!state)
        {
            ThisRigidBody2D.velocity = Vector3.zero;
            ThisRigidBody2D.angularVelocity = 0f;
        }
    }

    public IEnumerator KeepInAir()
    {
        var guidtemp = GuidForThrow;
        ControlGravity(false);
        yield return new WaitForSeconds(0.3f);
        if (!GuidForThrow.Equals(guidtemp))
            goto End;
        //print(GuidForThrow + "\n" + guidtemp);
        ControlGravity(true);
    //this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
    End:
        yield return new WaitForSeconds(0);

        //Vector3 _vec = this.transform.position;
        //_vec.y = this.transform.position.y + 1f;
        //this.transform.position = _vec;
        //yield return new WaitForSeconds(0.15f);
        //ControlGravity(true);
    }
    #endregion
}

