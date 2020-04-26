using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class UserSystem
{
    public static readonly int GemsDefault = 3000;//Giá trị gem khi tạo acc
    public static readonly int GoldsDefault = 50000;//Giá trị gold khi tạo acc
    /// <summary>
    /// Kiểm tra số lượng đá quý
    /// </summary>
    public static bool CheckGems(float quantity)
    {
        if (DataUserController.User.Gems >= quantity)
        {
            return true;
        }
        GameSystem.ControlFunctions.ShowMessage(Languages.lang[250]);//"Bạn không đủ đá quý";
        return false;
    }

    /// <summary>
    /// Kiểm tra số lượng vàng
    /// </summary>
    public static bool CheckGolds(float quantity)
    {
        if (DataUserController.User.Golds >= quantity)
        {
            return true;
        }
        GameSystem.ControlFunctions.ShowMessage(Languages.lang[304]);//"Bạn không đủ vàng";
        return false;
    }

    /// <summary>
    /// Kiểm tra số lượng vàng và đá quý
    /// </summary>
    public static bool CheckGoldsGems(float goldsQuantity, float gemsQuantity)
    {
        if (DataUserController.User.Golds >= goldsQuantity && DataUserController.User.Gems >= gemsQuantity)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Giảm trừ vàng
    /// </summary>
    public static bool DecreaseGolds(float quantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Golds -= quantity;
            if (showMessage)
            GameSystem.ControlFunctions.ShowMessage(new StringBuilder("- ").Append(quantity.ToString()).Append(Languages.lang[276]).ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Giảm trừ đá quý
    /// </summary>
    public static bool DecreaseGems(float quantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Gems -= quantity;
            if (showMessage)
            GameSystem.ControlFunctions.ShowMessage(new StringBuilder("- ").Append(quantity.ToString()).Append(Languages.lang[277]).ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Giảm trừ tiền tệ
    /// </summary>
    public static bool DecreaseGoldsGems(float goldsQuantity, float gemsQuantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Golds -= goldsQuantity;
            DataUserController.User.Gems -= gemsQuantity;
            if (showMessage)
            GameSystem.ControlFunctions.ShowManyMessage(new string[] { new StringBuilder("- ").Append(goldsQuantity.ToString()).Append(Languages.lang[276]).ToString(), new StringBuilder("- ").Append(gemsQuantity.ToString()).Append(Languages.lang[277]).ToString() });
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cộng golds cho user
    /// </summary>
    public static bool IncreaseGolds(float quantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Golds += quantity;
            if (showMessage)
            GameSystem.ControlFunctions.ShowMessage(new StringBuilder("+ ").Append(quantity.ToString()).Append(Languages.lang[276]).ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cộng gems cho user
    /// </summary>
    public static bool IncreaseGems(float quantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Gems += quantity;
            if (showMessage)
                GameSystem.ControlFunctions.ShowMessage(new StringBuilder("+ ").Append(quantity.ToString()).Append(Languages.lang[277]).ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cộng tiền tệ cho user
    /// </summary>
    public static bool IncreaseGoldsGems(float goldsQuantity, float gemsQuantity, bool showMessage)
    {
        try
        {
            DataUserController.User.Golds += goldsQuantity;
            DataUserController.User.Gems += gemsQuantity;
            if(showMessage)
            GameSystem.ControlFunctions.ShowManyMessage(new string[] { new StringBuilder("+ ").Append(goldsQuantity.ToString()).Append(Languages.lang[276]).ToString(), new StringBuilder("+ ").Append(gemsQuantity.ToString()).Append(Languages.lang[277]).ToString() });
            return true;
        }
        catch
        {
            return false;
        }
    }
}