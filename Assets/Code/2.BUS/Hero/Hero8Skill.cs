using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using System.Collections;
namespace Controller.Hero8
{
    //Skill bắn rocket
    public class Hero8Skill : SkillCore
    {
        //public Hero8 Hero;
        private float SpeedWeaponFly = 60f;//Tốc độ bay của phi tiêu
        private float SpeedWeaponRotate = 1306f;//Tốc độ quay của phi tiêu
        private Vector3 Vec;
        Vector3 Rot;
        public Vector3 CurentPos;//Vị trí hiện tại của object
        public Vector3 TargetPos;//Vị trí mà phi tiêu sẽ bay tới
        public bool Expired;//Kiểm tra xem đã va chạm với đối phương hay chưa (Để di chuyển object)
        private int Count;
        private ParticleSystem EffectParticle;
        #region Initialize 

        public override void Awake()
        {
            base.Awake();
            EffectParticle = GetComponent<ParticleSystem>();
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[5];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H8Skill" + (i + 1).ToString ());
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
            DamagePercent = 250;//Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject>();
            SetupEffectExtension("H8SkillEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
            transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable()
        {
            //Thiết lập âm thanh bắn ra
            if (GameSystem.Settings.SoundEnable) {
                StartCoroutine (Battle.PlaySound (SoundClip[0], 0));
            }
            Expired = false;
            EffectParticle.Play(true);
            //TargetPos = Team.Equals(1) ? new Vector3(0 - Camera.main.aspect * 11f, CurentPos.y, CurentPos.z) : new Vector3(0 + Camera.main.aspect * 11f, CurentPos.y, CurentPos.z);//Set vị trí mà object sẽ di chuyển tới
            GetComponent<Collider2D>().enabled = true;
            Vec = gameObject.transform.position;
            //Rot = gameObject.transform.localEulerAngles;
            CollisionType = 0;//Đưa skill về trạng thái mặc định
            RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine(AutoHiden(5f, this.gameObject));//Ẩn game object nếu xuất hiện quá lâu
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D(Collider2D col)
        {
            try
            {
                if ((Hero.Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
                {
                    //Thiết lập âm thanh va chamj
                    if (GameSystem.Settings.SoundEnable)
                    {
                        var rand = UnityEngine.Random.Range(1, SoundClip.Length);
                        StartCoroutine(Battle.PlaySound(SoundClip[rand], 0));
                    }
                    CheckExistAndCreateEffectExtension(col.transform.position, EffectExtension);//Hiển thị hiệu ứng trúng đòn lên đối phương
                    if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                    {
                        Expired = true;
                        StartCoroutine(ParticleStop(gameObject, EffectParticle, .7f));
                    }
                    //Hide(this.gameObject);//Ẩn object sau khi va chạm 
                }
            }
            catch
            {

            }
        }

        #endregion
        //Update
        private void Update()
        {
            if (!Module.PAUSEGAME)
            {
                if (!Expired)//Khi chưa chạm đối phương, thì update tọa độ
                {
                    if (Hero.Team.Equals(0))
                    {
                        Vec.x += SpeedWeaponFly * Time.deltaTime;
                        //Rot.z += SpeedWeaponRotate * Time.deltaTime;
                    }
                    else
                    {
                        Vec.x -= SpeedWeaponFly * Time.deltaTime;
                        //Rot.z -= SpeedWeaponRotate * Time.deltaTime;
                    }
                    //gameObject.transform.localEulerAngles = Rot;
                    gameObject.transform.position = Vec;
                }
            }
        }
    }
}
