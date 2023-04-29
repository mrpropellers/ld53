using System;
using LeftOut.Extensions;
using UnityEngine;

namespace LeftOut.JamAids
{
    // TODO: fill out the rest of these methods
    public class ChildCollisionSender : MonoBehaviour
    {
        ICollisionReceiver m_CollisionReceiver;
        ITriggerReceiver m_TriggerReceiver;
        ICollision2DReceiver m_Collision2DReceiver;
        ITrigger2DReceiver m_Trigger2DReceiver;

        ICollisionReceiver CollisionReceiver =>
            m_CollisionReceiver ?? gameObject.GetComponentInParent<ICollisionReceiver>();
        ITriggerReceiver TriggerReceiver =>
            m_TriggerReceiver ?? gameObject.GetComponentInParent<ITriggerReceiver>();
        ICollision2DReceiver Collision2DReceiver =>
            m_Collision2DReceiver ?? gameObject.GetComponentInParent<ICollision2DReceiver>();
        ITrigger2DReceiver Trigger2DReceiver =>
            m_Trigger2DReceiver ?? gameObject.GetComponentInParent<ITrigger2DReceiver>();

        void OnCollisionEnter(Collision collision)
        {
            CollisionReceiver.OnChildCollisionEnter(collision);
        }

        void OnCollisionStay(Collision collision)
        {
            CollisionReceiver.OnChildCollisionStay(collision);
        }

        void OnCollisionExit(Collision collision)
        {
            CollisionReceiver.OnChildCollisionExit(collision);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            Collision2DReceiver.OnChildCollision2DEnter(col);
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            Collision2DReceiver.OnChildCollision2DStay(collision);
        }

        void OnCollisionExit2D(Collision2D other)
        {
            Collision2DReceiver.OnChildCollision2DExit(other);
        }

        void OnTriggerEnter(Collider other)
        {
            TriggerReceiver.OnChildTriggerEnter(other);
        }

        void OnTriggerStay(Collider other)
        {
            TriggerReceiver.OnChildTriggerStay(other);
        }

        void OnTriggerExit(Collider other)
        {
            TriggerReceiver.OnChildTriggerExit(other);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Trigger2DReceiver.OnChildTrigger2DEnter(col);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            Trigger2DReceiver.OnChildTrigger2DStay(other);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Trigger2DReceiver.OnChildTrigger2DExit(other);
        }
    }
}
