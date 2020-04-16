using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heroes1_skill4 : SkillBase
{
    //Skill bắn 3 tia và rút về  - Heroes 1
    private float[] RangeXY = new float[2] { 15f, 3f };//Khoảng cách gây sát thương của skill
    private float SpeedWeaponRotate = 30f;//Tốc độ xoay của phi tiêu
    private float TimeMove = 0.3f;//Thời gian bay của phi tiêu tới đích
    public bool herosee;//check hướng sẽ bay về bên nào
    public bool Expired;//Kiểm tra xem phi tiêu đã tới nơi hay chưa
    public float RepelValue;//Giá trị đẩy lùi quái của đòn đánh 
    Vector3 Rot;
    public Vector3 CurentPos;//Vị trí hiện tại của object
    public Vector3 TargetPos;//Vị trí mà phi tiêu sẽ bay tới
    public int ObjectType;//Set trong khi tạo object sử dụng scirpt này, phi tiêu sẽ bay chéo lên, ngang, hoặc chéo xuống tương ứng 123

    [Header("Draw Curve")]
    public AnimationCurve moveCurve;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
    }
    private void OnEnable()
    {
        //herosee = Module.CURRENSEE;//Set ở hàm bắt đầu chạy animation
        GetComponent<Collider2D>().enabled = true;
        CurentPos = gameObject.transform.position;
        if (ObjectType.Equals(1))//Phi tiêu bay chéo lên phía trên
        {
            TargetPos = herosee ? new Vector3(CurentPos.x + RangeXY[0], CurentPos.y + RangeXY[1], CurentPos.z) : new Vector3(CurentPos.x - RangeXY[0], CurentPos.y + RangeXY[1], CurentPos.z);
        }
        if (ObjectType.Equals(2))//Phi tiêu bay ngang
        {
            TargetPos = herosee ? new Vector3(CurentPos.x + RangeXY[0], CurentPos.y, CurentPos.z) : new Vector3(CurentPos.x - RangeXY[0], CurentPos.y, CurentPos.z);
        }
        if (ObjectType.Equals(3))//Phi tiêu chéo xuống
        {
            TargetPos = herosee ? new Vector3(CurentPos.x + RangeXY[0], CurentPos.y - RangeXY[1], CurentPos.z) : new Vector3(CurentPos.x - RangeXY[0], CurentPos.y - RangeXY[1], CurentPos.z);
        }
        Rot = gameObject.transform.localEulerAngles;
        StartCoroutine(MoveObject(TargetPos, TimeMove));//Khởi động bay khi vừa xuất hiện
    }

    public IEnumerator MoveObject(Vector3 targetPos, float duration)
    {
        float time = 0;
        float rate = 1 / duration;
        Vector3 startPos = transform.localPosition;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(time));
            yield return 0;
        }
        transform.localPosition = targetPos;
    }
    /// <summary>
    /// Xử lý va chạm
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        //Va chạm với enemy
        if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2]))//BASELAYERRIGID2D xem trong Module
        {
            BaseEnemy = col.GetComponent<BaseEnemys>();
            if (GameSystem.Settings.SoundEnable)
                StartCoroutine(BaseHero.PlaySound(BaseHero.AudioHit[UnityEngine.Random.Range(0, BaseHero.AudioHit.Length)], 0));//Play random sound
            BaseEnemy.BaseValues[5] = Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko            
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);

        }
    }

    public override IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]);
        gameObject.transform.localEulerAngles = new Vector3();
        Expired = false;
    }
    private void Update()
    {
        if (!Module.PAUSEGAME)
        {
            //Rotate control
            if (herosee)
                Rot.z += SpeedWeaponRotate * Time.deltaTime * GameSystem.Settings.FPSLimit;
            else
                Rot.z -= SpeedWeaponRotate * Time.deltaTime * GameSystem.Settings.FPSLimit;
            gameObject.transform.localEulerAngles = Rot;
            if (!Expired)
            {
                if (gameObject.transform.position == TargetPos)
                {
                    GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(MoveObject(CurentPos, TimeMove));
                    GetComponent<Collider2D>().enabled = true;

                    Expired = true;
                }
            }
            if (Expired)
            {
                if (gameObject.transform.position == CurentPos)
                {
                    Hide();
                }
            }
        }
    }
}
