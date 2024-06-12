using Godot;
using System;
using System.Collections.Generic;

public partial class upgrade_data : Node
{
    public Dictionary<string, Dictionary<string, object>> Data { get; set; }

    public upgrade_data(Dictionary<string, Dictionary<string, object>> data)
    {
        Data = data;
    }
}
