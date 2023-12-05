using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class Room : MonoBehaviour
    {
        [SerializeField] List<Transform> m_doors;

        [SerializeField] List<Transform> m_enemies;

        public void ActiveRoom()
        {

        }

        public void DesactiveRoom()
        {

        }
    }
}