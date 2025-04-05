using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNode : MonoBehaviour
{
    public int ID;
    public UpgradeNode parent;
    public List<UpgradeNode> children;

    public UpgradeNode()
    {
        children = new List<UpgradeNode>();
    }

    public void UpgradeInfoChange()
    {
        ItemManager.Instance.playerInventory.Upgrade_ID = ID;
    }

    public void AddChild(UpgradeNode node)
    {
        children.Add(node);
        node.parent = this;
    }
}
