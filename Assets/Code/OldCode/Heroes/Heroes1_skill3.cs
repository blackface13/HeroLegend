using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Heroes1_skill3 : SkillBase
{
    //Đặt bẫy - Hero 1 - Giữ chân enemy 2 giây, sau 2s, bẫy nổ và đẩy lùi enemy
    GameObject[] EffectObject = new GameObject[1];//Tạo object hiệu ứng nổ sau khi enemy dẫm phải bẫy
    float TimeExpired = 20f;//Thời gian tồn tại của bẫy khi chưa bị dẫm phải
    float TimeKeep = 2f;//Thời gian giữ chân đối phương của skill
    float TimeBeforeHide = 1f;//Thời gian tồn tại bẫy trước khi biến mất
    Sprite[] ImgChange = new Sprite[2];
    /// <summary>
    /// Khởi tạo sau khi tạo object
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        EffectObject[0] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk3_eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
        EffectObject[0].SetActive(false);
        ImgChange[0] = Resources.Load<Sprite>("Skill/0");//Hình ảnh trước khi bị dẫm
        ImgChange[1] = Resources.Load<Sprite>("Skill/1");//Hình sau khi bị dẫm
        DamePer = 30;//Số lượng % dame bị dính khi dẫm phải bẫy (10 = 10%)
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
    }

    /// <summary>
    /// Khởi tạo sau khi tái sử dụng
    /// </summary>
    private void OnEnable()
    {
        // gameObject.tag = "Keep";
        GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = ImgChange[0];
        StartCoroutine(AutoHiden(TimeExpired));//Khởi động tự biến mất sau khoảng time ko ai động vào
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
            gameObject.GetComponent<SpriteRenderer>().sprite = ImgChange[1];
            BaseEnemy = col.GetComponent<BaseEnemys>();//Khởi tạo Enemy
                if (GameSystem.Settings.SoundEnable)
                StartCoroutine(BaseHero.PlaySound(BaseHero.AudioHit[UnityEngine.Random.Range(0, BaseHero.AudioHit.Length)], 0));//Play random sound
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            BaseEnemy.BaseValues[7] = TimeKeep;//Thời gian giữ chân đối phương
            //SystemBattle.ShowDamage(col.transform.position, Random.Range(10, 99).ToString());
            StartCoroutine(AutoHiden(TimeBeforeHide));//Khởi động tự biến mất sau khi bẫy sập
            GetComponent<Collider2D>().enabled = false;//Khóa va chạm khi bẫy đã sập
                                                       // gameObject.tag = "Untagged";
        }
    }
    /// <summary>
    /// Tự động ẩn object
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public override IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        ShowSkill(EffectObject[0], this.transform.position, new Quaternion());//Hiển thị hiệu ứng nổ trước khi bẫy biến mất
        Hide();
    }
}
