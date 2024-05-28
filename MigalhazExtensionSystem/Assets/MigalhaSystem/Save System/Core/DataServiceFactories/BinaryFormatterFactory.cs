using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    [CreateAssetMenu(fileName = "BinaryFormatterFactory", menuName = "Scriptable Object/Save Factory/Binary Formatter Factory")]
    public class BinaryFormatterFactory : DataServiceFactory
    {
        public override IDataService ProvideDataService() => (m_dataService ??= new BinaryFormatterDataService());
    }
}