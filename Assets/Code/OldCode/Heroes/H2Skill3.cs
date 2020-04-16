using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2Skill3 : SkillBase
{
    float TimeDelay = 0.1f;//Delay start
    float TimeAction = 0.1f;//Time tồn tại
    int ComboTotal = 8, ComboCount = 0;
    Collider2D ThisCollider;
    public override void Awake()
    {
        base.Awake();
        ThisCollider = GetComponent<Collider2D>();
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
    }
    private void OnEnable()
    {
        ThisCollider.enabled = false;
        ComboCount = 0;
        StartCoroutine(StartDelay(TimeDelay));
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
            BaseEnemy.BaseValues[5] = Random.Range(0.1f, 0.2f);//Tạo giá trị đẩy lùi quái
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            //Hide();//Ẩn object sau khi va chạm 
        }
    }

    /// <summary>
    /// Khởi tạo delay chờ tới lúc gây sát thương
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDelay(float _time)
    {
        yield return new WaitForSeconds(_time);
        StartCoroutine(ControlDamage());
    }

    /// <summary>
    /// Điều khiển sát thương, sử dụng đệ quy
    /// </summary>
    /// <returns></returns>
    private IEnumerator ControlDamage()
    {
        ThisCollider.enabled = true;
        ComboCount++;
        yield return new WaitForSeconds(TimeAction);
        ThisCollider.enabled = false;
        if (ComboCount < ComboTotal)
            StartCoroutine(ControlDamage());
    }
}
