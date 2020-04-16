using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Danh sách chi tiết những item được chế tạo
/// </summary>
public static class CraftTemplate {
    public static List<string> CraftItemWeaponPhysic;
    public static List<string> CraftItemWeaponMagic;
    public static List<string> CraftItemDefense;
    public static List<string> CraftItemUse;
    public static List<string> CraftItemSets;

    public static void Initialize () {
        CreateItemEquip ();
        CreateItemUse ();
    }

    /// <summary>
    /// Tạo template cho item use
    /// </summary>
    private static void CreateItemUse () {
        if (CraftItemUse == null) {
            //Cấu trúc: A;B,C;B,C
            //A: ID item sẽ được tạo
            //B: ID item cần thiết
            //C: số lượng của item đó
            CraftItemUse = new List<string> ();

            #region Bình máu 
            CraftItemUse.Add ("10,0;1;11,0,5;11,1,4");
            CraftItemUse.Add ("10,1;1;11,0,1;11,0,5;11,1,4");
            CraftItemUse.Add ("10,2;1;11,1,6;11,2,5;11,3,4");
            CraftItemUse.Add ("10,3;1;11,2,1;11,3,5;11,4,4");
            CraftItemUse.Add ("10,4;1;11,4,6;11,5,5;11,6,4");
            CraftItemUse.Add ("10,5;1;11,4,1;11,6,5;11,7,4");
            CraftItemUse.Add ("10,6;1;11,7,6;11,8,5;11,9,4");
            CraftItemUse.Add ("10,7;1;11,9,6;11,10,5;11,11,4");
            CraftItemUse.Add ("10,8;1;11,11,6;11,12,5;11,13,4");
            #endregion

            #region Đá cường hóa 
            CraftItemUse.Add ("10,9;1;11,20,5;11,21,4");
            CraftItemUse.Add ("10,10;1;10,9,1;11,21,5;11,22,4");
            CraftItemUse.Add ("10,11;1;11,21,6;11,22,5;11,23,4");
            CraftItemUse.Add ("10,12;1;11,22,6;11,23,5;11,24,4");
            CraftItemUse.Add ("10,13;1;11,23,6;11,24,5;11,25,4");
            CraftItemUse.Add ("10,14;1;11,24,6;11,25,5;11,26,4");
            CraftItemUse.Add ("10,15;1;11,25,6;11,26,5;11,27,4");
            CraftItemUse.Add ("10,16;1;11,26,6;11,27,5;11,28,4");
            CraftItemUse.Add ("10,17;1;11,27,6;11,28,5;11,29,4");
            CraftItemUse.Add ("10,18;1;11,28,6;11,29,5;11,30,4");
            #endregion

            #region Đá nâng phẩm 
            CraftItemUse.Add ("10,19;1;11,29,1;11,44,1;11,39,1");
            CraftItemUse.Add ("10,20;1;11,30,1;11,44,2;11,39,2");
            CraftItemUse.Add ("10,21;1;11,40,1;11,44,3;11,39,3");
            CraftItemUse.Add ("10,22;1;11,41,1;11,44,4;11,39,4");
            CraftItemUse.Add ("10,23;1;11,42,1;11,44,5;11,39,5");
            CraftItemUse.Add ("10,24;1;11,43,1;11,44,6;11,39,6");
            #endregion
        }
    }

    /// <summary>
    /// Khởi tạo template cho item equip
    /// </summary>
    private static void CreateItemEquip () {

        #region Vũ khí vật lý 

        if (CraftItemWeaponPhysic == null) {
            CraftItemWeaponPhysic = new List<string> ();
            //Cấu trúc: A,F;E;F,B,C;F,B,C
            //A: Loại item
            //F: ID item
            //B: ID item cần thiết
            //C: số lượng của item đó
            //E: Cấp độ để tạo ra chỉ số, cấp càng cao thì chỉ số càng cao
            //Dao nhỏ
            CraftItemWeaponPhysic.Add ("0,0;1;11,20,10;11,31,7;11,38,1");
            CraftItemWeaponPhysic.Add ("0,1;2;11,21,10;11,31,6;11,38,1");
            CraftItemWeaponPhysic.Add ("0,2;3;11,22,10;11,31,5;11,38,1");
            CraftItemWeaponPhysic.Add ("0,3;4;11,23,10;11,31,4;11,38,2");
            CraftItemWeaponPhysic.Add ("0,4;5;11,24,10;11,31,3;11,38,3");
            CraftItemWeaponPhysic.Add ("0,5;6;11,25,10;11,31,2;11,38,4");
            CraftItemWeaponPhysic.Add ("0,6;7;11,26,10;11,31,1;11,38,5");
            //Kiếm
            CraftItemWeaponPhysic.Add ("0,7;1;11,20,10;11,31,8;11,38,1");
            CraftItemWeaponPhysic.Add ("0,8;2;11,21,10;11,31,7;11,38,2");
            CraftItemWeaponPhysic.Add ("0,9;3;11,22,10;11,31,6;11,38,3");
            CraftItemWeaponPhysic.Add ("0,10;4;11,23,10;11,31,5;11,38,4");
            CraftItemWeaponPhysic.Add ("0,11;5;11,24,10;11,31,4;11,38,5");
            CraftItemWeaponPhysic.Add ("0,12;6;11,25,10;11,31,3;11,38,6");
            CraftItemWeaponPhysic.Add ("0,13;7;11,26,10;11,31,2;11,38,7");
            CraftItemWeaponPhysic.Add ("0,14;8;11,27,10;11,31,1;11,38,8");
            //Rìu
            CraftItemWeaponPhysic.Add ("0,15;1;11,20,10;11,31,10;11,38,1");
            CraftItemWeaponPhysic.Add ("0,16;2;11,21,10;11,31,10;11,38,2");
            CraftItemWeaponPhysic.Add ("0,17;3;11,22,10;11,32,9;11,38,3");
            CraftItemWeaponPhysic.Add ("0,18;4;11,23,10;11,32,9;11,38,4");
            CraftItemWeaponPhysic.Add ("0,19;5;11,24,10;11,33,8;11,38,5");
            CraftItemWeaponPhysic.Add ("0,20;6;11,25,10;11,33,8;11,38,6");
            CraftItemWeaponPhysic.Add ("0,21;7;11,26,10;11,34,7;11,38,7");
            CraftItemWeaponPhysic.Add ("0,22;8;11,27,10;11,34,7;11,38,8");
            CraftItemWeaponPhysic.Add ("0,23;9;11,28,10;11,35,6;11,38,9");
            CraftItemWeaponPhysic.Add ("0,24;10;11,29,10;11,35,6;11,38,10");
            CraftItemWeaponPhysic.Add ("0,25;11;11,20,10;11,36,5;11,39,1");
            CraftItemWeaponPhysic.Add ("0,26;12;11,20,15;11,37,5;11,39,2");
            //Búa
            CraftItemWeaponPhysic.Add ("0,27;1;11,20,10;11,31,10;11,38,1");
            CraftItemWeaponPhysic.Add ("0,28;2;11,21,10;11,31,10;11,38,2");
            CraftItemWeaponPhysic.Add ("0,29;3;11,22,10;11,32,9;11,38,3");
            CraftItemWeaponPhysic.Add ("0,30;4;11,23,10;11,32,9;11,38,4");
            CraftItemWeaponPhysic.Add ("0,31;5;11,24,10;11,33,8;11,38,5");
            CraftItemWeaponPhysic.Add ("0,32;6;11,25,10;11,33,8;11,38,6");
            CraftItemWeaponPhysic.Add ("0,33;7;11,26,10;11,34,7;11,38,7");
            CraftItemWeaponPhysic.Add ("0,34;8;11,27,10;11,34,7;11,38,8");
            CraftItemWeaponPhysic.Add ("0,35;9;11,28,10;11,35,6;11,38,9");
            CraftItemWeaponPhysic.Add ("0,36;10;11,29,10;11,35,6;11,38,10");
            CraftItemWeaponPhysic.Add ("0,37;11;11,20,10;11,36,5;11,39,1");
            CraftItemWeaponPhysic.Add ("0,38;12;11,20,15;11,37,5;11,39,2");
            //Chùy
            CraftItemWeaponPhysic.Add ("0,39;1;11,20,10;11,31,10;11,38,1");
            CraftItemWeaponPhysic.Add ("0,40;2;11,21,10;11,31,10;11,38,2");
            CraftItemWeaponPhysic.Add ("0,41;3;11,22,10;11,32,9;11,38,3");
            CraftItemWeaponPhysic.Add ("0,42;4;11,23,10;11,32,9;11,38,4");
            CraftItemWeaponPhysic.Add ("0,43;5;11,24,10;11,33,8;11,38,5");
            CraftItemWeaponPhysic.Add ("0,44;6;11,25,10;11,33,8;11,38,6");
            CraftItemWeaponPhysic.Add ("0,45;7;11,26,10;11,34,7;11,38,7");
            CraftItemWeaponPhysic.Add ("0,46;8;11,27,10;11,34,7;11,38,8");
            CraftItemWeaponPhysic.Add ("0,47;9;11,28,10;11,35,6;11,38,9");
            CraftItemWeaponPhysic.Add ("0,48;10;11,29,10;11,35,6;11,38,10");
            CraftItemWeaponPhysic.Add ("0,49;11;11,30,10;11,36,5;11,39,1");
            //Cung
            CraftItemWeaponPhysic.Add ("0,50;1;11,20,10;11,31,10;11,38,1");
            CraftItemWeaponPhysic.Add ("0,51;2;11,21,10;11,31,10;11,38,2");
            CraftItemWeaponPhysic.Add ("0,52;3;11,22,10;11,32,9;11,38,3");
            CraftItemWeaponPhysic.Add ("0,53;4;11,23,10;11,32,9;11,38,4");
            CraftItemWeaponPhysic.Add ("0,54;5;11,24,10;11,33,8;11,38,5");
            CraftItemWeaponPhysic.Add ("0,55;6;11,25,10;11,33,8;11,38,6");
            CraftItemWeaponPhysic.Add ("0,56;7;11,26,10;11,34,7;11,38,7");
            CraftItemWeaponPhysic.Add ("0,57;8;11,27,10;11,34,7;11,38,8");
            CraftItemWeaponPhysic.Add ("0,58;9;11,28,10;11,35,6;11,38,9");
            //Súng
            CraftItemWeaponPhysic.Add ("0,59;1;11,20,10;11,31,10;11,38,1");
            CraftItemWeaponPhysic.Add ("0,60;2;11,21,10;11,31,10;11,38,2");
            CraftItemWeaponPhysic.Add ("0,61;3;11,22,10;11,32,9;11,38,3");
            CraftItemWeaponPhysic.Add ("0,62;4;11,23,10;11,32,9;11,38,4");
            CraftItemWeaponPhysic.Add ("0,63;5;11,24,10;11,33,8;11,38,5");
            CraftItemWeaponPhysic.Add ("0,64;6;11,25,10;11,33,8;11,38,6");
            CraftItemWeaponPhysic.Add ("0,65;7;11,26,10;11,34,7;11,38,7");
            CraftItemWeaponPhysic.Add ("0,66;8;11,27,10;11,34,7;11,38,8");
        }

        #endregion

        #region Vũ khí phép 

        if (CraftItemWeaponMagic == null) {
            CraftItemWeaponMagic = new List<string> ();
            //Cấu trúc: A,F;E;F,B,C;F,B,C
            //A: Loại item
            //F: ID item
            //B: ID item cần thiết
            //C: số lượng của item đó
            //E: Cấp độ để tạo ra chỉ số, cấp càng cao thì chỉ số càng cao
            //Trượng phép
            CraftItemWeaponMagic.Add ("1,0;1;11,23,10;11,33,20;11,38,3");
            CraftItemWeaponMagic.Add ("1,1;2;11,24,10;11,34,20;11,38,4");
            CraftItemWeaponMagic.Add ("1,2;3;11,25,10;11,35,20;11,38,5");
            CraftItemWeaponMagic.Add ("1,3;4;11,26,10;11,36,20;11,38,6");
            CraftItemWeaponMagic.Add ("1,4;5;11,27,10;11,37,20;11,38,7");
            CraftItemWeaponMagic.Add ("1,5;6;11,28,10;10,11,3;11,39,2");
            CraftItemWeaponMagic.Add ("1,6;7;11,29,10;10,12,3;11,39,3");
            CraftItemWeaponMagic.Add ("1,7;8;11,30,10;10,13,3;11,39,4");
            //Áo phép
            CraftItemWeaponMagic.Add ("2,0;1;11,23,10;11,33,20;11,38,3");
            CraftItemWeaponMagic.Add ("2,1;2;11,24,10;11,34,20;11,38,4");
            CraftItemWeaponMagic.Add ("2,2;3;11,25,10;11,35,20;11,38,5");
            CraftItemWeaponMagic.Add ("2,3;4;11,26,10;11,36,20;11,38,6");
            CraftItemWeaponMagic.Add ("2,4;5;11,27,10;11,37,20;11,38,7");
            CraftItemWeaponMagic.Add ("2,5;6;11,28,10;12,11,3;11,39,2");
            //Nhẫn phép
            CraftItemWeaponMagic.Add ("3,0;1;11,23,10;11,33,20;11,38,3");
            CraftItemWeaponMagic.Add ("3,1;2;11,24,10;11,34,20;11,38,4");
            CraftItemWeaponMagic.Add ("3,2;3;11,25,10;11,35,20;11,38,5");
            CraftItemWeaponMagic.Add ("3,3;4;11,26,10;11,36,20;11,38,6");
            CraftItemWeaponMagic.Add ("3,4;5;11,27,10;11,37,20;11,38,7");
        }

        #endregion

        #region Trang bị phòng thủ 

        if (CraftItemDefense == null) {
            CraftItemDefense = new List<string> ();
            //Cấu trúc: A,F;E;F,B,C;F,B,C
            //A: Loại item
            //F: ID item
            //B: ID item cần thiết
            //C: số lượng của item đó
            //E: Cấp độ để tạo ra chỉ số, cấp càng cao thì chỉ số càng cao
            //Áo giáp
            CraftItemDefense.Add ("4,0;1;11,14,10;11,20,10;11,38,1");
            CraftItemDefense.Add ("4,1;2;11,14,15;11,21,10;11,38,2");
            CraftItemDefense.Add ("4,2;3;11,15,10;11,22,10;11,38,3");
            CraftItemDefense.Add ("4,3;4;11,15,15;11,23,10;11,38,4");
            CraftItemDefense.Add ("4,4;5;11,16,10;11,24,10;11,38,5");
            CraftItemDefense.Add ("4,5;6;11,16,15;11,25,10;11,38,6");
            CraftItemDefense.Add ("4,6;7;11,17,10;11,26,10;11,38,7");
            CraftItemDefense.Add ("4,7;8;11,17,15;11,27,10;11,38,8");
            CraftItemDefense.Add ("4,8;9;11,18,10;11,28,10;11,38,9");
            CraftItemDefense.Add ("4,9;10;11,19,10;11,29,10;11,38,10");
            //Đai lưng
            CraftItemDefense.Add ("5,0;1;11,14,10;11,20,10;11,38,1");
            CraftItemDefense.Add ("5,1;2;11,14,15;11,21,10;11,38,2");
            CraftItemDefense.Add ("5,2;3;11,15,10;11,22,10;11,38,3");
            CraftItemDefense.Add ("5,3;4;11,15,15;11,23,10;11,38,4");
            CraftItemDefense.Add ("5,4;5;11,16,10;11,24,10;11,38,5");
            CraftItemDefense.Add ("5,5;6;11,16,15;11,25,10;11,38,6");
            //Giáp tay
            CraftItemDefense.Add ("6,0;1;11,14,10;11,20,10;11,38,1");
            CraftItemDefense.Add ("6,1;2;11,14,15;11,21,10;11,38,2");
            CraftItemDefense.Add ("6,2;3;11,15,10;11,22,10;11,38,3");
            CraftItemDefense.Add ("6,3;4;11,15,15;11,23,10;11,38,4");
            CraftItemDefense.Add ("6,4;5;11,16,10;11,24,10;11,38,5");
            CraftItemDefense.Add ("6,5;6;11,16,15;11,25,10;11,38,6");
            CraftItemDefense.Add ("6,6;7;11,17,10;11,26,10;11,38,7");
            CraftItemDefense.Add ("6,7;8;11,17,15;11,27,10;11,38,8");
            CraftItemDefense.Add ("6,8;9;11,18,10;11,28,10;11,38,9");
            CraftItemDefense.Add ("6,9;10;11,19,10;11,29,10;11,38,10");
            //Găng tay
            CraftItemDefense.Add ("7,0;1;11,14,10;11,20,10;11,38,1");
            CraftItemDefense.Add ("7,1;2;11,14,15;11,21,10;11,38,2");
            CraftItemDefense.Add ("7,2;3;11,15,10;11,22,10;11,38,3");
            CraftItemDefense.Add ("7,3;4;11,15,15;11,23,10;11,38,4");
            CraftItemDefense.Add ("7,4;5;11,16,10;11,24,10;11,38,5");
            //Khiên
            CraftItemDefense.Add ("8,0;1;11,14,10;11,20,10;11,38,1");
            CraftItemDefense.Add ("8,1;2;11,14,15;11,21,10;11,38,2");
            CraftItemDefense.Add ("8,2;3;11,15,10;11,22,10;11,38,3");
            CraftItemDefense.Add ("8,3;4;11,15,15;11,23,10;11,38,4");
            CraftItemDefense.Add ("8,4;5;11,16,10;11,24,10;11,38,5");
        }

        #endregion

        #region Set đồ 

        // if (CraftItemSets == null) {
        //     CraftItemSets = new List<string> ();
        //     //Cấu trúc: A;B,C;B,C
        //     //A: ID item sẽ được tạo
        //     //B: ID item cần thiết
        //     //C: số lượng của item đó
        //     //set đồ 1
        //     CraftItemSets.Add ("500;10;67,1;350,20;354,5");
        //     CraftItemSets.Add ("501;10;155,1;350,20;354,5");
        //     CraftItemSets.Add ("502;10;165,1;353,20;354,5");
        //     CraftItemSets.Add ("503;10;184,1;353,20;354,5");
        //     CraftItemSets.Add ("504;10;179,1;353,20;354,5");
        //     CraftItemSets.Add ("505;10;325,10;353,20;354,5");
        // }

        #endregion

    }
}