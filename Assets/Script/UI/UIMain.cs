using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMain : UIBase {

    private void Awake()
    {
    }

    public override void OnOpen()
    {
        //刷新UI
        this.RefreshUI();
    }

    public void RefreshUI()
    {
        
        //设置名字
        var nameText = transform.Find("PlayerInfo/PlayerName").GetComponent<Text>();
        nameText.text = PlayerData.userName;
        //头像
        if (!string.IsNullOrEmpty(PlayerData.userIcon))
        {
            var headIcon = transform.Find("PlayerInfo/HeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, PlayerData.userIcon));
        }
        //体力
        var energyText = transform.Find("Energy/Number").GetComponent<Text>();
        energyText.text = PlayerData.energy.ToString();

        //钻石
        var gemsText = transform.Find("Gems/Number").GetComponent<Text>();
        gemsText.text = PlayerData.gems.ToString();

        //等级
        var levelText = transform.Find("PlayerInfo/Level").GetComponent<Text>();
        levelText.text = Function.LevelToString(PlayerData.level);

        //体力
        var proficiencyText = transform.Find("PlayerInfo/Proficiency").GetComponent<Text>();
        proficiencyText.text = Function.ProficiencyToString(PlayerData.proficiency);

        //六边形
        var polygon = transform.Find("Hexagon/Polygon").GetComponent<Polygon>();
        polygon.SetValue(new float[] {PlayerData.speed, PlayerData.judgment, PlayerData.calculate, PlayerData.accuracy, PlayerData.observation, PlayerData.memory});

    }
}
