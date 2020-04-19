using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeroUserController : MonoBehaviour {

    #region Hero System 
    public GameObject[] ObjectController;
    private bool[] HeroFilter;
    private float[] PositionButtonHeroFilter; //Tọa độ x của thanh kéo ra vào lọc item xem theo loại, mảng thứ 0 = tọa độ ban đầu, mảng thứ 1 là khoảng cách sẽ dịch chuyển, mảng 2 là tốc độ dịch chuyển
    private List<GameObject> Hero;
    [Header ("Draw Curve")]
    public AnimationCurve moveCurve;
    private List<Image> HeroAvt;
    private List<Vector2> ListVectorObjectHero; //Tọa độ các avt hero trong list
    List<HeroesProperties> HeroListTemplate = new List<HeroesProperties> ();
    private int HorizontalQuantityEquip = 6; //Số slot trên 1 hàng ngang của hệ thống trang bị vật phẩm cho hero
    private List<GameObject> ListBackgroundItemEquip = new List<GameObject> ();
    private List<GameObject> ListItemEquip = new List<GameObject> ();
    private ItemModel ItemViewing;
    public Text[] TextUI; //Set in interface
    private int HorizontalQuantity = 13; //Số slot trên 1 hàng ngang (được tính toán lại ở hàm SetupInventory phía dưới, vì nó scale theo tỉ lệ màn hình mỗi thiết bị)
    private int? ViewingItemType; //Kiểu xem item trang bị: null: xem trong inventory, 0: xem trong chi tiết nhân vật, item chưa trang bị, 1: xem trong chi tiết nhân vật, item đã được trang bị cho hero
    private Image ItemDetailColor; //Hình nền phía sau của item, thể hiện màu item, chi tiết item
    private int ItemSlotViewing = -1; //Slot item đang được xem, dùng để bán hoặc xem item tiếp theo

    #endregion

    void Start () {
        GlobalVariables.HeroSlotListSelected = new List<int> ();
        HeroFilter = new bool[6]; //6 Class
        for (int i = 0; i < HeroFilter.Length; i++)
            HeroFilter[i] = true;
        PositionButtonHeroFilter = new float[3];
        PositionButtonHeroFilter[0] = ObjectController[2].GetComponent<RectTransform> ().localPosition.x; //Set tọa độ ban đầu
        PositionButtonHeroFilter[1] = 255f; //Khoảng cách sẽ dịch chuyển
        SetupLanguage ();
        SetupHeroList ();
        ButtonFunctions (0);
        var count = DataUserController.Heroes.DBHeroes.Count;
        for (int i = 0; i < count; i++) {
            GlobalVariables.HeroSlotListSelected.Add (i); //Đưa danh sách slot của những hero đã chọn vô mảng global
        }
    }

    private void SetupLanguage () {
        for (int i = 0; i < 6; i++) //Các text class nhân vật
            TextUI[i].text = Languages.lang[i + 16];

        //Introductions
        TextUI[8].text = Languages.lang[286];
        TextUI[9].text = Languages.lang[287];
    }

    /// <summary>
    /// Khởi tạo mảng danh sách hero đã sở hữu, và đưa lên UI
    /// </summary>
    private void SetupHeroList () {
        ListVectorObjectHero = new List<Vector2> ();
        HorizontalQuantity = Convert.ToInt32 (ObjectController[11].GetComponent<RectTransform> ().sizeDelta.x + ObjectController[10].GetComponent<RectTransform> ().sizeDelta.x) / 208; //Tính lại số lượng item trên 1 hàng theo scale của màn hình
        var totalHeroesInList = DataUserController.Heroes.DBHeroes.Count;
        int i_temp_x = 0; //Biến tạm hàng ngang
        int i_temp_y = 0; //Biến tạm hàng dọc
        float regionSpace = 210f; //Khoảng cách giữa các object
        ObjectController[0].GetComponent<RectTransform> ().sizeDelta = totalHeroesInList % HorizontalQuantity == 0 ? new Vector2 (0, (totalHeroesInList / HorizontalQuantity) * regionSpace) : new Vector2 (0, ((totalHeroesInList / HorizontalQuantity) + 1) * regionSpace);
        float verticalcounttemp = ObjectController[0].GetComponent<RectTransform> ().sizeDelta.y / 2 - 105;
        //Khởi tạo button inventory
        Hero = new List<GameObject> ();
        HeroAvt = new List<Image> ();
        for (int i = 0; i < totalHeroesInList; i++) {
            var temp = i;
            Hero.Add ((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/UI/HeroItemSmall"), new Vector3 (-((HorizontalQuantity * regionSpace) / 2) + 102 + regionSpace * i_temp_x, verticalcounttemp + regionSpace * -i_temp_y, 0), Quaternion.identity));
            Hero[i].transform.SetParent (ObjectController[0].transform, false); //Đẩy prefab vào scroll
            HeroAvt.Add (Hero[i].transform.GetChild (0).GetComponent<Image> ()); //Lấy biến avt
            HeroAvt[i].sprite = Resources.Load<Sprite> ("HeroAvt/" + DataUserController.Heroes.DBHeroes[i].ID.ToString ()); //Set avt theo ID hero
            HeroAvt[i].GetComponent<Button> ().onClick.AddListener (() => ShowHeroInforDetail (temp));
            i_temp_x++;
            //Space line
            if ((i + 1) % HorizontalQuantity == 0 && i != 0) {
                i_temp_y++;
                i_temp_x = 0;
            }
            //Gán tọa độ
            ListVectorObjectHero.Add (Hero[i].GetComponent<RectTransform> ().localPosition);
        }
    }

    /// <summary>
    /// Lọc danh sách hero theo class, cận chiến, sát thủ, hỗ trợ, đỡ đòn, xạ thủ, pháp sư
    /// </summary>
    /// <param name="slotClass"></param>
    public void FilterHero (int slotClass) {
        GlobalVariables.HeroSlotListSelected.Clear();
        GlobalVariables.HeroSlotListSelected = new List<int> ();
        var listFilter = new List<HeroesProperties> (); //Khởi tảo mảng lọc tạm
        HeroFilter[slotClass] = !HeroFilter[slotClass];
        if (!HeroFilter[0] && !HeroFilter[1] && !HeroFilter[2] && !HeroFilter[3] && !HeroFilter[4] && !HeroFilter[5])
            HeroFilter[0] = HeroFilter[1] = HeroFilter[2] = HeroFilter[3] = HeroFilter[4] = HeroFilter[5] = true;

        #region Lọc hero theo class 

        // cận chiến
        if (!HeroFilter[0]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[2], ObjectController[2].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[2].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.canchien).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[2], ObjectController[2].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[2].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }
        //sát thủ
        if (!HeroFilter[1]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[3], ObjectController[3].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[3].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.satthu).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[3], ObjectController[3].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[3].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }
        //hỗ trợ
        if (!HeroFilter[2]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[4], ObjectController[4].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[4].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.hotro).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[4], ObjectController[4].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[4].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }
        //đỡ đòn
        if (!HeroFilter[3]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[5], ObjectController[5].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[5].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.dodon).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[5], ObjectController[5].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[5].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }
        //xạ thủ
        if (!HeroFilter[4]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[6], ObjectController[6].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[6].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.xathu).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[6], ObjectController[6].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[6].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }
        //pháp sư
        if (!HeroFilter[5]) {
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[7], ObjectController[7].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0] - PositionButtonHeroFilter[1], ObjectController[7].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        } else {
            listFilter.AddRange (DataUserController.Heroes.DBHeroes.Where (x => x.Type == global::HeroesProperties.player_type.phapsu).ToList ());
            StartCoroutine (GameSystem.MoveObjectCurve (true, ObjectController[7], ObjectController[7].GetComponent<RectTransform> ().localPosition, new Vector2 (PositionButtonHeroFilter[0], ObjectController[7].GetComponent<RectTransform> ().localPosition.y), .2f, moveCurve));
        }

        #endregion

        #region Xử lý mảng sau khi lọc 

        //Ẩn toàn bộ obect hero
        var count = ListVectorObjectHero.Count;
        for (int i = 0; i < count; i++) {
            Hero[i].SetActive (false);
        }
        //Hiển thị những hero có trong bộ lọc
        listFilter = listFilter.OrderBy(x => x.ID).ToList();//Sắp xếp lại theo ID

        count = listFilter.Count;
        for (int i = 0; i < count; i++) {
            var index = DataUserController.Heroes.DBHeroes.FindIndex (x => x.ID == listFilter[i].ID);
            Hero[index].SetActive (true);
            GlobalVariables.HeroSlotListSelected.Add (index); //Đưa danh sách slot của những hero đã chọn vô mảng global
        }
        //Sắp xếp lại vị trí các hero
        count = ListVectorObjectHero.Count;
        var counter = 0;
        for (int i = 0; i < count; i++) {
            if (Hero[i].activeSelf) {
                Hero[i].GetComponent<RectTransform> ().localPosition = ListVectorObjectHero[counter];
                counter++;
            }
        }

        #endregion
    }

    /// <summary>
    /// Click vào nhân vật và hiển thị chi tiết thông tin nhân vật
    /// </summary>
    private void ShowHeroInforDetail (int slotHero) {
        GlobalVariables.HeroSlotSelected = slotHero;
        GameSystem.InitializePrefabUI (4);
    }
    /// <summary>
    /// Nút thực hiện các chức năng của hệ thống nhân vật
    /// 0: Đóng thông tin chi tiết
    /// 1: Trang bị item
    /// 2: Gỡ bỏ item
    /// 3: Hero trước đó
    /// 4: Hero tiếp theo
    /// 5: Trang bị nhanh
    /// 6: Gỡ nhanh
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case -1: //Đóng UI
                GameSystem.DisposePrefabUI (2);
                break;
            case 0: //refresh vàng và gem
                TextUI[6].text = string.Format ("{0:#,#}", DataUserController.User.Golds); //Vang
                TextUI[7].text = string.Format ("{0:#,#}", DataUserController.User.Gems); //Kim cuong
                break;
            case 1: //Mở UI hướng dẫn
            ObjectController[12].SetActive(true);
                break;
            case 2: //Đóng UI hướng dẫn
            ObjectController[12].SetActive(false);
                break;
            default:
                break;
        }
    }

}