using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
//Layer map: 
//0: Background
//1-19: Các object gần với bg nhất
//20: Layer ban đêm đầu tiên (Không có object nào được set layer này)
//21-50: Các object trước nền, sau cảnh chính
//51: Layer ban đêm thứ 2 (Không có object nào được set layer này)
//52-99: Các object phía sau map chính
//100-110: Map chính và các object trên map
//111-120: Các hiệu ứng bổ sung trong map (Cho sinh động)
//149: Layer enemy
//150: Layer nhân vật
//154: Layer skill enemy
//155: Layer skill nhân vật
//180-199: Các object phía trước
//200-220: Layer các hiệu ứng phía trước toàn bộ (thời tiết, sương mù...)
public static class Module {
    #region Setting Default Value
    #region Default Values 
    public static string PROTECKEY = "BlackF";
    /// <summary>
    /// 0 -5: Skill Total
    /// 1 -4
    /// 2 -3: Skill of Hero
    /// 3 -2: Skill of Enemy
    /// 4 -1: Black Background - nền đen mờ nổi lên để làm nổi bật skill
    /// 5 0: Main Heroes
    /// 6 1: Enemy
    /// 7 2
    /// 8 3
    /// 9 4: Damage text
    /// 10 5
    /// </summary>
    public static float[] BASELAYER = new float[11] {-0.005f, -0.004f, -0.003f, -0.002f, -0.001f, 0, 0.001f, 0.002f, 0.003f, 0.004f, 0.005f };
    /// <summary>
    /// Layer mặc định cho các loại object để xác định va chạm (Khác với layer hiển thị object)
    /// 0: Lane
    /// 1: Hero
    /// 2: Enemey
    /// 3: SkillHero (Skill tương tác với team mình và đối phương)
    /// 4: SkillEnemy (Skill tương tác với team mình và đối phương)
    /// 5: SkillHeroCollision (Skill vật lý, ko va chạm với team mình)
    /// 6: SkillEnemyCollision (Skill vật lý, ko va chạm với team mình)
    /// 7: SkillHeroOnlyTeam (Skill của hero chỉ tương tác với team mình)
    /// 8: SkillEnemyOnlyTeam (Skill của enemy chỉ tương tác với team mình)
    /// </summary>
    public static int[] BASELAYERRIGID2D = new int[9] { 8, 9, 10, 11, 12, 13, 14, 15, 16 };
    //=====================================================================
    public static bool PAUSEGAME = false;
    public static bool CURRENSEE = true; //Hướng nhìn của hero thời gian thực, false = left, true = right
    public static readonly Vector3 BASEVECTORHIDENOBJECT = new Vector3 (-1000, -1000);
    public static readonly float BASESKILL1SPEEDDEFAULT = 0.5f; //Tốc độ lướt hoặc nhảy né đòn của các hero
    public static readonly float[] POSMAINHERO = new float[3] { 0f, -2f, 0f };
    public static readonly float TIMEDELAYCOMBO = 1f;
    public static readonly float HeroMoveSpeedDefault = 0.15f;
    public static readonly float EnemyMoveSpeedDefault = 0.1f;
    public static readonly float[] LIMITMAPMOVE = new float[2] { 0f, 520f }; //Giới hạn 2 đầu map
    public static readonly float RANGEMOVELIMIT = 20f;
    public static float deltaTime;
    public static readonly string COMBOTEXT = "Combo ";
    public static readonly float JUMPSPEED = 8.0f;
    public static readonly float GRAVITYHERO = 1f;
    public static bool IsLimitFPS = true; //On/Off FPS limit
    public static readonly int NightRandom = 50; //Tỷ lệ % ban đêm ngẫu nhiên trong battle, còn lại là ngày.
    public static readonly Vector2 RANGEMINIMAP = new Vector2 (17.5f, 2.7f); //x = độ rộng trừ đi, y = Tốc độ cuộn các object biểu tượng hero và enemy của minimap (Tốc độ càng thap map càng lớn.)
    public static readonly int GRAVITYCHARACTER = 10; //Độ nặng của nhân vật

    public static readonly string DBENEMY = "hBjvW+Ji1oATBTpz807tx3PBXmlloLvOJbSnxpcJnYIyStRPBwKn0U2faZD8Py92nwCB9VuGw/8DGSDj4WtFOXCuGhSLoBRuc74om9/0/6D8lEb0THN8fo30XL1R0X43I5eBL2YDdQWVqUeFY3jq0it6L2oIXTFobt5kdoos+ISTpzWFbR62XtFGduX+qzj6OWkoIBciY+nQbwCfycK0DqKc56Ja78ON7PiWGE/BSCu4Be2vxFrXgCovgJ8Z0/hJYvOfedZS8qh7d6yMBPdWuHINUUSEN+a57SBIJQ4rA3HkQXNBeOWz36WLAaXjjyueQS1TdJtpwzqryCmdkPnfh9Jb8H/6qBwlOwd1Pki45ljQLa56kJ8XGLtYT1MTBatTPYsF8sIfdWbp8lVnxtqqSz8H0XV94GKwgTOoQ4OTJuNvreHq2EllkPElOGVOlW/zHIVRBSeyuvcOw9ab8DXPPQw4Kw24KOgPyB+A8EvR1RYLSgAExLKzRALvWP81SGWj3cpKrxmuwpCQIa9mhV0VaQNiNcEaG3mUwmghVKB7t9g8KmgpyYofzIBJlTn+SQ7C3k4WlFBjLRfhR7a7/SWWFAgzVXBQM2riZ1i8CXICSCJmW8lA49+zZ8t0aMS3BejTYo4lL0Y6EW0eVKBAdAstKSZKU/LMsu503wEeIx/aMmo7pMM0CpsoQV2pq16yazE3IptTxb+yoh9WZMHv4IXsaHQfBHZMnLqEUwGrhj8GsjTAytflnjW0EARA8ZU1CBZDxBBWxhyeEtyTchv8Z8ATbHfl4yXcyimmP677EfDXongWJA8P1MlKd9HZIizoA5xRmd+JcLeDjlhy3fnfjOiFa3fl4yXcyimmy2A8rz3q3FuVwAUK1hdkqnSiFgnbMkcfBIK0qZtM4p8IL/FLjslNc3fl4yXcyimmx3cITh1YEP0ZvtYLjQttIoPN13IAWQuIVmTB7+CF7Gi8AzacCbG+4E3DSFzE1q6ubMLqDJh5PViok5pnIRaUgqG8T25kGtWGj7ots31hY89OZuPFNszYFksbQDE8RBnsgW+8QtMuUzFOZuPFNszYFksbQDE8RBnsgW+8QtMuUzFOZuPFNszYFksbQDE8RBnsgW+8QtMuUzFOZuPFNszYFksbQDE8RBnszJWdyAfyBuOTOcr9MB9j6TcYjmA01eo9";
    //public static string DBHEROES = "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";
    public static readonly string DBHEROES = "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";

    public static readonly string HeroDefault = "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";
    public static Guid GUILDBASE;

    #endregion//Danh sách các ID item được phép xuất hiện, thay đổi cái này, thay đổi trong Class Inventory, Item Drop, SpinController

    public static readonly float WAITREQUEST = 3f; //Thời gian chờ đợi request
    public static readonly float ReMP = 30f; //Tốc độ hồi lại thể lực mỗi giây (Battle cũ)
    public static sbyte WorldMapRegionSelected = 0; //User lựa chọn vùng bản đồ để chơi
    public static sbyte BattleModeSelected = 0; //User lựa chọn chế độ chơi, 0 = preview, 1 = random, 2 = survival
    public static readonly short ExpRatio = 890; //Chỉ số exp của nhân vật, càng cao càng khó lên level
    public static readonly short HeroMaxlevel = 10; //Level tối đa của nhân vật
    public static readonly short GemsForHireAssassin = 50; //Giá kim cương thuê sát thủ
    #region Spin controller 

    public static readonly float SpinRotateDefault = 3000f; //Tốc độ quay mặc định của mũi tên Spin
    public static readonly float SpinTimeStop = 1.5f; //Thời gian dừng mũi tên quay sau khi nhả nút bấm. 1 = 1s
    public static readonly sbyte SpinMainItemType = 10; //Type của item cần quay
    public static readonly byte SpinMainItemID = 25; //Item cần thiết để sử dụng spin, ID là lông phượng
    public static readonly int SpinItemQuantity = 1; //Số lượng item cần thiết cho 1 lần quay spin
    public static readonly int SpinItemPriceDiamond = 15; //Giá bán 1 item để quay spin, tính bằng kim cương
    #endregion
    public static string PrevScene = ""; //Scene trước đó, dùng để back lại
    //public static int CURRENTCOMBONORMALATK;//Set ở normal atk để detect được đòn combo hiện tại là thứ mấy -- Tạm thời ko dùng
    #region Giá bán của mỗi giá trị đơn vị chỉ số item 

    public static readonly int PricePerAtk = 90; //Mỗi 1 chỉ số atk 
    public static readonly int PricePerGetHPAtk = 120; //Mỗi 1 chỉ số hút máu 
    public static readonly int PricePerBuffAtk = 135; //Mỗi 1 chỉ số xuyên giáp
    public static readonly int PricePerCrit = 145; //Mỗi 1 chỉ số chí mạng
    public static readonly int PricePerMagic = 95;
    public static readonly int PricePerGetHPMagic = 125;
    public static readonly int PricePerBuffMagic = 140;
    public static readonly int PricePerHP = 50;
    public static readonly int PricePerReHP = 65;
    public static readonly int PricePerDefP = 75;
    public static readonly int PricePerDefM = 80;
    public static readonly int PricePerDefState = 150;

    #endregion

    #region Setting variable 
    public static int LimitFPS = 60; //Frame number FPS limit
    public static int SettingLanguage = 0;
    public static int SettingButtonCombat = 0; //Nút bấm skill trong battle, 0 = bên trái, 1 = bên phải
    public static int SettingSkillSlowMotion = 0; //Cho phép hiệu ứng làm chậm khi sử dụng kỹ năng của nhân vật, 0 = tắt, 1 = mở
    public static bool SettingSound = true, SettingMusic = true;

    #endregion
    public static readonly string AvatarHeroLink = "HeroAvt/";
    #endregion
    public static string ShowFPS () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        return Mathf.Ceil (fps).ToString ();
    }

    /// <summary>
    /// Tính toán exp cho cấp độ tiếp theo dựa trên level hiện tại truyền vào
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float NextExp (int level) {
        return level * ExpRatio + ((level * level) * ExpRatio / 10);
    }

    #region Security
    /// <summary>
    /// Encode a string by protect key (var name: PROTECTKEY)
    /// </summary>
    /// <param name="strEnCrypt"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string enc (string strEnCrypt, string key) {
        try {
            byte[] keyArr;
            byte[] EnCryptArr = UTF8Encoding.UTF8.GetBytes (strEnCrypt);
            MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider ();
            keyArr = MD5Hash.ComputeHash (UTF8Encoding.UTF8.GetBytes (key));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider ();
            tripDes.Key = keyArr;
            tripDes.Mode = CipherMode.ECB;
            tripDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = tripDes.CreateEncryptor ();
            byte[] arrResult = transform.TransformFinalBlock (EnCryptArr, 0, EnCryptArr.Length);
            return Convert.ToBase64String (arrResult, 0, arrResult.Length);
        } catch (Exception) { }
        return "";
    }
    /// <summary>
    /// Decode a string encoded by protect key (var name: PROTECTKEY)
    /// </summary>
    /// <param name="strDecypt"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string dec (string strDecypt, string key) {
        try {
            byte[] keyArr;
            byte[] DeCryptArr = Convert.FromBase64String (strDecypt);
            MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider ();
            keyArr = MD5Hash.ComputeHash (UTF8Encoding.UTF8.GetBytes (key));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider ();
            tripDes.Key = keyArr;
            tripDes.Mode = CipherMode.ECB;
            tripDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = tripDes.CreateDecryptor ();
            byte[] arrResult = transform.TransformFinalBlock (DeCryptArr, 0, DeCryptArr.Length);
            return UTF8Encoding.UTF8.GetString (arrResult);
        } catch (Exception) { }
        return "";
    }
    #endregion

    #region Save and Load game
    /// <summary>
    /// Save game
    /// </summary>
    /// <param name="strs">String base</param>
    /// <param name="values">Value of string base</param>
    public static void GameSave (string strs, string values) {
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
        PlayerPrefs.SetString (strs, values);
    }
    public static string GameLoad (string strs) {
        return PlayerPrefs.GetString (strs);
    }

    /// <summary>
    /// Tạo các giá trị mặc định khi khởi chạy game, tránh gây lỗi
    /// </summary>
    public static void CreateDefault () {
        if (GameLoad ("CharID").Equals (null) || GameLoad ("CharID").Equals (""))
            GameSave ("CharID", "1");
        if (GameLoad ("Map").Equals (null) || GameLoad ("Map").Equals (""))
            GameSave ("Map", "2");
    }

    /// <summary>
    /// Check values in save game (Kiểm tra giá trị có tồn tại trong hệ thống save game hay chưa)
    /// </summary>
    /// <param name="strs"></param>
    /// <returns></returns>
    public static bool CheckValue (string strs) {
        if (String.IsNullOrEmpty (GameLoad (strs)))
            return false;
        return true;
    }
    #endregion

    #region Battle

    /// <summary>
    /// Caculator damage, tính toán sát thương gây ra
    /// </summary>
    /// <param name="dame_physic">Dame gốc vật lý</param>
    /// <param name="dame_magic">Dame phép</param>
    /// <param name="def_physic">Thủ vật lý của đối phương</param>
    /// <param name="def_magic">Thủ phép của đối phương</param>
    /// <param name="pass_def_physic">Xuyên giáp</param>
    /// <param name="pass_def_magic">Xuyên phép</param>
    /// <param name="dameper">Số phần trăm tính toán</param>
    /// <param name="type">Kiểu sát thương. 0: Vật lý, 1: phép</param>
    /// <returns></returns>
    public static float Damage (float dame_physic, float dame_magic, float def_physic, float def_magic, float pass_def_physic, float pass_def_magic, int dameper, int type) {
        float dame_end = 0f;
        if (type == 0) //Sát thương vật lý
            dame_end = def_physic >= 0 ? dame_physic * (100 / (100 + (def_physic - (def_physic * pass_def_physic / 100)))) : dame_end = dame_physic * (2 - 100 / (100 - def_physic));
        if (type == 1) //Sát thương phép
            dame_end = def_magic >= 0 ? dame_end = dame_magic * (100 / (100 + (def_magic - (def_magic * pass_def_magic / 100)))) : dame_end = dame_magic * (2 - 100 / (100 - def_magic));
        dame_end = dame_end * dameper / 100; //Tính toán số phần trăm sát thương
        return UnityEngine.Random.Range (dame_end - (dame_end * 10 / 100), dame_end + (dame_end * 10 / 100)); //Dame sẽ chênh lệch 10%
    }
    #endregion
}