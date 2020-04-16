using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero3 {
    //Normal atk
    public class Hero3Atk : SkillCore {
        //public Hero3 Hero;
        private float SpeedWeaponFly = 60f; //Tốc độ bay của phi tiêu
        private Vector3 Vec;
        #region Initialize 

        public override void Awake () {
            base.Awake ();
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[4];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H3Nor" + (i + 1).ToString ());
            }
        }
        //
        public override void Start () {
            base.Start ();
            Status = status.Stun; //Hiệu ứng choáng
            TimeStatus = 3f; //Thời gian gây ra hiệu ứng
            RatioStatus = 0; //Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues; //Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            //DamagePercent = 100;//Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject> ();
            SetupEffectExtension ("H3AtkHitEffect"); //Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer (int team) {
            base.ReSetupLayer (team);
            transform.localScale = Team.Equals (0) ? transform.localScale : new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable () {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
            }
            GetComponent<Collider2D> ().enabled = true;
            Vec = gameObject.transform.position;
            CollisionType = 0; //Đưa skill về trạng thái mặc định
            RatioStatus = 0; //Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine (AutoHiden (1f, this.gameObject)); //Ẩn game object sau 1s nếu ko detect dc va chạm
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D (Collider2D col) {
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1]))) {
                CheckExistAndCreateEffectExtension (col.transform.position, EffectExtension); //Hiển thị hiệu ứng trúng đòn lên đối phương
                if (CollisionType.Equals (0)) //Nếu kiểu va chạm rồi ẩn
                    Hide (this.gameObject); //Ẩn object sau khi va chạm 

            }
        }

        #endregion
        //Update
        private void Update () {
            if (!Module.PAUSEGAME) {
                if (Hero.Team.Equals (0)) {
                    Vec.x += SpeedWeaponFly * Time.deltaTime;
                } else {
                    Vec.x -= SpeedWeaponFly * Time.deltaTime;
                }
                gameObject.transform.position = Vec;
            }
        }
    }
}