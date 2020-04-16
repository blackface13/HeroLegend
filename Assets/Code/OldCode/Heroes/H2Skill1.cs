using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2Skill1 : SkillBase
{
    public float TimeDelay;
    public float TimeAction;

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
        StartCoroutine(AutoEnableCol(TimeDelay));//Tự động bật va chạm
        StartCoroutine(AutoDisCol(TimeAction));//Tự động vô hiệu hóa gây dame sau time
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
            BaseEnemy.BaseValues[5] = 0;// Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            //Hide();//Ẩn object sau khi va chạm 
        }
    }
    /// <summary>
    /// Auto disable collision
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator AutoDisCol(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator AutoEnableCol(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Collider2D>().enabled = true;
    }
}
