using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;

public partial class ability_upgrade : Resource
{
	[Export] public string name;
	[Export] public string id;
	[Export] public int maxQuantity;
	[Export(PropertyHint.MultilineText)]public string description;
}
