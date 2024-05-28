using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    [CreateAssetMenu(fileName = "JsonFactory", menuName = "Scriptable Object/Save Factory/Json Factory")]
    public class JsonFactory : DataServiceFactory
    {
        public override IDataService ProvideDataService() => (m_dataService ??= new JsonDataService());
    }
}