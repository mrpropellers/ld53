using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LeftOut.Extensions;
using Unity.VisualScripting;

namespace LeftOut.LudumDare
{
    public class BeeFlowerSensor : MonoBehaviour
    { 
        List<Flower> m_FlowersInRange;

        [SerializeField]
        Transform BeeCenter;

        public bool DoesSenseFlower => m_FlowersInRange.Any();
        public Flower ClosestFlower => FindClosestFlower();

        void Start()
        {
            m_FlowersInRange = new List<Flower>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponentInParent<Flower>(out var flower))
            {
                HandleFlowerSensed(flower);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponentInParent<Flower>(out var flower))
            {
                HandleFlowerNotSensed(flower);
            }
        }

        Flower FindClosestFlower()
        {
            if (!DoesSenseFlower)
            {
                Debug.LogError($"Can't find closest flower because we don't sense any.");
                return null;
            }

            var minDistance = float.MaxValue;
            Flower closestFlower = null;
            foreach(var flower in m_FlowersInRange)
            {
                var distanceToFlower = CalculateDistance(BeeCenter, flower.LandingPointCenter);
                if (distanceToFlower < minDistance)
                {
                    minDistance = distanceToFlower;
                    closestFlower = flower;
                }
            }

            return closestFlower;
        }

        void HandleFlowerSensed(Flower flower)
        {
            if (m_FlowersInRange.Contains(flower))
            {
                Debug.LogWarning($"{nameof(Flower)} {flower.name} already in sensed list - doing nothing.");
                return;
            }
            Debug.Log($"{nameof(Flower)} sensed: {flower.name}");
            m_FlowersInRange.Add(flower);
        }

        static float CalculateDistance(Transform from, Transform to) =>
            Vector3.Magnitude(from.position - to.position);

        void HandleFlowerNotSensed(Flower flower)
        {
            if (!m_FlowersInRange.Contains(flower))
            {
                Debug.LogWarning($"{nameof(Flower)} {flower.name} not in sensed list - doing nothing.");
                return;
            }
            Debug.Log($"{nameof(Flower)} no longer sensed: {flower.name}");

            m_FlowersInRange.Remove(flower);
        }
    }
}
