using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Indexer
{
	public class Indexer : MonoBehaviour 
	{
		[SerializeField] List<IIndexer> m_itensToIndex;

		public void SetIndex()
		{
			for (int i = 0; i < m_itensToIndex.Count; i++)
			{
				m_itensToIndex[i].SetIndex(i);
			}
		}
    }

	public interface IIndexer
	{
		public void SetIndex(int newIndex);
		public int GetIndex();
	}
}