using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero4 {
    public class Hero4 : HeroBase {
        private GameObject SkillExtension;
        //Initialize
        public override void Awake () {
            base.Awake ();
            //SpeedAnimationValue = 3f;
            HType = HeroType.near; //Tướng đánh xa
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
            StartCoroutine (RunIntrinsic ());
        }

        /// <summary>
        /// Chạy nội tại cho nhân vật
        /// Nội tại: Đòn đánh thường lên mục tiêu sẽ gây thêm sát thương vật lý bằng 10% lượng máu đối phương, hiệu ứng này sẽ không xuất hiện trong 5 giây
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunIntrinsic () {
            Begin : yield return new WaitUntil (() => !IntrinsicEnable);
            if (!IntrinsicEnable) {
                yield return new WaitForSeconds (5);
                IntrinsicEnable = true;
                goto Begin;
            }
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object đánh thường
            Skill2 = new List<GameObject> (); //Skill hướng phải, trái
            //Skill3 = new GameObject[1];//Skill hướng trái
            SkillType = new int[2]; //Kiểu skill

            // Đưa vào scene
            for (int i = 0; i < 3; i++) {
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H4Atk" + (i + 1).ToString ()), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero4Atk> ().Hero = this;
                Skill1[i].SetActive (false);
            }
            //Hiệu ứng chạm đất
            SkillExtension = Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H4SkillEffect"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            //Skill 
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H4Atk3"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].GetComponent<Hero4Atk> ().Hero = this;
            Skill2[0].SetActive (false);
            // Skill2[1] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "Hero2Skill1R"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            // Skill2[0].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[0].SetActive(false);
            // Skill2[1].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[1].SetActive(false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - skill 1
            //SkillType[2] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam (GameObject obj) {
            base.RefreshTeam (obj);
            SkillExtension.transform.localScale = Team.Equals (0) ? SkillExtension.transform.localScale : new Vector3 (-SkillExtension.transform.localScale.x, SkillExtension.transform.localScale.y, SkillExtension.transform.localScale.z);
        }
        //Update
        public override void Update () {
            base.Update ();
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    {
                        switch (ComboNormalAtk) {
                            case 0:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 1.3f, transform.position.y + 0.22f, Module.BASELAYER[2]), Quaternion.Euler (36f, 48f, -125.28f));
                                ComboNormalAtk++;
                                break;
                            case 1:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 0.4f, transform.position.y + 0f, Module.BASELAYER[2]), Quaternion.Euler (73.35f, 0f, 0f));
                                ComboNormalAtk++;
                                break;
                            case 2:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 1.63f, transform.position.y + 3.19f, Module.BASELAYER[2]), Quaternion.Euler (0f, 0f, -60f));
                                ComboNormalAtk++;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case 1: //Skill1
                    Skill2[0].GetComponent<SkillCore> ().DamagePercent = 200; //Đoạn này viết thêm để tăng dame cho skill (do code hơi lỗi, ko sử dụng lại cách này)
                    ShowSkill (Skill2[0], new Vector3 (transform.position.x + 1.63f, transform.position.y + 3.19f, Module.BASELAYER[2]), Quaternion.Euler (0f, 0f, -60f));
                    //ShowSkill(Team.Equals(1) ? Skill2[0] : Skill2[1], Team.Equals(1) ? new Vector3(transform.position.x - 8f, transform.position.y + 3.5f, Module.BASELAYER[2]) : new Vector3(transform.position.x + 8f, transform.position.y + 3.5f, Module.BASELAYER[2]), Quaternion.identity);
                    break;
                case 2: //Hiệu ứng tiếp đất
                    ShowSkill (SkillExtension, transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
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