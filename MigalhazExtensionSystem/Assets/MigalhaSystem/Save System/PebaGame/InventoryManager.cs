using MigalhaSystem.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
	public class InventoryManager : MonoBehaviour
	{
		const string m_SAVEPATH = "Inventory";
        const bool m_ENCRYPTED = true;

		public InventoryJson m_CurrentInventory = new InventoryJson();
		public static event System.Action OnSaveInventory;
		public static event System.Action OnLoadInventory;

        private void Start()
        {
            LoadInventory();
        }

        public void SaveInventory()
		{
            m_CurrentInventory ??= new InventoryJson();
			if (SaveManager.IsNull) return;

            SaveManager.Instance.SaveData(m_CurrentInventory, m_SAVEPATH, m_ENCRYPTED);
			OnSaveInventory?.Invoke();
        }
		public void LoadInventory(bool repeatOnError = true)
		{
			try
			{
				m_CurrentInventory = SaveManager.Instance.LoadData<InventoryJson>(m_SAVEPATH, m_ENCRYPTED);
				OnLoadInventory?.Invoke();
            }
			catch
			{
				if (repeatOnError)
				{
					SaveInventory();
					LoadInventory(false);
                }
			}
		}

		public bool ContainsGem(int gemTypeBitMask) => m_CurrentInventory.m_Gems.Exists(x => x.m_GemTypeBitMask == gemTypeBitMask);
		public bool ContainsPotion(int potionType) => m_CurrentInventory.m_Potions.Exists(x => x.m_PotionType == potionType);
		public bool ContainsBioBuff(int bioBuffType) => m_CurrentInventory.m_BioBuffs.Exists(x => x.m_BioBuffType == bioBuffType);
		public bool ContainsArmor(int armorType) => m_CurrentInventory.m_Armors.Exists(x => x.m_ArmorType == armorType);

		public bool ContainsWeapon(int weaponType) => m_CurrentInventory.m_Weapons.Exists(x => x.m_WeaponType == weaponType);
		public bool ContainsWeaponWithGem(int weaponType, int gemTypeBitMask) => m_CurrentInventory.m_Weapons.Exists(x => x.m_WeaponType == weaponType && x.m_WeaponGemTypeBitMask == gemTypeBitMask);
		public bool ContainsWeaponWithLevel(int weaponType, int weaponLevel) => m_CurrentInventory.m_Weapons.Exists(x => x.m_WeaponType == weaponType && x.m_WeaponLevel == weaponLevel);
		public bool ContainsWeapon(int weaponType, int gemTypeBitMask, int weaponLevel)
		{
			bool Comparer(WeaponJSON weapon)
			{
				if (weapon.m_WeaponType != weaponType) return false;
				if (weapon.m_WeaponGemTypeBitMask != gemTypeBitMask) return false;
				if (weapon.m_WeaponLevel != weaponLevel) return false;
				return true;
			}
			return m_CurrentInventory.m_Weapons.Exists(x => Comparer(x));
		}

		public bool ContainsWarrior(int warriorType) => m_CurrentInventory.m_Warriors.Exists(x => x.m_WarriorType == warriorType);
		public bool CompletedDungeon(int dungeonID) => m_CurrentInventory.m_CompletedDungeons.Exists(x => x.m_DungeonID == dungeonID);

		public bool GetGem(int gemTypeBitMask, out GemsJSON gem)
		{
			bool contains = ContainsGem(gemTypeBitMask);
			gem = null;
			if (contains) gem = m_CurrentInventory.m_Gems.Find(x => x.m_GemTypeBitMask == gemTypeBitMask);
			return contains;
		}
		public bool GetPotion(int potionType, out PotionJSON potion)
		{
            bool contains = ContainsPotion(potionType);
            potion = null;
            if (contains) potion = m_CurrentInventory.m_Potions.Find(x => x.m_PotionType == potionType);
            return contains;
        }
		public bool GetBioBuff(int bioBuffType, out BioBuffJSON bioBuffJSON)
		{
            bool contains = ContainsBioBuff(bioBuffType);
            bioBuffJSON = null;
            if (contains) bioBuffJSON = m_CurrentInventory.m_BioBuffs.Find(x => x.m_BioBuffType == bioBuffType);
            return contains;
        }
		public bool GetArmor(int armorType, out ArmorJSON armorJSON)
		{
            bool contains = ContainsArmor(armorType);
            armorJSON = null;
            if (contains) armorJSON = m_CurrentInventory.m_Armors.Find(x => x.m_ArmorType == armorType);
            return contains;
        }
		public bool GetWeapons(int weaponType, out List<WeaponJSON> weaponsJSON)
		{
			weaponsJSON = m_CurrentInventory.m_Weapons.FindAll(x => x.m_WeaponType == weaponType);
			if (weaponsJSON == null) return false;
			return weaponsJSON.Count > 0;
        }
		public bool GetWarriors(int warriorsType, out List<WarriorsJSON> warriors)
		{
            warriors = m_CurrentInventory.m_Warriors.FindAll(x => x.m_WarriorType == warriorsType);
			if (warriors == null) return false;
			return warriors.Count > 0;
		}

    }

	[System.Serializable]
	public class InventoryJson
	{
		public List<GemsJSON> m_Gems = new List<GemsJSON>();
		public List<PotionJSON> m_Potions = new List<PotionJSON>();
		public List<BioBuffJSON> m_BioBuffs = new List<BioBuffJSON>();
		public List<WeaponJSON> m_Weapons = new List<WeaponJSON>();
		public List<ArmorJSON> m_Armors = new List<ArmorJSON>();
		public List<WarriorsJSON> m_Warriors = new List<WarriorsJSON>();

		public List<CompletedDungeonsJSON> m_CompletedDungeons = new List<CompletedDungeonsJSON>();
	}

	public abstract class ItemJSON
	{
		public string m_ItemName;
		public string m_ItemDescription;
		public int m_ItemPrice;
		public int m_ItemQuantity;
	}

    [System.Serializable]
    public class GemsJSON : ItemJSON
	{
		public int m_GemTypeBitMask;
	}

    [System.Serializable]
    public class PotionJSON : ItemJSON
	{
		public int m_PotionType;
	}

    [System.Serializable]
    public class BioBuffJSON : ItemJSON
	{
		public int m_BioBuffType;
	}

    [System.Serializable]
    public class WeaponJSON : ItemJSON
	{
		public int m_WeaponType;
		public int m_WeaponGemTypeBitMask;
		public int m_WeaponLevel;
	}

    [System.Serializable]
    public class ArmorJSON : ItemJSON
	{
		public int m_ArmorType;
	}

	[System.Serializable]
	public class WarriorsJSON
	{
		public int m_WarriorType;
		public WeaponJSON m_WarriorWeapon = new WeaponJSON();
		public int m_WarriorArmor;
		public int m_BioBuffType;
	}

	[System.Serializable]
	public class CompletedDungeonsJSON
	{
		public int m_DungeonID;
	}
}