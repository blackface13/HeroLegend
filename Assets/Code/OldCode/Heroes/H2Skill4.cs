//Skill 4, quét kiếm tạo ra 3 vệt gây sát thương trên đường thẳng (Object này ẩn để gây dame, object eff mới hiển thị effect)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2Skill4 : SkillBase
{
    private float RangeAction = 32f;
    private float TimeHiden = 0.35f;//Thời gian tồn tại, cũng là time gây ra sát thương
    private float SpeedFly = 1.6f;//Tốc độ bay của hiệu ứng
    private Vector3 VecTemp;
    private bool herosee;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
        herosee = BaseHero.BolNumber[5];//Set theo hướng nhìn của char, ko phải hướng bấm
    }
    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
        VecTemp = transform.position;
        StartCoroutine(AutoHiden(TimeHiden));
        if (BaseHero != null)
        herosee = BaseHero.BolNumber[5];//Set theo hướng nhìn của char, ko phải hướng bấm
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
            BaseEnemy.BaseValues[5] = Random.Range(0.2f, 0.3f);//Đòn đánh này có đẩy lùi quái hay ko
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            //Hide();//Ẩn object sau khi va chạm 
        }
    }
    private void Update()
    {
        if (!herosee)//Phải
        {
            VecTemp.x += SpeedFly;
        }
        else
        {
            VecTemp.x -= SpeedFly;
        }
        transform.position = VecTemp;
    }

    /// <summary>
    /// Auto hide object
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public override IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}