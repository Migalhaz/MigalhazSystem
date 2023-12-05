using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    [CreateAssetMenu(fileName = "New Room", menuName = "Scriptable Object/Game/Room")]
    public class RoomScriptableObject : ScriptableObject
    {
        [SerializeField] Direction m_doorDirections;

        [SerializeField] List<GameObject> m_rooms;

        public bool CompareDoor(Direction _expectedDoorDirection)
        {
            return m_doorDirections == _expectedDoorDirection;
        }

        public GameObject GetRandomRoom()
        {
            if (m_rooms.Count == 0 || m_rooms is null)
            {
                Debug.LogWarning($"Couldn't get random room from {m_rooms}! (the list may be null)".Error());
                return null;
            }
            GameObject targetRoom = m_rooms.GetRandom();
            if (targetRoom is null)
            {
                Debug.LogWarning($"{targetRoom} is null! (missing reference)".Error());
                return null;
            }
            return targetRoom;
        }
    }
    [System.Flags]
    public enum Direction
    {
        None = 0, Up = 1, Down = 2, Left = 4, Right = 8
    }

}