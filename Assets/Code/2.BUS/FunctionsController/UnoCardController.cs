using Assets.Code._0.DTO.Models;
using Assets.Code._2.BUS.Misc;
using Assets.Code._4.CORE.UnoCard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2.BUS.FunctionsController
{
    public class UnoCardController : MonoBehaviour
    {
        #region Variables
        [Header("Draw Curve")]
        public AnimationCurve moveCurve;
        private List<GameObject>[] ObjectsUserCard;
        private List<UnoCardModel>[] UserCards;
        public GameObject[] ObjController;
        public Text[] TextUI;
        public Text[] TextCountCardPlayer;
        UnityEngine.Object[] SpritesCardImg;
        private List<GameObject> CardsShowed;//Các object card đã bỏ xuống
        private UnoCardModel LastCard;
        private int SlotCardSelected = -1;//Vị trí bài đang chọn
        private bool IsLeftToRight = true;//Hướng chơi bài
        private int CurentColorType = 0;//0 = đỏ, 1 = vàng, 2 = lục, 3 = lam, 4: đa năng
        private int TornadoColor = 0;//Màu của lá bài lốc xoáy
        private bool IsControl = true;//Cho phép thao tác các button 
        private int TotalPlayer = 6;//Tổng số người chơi trong ván bài, kể cả AI
        private int CurentRound = 0;//Lượt chơi tới người nào. 0 = player
        private bool IsGetCard = false;//Lượt vừa rồi đã bốc bài hay chưa
        public GameObject[] ObjectPositionPlayer;
        public GameObject[] ObjectPositionCountCard;
        public GameObject[] ObjectGrpCardListPlayer;
        private int TotalCardOriginal = 0;
        private Vector3[] PosOriginal;
        private int SlotPlyerWinner;//Slot người chơi thắng cuộc
        private Dictionary<int, int> RankingPoint;
        public List<Image> ImgSupport;
        private bool Card4PlusVictim;//Sử dụng lá bài +4 chỉ định mục tiêu
        private int SlotVictim;//Slot user của lá bài +4 chỉ định mục tiêu

        //End game
        private bool IsStopGame = false;//Kết thúc game hay chưa
        private List<GameObject> ObjectRanking;
        private List<AudioClip> SoundClip;

        Vector3 rotationEuler = Vector3.zero;
        private float SpeedRotationZ = 50f; //Tốc độ quay, giá trị này sẽ được gán lại phía dưới
        private RectTransform RectArrowSpin;

        private readonly bool IsDeveloper = false;//Chế độ phát triển, khi public thì disable nó
        private bool EndCard;//Khi 1 ng chơi hết bài thì = true => chống chế độ tự bốc bài bốc thêm
        #endregion

        #region Initialize
        void Start()
        {
            SetupPositionOriginal();
            SetupSound();
            SetupTextUI();
            LoadSetting();
            UnoCardSystem.SetupUnoCard();
            ImgSupport = new List<Image>();
            TotalCardOriginal = UnoCardSystem.UnoCards.Count;
            ObjectsUserCard = new List<GameObject>[TotalPlayer];
            UserCards = new List<UnoCardModel>[TotalPlayer];
            for (int i = 0; i < TotalPlayer; i++)
            {
                ObjectsUserCard[i] = new List<GameObject>();
                UserCards[i] = new List<UnoCardModel>();
            }
            CardsShowed = new List<GameObject>();
            RectArrowSpin = ObjController[1].GetComponent<RectTransform>();
            LoadAddCardImg();
            TextUI[1].text = UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot].ToString();//Số tiền cược
            TextUI[2].text = UnoCardSystem.PlayersNumber[UnoCardSystem.PlayerNumberSlot].ToString();//Số người chơi
            ObjController[17].SetActive(true);//Hiển thị UI lựa chọn cược và người chơi
            //SetupNewBattle();
        }

        /// <summary>
        /// Khởi tạo tọa độ ban đầu của các slot player
        /// </summary>
        private void SetupPositionOriginal()
        {
            PosOriginal = new Vector3[UnoCardSystem.PlayersNumber[UnoCardSystem.PlayersNumber.Length - 1]];
            for (int i = 0; i < PosOriginal.Length; i++)
            {
                PosOriginal[i] = ObjectPositionPlayer[i].transform.position;
            }
        }

        /// <summary>
        /// Khởi tạo âm thanh
        /// </summary>
        private void SetupSound()
        {
            if (GameSystem.Settings.SoundEnable)
            {
                SoundClip = new List<AudioClip>();
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/UnoGetCard"));//Phát bài
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/UnoThrowToTable"));//Đánh bài
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/Error"));//Lỗi chọn
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/Select2"));//Tới lượt player
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/UnoLose"));//Thua
                SoundClip.Add(Resources.Load<AudioClip>("Audio/SE/UnoWin"));//Win
            }
        }

        /// <summary>
        /// Gán giao diện ngôn ngữ
        /// </summary>
        private void SetupTextUI()
        {
            TextUI[0].text = Languages.lang[318];// = "Players";
            TextUI[4].text = Languages.lang[311];// = "Chọn màu muốn đổi";
            TextUI[5].text = Languages.lang[312];// = "Đánh";
            TextUI[6].text = Languages.lang[313];// = "Bỏ lượt";
            TextUI[7].text = Languages.lang[314];// = "Thoát";
            TextUI[8].text = Languages.lang[315];// = "Chơi lại";
            TextUI[9].text = Languages.lang[319];// = "Tùy chọn";
            TextUI[10].text = Languages.lang[317];// = "Đặt cược";
        }

        /// <summary>
        /// Khởi tạo mới các biến trước khi tạo màn chơi mới
        /// </summary>
        private void ResetVariables()
        {
            IsStopGame = false;

            //Cài đặt tọa độ và hiển thị các đối thủ + player
            TotalPlayer = UnoCardSystem.PlayersNumber[UnoCardSystem.PlayerNumberSlot];

            //Ẩn hiện các player theo tùy chọn số lượng
            for (int i = 0; i < UnoCardSystem.PlayersNumber[UnoCardSystem.PlayersNumber.Length - 1]; i++)
            {
                if (i < TotalPlayer)
                    ObjectPositionPlayer[i].SetActive(true);
                else
                    ObjectPositionPlayer[i].SetActive(false);
            }

            //Sắp xếp vị trí các đối thủ trong bàn theo tùy chọn số lượng
            switch (TotalPlayer)
            {
                case 2:
                    ObjectPositionPlayer[1].transform.position = PosOriginal[3];
                    break;
                case 3:
                    ObjectPositionPlayer[1].transform.position = PosOriginal[2];
                    ObjectPositionPlayer[2].transform.position = PosOriginal[4];
                    break;
                case 4:
                    ObjectPositionPlayer[1].transform.position = PosOriginal[1];
                    ObjectPositionPlayer[2].transform.position = PosOriginal[3];
                    ObjectPositionPlayer[3].transform.position = PosOriginal[5];
                    break;
                default:
                    ObjectPositionPlayer[1].transform.position = PosOriginal[1];
                    ObjectPositionPlayer[2].transform.position = PosOriginal[2];
                    ObjectPositionPlayer[3].transform.position = PosOriginal[3];
                    ObjectPositionPlayer[4].transform.position = PosOriginal[4];
                    ObjectPositionPlayer[5].transform.position = PosOriginal[5];
                    break;
            }

            IsControl = false;
            if (!IsLeftToRight)
                ChangeCircle();
            LastCard = new UnoCardModel();
            LastCard.NumberID = -1;
            SlotCardSelected = -1;//Vị trí bài đang chọn
            CurentRound = 0;
            IsGetCard = false;

            //Clear danh sách object card
            for (int i = 0; i < TotalPlayer; i++)
            {
                GameSystem.DisposeObjectList(ObjectsUserCard[i], i > TotalPlayer - 1 ? true : false);
                ObjectsUserCard[i].Clear();
                UserCards[i].Clear();
            }

            //Clear object ranking
            GameSystem.DisposeObjectList(ObjectRanking, false);
            if (ObjectRanking != null)
                ObjectRanking.Clear();
        }

        /// <summary>
        /// Khởi tạo ván bài mới
        /// </summary>
        private void SetupNewBattle()
        {
            ObjController[21].SetActive(false);//Ẩn anim bốc bài
            EndCard = false;
            // GameSystem.UpdateResourceText(TextUI[3], TextUI[11]);//Cập nhật lại tiền và kim cương
            ObjController[16].SetActive(false);
            GameSystem.DisposeAllObjectChild(ObjController[3]);
            GameSystem.DisposeAllObjectChild(ObjController[5]);
            IsLeftToRight = true;
            IsControl = false;
            CurentColorType = UnityEngine.Random.Range(0, 4);
            ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
            ObjController[1].SetActive(true);
            for (int i = 0; i < TotalPlayer; i++)
                StartCoroutine(CreateAndMoveCard(i));//Tạo mới card cho Player
            ObjController[11].transform.position = ObjController[12].transform.position;//Di chuyển nút loading
            ObjController[11].SetActive(true);
            SetupButtonSelectCard4Plus();
        }

        /// <summary>
        /// Khởi tạo button chọn mục tiêu cho card +4
        /// </summary>
        private void SetupButtonSelectCard4Plus()
        {
            for (int i = 1; i < TotalPlayer; i++)
            {
                var temp = i;
                ObjectPositionPlayer[i].transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                ObjectPositionPlayer[i].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SelectVictimForCard4Plus(temp));
            }
        }

        /// <summary>
        /// Load các hình ảnh card vào bộ nhớ
        /// </summary>
        private void LoadAddCardImg()
        {
            SpritesCardImg = Resources.LoadAll("Images/UnoCard/UnoCard");
        }

        /// <summary>
        /// Khởi tạo cài đặt chế độ chơi
        /// </summary>
        private void LoadSetting()
        {
            ObjController[10].SetActive(!DataUserController.User.UnoSettingFastPush);
            ObjController[18].GetComponent<Image>().color = new Color32(DataUserController.User.UnoBGColorR, DataUserController.User.UnoBGColorG, DataUserController.User.UnoBGColorB, 255);
            ObjController[19].GetComponent<Image>().color = new Color32(DataUserController.User.UnoBGColorR, DataUserController.User.UnoBGColorG, DataUserController.User.UnoBGColorB, 255);
            ShowImgSupport();
        }
        #endregion

        #region Functions

        private void Update()
        {
            rotationEuler -= Vector3.forward * SpeedRotationZ * Time.deltaTime; //increment 30 degrees every second
            RectArrowSpin.rotation = Quaternion.Euler(rotationEuler);
        }

        /// <summary>
        /// Hiển thị hiệu ứng hỗ trợ
        /// </summary>
        private void ShowImgSupport()
        {
            try
            {
                if (CurentRound == 0 && DataUserController.User.UnoSettingImgSupport)
                {
                    //Loại bỏ các object null
                    var count = ImgSupport.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        if (ImgSupport[i] == null)
                            ImgSupport.RemoveAt(i);
                        else
                            ImgSupport[i].gameObject.SetActive(false);
                    }

                    var result = UserCards[0].Select((c, i) => new { Item = c, Index = i }).Where(x => x.Item.ColorType.Equals(CurentColorType) || x.Item.NumberID.Equals(LastCard.NumberID) || x.Item.ColorType.Equals(4)).Select(x => x.Index).ToList();

                    if (result != null)
                    {
                        count = result.Count;
                        var count2 = ImgSupport.Count;

                        if (count > count2)
                        {
                            for (int i = 0; i < count - count2; i++)
                            {
                                ImgSupport.Add(new GameObject().AddComponent<Image>());
                            }
                        }

                        if (count > 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                ImgSupport[i].gameObject.SetActive(true);
                                ImgSupport[i].gameObject.transform.SetParent(ObjectsUserCard[0][result[i]].transform, false);
                                ImgSupport[i].transform.SetParent(ObjectsUserCard[0][result[i]].transform, false);
                                ImgSupport[i].sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == "UnoCard_75");
                                ImgSupport[i].gameObject.transform.position = ObjectsUserCard[0][result[i]].transform.position;
                                ImgSupport[i].GetComponent<Image>().SetNativeSize();
                                ImgSupport[i].GetComponent<Image>().raycastTarget = false;
                            }
                        }
                    }
                }
                else
                {
                    //Ẩn toàn bộ object
                    var count = ImgSupport.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (ImgSupport[i] != null)
                            ImgSupport[i].gameObject.SetActive(false);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Next lượt tới người chơi tiếp theo
        /// </summary>
        private IEnumerator NextPlayer(int round, float delayTime)
        {
            IsGetCard = false;
            IsControl = false;
            ObjController[16].SetActive(false);
            for (int i = 0; i < round; i++)
            {
                CurentRound += IsLeftToRight ? 1 : -1;
                if (CurentRound < 0)
                    CurentRound = TotalPlayer - 1;
                if (CurentRound > TotalPlayer - 1)
                    CurentRound = 0;
            }
            if (CurentRound == 0)
            {
                StartCoroutine(GameSystem.PlaySound(SoundClip[3], 0));//Play sound nếu tới lượt player
                yield return new WaitForSeconds(delayTime);
                IsControl = true;

                //Tìm xem có lá bài nào có thể đánh dc ko để show anim bốc bài
                if (!EndCard)
                {
                    var result = FindMatchCard(UserCards[0]);
                    if (result == null)
                    {
                        ObjController[21].SetActive(true);//Show anim bốc bài
                        ObjController[21].GetComponent<Animator>().SetTrigger("GetCard");
                    }

                    //Tự bốc bài nếu bật chức năng
                    if (DataUserController.User.UnoSettingFastGetCard && UserCards[0].Count > 0 && result == null)
                    {
                        GeneralFunctions(5);
                    }
                    ShowImgSupport();
                }
            }
            else
                ObjController[21].SetActive(false);

            ObjController[11].transform.position = ObjectPositionCountCard[CurentRound].transform.position;//Di chuyển nút loading
        }

        /// <summary>
        /// Trả về lượt người chơi tiếp theo
        /// </summary>
        /// <param name="thisRound"></param>
        /// <returns></returns>
        private int GetNextPlayer(int thisRound)
        {
            int result = CurentRound;
            result += IsLeftToRight ? 1 : -1;
            if (result < 0)
                result = TotalPlayer - 1;
            if (result > TotalPlayer - 1)
                result = 0;
            return result;
        }

        /// <summary>
        /// Đảo ngược hướng đánh
        /// </summary>
        private void ChangeCircle()
        {
            IsLeftToRight = !IsLeftToRight;
            SpeedRotationZ = -SpeedRotationZ;
            ObjController[1].transform.localScale = new Vector3(-ObjController[1].transform.localScale.x, ObjController[1].transform.localScale.y, ObjController[1].transform.localScale.z);
        }

        /// <summary>
        /// Cập nhật lại màu trên bàn
        /// </summary>
        private void UpdateColorRound(bool isUpdateColor)
        {
            try
            {
                if (isUpdateColor)
                    CurentColorType = LastCard.ColorType != 4 ? LastCard.ColorType : CurentColorType;
                ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
            }
            catch
            {
                print("Loi update color");
            }
        }

        /// <summary>
        /// Di chuyển thẻ bài tới vị trí người chơi
        /// </summary>
        private IEnumerator CreateAndMoveCard(int slotUser)
        {
            //Tạo card cho player
            //if (slotUser == 0)
            {
                ObjectsUserCard[slotUser].Clear();
                UserCards[slotUser].Clear();
                for (int i = 0; i < UnoCardSystem.StartCardNumber; i++)
                {
                    UserCards[slotUser].Add(UnoCardSystem.UnoCards[UnityEngine.Random.Range(0, TotalCardOriginal)]);
                    ObjectsUserCard[slotUser].Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), ObjController[2].transform.position, Quaternion.identity));
                    ObjectsUserCard[slotUser][i].transform.SetParent(ObjectGrpCardListPlayer[slotUser].transform, false);

                    if (IsDeveloper || (!IsDeveloper && slotUser == 0))
                        ObjectsUserCard[slotUser][i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][i].CardImg);
                    else
                        ObjectsUserCard[slotUser][i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == "UnoCard_74");

                    //Move card to player
                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][i], ObjController[2].transform.position, ObjectGrpCardListPlayer[slotUser].transform.position, UnoCardSystem.DelayTimeMoveCard, moveCurve));

                    //Đây là tạo mới cho nên chỉ cần chạy âm thanh với lượt phát bài của player là đủ
                    if (slotUser == 0)
                        StartCoroutine(GameSystem.PlaySound(SoundClip[0], 0));

                    yield return new WaitForSeconds(UnoCardSystem.DelayTimeCreateCard);
                }
                IsControl = true;
                StartCoroutine(AdjustCard(slotUser));
                ShowImgSupport();
            }
        }

        /// <summary>
        /// Tự động dãn cách quân bài
        /// </summary>
        private IEnumerator AdjustCard(int slotUser)
        {
            if (IsDeveloper || (!IsDeveloper && slotUser == 0))
            {
                SortCard(slotUser);

                var count = UserCards[slotUser].Count;
                var startLocation = count % 2 == 0 ? 0 : UnoCardSystem.RangeMinBetween2Card / 2;
                float mid = count / 2;
                for (int i = 0; i < count; i++)
                {
                    //Set index cho card
                    ObjectsUserCard[slotUser][i].transform.SetSiblingIndex(i);

                    //Gán chức năng click cho card
                    if (slotUser == 0)
                    {
                        var temp = i;
                        ObjectsUserCard[slotUser][i].GetComponent<Button>().onClick.RemoveAllListeners();
                        ObjectsUserCard[slotUser][i].GetComponent<Button>().onClick.AddListener(() => CardSelection(temp));
                    }

                    //Di chuyển card ra vị trí phù hợp
                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][i], ObjectsUserCard[slotUser][i].transform.position, new Vector2(ObjectGrpCardListPlayer[slotUser].transform.position.x - (mid - i * UnoCardSystem.RangeMinBetween2Card), ObjectGrpCardListPlayer[slotUser].transform.position.y), UnoCardSystem.DelayTimeMoveCard, moveCurve));
                }
            }
            yield return null;
            //SlotCardSelected = -1;
            TextCountCardPlayer[slotUser].text = UserCards[slotUser].Count.ToString();//Update số lượng thẻ bài player
        }

        /// <summary>
        /// Sắp xếp các lá bài theo trình tự màu sắc
        /// </summary>
        private void SortCard(int slotUser)
        {
            UserCards[slotUser] = UserCards[slotUser].OrderBy(x => x.ColorType).ThenBy(x => x.NumberID).ToList();

            var count = UserCards[slotUser].Count;
            for (int i = 0; i < count; i++)
            {
                ObjectsUserCard[slotUser][i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][i].CardImg);
            }
        }

        /// <summary>
        /// Rút thêm lá bài
        /// </summary>
        /// <param name="slot">Vị trí: 0 = player, 1,2,3 tương ứng với các vị trí của AI</param>
        /// <param name="quantity">Số lượng rút thêm</param>
        private IEnumerator GetCard(int slotUser, int quantity, bool isAuto)
        {
            //Bốc bài cho player chủ động
            if (!IsGetCard && !isAuto)
            {
                IsControl = false;
                var totalCardInList = UserCards[slotUser].Count;
                for (int i = 0; i < quantity; i++)
                {
                    UserCards[slotUser].Add(UnoCardSystem.UnoCards[UnityEngine.Random.Range(0, TotalCardOriginal)]);
                    ObjectsUserCard[slotUser].Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), ObjController[2].transform.position, Quaternion.identity));
                    ObjectsUserCard[slotUser][totalCardInList + i].transform.SetParent(ObjectGrpCardListPlayer[slotUser].transform, false);

                    if (IsDeveloper || (!IsDeveloper && slotUser == 0))
                        ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][totalCardInList + i].CardImg);
                    else
                        ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == "UnoCard_74");
                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjController[2].transform.position, UnoCardSystem.TimeDelayGetCard, moveCurve));
                    yield return new WaitForSeconds(UnoCardSystem.TimeDelayGetCard);

                    //Move card to player
                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjectGrpCardListPlayer[slotUser].transform.position, UnoCardSystem.DelayTimeMoveCard, moveCurve));
                    StartCoroutine(GameSystem.PlaySound(SoundClip[0], 0));

                    yield return new WaitForSeconds(UnoCardSystem.DelayTimeCreateCard);
                }

                if (slotUser == 0)
                    IsControl = true;
                StartCoroutine(AdjustCard(slotUser));

                //Player rút chủ động
                if (!isAuto && slotUser == 0)
                {
                    int? findCard = null;
                    if (DataUserController.User.UnoSettingFastPass)//Nếu bật chức năng bỏ lượt nhanh
                    {
                        findCard = FindMatchCard(UserCards[slotUser]);
                        if (findCard != null)
                        {
                            ObjController[16].SetActive(true);
                            IsGetCard = true;
                        }
                        else
                            GeneralFunctions(7);
                    }
                    else
                    {
                        ObjController[16].SetActive(true);
                        IsGetCard = true;
                    }
                    ShowImgSupport();
                }

                if (slotUser != 0 && !isAuto)
                {
                    StartCoroutine(ThrowCardToTable(slotUser, true));
                }
            }
            else if (isAuto)
            {
                IsControl = false;
                var totalCardInList = UserCards[slotUser].Count;
                for (int i = 0; i < quantity; i++)
                {
                    UserCards[slotUser].Add(UnoCardSystem.UnoCards[UnityEngine.Random.Range(0, TotalCardOriginal)]);
                    ObjectsUserCard[slotUser].Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), ObjController[2].transform.position, Quaternion.identity));
                    ObjectsUserCard[slotUser][totalCardInList + i].transform.SetParent(ObjectGrpCardListPlayer[slotUser].transform, false);

                    if (IsDeveloper || (!IsDeveloper && slotUser == 0))
                        ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][totalCardInList + i].CardImg);
                    else
                        ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == "UnoCard_74");

                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjController[2].transform.position, UnoCardSystem.TimeDelayGetCard, moveCurve));
                    yield return new WaitForSeconds(UnoCardSystem.TimeDelayGetCard);

                    //Move card to player
                    StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjectGrpCardListPlayer[slotUser].transform.position, UnoCardSystem.DelayTimeMoveCard, moveCurve));
                    StartCoroutine(GameSystem.PlaySound(SoundClip[0], 0));

                    yield return new WaitForSeconds(UnoCardSystem.DelayTimeMoveCard);
                }
                if (CurentRound == 0)
                    IsControl = true;
                StartCoroutine(AdjustCard(slotUser));
                if (!isAuto && slotUser == 0)
                {
                    ObjController[16].SetActive(true);
                    IsGetCard = true;
                }
                if (slotUser != 0 && !isAuto)
                {
                    StartCoroutine(ThrowCardToTable(slotUser, true));
                }
            }
        }

        /// <summary>
        /// Khởi tạo danh sách card được rút ra từ card bão tố
        /// </summary>
        /// <param name="slotUser"></param>
        /// <param name="tornadoColor"></param>
        /// <returns></returns>
        private List<UnoCardModel> StartTornadoCard(int slotUser, int tornadoColor)
        {
            TornadoColor = tornadoColor;
            var result = new List<UnoCardModel>();
            var i = 0;

        //Add card cho đối thủ
        BeginAddCard:
            result.Add(UnoCardSystem.UnoCards[UnityEngine.Random.Range(0, TotalCardOriginal)]);

            //Dừng thêm khi tìm thấy trùng màu hoặc lá bài đổi màu
            if (result[i].ColorType.Equals(TornadoColor) || result[i].ColorType.Equals(4))
            {
                return result;
            }
            else { i++; goto BeginAddCard; }
        }

        /// <summary>
        /// Chức năng lá bài lốc xoáy
        /// </summary>
        private IEnumerator GetTornadoCard(int slotUser, List<UnoCardModel> listCard)
        {
            //IsControl = false;
            var totalCardInList = UserCards[slotUser].Count;
            var count = listCard.Count;
            for (int i = 0; i < count; i++)
            {
                UserCards[slotUser].Add(listCard[i]);
                ObjectsUserCard[slotUser].Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), ObjController[2].transform.position, Quaternion.identity));
                ObjectsUserCard[slotUser][totalCardInList + i].transform.SetParent(ObjectGrpCardListPlayer[slotUser].transform, false);

                if (IsDeveloper || (!IsDeveloper && slotUser == 0))
                    ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][totalCardInList + i].CardImg);
                else
                    ObjectsUserCard[slotUser][totalCardInList + i].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == "UnoCard_74");

                StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjController[2].transform.position, UnoCardSystem.TimeDelayGetCard, moveCurve));
                yield return new WaitForSeconds(UnoCardSystem.TimeDelayGetCard);

                //Move cart to player
                StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[slotUser][totalCardInList + i], ObjController[2].transform.position, ObjectGrpCardListPlayer[slotUser].transform.position, UnoCardSystem.DelayTimeMoveCard, moveCurve));
                StartCoroutine(GameSystem.PlaySound(SoundClip[0], 0));


                yield return new WaitForSeconds(UnoCardSystem.DelayTimeMoveCard);
            }
            //IsControl = true;
            StartCoroutine(AdjustCard(slotUser));
        }

        /// <summary>
        /// Gán button cho card
        /// </summary>
        private void CardSelection(int slot)
        {
            if (IsControl && CurentRound == 0)
            {
                if (DataUserController.User.UnoSettingFastPush)//Chế độ đánh nhanh
                {
                    SlotCardSelected = slot;//Đổi vị trí card chọn
                    GeneralFunctions(6);
                }
                else//Chế độ chọn xong mới đánh
                {
                    if (SlotCardSelected != -1 && SlotCardSelected != slot)//Đẩy card chọn lên và hạ card đã chọn kia xuống
                    {
                        StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[0][slot], ObjectsUserCard[0][slot].transform.position, new Vector2(ObjectsUserCard[0][slot].transform.position.x, ObjectsUserCard[0][slot].transform.position.y + UnoCardSystem.RangeCardMoveUp), UnoCardSystem.DelayTimeMoveCard, moveCurve));
                        StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[0][SlotCardSelected], ObjectsUserCard[0][SlotCardSelected].transform.position, new Vector2(ObjectsUserCard[0][SlotCardSelected].transform.position.x, ObjController[3].transform.position.y), UnoCardSystem.DelayTimeMoveCard, moveCurve));
                        SlotCardSelected = slot;//Đổi vị trí card chọn
                    }
                    else if (SlotCardSelected == slot)//Hạ card xuống
                    {
                        StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[0][SlotCardSelected], ObjectsUserCard[0][SlotCardSelected].transform.position, new Vector2(ObjectsUserCard[0][SlotCardSelected].transform.position.x, ObjController[3].transform.position.y), UnoCardSystem.DelayTimeMoveCard, moveCurve));
                        SlotCardSelected = -1;//Đưa về trạng thái chưa chọn card nào
                    }
                    else if (SlotCardSelected == -1)
                    {
                        StartCoroutine(GameSystem.MoveObjectCurve(false, ObjectsUserCard[0][slot], ObjectsUserCard[0][slot].transform.position, new Vector2(ObjectsUserCard[0][slot].transform.position.x, ObjectsUserCard[0][slot].transform.position.y + UnoCardSystem.RangeCardMoveUp), UnoCardSystem.DelayTimeMoveCard, moveCurve));
                        SlotCardSelected = slot;//Đổi vị trí card chọn
                    }
                }
            }
        }

        /// <summary>
        /// Chờ xác nhận thao tác
        /// </summary>
        private IEnumerator WaitingForActions(int type)
        {
            switch (type)
            {
                case 0://Đóng UI confirmbox
                    //Chờ thao tác từ use
                    yield return new WaitUntil(() => GameSystem.ConfirmBoxResult != 0);

                    //Xác nhận đồng ý
                    if (GameSystem.ConfirmBoxResult == 1)
                    {
                        GameSystem.DisposePrefabUI(9);
                    }
                    break;
                case 1://Đóng UI GameEnding
                    yield return new WaitUntil(() => !ObjController[17].activeSelf);

                    //UI đã đóng
                    if (ObjController[17].activeSelf)
                    {
                        //GameSystem.DisposePrefabUI(9);
                    }
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Thả bài xuống bàn
        /// </summary>
        private IEnumerator ThrowCardToTable(int slotUser, bool isUpdateColor)
        {
            if (!IsStopGame)
            {
                float delayTime = 0f;
                CurentRound = slotUser;
                int? slotThrow = null;
                if (slotUser != 0)
                {
                    slotThrow = FindMatchCard(UserCards[slotUser]);
                    yield return new WaitForSeconds(UnoCardSystem.TimeDelayAIAction);

                    //Tìm thấy lá bài cuối cùng phù hợp để đánh
                    if (slotThrow != null && UserCards[slotUser].Count == 1)
                    {
                        EndCard = true;
                    }

                    //AI rút thêm bài nếu ko tìm thấy lá bài phù hợp trong list
                    if (slotThrow == null && !IsGetCard)
                    {
                        StartCoroutine(GetCard(slotUser, 1, false));
                        IsGetCard = true;
                        goto End;
                    }
                    else if (slotThrow == null && IsGetCard)//Nếu đã rút card nhưng vẫn ko đánh dc => next lượt
                    {
                        StartCoroutine(NextPlayer(1, delayTime));
                        IsGetCard = false;
                        if (CurentRound != 0)
                            StartCoroutine(ThrowCardToTable(CurentRound, true));
                        goto End;
                    }
                }

                //Xóa bớt bài dưới bàn nếu quá nhiều
                if (CardsShowed.Count > UnoCardSystem.MaxCardTemp)
                {
                    GameSystem.DisposeObjectCustom(CardsShowed[0], false);
                    CardsShowed.RemoveAt(0);
                }

                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) != 15)
                {
                    //Tạo lá bài tạm thời từ bộ bài để đẩy xuống bàn
                    CardsShowed.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), slotUser == 0 ? ObjectsUserCard[slotUser][SlotCardSelected].transform.position : ObjectGrpCardListPlayer[slotUser].transform.position, Quaternion.identity));
                    CardsShowed[CardsShowed.Count - 1].transform.SetParent(ObjController[5].transform, false);
                    //Di chuyển bài ra giữa
                    StartCoroutine(GameSystem.MoveObjectCurve(false, CardsShowed[CardsShowed.Count - 1], slotUser == 0 ? ObjectsUserCard[slotUser][SlotCardSelected].transform.position : ObjectGrpCardListPlayer[slotUser].transform.position, Vector3.zero, UnoCardSystem.DelayTimeMoveCard, moveCurve));
                    //Chạy âm thanh
                    StartCoroutine(GameSystem.PlaySound(SoundClip[1], 0));
                    //Hiệu ứng xoay bài
                    StartCoroutine(GameSystem.RotationObject(false, CardsShowed[CardsShowed.Count - 1], slotUser == 0 ? ObjectsUserCard[slotUser][SlotCardSelected].transform.eulerAngles : ObjectGrpCardListPlayer[slotUser].transform.eulerAngles, new Vector3(0, 0, UnoCardSystem.CardAnglePos[UnityEngine.Random.Range(0, UnoCardSystem.CardAnglePos.Length)]), UnoCardSystem.DelayTimeMoveCard));
                    //Hiệu ứng scale bài
                    StartCoroutine(GameSystem.ScaleUI(1, CardsShowed[CardsShowed.Count - 1], UnoCardSystem.SizeToTable, UnoCardSystem.DelayTimeMoveCard));
                    //Gán hình cho lá bài dưới bàn
                    CardsShowed[CardsShowed.Count - 1].GetComponent<Image>().sprite = slotUser == 0 ? ObjectsUserCard[slotUser][SlotCardSelected].GetComponent<Image>().sprite : (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][(int)slotThrow].CardImg);
                    //Gán lá bài trên cùng dưới bàn
                    LastCard = slotUser == 0 ? UserCards[slotUser][SlotCardSelected] : UserCards[slotUser][(int)slotThrow];
                }

                //Update lại màu
                if (slotUser == 0)
                {
                    if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 15)
                        UpdateColorRound(false);
                    else
                        UpdateColorRound(isUpdateColor);
                }
                else
                {
                    //Nếu là thẻ đổi màu => tìm màu có số lượng lớn nhất và đổi thành màu đó
                    if (UserCards[slotUser][(int)slotThrow].ColorType.Equals(4))
                    {
                        CurentColorType = GameSystem.FindSlotNumberHighest(new int[] { UserCards[slotUser].Count(x => x.ColorType.Equals(0)), UserCards[slotUser].Count(x => x.ColorType.Equals(1)), UserCards[slotUser].Count(x => x.ColorType.Equals(2)) });
                        UpdateColorRound(false);
                    }
                    else//Update màu ở lá bài cuối cùng trên bàn
                    {
                        if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 15)
                            UpdateColorRound(false);
                        else
                            UpdateColorRound(true);
                    }
                }
                //Update lại vòng quay nếu như đánh con đổi vòng
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 11)
                {
                    ChangeCircle();
                }


                //Lá bài +2
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 12)
                {
                    StartCoroutine(GetCard(GetNextPlayer(CurentRound), 2, true));
                    delayTime = UnoCardSystem.TimeDelayGetCard * 3;
                }
                //Lá bài bão tố
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 14)
                {
                    var createTornado = StartTornadoCard(GetNextPlayer(CurentRound), (slotUser == 0 ? UserCards[slotUser][SlotCardSelected].ColorType : UserCards[slotUser][(int)slotThrow].ColorType));
                    StartCoroutine(GetTornadoCard(GetNextPlayer(CurentRound), createTornado));
                    delayTime = (UnoCardSystem.TimeDelayGetCard + UnoCardSystem.DelayTimeMoveCard) * (createTornado.Count + 1);
                    StartCoroutine(NextPlayer(2, delayTime));
                    goto BaoTo;
                }
                //Xả bài
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 15)
                {
                    var count = UserCards[slotUser].Count(x => x.ColorType.Equals(slotUser == 0 ? UserCards[slotUser][SlotCardSelected].ColorType : UserCards[slotUser][(int)slotThrow].ColorType));
                    StartCoroutine(ThrowAllCardByColor(slotUser, (slotUser == 0 ? UserCards[slotUser][SlotCardSelected].ColorType : UserCards[slotUser][(int)slotThrow].ColorType)));
                    delayTime = (UnoCardSystem.DelayTimeMoveCard) * (count + 1);
                    StartCoroutine(NextPlayer(1, delayTime));
                    goto XaBai;
                }
                //Lá bài +4
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 18)
                {
                    StartCoroutine(GetCard(GetNextPlayer(CurentRound), 4, true));
                    delayTime = (UnoCardSystem.TimeDelayGetCard + UnoCardSystem.DelayTimeCreateCard) * 5;
                }
                //Lá bài +4 mục tiêu
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 20)
                {
                    //Tìm player có số card nhỏ nhất để phang
                    if (slotUser != 0)
                    {
                        if (TotalPlayer == 2)
                            SlotVictim = 0;
                        else
                        {
                            SlotVictim = 0;
                            for (int i = 0; i < TotalPlayer; i++)
                            {
                                if (i != slotUser)
                                {
                                    if (UserCards[SlotVictim].Count > UserCards[i].Count)
                                        SlotVictim = i;
                                }
                            }
                        }
                    }

                    StartCoroutine(GetCard(SlotVictim, 4, true));
                    delayTime = (UnoCardSystem.TimeDelayGetCard + UnoCardSystem.DelayTimeCreateCard) * 5;
                }
                //Lá bài + ngẫu nhiên
                if ((slotUser == 0 ? UserCards[slotUser][SlotCardSelected].NumberID : UserCards[slotUser][(int)slotThrow].NumberID) == 21)
                {
                    var quantityCardGet = UnityEngine.Random.Range(UnoCardSystem.MinRandomPustCardToVictim, UnoCardSystem.MaxRandomPustCardToVictim + 1);
                    StartCoroutine(GetCard(GetNextPlayer(CurentRound), quantityCardGet, true));
                    delayTime = (UnoCardSystem.TimeDelayGetCard + UnoCardSystem.DelayTimeCreateCard) * (quantityCardGet + 1);
                }
                if (slotUser == 0)
                {
                    switch (UserCards[slotUser][SlotCardSelected].NumberID)
                    {
                        case 10://Cấm lượt
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 11://Đổi chiều
                            if (TotalPlayer == 2)
                                StartCoroutine(NextPlayer(2, delayTime));
                            else
                                StartCoroutine(NextPlayer(1, delayTime));
                            break;
                        case 12://+2
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 13://Next 2 lượt đối với quân bài cấm lượt X2 (Nếu chơi chế độ 2 người thì next 1 lần)
                            if (TotalPlayer != 2)
                                StartCoroutine(NextPlayer(3, delayTime));
                            else
                                StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 18://Đổi màu +4
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 20://Đổi màu +4 chỉ định
                            CurentRound = SlotVictim;
                            StartCoroutine(NextPlayer(1, delayTime));
                            break;
                        case 21://Đổi màu + ngẫu nhiên
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        default:
                            StartCoroutine(NextPlayer(1, delayTime));
                            break;
                    }
                }
                else
                {
                    switch (UserCards[slotUser][(int)slotThrow].NumberID)
                    {
                        case 10://Cấm lượt
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 11://Đổi chiều
                            if (TotalPlayer == 2)
                                StartCoroutine(NextPlayer(2, delayTime));
                            else
                                StartCoroutine(NextPlayer(1, delayTime));
                            break;
                        case 12://+2
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 13://Next 2 lượt đối với quân bài cấm lượt X2 (Nếu chơi chế độ 2 người thì next 1 lần)
                            if (TotalPlayer != 2)
                                StartCoroutine(NextPlayer(3, delayTime));
                            else
                                StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 18://Đổi màu +4
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        case 20://Đổi màu +4 chỉ định
                            CurentRound = SlotVictim;
                            StartCoroutine(NextPlayer(1, delayTime));
                            break;
                        case 21://Đổi màu + ngẫu nhiên
                            StartCoroutine(NextPlayer(2, delayTime));
                            break;
                        default:
                            StartCoroutine(NextPlayer(1, delayTime));
                            break;
                    }
                }


            BaoTo:
                //Loại bỏ lá bài ra khỏi danh sách
                GameSystem.DisposeObjectCustom(slotUser == 0 ? ObjectsUserCard[slotUser][SlotCardSelected] : ObjectsUserCard[slotUser][(int)slotThrow], false);
                ObjectsUserCard[slotUser].RemoveAt(slotUser == 0 ? SlotCardSelected : (int)slotThrow);
                UserCards[slotUser].RemoveAt(slotUser == 0 ? SlotCardSelected : (int)slotThrow);

                SlotCardSelected = -1;
                StartCoroutine(AdjustCard(slotUser));

            //Xả bài
            XaBai:
                yield return new WaitForSeconds(delayTime);

                //Kết thúc ván bài nếu 1 người chơi hết bài trong list
                if (UserCards[slotUser].Count <= 0)
                {
                    IsStopGame = true;
                    SlotPlyerWinner = slotUser;
                    yield return new WaitForSeconds(UnoCardSystem.TimeDelayGameEnding);
                    StartCoroutine(GameEnding());
                    goto End;
                }

                //Next nếu lượt vừa rồi là AI
                if (CurentRound != 0)
                    StartCoroutine(ThrowCardToTable(CurentRound, true));

                //Ẩn nút bỏ lượt nếu player đã đánh
                if (slotUser == 0)
                    ObjController[16].SetActive(false);

            }
        End:
            yield return null;
        }

        /// <summary>
        /// Tổng kết điểm khi kết thúc ván
        /// </summary>
        private void CalculatorPoint()
        {
            RankingPoint = new Dictionary<int, int>();

            if (RankingPoint == null)
                RankingPoint = new Dictionary<int, int>();

            RankingPoint.Clear();
            for (int i = 0; i < TotalPlayer; i++)
            {
                RankingPoint.Add(i, UserCards[i].Sum(x => x.CardPoint));
            }
        }

        /// <summary>
        /// Endgame
        /// </summary>
        private IEnumerator GameEnding()
        {
            CalculatorPoint();

            //Cộng tiền nếu win
            if (SlotPlyerWinner == 0)
            {
                UserSystem.IncreaseGolds(UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot] * TotalPlayer);
                DataUserController.SaveUserInfor();
            }

            IsControl = false;
            StartCoroutine(GameSystem.PlaySound(SoundClip[5], 0));//Âm thanh kết thúc

            //Hiển thị UI thống kê
            TextUI[9].text = Languages.lang[316];// = "Xếp hạng";
            ObjController[17].SetActive(true);
            if (ObjectRanking == null)
                ObjectRanking = new List<GameObject>();

            var listPoint = from entry in RankingPoint orderby entry.Value ascending select entry;

            var objectHeight = 0f;
            for (int i = 0; i < TotalPlayer; i++)
            {
                //Tạo object ranking
                ObjectRanking.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCardRankGameEnding"), Vector3.zero, Quaternion.identity));
                ObjectRanking[i].transform.SetParent(ObjController[17].transform, false);
                //Tạo dữ liệu object ranking
                ObjectRanking[i].GetComponent<UnoCardRankGameEnding>().Initialize((i + 1).ToString(), listPoint.ElementAt(i).Key == 0 ? DataUserController.User.UserName : (Languages.lang[321] + (listPoint.ElementAt(i).Key + 1)), i == 0 ? (UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot] * TotalPlayer).ToString() : (0 - UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot]).ToString());
                objectHeight = objectHeight == 0 ? ObjectRanking[0].GetComponent<RectTransform>().sizeDelta.y : objectHeight;
                ObjectRanking[i].transform.localPosition = new Vector2(0, (objectHeight * 3));
            }

            //Hiệu ứng di chuyển object ranking
            objectHeight = ObjectRanking[0].GetComponent<RectTransform>().sizeDelta.y;
            var count = ObjectRanking.Count;
            float mid = count / 2;
            for (int i = 0; i < count; i++)
            {
                //Di chuyển object ranking
                StartCoroutine(GameSystem.MoveObjectCurve(true, ObjectRanking[i], ObjectRanking[i].transform.localPosition, new Vector2(ObjectRanking[i].transform.localPosition.x, ObjectRanking[i].transform.localPosition.y - (objectHeight * i)), UnoCardSystem.TimeDelayAIAction, moveCurve));
            }
            yield return new WaitForSeconds(UnoCardSystem.TimeDelayAIAction);
            IsControl = true;
            yield return null;
        }

        /// <summary>
        /// Xả hết bài có cùng màu xuống bàn
        /// </summary>
        private IEnumerator ThrowAllCardByColor(int slotUser, int colorType)
        {
            IsControl = false;
            var result = UserCards[slotUser].Select((c, i) => new { Item = c, Index = i }).Where(x => x.Item.ColorType.Equals(colorType)).Select(x => x.Index).ToList();
            var count = result.Count;
            for (int i = count - 1; i >= 0; i--)
            {

                //Xóa bớt bài dưới bàn nếu quá nhiều
                if (CardsShowed.Count > UnoCardSystem.MaxCardTemp)
                {
                    GameSystem.DisposeObjectCustom(CardsShowed[0], false);
                    CardsShowed.RemoveAt(0);
                }

                //Tạo lá bài tạm thời từ bộ bài để đẩy xuống bàn
                CardsShowed.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCard"), ObjectGrpCardListPlayer[slotUser].transform.position, Quaternion.identity));
                CardsShowed[CardsShowed.Count - 1].transform.SetParent(ObjController[5].transform, false);
                //Di chuyển bài ra giữa
                StartCoroutine(GameSystem.MoveObjectCurve(false, CardsShowed[CardsShowed.Count - 1], ObjectGrpCardListPlayer[slotUser].transform.position, Vector3.zero, UnoCardSystem.DelayTimeMoveCard, moveCurve));
                StartCoroutine(GameSystem.PlaySound(SoundClip[0], 0));
                //Hiệu ứng xoay bài
                StartCoroutine(GameSystem.RotationObject(false, CardsShowed[CardsShowed.Count - 1], ObjectGrpCardListPlayer[slotUser].transform.eulerAngles, new Vector3(0, 0, UnoCardSystem.CardAnglePos[UnityEngine.Random.Range(0, UnoCardSystem.CardAnglePos.Length)]), UnoCardSystem.DelayTimeMoveCard));
                //Hiệu ứng scale bài
                StartCoroutine(GameSystem.ScaleUI(1, CardsShowed[CardsShowed.Count - 1], UnoCardSystem.SizeToTable, UnoCardSystem.DelayTimeMoveCard));
                //Gán hình cho lá bài dưới bàn
                CardsShowed[CardsShowed.Count - 1].GetComponent<Image>().sprite = (Sprite)SpritesCardImg.FirstOrDefault(x => x.name == UserCards[slotUser][result[i]].CardImg);
                //Gán lá bài trên cùng dưới bàn
                LastCard = UserCards[slotUser][result[i]];
                //Loại bỏ lá bài ra khỏi danh sách
                GameSystem.DisposeObjectCustom(ObjectsUserCard[slotUser][result[i]], false);
                ObjectsUserCard[slotUser].RemoveAt(result[i]);
                UserCards[slotUser].RemoveAt(result[i]);
                SlotCardSelected = -1;
                yield return new WaitForSeconds(UnoCardSystem.DelayTimeMoveCard);
            }
            UpdateColorRound(true);
            StartCoroutine(AdjustCard(slotUser));
            if (slotUser == 0)
                IsControl = true;
        }

        /// <summary>
        /// Lựa chọn màu sắc của card đổi màu
        /// </summary>
        /// <param name="colorType"></param>
        public void SelectColorForCardSpecial(int colorType)
        {
            if (IsControl)
            {
                if (!Card4PlusVictim)//Lá bài đổi màu +4
                {
                    CurentColorType = colorType;
                    ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
                    StartCoroutine(ThrowCardToTable(0, false));
                    ObjController[4].SetActive(false);
                }
                else//Lá bài +4 chọn mục tiêu
                {
                    //Nếu chỉ có 2 người chơi => mặc định đẩy bài sang đối phương, không cần chọn
                    if (TotalPlayer == 2)
                    {
                        SlotVictim = 1;
                        CurentColorType = colorType;
                        ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
                        StartCoroutine(ThrowCardToTable(0, false));
                        ObjController[4].SetActive(false);
                        Card4PlusVictim = false;
                    }
                    else
                    {
                        CurentColorType = colorType;
                        ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
                        ObjController[20].SetActive(true);
                        for (int i = 0; i < TotalPlayer; i++)
                        {
                            ObjectPositionPlayer[i].transform.GetChild(1).gameObject.SetActive(true);
                        }
                        ObjController[4].SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Chọn mục tiêu của lá bài +4 vào mục tiêu
        /// </summary>
        public void SelectVictimForCard4Plus(int slotUser)
        {
            SlotVictim = slotUser;
            ObjController[1].GetComponent<Image>().color = UnoCardSystem.ClrsCard[CurentColorType];
            StartCoroutine(ThrowCardToTable(0, false));
            ObjController[4].SetActive(false);

            //Ẩn UI
            ObjController[20].SetActive(false);
            for (int i = 1; i < TotalPlayer; i++)
            {
                ObjectPositionPlayer[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            Card4PlusVictim = false;
        }

        /// <summary>
        /// Valid data
        /// </summary>
        private bool Validate(int slotUser)
        {
            if (SlotCardSelected == -1)
            {
                GameSystem.ControlFunctions.ShowMessage(Languages.lang[307]);// = "Bạn cần chọn 1 lá bài hoặc bỏ lượt";
                StartCoroutine(GameSystem.PlaySound(SoundClip[2], 0));
                return false;
            }
            if (UserCards[slotUser][SlotCardSelected].ColorType != 4 && UserCards[slotUser][SlotCardSelected].ColorType != CurentColorType && LastCard.NumberID != UserCards[slotUser][SlotCardSelected].NumberID)
            {
                GameSystem.ControlFunctions.ShowMessage(Languages.lang[308]);// = "Lá bài không phù hợp";
                StartCoroutine(GameSystem.PlaySound(SoundClip[2], 0));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Chờ thao tác từ player
        /// </summary>
        private IEnumerator WaitingForAction(int type)
        {
            switch (type)
            {
                case 0://Chờ đóng UI setting
                    yield return new WaitUntil(() => !ObjController[9].activeSelf);
                    if (!ObjController[9].activeSelf)
                    {
                        LoadSetting();
                    }
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Chức năng tổng
        /// </summary>
        public void GeneralFunctions(int type)
        {
            switch (type)
            {
                case 0://Đóng UI
                    GameSystem.ShowConfirmDialog(Languages.lang[306]);
                    StartCoroutine(WaitingForActions(type));
                    break;
                case 1://Introduction
                    GameSystem.InitializePrefabUI(10);
                    break;
                case 2://Khởi tạo ván bài mới
                    if (IsControl)
                    {
                        //Trừ tiền cược khi bắt đầu ván chơi
                        if (UserSystem.CheckGolds(UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot]))
                        {
                            ResetVariables();
                            SetupNewBattle();
                            ObjController[17].SetActive(false);
                            UserSystem.DecreaseGolds(UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot]);
                            DataUserController.SaveUserInfor();
                        }
                    }
                    break;
                case 3://Đóng UI lựa chọn màu
                    if (IsControl)
                        ObjController[4].SetActive(false);
                    break;
                case 4://Mở UI tùy chọn cách chơi
                    ObjController[9].SetActive(true);
                    StartCoroutine(WaitingForAction(0));
                    break;
                case 5://Rút thêm thẻ bài
                    if (IsControl && !IsGetCard)
                    {
                        ObjController[21].SetActive(false);//Ẩn anim bốc bài
                        StartCoroutine(GetCard(0, 1, false));
                    }
                    break;
                case 6://Đánh quân bài đang chọn
                    if (IsControl && CurentRound == 0)
                    {
                        if (Validate(0))
                        {
                            //Lựa chọn bài đổi màu
                            if (UserCards[0][SlotCardSelected].ColorType == 4)
                            {
                                if (UserCards[0][SlotCardSelected].NumberID == 20)//Lá bài +4 chọn mục tiêu
                                    Card4PlusVictim = true;
                                ObjController[4].SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(ThrowCardToTable(0, true));
                            }
                        }
                    }
                    break;
                case 7://Bỏ vòng
                    StartCoroutine(NextPlayer(1, 0));
                    StartCoroutine(ThrowCardToTable(CurentRound, true));
                    SlotCardSelected = -1;
                    ObjController[16].SetActive(false);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Tìm card phù hợp để chơi
        /// </summary>
        /// <param name="listCard"></param>
        /// <returns></returns>
        private int? FindMatchCard(List<UnoCardModel> listCard)
        {
            var result = listCard.LastOrDefault(x => x.ColorType.Equals(CurentColorType));
            if (result == null)
                result = listCard.LastOrDefault(x => x.NumberID.Equals(LastCard.NumberID));
            if (result == null)
                result = listCard.LastOrDefault(x => x.ColorType.Equals(4));
            if (result == null)
                return null;
            else return listCard.IndexOf(result);
        }

        #endregion

        #region Events

        /// <summary>
        /// Thay đổi số tiền cược
        /// </summary>
        public void ChangeBetMoney(int valuePlus)
        {
            UnoCardSystem.BetLevelSlot += valuePlus;
            if (UnoCardSystem.BetLevelSlot < 0)
                UnoCardSystem.BetLevelSlot = UnoCardSystem.BetLevel.Length - 1;
            if (UnoCardSystem.BetLevelSlot > UnoCardSystem.BetLevel.Length - 1)
                UnoCardSystem.BetLevelSlot = 0;
            TextUI[1].text = UnoCardSystem.BetLevel[UnoCardSystem.BetLevelSlot].ToString();//Số tiền cược
        }

        /// <summary>
        /// Thay đổi số lượng người chơi
        /// </summary>
        public void ChangeQuantityPlayers(int valuePlus)
        {
            UnoCardSystem.PlayerNumberSlot += valuePlus;
            if (UnoCardSystem.PlayerNumberSlot < 0)
                UnoCardSystem.PlayerNumberSlot = UnoCardSystem.PlayersNumber.Length - 1;
            if (UnoCardSystem.PlayerNumberSlot > UnoCardSystem.PlayersNumber.Length - 1)
                UnoCardSystem.PlayerNumberSlot = 0;
            TextUI[2].text = UnoCardSystem.PlayersNumber[UnoCardSystem.PlayerNumberSlot].ToString();//Số người chơi
        }
        #endregion
    }
}
