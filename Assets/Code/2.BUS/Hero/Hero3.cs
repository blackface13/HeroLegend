using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero3 {
    public class Hero3 : HeroBase {
        //Initialize
        public override void Awake () {
            base.Awake ();
            HType = HeroType.far; //Tướng đánh xa
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object phi tiêu
            Skill2 = new List<GameObject> (); //Skill như Q của Sivir
            Skill3 = null;
            SkillType = new int[3]; //Kiểu skill

            // Đưa vào scene
            // for (int i = 0; i < 5; i++)
            // {
            Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero3Atk"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill1[0].GetComponent<Hero3Atk> ().Hero = this;
            Skill1[0].SetActive (false);
            //}
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero3Skill"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler (70f, 0, 0)));
            Skill2[0].GetComponent<Hero3Skill> ().Hero = this;
            Skill2[0].SetActive (false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - skill 1
            SkillType[2] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam (GameObject obj) {
            base.RefreshTeam (obj);
        }
        //Update
        public override void Update () {
            base.Update ();
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    // var objdontactive = GetObjectDontActive(Skill1);//Tìm object chưa dc kích hoạt
                    // if (objdontactive != null)
                    // {
                    if (ComboNormalAtk == 2) //Nếu là đòn đánh thứ 3 của combo
                    {
                        StartCoroutine (MultiAtk (Skill1, this.transform.position, .1f, 3, Quaternion.identity));
                        //StartCoroutine(ComboAtk());
                        // var obj1 = GetObjectDontActive(Skill1);
                        // ShowSkill(objdontactive, this.transform.position, Quaternion.identity);
                        // obj1.GetComponent<Hero3Atk>().CollisionType = 1;//Bay xuyên team địch
                        // obj1.GetComponent<Hero3Atk>().RatioStatus = 20;//Có 20% tỉ lệ choáng mỗi tướng địch
                        ComboNormalAtk = 0;
                    } else {
                        //ShowSkill(objdontactive, this.transform.position, Quaternion.identity);
                        //ComboNormalAtk++;
                        //Kiểm tra và khởi tạo object skill nếu tất cả các object đều active
                        if (CheckExistAndCreateEffectExtension (this.transform.position, Skill1, Quaternion.identity))
                            Skill1[Skill1.Count - 1].GetComponent<Hero3Atk> ().Hero = this; //Gán class cho object skill mới tạo
                        ComboNormalAtk++;
                    }
                    //}
                    break;
                case 1: //Skill1
                    ShowSkill (Skill2[0], this.transform.position, Quaternion.Euler (70f, 0, 0));
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Viết riêng cho combo normal atk của hero này, sản sinh ra liên tục 3 phi tiêu
        /// </summary>
        /// <returns></returns>
        private IEnumerator ComboAtk () {
            int count = 0;
            Begin:
                var obj1 = GetObjectDontActive (Skill1);
            if (obj1 != null)
                ShowSkill (obj1, this.transform.position, Quaternion.identity);
            count++;
            yield return new WaitForSeconds (.1f);
            if (count < 3)
                goto Begin;
        }
        //Va chạm
        public override void OnTriggerEnter2D (Collider2D col) {
            base.OnTriggerEnter2D (col);
        }
        public override void OnCollisionEnter2D (Collision2D col) {
            base.OnCollisionEnter2D (col);
        }
    }
}