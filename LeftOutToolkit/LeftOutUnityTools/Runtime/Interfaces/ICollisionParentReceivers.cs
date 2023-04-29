using UnityEngine;

namespace LeftOut.JamAids
{
    public interface ICollision2DReceiver
    {
        public void OnChildCollision2DEnter(Collision2D collision2D);
        public void OnChildCollision2DStay(Collision2D collision2D);
        public void OnChildCollision2DExit(Collision2D collision2D);
    }

    public interface ITrigger2DReceiver
    {
        public void OnChildTrigger2DEnter(Collider2D collider2D);
        public void OnChildTrigger2DStay(Collider2D collider2D);
        public void OnChildTrigger2DExit(Collider2D collider2D);
    }
    
    public interface ICollisionReceiver
    {
        public void OnChildCollisionEnter(Collision collision);
        public void OnChildCollisionStay(Collision collision);
        public void OnChildCollisionExit(Collision collision);
    }

    public interface ITriggerReceiver
    {
        public void OnChildTriggerEnter(Collider collider);
        public void OnChildTriggerStay(Collider collider);
        public void OnChildTriggerExit(Collider collider);
    }
}
