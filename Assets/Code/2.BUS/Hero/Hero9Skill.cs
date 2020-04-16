using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using System.Collections;
namespace Controller.Hero9
{
    //Skill hồi máu
    public class Hero9Skill : SkillCore
    {
        //public Hero9 Hero;
        private Vector3 Vec;
        private GameObject ParentObject;//Object giãn nở scale để va chạm (vì đây là hiệu ứng)
        #region Initialize 

        public override void Awake()
        {
            base.Awake();
           ParentObject = transform.parent.gameObject;
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[1];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H9Skill" + (i + 1).ToString ());
            }
        }
        //
        public override void Start()
        {
            base.Start();
            //Status = status.Stun;//Hiệu ứng choáng
            //TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            //RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues;//Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 60;//Mặc định là 100% damage gây ra
            SkillType = 1;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            // EffectExtension = new List<GameObject>();
            // SetupEffectExtension("H8SkillEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
            gameObject.layer = Team.Equals(0) ? Module.BASELAYERRIGID2D[7]: Module.BASELAYERRIGID2D[8];//set riêng cho skill này, chỉ tương tác với team mình
            //transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable()
        {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
            }
            //TargetPos = Team.Equals(1) ? new Vector3(0 - Camera.main.aspect * 11f, CurentPos.y, CurentPos.z) : new Vector3(0 + Camera.main.aspect * 11f, CurentPos.y, CurentPos.z);//Set vị trí mà object sẽ di chuyển tới
            GetComponent<Collider2D>().enabled = true;
            Vec = gameObject.transform.position;
            //Rot = gameObject.transform.localEulerAngles;
            CollisionType = 1;//Đưa skill về trạng thái mặc định
            RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine(AutoHiden(5f, ParentObject));//Ẩn game object nếu xuất hiện quá lâu
            StartCoroutine(LoopingAtk(.3f, 4));
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((Hero.Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
            {
                //CheckExistAndCreateEffectExtension(col.transform.position, EffectExtension);//Hiển thị hiệu ứng trúng đòn lên đối phương
                if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                    Hide(this.gameObject);//Ẩn object sau khi va chạm 
            }
        }

        #endregion
        //Update
        // private void Update()
        // {
        //     if (!Module.PAUSEGAME)
        //     {
        //             if (Hero.Team.Equals(0))
        //             {
        //                 Vec.x += SpeedWeaponFly * Time.deltaTime;
        //                 //Rot.z += SpeedWeaponRotate * Time.deltaTime;
        //             }
        //             else
        //             {
        //                 Vec.x -= SpeedWeaponFly * Time.deltaTime;
        //                 //Rot.z -= SpeedWeaponRotate * Time.deltaTime;
        //             }
        //             //gameObject.transform.localEulerAngles = Rot;
        //             gameObject.transform.position = Vec;
        //     }
        // }
    }
}
