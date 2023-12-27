using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaterialSelect : MonoBehaviour
{
    public AlembicPlayScene UIToggles;
    public Define.PatternType PatternType;

    //public void InstantiateMaterialUI()
    //{
    //    var array = System.Enum.GetValues(typeof(Define.PatternType));
    //    int length = array.Length;

    //    for (int i = 0; i < length; i++)
    //    {
    //        GameObject go = Instantiate(_prefab, gameObject.transform);
    //        go.name = array.GetValue(i).ToString() + "_Button";

    //        go.GetComponent<Button>().onClick.AddListener(() => {  });
    //    }
    //}

    public void ToggleCheck()
    {
        if (UIToggles.TopToggle.isOn) Managers.Player.ChangeMaterial(Define.ClothesType.Top, PatternType);
        if (UIToggles.BotToggle.isOn) Managers.Player.ChangeMaterial(Define.ClothesType.Bottom, PatternType);
        if (UIToggles.AccToggle.isOn) Managers.Player.ChangeMaterial(Define.ClothesType.Accessory, PatternType);
    }
}
