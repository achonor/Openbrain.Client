using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReady : UIBase {

    private LoopHorizontalScrollRect emojiViewList;
    private void Awake()
    {
        emojiViewList = transform.Find("Emoji/EmojiList").GetComponent<LoopHorizontalScrollRect>();
        emojiViewList.RegisterInitCallback(CreateEmoji);
    }
    public override void OnOpen()
    {
        base.OnOpen();
        //表情包
        emojiViewList.totalCount = 10;
        emojiViewList.RefillCells();
    }

    public void RefreshUI(rep_message_start_ready readyInfo)
    {

    }


    public void CreateEmoji(Transform obj, int index)
    {

    }
}
