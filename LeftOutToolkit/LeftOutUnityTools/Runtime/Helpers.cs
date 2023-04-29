using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeftOut.Runtime
{
    public static class Helpers
    {
        static void LogMissingReference(string nameOwner, string typeMissing, string nameMissing)
        {
            Debug.LogWarning($"{nameOwner} should have a {typeMissing} assigned for {nameMissing} but does not, looking for something to assign now...");
        }

        public static bool CheckIfNullAndLogError(this Object _, Object obj, string name = nameof(Object))
        {
            if (obj == null)
            {
                Debug.LogError($"{name} is null and would cause issues downstream if not ignored.");
                return true;
            }

            return false;
        }

        public static bool TryAssignNonNull<T>(ref T target, T assignment)
        {
            if (assignment == null)
            {
                return false;
            }

            target = assignment;
            return true;
        }

        public static bool AssureComponentAssigned<T>(this Object parent, ref T fieldToAssign, string fieldName, Func<T> findMethod) where T : UnityEngine.Object
        {
            var nameType = typeof(T).ToString();
            if (fieldToAssign == null)
            {
                LogMissingReference(parent.name, nameType, fieldName);
                fieldToAssign = findMethod.Invoke();
                if (fieldToAssign == null)
                {
                    Debug.LogError($"{parent.name} failed to find a valid {nameType} to assign to {fieldName}. You may need to manually assign, or change the find method.");
                    return false;
                }
                Debug.Log($"{parent.name} found a {nameType} to use for {fieldName}!");
            }

            return true;
        }

        public static T ThrowExceptionIfAlreadySet<T>(this Object _, T field, T value,
            string errorMsg = "This field can only be set once!")
        {
            if (field != null)
            {
                throw new Exception(errorMsg);
            }

            return value;
        }

        public static GameObject FindWithTagWarnIfMultiple(this Object _, string tag)
        {
            var objects = GameObject.FindGameObjectsWithTag(tag);
            if (objects.Length < 1)
            {
                Debug.LogError($"No GameObjects found with tag {tag}");
                return null;
            }

            if (objects.Length > 1)
            {
                Debug.LogWarning($"More than one GameObject found with tag {tag}, expected only one.");
            }

            return objects[0];
        }

        public static T WarnIfNull<T>(T nullable)
        {
            return WarnIfNull(null, nullable, out _);
        }

        public static T WarnIfNull<T>(this Object _, T nullable, out bool referenceIsNull)
        {
            referenceIsNull = nullable == null;
            if (referenceIsNull)
            {
                Debug.LogWarning($"Reference to {typeof(T)} is null which may be un-intended.");
            }

            return nullable;
        }

        public static bool TryGetAttached<T>(this Collider collider, out T component) where T : MonoBehaviour
        {
            component = collider.gameObject.GetComponentInChildren<T>();
            return component != null;
        }
        
        // TODO: Not sure if useful or overkill...
        // public static void InitializeAndThrowErrorIfPlaying(INeedsInitialization target)
        // {
        //     if (!target.TryInitialize() && Application.isPlaying)
        //     {
        //         Debug.LogError($"{target.Name} failed to initialize.");
        //     }
        // }
    }
}
