/* Created: Bkack Face (bf.blackface@gmail.com)
 * Battle system
 * 2018/11/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class System_Battle : OverridableMonoBehaviour
{
    #region Variable
    BaseHeroes Heroes;
    public GameObject FPS_Text;
    public Text FPS_TextComponent, ComboTextComponent;
    public Gradient ComboGradienComponent;
    /// <summary>
    /// 0: Canvas để set parent cho Damage text - Setup ở interface
    /// 1: Combo text - Setup ở interface
    /// 2: Setting Menu - Setup ở interface
    /// 3: Empty
    /// 4: Empty
    /// 5: Group Show dame - Setup ở interface
    /// 6: Group Minimap - Setup ở interface
    /// </summary>
    public GameObject[] GObject;//Các object cần để điều khiển trong scene - Setup ở interface
    private Color32[] ClrComboText = new Color32[2];//Giá trị màu của top và bot Combotext, để làm mờ
    public GameObject[] HeroObject;//Khởi tạo Object ở CharacterControl
    public GameObject Player;//object player
    public GameObject Map;//object curent map
    public GameObject[] EnemyObject;//Mảng object enemy
    public GameObject[] Damamge;//Mảng object dame text
    public Text[] DamamgeComponentText;
    public enum PlayerPos { Left, Mid, Right };//Trạng thái của Player đang ở lề trái, giữa, hay lề phải của map
    public PlayerPos PPos;
    Color ComboTextColor;
    private GameObject MinimapPlayer;
    private GameObject[] MinimapEnemy;
    /// <summary>
    /// 0: Hệ số số lượng object Damage text được tạo (số object dame text = số lượng enemy * hệ số này)
    /// 1: Số combo
    /// </summary>
    public int[] IntNumber = new int[10];

    /// <summary>
    /// 0: set true khi đã hoàn tất khởi tạo Object player và enemy để update các object trong minimap
    /// 1: start or end shake
    /// 2: random day and night: true = day, false = night;
    /// </summary>
    public bool[] BolNumber = new bool[10];
    private Transform PlayerHPBar;
    private Transform PlayerMPBar;
    #endregion

    #region Functions
    protected override void Awake()
    {
        FPS_TextComponent = FPS_Text.GetComponent<Text>();
        StartCoroutine(SetupComboText());//Khởi tạo combotext
        IntNumber[0] = 15; //Hệ số số lượng object Damage text được tạo(số object dame text = số lượng enemy * hệ số này)
        int MapID = 2;//ID map, Thay đổi sau
        Map = Instantiate(Resources.Load<GameObject>("Prefabs/Map" + MapID.ToString()), new Vector3(0, 0, 0), Quaternion.identity);//Setup map
        StartCoroutine(SetupDayNight());//Khởi tạo random ngày đêm
        StartCoroutine(SetupEnemy());//Khởi tạo enemy
        StartCoroutine(SetupDamageText());//Khởi tạo damagetext
        //StartCoroutine(SetupMinimap());//Khởi tạo minimap
        m_vector_origin = Camera.main.transform.position;//Vector cho chức năng shake
        PPos = PlayerPos.Mid;//Setup vùng mặc định khi bắt đầu battle (cho heroes)
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Heroes = Player.GetComponent<BaseHeroes>();
        BolNumber[0] = true;
        PlayerHPBar = GObject[3].GetComponent<Transform>();
        PlayerMPBar = GObject[4].GetComponent<Transform>();
        //Player.GetComponent<BaseHeroes>().Prop[0].hp
    }

    #region Function Setup

    /// <summary>
    /// Setup random day night
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupDayNight()
    {
        //BolNumber[2] = UnityEngine.Random.Range(0, 100) >= Module.NightRandom ? true : false;//Tạo random ngày đêm
        BolNumber[2] = true;//Ngày
        for (int i = 7; i < 10; i++)
        {
            GObject[i].SetActive(!BolNumber[2]);
        }
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Setup Enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupEnemy()
    {
        EnemyObject = new GameObject[5];//Khởi tạo mảng object
        for (int i = 0; i < EnemyObject.Length; i++)
            EnemyObject[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy1"), new Vector3(UnityEngine.Random.Range(0, 20), Module.POSMAINHERO[1], Module.BASELAYER[6]), Quaternion.identity);

        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Khởi tạo các object cho minimap
    /// </summary>
    /// <param name="IdUser"></param>
    /// <param name="DataString"></param>
    /// <returns></returns>
    private IEnumerator SetupMinimap()
    {
        //----------Khởi tạo các object cho minimap----------
        //----------Enemy----------
        MinimapEnemy = new GameObject[EnemyObject.Length];
        for (int i = 0; i < MinimapEnemy.Length; i++)
        {
            MinimapEnemy[i] = Instantiate(Resources.Load<GameObject>("Prefabs/MinimapIconEnemy"), new Vector3(-3.6f, -8.9f, 70f), Quaternion.identity);
            MinimapEnemy[i].transform.SetParent(GObject[6].transform, false);//Set parent
            MinimapEnemy[i].transform.localScale = new Vector3(1, 1, 0);//Set lại kích thước
            MinimapEnemy[i].transform.position = new Vector3(-3.6f, -33f, 70f);
        }
        //----------Player----------
        MinimapPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/MinimapIconPlayer"), new Vector3(-3.6f, -8.9f, 70f), Quaternion.identity);
        MinimapPlayer.transform.SetParent(GObject[6].transform, false);//Set parent
        MinimapPlayer.transform.localScale = new Vector3(1, 1, 0);//Set lại kích thước
        MinimapPlayer.transform.position = new Vector3(-3.6f, -33f, 70f);
        //===================================================
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Setup combo text
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupComboText()
    {
        ClrComboText[0] = GObject[1].GetComponent<Gradient>().topColor;//Giá trị màu của top Combotext, để làm mờ
        ClrComboText[1] = GObject[1].GetComponent<Gradient>().bottomColor;//Giá trị màu của bot Combotext, để làm mờ
        ComboTextColor = GObject[1].GetComponent<Text>().color;
        ComboTextComponent = GObject[1].GetComponent<Text>();
        ComboGradienComponent = GObject[1].GetComponent<Gradient>();
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Setup damage text
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupDamageText()
    {
        Damamge = new GameObject[EnemyObject.Length * IntNumber[0]];//Khởi tạo Damage text cho battle scene
        DamamgeComponentText = new Text[Damamge.Length];
        for (int i = 0; i < Damamge.Length; i++)
        {
            Damamge[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Damage"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[9]), Quaternion.identity);
            DamamgeComponentText[i] = Damamge[i].GetComponent<Text>();
            Damamge[i].SetActive(false);
            Damamge[i].transform.SetParent(GObject[5].transform, false);//Set parent cho Damage text là Canvas để có thể hiển thị text
            Damamge[i].transform.localScale = new Vector3(0.3f, 0.3f, 0);//Set lại kích thước
        }
        yield return new WaitForSeconds(0);
    }
    #endregion

    #region Function Update
    /// <summary>
    /// Update minimap
    /// </summary>
    private void UpdateMinimap()
    {
        //if (BolNumber[0])//Nếu đã khởi tạo xong các object minimap
        //{
        //    MinimapPlayer.transform.position = new Vector3(Camera.main.transform.position.x - Module.RANGEMINIMAP.x + Player.transform.position.x / Module.RANGEMINIMAP.y, MinimapPlayer.transform.position.y, MinimapPlayer.transform.position.z);
        //    for (int i = 0; i < MinimapEnemy.Length; i++)
        //    {
        //        MinimapEnemy[i].transform.position = new Vector3(Camera.main.transform.position.x - Module.RANGEMINIMAP.x + EnemyObject[i].transform.position.x / Module.RANGEMINIMAP.y,
        //            MinimapEnemy[i].transform.position.y,
        //            MinimapEnemy[i].transform.position.z);

        //    }
        //}
    }

    /// <summary>
    /// Hiển thị số máu bị mất
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="text"></param>
    public void ShowDamage(Vector3 vec, string text, int damefrom)//damefrom (sát thương gây ra bởi): 0-Player, 1-Enemy
    {
        for (int i = 0; i < this.Damamge.Length; i++)
        {
            if (!this.Damamge[i].activeSelf)
            {
                Damamge[i].transform.position = new Vector3(vec.x, vec.y, Module.BASELAYER[9]);
                DamamgeComponentText[i].text = text.ToString();//Set dame text
                Damamge[i].SetActive(true);
                if (damefrom.Equals(0))//Sát thương gây ra bởi Player
                {
                    IntNumber[1]++;
                    if (!GObject[1].activeSelf)
                        GObject[1].SetActive(true);
                    ComboTextComponent.text = Module.COMBOTEXT + IntNumber[1].ToString();
                    ComboTextColor.a = 1f;
                    ClrComboText[0].a = 255;
                    ClrComboText[1].a = 255;
                }
                break;
            }
        }
    }

    /// <summary>
    /// Gây sát thương
    /// </summary>
    /// <param name="Hero"></param>
    /// <param name="Enemy"></param>
    /// <param name="vec">Vị trí để showdame</param>
    /// <param name="Dameper">Phần trăm dame</param>
    /// <param name="type">Kiểu dame, magic hay physic</param>
    /// <param name="damefrom">Bên nào tấn công</param>
    public void Damage(BaseHeroes Hero, BaseEnemys Enemy, Vector3 vec, int Dameper, int type, int damefrom)
    {
        if (damefrom.Equals(0) && Enemy.Prop[0].HP_curent > 0)//Sát thương từ hero và máu enemy > 0 (phải là chưa die)
        {
            float DameEnd = damefrom.Equals(0) ? Module.Damage(Hero.Prop[0].Atk_physic, Hero.Prop[0].Atk_magic, Enemy.Prop[0].Def_physic, Enemy.Prop[0].Def_magic, Hero.Prop[0].Pass_def_physic, Hero.Prop[0].Pass_def_magic, Dameper, type) : Module.Damage(Enemy.Prop[0].Atk_physic, Enemy.Prop[0].Atk_magic, Hero.Prop[0].Def_physic, Hero.Prop[0].Def_magic, Enemy.Prop[0].Pass_def_physic, Enemy.Prop[0].Pass_def_magic, Dameper, type);
            DameEnd = DameEnd / 10;//Test
            ShowDamage(vec, Convert.ToInt32(DameEnd).ToString(), damefrom);
            Enemy.Prop[0].HP_curent -= Enemy.Prop[0].HP_curent <= 0 ? 0 : DameEnd;
            Enemy.Prop[0].HP_curent = Enemy.Prop[0].HP_curent <= 0 ? 0 : Enemy.Prop[0].HP_curent;
        }
        if (damefrom.Equals(1) && Hero.Prop[0].HP_curent > 0)//Sát thương từ enemy
        {
            float DameEnd = damefrom.Equals(0) ? Module.Damage(Hero.Prop[0].Atk_physic, Hero.Prop[0].Atk_magic, Enemy.Prop[0].Def_physic, Enemy.Prop[0].Def_magic, Hero.Prop[0].Pass_def_physic, Hero.Prop[0].Pass_def_magic, Dameper, type) : Module.Damage(Enemy.Prop[0].Atk_physic, Enemy.Prop[0].Atk_magic, Hero.Prop[0].Def_physic, Hero.Prop[0].Def_magic, Enemy.Prop[0].Pass_def_physic, Enemy.Prop[0].Pass_def_magic, Dameper, type);
            ShowDamage(vec, Convert.ToInt32(DameEnd).ToString(), damefrom);
            Hero.Prop[0].HP_curent -= Hero.Prop[0].HP_curent <= 0 ? 0 : DameEnd;
            Hero.Prop[0].HP_curent = Hero.Prop[0].HP_curent <= 0 ? 0 : Hero.Prop[0].HP_curent;
        }
    }

    /// <summary>
    /// Update các thanh bar của player
    /// </summary>
    /// <param name="currentvalue">Giá trị hiện tại</param>
    /// <param name="maxvalue">Giá trị tối đa</param>
    /// <param name="typebar">Kiểu bar: 0-Máu, 1-Thể lực</param>
    private void UpdateBarPlayer()//(float currentvalue, float maxvalue, int typebar)
    {
        PlayerHPBar.localScale = new Vector3(((Heroes.Prop[0].HP_curent / Heroes.Prop[0].HP_total) * PlayerHPBar.localScale.y), PlayerHPBar.localScale.y, PlayerHPBar.localScale.z);
        PlayerMPBar.localScale = new Vector3(((Heroes.Prop[0].MP_curent / Heroes.Prop[0].MP_total) * PlayerMPBar.localScale.y), PlayerMPBar.localScale.y, PlayerMPBar.localScale.z);
        Heroes.Prop[0].MP_curent += Heroes.Prop[0].MP_curent >= Heroes.Prop[0].MP_total || Heroes.BolNumber[0] ? 0 : Module.ReMP * Time.deltaTime;
    }

    /// <summary>
    /// Combo text control update
    /// </summary>
    private void UpdateColorCombotext()
    {
        if (GObject[1].activeSelf)
        {
            ComboTextColor.a -= 0.004f;
            ComboTextComponent.color = ComboTextColor;
            ClrComboText[0].a -= (byte)(0.005f * 255);
            ClrComboText[1].a -= (byte)(0.005f * 255);
            ComboGradienComponent.topColor = ClrComboText[0];
            ComboGradienComponent.bottomColor = ClrComboText[1];
            if (ComboTextColor.a <= 0)
            {
                GObject[1].SetActive(false);
                IntNumber[1] = 0;
            }
        }
    }

    /// <summary>
    /// Update this scene
    /// </summary>
    void Update()
    {
        if (!Module.PAUSEGAME)
        {
            FPS_TextComponent.text = Module.ShowFPS() + " FPS" + " - " + Screen.width + ":" + Screen.height;
            if (BolNumber[1])//Cho phép update shake khi thực hiện rung
                Shaker_action();
            UpdateColorCombotext();
            UpdateBarPlayer();//Update các thanh bar của nhân vật
            if (BolNumber[0])//Update minimap khi nó dc enable
                UpdateMinimap();
        }
    }
    #endregion

    #region Shake effect (Hiệu ứng rung màn hình)
    public GameObject m_shakeAmountSlider;
    private float m_shakeIntensity;
    private float m_shakeDecay;
    private Vector3 m_vector_origin;
    private Vector3 m_originPosition;
    private Quaternion m_originRotation;
    private void Shaker_action()
    {
        if (m_shakeIntensity > 0)
        {
            Camera.main.transform.position = m_originPosition + UnityEngine.Random.insideUnitSphere * m_shakeIntensity;
            Camera.main.transform.rotation = new Quaternion(
                m_originRotation.x + UnityEngine.Random.Range(-m_shakeIntensity, m_shakeIntensity) * 0.1f,
                m_originRotation.y + UnityEngine.Random.Range(-m_shakeIntensity, m_shakeIntensity) * 0.1f,
                m_originRotation.z + UnityEngine.Random.Range(-m_shakeIntensity, m_shakeIntensity) * 0.1f,
                m_originRotation.w + UnityEngine.Random.Range(-m_shakeIntensity, m_shakeIntensity) * 0.1f
            );
            m_shakeIntensity -= m_shakeDecay;
            switch (PPos)
            {
                case PlayerPos.Mid:
                    Camera.main.transform.position = new Vector3(Player.transform.position.x, m_vector_origin.y, m_vector_origin.z);
                    break;
                default: break;
            }
        }
        else
        {
            switch (PPos)
            {
                case PlayerPos.Left:
                    Camera.main.transform.position = new Vector3(0, m_vector_origin.y, m_vector_origin.z);
                    break;
                case PlayerPos.Mid:
                    Camera.main.transform.position = new Vector3(Player.transform.position.x, m_vector_origin.y, m_vector_origin.z);
                    break;
                case PlayerPos.Right:
                    Camera.main.transform.position = new Vector3(Module.LIMITMAPMOVE[1], m_vector_origin.y, m_vector_origin.z);
                    break;
                default: break;
            }
            Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
            BolNumber[1] = false;//Kết thúc shaker
        }
    }

    /// <summary>
    /// Dành cho các script khác gọi tới
    /// </summary>
    /// <param name="values"></param>
    public void Shake(float values)
    {
        //BolNumber[1] = true;//Bắt đầu xử lý rung
        //m_originPosition = Camera.main.transform.position;
        //m_originRotation = Camera.main.transform.rotation;
        //m_vector_origin = cam.transform.position;
        //if (m_shakeIntensity <= 0)
        //{
        //    m_shakeIntensity = values;
        //    m_shakeDecay = 0.01f;
        //}
    }
    #endregion

    #region Button
    /// <summary>
    /// Button ẩn hiển menu setting
    /// </summary>
    public void ButtonSettingMenu()
    {
        if (!GObject[2].activeSelf)
        {
            GObject[2].SetActive(true);
            Time.timeScale = 0f;
            Module.PAUSEGAME = true;
        }
        else
        {
            GObject[2].SetActive(false);
            Time.timeScale = 1f;
            Module.PAUSEGAME = false;
        }
    }
    #endregion

    #endregion

}
