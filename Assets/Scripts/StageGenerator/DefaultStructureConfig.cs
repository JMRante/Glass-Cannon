using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStructureConfig : StructureConfig
{
    public DefaultStructureConfig() : base()
    {
        AddWeight(StructureType.start, StructureType.pathway, 5);
        AddWeight(StructureType.start, StructureType.hub, 1);

        AddWeight(StructureType.pathway, StructureType.pathway, 2);
        AddWeight(StructureType.pathway, StructureType.end, 10);
        AddWeight(StructureType.pathway, StructureType.hub, 1);

        AddWeight(StructureType.hub, StructureType.pathway, 6);
        AddWeight(StructureType.hub, StructureType.end, 1);

        AddRoomMapping(StructureType.start, RoomType.minorCap);
        AddRoomMapping(StructureType.end, RoomType.minorCap);
        AddRoomMapping(StructureType.pathway, RoomType.pathway);
        AddRoomMapping(StructureType.hub, RoomType.hub);

        AddChildRange(StructureType.start, 1, 1);
        AddChildRange(StructureType.end, 0, 0);
        AddChildRange(StructureType.pathway, 2, 2);
        AddChildRange(StructureType.hub, 4, 4);

        SetRoomLimit(25);
    }
}
