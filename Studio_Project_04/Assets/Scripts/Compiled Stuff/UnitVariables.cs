using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FACTION{
	ALLY,
	ENEMY,
	NEUTRAL
}

public class UnitVariables : MonoBehaviour {

	public Nodes currNode;
	public Nodes nextNode;

	public FACTION Side;

	//Take From Database
	public string Name;

	public int HP;

	public int AP;

	public int Initiative;

	public int ID;

	public Weapon _weapon;
	public Armor _armor;

	public UnitVariables (string name, int hp, int ap, int initiative, int id, string weapon, string armor)
	{
		this.Name = name;
		this.HP = hp;
		this.AP = ap;
		this.Initiative = initiative;
		this.ID = id;
		this._weapon = WeaponDatabase.Instance.FetchWeaponByName (weapon);
		this._armor = ArmorDatabase.Instance.FetchArmorByName (armor);
	}

	public void Copy(UnitVariables RealStats)
	{
		this.Name = RealStats.Name;
		this.HP = RealStats.HP;
		this.AP = RealStats.AP;
		this.Initiative = RealStats.Initiative;
		this.ID = RealStats.ID;
		this._weapon = RealStats._weapon;
		this._armor = RealStats._armor;
	}
}
