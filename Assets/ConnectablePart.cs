using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectablePart : MonoBehaviour
{
    List<Joint2D> connections = new List<Joint2D>();
    Rigidbody2D rb;
    AdvancedCollider connectionArea;
    
    bool _frozen = false;
    public bool frozen{
        get { return _frozen;}
        private set { _frozen = value; }
    }

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        connectionArea = GetComponentInChildren<AdvancedCollider>();
    }
    public void AddConnection(Joint2D joint){
        connections.Add(joint);
    }
    public void ConvertToCreature(){

    }
    public void CreateConnections(){
        Rigidbody2D[] contacts = connectionArea.GetCollidingBodies();
        foreach(Rigidbody2D contact in contacts){
            CreateConnection(contact);
        }
    }
    void CreateConnection(Rigidbody2D other){
        ConnectablePart otherPart = other.gameObject.GetComponent<ConnectablePart>();
        if(otherPart == null){
            return;
        }
        Vector2 selfSurfacePoint = rb.ClosestPoint(other.position);
        Vector2 otherSurfacePoint = other.ClosestPoint(rb.position);
        Vector2 jointPos = (selfSurfacePoint + otherSurfacePoint) * 0.5f;


        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();

        joint.connectedBody = other;
        joint.anchor = transform.InverseTransformPoint(jointPos);
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = other.transform.InverseTransformPoint(jointPos);
        AddConnection(joint);
        otherPart.AddConnection(joint);
    }
    public void RemoveConnections(){
        foreach(Joint2D joint in connections){
            Destroy(joint);
        }
    }

    // Remove null connections
    void Update()
    {
        for(int i = connections.Count-1; i >=0; i--){
            if(connections[i] == null){
                connections.RemoveAt(i);
            }
        }
    }
    void Freeze(){

    }
    void Unfreeze(){
        
    }
    public void Freeze(bool toggleValue){
        if(toggleValue){
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else{
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}
