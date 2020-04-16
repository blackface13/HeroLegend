using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using System.Collections;
namespace Controller.Hero3
{
    //Skill giống Q của Sivir
    public class Hero3Skill : SkillCore
    {
        //public Hero3 Hero;
        private float SpeedWeaponFly = 60f;//Tốc độ bay của phi tiêu
        private float SpeedWeaponRotate = 1306f;//Tốc độ quay của phi tiêu
        private Vector3 Vec;
        Vector3 Rot;
        public Vector3 CurentPos;//Vị trí hiện tại của object
        public Vector3 TargetPos;//Vị trí mà phi tiêu sẽ bay tới
        public bool Expired;//Kiểm tra xem phi tiêu đã tới nơi hay chưa
        private int Count;
        #region Initialize 

        public override void Awake()
        {
            base.Awake();
            TimeMove = .6f;//Định thời gian move của object này
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[4];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H3Nor" + (i + 1).ToString ());
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
            DamagePercent = 135;//Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject>();
            SetupEffectExtension("H3AtkHitEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
            transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable()
        {
            Expired = false;
            CurentPos = gameObject.transform.position;//Set vị trí hiện tại khi enable để quay trở về
            TargetPos = Team.Equals(1) ? new Vector3(0 - Camera.main.aspect * 11f, CurentPos.y, CurentPos.z) : new Vector3(0 + Camera.main.aspect * 11f, CurentPos.y, CurentPos.z);//Set vị trí mà object sẽ di chuyển tới
            GetComponent<Collider2D>().enabled = true;
            //Vec = gameObject.transform.position;
            Rot = gameObject.transform.localEulerAngles;
            CollisionType = 1;//Đưa skill về trạng thái mặc định
            RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine(AutoHiden(5f, this.gameObject));//Ẩn game object nếu xuất hiện quá lâu
            Count++;
            //if (Count > 1)
                StartCoroutine(MoveObject(TargetPos, TimeMove));//Khởi động bay khi vừa xuất hiện
        if(GameSystem.Settings.SoundEnable)
            StartCoroutine(PlaySound());
        }

//Hàm chạy âm thành dành riêng cho skill này
        private IEnumerator PlaySound()
        {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.2f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.2f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.2f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
yield return new WaitForSeconds(.2f);
                rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
        }
        /// <summary>
        /// Move Object theo tọa độ và thời gian cho sẵn
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator MoveObject(Vector3 targetPos, float duration)
        {
            float time = 0;
            float rate = 1 / duration;
            Vector3 startPos = transform.localPosition;
            while (time < 1)
            {
                time += rate * Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(time));
                yield return 0;
            }
            transform.localPosition = targetPos;
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((Hero.Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
            {
                if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                    Hide(this.gameObject);//Ẩn object sau khi va chạm 
                CheckExistAndCreateEffectExtension(col.transform.position, EffectExtension);//Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }

        #endregion
        //Update
        private void Update()
        {
            if (!Module.PAUSEGAME)
            {
                if (Hero.Team.Equals(0))
                {
                    //Vec.x += SpeedWeaponFly * Time.deltaTime;
                    Rot.z += SpeedWeaponRotate * Time.deltaTime;
                }
                else
                {
                    //Vec.x -= SpeedWeaponFly * Time.deltaTime;
                    Rot.z -= SpeedWeaponRotate * Time.deltaTime;
                }
                gameObject.transform.localEulerAngles = Rot;
                if (!Expired)
                {
                    if (gameObject.transform.position == TargetPos)
                    {
                        GetComponent<Collider2D>().enabled = false;
                        StartCoroutine(MoveObject(CurentPos, TimeMove));
                        GetComponent<Collider2D>().enabled = true;
                        Expired = true;
                    }
                }
                if (Expired)
                {
                    if (gameObject.transform.position == CurentPos)
                    {
                        Expired = false;
                        Hide(gameObject);
                        gameObject.transform.localEulerAngles = new Vector3(70, 0, 0);
                    }
                }
                //gameObject.transform.position = Vec;
            }
        }
    }
}
