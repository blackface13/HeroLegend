using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero1 {
    public class Hero1 : HeroBase {
        private List<GameObject> Skill1Extension1; //Tạo object mở rộng cho normal atk, phi tiêu lớn
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
            Skill1Extension1 = new List<GameObject> (); //Obj phi tiêu lớn
            Skill2 = null;
            Skill3 = null;
            SkillType = new int[3]; //Kiểu skill

            // Đưa vào scene
            for (int i = 0; i < 10; i++) {
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero1Skill1"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero1Skill1> ().Hero = this;
                Skill1[i].SetActive (false);
            }
            for (int i = 0; i < 10; i++) {
                Skill1Extension1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero1Skill1"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1Extension1[i].GetComponent<Hero1Skill1> ().Hero = this;
                Skill1Extension1[i].transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
                Skill1Extension1[i].SetActive (false);
            }
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - đánh thường
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
            var count = Skill1Extension1.Count;
            for (int i = 0; i < count; i++) //Set lại layer của skill mở rộng khi đổi team của nhân vật
            {
                Skill1Extension1[i].GetComponent<SkillCore> ().ReSetupLayer (Team);
            }
        }
        //Update
        public override void Update () {
            base.Update ();
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    try {
                        var objdontactive = ComboNormalAtk == 0 ? GetObjectDontActive (Skill1) : ComboNormalAtk == 1 ? GetObjectDontActive (Skill1Extension1) : GetObjectDontActive (Skill1); //Tìm object chưa dc kích hoạt
                        if (objdontactive != null) {
                            if (ComboNormalAtk == 2) //Nếu là đòn đánh thứ 3 của combo
                            {
                                var obj1 = GetObjectDontActive (Skill1);
                                var obj2 = GetObjectDontActive (Skill1Extension1);
                                obj1.GetComponent<SkillCore> ().DamagePercent = 100; //Đoạn này viết thêm để giảm dame cho đánh thường (do code hơi lỗi, ko sử dụng lại cách này)
                                obj2.GetComponent<SkillCore> ().DamagePercent = 100; //Đoạn này viết thêm để giảm dame cho đánh thường (do code hơi lỗi, ko sử dụng lại cách này)
                                ShowSkill (obj1, this.transform.position, Quaternion.identity);
                                ShowSkill (GetObjectDontActive (Skill1Extension1), this.transform.position, Quaternion.identity);
                                obj1.GetComponent<Hero1Skill1> ().CollisionType = 1; //Bay xuyên team địch
                                obj2.GetComponent<Hero1Skill1> ().CollisionType = 1;
                                obj2.GetComponent<Hero1Skill1> ().RatioStatus = 0; //Có 20% tỉ lệ hiệu ứng cho mỗi tướng địch
                                ComboNormalAtk = 0;
                            } else {
                                ShowSkill (objdontactive, this.transform.position, Quaternion.identity);
                                ComboNormalAtk++;
                            }
                        }
                    } catch {
                        ErrorCode.WriteErrorLog (0);
                        GameSystem.ControlFunctions.ShowMessage( (ErrorCode.Error[0]));
                    }
                    break;
                case 1: //Skill1
                    var temp = GetObjectDontActive (Skill1);
                    temp.GetComponent<SkillCore> ().DamagePercent = 150; //Đoạn này viết thêm để tăng dame cho skill (do code hơi lỗi, ko sử dụng lại cách này)
                    ShowSkill (temp, this.transform.position, Quaternion.identity);
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