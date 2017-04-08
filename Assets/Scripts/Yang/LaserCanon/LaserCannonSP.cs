using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpritePair{
	public Transform trans;
	public Sprite enemySprite;
	public Sprite playerSprite;
	public Sprite noneSprite;

	public SpriteRenderer renderer{
		get{
			return trans.GetComponent<SpriteRenderer>();
		}
	}

	public void SetEnemy(){
		if(renderer && enemySprite){
			renderer.sprite = enemySprite;
		}
	}

	public void SetPlayer(){
		if(renderer && playerSprite){
			renderer.sprite = playerSprite;
		}
	}

	public void SetNone(){
		if(renderer && noneSprite){
			renderer.sprite = noneSprite;
		}
	}
}

[System.Serializable]
public struct ColorPair{
	public Color enemyColor;
	public Color playerColor;
}

[System.Serializable]
public struct MaterialPair{
	public Material enemyMaterial;
	public Material playerMaterial;
}

[System.Serializable]
public struct ParticlePair{
	public ParticleSystem enemyParticle;
	public ParticleSystem playerParticle;
}


public class LaserCannonSP : MonoBehaviour {
	public SpritePair baseColorSP;
	public SpritePair baseGreySP;

	public SpritePair crystalSP;
	public SpritePair aimPointSP;

	public SpritePair reflectionSP;

	public ColorPair shootTintCP;
	public ColorPair shootGlowCP;
	public ColorPair aimCP;


	public MaterialPair shootMaterials;
	public ParticlePair shootParticles;

	public void SetEnemySprite(){
		baseColorSP.SetEnemy ();
		crystalSP.SetEnemy ();
		aimPointSP.SetEnemy ();
		reflectionSP.SetEnemy ();
	}

	public void SetPlayerSprite(){
		baseColorSP.SetPlayer ();
		crystalSP.SetPlayer ();
		aimPointSP.SetPlayer ();
		reflectionSP.SetPlayer ();
	}

	public void SetNoneSprite(){
		baseColorSP.SetNone ();
		crystalSP.SetNone ();
		reflectionSP.SetNone ();
	}

	public void SetEnemyColor(){
		if(state){
			state.SetEnemyColor ();
		}
	}

	public void SetPlayerColor(){
		if(state){
			state.SetPlayerColor ();
		}
	}

	LaserCannonState state;
	// Use this for initialization
	void Start () {
		state = GetComponent<LaserCannonState> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
