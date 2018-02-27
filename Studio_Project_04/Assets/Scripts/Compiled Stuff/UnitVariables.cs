using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FACTION{
	ALLY,
	ENEMY,
	NEUTRAL
}

public class UnitVariables : MonoBehaviour {

	public Nodes currNode;
	public Nodes nextNode;

	public FACTION Side;

	[SerializeField]
	private Image healthbar;
	private int startHp;

	[SerializeField]
	private GameObject UnitInfoObject;
	[SerializeField]
	private GameObject OpponentUnitInfoObject;

	//Take From Database
	public string Name;
	public int HP;
	public int AP;
	public int startAP;
	public int Initiative;
	public int ID;
	public Weapon _weapon;
	public Armor _armor;

	public void SetUnitVariables (string name, int hp, int ap,
		int initiative, int id, string weapon,
		string armor)
	{
		this.Name = name;
		this.HP = hp;
		this.AP = ap;
		this.Initiative = initiative;
		this.ID = id;
		this._weapon = WeaponDatabase.Instance.FetchWeaponByName (weapon);
		this._armor = ArmorDatabase.Instance.FetchArmorByName (armor);

		this.startHp = this.HP;
		this.startAP = this.AP;
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

		this.startHp = RealStats.startHp;
		this.startAP = RealStats.startAP;
	}

	// Update health bar for all units
	public void UpdateHealthBar()
	{
		// Calculation of health percentage
		healthbar.fillAmount = ((float)this.HP / (float)this.startHp);
	}

	// Update Unit Info Window for all units - Active status
	public void SetUnitInfoWindow(bool isActive)
	{
		// Set Unit Info Window according to passed in boolean
		UnitInfoObject.transform.GetChild (0).gameObject.SetActive (isActive);
	}

	// Update Unit Info Window for all units - Text values
	public void UpdateUnitInfo()
	{
		// Get the end button gameobject
		Transform UnitInfoWindow = UnitInfoObject.transform.GetChild(0).transform;

		// Update values inside Unit Infor Window

		// Name
		Text Name = UnitInfoWindow.Find("Name variable").GetChild(0).GetComponent<Text>();
		Name.text = this.Name.ToString();

		// HP
		Text HP = UnitInfoWindow.Find("HP variable").GetChild(0).GetComponent<Text>();
		HP.text = this.HP.ToString();

		// AP
		Text AP = UnitInfoWindow.Find("AP variable").GetChild(0).GetComponent<Text>();
		AP.text = this.AP.ToString();

		// Weapon
		Text WeaponT = UnitInfoWindow.Find("Weapon variable").GetChild(0).GetComponent<Text>();
		WeaponT.text = this._weapon.Title;
		Image WeaponI = UnitInfoWindow.Find("Weapon variable").GetChild(1).GetComponent<Image>();
		WeaponI.sprite = Resources.Load<Sprite>("Sprite/Items/Weapons/" + this._weapon.Icon);

		// Armor
		Text ArmorT = UnitInfoWindow.Find("Armor variable").GetChild(0).GetComponent<Text>();
		ArmorT.text = this._armor.Title;
		Image ArmorI = UnitInfoWindow.Find("Armor variable").GetChild(1).GetComponent<Image>();
		ArmorI.sprite = Resources.Load<Sprite>("Sprite/Items/Armors/" + this._armor.Icon);
	}

	public void SetOpponentUnitInfoWindow(bool isActive)
	{
		// Set Unit Info Window according to passed in boolean
		OpponentUnitInfoObject.transform.GetChild (0).gameObject.SetActive (isActive);
	}

	// Update Unit Info Window for all units - Text values
	public void UpdateOpponentUnitInfo(UnitVariables OpponentStats)
	{
		// Get the end button gameobject
		Transform OpponentUnitInfoWindow = OpponentUnitInfoObject.transform.GetChild(0).transform;

		// Update values inside Unit Infor Window

		// Name
		Text Name = OpponentUnitInfoWindow.Find("Name variable").GetChild(0).GetComponent<Text>();
		Name.text = OpponentStats.Name.ToString();

		// HP
		Text HP = OpponentUnitInfoWindow.Find("HP variable").GetChild(0).GetComponent<Text>();
		HP.text = OpponentStats.HP.ToString();

		// AP
		Text AP = OpponentUnitInfoWindow.Find("AP variable").GetChild(0).GetComponent<Text>();
		AP.text = OpponentStats.AP.ToString();

		// Weapon
		Text WeaponT = OpponentUnitInfoWindow.Find("Weapon variable").GetChild(0).GetComponent<Text>();
		WeaponT.text = OpponentStats._weapon.Title;
		Image WeaponI = OpponentUnitInfoWindow.Find("Weapon variable").GetChild(1).GetComponent<Image>();
		WeaponI.sprite = Resources.Load<Sprite>("Sprite/Items/Weapons/" + OpponentStats._weapon.Icon);

		// Armor
		Text ArmorT = OpponentUnitInfoWindow.Find("Armor variable").GetChild(0).GetComponent<Text>();
		ArmorT.text = OpponentStats._armor.Title;
		Image ArmorI = OpponentUnitInfoWindow.Find("Armor variable").GetChild(1).GetComponent<Image>();
		ArmorI.sprite = Resources.Load<Sprite>("Sprite/Items/Armors/" + OpponentStats._armor.Icon);
	}
}
