using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy1 : BaseEnemys
{
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(GetValuesBase());//Setup values for this Enemy
        Rectangle = new Vector2(2f, 2.5f);//Kích thước enemy, phục vụ va chạm
        Gobject = new GameObject[5];
        ThisVec = gameObject.transform.position;
        //Cài đặt chỉ số riêng cho mỗi Enemy
        BaseValues[0] = 1;
        BaseValues[1] = 10;//Tầm phát hiện Hero
        BaseValues[2] = Random.Range(3f, 6f);//Tầm đánh
        BaseValues[3] = 0.8f;//Tốc độ thể hiện anim khi chạy
        BaseValues[4] = Module.EnemyMoveSpeedDefault;//Tốc độ chạy
        //BaseValues[5] = Random.Range(0.1f, 0.3f);//Khoang cach bi day lui - Set cái này ở object skill của Hero
        BaseValues[6] = Random.Range(3f, 8f);//Thời gian delay normal attack
    }
    //public override void Start()
    //{
    //    base.Start();
    //}
    /// <summary>
    /// Setup values for this Enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetValuesBase()
    {
        Prop = new List<SystemProperties>();
        for (int i = 0; i < DBEnemy.DBEnemy.Count; i++)
            foreach (SystemProperties baseenemy in DBEnemy.DBEnemy)
            {
                if (baseenemy.ID.Equals(EnemyID))
                {
                    Prop.Add(baseenemy);
                    break;
                }
            }
        yield return new WaitForSeconds(0);
    }

    public override void Update()
    {
        if (!Module.PAUSEGAME)
        {
            base.Update();
            //EnemyMove();
            //LimitMove();
            //print(EneStatus);
            //print(BaseValues[1]);
        }
    }

    public void LimitMove()
    {
        if (transform.position.x < Module.LIMITMAPMOVE[0] - Module.RANGEMOVELIMIT)
            transform.position = new Vector3(Module.LIMITMAPMOVE[0] - Module.RANGEMOVELIMIT + 1, transform.position.y, transform.position.z);
        if (transform.position.x > Module.LIMITMAPMOVE[1] + Module.RANGEMOVELIMIT)
            transform.position = new Vector3(Module.LIMITMAPMOVE[1] + -Module.RANGEMOVELIMIT - 1, transform.position.y, transform.position.z);
    }
    /// <summary>
    /// Gây sát thương
    /// </summary>
    public void Hit()
    {
        if (BaseFloatNumber[3])//Nếu đang trong tầm gây sát thương
            SystemBattle.Damage(BaseHero, this, Player.transform.position, DamePer, 0, 1);

    }

    void OnBecameVisible()
    {
        gameObject.SetActive(true);
    }
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
