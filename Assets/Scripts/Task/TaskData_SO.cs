using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Data/Task Data")]
[System.Serializable]
public class TaskData_SO : ScriptableObject
{
    [System.Serializable]
    public class TaskRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }
    public string taskID;
    public string taskName;
    [TextArea] public string taskDescription;
    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;
    public List<TaskRequire> requireList = new List<TaskRequire>();
    public List<PackageItem> rewardList = new List<PackageItem>();

    public void CheckTaskProgress()
    {
        var finishRequires = requireList.Where(r => r.requireAmount <= r.currentAmount);
        isCompleted = finishRequires.Count() == requireList.Count;
    }
    public List<string> RequireItemName()
    {
        List<string> nameList = new List<string>();
        foreach (var require in requireList)
        {
            nameList.Add(require.name);
        }
        return nameList;
    }
    public void GiveRewards()
    {
        foreach (var reward in rewardList)
        {
            if (reward.amount < 0)//需要上交任务物品的情况
            {
                int requireCount = Mathf.Abs(reward.amount);
                if (InventoryManager.Instance.SearchTaskItemInBag(reward.itemData) != null)//背包当中有需要交的物品
                {
                    if (InventoryManager.Instance.SearchTaskItemInBag(reward.itemData).amount <= requireCount)//背包当中需要上交物品的数量刚好够或者不够的情况
                    {
                        requireCount -= InventoryManager.Instance.SearchTaskItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.SearchTaskItemInBag(reward.itemData).amount = 0;

                        if (InventoryManager.Instance.SearchTaskItemInAction(reward.itemData) != null)
                        {

                            InventoryManager.Instance.SearchTaskItemInAction(reward.itemData).amount -= requireCount;
                        }
                    }
                    else//背包当中上交物品的数量充足
                    {
                        InventoryManager.Instance.SearchTaskItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else//背包当中没有上交物品代表Action中一定满足了任务物品的数量
                {
                    InventoryManager.Instance.SearchTaskItemInAction(reward.itemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }
            InventoryManager.Instance.inventoryUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }
}
