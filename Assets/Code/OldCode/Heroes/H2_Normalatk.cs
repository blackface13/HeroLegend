using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2_Normalatk : SkillBase
{
    public bool herosee;
    private float TimeAutoHide = 0.5f; //Tự động ẩn sau time
    private float TimeDisableCollision = 0.3f;//Tự động vô hiệu hóa gây dame sau time
    private GameObject[] EffectAtk = new GameObject[5];//Hiệu ứng đòn đánh khi đánh trúng enemy, số lượng mảng tương ứng với số lượng enemy mỗi lần xuất hiện
    [Header("Kiểu skill. 0=normal atk. 1=skill3.")]
    public int TypeSkill;//Kiểu skill 0 = normal atk, 1 = skill 3 (Cái này dùng chung cho normal atk và skill 3)
    [Header("Thứ tự combo. 1, 2, 3")]//Set ở hiệu ứng chém
    public int STTCombo;//Thứ tự combo. 0, 1, 2
    public override void Awake()
    {
        base.Awake();
        if (TypeSkill.Equals(0))//Chỉ tạo hiệu ứng với các đòn đánh thường
            for (int i = 0; i < EffectAtk.Length; i++)
            {
                //Khởi tạo object effect cho slash 1
                EffectAtk[i] = Instantiate(Resources.Load<GameObject>("Prefabs/H2Atk" + STTCombo + "Slash"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f));
                EffectAtk[i].SetActive(false);
            }
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
    }

    /// <summary>
    /// Enable this object
    /// </summary>
    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
        //herosee = Module.CURRENSEE;
        if (TypeSkill.Equals(0))//Nếu là normal atk thì sẽ tự động ẩn, còn skill 3 thì ko
        {
            StartCoroutine(AutoHiden(TimeAutoHide));//Tự động ẩn sau time
            if (!herosee)
            {
                if (Player != null)
                {
                    transform.position = new Vector3(transform.position.x - ((transform.position.x - Player.transform.position.x) * 2), transform.position.y, transform.position.z);
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            transform.GetChild(0).transform.localScale = transform.localScale;
        }
        StartCoroutine(AutoDisCol(TimeDisableCollision));//Tự động vô hiệu hóa gây dame sau time
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
            if (STTCombo.Equals(3))//Nếu đòn đánh thứ 3 mới đẩy lùi
                BaseEnemy.BaseValues[5] = Random.Range(0.05f, 0.15f);//Đòn đánh này có đẩy lùi quái hay ko
            else BaseEnemy.BaseValues[5] = 0f;
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            //Enable hiệu ứng chém trúng
            if (TypeSkill.Equals(0))//Chỉ có tác dụng với các đòn đánh thường
                for (int i = 0; i < EffectAtk.Length; i++)
                {
                    if (!EffectAtk[i].activeSelf)
                    {
                        EffectAtk[i].transform.position = col.transform.position;
                        EffectAtk[i].transform.localScale = new Vector3(this.transform.localScale.x * -1, EffectAtk[i].transform.localScale.y, EffectAtk[i].transform.localScale.z);
                        EffectAtk[i].SetActive(true);
                        break;
                    }
                }
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

    public override IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
    public override void Hide()
    {
        base.Hide();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
