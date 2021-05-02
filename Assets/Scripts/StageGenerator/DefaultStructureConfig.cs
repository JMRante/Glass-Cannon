using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStructureConfig : StructureConfig
{
    public DefaultStructureConfig() : base()
    {
        AddStructureTypeDefinition(new StructureType("start"));
        AddStructureTypeDefinition(new StructureType("exit"));
        AddStructureTypeDefinition(new StructureType("pathway"));
        AddStructureTypeDefinition(new StructureType("hub"));
        AddStructureTypeDefinition(new StructureType("deadEnd"));

        AddWeight(GetStructureType("start"), GetStructureType("pathway"), 5);
        AddWeight(GetStructureType("start"), GetStructureType("hub"), 2);

        AddWeight(GetStructureType("pathway"), GetStructureType("pathway"), 5);
        AddWeight(GetStructureType("pathway"), GetStructureType("deadEnd"), 3);
        AddWeight(GetStructureType("pathway"), GetStructureType("hub"), 8);

        AddWeight(GetStructureType("hub"), GetStructureType("pathway"), 10);
        AddWeight(GetStructureType("hub"), GetStructureType("deadEnd"), 3);

        AddRoomMapping(GetStructureType("start"), GetRoomType("minorCap"));
        AddRoomMapping(GetStructureType("exit"), GetRoomType("minorCap"));
        AddRoomMapping(GetStructureType("pathway"), GetRoomType("pathway"));
        AddRoomMapping(GetStructureType("hub"), GetRoomType("hub"));
        AddRoomMapping(GetStructureType("deadEnd"), GetRoomType("minorCap"));

        AddDoorRange(GetStructureType("start"), 1, 1);
        AddDoorRange(GetStructureType("exit"), 1, 1);
        AddDoorRange(GetStructureType("pathway"), 2, 2);
        AddDoorRange(GetStructureType("hub"), 4, 4);
        AddDoorRange(GetStructureType("deadEnd"), 1, 1);

        SetRoomLimit(25);
    }
}
