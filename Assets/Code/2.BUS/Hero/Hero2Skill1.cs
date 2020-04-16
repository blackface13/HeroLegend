using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackCore;
using UnityEngine.UI;
using System;
namespace Controller.Hero2
{
    public class Hero2Skill1 : SkillCore
    {
        public GameObject[] ObjChild;
        //public Hero2 Hero;
        public float _TimeDelay;
        public float _TimeAction;
        public override void Awake()
        {
            base.Awake();
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[4];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H3Nor" + (i + 1).ToString ());
            }
        }
        public override void Start()
        {
            base.Start();
            //Status = status.Stun;//Hiệu ứng choáng
            //TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues;//Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 175;//Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            for (int i = 0; i < ObjChild.Length; i++)
            {
                ObjChild[i].GetComponent<Hero2Skill1>().Hero = Hero;
                ObjChild[i].GetComponent<Hero2Skill1>().ReSetupLayer(Team);
            }
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
        }
        private void OnEnable()
        {
            StartCoroutine(AutoEnableCol(_TimeDelay, gameObject));//Tự động bật va chạm
            StartCoroutine(AutoDisCol(_TimeAction, gameObject));//Tự động vô hiệu hóa gây dame sau time
            //StartCoroutine(AutoHiden(2f, gameObject));
            if(GameSystem.Settings.SoundEnable)
            StartCoroutine(PlaySound());
        }

//Hàm chạy âm thành dành riêng cho skill này
        private IEnumerator PlaySound()
        {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.1f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.1f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.1f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
        }
        /// <summary>
        /// Xử lý va chạm
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerEnter2D(Collider2D col)
        {
            //Va chạm với enemy
            // if (col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2]))//BASELAYERRIGID2D xem trong Module
            // {
            //     // BaseEnemy = col.GetComponent<BaseEnemys>();
            //     // BaseEnemy.BaseValues[5] = 0;// Random.Range(0.1f, 0.2f);//Đòn đánh này có đẩy lùi quái hay ko
            //     // SystemBattle.Damage(BaseHero, BaseEnemy, col.transform.position, DamePer, 0, 0);
            //     //Hide();//Ẩn object sau khi va chạm 
            // }
        }
    }
}