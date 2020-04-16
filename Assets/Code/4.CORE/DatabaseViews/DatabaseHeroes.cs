using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseHeroes {
    public List<string> SecurityCode;//Mã guid lưu ID lần cuối cùng thao tác để chống hack
    public List<HeroesProperties> DBHeroes = new List<HeroesProperties>();
}
