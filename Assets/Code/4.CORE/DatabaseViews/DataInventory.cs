using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInventory {

    public List<string> SecurityCode;//Mã guid lưu ID lần cuối cùng thao tác để chống hack
    public List<ItemModel> DBItems = new List<ItemModel>();
}
