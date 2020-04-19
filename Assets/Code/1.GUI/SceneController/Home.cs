using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    #region Variables
    public GameObject[] ObjHome;
    public Text[] TextLanguage;
    private bool BtnFunctionIsShow = true;
    private bool BtnFunctionIsLeft = true;
    [Header("Draw Curve")]
    public AnimationCurve moveCurve;
    public GameObject[] ListButtonToMove; //Danh sách các button để thực hiện move
    private Vector2[] ListButtonToMoveVec; //Danh sách tọa độ các button để thực hiện move
    private GameObject[] HeroAvtInHome; //Danh sách hero hiển thị ngoài home 
    #endregion

    #region Initialize

    void Start()
    {
        //GameSystem.GenerateSecurity (0);
        GameSystem.AnimCurve = moveCurve;
        DataUserController.LoadAll();
        Languages.SetupDefaultLanguage();
        Module.PrevScene = "Home";
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
        StartCoroutine(API.SetupServer());
        GameSystem.RunBGM(0);
        SetupLanguage();
        SetupVectorButtonFunctions();
        SetupShowHeroInTeam();

        if(string.IsNullOrEmpty(DataUserController.User.UserName))
        {
            HiddenOrShowEffect(false);
            GameSystem.InitializePrefabUI(12);
            StartCoroutine(WaitingCloseUI(12));
        }
        //RunTutorial();
    }

    /// <summary>
    /// Thiết lập ngôn ngữ
    /// </summary>
    private void SetupLanguage()
    {
        TextLanguage[0].text = Languages.lang[119];
        TextLanguage[1].text = Languages.lang[184];
        TextLanguage[2].text = string.Format("{0:#,#}", DataUserController.User.Golds); //Vang
        TextLanguage[3].text = string.Format("{0:#,#}", DataUserController.User.Gems); //Kim cuong
        TextLanguage[5].text = Languages.lang[271]; //Spin
        TextLanguage[6].text = Languages.lang[269]; //Break ball
        TextLanguage[7].text = Languages.lang[145]; // = "Chế tạo";
        TextLanguage[8].text = Languages.lang[274]; // = "Túi đồ";
        TextLanguage[9].text = Languages.lang[146]; // = "Nhân vật";
        TextLanguage[10].text = Languages.lang[297]; // = "Khảm ngọc";
        TextLanguage[11].text = Languages.lang[322]; // = "card game";
        TextLanguage[12].text = Languages.lang[305]; // = "Tháp địa ngục";
    }

    /// <summary>
    /// Hiển thị các hero trong team ra home
    /// </summary>
    private void SetupShowHeroInTeam()
    {
        HeroAvtInHome = new GameObject[3]; //
        string[] temp = DataUserController.Team.Split(';');
        var count = DataUserController.Heroes.DBHeroes.Count;
        if (temp[0] != "0")
        {
            HeroAvtInHome[0] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/HeroAvt/Hero" + temp[0]), new Vector3(-3.85f, -3f, 0), Quaternion.identity);
            HeroAvtInHome[0].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (temp[1] != "0")
        {
            HeroAvtInHome[1] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/HeroAvt/Hero" + temp[1]), new Vector3(2.65f, -3f, 0), Quaternion.identity);
            HeroAvtInHome[1].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (temp[2] != "0")
        {
            HeroAvtInHome[2] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/HeroAvt/Hero" + temp[2]), new Vector3(8.7f, -3f, 0), Quaternion.identity);
            HeroAvtInHome[2].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }

    /// <summary>
    /// Thiết lập danh sách tọa độ các button function để thực hiện hiệu ứng move
    /// </summary>
    private void SetupVectorButtonFunctions()
    {
        ListButtonToMoveVec = new Vector2[ListButtonToMove.Length];
        for (int i = 0; i < ListButtonToMove.Length; i++)
        {
            ListButtonToMoveVec[i] = ListButtonToMove[i].GetComponent<RectTransform>().anchoredPosition;
        }
    }

    #endregion

    #region Functions

    // Update is called once per frame
    // void Update () {

    // }

    /// <summary>
    /// Ẩn hoặc hiển thị các hiệu ứng của map khi hiển thị các UI khác
    /// </summary>
    /// <param name="type">0: ẩn, 1: hiển thị</param>
    public void HiddenOrShowEffect(bool type)
    {
        ObjHome[4].SetActive(type);
        ObjHome[5].SetActive(type);
        ObjHome[6].SetActive(type);
        ObjHome[7].SetActive(type);
        ObjHome[8].SetActive(type);
        ObjHome[9].SetActive(type);
        ObjHome[10].SetActive(type);
        ObjHome[11].SetActive(type);
        ObjHome[12].SetActive(type);
        ObjHome[13].SetActive(type);
        ObjHome[0].GetComponent<ScreenReflection>().enabled = type;
    }

    /// <summary>
    /// Các chức năng của các button
    /// </summary>
    /// <param name="type">0: hiển thị UI setting, 1: đóng các UI, kèm theo enable hiệu ứng, 2: hiển thị UI chatbox</param>
    public void ButtonFunctions(int type)
    {
        switch (type)
        {
            case -1://Test
                StartCoroutine(API.APIPut("http://localhost:12345/home/44", null));
                break;
            case 0: //Hiển thị UI setting
                //ObjHome[1].SetActive(true);
                //HiddenOrShowEffect(false);

                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(11);
                StartCoroutine(WaitingCloseUI(11)); //Chờ đóng UI
                break;
            case 1: //đóng UI setting, kèm theo enable hiệu ứng 
                HiddenOrShowEffect(true);
                TextLanguage[2].text = string.Format("{0:#,#}", DataUserController.User.Golds); //Vang
                TextLanguage[3].text = string.Format("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            case 2: //Hiển thị UI chatbox
                ObjHome[2].SetActive(true);
                HiddenOrShowEffect(false);
                break;
            case 3: //Hiển thị UI xem video nhận vàng
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(3);
                StartCoroutine(WaitingCloseUI(3)); //Chờ đóng UI
                break;
            case 4: //Hiển thị UI Spin
                ObjHome[14].SetActive(true);
                HiddenOrShowEffect(false);
                break;
            case 5: //Hiển thị các button function
                BtnFunctionIsShow = !BtnFunctionIsShow;
                // ObjHome[17].SetActive (BtnFunctionIsShow);
                ObjHome[16].GetComponent<RectTransform>().localScale = new Vector3(1, (BtnFunctionIsShow ? -1 : 1), 1);
                //if (!BtnFunctionIsShow) {
                StartCoroutine(MoveButtonFunction(BtnFunctionIsShow, ObjHome[16].GetComponent<RectTransform>().anchoredPosition, .1f));
                //BtnFunctionIsShow = true;
                //}
                break;
            case 6: //Hiển thị UI Break ball
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(0);
                StartCoroutine(WaitingCloseUI(0)); //Chờ đóng UI
                break;
            case 7: //Hiển thị UI Inventory
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(1);
                StartCoroutine(WaitingCloseUI(1)); //Chờ đóng UI
                break;
            case 8: //Refresh lại giá trị tiền tệ
                TextLanguage[2].text = string.Format("{0:#,#}", DataUserController.User.Golds); //Vang
                TextLanguage[3].text = string.Format("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            case 9: //Hiển thị UI Hero
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(2);
                StartCoroutine(WaitingCloseUI(2)); //Chờ đóng UI
                break;
            case 10: //Hiển thị UI Craft
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(5);
                StartCoroutine(WaitingCloseUI(5)); //Chờ đóng UI
                break;
            case 11: //Hiển thị UI khảm socket
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(8);
                StartCoroutine(WaitingCloseUI(8)); //Chờ đóng UI
                break;
            case 12: //Hiển thị UI Tower
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(7);
                StartCoroutine(WaitingCloseUI(7)); //Chờ đóng UI
                break;
            case 13: //Hiển thị UI Uno Card
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(9);
                StartCoroutine(WaitingCloseUI(9)); //Chờ đóng UI
                break;
            case 14: //Hiển thị UI Vote
                HiddenOrShowEffect(false);
                GameSystem.InitializePrefabUI(13);
                StartCoroutine(WaitingCloseUI(13)); //Chờ đóng UI
                break;
            default:
                break;
        }
    }

    #region Waiting for close UI (Chờ giao diện đóng lại)

    private IEnumerator WaitingCloseUI(int type)
    {
        switch (type)
        {
            case 0://BreakBall
                yield return new WaitUntil(() => GameSystem.BreakBallUI == null);
                //Success
                if (GameSystem.BreakBallUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 1://Inventory
                yield return new WaitUntil(() => GameSystem.InventoryUI == null);
                if (GameSystem.InventoryUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 2://Hero
                yield return new WaitUntil(() => GameSystem.HeroUserUI == null);
                if (GameSystem.HeroUserUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 3://Gold Gems UI
                yield return new WaitUntil(() => GameSystem.VideoRewardGoldGemUI == null);
                //Success
                if (GameSystem.VideoRewardGoldGemUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 5://Craft
                yield return new WaitUntil(() => GameSystem.CraftUI == null);
                if (GameSystem.CraftUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 8://Socket system
                yield return new WaitUntil(() => GameSystem.InsertSocketCanvasUI == null);
                if (GameSystem.InsertSocketCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 7://Hell Tower
                yield return new WaitUntil(() => GameSystem.TowerCanvasUI == null);
                if (GameSystem.TowerCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 9://Uno Card
                yield return new WaitUntil(() => GameSystem.UnoCardCanvasUI == null);
                if (GameSystem.UnoCardCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 11://SettingCanvasUI
                yield return new WaitUntil(() => GameSystem.SettingCanvasUI == null);
                if (GameSystem.SettingCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                    SetupLanguage();
                }
                break;
            case 12://InputNameCanvasUI
                yield return new WaitUntil(() => GameSystem.InputNameCanvasUI == null);
                if (GameSystem.InputNameCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
            case 13://VoteFunctionCanvasUI
                yield return new WaitUntil(() => GameSystem.VoteFunctionCanvasUI == null);
                if (GameSystem.VoteFunctionCanvasUI == null)
                {
                    HiddenOrShowEffect(true);
                }
                break;
        }
        ButtonFunctions(8); //Refresh lại giá trị tiền tệ
    }

    #endregion

    /// <summary>
    /// Di chuyển kéo ra vào group button functions
    /// </summary>
    /// <param name="isExpand">true = kéo ra, false = gộp vào</param>
    /// <param name="originPos">Tọa độ gốc</param>
    /// <param name="duration">Thời gian di chuyển</param>
    /// <returns></returns>
    private IEnumerator MoveButtonFunction(bool isExpand, Vector2 originPos, float duration)
    {
        RectTransform[] rectTemp = new RectTransform[ListButtonToMove.Length];
        for (int i = 0; i < ListButtonToMove.Length; i++)
        {
            rectTemp[i] = ListButtonToMove[i].GetComponent<RectTransform>();
            if (isExpand)
                ListButtonToMove[i].SetActive(isExpand);
        }
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            for (int i = 0; i < ListButtonToMove.Length; i++)
                rectTemp[i].anchoredPosition = Vector2.Lerp(isExpand ? originPos : ListButtonToMoveVec[i], isExpand ? ListButtonToMoveVec[i] : originPos, moveCurve.Evaluate(time));
            yield return 0;
        }
        for (int i = 0; i < ListButtonToMove.Length; i++)
        {
            rectTemp[i].anchoredPosition = isExpand ? ListButtonToMoveVec[i] : originPos;
            ListButtonToMove[i].SetActive(isExpand);
        }
    }

    #endregion

}