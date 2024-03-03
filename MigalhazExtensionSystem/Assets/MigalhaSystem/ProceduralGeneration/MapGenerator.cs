using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] List<RoomScriptableObject> m_roomsScriptableObject;
        [SerializeField, ContextMenuItem("Get Random room by direction", nameof(GetRoom))] Direction m_doorDirections;

        
        public void GetRoom()
        {
            List<RoomScriptableObject> availableRoomsScriptable = GetAvailableRooms();
            if (availableRoomsScriptable is null || availableRoomsScriptable.Count == 0)
            {
                Debug.LogWarning($"No available ScriptableRoom with {m_doorDirections} settings!".Error());
                return;
            }

            RoomScriptableObject RoomScriptable = availableRoomsScriptable.GetRandom();
            if (RoomScriptable is null)
            {
                Debug.LogWarning($"Couldn't get random room from {availableRoomsScriptable}! (Room Scriptable is null)".Error());
                return;
            }

            GameObject room = RoomScriptable.GetRandomRoom();
            if (room is null)
            {
                Debug.LogWarning($"Couldn't get random room from {RoomScriptable}! (Game Object is null)".Error());
                return;
            }

            Debug.Log(room.name);
        }

        List<RoomScriptableObject> GetAvailableRooms()
        {
            return m_roomsScriptableObject.FindAll(x => x.CompareDoor(m_doorDirections));
        }
    }
}