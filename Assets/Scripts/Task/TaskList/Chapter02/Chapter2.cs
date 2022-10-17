﻿using UnityEngine;

namespace Task
{
    /// <summary>
    /// 第三章，获得前往下一个场景的机会，也就是只有一个技能
    /// </summary>
    public class Chapter2 : AsynChapterBase
    {

        public Chapter2()
        {
            chapterName = "祭往开来";
            chapterDescription = "神秘祭坛，谜之信件，到底有什么关联";
            taskPartCount = 1;
            chapterID = 2;

            chapterSavePath = Application.streamingAssetsPath + "/Task/Chapter/2.task";
            targetPart = targetPart + "Chapter2_Task";
            runtimeScene = "MainScene";
        }

        public override void CheckAndLoadChapter()
        {
            if (AsynTaskControl.Instance.CheckChapterIsComplete(1))
                AsynTaskControl.Instance.AddChapter(this);
            return;
        }

        public override void CompleteChapter(bool isInThisScene)
        {
            Debug.Log("第三章结束");
        }

        public override void ExitChapter()
        {
            AsynTaskControl.Instance.AddChapter(3);
            return;
        }
    }
}