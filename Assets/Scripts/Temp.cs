using UnityEngine;

public class Temp : MonoBehaviour
{
    [GradientUsage(true)]
    public Gradient gradient;
    void Start()
    {
        //UI.BigDialog.Instance.ShowBigdialog("���\n��������\n�Һ���Ǯ", null);
        //UI.SmallDialog.Instance.ShowSmallDialog("���\n��������\n�Һ���Ǯ", null);
        GradientColorKey[] colorKeys = gradient.colorKeys;
        for(int i=0; i<colorKeys.Length; i++)
        {
            Debug.Log(i.ToString() + " " + colorKeys[i].color);
            Debug.Log(i.ToString() + " " + colorKeys[i].time);
        }
    }


}
