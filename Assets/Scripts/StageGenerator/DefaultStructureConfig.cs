using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStructureConfig : StructureConfig
{
    public DefaultStructureConfig() : base()
    {
        AddStructureTypeDefinition("start");
        AddStructureTypeDefinition("exit");
        AddStructureTypeDefinition("pathway");
        AddStructureTypeDefinition("hub");
        AddStructureTypeDefinition("deadEnd");

        AddWeight("start", "pathway", 5);
        AddWeight("start", "hub", 2);

        AddWeight("pathway", "pathway", 5);
        AddWeight("pathway", "deadEnd", 3);
        AddWeight("pathway", "hub", 8);

        AddWeight("hub", "pathway", 10);
        AddWeight("hub", "deadEnd", 3);

        AddRoomMapping("start", "minorCap");
        AddRoomMapping("exit", "minorCap");
        AddRoomMapping("pathway", "pathway");
        AddRoomMapping("hub", "hub");
        AddRoomMapping("deadEnd", "minorCap");

        AddCapRoom("deadEnd");

        SetRoomLimit(25);
    }
}
