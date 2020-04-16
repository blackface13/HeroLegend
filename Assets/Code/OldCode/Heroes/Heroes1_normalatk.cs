using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heroes1_normalatk : SkillBase
{
    //Normal atk đánh xa - Heroes 1
    private float SpeedWeaponFly = 0.8f;//Tốc độ bay của phi tiêu
    private float SpeedWeaponRotate = 30f;//Tốc độ bay của phi tiêu
    public bool herosee;
    //private GameObject[] EffectObject = new GameObject[1];
    private List<GameObject> EffectObject = new List<GameObject>();
    private Vector3 Vec;
    public bool Expired;
    Vector3 Rot;
    public override void Awake()
    {
        base.Awake();
        EffectObject.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk1_Eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f)));
        //EffectObject[0] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk1_Eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f));
        EffectObject[0].SetActive(false);
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
        //herosee = Module.CURRENSEE;
        StartCoroutine(AutoHiden(1f));
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
            BaseEnemy.BaseValues[5] = Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko
                                                               //if (GameSystem.Settings.SoundEnable)
                                                               //StartCoroutine(BaseHero.PlaySound(BaseHero.AudioHit[UnityEngine.Random.Range(0, BaseHero.AudioHit.Length)], 0));//Play random sound
            SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            for (int i = 0; i < EffectObject.Count; i++)
                if (!EffectObject[i].activeSelf)
                {
                    EffectObject[i].transform.position = gameObject.transform.position;
                    EffectObject[i].SetActive(true);
                    break;
                }
                else
                {
                    if (i.Equals(EffectObject.Count - 1))
                    {
                        EffectObject.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk1_Eff"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f)));
                        EffectObject[i + 1] = Instantiate(Resources.Load<GameObject>("Prefabs/Heroes1_objatk1_Eff"), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Module.BASELAYER[2]), Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            Hide();//Ẩn object sau khi va chạm 
        }
    }

    private void Update()
    {
        if (!Module.PAUSEGAME)
        {
            if (herosee)
            {
                Vec.x += SpeedWeaponFly;
                Rot.z += SpeedWeaponRotate;
            }
            else
            {
                Vec.x -= SpeedWeaponFly;
                Rot.z -= SpeedWeaponRotate;
            }
            gameObject.transform.localEulerAngles = Rot;
            gameObject.transform.position = Vec;
        }
    }
}
