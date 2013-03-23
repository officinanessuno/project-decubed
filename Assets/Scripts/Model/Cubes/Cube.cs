using UnityEngine;
using System.Collections.Generic;

public class Cube : GameEntity, IClickable{
	
	#region Variables
	/// <summary>
	/// The cube is selected.
	/// </summary>
    protected bool selected = false;
	
    /// <summary>
    /// The current command that is executing, if its null then there.
    /// </summary>
	private Command command;
	private int jumpHeight = 1;
	#endregion
		
	#region Command Management
	
	/// <summary>
	/// Moves Cube to the direction .
	/// </summary>
	/// <param name='nextPosition'>
	/// Next position.	/// </param>
    public virtual void MoveTo(Vector3Int nextPosition) {
        Level.Singleton.RemoveEntity(new Vector3Int(transform.position));
		//TODO:Fix Animation
		CubeAnimations.AnimateMove(gameObject, Vector3.down, nextPosition.ToVector3);
        Level.Singleton.AddEntity(this, nextPosition);
    }
	
	/// <summary>
	/// Gets the options of the normal cube.
	/// </summary>
	/// <value>
	/// The options of commands of the chosen cube.
	/// </value>
    public virtual Command[] Options{ 
        get {
            List<Command> options = new List<Command>();
			Vector3Int pos;
			if (CubeHelper.CheckAvailablePosition(transform.position + Vector3.forward,out pos,jumpHeight)){
				options.Add(new Move(this,pos));
			}
			if (CubeHelper.CheckAvailablePosition(transform.position + Vector3.back,out pos,jumpHeight)){
				options.Add(new Move(this,pos));
			}
			if (CubeHelper.CheckAvailablePosition(transform.position + Vector3.right,out pos,jumpHeight)){
				options.Add(new Move(this,pos));
			}
			if (CubeHelper.CheckAvailablePosition(transform.position + Vector3.left,out pos,jumpHeight)){
				options.Add(new Move(this,pos));
			}
            return options.ToArray();
        }
    }
	
	public void EndExecution(){
		OrganizeTransform();
		if(command != null){
			command.EndExecution();
		}
	}
	
	public void OrganizeTransform(){
		Transform obj = gameObject.transform.parent;
		transform.parent = null;
		if (obj != null){
			MonoBehaviour.Destroy(obj.gameObject);
		}
		transform.position = Vector3Round(transform.position);
		transform.rotation = Quaternion.Euler(Vector3Round(transform.rotation.eulerAngles));
	}
	
    public void FallOutOfBounds(Vector3 outOfBouncePosition)
    {
		//TODO Arreglar Esto
//		CubeAnimations.AnimateSlide(gameObject,outOfBouncePosition + new Vector3(0,-10,0), "KillCube", null);
    }
	
	public void KillCube(){
		Destroy(gameObject);
	}
	
	#endregion
	
	#region IClickable methods
	
    public void NotifyClick()
    {
       Level.Singleton.SelectedCube = this;
    }
	
	#endregion
	
	#region GameEntity overrides
	
	public override void Update(){
	}
	
	#endregion
	
	#region Gets and Sets
	public Command Command {
		get {
			return this.command;
		}
		set {
			command = value;
		}
	}
	
	public virtual bool IsSelected {
		get { 
			Vector3Int upPosition = new Vector3Int (transform.position + Vector3.up);
			
			//The cube can't be selected if it have another cube on it
			return selected && CubeHelper.IsFree (upPosition); 
		}
		set { selected = value; }
	}

	public int JumpHeight {
		get {
			return this.jumpHeight;
		}
		set {
			jumpHeight = value;
		}
	}
	#endregion
	
	#region Helper Methods
	
	private Vector3 Vector3Round(Vector3 v){
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}
	#endregion
}
