using MigalhaSystem.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
    public class NewScript : MonoBehaviour, IOnRemoveComponent
    {
        private void Start()
        {
            Benchmark.TimeExecution(() => While(100), "Label");
            Benchmark.TimeExecution(() => While(1000), "2");
            Benchmark.TimeExecution(() => While(10000), "3");
            Benchmark.TimeExecution(() => While(100000), "4");
            Benchmark.TimeExecution(() => While(1000000), "5");
            Benchmark.TimeExecution(() => While(10000000), "6");
            Benchmark.TimeExecution(() => While(100000000), "7");
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        void While(int value)
        {
            int i = 0;
            while (i < value)
            {
                //int result = value.RangeBy0();
                i++;
            }
        }

        public void OnRemoveComponent()
        {
            Debug.Log($"UOU ESTOU SENDO REMOVIDO {gameObject.name}");
        }
    }

    [System.Serializable]
    public class Bitmask
    {
        [SerializeField] List<BitmaskItem> items;

        public List<BitmaskItem> GetItems(int bitmask)
        {
            List<BitmaskItem> result = new List<BitmaskItem>();
            foreach (BitmaskItem item in items)
            {

            }

            return result;
        }
    }

    [System.Serializable]
    public class BitmaskItem
    {
        [field: SerializeField, HideInInspector]
        public string tag { get; private set; }
        int bitmask;
        [SerializeField] string name;
        public void UpdateBitMask(float newValue)
        {
            bitmask = (int)newValue;

            tag = $"{bitmask}: {name}";
        }
    }
}