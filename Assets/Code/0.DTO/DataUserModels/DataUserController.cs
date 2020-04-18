using Assets.Code._0.DTO.Models;
using Assets.Code._4.CORE;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thao tác với dữ liệu người chơi

//listhero = tập hợp id các hero hiện có, ngăn cách = ;
//teamhero = danh sách hero đang trong đội hình, ngăn cách = ;
//Map = màn chơi hiện tại
//music = âm nhạc
//sound = âm thanh
//ScreenSizeDefault = Kích thước màn hình mặc định của máy
//CharID = ID nhân vật được chọn lúc vào trận
//SettingGraphics = 1 tới 4: mức thiết lập đồ họa trong game
//SettingMusic
//SettingSound
public static class DataUserController
{
    public static Player User;
    private static bool IsNewGame = false;
    //public static AccountInformations UserInfor = new AccountInformations(); //Thông tin user
    public static DatabaseHeroDefault HeroesDefault = new DatabaseHeroDefault(); //Toàn bộ hero mặc định
    public static DatabaseHeroes Heroes = new DatabaseHeroes(); //Danh sách hero mà user sở hữu
    public static DataInventory Inventory = new DataInventory(); //Inventory của user
    public static EquipBag Bag = new EquipBag();
    public static string Team = "0;0;0";
    public static string UserName = "";
    public static string GameSettings;
    public static string HeroesStringSave = "Data1";
    public static string InventoryStringSave = "Data2";
    public static string BagStringSave = "Data3";
    public static string TeamStringSave = "Data4";
    public static string UserInforStringSave = "Data5";

    public static string StrSaveSetting = "Settings";
    public static string HeroesDefaultStringSave = "DataHTemplate";
    public static string GameSetting_Language = "Setting1";
    public static string GameSetting_FPS = "Setting2";
    public static string GameSetting_ButtonBattle = "Setting3";
    public static string GameSetting_SkillSlowMotion = "Setting4";
    public static string GameSetting_Sound = "Setting5";
    public static string GameSetting_Music = "Setting6";
    public static string UserNameStringSave = "Username";

    #region Save game 
    public static void SaveAll()
    {
        SaveHeroes();
        SaveInventory();
        SaveUserInfor();
    }
    //Lưu danh sách hero sở hữu
    public static void SaveHeroes()
    {
        PlayerPrefs.SetString(HeroesStringSave, Securitys.Encrypt(JsonUtility.ToJson(Heroes)).ToString());
    }
    //Lưu thùng đồ
    public static void SaveInventory()
    {
        PlayerPrefs.SetString(InventoryStringSave, Securitys.Encrypt(JsonUtility.ToJson(Inventory)).ToString());
    }
    //Lưu bộ trang bị
    public static void SaveBag()
    {
        PlayerPrefs.SetString(BagStringSave, Securitys.Encrypt(JsonUtility.ToJson(Bag)).ToString());
    }
    public static void SaveTeam()
    {
        PlayerPrefs.SetString(TeamStringSave, Securitys.Encrypt(Team).ToString());
    }
    public static void SaveUsername()
    {
        PlayerPrefs.SetString(UserNameStringSave, Securitys.Encrypt(UserName).ToString());
    }
    public static void SaveUserInfor()
    {
        SaveData(UserInforStringSave, JsonUtility.ToJson(User));
        //PlayerPrefs.SetString(UserInforStringSave, Securitys.Encrypt(JsonUtility.ToJson(UserInfor)).ToString());
    }

    /// <summary>
    /// Lưu setting của game
    /// </summary>
    /// <param name="type">0: ngôn ngữ, 1 khung hình, 2: vị trí button trong combat</param>
    /// <param name="value">Giá trị sẽ lưu</param>
    public static void SaveSetting(string strsave, string value)
    {
        //Lưu các giá trị mặc định ban đầu nếu chơi game lần đầu
        if (string.IsNullOrEmpty(strsave))
        {
            PlayerPrefs.SetString(GameSetting_Language, Securitys.Encrypt("1").ToString()); //Save language
            PlayerPrefs.SetString(GameSetting_FPS, Securitys.Encrypt("60").ToString()); //Save FPS
            PlayerPrefs.SetString(GameSetting_ButtonBattle, Securitys.Encrypt("0").ToString()); //Save FPS
            PlayerPrefs.SetString(GameSetting_Sound, Securitys.Encrypt("1").ToString()); //Save Sound
            PlayerPrefs.SetString(GameSetting_Music, Securitys.Encrypt("1").ToString()); //Save Music
        }
        else
            PlayerPrefs.SetString(strsave, Securitys.Encrypt(value).ToString()); //Save language
    }

    /// <summary>
    /// Lưu dữ liệu
    /// </summary>
    public static void SaveData(string strsave, string value)
    {
        var a = Securitys.Encrypt(value).ToString();
        PlayerPrefs.SetString(strsave, Securitys.Encrypt(value).ToString()); //Save language
    }

    /// <summary>
    /// Load Data
    /// </summary>
    public static string LoadData(string strsave)
    {
        return PlayerPrefs.GetString(strsave);
    }
    /// <summary>
    /// Lưu dữ liệu hero mặc định
    /// </summary>

    public static void SaveHeroBase()
    {
        PlayerPrefs.SetString(HeroesDefaultStringSave, Securitys.Encrypt(JsonUtility.ToJson(HeroesDefault)).ToString());
    }
    #endregion

    #region Load game 
    public static void LoadAll()
    {
        LoadSetting();
        LoadHeroes();
        LoadInventory();
        LoadTeam();
        LoadHeroDefault();
        //LoadUsername();
        LoadUserInfor();
    }

    /// <summary>
    /// Khởi tạo lại user mới, dành cho user nào cố tình thay đổi dữ liệu game
    /// </summary>
    public static void SetupNewUser()
    {
        //Set danh sách hero bằng mặc định
        Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[0]);
        Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[2]);
        Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[4]);

        //Tặng item
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 1, 1));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 21, 1));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 40, 1));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 140, 1));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 160, 1));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.equip, 1, false, 170, 1));

        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.quest, 1, false, 330, 100));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.quest, 1, false, 331, 80));
        // Inventory.DBItems.Add (ItemDropController.CreateItem (global::Items.ItemType.quest, 1, false, 230, 20));
    }
    public static void LoadHeroes()
    {
        var str = PlayerPrefs.GetString(HeroesStringSave);
        var strsave = !string.IsNullOrEmpty(str) ? Securitys.Decrypt(str).ToString() : "";
        if (string.IsNullOrEmpty(strsave))
        {
            IsNewGame = true; //set biến new game = true
            LoadHeroDefault();
            //Heroes.DBHeroes = HeroesDefault.DBHeroesDefault; //Set danh sách hero bằng mặc định khi new game
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[0]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[1]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[2]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[3]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[4]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[5]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[6]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[7]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[8]);
            Heroes.DBHeroes.Add(HeroesDefault.DBHeroesDefault[9]);
            SaveHeroes();
        }
        else
            Heroes = JsonUtility.FromJson<DatabaseHeroes>(strsave);
    }
    public static void LoadInventory()
    {
        var str = PlayerPrefs.GetString(InventoryStringSave);
        var strsave = !string.IsNullOrEmpty(str) ? Securitys.Decrypt(str).ToString() : "";
        Inventory = string.IsNullOrEmpty(strsave) ? Inventory : JsonUtility.FromJson<DataInventory>(strsave);
        if (!ItemSystem.IsCalculatorItemPrice)
        {
            var count = Inventory.DBItems.Count;
            for (int i = 0; i < count; i++)
                Inventory.DBItems[i].ItemPrice = ItemSystem.GetItemPrice(Inventory.DBItems[i]);
        }
        //Tặng item nếu như game mới
        if (IsNewGame)
        {
            //Trang bị
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 0, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 1, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 2, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 3, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 4, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 5, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 6, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 7, 0, 1));//Kiếm ngắn
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 8, 0, 1));//Kiếm ngắn

            //Vật phẩm
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 0,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 1,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 2,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 3,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 4,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 5,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 6,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 7,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 8,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 9,  5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 10, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 11, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 12, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 13, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 14, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 15, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 16, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 17, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 18, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 19, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 20, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 21, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 22, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 23, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 24, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 25, 5));
            Inventory.DBItems.Add(ItemSystem.CreateItem(false, false, 1, 10, 26, 10));

            SaveInventory();
            IsNewGame = false;
        }
    }
    public static void LoadEquipBag()
    {
        var strsave = Securitys.Decrypt(PlayerPrefs.GetString(BagStringSave)).ToString();
        Bag = string.IsNullOrEmpty(strsave) ? Bag : JsonUtility.FromJson<EquipBag>(strsave);
    }
    public static void LoadTeam()
    {
        var str = PlayerPrefs.GetString(TeamStringSave);
        var strsave = !string.IsNullOrEmpty(str) ? Securitys.Decrypt(str).ToString() : "";
        Team = string.IsNullOrEmpty(strsave) ? Team : strsave;
    }
    public static void LoadUserInfor()
    {
        var str = PlayerPrefs.GetString(UserInforStringSave);
        var strsave = !string.IsNullOrEmpty(str) ? Securitys.Decrypt(str).ToString() : "";
        //UserInfor = !string.IsNullOrEmpty(str) ? JsonUtility.FromJson<AccountInformations>(strsave) : UserInfor;

        if (string.IsNullOrEmpty(strsave))
        {
            CreateNewPlayer("", "");
        }
        else
            User = JsonUtility.FromJson<Player>(strsave);

        // if(string.IsNullOrEmpty(UserInfor.CurrentVersion))//Kiểm tra version
        // {
        //     ClearAllData ();
        //     SetupNewUser ();
        //     UserInfor.CurrentVersion = Application.version;
        //     SaveAll ();
        //     SceneLoad scn = new SceneLoad ();
        //     scn.Change_scene ("Loading");
        // }
    }
    public static void LoadHeroDefault()
    {
        if (HeroesDefault.DBHeroesDefault == null || HeroesDefault.DBHeroesDefault.Count <= 0)
        { //chỉ load 1 lần
            // var strsave = Securitys.Decrypt (PlayerPrefs.GetString (HeroesDefaultStringSave)).ToString ();
            // HeroesDefault = string.IsNullOrEmpty (strsave) ? FirstSetupDefault () : JsonUtility.FromJson<DatabaseHeroDefault> (strsave);
            //HeroesDefault = JsonUtility.FromJson<DatabaseHeroDefault> (Securitys.Decrypt (Module.HeroDefault).ToString ());
            HeroesDefault.DBHeroesDefault = new List<HeroesProperties>();
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1, "Oralie", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[0], 1, 0, 1328, 334, 67.22f, 0, 32.5f, 31, 2.07f, 45.9f, 0, 0, 0, 0, 0, 0, 0.3f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 5.3f }, new float[] { 0, 250 }, 88, 3.11f, 0, 3.25f, 0.5f, 2.75f, 4.5f, 0, global::HeroesProperties.player_type.satthu, global::HeroesProperties.HeroType.far));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(2, "Griselda", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[1], 1, 0, 1473, 292, 72, 0, 39, 34.6f, 2.28f, 50.45f, 0, 0, 0, 0, 0, 0, 0.4f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 4.3f }, new float[] { 0, 250 }, 92, 3, 0, 3, 1.25f, 3.25f, 2.25f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(3, "Fiona", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[2], 1, 0, 1355, 350, 69, 0, 30, 34.6f, 1.3f, 46.56f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 4 }, new float[] { 0, 250 }, 85, 3.3f, 0, 3.5f, 1.25f, 2.5f, 2.8f, 0, global::HeroesProperties.player_type.satthu, global::HeroesProperties.HeroType.far));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(4, "Celina", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[3], 1, 0, 1412, 342, 70.8f, 0, 41.2f, 34.6f, 2.44f, 72.45f, 0, 0, 0, 0, 0, 0, 0.37f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 4.5f }, new float[] { 0, 250 }, 90, 3.4f, 0, 3.6f, 1.25f, 3.5f, 2.25f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(5, "Aurelia", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[4], 1, 0, 1409, 304, 76, 0, 41, 34.6f, 1.98f, 86.65f, 0, 0, 0, 0, 0, 0, 0.45f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 6.8f }, new float[] { 0, 250 }, 85, 4f, 0, 3.5f, 1.25f, 3.25f, 3.25f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(6, "Eira", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[5], 1, 0, 1235, 503, 21.7f, 60.14f, 26.72f, 31, 1.76f, 70.8f, 0, 0, 0, 0, 0, 0, 0.3f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 6.2f }, new float[] { 0, 450 }, 85, 1.3f, 3.3f, 4, 0.5f, 2.75f, 4f, 0, global::HeroesProperties.player_type.phapsu, global::HeroesProperties.HeroType.far));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(7, "Heulwen", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[6], 1, 0, 1433, 320, 37, 67.4f, 37.88f, 34.6f, 2.32f, 65.4f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 7.5f }, new float[] { 0, 350 }, 85, 2, 3.2f, 3.5f, 1.25f, 3, 2f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(8, "Veronica", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[7], 1, 0, 1382, 278, 67.22f, 0, 32, 31, 1.53f, 76.45f, 0, 0, 0, 0, 0, 0, 0.4f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 4 }, new float[] { 0, 250 }, 88, 3.11f, 0, 3, 0.5f, 3.25f, 2.25f, 0, global::HeroesProperties.player_type.xathu, global::HeroesProperties.HeroType.far));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(9, "Vera", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[8], 1, 0, 1292, 410, 21, 56.04f, 39.6f, 31, 1.1f, 90.4f, 0, 0, 0, 0, 0, 0, 0.25f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 7.5f }, new float[] { 0, 380 }, 79, 1, 3, 3.8f, 0.5f, 0.25f, 2f, 0, global::HeroesProperties.player_type.hotro, global::HeroesProperties.HeroType.far));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(10, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, 1413, 342, 66, 0, 54.2f, 34.6f, 2.37f, 68.1f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, 8 }, new float[] { 0, 330 }, 95, 3, 0, 3.6f, 1.25f, 0.25f, 1.5f, 0, global::HeroesProperties.player_type.dodon, global::HeroesProperties.HeroType.near));

            //Enemy
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1000, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, 1413, 342, 66, 0, 54.2f, 34.6f, 2.37f, 68.1f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, -1 }, new float[] { 0, 330 }, 95, 3, 0, 3.6f, 1.25f, 0.25f, 1.5f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1001, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, 1413, 342, 66, 0, 54.2f, 34.6f, 2.37f, 68.1f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, -1 }, new float[] { 0, 330 }, 95, 3, 0, 3.6f, 1.25f, 0.25f, 1.5f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1002, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, 1413, 342, 66, 0, 54.2f, 34.6f, 2.37f, 68.1f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, -1 }, new float[] { 0, 330 }, 95, 3, 0, 3.6f, 1.25f, 0.25f, 1.5f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1003, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, 1413, 342, 66, 0, 54.2f, 34.6f, 2.37f, 68.1f, 0, 0, 0, 0, 0, 0, 0.35f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { 0, -1 }, new float[] { 0, 330 }, 95, 3, 0, 3.6f, 1.25f, 0.25f, 1.5f, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));

            //Boss đứng yên
            HeroesDefault.DBHeroesDefault.Add(new HeroesProperties(1500, "Xandra", "Descriptions", Languages.HeroIntrinsic[0], Languages.HeroSkillDescription[9], 1, 0, float.MaxValue, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new float[] { -1, -1 }, new float[] { 0, 330 }, 0, 0, 0, 0, 0, 0, 0, 0, global::HeroesProperties.player_type.canchien, global::HeroesProperties.HeroType.near));
        }
    }
    public static void LoadSetting()
    {
        var str = LoadData(StrSaveSetting);
        var strsave = !string.IsNullOrEmpty(str) ? Securitys.Decrypt(str) : "";
        if (string.IsNullOrEmpty(strsave))
        {
            CreateSetting();
        }
        else 
            GameSystem.Settings = JsonUtility.FromJson<SettingModel>(strsave);

        //print (Application.systemLanguage);
        //Module.SettingLanguage = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_Language).ToString()) ? (Application.systemLanguage.ToString().StartsWith("Vietnam") ? 1 : 0) : Convert.ToInt32(Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_Language)).ToString());
        //Module.LimitFPS = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_FPS).ToString()) ? 60 : int.Parse(Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_FPS)).ToString());
        //Module.SettingButtonCombat = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_ButtonBattle).ToString()) ? 0 : int.Parse(Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_ButtonBattle)).ToString());
        //Module.SettingSkillSlowMotion = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_SkillSlowMotion).ToString()) ? 0 : int.Parse(Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_SkillSlowMotion)).ToString());
        //Module.SettingSound = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_Sound).ToString()) ? true : Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_Sound)).ToString().Equals("0") ? false : true;
        //Module.SettingMusic = string.IsNullOrEmpty(PlayerPrefs.GetString(GameSetting_Music).ToString()) ? true : Securitys.Decrypt(PlayerPrefs.GetString(GameSetting_Music)).ToString().Equals("0") ? false : true;
        //Languages.SetupLanguage(Module.SettingLanguage);
        //Application.targetFrameRate = Module.LimitFPS;
        ////Nếu là lần đầu mở game => save các giá trị ban đầu
        //if (string.IsNullOrEmpty(PlayerPrefs.GetString("Setting1").ToString()))
        //    SaveSetting(null, null);
    }

    public static void LoadUsername()
    {
        UserName = Securitys.Decrypt(PlayerPrefs.GetString(UserNameStringSave)).ToString();
    }
    #endregion

    #region Clear dữ liệu 
    public static void ClearAllData()
    {
        ClearUserInfor();
        ClearHeroes();
        ClearInventory();
        Team = "0;0;0";
        UserName = "";
    }
    public static void ClearUserInfor()
    {
        CreateNewPlayer(User.UserName, User.PassCode);
        //UserInfor = new AccountInformations(); //Thông tin user
    }
    public static void ClearHeroes()
    {
        Heroes = new DatabaseHeroes(); //Danh sách hero mà user sở hữu
    }
    public static void ClearInventory()
    {
        Inventory = new DataInventory(); //Inventory của user
    }

    #endregion

    /// <summary>
    /// Chơi game lần đầu
    /// </summary>
    public static void CreateNewGame(string userName, string passCode)
    {
        //Khởi tạo cài đặt
        CreateSetting();

        //Khởi tạo dữ liệu người dùng
        CreateNewPlayer(userName, passCode);
    }

    /// <summary>
    /// Khởi tạo cài đặt
    /// </summary>
    public static void CreateSetting()
    {
        //Khởi tạo cài đặt
        GameSystem.Settings = new SettingModel
        {
            ButtonBattle = false,
            FPSLimit = 60,
            Language = Application.systemLanguage.ToString().StartsWith("Vietnam") ? 1 : 0,
            MusicEnable = true,
            SkillSlowMotion = false,
            SoundEnable = true,
            Tutorial = true
        };
        SaveData(StrSaveSetting, JsonUtility.ToJson(GameSystem.Settings));
    }

    /// <summary>
    /// Khởi tạo dữ liệu người chơi mới
    /// </summary>
    public static void CreateNewPlayer(string userName, string passCode)
    {
        User = new Player
        {
            UserID = Guid.NewGuid().ToString(),
            UserName = userName,
            PassCode = passCode,
            BattleLose = 0,
            BattleWin = 0,
            NumberSpined = 0,
            DifficultMap = new float[7] { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f },
            EnemyFutureMap = new string[7],
            Gems = 100,
            Golds = 1000,
            HWID = SystemInfo.deviceUniqueIdentifier.ToString(),
            InventorySlot = 100,
            IsAutoBattle = false,
            ItemUseForBattle = ";;",
            LastUpdate = 0,
            IsChangeDevice = false
        };
        SaveData(UserInforStringSave, JsonUtility.ToJson(User));
    }

    public static void CreateNewInventory()
    {

    }

    #region Đồng bộ online

    #endregion
}