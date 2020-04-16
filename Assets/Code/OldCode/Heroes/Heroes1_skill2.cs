using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heroes1_skill2 : SkillBase
{
    //Skill bắn lan 2 bên  - Heroes 1
    private float SpeedWeaponFly = 0.8f;//Tốc độ bay ngang của phi tiêu
    private float SpeedWeaponFlyY = 0.4f;//Tốc độ bay xuống của phi tiêu
    private float SpeedWeaponRotate = 30f;//Tốc độ bay của phi tiêu
    private float PosLine = -3.0f;//Toa do cham dat
    private float TimeLive = 1.0f;//Thời gian tồn tại sau khi chạm đât
    public bool herosee;//check hướng sẽ bay về bên nào
    public bool Expired;//Kiểm tra xem phi tiêu đã chạm đất hay chưa, nếu chạm đất thì ko còn gây sát thương được nữa
    public float RepelValue;//Giá trị đẩy lùi quái của đòn đánh 
    private Vector3 Vec;
    Vector3 Rot;
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
        GetComponent<Collider2D>().enabled = true;
        Vec = gameObject.transform.position;
        Rot = gameObject.transform.localEulerAngles;
        SpeedWeaponFlyY = Random.Range(0.3f, 1.5f);
        StartCoroutine(AutoHiden(1f));
    }

    /// <summary>
    /// Xử lý va chạm
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!Expired)//Nếu phi tiêu chưa chạm đất
        {
            //Va chạm với enemy
            if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2]))//BASELAYERRIGID2D xem trong Module
            {
                BaseEnemy = col.GetComponent<BaseEnemys>();
                if (GameSystem.Settings.SoundEnable)
                StartCoroutine(BaseHero.PlaySound(BaseHero.AudioHit[Random.Range(0, BaseHero.AudioHit.Length)], 0));//Play random sound
                BaseEnemy.BaseValues[5] = Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko
                SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            }
        }
    }
    public override IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
    public override void Hide()
    {
        base.Hide();
        gameObject.tag = "Repel";
        Expired = false;//Đưa về trạng thái sẽ gây sát thương
    }
    private void Update()
    {
        if (!Module.PAUSEGAME)
        {
            //Rơi xuống đất => hết tác dụng gây dame
            if (Vec.y < -3f)
            {
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";
                Expired = true;
            }
            //Nếu chạm đất
            if (!Expired)//Nếu chưa chạm đất
            {
                if (herosee)
                {
                    Vec.x += SpeedWeaponFly * Time.deltaTime * GameSystem.Settings.FPSLimit;
                    Vec.y -= SpeedWeaponFlyY * Time.deltaTime * GameSystem.Settings.FPSLimit;
                    Rot.z += SpeedWeaponRotate * Time.deltaTime * GameSystem.Settings.FPSLimit;
                }
                else
                {
                    Vec.x -= SpeedWeaponFly * Time.deltaTime * GameSystem.Settings.FPSLimit;
                    Vec.y -= SpeedWeaponFlyY * Time.deltaTime * GameSystem.Settings.FPSLimit;
                    Rot.z -= SpeedWeaponRotate * Time.deltaTime * GameSystem.Settings.FPSLimit;
                }
                gameObject.transform.localEulerAngles = Rot;
                gameObject.transform.position = Vec;
            }
        }
    }
}
