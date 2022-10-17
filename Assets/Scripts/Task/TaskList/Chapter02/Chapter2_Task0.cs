using Interaction;
using UnityEngine;

namespace Task
{
    //�½�2��һ�£�Ҳ��������һ�´��ͼ�̳���Ͷ���
    public class Chapter2_Task0 : ChapterPart
    {
        public Chapter2_Task0()
        {
            partDescription = "ǰ����̳";
        }

        float moveTime = 3f;
        float nowRadio;

        public override void EnterTaskEvent(Chapter chapter, bool isLoaded)
        {
            Debug.Log("��ʼ�����µ�һ��");
            this.chapter = chapter;
            Common.SustainCoroutine.Instance.AddCoroutine(ShowHint);
            nowRadio = 0;
            Common.SustainCoroutine.Instance.AddCoroutine(CreateAlterTrigger);
        }

        //��ʾ��ʾ
        bool ShowHint()
        {
            UI.SmallDialog.Instance.ShowSmallDialog(
                chapter.GetDiglogText(0), null);
            //�����Ʒ
            Package.PackageSimple.Instance.AddItem<Package.Letter>("Letter");
            return true;
        }

        //�ڼ�̳�����һ��������
        bool CreateAlterTrigger()
        {
            GameObject game = new GameObject("TempTrigger");
            game.transform.position = new Vector3(24, 6, 275);
            SphereCollider sphere = game.AddComponent<SphereCollider>();
            sphere.radius = 30; sphere.isTrigger = true;
            TrigerInteracteDelegate triger = game.AddComponent<TrigerInteracteDelegate>();
            triger.trigerDelegate = () =>
            {
                GameObject animate = Common.SceneObjectMap.Instance.FindControlObject("AlterAnimate");
                ScriptAnimate.ScriptAnimateControl animateControl = 
                    animate.GetComponent<ScriptAnimate.ScriptAnimateControl>();
                animateControl.BeginUse();
                Common.SustainCoroutine.Instance.AddCoroutine(MoveCameraToTarget);
                AsynTaskControl.Instance.CheckChapter(chapter.chapterID,
                   new InteracteInfo
                   {
                       data = "2_0"
                   });
            };
            return true;
        }

        Vector3 target = new Vector3(46, 0.5f, 275);
        Quaternion targetQuater = Quaternion.Euler(new Vector3(-35, -90, 0));

        bool MoveCameraToTarget()
        {
            nowRadio += Time.deltaTime * (1.0f / moveTime);
            Transform camTrans = Camera.main.transform;
            if(nowRadio >= 1.0f)
            {
                camTrans.position = target;
                camTrans.rotation = targetQuater;
                return true;
            }
            camTrans.position = Vector3.Lerp(camTrans.position, target, nowRadio);
            camTrans.rotation = Quaternion.Lerp(camTrans.rotation, targetQuater, nowRadio);
            return false;
        }

        public override void ExitTaskEvent(Chapter chapter)
        {
            Debug.Log("�������");
        }

        public override bool IsCompleteTask(Chapter chapter, InteracteInfo info)
        {
            return true;
        }
    }
}