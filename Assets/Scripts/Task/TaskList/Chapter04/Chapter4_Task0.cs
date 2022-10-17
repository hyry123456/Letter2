using Interaction;
using UnityEngine;
using Common;

namespace Task
{

    public class Chapter4_Task0 : ChapterPart
    {
        GameObject childe;
        NPC_Pooling child_Pooling;


        public override void EnterTaskEvent(Chapter chapter, bool isLoaded)
        {
            base.EnterTaskEvent(chapter, isLoaded);
            if (!isLoaded)
                SustainCoroutine.Instance.AddCoroutine(CreateChildAndDialog);
            else
                SustainCoroutine.Instance.AddCoroutine(CreateChilde);
            SustainCoroutine.Instance.AddCoroutine(AddNPC);
        }

        //������ү�������һ����ү�ĶԻ�������ǵ�һ�μ���ʱ���е�
        bool CreateChildAndDialog()
        {
            if (childe == null)
            {
                childe = Resources.Load<GameObject>("Prefab/Character/Childe");
                return false;
            }
            child_Pooling = SceneObjectPool.Instance.GetObject<NPC_Pooling>(
                "Childe", childe, new Vector3(155, 7.6f, 110), new Vector3(156, 7.6f, 110));
            InteracteDelegate interacte = child_Pooling.gameObject.AddComponent<InteracteDelegate>();
            interacte.interDelegate = () =>
            {
                UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(0),
                    //�ı佻��
                    () =>
                    {
                        GameObject.Destroy(interacte);      //�����ǰ����
                        //��һ������
                        interacte = child_Pooling.gameObject.AddComponent<InteracteDelegate>();
                        interacte.interDelegate = () =>
                        {
                            AsynTaskControl.Instance.CheckChapter(chapter.chapterID,
                                new InteracteInfo { data = "4_0" });
                        };


                    });
            };
            return true;
        }

        //������ү���ڷǵ�һ��ʱ����
        bool CreateChilde()
        {
            if (childe == null)
            {
                childe = Resources.Load<GameObject>("Prefab/Character/Childe");
                return false;
            }
            child_Pooling = SceneObjectPool.Instance.GetObject<NPC_Pooling>(
                "Childe", childe, new Vector3(155, 7.6f, 110), new Vector3(156, 7.6f, 110));
            InteracteDelegate interacte = child_Pooling.gameObject.AddComponent<InteracteDelegate>();
            interacte.interDelegate = () =>
            {
                AsynTaskControl.Instance.CheckChapter(chapter.chapterID,
                    new InteracteInfo { data = "4_0" });
            };
            //�ƶ�����
            Control.PlayerControl.Instance.transform.position = new Vector3(210, 1, 108);

            return true;
        }

        //��������
        bool AddNPC()
        {
            //��ӻ�ȡ�ż�����
            GameObject addLetter = SceneObjectMap.Instance.FindControlObject("Chapter4_AddLetter");
            addLetter.AddComponent<InteracteDelegate>().interDelegate = () =>
            {
                Package.PackageSimple.Instance.AddItem<Package.SkillLetter>("SkillLetter");
                UI.SmallDialog.Instance.ShowSmallDialog(chapter.GetDiglogText(1), null);
            };

            //�������֮��ǰ���NPC
            GameObject oldMan = SceneObjectMap.Instance.FindControlObject("OldMan");
            oldMan.AddComponent<InteracteDelegate>().interDelegate = () =>
            {
                UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(4), null);
            };
            return true;
        }

        public override void ExitTaskEvent(Chapter chapter)
        {
            //�ر���ү
            child_Pooling?.CloseObject();
        }

        public override bool IsCompleteTask(Chapter chapter, InteracteInfo info)
        {
            //һ��ʼ�Ľ���
            if(info.data == "4_0")
            {
                if (Package.PackageSimple.Instance.CheckItemByName("ף�������ص��ż�"))
                {
                    UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(3),
                        () =>
                        {
                            AsynTaskControl.Instance.CheckChapter(chapter.chapterID,
                                new InteracteInfo { data = "4_1" });
                        });
                }
                else
                {
                    UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(2), null);
                }
                return false;
            }
            if (info.data == "4_1")
                return true;
            return false;
        }
    }
}