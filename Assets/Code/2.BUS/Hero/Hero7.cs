using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero7 {
    public class Hero7 : HeroBase {
        //Initialize
        public override void Awake () {
            EffectSpaceY = 8f;//Khoảng cách hiệu ứng choáng, đóng băng...tới hero, set ở các hero to hơn kích thước mặc định
            base.Awake ();
            HType = HeroType.near; //Tướng cận chiến
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object phi tiêu
            Skill2 = new List<GameObject> (); //Skill
            Skill3 = null;
            SkillType = new int[3]; //Kiểu skill

            // Đưa vào scene
            for (int i = 0; i < 3; i++) {
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H7Atk" + (i + 1).ToString ()), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero7Atk> ().Hero = this;
                Skill1[i].SetActive (false);
            }
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H7Skill"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].transform.GetChild (0).transform.GetComponent<Hero7Skill> ().Hero = this;
            Skill2[0].SetActive (false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - skill 1
            SkillType[2] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam (GameObject obj) {
            base.RefreshTeam (obj);
            Skill2[0].transform.GetChild (0).transform.GetComponent<SkillCore> ().ReSetupLayer (Team); //Dành riêng cho skill của hero này, vì object va chạm là object con
        }
        //Update
        public override void Update () {
            base.Update ();
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    switch (ComboNormalAtk) {
                        case 0:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 2.7f : -2.7f), transform.position.y + 2.6f, Module.BASELAYER[2]), Quaternion.Euler (-53f, 60f, -36f));
                            ComboNormalAtk++;
                            break;
                        case 1:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 2.75f : -2.75f), transform.position.y + 3.2f, Module.BASELAYER[2]), Quaternion.Euler (0, 0, -52f));
                            ComboNormalAtk++;
                            break;
                        case 2:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 5.5f : -5.5f), transform.position.y + 0.8f, Module.BASELAYER[2]), Quaternion.identity);
                            ComboNormalAtk++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //Skill1
                    if (CheckExistAndCreateEffectExtension (new Vector3 (this.transform.position.x + (Team.Equals (0) ? 4.2f : -4.2f), this.transform.position.y + 1.7f, Module.BASELAYER[Team.Equals (0) ? 2 : 3]), Skill2, Quaternion.identity))
                        Skill2[Skill2.Count - 1].transform.GetChild (0).GetComponent<Hero7Skill> ().Hero = this; //Gán class cho object skill mới tạo
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