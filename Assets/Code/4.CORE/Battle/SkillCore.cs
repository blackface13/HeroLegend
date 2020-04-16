using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.Collections;
using Assets.Code.Controller.SceneController;

namespace BlackCore
{
    //Class control tất cả các skill của các nhân vật
    public class SkillCore : MonoBehaviour
    {
        #region Variables 

        [Header("Draw Curve")]
        public AnimationCurve moveCurve;
        public bool PercentHealthAtkEnable = false;//Đòn đánh này có trừ % máu hay ko
        public float PercentHealthCreated = 0;//Số % máu gây ra cho đối phương
        public float TimeMove;//Thời gian move của các Object muốn định hướng di chuyển sẵn
        public HeroesProperties DataValues;//Các chỉ số của nhân vật sẽ được truyền vào khi khởi tạo skill
        public int DamagePercent = 100;//Số phần trăm damage gây ra so với damage gốc (default = 100)
        public int SkillType = 0;//Kiểu sát thương, vật lý hoặc phép (default = vật lý)
        /// <summary>
        /// Kiểu va chạm. 0 = chạm là ẩn, 1 = xuyên đối thủ
        /// </summary>
        public int CollisionType = 0;//Kiểu va chạm. 0 = chạm là ẩn, 1 = xuyên đối thủ
        public float TimeDelay;//Thời gian delay trước khi cho phép va chạm
        public BattleSystem Battle;

        public float TimeAction;//Thời gian va chạm được hoạt động trước khi bị vô hiệu hóa
        public int Team;//0: team trái, 1 team phải
        public float TimeStatus;//Thời gian của hiệu ứng, set trong hàm kế thừa
        public float PercentDamageFireBurn = 30;//Số % dame gây ra hiệu ứng thiêu đốt cho đối phương, mặc định 30%
        public float RatioStatus;//Tỉ lệ gây ra hiệu ứng, set trong hàm kế thừa
        public status Status;
        public enum status
        {
            Normal,//Bình thường
            Silent,//Câm lặng
            Stun,//Choáng
            Root,//Giữ chân
            Ice,//Đóng băng
            Static,//Tĩnh, không thể bị chọn làm mục tiêu
            Blind,//Mù
            Slow,//Làm chậm
        }
        public Effect Eff;
        public enum Effect
        {
            Normal,//Bình thường
            Fire,//Thiêu đốt
            Poison,//Trúng độc
        }
        public List<GameObject> EffectExtension;
        public HeroBase Hero;

    public AudioClip[] SoundClip;//Âm thanh của skill
    public AudioClip[] SoundClipHited;//Âm thanh của skill
        #endregion

        #region Initialize 

        public virtual void Awake()
        {
            Battle = GameObject.Find("SceneControl").GetComponent<BattleSystem>();
            Eff = Effect.Normal;//Set mặc định hiệu ứng
            Status = status.Normal;//Set mặc định trang thái mang theo
            //Khởi tạo âm thanh bị trúng đòn
            if(GameSystem.Settings.SoundEnable)
            {
            SoundClipHited = new AudioClip[5];
            for(int i = 0;i< SoundClipHited.Length;i++)
            SoundClipHited[i] = Resources.Load<AudioClip>("Audio/Skill/Hited" + (i+1).ToString());
            }
        }
        public virtual void Start()
        {

        }

        /// <summary>
        /// Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        /// </summary>
        public virtual void SetupEffectExtension(string prefabname)
        {
            EffectExtension.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + prefabname), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            EffectExtension[0].SetActive(false);
        }
        #endregion

        #region Core Functions 

        /// <summary>
        /// Check khởi tạo và hiển thị hiệu ứng trúng đòn lên đối phương
        /// </summary>
        /// <param name="col"></param>
        public void CheckExistAndCreateEffectExtension(Vector3 col, List<GameObject> objectExtension)
        {
            var a = GetObjectDontActive(objectExtension);
            if (a == null)
                objectExtension.Add(Instantiate(objectExtension[0], new Vector3(col.x, col.y, Module.BASELAYER[2]), Quaternion.identity));
            else
                ShowSkill(a, new Vector3(col.x, col.y, Module.BASELAYER[2]), Quaternion.identity);
        }

        /// <summary>
        /// Set lại layer của skill cho đúng với team bên nào
        /// </summary>
        public virtual void ReSetupLayer(int team)
        {
            Team = team;
            gameObject.layer = Team.Equals(0) ? 11 : 12;
        }

        /// <summary>
        /// Trả về object skill đang ko hoạt động để xuất hiện
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public GameObject GetObjectDontActive(List<GameObject> obj)
        {
            int count = obj.Count;
            for (int i = 0; i < count; i++)
            {
                if (!obj[i].activeSelf)
                    return obj[i];
            }
            return null;
        }

        //Hiện các object skill nhỏ nhơn như hiệu ứng hoặc các object con của skill
        public void ShowSkill(GameObject obj, Vector3 vec, Quaternion quater)
        {
            obj.transform.position = vec;
            obj.transform.rotation = quater;
            obj.SetActive(true);
        }

        /// <summary>
        /// Gây sát thương liên tục
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator LoopingAtk(float TimeRespawn, int MaxAtk)
        {
            var count = 0;
        Start:
            yield return new WaitForSeconds(TimeRespawn);
            if (count >= MaxAtk)
                goto End;
            else
            {
                count++;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Collider2D>().enabled = true;
                goto Start;
            }
        End:
            GetComponent<Collider2D>().enabled = false;
        }

        #region Ẩn hoặc tự động ẩn skill 

        //Tự động ẩn object skill sau 1 khoảng thời gian
        public virtual IEnumerator AutoHiden(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            Hide(obj);
        }

        /// <summary>
        /// Ẩn object skill ngay lập tức
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Hide(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]);
            obj.transform.localEulerAngles = new Vector3();
        }

        /// <summary>
        /// Dừng particle kèm theo move object không cho sản sinh thêm object và ẩn sau 1 khoảng time (Dành cho các hiệu ứng particle theo object)
        /// </summary>
        public virtual IEnumerator ParticleStop(GameObject obj, ParticleSystem parEffect, float time)
        {
            obj.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]);
            obj.transform.localEulerAngles = new Vector3();
            parEffect.Stop();
            yield return new WaitForSeconds(time);
            obj.SetActive(false);
        }
        #endregion

        #region Điều khiển va chạm 

        //Tự động vô hiệu hóa va chạm sau 1 khoảng thời gian
        public IEnumerator AutoDisCol(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            try
            {
                obj.GetComponent<Collider2D>().enabled = false;
            }
            catch { }
        }

        //Tự động cho phép va chạm sau 1 khoảng thời gian
        public IEnumerator AutoEnableCol(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            try
            {
                obj.GetComponent<Collider2D>().enabled = true;
            }
            catch { }
        }

        #endregion

        #endregion
    }
}
