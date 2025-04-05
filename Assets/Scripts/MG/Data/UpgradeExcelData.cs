using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class UpgradeExcelData : ScriptableObject
{
	public List<UpgradeInfo> Carriage; // Replace 'EntityType' to an actual type that is serializable.
}
