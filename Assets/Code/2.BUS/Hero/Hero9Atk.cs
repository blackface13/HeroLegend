using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero9 {
    //Normal atk
    public class Hero9Atk : SkillCore {
        //public Hero9 Hero;
        private float SpeedWeaponFly = 20f; //Tốc độ bay
        private float SpeedWeaponRotate = -100f; //Tốc độ quay
        private Vector3 Vec;
        Vector3 Rot;
        private ParticleSystem EffectParticle;
        private List<GameObject> EffectExtension2;
        public bool Expired; //Kiểm tra xem đã va chạm với đối phương hay chưa (Để di chuyển object)
        #region Initialize 

        public override void Awake () {
            base.Awake ();
            EffectParticle = GetComponent<ParticleSystem> ();
            EffectExtension2 = new List<GameObject> ();
            EffectExtension2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "EffectReHP"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            EffectExtension2[0].SetActive (false);
        }
        //
        public override void Start () {
            base.Start ();
            // Status = status.Stun;//Hiệu ứng choáng
            // TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues; //Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 10; //Mặc định là 100% damage gây ra
            SkillType = 1; //Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject> ();
            SetupEffectExtension ("H9AtkHitEffect"); //Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer (int team) {
            base.ReSetupLayer (team);
            //transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);//Dành cho object cần đảo ngược
        }
        //Sau khi được active
        private void OnEnable () {
            Expired = false;
            EffectParticle.Play (true);
            GetComponent<Collider2D> ().enabled = true;
            Vec = gameObject.transform.position;
            Rot = gameObject.transform.localEulerAngles;
            Vec = gameObject.transform.position;
            CollisionType = 0; //Đưa skill về trạng thái mặc định
            RatioStatus = 0; //Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine (AutoHiden (5f, this.gameObject)); //Ẩn game object sau 1s nếu ko detect dc va chạm
        }
        /// <summary>
        /// Xử lý va chạm
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerEnter2D (Collider2D col) {
            //Va chạm với enemy -> gây dame
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1]))) {
                CheckExistAndCreateEffectExtension (transform.position, EffectExtension); //Hiển thị hiệu ứng trúng đòn lên đối phương
                if (CollisionType.Equals (0)) //Nếu kiểu va chạm rồi ẩn
                {
                    Expired = true;
                    try {
                        if (gameObject.activeSelf)
                            StartCoroutine (ParticleStop (gameObject, EffectParticle, 1f));
                    } catch { }
                }
            }
            //Va chạm với team mình -> hồi máu
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2]))) {
                // if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                //     Hide(this.gameObject);//Ẩn object sau khi va chạm 
                CheckExistAndCreateEffectExtension (col.transform.position, EffectExtension2); //Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }
        #endregion
        //Update
        private void Update () {
            if (!Module.PAUSEGAME && !Expired) //Khi chưa chạm đối phương, thì update tọa độ
            {
                Vec.x += (Hero.Team.Equals (0) ? SpeedWeaponFly : -SpeedWeaponFly) * Time.deltaTime;
                Rot.z += (Hero.Team.Equals (0) ? SpeedWeaponRotate : -SpeedWeaponRotate) * Time.deltaTime;
                gameObject.transform.localEulerAngles = Rot;
                gameObject.transform.position = Vec;
            }
        }
    }
}