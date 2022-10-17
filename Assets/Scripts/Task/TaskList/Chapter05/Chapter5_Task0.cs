using Interaction;
using UnityEngine;
using Common;

namespace Task
{

    public class Chapter5_Task0 : ChapterPart
    {
        GameObject childe;
        NPC_Pooling childe_Pooling;

        public Chapter5_Task0()
        {
            partName = "��ǽ�����";
            partDescription = "��ʾ��������ֱ����Ի�ǽ��ͨ����ǽ�Լ�����������Ŀ��";
        }

        public override void EnterTaskEvent(Chapter chapter, bool isLoaded)
        {
            base.EnterTaskEvent(chapter, isLoaded);
            if(!isLoaded)
                SustainCoroutine.Instance.AddCoroutine(ShowDialog);
            //��ӽ�������
            SustainCoroutine.Instance.AddCoroutine(AddEndInteracte);
        }

        /// <summary>   /// �����ү�Ի�   /// </summary>
        bool ShowDialog()
        {
            if(childe == null)
            {
                childe = Resources.Load<GameObject>("Prefab/Character/Childe");
                return false;
            }
            childe_Pooling = SceneObjectPool.Instance.GetObject<NPC_Pooling>(
               "Childe", childe, new Vector3(8, -9.5f, -31), Quaternion.Euler(new Vector3(0, -90, 0)));
            InteracteDelegate interacte = childe_Pooling.gameObject.AddComponent<InteracteDelegate>();
            interacte.interDelegate = () =>
            {
                if (AsynTaskControl.Instance.CheckChapterIsComplete(4))
                {
                    UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(1), null);
                }
                else
                {
                    UI.BigDialog.Instance.ShowBigdialog(chapter.GetDiglogText(0), null);
                }
            };
            return true;
        }

        /// <summary>        /// ��ӽ������񴥷�        /// </summary>
        bool AddEndInteracte()
        {
            GameObject temCollen = new GameObject("TempCollider");
            BoxCollider collider = temCollen.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(5, 10, 15);
            temCollen.transform.position = new Vector3(-48.23f, -4.44f, 22.96f);
            TrigerInteracteDelegate trigerInteracte = temCollen.AddComponent<TrigerInteracteDelegate>();
            trigerInteracte.trigerDelegate = () =>
            {
                AsynTaskControl.Instance.CheckChapter(chapter.chapterID,
                    new InteracteInfo());
            };
            return true;
        }


        public override void ExitTaskEvent(Chapter chapter)
        {
            childe_Pooling?.CloseObject();
        }

        public override bool IsCompleteTask(Chapter chapter, InteracteInfo info)
        {
            //�����˾�ֱ���˳�
            return true;
        }
    }
}