using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LeftOut.GameplayManagement;
using UnityEngine;

namespace LeftOut
{
    public static class InstanceTrackingList<T> where T : class, ITrackableInstance
    {
        public enum ListChangeEventType
        {
            Added,
            Removed
        }

        public class TrackingChangeEventArgs : EventArgs
        {
            public ListChangeEventType ChangeEventType { get; }
            public T InstanceAffected { get; }

            internal TrackingChangeEventArgs(ListChangeEventType eventType, T instance)
            {
                ChangeEventType = eventType;
                InstanceAffected = instance;
            }
        }
        
        static List<T> s_Instances;
        // Ensure the list of instances is initialized before returning
        static List<T> InstancesInitialized => s_Instances ??= new List<T>();

        public static event EventHandler<TrackingChangeEventArgs> OnChange;

        public static void Add(T instance)
        {
            if (InstancesInitialized.Contains(instance))
            {
                Debug.LogWarning($"Can't {nameof(Add)} {instance} because it is already being tracked.");
                return;
            }

            s_Instances.Add(instance);
            instance.OnDestroyed += HandleInstanceDestroyed;
            OnChange?.Invoke(null, new TrackingChangeEventArgs(ListChangeEventType.Added, instance));
        }

        static void HandleInstanceDestroyed(ITrackableInstance instance)
        {
            Debug.Assert(instance is T,
                $"Tried to handle destruction of {instance} but it's not even of type {typeof(T)}");
            Debug.Assert(s_Instances.Contains(instance),
                $"Trying to handle destruction of {instance}, which we weren't tracking in {s_Instances}");
            s_Instances.Remove((T)instance);
            OnChange?.Invoke(null, new TrackingChangeEventArgs(ListChangeEventType.Removed, (T)instance));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AssertOnNull()
        {
            Debug.Assert(!s_Instances.Contains(null), 
                $"{nameof(InstanceTrackingList<T>)} has null items - " +
                $"did we not properly clean up after a destroy?");
        }

        public static List<T> GetReference()
        {
            return InstancesInitialized;
        }
        
        static bool TryGetFirst(out T instance)
        {
            AssertOnNull();
            if (s_Instances.Any())
            {
                instance = s_Instances[0];
                return true;
            }

            instance = default;
            return false;
        }

    }
}
