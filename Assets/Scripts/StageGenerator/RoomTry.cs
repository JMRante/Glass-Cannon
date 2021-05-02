using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTry
{
    private RoomPrefab roomPrefab;
    private Transform roomTransform;

    private string hashCodeSource;
    private int hashCode;

    public RoomTry(RoomPrefab roomPrefab, Transform roomTransform)
    {
        this.roomPrefab = roomPrefab;
        this.roomTransform = roomTransform;

        hashCodeSource = roomPrefab.GetHashCode() + " pos: " + roomTransform.position.ToString() + " rot: " + roomTransform.rotation.ToString();
        hashCode = hashCodeSource.GetHashCode();
    }

    public RoomPrefab GetRoomPrefab()
    {
        return roomPrefab;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            RoomTry otherRoomTry = (RoomTry)obj;
            return roomPrefab == otherRoomTry.roomPrefab && roomTransform.position.Equals(otherRoomTry.roomTransform.position) && roomTransform.rotation.Equals(otherRoomTry.roomTransform.rotation);
        }
    }

    public override int GetHashCode()
    {
        return hashCode;
    }
}
