using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCoreSetting {
    public static List<CraftModel> CraftItemWeaponPhysic;
    public static List<CraftModel> CraftItemWeaponMagic;
    public static List<CraftModel> CraftItemDefense;
    public static List<CraftModel> CraftItemUse;
    public static List<CraftModel> CraftItemSets;

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
            //D: Giá tiền để chế tạo
            CraftItemUse = new List<CraftModel> ();

            #region Bình máu 
            CraftItemUse.Add (new CraftModel (10, 0, 1, 1000, new int[] { 11, 11 }, new int[] { 0, 1 }, new int[] { 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 1, 1, 1500, new int[] { 11, 11, 11 }, new int[] { 0, 1, 2 }, new int[] { 1, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 2, 1, 2000, new int[] { 11, 11, 11 }, new int[] { 1, 2, 3 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 3, 1, 2500, new int[] { 11, 11, 11 }, new int[] { 2, 3, 4 }, new int[] { 1, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 4, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 4, 5, 6 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 5, 1, 3500, new int[] { 11, 11, 11 }, new int[] { 4, 6, 7 }, new int[] { 1, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 6, 1, 4000, new int[] { 11, 11, 11 }, new int[] { 7, 8, 9 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 7, 1, 4500, new int[] { 11, 11, 11 }, new int[] { 9, 10, 11 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 8, 1, 5000, new int[] { 11, 11, 11 }, new int[] { 11, 12, 13 }, new int[] { 6, 5, 4 }));

            #endregion

            #region Đá cường hóa 
            CraftItemUse.Add (new CraftModel (10, 9, 1, 2000, new int[] { 11, 11 }, new int[] { 20, 21 }, new int[] { 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 10, 1, 3000, new int[] { 10, 11, 11 }, new int[] { 9, 21, 22 }, new int[] { 1, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 11, 1, 4000, new int[] { 11, 11, 11 }, new int[] { 21, 22, 23 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 12, 1, 5000, new int[] { 11, 11, 11 }, new int[] { 22, 23, 24 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 13, 1, 6000, new int[] { 11, 11, 11 }, new int[] { 23, 24, 25 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 14, 1, 7000, new int[] { 11, 11, 11 }, new int[] { 24, 25, 26 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 15, 1, 8000, new int[] { 11, 11, 11 }, new int[] { 25, 26, 27 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 16, 1, 9000, new int[] { 11, 11, 11 }, new int[] { 26, 27, 28 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 17, 1, 10000, new int[] { 11, 11, 11 }, new int[] { 27, 28, 29 }, new int[] { 6, 5, 4 }));
            CraftItemUse.Add (new CraftModel (10, 18, 1, 11000, new int[] { 11, 11, 11 }, new int[] { 28, 29, 30 }, new int[] { 6, 5, 4 }));

            #endregion

            #region Đá nâng phẩm 
            CraftItemUse.Add (new CraftModel (10, 19, 1, 5000, new int[] { 11, 11, 11 }, new int[] { 29, 44, 39 }, new int[] { 1, 1, 1 }));
            CraftItemUse.Add (new CraftModel (10, 20, 1, 10000, new int[] { 11, 11, 11 }, new int[] { 30, 44, 39 }, new int[] { 1, 2, 2 }));
            CraftItemUse.Add (new CraftModel (10, 21, 1, 15000, new int[] { 11, 11, 11 }, new int[] { 40, 44, 39 }, new int[] { 1, 3, 3 }));
            CraftItemUse.Add (new CraftModel (10, 22, 1, 20000, new int[] { 11, 11, 11 }, new int[] { 41, 44, 39 }, new int[] { 1, 4, 4 }));
            CraftItemUse.Add (new CraftModel (10, 23, 1, 25000, new int[] { 11, 11, 11 }, new int[] { 42, 44, 39 }, new int[] { 1, 5, 5 }));
            CraftItemUse.Add (new CraftModel (10, 24, 1, 30000, new int[] { 11, 11, 11 }, new int[] { 43, 44, 39 }, new int[] { 1, 6, 6 }));

            #endregion

            //Khuôn đục lỗ
            CraftItemUse.Add (new CraftModel (10, 26, 1, 15000, new int[] { 10, 11, 11 }, new int[] { 9, 27, 39 }, new int[] { 1, 1, 1 }));

        }
    }

    /// <summary>
    /// Khởi tạo template cho item equip
    /// </summary>
    private static void CreateItemEquip () {

        #region Vũ khí vật lý 

        if (CraftItemWeaponPhysic == null) {
            CraftItemWeaponPhysic = new List<CraftModel> ();
            //Template
            //CraftItemWeaponPhysic.Add (new CraftModel (-1, -1, -1, 1000, new int[] { -1, -1, -1 }, new int[] { -1, -1, -1 }, new int[] { -1, -1, -1}));
            //Dao nhỏ
            CraftItemWeaponPhysic.Add (new CraftModel (0, 0, 1, 1000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 7, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 1, 2, 2000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 6, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 22, 31, 38 }, new int[] { 10, 5, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 3, 4, 4000, new int[] { 11, 11, 11 }, new int[] { 23, 31, 38 }, new int[] { 10, 4, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 4, 5, 5000, new int[] { 11, 11, 11 }, new int[] { 24, 31, 38 }, new int[] { 10, 3, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 5, 6, 6000, new int[] { 11, 11, 11 }, new int[] { 25, 31, 38 }, new int[] { 10, 2, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 6, 7, 7000, new int[] { 11, 11, 11 }, new int[] { 26, 31, 38 }, new int[] { 10, 1, 5 }));

            //Kiếm
            CraftItemWeaponPhysic.Add (new CraftModel (0, 7, 1, 2000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 8, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 8, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 7, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 9, 3, 4000, new int[] { 11, 11, 11 }, new int[] { 22, 31, 38 }, new int[] { 10, 6, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 10, 4, 5000, new int[] { 11, 11, 11 }, new int[] { 23, 31, 38 }, new int[] { 10, 5, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 11, 5, 6000, new int[] { 11, 11, 11 }, new int[] { 24, 31, 38 }, new int[] { 10, 4, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 12, 6, 7000, new int[] { 11, 11, 11 }, new int[] { 25, 31, 38 }, new int[] { 10, 3, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 13, 7, 8000, new int[] { 11, 11, 11 }, new int[] { 26, 31, 38 }, new int[] { 10, 2, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 14, 8, 9000, new int[] { 11, 11, 11 }, new int[] { 27, 31, 38 }, new int[] { 10, 1, 8 }));

            //Rìu
            CraftItemWeaponPhysic.Add (new CraftModel (0, 15, 1, 2000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 10, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 16, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 10, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 17, 3, 4000, new int[] { 11, 11, 11 }, new int[] { 22, 32, 38 }, new int[] { 10, 9, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 18, 4, 5000, new int[] { 11, 11, 11 }, new int[] { 23, 32, 38 }, new int[] { 10, 9, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 19, 5, 6000, new int[] { 11, 11, 11 }, new int[] { 24, 33, 38 }, new int[] { 10, 8, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 20, 6, 7000, new int[] { 11, 11, 11 }, new int[] { 25, 33, 38 }, new int[] { 10, 8, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 21, 7, 8000, new int[] { 11, 11, 11 }, new int[] { 26, 34, 38 }, new int[] { 10, 7, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 22, 8, 9000, new int[] { 11, 11, 11 }, new int[] { 27, 34, 38 }, new int[] { 10, 7, 8 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 23, 9, 10000, new int[] { 11, 11, 11 }, new int[] { 28, 35, 38 }, new int[] { 10, 6, 9 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 24, 10, 11000, new int[] { 11, 11, 11 }, new int[] { 29, 35, 38 }, new int[] { 10, 6, 10 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 25, 11, 12000, new int[] { 11, 11, 11 }, new int[] { 20, 36, 39 }, new int[] { 10, 5, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 26, 12, 13000, new int[] { 11, 11, 11 }, new int[] { 20, 37, 39 }, new int[] { 15, 5, 2 }));

            //Búa
            CraftItemWeaponPhysic.Add (new CraftModel (0, 27, 1, 2000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 10, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 28, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 10, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 29, 3, 4000, new int[] { 11, 11, 11 }, new int[] { 22, 32, 38 }, new int[] { 10, 9, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 30, 4, 5000, new int[] { 11, 11, 11 }, new int[] { 23, 32, 38 }, new int[] { 10, 9, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 31, 5, 6000, new int[] { 11, 11, 11 }, new int[] { 24, 33, 38 }, new int[] { 10, 8, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 32, 6, 7000, new int[] { 11, 11, 11 }, new int[] { 25, 33, 38 }, new int[] { 10, 8, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 33, 7, 8000, new int[] { 11, 11, 11 }, new int[] { 26, 34, 38 }, new int[] { 10, 7, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 34, 8, 9000, new int[] { 11, 11, 11 }, new int[] { 27, 34, 38 }, new int[] { 10, 7, 8 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 35, 9, 10000, new int[] { 11, 11, 11 }, new int[] { 28, 35, 38 }, new int[] { 10, 6, 9 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 36, 10, 11000, new int[] { 11, 11, 11 }, new int[] { 29, 35, 38 }, new int[] { 10, 6, 10 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 37, 11, 12000, new int[] { 11, 11, 11 }, new int[] { 20, 36, 39 }, new int[] { 10, 5, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 38, 12, 13000, new int[] { 11, 11, 11 }, new int[] { 20, 37, 39 }, new int[] { 15, 5, 2 }));

            //Chùy
            CraftItemWeaponPhysic.Add (new CraftModel (0, 39, 1, 2000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 10, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 40, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 10, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 41, 3, 4000, new int[] { 11, 11, 11 }, new int[] { 22, 32, 38 }, new int[] { 10, 9, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 42, 4, 5000, new int[] { 11, 11, 11 }, new int[] { 23, 32, 38 }, new int[] { 10, 9, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 43, 5, 6000, new int[] { 11, 11, 11 }, new int[] { 24, 33, 38 }, new int[] { 10, 8, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 44, 6, 7000, new int[] { 11, 11, 11 }, new int[] { 25, 33, 38 }, new int[] { 10, 8, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 45, 7, 8000, new int[] { 11, 11, 11 }, new int[] { 26, 34, 38 }, new int[] { 10, 7, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 46, 8, 9000, new int[] { 11, 11, 11 }, new int[] { 27, 34, 38 }, new int[] { 10, 7, 8 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 47, 9, 10000, new int[] { 11, 11, 11 }, new int[] { 28, 35, 38 }, new int[] { 10, 6, 9 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 48, 10, 11000, new int[] { 11, 11, 11 }, new int[] { 29, 35, 38 }, new int[] { 10, 6, 10 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 49, 11, 12000, new int[] { 11, 11, 11 }, new int[] { 30, 36, 39 }, new int[] { 10, 5, 1 }));

            //Cung
            CraftItemWeaponPhysic.Add (new CraftModel (0, 50, 1, 1000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 10, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 51, 2, 2000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 10, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 52, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 22, 32, 38 }, new int[] { 10, 9, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 53, 4, 4000, new int[] { 11, 11, 11 }, new int[] { 23, 32, 38 }, new int[] { 10, 9, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 54, 5, 5000, new int[] { 11, 11, 11 }, new int[] { 24, 33, 38 }, new int[] { 10, 8, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 55, 6, 6000, new int[] { 11, 11, 11 }, new int[] { 25, 33, 38 }, new int[] { 10, 8, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 56, 7, 7000, new int[] { 11, 11, 11 }, new int[] { 26, 34, 38 }, new int[] { 10, 7, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 57, 8, 8000, new int[] { 11, 11, 11 }, new int[] { 27, 34, 38 }, new int[] { 10, 7, 8 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 58, 9, 9000, new int[] { 11, 11, 11 }, new int[] { 28, 35, 38 }, new int[] { 10, 6, 9 }));

            //Súng
            CraftItemWeaponPhysic.Add (new CraftModel (0, 59, 1, 1000, new int[] { 11, 11, 11 }, new int[] { 20, 31, 38 }, new int[] { 10, 10, 1 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 60, 2, 2000, new int[] { 11, 11, 11 }, new int[] { 21, 31, 38 }, new int[] { 10, 10, 2 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 61, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 22, 32, 38 }, new int[] { 10, 9, 3 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 62, 4, 4000, new int[] { 11, 11, 11 }, new int[] { 23, 32, 38 }, new int[] { 10, 9, 4 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 63, 5, 5000, new int[] { 11, 11, 11 }, new int[] { 24, 33, 38 }, new int[] { 10, 8, 5 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 64, 6, 6000, new int[] { 11, 11, 11 }, new int[] { 25, 33, 38 }, new int[] { 10, 8, 6 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 65, 7, 7000, new int[] { 11, 11, 11 }, new int[] { 26, 34, 38 }, new int[] { 10, 7, 7 }));
            CraftItemWeaponPhysic.Add (new CraftModel (0, 66, 8, 8000, new int[] { 11, 11, 11 }, new int[] { 27, 34, 38 }, new int[] { 10, 7, 8 }));

        }

        #endregion

        #region Vũ khí phép 

        if (CraftItemWeaponMagic == null) {
            CraftItemWeaponMagic = new List<CraftModel> ();

            //Trượng phép
            CraftItemWeaponMagic.Add (new CraftModel (1, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 23, 33, 38 }, new int[] { 10, 20, 3 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 1, 2, 4000, new int[] { 11, 11, 11 }, new int[] { 24, 34, 38 }, new int[] { 10, 20, 4 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 2, 3, 5000, new int[] { 11, 11, 11 }, new int[] { 25, 35, 38 }, new int[] { 10, 20, 5 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 3, 4, 6000, new int[] { 11, 11, 11 }, new int[] { 26, 36, 38 }, new int[] { 10, 20, 6 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 4, 5, 7000, new int[] { 11, 11, 11 }, new int[] { 27, 37, 38 }, new int[] { 10, 20, 7 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 5, 6, 8000, new int[] { 11, 11, 11 }, new int[] { 28, 11, 39 }, new int[] { 10, 3, 2 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 6, 7, 9000, new int[] { 11, 11, 11 }, new int[] { 29, 12, 39 }, new int[] { 10, 3, 3 }));
            CraftItemWeaponMagic.Add (new CraftModel (1, 7, 8, 10000, new int[] { 11, 11, 11 }, new int[] { 30, 13, 39 }, new int[] { 10, 3, 4 }));

            //Áo phép
            CraftItemWeaponMagic.Add (new CraftModel (2, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 23, 33, 38 }, new int[] { 10, 20, 3 }));
            CraftItemWeaponMagic.Add (new CraftModel (2, 1, 2, 4000, new int[] { 11, 11, 11 }, new int[] { 23, 34, 38 }, new int[] { 10, 20, 4 }));
            CraftItemWeaponMagic.Add (new CraftModel (2, 2, 3, 5000, new int[] { 11, 11, 11 }, new int[] { 23, 35, 38 }, new int[] { 10, 20, 5 }));
            CraftItemWeaponMagic.Add (new CraftModel (2, 3, 4, 6000, new int[] { 11, 11, 11 }, new int[] { 23, 36, 38 }, new int[] { 10, 20, 6 }));
            CraftItemWeaponMagic.Add (new CraftModel (2, 4, 5, 7000, new int[] { 11, 11, 11 }, new int[] { 23, 37, 38 }, new int[] { 10, 20, 7 }));
            CraftItemWeaponMagic.Add (new CraftModel (2, 5, 6, 8000, new int[] { 11, 12, 11 }, new int[] { 23, 11, 39 }, new int[] { 10, 3, 2 }));

            //Nhẫn phép
            CraftItemWeaponMagic.Add (new CraftModel (3, 0, 1, 1000, new int[] { 11, 11, 11 }, new int[] { 23, 33, 38 }, new int[] { 10, 20, 3 }));
            CraftItemWeaponMagic.Add (new CraftModel (3, 1, 2, 2000, new int[] { 11, 11, 11 }, new int[] { 24, 34, 38 }, new int[] { 10, 20, 4 }));
            CraftItemWeaponMagic.Add (new CraftModel (3, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 25, 35, 38 }, new int[] { 10, 20, 5 }));
            CraftItemWeaponMagic.Add (new CraftModel (3, 3, 4, 4000, new int[] { 11, 11, 11 }, new int[] { 26, 36, 38 }, new int[] { 10, 20, 6 }));
            CraftItemWeaponMagic.Add (new CraftModel (3, 4, 5, 5000, new int[] { 11, 11, 11 }, new int[] { 27, 37, 38 }, new int[] { 10, 20, 7 }));

        }

        #endregion

        #region Trang bị phòng thủ 

        if (CraftItemDefense == null) {
            CraftItemDefense = new List<CraftModel> ();
            //Cấu trúc: A,F;E;F,B,C;F,B,C
            //A: Loại item
            //F: ID item
            //B: ID item cần thiết
            //C: số lượng của item đó
            //E: Cấp độ để tạo ra chỉ số, cấp càng cao thì chỉ số càng cao
            //Áo giáp
            CraftItemDefense.Add (new CraftModel (4, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 20, 38 }, new int[] { 10, 10, 1 }));
            CraftItemDefense.Add (new CraftModel (4, 1, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 21, 38 }, new int[] { 15, 10, 2 }));
            CraftItemDefense.Add (new CraftModel (4, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 22, 38 }, new int[] { 10, 10, 3 }));
            CraftItemDefense.Add (new CraftModel (4, 3, 4, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 23, 38 }, new int[] { 15, 10, 4 }));
            CraftItemDefense.Add (new CraftModel (4, 4, 5, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 24, 38 }, new int[] { 10, 10, 5 }));
            CraftItemDefense.Add (new CraftModel (4, 5, 6, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 25, 38 }, new int[] { 15, 10, 6 }));
            CraftItemDefense.Add (new CraftModel (4, 6, 7, 3000, new int[] { 11, 11, 11 }, new int[] { 17, 26, 38 }, new int[] { 10, 10, 7 }));
            CraftItemDefense.Add (new CraftModel (4, 7, 8, 3000, new int[] { 11, 11, 11 }, new int[] { 17, 27, 38 }, new int[] { 15, 10, 8 }));
            CraftItemDefense.Add (new CraftModel (4, 8, 9, 3000, new int[] { 11, 11, 11 }, new int[] { 18, 28, 38 }, new int[] { 10, 10, 9 }));
            CraftItemDefense.Add (new CraftModel (4, 9, 10, 3000, new int[] { 11, 11, 11 }, new int[] { 19, 29, 38 }, new int[] { 10, 10, 10 }));

            //Đai lưng
            CraftItemDefense.Add (new CraftModel (5, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 20, 38 }, new int[] { 10, 10, 1 }));
            CraftItemDefense.Add (new CraftModel (5, 1, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 21, 38 }, new int[] { 15, 10, 2 }));
            CraftItemDefense.Add (new CraftModel (5, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 22, 38 }, new int[] { 10, 10, 3 }));
            CraftItemDefense.Add (new CraftModel (5, 3, 4, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 23, 38 }, new int[] { 15, 10, 4 }));
            CraftItemDefense.Add (new CraftModel (5, 4, 5, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 24, 38 }, new int[] { 10, 10, 5 }));
            CraftItemDefense.Add (new CraftModel (5, 5, 6, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 25, 38 }, new int[] { 15, 10, 6 }));

            //Giáp tay
            CraftItemDefense.Add (new CraftModel (6, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 20, 38 }, new int[] { 10, 10, 1 }));
            CraftItemDefense.Add (new CraftModel (6, 1, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 21, 38 }, new int[] { 15, 10, 2 }));
            CraftItemDefense.Add (new CraftModel (6, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 22, 38 }, new int[] { 10, 10, 3 }));
            CraftItemDefense.Add (new CraftModel (6, 3, 4, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 23, 38 }, new int[] { 15, 10, 4 }));
            CraftItemDefense.Add (new CraftModel (6, 4, 5, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 24, 38 }, new int[] { 10, 10, 5 }));
            CraftItemDefense.Add (new CraftModel (6, 5, 6, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 25, 38 }, new int[] { 15, 10, 6 }));
            CraftItemDefense.Add (new CraftModel (6, 6, 7, 3000, new int[] { 11, 11, 11 }, new int[] { 17, 26, 38 }, new int[] { 10, 10, 7 }));
            CraftItemDefense.Add (new CraftModel (6, 7, 8, 3000, new int[] { 11, 11, 11 }, new int[] { 17, 27, 38 }, new int[] { 15, 10, 8 }));
            CraftItemDefense.Add (new CraftModel (6, 8, 9, 3000, new int[] { 11, 11, 11 }, new int[] { 18, 28, 38 }, new int[] { 10, 10, 9 }));
            CraftItemDefense.Add (new CraftModel (6, 9, 10, 3000, new int[] { 11, 11, 11 }, new int[] { 19, 29, 38 }, new int[] { 10, 10, 10 }));

            //Găng tay
            CraftItemDefense.Add (new CraftModel (7, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 20, 38 }, new int[] { 10, 10, 1 }));
            CraftItemDefense.Add (new CraftModel (7, 1, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 21, 38 }, new int[] { 15, 10, 2 }));
            CraftItemDefense.Add (new CraftModel (7, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 22, 38 }, new int[] { 10, 10, 3 }));
            CraftItemDefense.Add (new CraftModel (7, 3, 4, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 23, 38 }, new int[] { 15, 10, 4 }));
            CraftItemDefense.Add (new CraftModel (7, 4, 5, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 24, 38 }, new int[] { 10, 10, 5 }));

            //Khiên
            CraftItemDefense.Add (new CraftModel (8, 0, 1, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 20, 38 }, new int[] { 10, 10, 1 }));
            CraftItemDefense.Add (new CraftModel (8, 1, 2, 3000, new int[] { 11, 11, 11 }, new int[] { 14, 21, 38 }, new int[] { 15, 10, 2 }));
            CraftItemDefense.Add (new CraftModel (8, 2, 3, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 22, 38 }, new int[] { 10, 10, 3 }));
            CraftItemDefense.Add (new CraftModel (8, 3, 4, 3000, new int[] { 11, 11, 11 }, new int[] { 15, 23, 38 }, new int[] { 15, 10, 4 }));
            CraftItemDefense.Add (new CraftModel (8, 4, 5, 3000, new int[] { 11, 11, 11 }, new int[] { 16, 24, 38 }, new int[] { 10, 10, 5 }));

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