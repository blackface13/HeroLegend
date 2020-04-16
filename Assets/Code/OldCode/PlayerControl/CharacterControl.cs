using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
public class CharacterControl : OverridableMonoBehaviour
{

    #region Variable
    /// <summary>
    /// Game Object array for this scene
    /// 0: Main heroes - Object của Heroes mà người chơi điều khiển
    /// 1: Object dùng để thay đổi parent main camera
    /// 2: Skill control - Object điều khiển skill của Heroes
    /// </summary>
    public GameObject[] GObjectNumber = new GameObject[10];

    /// <summary>
    /// Các button skill - Set đối tượng ngoài interface, ko gán đối tượng trong code
    /// 0: Attack button
    /// 1-9: tương ứng với button skill từ 1 - 9
    /// </summary>
    public GameObject[] ObjButtonSkill;

    public Animator Anim;
    public BaseHeroes HeroComponent;
    GameObject Ctrl; // Tạo biến control để điều khiển var của Battle System
    public Vector3 HeroPos; // Biến tạm vị trí hero
    public Vector3 CamTemp; // Biến tạm camera pos
    public System_Battle SystemBattle;
    public bool MoveLeft = false, MoveRight = false, Fliped = false, HeroSee = false, LockMove = false, PressUp = false, PressDown = false;//Biến xác định hero di chuyển trái phải, flip
    /// <summary>
    /// Int array for this scene
    /// 0: EndMap - tạo biến di chuyển vào vùng kết thúc map, 0 = ngoài vùng end map, 1 = bên trái map, 2 = bên phải map
    /// </summary>
    private int[] IntNumber = new int[10];

    /// <summary>
    /// Bool array for this scene
    /// 0: EndMap - check xem đã end map hay chưa, kết hợp với IntNumber[0]
    /// 1: Set Anim - check xem đã set Anim hay chưa
    /// 2: Check xem đã nhả di chuyển trái hay chưa
    /// 3: Check xem đã nhả di chuyển phải hay chưa
    /// 4: Check anim stand
    /// </summary>
    private bool[] BolNumber = new bool[10];

    /// <summary>
    /// Float array for this scene
    /// 0: Tọa độ Y cứng của camera
    /// </summary>
    private float[] FloatNumber = new float[1];
    #endregion

    #region Functions
    protected override void Awake()
    {
        int IDHeroes = Convert.ToInt32(Module.GameLoad("CharID"));//Sửa lại chỗ này, sẽ tạo biến get ID Heroes khi load game
        GObjectNumber[0] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Heroes" + IDHeroes), new Vector3(Module.POSMAINHERO[0], Module.POSMAINHERO[1], Module.POSMAINHERO[2]), Quaternion.identity);
        //GObjectNumber[2] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/SkillControlHeroes" + IDHeroes), new Vector3(-1000, -1000, 0), Quaternion.identity);
        Camera.main.transform.SetParent(GObjectNumber[0].transform);
        FloatNumber[0] = Camera.main.transform.position.y;
        Anim = GObjectNumber[0].GetComponent<Animator>();
        BolNumber[1] = true;
        Ctrl = GameObject.FindGameObjectWithTag("ControlScene");
        Ctrl.GetComponent<System_Battle>().HeroObject = new GameObject[1];
        Ctrl.GetComponent<System_Battle>().HeroObject[0] = GObjectNumber[0];//Gán object cho battle system để quản lý
        HeroComponent = GObjectNumber[0].GetComponent<BaseHeroes>();
        SystemBattle = Ctrl.GetComponent<System_Battle>();
    }

    private void Start()
    {
        for (int i = 0; i < ObjButtonSkill.Length; i++)
            ObjButtonSkill[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
        HeroPos = GObjectNumber[0].transform.position;
    }

    #region Move tap
    public void HeroMoveLeft(BaseEventData eventData)
    {
        BolNumber[1] = false;
        MoveRight = false;
        MoveLeft = true;
        HeroSee = true;
        Module.CURRENSEE = !HeroSee;
    }

    public void HeroMoveRight(BaseEventData eventData)
    {
        BolNumber[1] = false;
        MoveRight = true;
        MoveLeft = false;
        HeroSee = false;
        Module.CURRENSEE = !HeroSee;
    }

    public void PointerUpLeft(BaseEventData eventData)
    {
        if (!HeroComponent.BolNumber[0])
        {
            if (HeroSee)
            {
                if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                    Anim.SetTrigger("Idie");
                MoveLeft = false;
                if (HeroComponent.BolNumber[0])
                    HeroComponent.EndAtk();
            }
        }
        else
        {
            MoveLeft = false;
            BolNumber[3] = false;
            BolNumber[2] = true;

        }
    }

    public void PointerUpRight(BaseEventData eventData)
    {
        if (!HeroComponent.BolNumber[0])
        {
            if (!HeroSee)
            {
                if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                    Anim.SetTrigger("Idie");
                MoveRight = false;
                if (HeroComponent.BolNumber[0])
                    HeroComponent.EndAtk();
            }
        }
        else
        {
            MoveRight = false;
            BolNumber[2] = false;
            BolNumber[3] = true;
        }
        //PointerUpRight(eventData);
    }

    /// <summary>
    /// Nhấn Up
    /// </summary>
    /// <param name="eventData"></param>
    public void PointerEnterUp(BaseEventData eventData)
    {
        PressUp = true;
    }

    /// <summary>
    /// Nhả Up
    /// </summary>
    /// <param name="eventData"></param>
    public void PointerUpUp(BaseEventData eventData)
    {
        PressUp = false;
    }

    /// <summary>
    /// Nhấn Down
    /// </summary>
    /// <param name="eventData"></param>
    public void PointerEnterDown(BaseEventData eventData)
    {
        PressDown = true;
    }

    /// <summary>
    /// Nhả Down
    /// </summary>
    /// <param name="eventData"></param>
    public void PointerUpDown(BaseEventData eventData)
    {
        PressDown = false;
    }

    #endregion

    public void Flip(int x)
    {
        Vector3 theScale = GObjectNumber[0].transform.localScale;
        switch (x)
        {
            case 1://Left
                if (theScale.x > 0)
                    theScale.x *= -1;
                break;
            case 2://Right
                if (theScale.x < 0)
                    theScale.x *= -1;
                break;
            default: break;
        }
        GObjectNumber[0].transform.localScale = theScale;
    }

    /// <summary>
    /// Thay đổi parent camera
    /// </summary>
    /// <param name="enable"></param>
    private void ChangeParentCamera(bool enable)
    {
        if (enable)
        {
            Camera.main.transform.SetParent(GObjectNumber[0].transform);
        }
        else
        {
            Camera.main.transform.SetParent(GObjectNumber[1].transform);
        }
        //Camera.main.transform.position = new Vector3(0f,2f, Camera.main.transform.position.z);
        BolNumber[0] = false;
        //yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Kiểm tra khi Heroes đi vào 2 bên map
    /// </summary>
    private void CheckAndChangeMove()
    {
        if (HeroPos.x < Module.LIMITMAPMOVE[0])//Giới hạn di chuyển sang trái
        {
            if (IntNumber[0] != 1)
            {
                SystemBattle.PPos = System_Battle.PlayerPos.Left;
                BolNumber[0] = true;
                ChangeParentCamera(false);
                IntNumber[0] = 1;
                CamTemp.x = Module.LIMITMAPMOVE[0];
                Camera.main.transform.position = new Vector3(CamTemp.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        if (HeroPos.x >= Module.LIMITMAPMOVE[0] && HeroPos.x <= Module.LIMITMAPMOVE[1])
        {
            if (IntNumber[0] != 0)
            {
                SystemBattle.PPos = System_Battle.PlayerPos.Mid;
                BolNumber[0] = true;
                ChangeParentCamera(true);
                IntNumber[0] = 0;
            }
        }
        if (HeroPos.x > Module.LIMITMAPMOVE[1])//Giới hạn di chuyển sang phải
        {
            if (IntNumber[0] != 2)
            {
                SystemBattle.PPos = System_Battle.PlayerPos.Right;
                BolNumber[0] = true;
                ChangeParentCamera(false);
                IntNumber[0] = 2;
                CamTemp.x = Module.LIMITMAPMOVE[1];
                Camera.main.transform.position = new Vector3(CamTemp.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        GObjectNumber[1].transform.position = new Vector3(GObjectNumber[1].transform.position.x, FloatNumber[0], GObjectNumber[1].transform.position.z);
    }

    /// <summary>
    /// Di chuyển và thay đổi Anim hero
    /// </summary>
    private void MoveControl()
    {
        //Điều khiển nhấn/nhả phím di chuyển trong lúc tung skill
        if (!HeroComponent.BolNumber[0] && HeroComponent.BolNumber[6])//Nếu đang ko dùng skill và cho phép di chuyển
        {
            if (BolNumber[2])//Chuyển move từ phải sang trái
            {
                if (MoveLeft)
                    HeroSee = HeroComponent.BolNumber[5];
                MoveLeft = false;
                BolNumber[2] = false; BolNumber[4] = true;
            }
            if (BolNumber[3])//Chuyển move từ trái sang phải
            {
                if (MoveRight)
                    HeroSee = HeroComponent.BolNumber[5];
                MoveRight = false;
                BolNumber[3] = false; BolNumber[4] = true;
            }
            if (BolNumber[4])
                if (!MoveLeft && !MoveRight)
                {
                    Anim.SetTrigger("Idie");
                    BolNumber[4] = false;
                }
            if (HeroComponent.BolNumber[0])//Gỡ lỗi animation chưa được end attack
                HeroComponent.EndAtk();
        }
        if (HeroComponent.BolNumber[0] && HeroComponent.BolNumber[6])//Nếu đang dùng skill và cho phép di chuyển
        {
            if (MoveLeft)
            {
                if (HeroPos.x >= Module.LIMITMAPMOVE[0] - Module.RANGEMOVELIMIT)
                    HeroPos.x -= Module.HeroMoveSpeedDefault * Time.deltaTime * GameSystem.Settings.FPSLimit;//Di chuyển chậm lại 1/2
            }

            if (MoveRight)
            {
                if (HeroPos.x <= Module.LIMITMAPMOVE[1] + Module.RANGEMOVELIMIT)
                    HeroPos.x += Module.HeroMoveSpeedDefault * Time.deltaTime * GameSystem.Settings.FPSLimit;//Di chuyển chậm lại 1/2
            }
        }
        //Điều khiển di chuyển
        if (!HeroComponent.BolNumber[0] && HeroComponent.BolNumber[6])//Nếu đang ko dùng skill và cho phép di chuyển
        {
            if (MoveLeft)
            {
                if (!BolNumber[1])
                {
                    Flip(1);
                    Anim.ResetTrigger("Idie");
                    if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                        Anim.SetTrigger("Move");
                    BolNumber[1] = true;
                }
                if (HeroPos.x >= Module.LIMITMAPMOVE[0] - Module.RANGEMOVELIMIT)
                    HeroPos.x -= Module.HeroMoveSpeedDefault * Time.deltaTime * 60;
            }
            if (MoveRight)
            {
                if (!BolNumber[1])
                {
                    Flip(2);
                    Anim.ResetTrigger("Idie");
                    if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                        Anim.SetTrigger("Move");
                    BolNumber[1] = true;
                }
                if (HeroPos.x <= Module.LIMITMAPMOVE[1] + Module.RANGEMOVELIMIT)
                    HeroPos.x += Module.HeroMoveSpeedDefault * Time.deltaTime * 60;

            }
        }
    }

    /// <summary>
    /// Update for this scene
    /// </summary>
    private void Update()
    {
        if (!Module.PAUSEGAME)
        {
            MoveControl();//Di chuyển và thay đổi Anim hero
                          //HeroPos.y -= Module.GRAVITYHERO * Time.deltaTime;
            GObjectNumber[0].transform.position = new Vector3(HeroPos.x, GObjectNumber[0].transform.position.y, HeroPos.z);
            if (SystemBattle.PPos.Equals(System_Battle.PlayerPos.Mid))
                Camera.main.transform.position = new Vector3(GObjectNumber[0].transform.position.x, FloatNumber[0], Camera.main.transform.position.z);
            CheckAndChangeMove();//Kiểm tra và thay đổi parent camera khi Heroes đi vào 2 bên map
        }
        //Develop mode - Disable khi build cho mobile
        DevelopMode();
    }
    #endregion

    #region Develop mode

    /// <summary>
    /// Develop mode - Test chức năng cho develop
    /// </summary>
    private void DevelopMode()
    {
        //Nhập keyword lên
        if (Input.GetKeyDown(KeyCode.W))
        {
            PressUp = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            PressUp = false;
        }
        //Nhập keyword trái phải
        if (Input.GetKeyDown(KeyCode.A))
        {
            BolNumber[1] = false;
            MoveRight = false;
            MoveLeft = true;
            HeroSee = true;
            Module.CURRENSEE = !HeroSee;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!HeroComponent.BolNumber[0])
            {
                if (HeroSee)
                {
                    if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                        Anim.SetTrigger("Idie");
                    MoveLeft = false;
                    if (HeroComponent.BolNumber[0])
                        HeroComponent.EndAtk();
                }
            }
            else
            {
                MoveLeft = false;
                BolNumber[3] = false;
                BolNumber[2] = true;

            }
        }
        //Nhập keyword phải
        if (Input.GetKeyDown(KeyCode.D))
        {
            BolNumber[1] = false;
            MoveRight = true;
            MoveLeft = false;
            HeroSee = false;
            Module.CURRENSEE = !HeroSee;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!HeroComponent.BolNumber[0])
            {
                if (!HeroSee)
                {
                    if (!HeroComponent.BolNumber[7])//Nếu đang ko nhảy
                        Anim.SetTrigger("Idie");
                    MoveRight = false;
                    if (HeroComponent.BolNumber[0])
                        HeroComponent.EndAtk();
                }
            }
            else
            {
                MoveRight = false;
                BolNumber[2] = false;
                BolNumber[3] = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (!Module.PAUSEGAME)
            {
                HeroComponent.Attack(PressUp, PressDown);
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (!Module.PAUSEGAME)
            {
                StartCoroutine(HeroComponent.Skill(1));
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (!Module.PAUSEGAME)
            {
                StartCoroutine(HeroComponent.Skill(2));
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (!Module.PAUSEGAME)
            {
                StartCoroutine(HeroComponent.Skill(3));
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (!Module.PAUSEGAME)
            {
                StartCoroutine(HeroComponent.Skill(4));
            }
        }
    }
    #endregion

    #region Skill System

    public void Jump(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            //HeroPos.y = Module.JUMPSPEED;
            if (!HeroComponent.BolNumber[7] && !HeroComponent.BolNumber[0] && !HeroComponent.BolNumber[8])//Nếu chưa nhảy và đang ko sử dụng skill và đang ko lướt
            {
                Anim.SetTrigger("Jump");
                HeroComponent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 40), ForceMode2D.Impulse);
                HeroComponent.BolNumber[7] = true;
            }
            return;
        }
    }
    public void Attack(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            HeroComponent.Attack(PressUp, PressDown);
            return;
        }
    }
    public void Skill1(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            StartCoroutine(HeroComponent.Skill(1));
            return;
        }
    }
    public void Skill2(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            StartCoroutine(HeroComponent.Skill(2));
            return;
        }
    }
    public void Skill3(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            StartCoroutine(HeroComponent.Skill(3));
            return;
        }
    }
    public void Skill4(BaseEventData eventData)
    {
        if (!Module.PAUSEGAME)
        {
            StartCoroutine(HeroComponent.Skill(4));
            return;
        }
    }
    #endregion
}

