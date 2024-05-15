using UnityEngine;
[System.Serializable]
public enum TargetScene
{
    Forest_1,
    Village_2,
    Castle_3,
    Halloween_4,
}
public class Develop : MonoBehaviour
{
    public TargetScene targetScene = TargetScene.Forest_1;
    public string GetTargetScene()
    {
        switch (targetScene)
        {
            case TargetScene.Forest_1:
                return "1_Forest";
            case TargetScene.Village_2:
                return "2_Village";
            case TargetScene.Castle_3:
                return "3_Castle";
            case TargetScene.Halloween_4:
                return "4_Halloween";
        }
        return null;
    }
}
