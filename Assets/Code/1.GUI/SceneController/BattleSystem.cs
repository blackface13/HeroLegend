using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using BlackCore;
using StartApp;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Code.Controller.SceneController
{
    public class BattleSystem : MonoBehaviour
    {
        #region Variables 

        public GameObject MapObject;
        public Button[] ButtonSkillHero; //Set on Editor. Các button atk hay skill của nhân vật
        public GameObject[] Team1; //Team bên trái màn hình
        public GameObject[] Team2; //Team bên phải màn hình
        public GameObject PositionCenterCombat; //Set on Editor, object căn cho tọa độ camera vitual ăn theo
        private Image[] ItemUseImg; //Hình ảnh item đã được trang bị trước khi vào trận
        private bool[] IsUseItem = new bool[3]; //3 slot item
        public Image[] RespawnSkillHero1;
        public Image[] RespawnSkillHero2;
        public Image[] RespawnSkillHero3;
        public Image[] HPBarCircle;
        public Text[] TextTimeRespawnSkill;
        private float[] TimeRespawnTemp;
        public GameObject[] ObjectEffectReady;
        private HeroBase[] Hero;
        public List<GameObject> DamamgeObject; //Mảng object dame text
        public List<Damage> DamageScript; //Mảng script damage
        public List<Text> DamamgeComponentText;
        public List<Gradient> DamageGradient; //Mảng chứa màu của dametext
        public List<Gradient> GradientColor; //Mảng chứa màu của dametext
        public GameObject[] GObject;
        public GameObject[] HPBarTeam1; //Set on interface
        public GameObject[] HPBarTeam2; //Set on interface
        public RectTransform[] ImgHPTeam1; //Hình ảnh thể hiện % hp team 1, set on interface
        public RectTransform[] ImgHPTeam2; //Hình ảnh thể hiện % hp team 2, set on interface
        public Text[] TextPercentHPTeam1; //Text thể hiện số % hp team 1, set on interface
        public Text[] TextPercentHPTeam2; //Text thể hiện số % hp team 2, set on interface
        public GameObject[] GrpButtonSkill; //Nhóm button skill của team hero, set on interface
        private float[] CurentEnergy; //Thể lực hiện tại của cả team
        public RectTransform[] EneryBar; //Set on interface, thanh thể lực của team 
        public Image BackgroundSkill; //Nền đen xuất hiện khi nhân vật tung skill
        private Color ColorBackgroundSkill; //Màu nền đen khi nhân vật tung skill
        private bool EndCombat; //Xác định xem trận đấu kết thúc chưa
        private bool LockAction = false; //Khóa thao tác
        private bool IsWin; //Xác định xem thắng hay thua
        private float[] HPTeam; //Máu của 2 team, check xem chết hết chưa để dừng trận đấu
        private List<GameObject> ListBackgroundItemsReward; //Danh sách các item có thể nhận được
        private List<GameObject> ListItemsRewardImg; //Danh sách các item có thể nhận được
        private List<ItemDroped> ListItemsReward; //Danh sách các item có thể nhận được
        private List<int> ListItemsRewardQuantity; //Danh sách các item có thể nhận được
        public Text[] TextItemReward;
        public Image[] EndBattleAvtHero;
        //==========Sound==========
        public AudioSource Sound; //Control Âm thanh của skill
        public AudioSource BGM;
        //=========================

        InterstitialAd ADSInters;
        #endregion

        #region Initialize 

        private void Awake()
        {
            //Debug.Log(Camera.main.aspect);
            BattleCore.UserVitualCamera = true; //Cho phép sử dụng vitual camera hay ko
            Sound = GetComponent<AudioSource>();
            PositionCenterCombat.transform.position = Camera.main.transform.position;
            CurentEnergy = new float[2];
            ColorBackgroundSkill = BackgroundSkill.color; //Gán màu nền đen xuất hiện khi nhân vật tung skill
            Time.timeScale = BattleCore.BattleSpeed[GlobalVariables.SlotBattleSpeed]; //Tốc độ bắt đầu
            TextItemReward[9].text = "X" + (GlobalVariables.SlotBattleSpeed + 1).ToString();
            GObject[14].SetActive(DataUserController.User.IsAutoBattle); //Gán tự động đánh đã dc lưu vào data
            BattleCore.ResetIntrisic(); //Đặt lại các biến nội tại trước khi vào trận
            LoadMap();
            SetupTeam();
            SetupColorDamageText();
            SetupDamageText();
            SetupHPBar();
            SetupButtonSkill();
            SetupBattleUI();
        }
        private void Start()
        {
            #region Khởi tạo hoặc set Canvas thông báo cho Scene 

            try
            {
                GameSystem.MessageCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            }
            catch
            {
                GameSystem.Initialize(); //Khởi tạo này dành cho scene nào test ngay
                GameSystem.MessageCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
                GameSystem.MessageCanvas.GetComponent<Canvas>().planeDistance = 1;
            }

            #endregion
            //Âm nhạc
            GameSystem.StopBGM(); //Dừng nhạc nền 
            GameSystem.RunBGM(1);
            SetupItemUseEquiped();
            ItemDropController.Initialize(); //Khởi tạo item droped

            //Quảng cáo xen kẽ
            ADSInters = AdSdk.Instance.CreateInterstitial();
            ADSInters.RaiseAdLoaded += (sender, e) =>
            {
                //ADSInters.ShowAd("adTagIfNeeded");
            };
            ADSInters.LoadAd(InterstitialAd.AdType.Automatic);
        }

        /// <summary>
        /// Thay đổi vị trí nút bấm sang phải nếu được user setting 
        /// </summary>
        private void SetupBattleUI()
        {
            if (GameSystem.Settings.ButtonBattle)
            {
                var posxTemp = -GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition.x;
                GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(posxTemp, GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[0].GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                GrpButtonSkill[0].GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                GrpButtonSkill[1].GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                GrpButtonSkill[1].GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                GrpButtonSkill[2].GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                GrpButtonSkill[2].GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);

                posxTemp = -GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition.x;
                GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(posxTemp, GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition.y);
                GrpButtonSkill[3].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                GrpButtonSkill[3].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                GrpButtonSkill[4].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                GrpButtonSkill[4].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                GrpButtonSkill[5].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                GrpButtonSkill[5].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);

                // GObject[1].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-GrpButtonSkill[2].GetComponent<RectTransform> ().anchoredPosition.x, GObject[1].GetComponent<RectTransform> ().anchoredPosition.y);
                // GObject[1].GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0);
                // GObject[1].GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 0);
            }
        }

        /// <summary>
        /// Hoán đổi vị trí UI trái phải realtime khi setting trong battle
        /// </summary>
        private void SwapBattleUI()
        {
            var posxTemp = -GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition.x;
            GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[0].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[1].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(posxTemp, GrpButtonSkill[2].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[0].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);
            GrpButtonSkill[0].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);
            GrpButtonSkill[1].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);
            GrpButtonSkill[1].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);
            GrpButtonSkill[2].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);
            GrpButtonSkill[2].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 1 : 0, 0);

            posxTemp = -GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition.x;
            GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[3].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(-GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition.x, GrpButtonSkill[4].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(posxTemp, GrpButtonSkill[5].GetComponent<RectTransform>().anchoredPosition.y);
            GrpButtonSkill[3].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
            GrpButtonSkill[3].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
            GrpButtonSkill[4].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
            GrpButtonSkill[4].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
            GrpButtonSkill[5].GetComponent<RectTransform>().anchorMin = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
            GrpButtonSkill[5].GetComponent<RectTransform>().anchorMax = new Vector2(GameSystem.Settings.ButtonBattle ? 0 : 1, 0);
        }

        /// <summary>
        /// Hiển thị các item use đã được trang bị trước khi vào trận
        /// </summary>
        private void SetupItemUseEquiped()
        {
            ItemUseImg = new Image[3];
            //Object hình ảnh item trang bị trước khi vào trận
            ItemUseImg[0] = GrpButtonSkill[3].transform.GetChild(1).GetComponent<Image>();
            ItemUseImg[1] = GrpButtonSkill[4].transform.GetChild(1).GetComponent<Image>();
            ItemUseImg[2] = GrpButtonSkill[5].transform.GetChild(1).GetComponent<Image>();
            var itemUseEquip = DataUserController.User.ItemUseForBattle.Split(';'); //Lấy danh sách item use đã dc mang
            for (sbyte i = 0; i < itemUseEquip.Length; i++)
            {
                if (!string.IsNullOrEmpty(itemUseEquip[i])) //Nếu slot đang chọn đã trang bị item rồi
                {
                    var thisItem = DataUserController.Inventory.DBItems.Find(x => x.ItemType.ToString() == itemUseEquip[i].Split(',')[0] && x.ItemID.ToString() == itemUseEquip[i].Split(',')[1]);
                    if (thisItem != null)
                        ItemUseImg[i].sprite = Resources.Load<Sprite>("Images/Items/" + thisItem.ItemType + @"/" + thisItem.ItemID);
                    else
                    {
                        itemUseEquip[i] = "";
                        ItemUseImg[i].sprite = Resources.Load<Sprite>("Images/none");
                    }
                }
                else
                {
                    ItemUseImg[i].sprite = Resources.Load<Sprite>("Images/none");
                }
            }
        }

        /// <summary>
        /// Load map
        /// </summary>
        private void LoadMap()
        {
            var mapID = 1;
            switch (Module.WorldMapRegionSelected)
            {
                case 0: //Map rừng
                    mapID = UnityEngine.Random.Range(3, 11);
                    break;
                case 1: //Đồng bằng
                    mapID = UnityEngine.Random.Range(1, 12);
                    break;
                case 2: //Núi lửa
                    mapID = UnityEngine.Random.Range(1, 4);
                    break;
                case 3: //Núi tuyết
                    mapID = UnityEngine.Random.Range(1, 5);
                    break;
                case 4: //Địa ngục
                    mapID = UnityEngine.Random.Range(1, 6);
                    break;
                case 5: //Hang độc
                    mapID = UnityEngine.Random.Range(1, 7);
                    break;
                case 6: //Hang ma
                    mapID = UnityEngine.Random.Range(1, 7);
                    break;
                default:
                    break;
            }
            MapObject = Instantiate(Resources.Load<GameObject>(BattleCore.MapObjectLink + Module.WorldMapRegionSelected.ToString() + "/" + "Map" + mapID), new Vector3(0, 2f, 10f), Quaternion.identity);
            // if(Module.WorldMapRegionSelected.Equals(6))//Hiệu ứng nước
            // Camera.main.GetComponent<ScreenReflection>().enabled = true;
        }

        /// <summary>
        /// Khởi tạo tham số cho các biến
        /// </summary>
        private void SetupTeam()
        {
            DataUserController.LoadTeam();
            string[] temp = DataUserController.Team.Split(';');

            //Mỗi team có tối đa 3 hero
            Hero = new HeroBase[6]; //6 Hero của 2 team
            HPTeam = new float[6]; //Máu của 2 team, check xem chết hết chưa để dừng trận đấu
            Team1 = new GameObject[3];
            Team2 = new GameObject[3];
            Team1[0] = temp[0] != "0" ? Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + temp[0]), new Vector3(0 - Camera.main.aspect * 11f, BattleCore.DefaultVectorY, 0), Quaternion.identity) : null;
            Team1[1] = temp[1] != "0" ? Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + temp[1]), new Vector3(0 - Camera.main.aspect * 7f, BattleCore.DefaultVectorY, 0), Quaternion.identity) : null;
            Team1[2] = temp[2] != "0" ? Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + temp[2]), new Vector3(0 - Camera.main.aspect * 3f, BattleCore.DefaultVectorY, 0), Quaternion.identity) : null;
            for (int i = 0; i < 3; i++)
                if (Team1[i] != null)
                    Team1[i].GetComponent<HeroBase>().RefreshTeam(Team1[i]);
            string[] listEnemy = listEnemy = new string[] { "1500", "1003", "1002" }; //new string[3];
            if (Module.BattleModeSelected.Equals(0))
            { //Nếu là chế độ chơi preview
                listEnemy = DataUserController.User.EnemyFutureMap[Module.WorldMapRegionSelected].Split(';');
            }
            //Team2[0] = Instantiate (Resources.Load<GameObject> (BattleCore.HeroObjectLink + "Hero" + (Module.BattleModeSelected.Equals (0) ? listEnemy[0].ToString () : BattleCore.HeroIDLine3[UnityEngine.Random.Range (0, BattleCore.HeroIDLine3.Count)].ToString ())), new Vector3 (0 + Camera.main.aspect * 11f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[0] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + listEnemy[0].ToString()), new Vector3(0 + Camera.main.aspect * 11f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[0].GetComponent<HeroBase>().Team = 1;

            //Thêm item trang bị cho enemy
            Team2[0].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[0].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[0].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected] / 10f, 0, 0, 1));
            Team2[0].GetComponent<HeroBase>().RefreshTeam(Team2[0]);
            // //----------------------------
            //Team2[1] = Instantiate (Resources.Load<GameObject> (BattleCore.HeroObjectLink + "Hero" + (Module.BattleModeSelected.Equals (0) ? listEnemy[1].ToString () : BattleCore.HeroIDLine2[UnityEngine.Random.Range (0, BattleCore.HeroIDLine2.Count)].ToString ())), new Vector3 (0 + Camera.main.aspect * 7f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[1] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + listEnemy[1].ToString()), new Vector3(0 + Camera.main.aspect * 7f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[1].GetComponent<HeroBase>().Team = 1;

            // //Thêm item trang bị cho enemy
            Team2[1].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[1].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[1].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected] / 10f, 0, 0, 1));
            Team2[1].GetComponent<HeroBase>().RefreshTeam(Team2[1]);
            // //----------------------------
            //Team2[2] = Instantiate (Resources.Load<GameObject> (BattleCore.HeroObjectLink + "Hero" + (Module.BattleModeSelected.Equals (0) ? listEnemy[2].ToString () : BattleCore.HeroIDLine1[UnityEngine.Random.Range (0, BattleCore.HeroIDLine1.Count)].ToString ())), new Vector3 (0 + Camera.main.aspect * 3f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[2] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroObjectLink + "Hero" + listEnemy[2].ToString()), new Vector3(0 + Camera.main.aspect * 3f, BattleCore.DefaultVectorY, 0), Quaternion.identity);
            Team2[2].GetComponent<HeroBase>().Team = 1;

            // //Thêm item trang bị cho enemy
            Team2[2].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[2].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected]/10f, 0, 0, 1));
            Team2[2].GetComponent<HeroBase>().ItemsEnemy.Add(ItemSystem.CreateItem(true, true, DataUserController.User.LevelMap[Module.WorldMapRegionSelected] / 10f, 0, 0, 1));
            Team2[2].GetComponent<HeroBase>().RefreshTeam(Team2[2]);
            //----------------------------
            //Team 1
            Hero[0] = Team1[0] != null ? Team1[0].GetComponent<HeroBase>() : null;
            Hero[1] = Team1[1] != null ? Team1[1].GetComponent<HeroBase>() : null;
            Hero[2] = Team1[2] != null ? Team1[2].GetComponent<HeroBase>() : null;
            //Gán giá trị hp hiện tại để check
            HPTeam[0] = Team1[0] != null ? Team1[0].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;
            HPTeam[1] = Team1[1] != null ? Team1[1].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;
            HPTeam[2] = Team1[2] != null ? Team1[2].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;

            //Team 2
            Hero[3] = Team2[0] != null ? Team2[0].GetComponent<HeroBase>() : null;
            Hero[4] = Team2[1] != null ? Team2[1].GetComponent<HeroBase>() : null;
            Hero[5] = Team2[2] != null ? Team2[2].GetComponent<HeroBase>() : null;
            //Gán giá trị hp hiện tại để check
            HPTeam[3] = Team2[0] != null ? Team2[0].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;
            HPTeam[4] = Team2[1] != null ? Team2[1].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;
            HPTeam[5] = Team2[2] != null ? Team2[2].GetComponent<HeroBase>().DataValues.vHealthCurrent : 0;

            BattleCore.SpeedRespawnEnergyTeam1 = (Team1[0] != null ? Team1[0].GetComponent<HeroBase>().DataValues.vManaRegen : 0) +
                (Team1[1] != null ? Team1[1].GetComponent<HeroBase>().DataValues.vManaRegen : 0) +
                (Team1[2] != null ? Team1[2].GetComponent<HeroBase>().DataValues.vManaRegen : 0);
            BattleCore.SpeedRespawnEnergyTeam2 = (Team2[0] != null ? Team2[0].GetComponent<HeroBase>().DataValues.vManaRegen : 0) +
                (Team2[1] != null ? Team2[1].GetComponent<HeroBase>().DataValues.vManaRegen : 0) +
                (Team2[2] != null ? Team2[2].GetComponent<HeroBase>().DataValues.vManaRegen : 0);
            //Tổng mana team 1
            BattleCore.MaxEnergyTeam1 = (Team1[0] != null ? Team1[0].GetComponent<HeroBase>().DataValues.vMana : 0) +
                (Team1[1] != null ? Team1[1].GetComponent<HeroBase>().DataValues.vMana : 0) +
                (Team1[2] != null ? Team1[2].GetComponent<HeroBase>().DataValues.vMana : 0);
            CurentEnergy[0] = BattleCore.MaxEnergyTeam1; //Set thể lực hiện tại = thể lực max khi vào trận
            //Tổng mana team 2
            BattleCore.MaxEnergyTeam2 = (Team2[0] != null ? Team2[0].GetComponent<HeroBase>().DataValues.vMana : 0) +
                (Team2[1] != null ? Team2[1].GetComponent<HeroBase>().DataValues.vMana : 0) +
                (Team2[2] != null ? Team2[2].GetComponent<HeroBase>().DataValues.vMana : 0);
            CurentEnergy[1] = BattleCore.MaxEnergyTeam2;
            //gán thời gian hồi chiêu tạm thời để hiển thị số giây còn lại
            TimeRespawnTemp = new float[3];
            TimeRespawnTemp[0] = Team1[0] != null ? Team1[0].GetComponent<HeroBase>().DataValues.vCooldown[1] : 0;
            TimeRespawnTemp[1] = Team1[1] != null ? Team1[1].GetComponent<HeroBase>().DataValues.vCooldown[1] : 0;
            TimeRespawnTemp[2] = Team1[2] != null ? Team1[2].GetComponent<HeroBase>().DataValues.vCooldown[1] : 0;
        }

        /// <summary>
        /// Khởi tạo màu sắc cho chỉ số sát thương gây ra
        /// </summary>
        private void SetupColorDamageText()
        {
            //Gradient fff = new Gradient () { topColor = new Color32 (255, 191, 24, 255), bottomColor = new Color32 (255, 242, 214, 255) };
            GradientColor = new List<Gradient>();
            GradientColor.Add(new Gradient() { topColor = new Color32(255, 191, 24, 255), bottomColor = new Color32(255, 242, 214, 255) }); //Màu cam - dmg vật lý
            GradientColor.Add(new Gradient() { topColor = new Color32(0, 102, 192, 255), bottomColor = new Color32(141, 184, 255, 255) }); //màu xanh dương - dmg phép
            GradientColor.Add(new Gradient() { topColor = new Color32(23, 192, 0, 255), bottomColor = new Color32(158, 255, 141, 255) }); //màu xanh lá - hồi máu
            GradientColor.Add(new Gradient() { topColor = new Color32(140, 0, 192, 255), bottomColor = new Color32(229, 141, 255, 255) }); //màu tím - phản xạ từ đối thủ
        }
        /// <summary>
        /// Khởi tạo các object hiển thị damage lên màn hình
        /// </summary>
        /// <returns></returns>
        private void SetupDamageText()
        {
            DamamgeObject.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Damage"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[9]), Quaternion.identity));
            DamageScript.Add(DamamgeObject[0].GetComponent<Damage>());
            DamamgeComponentText.Add(DamamgeObject[DamamgeObject.Count - 1].GetComponent<Text>());
            DamageGradient.Add(DamamgeObject[DamamgeObject.Count - 1].GetComponent<Gradient>());
            DamamgeObject[0].SetActive(false);
            DamamgeObject[0].transform.SetParent(GObject[0].transform, false); //Set parent cho Damage text là Canvas để có thể hiển thị text
        }
        //

        /// <summary>
        /// Khởi tạo HP bar
        /// </summary>
        private void SetupHPBar()
        {
            HPBarCircle = new Image[3];
            for (int i = 0; i < HPBarTeam1.Length; i++)
            {
                //Ẩn hoặc hiện HP bar khi tồn tại nhân vật trong team 1
                HPBarTeam1[i].SetActive(Team1[i] == null ? false : true);
                //Ẩn hoặc hiện HP bar khi tồn tại nhân vật trong team 2
                HPBarTeam2[i].SetActive(Team2[i] == null ? false : true);
                //Set avatar team 1
                HPBarTeam1[i].transform.GetChild(1).GetComponent<Image>().sprite = Team1[i] == null ? null : Resources.Load<Sprite>("HeroAvt/" + Team1[i].GetComponent<HeroBase>().HeroID);
                //Set avatar team 2
                HPBarTeam2[i].transform.GetChild(1).GetComponent<Image>().sprite = Team2[i] == null ? null : Resources.Load<Sprite>("HeroAvt/" + Team2[i].GetComponent<HeroBase>().HeroID);

                HPBarCircle[i] = GrpButtonSkill[i].transform.GetChild(0).GetComponent<Image>();
            }
        }

        /// <summary>
        /// Khởi tạo cài đặt button skill cho team của player
        /// </summary>
        private void SetupButtonSkill()
        {
            for (int i = 0; i < 3; i++)
            {
                GrpButtonSkill[i].SetActive(Team1[i] == null ? false : true);
                //GrpButtonSkill[i + 3].SetActive (Team1[i] == null ? false : true);
                if (GrpButtonSkill[i].activeSelf)
                {
                    var temp = i;
                    GrpButtonSkill[i].transform.GetChild(2).transform.GetComponent<Button>().onClick.AddListener(() => RunSkill(temp, 1)); //Get button normal atk và add listener
                    //GrpButtonSkill[i + 3].transform.GetChild (1).transform.GetComponent<Button> ().onClick.AddListener (() => RunSkill (temp, 1)); //Get button skill 1 và add listener
                    //GrpButtonSkill[i].transform.GetChild(4).transform.GetChild(1).transform.GetComponent<Button>().onClick.AddListener(() => RunSkill(temp, 2));//Get button skill 2 và add listener
                    GrpButtonSkill[i].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroAvt/" + Hero[i].HeroID); //Normal atk - hình ảnh mặt nhân vật
                    // GrpButtonSkill[i + 3].transform.GetChild (1).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("IconSkill/" + Hero[i].HeroID + "-1"); //Skill 1
                    //GrpButtonSkill[i].transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("IconSkill/" + Hero[i].HeroID + "-2");//Skill 2
                }
            }
        }

        #endregion

        #region Functions 

        /// <summary>
        /// Button tạm dừng trận đấu
        /// </summary>
        public void ButtonPauseBattle()
        {
            GameSystem.ShowConfirmDialog(Languages.lang[179]);
            StartCoroutine(ActionPauseBattle());
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Chờ lệnh thoát khỏi trận đấu giữa chừng
        /// </summary>
        /// <returns></returns>
        private IEnumerator ActionPauseBattle()
        {
            yield return new WaitUntil(() => GameSystem.ConfirmBoxResult != 0);
            //Accept
            if (GameSystem.ConfirmBoxResult == 1)
            {
                Time.timeScale = 1f;
                GameSystem.StopBGM(); //Dừng nhạc nền 
                DataUserController.User.BattleLose += 1; //Thoát game giữa chừng sẽ + 1 trận thua
                //BattleCore.ChangeDifficult(0, Module.WorldMapRegionSelected);
                DataUserController.SaveUserInfor();
                var sceneload = new SceneLoad();
                sceneload.Change_scene("Room");
            }
            else
            {
                Time.timeScale = BattleCore.BattleSpeed[GlobalVariables.SlotBattleSpeed];
            }
        }

        /// <summary>
        /// Xuất hiện nền tối để nổi bật skill
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeInBackroundSkill()
        {
        Begin: ColorBackgroundSkill.a += .05f;
            BackgroundSkill.color = ColorBackgroundSkill;
            yield return new WaitForSeconds(.01f);
            if (ColorBackgroundSkill.a >= 0.75f)
            {
                ColorBackgroundSkill.a = 0.75f;
                BackgroundSkill.color = ColorBackgroundSkill;
                goto End;
            }
            else goto Begin;
            End: yield return null;
        }

        /// <summary>
        /// Ẩn nền tối skill đi
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeOutBackroundSkill()
        {
        Begin: ColorBackgroundSkill.a -= .01f;
            BackgroundSkill.color = ColorBackgroundSkill;
            yield return new WaitForSeconds(.01f);
            if (ColorBackgroundSkill.a <= 0f)
                goto End;
            else goto Begin;
            End: yield return null;
        }

        /// <summary>
        /// Điều khiển thời gian hồi chiêu và hồi thể lực của team
        /// </summary>
        private void RespawnController()
        {
            #region Hồi chiêu của hero
            #region Hero 1 
            if (Hero[0] != null)
            {
                //Normal atk
                if (RespawnSkillHero1[0].fillAmount <= 0f)
                {
                    RespawnSkillHero1[0].fillAmount = 0;
                    TextTimeRespawnSkill[0].text = "";
                    ObjectEffectReady[0].SetActive(true);
                }
                else
                {
                    RespawnSkillHero1[0].fillAmount -= 1000f / (Hero[0].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;
                    TimeRespawnTemp[0] -= Time.deltaTime;
                    TextTimeRespawnSkill[0].text = TimeRespawnTemp[0] < 1.0f ? String.Format("{0:0.0}", TimeRespawnTemp[0]) : String.Format("{0:0}", TimeRespawnTemp[0]);
                }

                //Skill 1
                // if (RespawnSkillHero1[1].fillAmount >= 1f)
                //     RespawnSkillHero1[1].fillAmount = 1f;
                // else
                //     RespawnSkillHero1[1].fillAmount += 1000f / (Hero[0].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;

                // //Skill 2
                // if (RespawnSkillHero1[2].fillAmount >= 1f)
                //     RespawnSkillHero1[2].fillAmount = 1f;
                // else
                //     RespawnSkillHero1[2].fillAmount += 1000f / (Hero[0].DataValues.Time_respawn_skill[2] * 1000f) * Time.deltaTime;
            }
            #endregion
            #region Hero 2 
            if (Hero[1] != null)
            {
                //Normal atk
                if (RespawnSkillHero2[0].fillAmount <= 0f)
                {
                    RespawnSkillHero2[0].fillAmount = 0;
                    TextTimeRespawnSkill[1].text = "";
                    ObjectEffectReady[1].SetActive(true);
                }
                else
                {
                    RespawnSkillHero2[0].fillAmount -= 1000f / (Hero[1].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;
                    TimeRespawnTemp[1] -= Time.deltaTime;
                    TextTimeRespawnSkill[1].text = TimeRespawnTemp[1] < 1.0f ? String.Format("{0:0.0}", TimeRespawnTemp[1]) : String.Format("{0:0}", TimeRespawnTemp[1]);
                }

                //Skill 1
                // if (RespawnSkillHero2[1].fillAmount >= 1f)
                //     RespawnSkillHero2[1].fillAmount = 1f;
                // else
                //     RespawnSkillHero2[1].fillAmount += 1000f / (Hero[1].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;

                // //Skill 2
                // if (RespawnSkillHero2[2].fillAmount >= 1f)
                //     RespawnSkillHero2[2].fillAmount = 1f;
                // else
                //     RespawnSkillHero2[2].fillAmount += 1000f / (Hero[1].DataValues.Time_respawn_skill[2] * 1000f) * Time.deltaTime;
            }
            #endregion
            #region Hero 3 
            if (Hero[2] != null)
            {
                //Normal atk
                if (RespawnSkillHero3[0].fillAmount <= 0f)
                {
                    RespawnSkillHero3[0].fillAmount = 0;
                    TextTimeRespawnSkill[2].text = "";
                    ObjectEffectReady[2].SetActive(true);
                }
                else
                {
                    RespawnSkillHero3[0].fillAmount -= 1000f / (Hero[2].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;
                    TimeRespawnTemp[2] -= Time.deltaTime;
                    TextTimeRespawnSkill[2].text = TimeRespawnTemp[2] < 1.0f ? String.Format("{0:0.0}", TimeRespawnTemp[2]) : String.Format("{0:0}", TimeRespawnTemp[2]);
                }

                //Skill 1
                // if (RespawnSkillHero3[1].fillAmount >= 1f)
                //     RespawnSkillHero3[1].fillAmount = 1f;
                // else
                //     RespawnSkillHero3[1].fillAmount += 1000f / (Hero[2].DataValues.vCooldown[1] * 1000f) * Time.deltaTime;

                // //Skill 2
                // if (RespawnSkillHero3[2].fillAmount >= 1f)
                //     RespawnSkillHero3[2].fillAmount = 1f;
                // else
                //     RespawnSkillHero3[2].fillAmount += 1000f / (Hero[2].DataValues.Time_respawn_skill[2] * 1000f) * Time.deltaTime;
            }
            #endregion
            #endregion
            #region Hồi thể lực 
            //Thanh thể lực team player
            if (CurentEnergy[0] >= BattleCore.MaxEnergyTeam1)
                CurentEnergy[0] = BattleCore.MaxEnergyTeam1;
            if (CurentEnergy[1] >= BattleCore.MaxEnergyTeam2)
                CurentEnergy[1] = BattleCore.MaxEnergyTeam2;

            CurentEnergy[0] += CurentEnergy[0] < BattleCore.MaxEnergyTeam1 ? BattleCore.SpeedRespawnEnergyTeam1 * Time.deltaTime : 0;
            CurentEnergy[1] += CurentEnergy[1] < BattleCore.MaxEnergyTeam2 ? BattleCore.SpeedRespawnEnergyTeam2 * Time.deltaTime : 0;

            //Thanh thể lực team 1
            EneryBar[0].localScale = new Vector3((CurentEnergy[0] / BattleCore.MaxEnergyTeam1) * 100f / 100f, 1f, 1f); //Set độ ngắn dài của thanh thể lực theo lượng energy real time
            //Thanh thể lực team 2
            EneryBar[1].localScale = new Vector3((CurentEnergy[1] / BattleCore.MaxEnergyTeam2) * 100f / 100f, 1f, 1f); //Set độ ngắn dài của thanh thể lực theo lượng energy real time

            #endregion
        }
        /// <summary>
        /// Hiển thị lượng máu các hero theo số lượng còn lại
        /// </summary>
        private void HPController()
        {
            for (int i = 0; i < BattleCore.MaxHeroInTeam; i++)
            {
                //Team 1
                TextPercentHPTeam1[i].text = Hero[i] != null ? Convert.ToInt32(((Hero[i].DataValues.vHealthCurrent / Hero[i].DataValues.vHealth) * 100f)).ToString() + " %" : "";
                ImgHPTeam1[i].localScale = Hero[i] != null ? new Vector3((Hero[i].DataValues.vHealthCurrent / Hero[i].DataValues.vHealth) * 100f / 100f, 1f, 1f) : Vector3.zero;
                HPBarCircle[i].fillAmount = ImgHPTeam1[i].localScale.x; //HP vòng tròn tại nút bấm skill
                //Team 2
                TextPercentHPTeam2[i].text = Hero[i + 3] != null ? Convert.ToInt32(((Hero[i + 3].DataValues.vHealthCurrent / Hero[i + 3].DataValues.vHealth) * 100f)).ToString() + " %" : "";
                ImgHPTeam2[i].localScale = Hero[i + 3] != null ? new Vector3((Hero[i + 3].DataValues.vHealthCurrent / Hero[i + 3].DataValues.vHealth) * 100f / 100f, 1f, 1f) : Vector3.zero;
            }
        }

        /// <summary>
        /// Điều khiển team 2
        /// </summary>
        private IEnumerator Team2Controller()
        {
        Begin: yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 3f));
            //if (CurentEnergy[1] > BattleCore.MaxEnergy / 3)
            {
                if (Team2[0] != null)
                    if (ValidSkill(100, 1))
                    {
                        if (UnityEngine.Random.Range(0, 100) < 50)
                        {
                            Hero[3].SkillListAciton.Add(0);
                            Hero[3].SkillListAciton.Add(0);
                            Hero[3].SkillListAciton.Add(0);
                        }
                        else
                            Hero[3].SkillListAciton.Add(1);
                    }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
                if (Team2[1] != null)
                    if (ValidSkill(100, 1))
                    {
                        if (UnityEngine.Random.Range(0, 100) < 80)
                        {
                            Hero[4].SkillListAciton.Add(0);
                            Hero[4].SkillListAciton.Add(0);
                            Hero[4].SkillListAciton.Add(0);
                        }
                        else
                            Hero[4].SkillListAciton.Add(1);
                    }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
                if (Team2[2] != null)
                    if (ValidSkill(100, 1))
                    {
                        if (UnityEngine.Random.Range(0, 100) < 80)
                        {
                            Hero[5].SkillListAciton.Add(0);
                            Hero[5].SkillListAciton.Add(0);
                            Hero[5].SkillListAciton.Add(0);
                        }
                        else
                            Hero[5].SkillListAciton.Add(1);
                    }
            }
            goto Begin;
        }
        /// <summary>
        /// Kiểm tra skill có đủ thể lực để thực hiện hay ko
        /// </summary>
        /// <param name="eneryskill"></param>
        /// <returns></returns>
        private bool ValidSkill(float energyskill, int team)
        {
            switch (team)
            {
                case 0: //Team 1
                    if (CurentEnergy[0] >= energyskill)
                    {
                        CurentEnergy[0] -= energyskill;
                        return true;
                    }
                    break;
                case 1: //Team 2
                    if (CurentEnergy[1] >= energyskill)
                    {
                        CurentEnergy[1] -= energyskill;
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        /// <summary>
        /// Hàm atk khi người chơi thực hiện thao tác bấm skill
        /// </summary>
        /// <param name="skillnumber"></param>
        private void RunSkill(int heroslot, int skillnumber)
        {
            if (!Hero[heroslot].SilentStatus) //Nếu nhân vật ko bị câm lặng, thì mới cho bấm skill
            {
                switch (skillnumber)
                {
                    case 0: //Normal atk (Không làm hồi chiêu cho normal atk nữa nên disable lại)
                        // //Hero 1
                        // if (RespawnSkillHero1[0].fillAmount >= 0.333f) {
                        //     if (heroslot.Equals (0) && ValidSkill (Hero[heroslot].DataValues.Mana_skill[skillnumber], 0)) {
                        //         Hero[heroslot].SkillListAciton.Add (skillnumber);
                        //         RespawnSkillHero1[0].fillAmount -= 0.333f;
                        //     }
                        // }
                        // //Hero 1
                        // if (RespawnSkillHero2[0].fillAmount >= 0.333f) {
                        //     if (heroslot.Equals (1) && ValidSkill (Hero[heroslot].DataValues.Mana_skill[skillnumber], 0)) {
                        //         Hero[heroslot].SkillListAciton.Add (skillnumber);
                        //         RespawnSkillHero2[0].fillAmount -= 0.333f;
                        //     }
                        // }
                        // //Hero 1
                        // if (RespawnSkillHero3[0].fillAmount >= 0.333f) {
                        //     if (heroslot.Equals (2) && ValidSkill (Hero[heroslot].DataValues.Mana_skill[skillnumber], 0)) {
                        //         Hero[heroslot].SkillListAciton.Add (skillnumber);
                        //         RespawnSkillHero3[0].fillAmount -= 0.333f;
                        //     }
                        // }
                        break;
                    case 1: //Skill 1
                        //Hero 1
                        if (RespawnSkillHero1[0].fillAmount <= 0)
                        {
                            if (heroslot.Equals(0) && ValidSkill(Hero[heroslot].DataValues.Mana_skill[skillnumber], 0))
                            {
                                Hero[heroslot].AddSkillToListAction(skillnumber);
                                RespawnSkillHero1[0].fillAmount += 1f;
                                ObjectEffectReady[0].SetActive(false);
                                GrpButtonSkill[0].transform.GetChild(6).gameObject.SetActive(true);
                                GrpButtonSkill[0].transform.GetChild(6).gameObject.transform.localPosition = Vector3.zero;
                                //gán thời gian hồi chiêu tạm thời để hiển thị số giây còn lại
                                TimeRespawnTemp[0] = Team1[0].GetComponent<HeroBase>().DataValues.vCooldown[1];
                            }
                        }
                        //Hero 2
                        if (RespawnSkillHero2[0].fillAmount <= 0)
                        {
                            if (heroslot.Equals(1) && ValidSkill(Hero[heroslot].DataValues.Mana_skill[skillnumber], 0))
                            {
                                Hero[heroslot].AddSkillToListAction(skillnumber);
                                RespawnSkillHero2[0].fillAmount += 1f;
                                ObjectEffectReady[1].SetActive(false);
                                GrpButtonSkill[1].transform.GetChild(6).gameObject.SetActive(true);
                                GrpButtonSkill[1].transform.GetChild(6).gameObject.transform.localPosition = Vector3.zero;
                                //gán thời gian hồi chiêu tạm thời để hiển thị số giây còn lại
                                TimeRespawnTemp[1] = Team1[1].GetComponent<HeroBase>().DataValues.vCooldown[1];
                            }
                        }
                        //Hero 3
                        if (RespawnSkillHero3[0].fillAmount <= 0)
                        {
                            if (heroslot.Equals(2) && ValidSkill(Hero[heroslot].DataValues.Mana_skill[skillnumber], 0))
                            {
                                Hero[heroslot].AddSkillToListAction(skillnumber);
                                RespawnSkillHero3[0].fillAmount += 1f;
                                ObjectEffectReady[2].SetActive(false);
                                GrpButtonSkill[2].transform.GetChild(6).gameObject.SetActive(true);
                                GrpButtonSkill[2].transform.GetChild(6).gameObject.transform.localPosition = Vector3.zero;
                                //gán thời gian hồi chiêu tạm thời để hiển thị số giây còn lại
                                TimeRespawnTemp[2] = Team1[2].GetComponent<HeroBase>().DataValues.vCooldown[1];
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //Update
        private void Update()
        {
            //UpdatePisitionCenterCombat();
            HPController();
            RespawnController();
            if (!EndCombat)
            {
                //Ẩn nút skill khi hero chết
                for (int i = 0; i < 3; i++)
                {
                    if (Hero[i] != null)
                        if (!Hero[i].HeroAlive)
                        {
                            GrpButtonSkill[i].SetActive(false);
                            //GrpButtonSkill[i + 3].SetActive (false);
                        }
                }

                //Nếu 1 trong 2 team chết hết, dừng trận đấu
                if ((Hero[0].DataValues.vHealthCurrent <= 0 && Hero[1].DataValues.vHealthCurrent <= 0 && Hero[2].DataValues.vHealthCurrent <= 0) || ((Hero[3] != null ? Hero[3].DataValues.vHealthCurrent : -1) <= 0 && (Hero[4] != null ? Hero[4].DataValues.vHealthCurrent : -1) <= 0 && (Hero[5] != null ? Hero[5].DataValues.vHealthCurrent : -1) <= 0))
                {
                    if (Hero[0].DataValues.vHealthCurrent <= 0 && Hero[1].DataValues.vHealthCurrent <= 0 && Hero[2].DataValues.vHealthCurrent <= 0)
                        IsWin = false;
                    else IsWin = true;
                    Hero[0].LockAction =
                        Hero[1].LockAction =
                        Hero[2].LockAction =
                        Hero[3].LockAction =
                        Hero[4].LockAction =
                        Hero[5].LockAction = LockAction = true; //Khóa thao tác khi xác định thắng thua
                    Hero[0].ChangeAnim("Idie");
                    Hero[1].ChangeAnim("Idie");
                    Hero[2].ChangeAnim("Idie");
                    Hero[3].ChangeAnim("Idie");
                    Hero[4].ChangeAnim("Idie");
                    Hero[5].ChangeAnim("Idie");
                    StartCoroutine(SlowMotionEndBattle());
                    StartCoroutine(GameSystem.FadeOut(GameSystem.BGM, 3f)); //Nhỏ dần nhạc nền rồi tắt
                    EndCombat = true;
                }

                //Tự động đánh
                if (DataUserController.User.IsAutoBattle)
                {
                    if (ObjectEffectReady[0].activeSelf)
                        RunSkill(0, 1);
                    if (ObjectEffectReady[1].activeSelf)
                        RunSkill(1, 1);
                    if (ObjectEffectReady[2].activeSelf)
                        RunSkill(2, 1);
                }
            }
        }

        /// <summary>
        /// Làm chậm hoạt ảnh khi dùng skill
        /// </summary>
        /// <returns></returns>
        private IEnumerator SlowMotionEndBattle()
        {
            Time.timeScale = .2f;
            yield return new WaitForSeconds(.5f);
            Time.timeScale = 1f;
            Hero[0].EndBattle =
                Hero[1].EndBattle =
                Hero[2].EndBattle =
                Hero[3].EndBattle =
                Hero[4].EndBattle =
                Hero[5].EndBattle = true;
            yield return new WaitForSeconds(1f);
            GObject[5].SetActive(true); //Hiển thị canvas kết thúc trận đấu
            TextItemReward[2].text = IsWin ? Languages.lang[176] : "";
            TextItemReward[3].text = !IsWin ? Languages.lang[177] : "";
            SetupInforReward();
            SetupItemReward();
            // var sceneload = new SceneLoad ();
            // sceneload.Change_scene ("Room");
        }

        /// <summary>
        /// Hiển thị damage text
        /// </summary>
        /// <param name="vec">Tọa độ để show dame</param>
        /// <param name="type">Kiểu damage (vật lý hoặc phép thuật)</param>
        /// <param name="damefrom">Đòn đánh từ đối phương nào</param>
        /// <param name="damagevalue">Giá trị dame để hiển thị</param>
        public void DamageShow(Vector3 vec, int type, int damefrom, float damagevalue)
        {
            CheckExistAndCreateDamageObject(vec, type, DamamgeObject, damagevalue, damefrom);
            // var count = DamamgeObject.Count;
            // for (int i = 0; i < count; i++)
            // {
            //     if (!this.DamamgeObject[i].activeSelf)
            //     {
            //         DamamgeObject[i].transform.position = new Vector3(vec.x, vec.y, Module.BASELAYER[9]);
            //         DamamgeComponentText[i].text = "-" + Convert.ToInt32(damagevalue).ToString();//Set dame text
            //         DamamgeObject[i].SetActive(true);
            //         // if (damefrom.Equals(0))//Sát thương gây ra bởi Player
            //         // {
            //         //     IntNumber[1]++;
            //         //     if (!GObject[1].activeSelf)
            //         //         GObject[1].SetActive(true);
            //         //     ComboTextComponent.text = Module.COMBOTEXT + IntNumber[1].ToString();
            //         //     ComboTextColor.a = 1f;
            //         //     ClrComboText[0].a = 255;
            //         //     ClrComboText[1].a = 255;
            //         // }
            //         break;
            //     }
            // }
        }

        /// <summary>
        /// Check khởi tạo và hiển thị damage
        /// </summary>
        /// <param name="col"></param>
        public void CheckExistAndCreateDamageObject(Vector3 vec, int type, List<GameObject> listDamageObject, float valuesresult, int team)
        {
            var a = GetSlotObjectDontActive(listDamageObject);
            if (a == -1)
            { //Nếu tất cả các object đang hoạt động -> tạo mới object
                listDamageObject.Add(Instantiate(listDamageObject[0], new Vector3(vec.x, vec.y, Module.BASELAYER[9]), Quaternion.identity));
                var count = listDamageObject.Count - 1;
                DamageScript.Add(listDamageObject[count].GetComponent<Damage>());
                DamageScript[count].Team = team;
                DamamgeComponentText.Add(DamamgeObject[count].GetComponent<Text>());
                DamageGradient.Add(DamamgeObject[count].GetComponent<Gradient>());
                switch (type)
                {
                    case 0: //Damage vật lý
                        DamamgeComponentText[count].text = valuesresult > 0 ? ("-" + Convert.ToInt32(valuesresult).ToString()) : "Miss"; //Set dame text
                        DamageGradient[count].topColor = GradientColor[0].topColor;
                        DamageGradient[count].bottomColor = GradientColor[0].bottomColor;
                        break;
                    case 1: //Phép thuật
                        DamamgeComponentText[count].text = valuesresult > 0 ? ("-" + Convert.ToInt32(valuesresult).ToString()) : "Miss"; //Set dame text
                        DamageGradient[count].topColor = GradientColor[1].topColor;
                        DamageGradient[count].bottomColor = GradientColor[1].bottomColor;
                        break;
                    case 2: //Hồi máu
                        DamamgeComponentText[count].text = "+" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[count].topColor = GradientColor[2].topColor;
                        DamageGradient[count].bottomColor = GradientColor[2].bottomColor;
                        break;
                    case 3: //Phản dmg
                        DamamgeComponentText[count].text = "+" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[count].topColor = GradientColor[3].topColor;
                        DamageGradient[count].bottomColor = GradientColor[3].bottomColor;
                        break;
                }
                DamamgeObject[count].transform.SetParent(GObject[0].transform, false); //Set parent cho Damage text là Canvas để có thể hiển thị text
            }
            else
            {
                //DamageShow(this.transform.position, skillcore.SkillType, Team, valuesresult);
                listDamageObject[a].transform.position = new Vector3(vec.x, vec.y, Module.BASELAYER[9]);
                DamageScript[a].Team = team;
                switch (type)
                {
                    case 0: //Damage vật lý
                        DamamgeComponentText[a].text = "-" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[a].topColor = GradientColor[0].topColor;
                        DamageGradient[a].bottomColor = GradientColor[0].bottomColor;
                        break;
                    case 1: //Phép thuật
                        DamamgeComponentText[a].text = "-" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[a].topColor = GradientColor[1].topColor;
                        DamageGradient[a].bottomColor = GradientColor[1].bottomColor;
                        break;
                    case 2: //Hồi máu
                        DamamgeComponentText[a].text = "+" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[a].topColor = GradientColor[2].topColor;
                        DamageGradient[a].bottomColor = GradientColor[2].bottomColor;
                        break;
                    case 3: //Phản dmg
                        DamamgeComponentText[a].text = "+" + Convert.ToInt32(valuesresult).ToString(); //Set dame text
                        DamageGradient[a].topColor = GradientColor[3].topColor;
                        DamageGradient[a].bottomColor = GradientColor[3].bottomColor;
                        break;
                }
                listDamageObject[a].SetActive(true);
            }
        }

        /// <summary>
        /// Trả về object đang ko hoạt động để xuất hiện
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

        /// <summary>
        /// Trả về slot object đang ko hoạt động để xuất hiện
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetSlotObjectDontActive(List<GameObject> obj)
        {
            int count = obj.Count;
            for (int i = 0; i < count; i++)
            {
                if (!obj[i].activeSelf)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Update tọa độ cho camera vitual ăn theo
        /// </summary>
        public void UpdatePisitionCenterCombat(float posX)
        {
            if (BattleCore.UserVitualCamera)
                PositionCenterCombat.transform.position = new Vector3(posX / 5, PositionCenterCombat.transform.position.y, PositionCenterCombat.transform.position.z);
        }

        /// <summary>
        /// Khởi chạy âm thanh cho các skill của nhân vật
        /// </summary>
        /// <param name="audioFile">audio clip</param>
        /// <param name="timeDelay">thời gian chờ trước khi chạy âm thanh</param>
        /// <returns></returns>
        public virtual IEnumerator PlaySound(AudioClip audioFile, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            Sound.PlayOneShot(audioFile);
        }

        /// <summary>
        /// 3 nút sử dụng item trong battle
        /// </summary>
        /// <param name="slot">Slot button</param>
        public void ButtonItemUse(int slot)
        {
            var itemUseEquip = DataUserController.User.ItemUseForBattle.Split(';'); //Lấy danh sách item use đã dc mang
            if (!string.IsNullOrEmpty(itemUseEquip[slot]) && !IsUseItem[slot]) //Nếu slot đang chọn đã trang bị item rồi và chưa sử dụng
            {
                GObject[slot + 11].SetActive(true); //Bật hiệu ứng đã sử dụng
                InventorySystem.ReduceItemQuantityInventory(Convert.ToSByte(itemUseEquip[slot].Split(',')[0]), Convert.ToByte(itemUseEquip[slot].Split(',')[1]), 1); //Trừ item trong inventory

                StartCoroutine(ItemUseAction(Convert.ToSByte(itemUseEquip[slot].Split(',')[0]), Convert.ToByte(itemUseEquip[slot].Split(',')[1]))); //Sử dụng item

                itemUseEquip[slot] = ""; //Xóa thông tin item trong phần trang bị trước battle
                ItemUseImg[slot].sprite = Resources.Load<Sprite>("Images/none"); //Xóa hình ảnh

                //DataUserController.User.ItemUseForBattle = itemUseEquip[0] + ";" + itemUseEquip[1] + ";" + itemUseEquip[2];
                //DataUserController.SaveUserInfor ();
                DataUserController.SaveInventory();
            }
            IsUseItem[slot] = true;
        }

        /// <summary>
        /// Thực hiện sử dụng item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        private IEnumerator ItemUseAction(sbyte itemType, byte itemID)
        {

            #region Item 1 : phục hồi 10% lượng máu đã mất cho toàn đội hình trong 10 giây 

            if (itemType.Equals(10) && itemID.Equals(0))
            {
                StartCoroutine(Hero[0].ActionHealthRegen((Hero[0].DataValues.vHealth - Hero[0].DataValues.vHealthCurrent) * 10 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen((Hero[1].DataValues.vHealth - Hero[1].DataValues.vHealthCurrent) * 10 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen((Hero[2].DataValues.vHealth - Hero[2].DataValues.vHealthCurrent) * 10 / 100 / 10f, 1f, 10));
            }

            #endregion

            #region Item 2 : phục hồi 20% lượng máu đã mất cho toàn đội hình trong 10 giây

            if (itemType.Equals(10) && itemID.Equals(1))
            {
                StartCoroutine(Hero[0].ActionHealthRegen((Hero[0].DataValues.vHealth - Hero[0].DataValues.vHealthCurrent) * 20 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen((Hero[1].DataValues.vHealth - Hero[1].DataValues.vHealthCurrent) * 20 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen((Hero[2].DataValues.vHealth - Hero[2].DataValues.vHealthCurrent) * 20 / 100 / 10f, 1f, 10));
            }

            #endregion

            #region Item 3 : phục hồi 30% lượng máu đã mất cho toàn đội hình trong 10 giây

            if (itemType.Equals(10) && itemID.Equals(2))
            {
                StartCoroutine(Hero[0].ActionHealthRegen((Hero[0].DataValues.vHealth - Hero[0].DataValues.vHealthCurrent) * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen((Hero[1].DataValues.vHealth - Hero[1].DataValues.vHealthCurrent) * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen((Hero[2].DataValues.vHealth - Hero[2].DataValues.vHealthCurrent) * 30 / 100 / 10f, 1f, 10));
            }

            #endregion

            #region Item 4 : phục hồi 10% tổng lượng máu cho toàn đội hình trong 5 giây

            if (itemType.Equals(10) && itemID.Equals(3))
            {
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 10 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 10 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 10 / 100 / 5f, 1f, 5));
            }

            #endregion

            #region Item 5 : phục hồi 20% tổng lượng máu cho toàn đội hình trong 5 giây

            if (itemType.Equals(10) && itemID.Equals(4))
            {
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 20 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 20 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 20 / 100 / 5f, 1f, 5));
            }

            #endregion

            #region Item 6 : phục hồi 30% tổng lượng máu cho toàn đội hình trong 5 giây

            if (itemType.Equals(10) && itemID.Equals(5))
            {
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 30 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 30 / 100 / 5f, 1f, 5));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 30 / 100 / 5f, 1f, 5));
            }

            #endregion

            #region Item 7 : phục hồi 30% tổng lượng máu và tăng 10% hút máu thích ứng cho toàn đội hình trong 10 giây

            if (itemType.Equals(10) && itemID.Equals(6))
            {
                //Tăng máu
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                //Tăng hút máu thích ứng
                StartCoroutine(Hero[0].ActionBuffValues("vLifeSteal", 10f, 10));
                StartCoroutine(Hero[1].ActionBuffValues("vLifeSteal", 10f, 10));
                StartCoroutine(Hero[2].ActionBuffValues("vLifeSteal", 10f, 10));
            }
            #endregion

            #region Item 8 : phục hồi 30% tổng lượng máu và tăng 10% giáp, kháng phép cho toàn đội hình trong 10 giây

            if (itemType.Equals(10) && itemID.Equals(7))
            {
                //Tăng máu
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                //Tăng giáp
                StartCoroutine(Hero[0].ActionBuffValues("vArmor", Hero[0].DataValues.vArmor * 10 / 100f, 10));
                StartCoroutine(Hero[1].ActionBuffValues("vArmor", Hero[1].DataValues.vArmor * 10 / 100f, 10));
                StartCoroutine(Hero[2].ActionBuffValues("vArmor", Hero[2].DataValues.vArmor * 10 / 100f, 10));
                //Tăng kháng phép
                StartCoroutine(Hero[0].ActionBuffValues("vArmor", Hero[0].DataValues.vMagicResist * 10 / 100f, 10));
                StartCoroutine(Hero[1].ActionBuffValues("vArmor", Hero[1].DataValues.vMagicResist * 10 / 100f, 10));
                StartCoroutine(Hero[2].ActionBuffValues("vArmor", Hero[2].DataValues.vMagicResist * 10 / 100f, 10));
            }

            #endregion

            #region Item 9 : phục hồi 30% tổng lượng máu và tăng 10% sát thương cho toàn đội hình trong 10 giây

            if (itemType.Equals(10) && itemID.Equals(8))
            {
                //Tăng máu
                StartCoroutine(Hero[0].ActionHealthRegen(Hero[0].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[1].ActionHealthRegen(Hero[1].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                StartCoroutine(Hero[2].ActionHealthRegen(Hero[2].DataValues.vHealth * 30 / 100 / 10f, 1f, 10));
                //Tăng sát thương vly
                StartCoroutine(Hero[0].ActionBuffValues("vAtk", Hero[0].DataValues.vAtk * 10 / 100f, 10));
                StartCoroutine(Hero[1].ActionBuffValues("vAtk", Hero[1].DataValues.vAtk * 10 / 100f, 10));
                StartCoroutine(Hero[2].ActionBuffValues("vAtk", Hero[2].DataValues.vAtk * 10 / 100f, 10));
                //Tăng sát thương phép
                StartCoroutine(Hero[0].ActionBuffValues("vMagic", Hero[0].DataValues.vMagic * 10 / 100f, 10));
                StartCoroutine(Hero[1].ActionBuffValues("vMagic", Hero[1].DataValues.vMagic * 10 / 100f, 10));
                StartCoroutine(Hero[2].ActionBuffValues("vMagic", Hero[2].DataValues.vMagic * 10 / 100f, 10));
            }

            #endregion

            yield return 0;
        }

        #endregion

        #region Nhận thưởng khi kết thúc trận đấu 

        /// <summary>
        /// Hiển thị thông tin các chỉ số sau trận đấu
        /// </summary>
        private void SetupInforReward()
        {
            //if (ADS.interstitial.IsLoaded())
            //{
            //    ADS.interstitial.Show();
            //}

            //Hiển thị quảng cáo sau trận
            if (ADSInters.IsReady())
            {
                ADSInters.ShowAd();
            }

            if (IsWin)
            {
                DataUserController.User.BattleWin += 1; // + 1 trận thắng
                BattleCore.ChangeDifficult(0, Module.WorldMapRegionSelected);
            }
            else
                DataUserController.User.BattleLose += 1; // + 1 trận thua
            DataUserController.SaveUserInfor(); //Lưu data user
            //GameSystem.StopBGM (); //Dừng nhạc nền 
            EndBattleAvtHero[0].sprite = Resources.Load<Sprite>("HeroAvt/" + Hero[0].HeroID);
            EndBattleAvtHero[1].sprite = Resources.Load<Sprite>("HeroAvt/" + Hero[1].HeroID);
            EndBattleAvtHero[2].sprite = Resources.Load<Sprite>("HeroAvt/" + Hero[2].HeroID);
            //Tính lượng exp mà hero nhận dc
            var expH1 = IsWin ? (Hero[0].DamageValue + Hero[0].DefenseValue) / 3 : (Hero[0].DamageValue + Hero[0].DefenseValue) / 10;
            var expH2 = IsWin ? (Hero[1].DamageValue + Hero[1].DefenseValue) / 3 : (Hero[1].DamageValue + Hero[1].DefenseValue) / 10;
            var expH3 = IsWin ? (Hero[2].DamageValue + Hero[2].DefenseValue) / 3 : (Hero[2].DamageValue + Hero[2].DefenseValue) / 10;
            var h1 = DataUserController.Heroes.DBHeroes.Find(x => x.ID == Hero[0].HeroID);
            var h2 = DataUserController.Heroes.DBHeroes.Find(x => x.ID == Hero[1].HeroID);
            var h3 = DataUserController.Heroes.DBHeroes.Find(x => x.ID == Hero[2].HeroID);
            //Tính lượng vàng mà user nhận dc dựa trên sát thương gây ra + tỉ lệ vàng
            var goldReward = (int)((expH1 + expH2 + expH3) / 3 + DataUserController.User.LevelMap[Module.WorldMapRegionSelected] / 10f * 136);
            goldReward += goldReward * (int)(Team1[0].GetComponent<HeroBase>().DataValues.vRewardPlus + Team1[1].GetComponent<HeroBase>().DataValues.vRewardPlus + Team1[2].GetComponent<HeroBase>().DataValues.vRewardPlus) / 100;
            bool haveGem = IsWin ? UnityEngine.Random.Range(0, 100) < BattleCore.GemRewardEndBattleRate ? true : false : false; //Tính xem có thể nhận dc gem hay ko
            var gemReward = haveGem ? UnityEngine.Random.Range(0, BattleCore.MaxGemRewardEndBattle + 1) : 0; //Nếu win thì có tỉ lệ nhận dc ngẫu nhiên 1 số gem
            DataUserController.User.Golds += goldReward; //Cộng vàng cho user
            DataUserController.User.Gems += gemReward; //Cộng đá quý cho user
            TextItemReward[8].text = Languages.lang[258] + goldReward.ToString(); //Text số vàng nhận dc
            TextItemReward[7].text = gemReward > 0 ? Languages.lang[259] + gemReward.ToString() : ""; //Text số đá quý nhận dc

            TextItemReward[4].text = Languages.lang[23] + h1.Level;
            TextItemReward[5].text = Languages.lang[23] + h2.Level;
            TextItemReward[6].text = Languages.lang[23] + h3.Level;
            GObject[6].transform.localScale = new Vector3(h1.EXP / Module.NextExp(h1.Level), 1, 1);
            GObject[7].transform.localScale = new Vector3(h2.EXP / Module.NextExp(h2.Level), 1, 1);
            GObject[8].transform.localScale = new Vector3(h3.EXP / Module.NextExp(h3.Level), 1, 1);

            h1.EXP = h1.EXP + expH1;
            h2.EXP = h2.EXP + expH2;
            h3.EXP = h3.EXP + expH3;
            #region Cộng exp và level cho hero 

            var endwhile = false;
            while (!endwhile)
            {
                var expNext = Module.NextExp(h1.Level);
                if (h1.EXP - expNext >= 0)
                {
                    h1.EXP -= expNext;
                    h1.Level++;
                }
                else
                {
                    endwhile = true;
                }
                if (h1.Level >= Module.HeroMaxlevel)
                {
                    h1.EXP = 0;
                    endwhile = true;
                }
            }
            endwhile = false;
            while (!endwhile)
            {
                var expNext = Module.NextExp(h2.Level);
                if (h2.EXP - expNext >= 0)
                {
                    h2.EXP -= expNext;
                    h2.Level++;
                }
                else
                {
                    endwhile = true;
                }
                if (h2.Level >= Module.HeroMaxlevel)
                {
                    h2.EXP = 0;
                    endwhile = true;
                }
            }
            endwhile = false;
            while (!endwhile)
            {
                var expNext = Module.NextExp(h3.Level);
                if (h3.EXP - expNext >= 0)
                {
                    h3.EXP -= expNext;
                    h3.Level++;
                }
                else
                {
                    endwhile = true;
                }
                if (h3.Level >= Module.HeroMaxlevel)
                {
                    h3.EXP = 0;
                    endwhile = true;
                }
            }

            #endregion
            DataUserController.SaveHeroes();
            StartCoroutine(ChangeEXPBar(new float[] { expH1, expH2, expH3 }, new HeroesProperties[] { h1, h2, h3 }));
        }

        /// <summary>
        /// Thay đổi cách hiển thị thanh exp theo thời gian
        /// </summary>
        /// <param name="ExpAdd"></param>
        /// <returns></returns>
        private IEnumerator ChangeEXPBar(float[] ExpAdd, HeroesProperties[] ListHero)
        {
            yield return new WaitForSeconds(.3f);
            TextItemReward[4].text = Languages.lang[23] + ListHero[0].Level + ". " + Languages.lang[178] + " <color=#70FF00>+" + Convert.ToInt32(ExpAdd[0]).ToString() + "</color>";
            StartCoroutine(GameSystem.ScaleUI(0, GObject[6], new Vector3(ListHero[0].EXP / Module.NextExp(ListHero[0].Level), 1f, 1f), 1f));
            //GObject[6].transform.localScale = new Vector3 (ListHero[0].EXP / Module.NextExp (ListHero[0].Level), 1, 1);
            yield return new WaitForSeconds(.3f);
            TextItemReward[5].text = Languages.lang[23] + ListHero[1].Level + ". " + Languages.lang[178] + " <color=#70FF00>+" + Convert.ToInt32(ExpAdd[1]).ToString() + "</color>";
            StartCoroutine(GameSystem.ScaleUI(0, GObject[7], new Vector3(ListHero[1].EXP / Module.NextExp(ListHero[1].Level), 1f, 1f), 1f));
            //GObject[7].transform.localScale = new Vector3 (ListHero[1].EXP / Module.NextExp (ListHero[1].Level), 1, 1);
            yield return new WaitForSeconds(.3f);
            TextItemReward[6].text = Languages.lang[23] + ListHero[2].Level + ". " + Languages.lang[178] + " <color=#70FF00>+" + Convert.ToInt32(ExpAdd[2]).ToString() + "</color>";
            StartCoroutine(GameSystem.ScaleUI(0, GObject[8], new Vector3(ListHero[2].EXP / Module.NextExp(ListHero[2].Level), 1f, 1f), 1f));
            //GObject[8].transform.localScale = new Vector3 (ListHero[2].EXP / Module.NextExp (ListHero[2].Level), 1, 1);
        }
        /// <summary>
        /// Thêm item nhận được sau trận đấu
        /// </summary>
        private void SetupItemReward()
        {
            var count = UnityEngine.Random.Range(1, Module.BattleModeSelected.Equals(0) ? BattleCore.MaxItemReward / 2 : BattleCore.MaxItemReward); //Ngẫu nhiên số lượng item
            ListItemsReward = new List<ItemDroped>();
            for (byte i = 0; i < count; i++)
            {
                var idTemp = ItemDropController.DropedForBattle(Module.WorldMapRegionSelected);
                if (idTemp != null) //Nếu có rơi đồ => thêm đồ
                {
                    var checkExist = ListItemsReward.Find(x => x.ItemType == idTemp.ItemType && x.ItemID == idTemp.ItemID) != null; //Kiểm tra tồn tại item rơi ra có trong danh sách nhận chưa
                    if (checkExist) //Rồi thì +1
                        ListItemsReward[ListItemsReward.FindIndex(x => x.ItemType == idTemp.ItemType && x.ItemID == idTemp.ItemID)].Quantity++;
                    else //Chưa thì add vào
                        ListItemsReward.Add(new ItemDroped(idTemp.ItemType, idTemp.ItemID, 1));
                }
            }
            count = ListItemsReward.Count;
            for (byte i = 0; i < count; i++) //Đẩy item vào inventory
                InventorySystem.AddItemToInventory(ItemSystem.CreateItem(false, false, 0, ListItemsReward[i].ItemType, ListItemsReward[i].ItemID, ListItemsReward[i].Quantity));
            DataUserController.SaveInventory(); //Lưu inventory
            SetupObjectReward(ListItemsReward);
            if (ListBackgroundItemsReward.Count > 0)
                StartCoroutine(ShowingItemsReward(0));
        }

        /// <summary>
        /// Cài đặt các object để hiển thị
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemQuantity"></param>
        private void SetupObjectReward(List<ItemDroped> itemDropeds)
        {
            //Debug.Log (Camera.main.aspect);
            var quantity = itemDropeds.Count;
            var itemPerLine = 5; //Camera.main.aspect * 4; //Số lượng item trên 1 hàng ngang
            ListBackgroundItemsReward = new List<GameObject>();
            ListItemsRewardImg = new List<GameObject>();
            int i_temp_x = 0; //Biến tạm hàng ngang
            int i_temp_y = 0; //Biến tạm hàng dọc
            float regionSpace = 160f; //Khoảng cách giữa các object
            float horizonXOriginal = 214;
            for (int i = 0; i < quantity; i++)
            {
                ListBackgroundItemsReward.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/ItemBody"), new Vector3(0, 0, 0), Quaternion.identity));
                ListBackgroundItemsReward[i].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                ListItemsRewardImg.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/Items"), new Vector3(0, 0, 0), Quaternion.identity));
                ListItemsRewardImg[i].GetComponent<RectTransform>().sizeDelta = new Vector2(130, 130);
                ListBackgroundItemsReward[i].transform.SetParent(GObject[2].transform, false); //Đẩy prefab vào khung reward
                ListItemsRewardImg[i].transform.SetParent(ListBackgroundItemsReward[i].transform, false); //Đẩy prefab vào khung reward
                ListItemsRewardImg[i].transform.GetChild(0).transform.GetComponent<Text>().text = itemDropeds[i].Quantity.ToString(); //Gán text số lượng
                ListItemsRewardImg[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Items/" + itemDropeds[i].ItemType + @"/" + itemDropeds[i].ItemID); //Gán hình ảnh
                var temp = i;
                ListItemsRewardImg[i].GetComponent<Button>().onClick.AddListener(() => ItemClick(itemDropeds[temp]));
                //Chỉnh căn trái cho các item
                ListBackgroundItemsReward[i].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
                ListBackgroundItemsReward[i].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            }
            for (int i = 0; i < quantity; i++)
            {
                ListBackgroundItemsReward[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(horizonXOriginal + regionSpace * i_temp_x, 50 + regionSpace * -i_temp_y, 0);
                ListBackgroundItemsReward[i].transform.localScale = new Vector3(0, 1, 1);
                i_temp_x++;
                //Space line
                if ((i + 1) % Convert.ToInt32(itemPerLine) == 0 && i != 0)
                {
                    i_temp_y++;
                    i_temp_x = 0;
                }
            }
        }

        /// <summary>
        /// Hiệu ứng xuất hiện các item nhận được sau trận đấu
        /// </summary>
        /// <param name="timeDelay"></param>
        /// <returns></returns>
        private IEnumerator ShowingItemsReward(float timeDelay)
        {
            yield return new WaitForSeconds(.5f);
            var count = ListBackgroundItemsReward.Count;
            int start = 0;
            float scaleX = 0;
        Begin:
            scaleX += .2f;
            ListBackgroundItemsReward[start].transform.localScale = new Vector3(scaleX, 1, 1);
            yield return new WaitForSeconds(timeDelay);
            if (start >= count - 1 && scaleX >= 1f)
                goto End;
            if (scaleX >= 1f)
            {
                start++;
                scaleX = 0f;
                goto Begin;
            }
            else goto Begin;
            End:
            yield return new WaitForSeconds(0);
        }

        /// <summary>
        /// Show thông tin chi tiết từng item
        /// </summary>
        /// <param name="itemID"></param>
        private void ItemClick(ItemDroped item)
        {
            GObject[4].SetActive(true);
            TextItemReward[0].text = ItemSystem.GetItemName(item.ItemType, item.ItemID); //Tên item
            TextItemReward[1].text = ItemSystem.GetItemDescription(item.ItemType, item.ItemID); //Descriptions
            GObject[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Items/" + item.ItemType + @"/" + item.ItemID);
        }

        /// <summary>
        /// Ẩn thông tin chi tiết
        /// </summary>
        public void ButtonHideItemInfor()
        {
            GObject[4].SetActive(false);
        }

        /// <summary>
        /// Chức năng các button
        /// </summary>
        /// <param name="type"></param>
        public void ButtonFunctions(int type)
        {
            switch (type)
            {
                case 0: //Nút quay lại scene đội hình khi end battle và nhận thưởng
                    if (IsWin && Module.BattleModeSelected.Equals(0)) //Nếu win và chơi chế độ preview -> xóa thông tin đối thủ của map hiện tại để generate lại ở scene worldmap
                    {
                        DataUserController.User.EnemyFutureMap[Module.WorldMapRegionSelected] =
                            BattleCore.HeroIDLine3[UnityEngine.Random.Range(0, BattleCore.HeroIDLine3.Count)].ToString() + ";" +
                            BattleCore.HeroIDLine2[UnityEngine.Random.Range(0, BattleCore.HeroIDLine2.Count)].ToString() + ";" +
                            BattleCore.HeroIDLine1[UnityEngine.Random.Range(0, BattleCore.HeroIDLine1.Count)].ToString();
                    }
                    break;
                case 1: //Nút show UI setting
                    Time.timeScale = 0f;
                    //GObject[10].SetActive(true);
                    GameSystem.InitializePrefabUI(11);
                    StartCoroutine(WaitingCloseUI(11)); //Chờ đóng UI
                    break;
                case 2: //Đóng form setting kèm theo fix battle UI
                    Time.timeScale = BattleCore.BattleSpeed[GlobalVariables.SlotBattleSpeed];
                    break;
                case 3: //Hoán đổi vị trí battle UI
                    SwapBattleUI();
                    break;
                case 4: //Auto đánh
                    if (!LockAction)
                    { //Nếu chưa kết thúc battle
                        GObject[14].SetActive(!GObject[14].activeSelf);
                        DataUserController.User.IsAutoBattle = GObject[14].activeSelf;
                    }
                    break;
                case 5: //Tăng speed trận đấu
                    if (!LockAction)
                    { //Nếu chưa kết thúc battle
                        GlobalVariables.SlotBattleSpeed = GlobalVariables.SlotBattleSpeed >= (BattleCore.BattleSpeed.Length - 1) ? 0 : GlobalVariables.SlotBattleSpeed + 1;
                        Time.timeScale = BattleCore.BattleSpeed[GlobalVariables.SlotBattleSpeed];
                        TextItemReward[9].text = "X" + (GlobalVariables.SlotBattleSpeed + 1).ToString();
                    }
                    break;
                default:
                    break;
            }
            DataUserController.SaveUserInfor();
        }

        private IEnumerator WaitingCloseUI(int type)
        {
            switch (type)
            {
                case 11://SettingCanvasUI
                    yield return new WaitUntil(() => GameSystem.SettingCanvasUI == null);
                    if (GameSystem.SettingCanvasUI == null)
                    {
                        Time.timeScale = BattleCore.BattleSpeed[GlobalVariables.SlotBattleSpeed];
                    }
                    break;
            }
        }

        #endregion
    }
}