using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero5 {
    //Skill giống Q của Sivir
    public class Hero5Skill : SkillCore {
        //public Hero5 Hero;
        #region Initialize 
        int CountStart = 0;
        public override void Awake () {
            base.Awake ();
            TimeMove = .6f; //Định thời gian move của object này
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[1];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H5Skill" + (i + 1).ToString ());
            }
        }
        //
        public override void Start () {
            base.Start ();
            Status = status.Stun; //Hiệu ứng choáng
            TimeStatus = 1.5f; //Thời gian gây ra hiệu ứng
            RatioStatus = 100; //Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues; //Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 200; //Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject> ();
            SetupEffectExtension ("H5AtkHitEffect"); //Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer (int team) {
            base.ReSetupLayer (team);
            //transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable () {
            CountStart++;
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                if (CountStart > 1) //Fix vụ âm thanh sẽ chạy khi khởi tạo
                {
                    var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                    StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
                }
            }
            GetComponent<Collider2D> ().enabled = true;
            StartCoroutine (AutoDisCol (0.2f, gameObject));
            //Vec = gameObject.transform.position;
            CollisionType = 1; //Đưa skill về trạng thái mặc định
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            StartCoroutine (AutoHiden (1.5f, this.gameObject)); //Ẩn game object nếu xuất hiện quá lâu
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D (Collider2D col) {
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1]))) {
                if (CollisionType.Equals (0)) //Nếu kiểu va chạm rồi ẩn
                    Hide (this.gameObject); //Ẩn object sau khi va chạm 
                CheckExistAndCreateEffectExtension (col.transform.position, EffectExtension); //Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }

        #endregion
    }
}