using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heroes1_skill3_eff : SkillBase
{
    public float DelayShakeTime;//Set ở interface, thời gian delay hiệu ứng rung màn hình
    //Hiệu ứng nổ của skill đặt bẫy
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
    }
    /// <summary>
    /// Xuất hiện
    /// </summary>
    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
        //StartCoroutine(AutoHiden(1.5f));//Cho phép tồn tại bao lâu trước khi ẩn object (đây là object con, phải ẩn object cha, ko sử dụng hàm này)
        StartCoroutine(StartShake(DelayShakeTime));//Rung màn hình sau khi xuất hiện
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StartShake(float time)
    {
        yield return new WaitForSeconds(time);
        SystemBattle.Shake(0.1f);//Rung màn hình khi nổ
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
            BaseEnemy = col.GetComponent<BaseEnemys>();//Khởi tạo Enemy
                if (GameSystem.Settings.SoundEnable)
                StartCoroutine(BaseHero.PlaySound(BaseHero.AudioHit[UnityEngine.Random.Range(0, BaseHero.AudioHit.Length)], 0));//Play random sound
            BaseEnemy.BaseValues[5] = Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko           
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
        }
        GetComponent<Collider2D>().enabled = false;
    }
}
