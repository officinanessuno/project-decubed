using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	
	// Use this for initialization
	protected virtual void Start () {
        Level.Singleton.AddEntity(this, this.transform.position);
	}
	
	public virtual void Update(){
		
	}
	
}
