using System;
using System.Reflection;

namespace AI {
    internal class ReflectionUtil {
            
        private const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                       | BindingFlags.Static;

        public static void SetValue(object target, string fieldName, object value) {
            Type objectType = target.GetType();
            FieldInfo fieldInfo = objectType.GetField(fieldName, bindFlags);
            fieldInfo.SetValue(target, value);
            // } catch (Exception e) {
            //     Logger.Log("Exception thrown:");
            //     Logger.Log($"    Type: {e.GetType()}");
            //     Logger.Log($"    Data: {e.Data}");
            //     Logger.Log($"    Message: {e.Message}");
            //     Logger.Log($"    Source: {e.Source}");
            //     Logger.Log($"    Stacktrace: {e.StackTrace}");
            // }
        }

        public static object GetValue(object target, string fieldName) {
            Type objectType = target.GetType();
            FieldInfo fieldInfo = objectType.GetField(fieldName, bindFlags);
            return fieldInfo.GetValue(target);
            // } catch (Exception e) {
            //     Logger.Log("Exception thrown:");
            //     Logger.Log($"    Type: {e.GetType()}");
            //     Logger.Log($"    Data: {e.Data}");
            //     Logger.Log($"    Message: {e.Message}");
            //     Logger.Log($"    Source: {e.Source}");
            //     Logger.Log($"    Stacktrace: {e.StackTrace}");
            //     
            //     return null;
            // }
        }

        public static object GetValue(object target, Type type, string fieldName) {
            FieldInfo fieldInfo = type.GetField(fieldName, bindFlags);
            return fieldInfo.GetValue(target);
        }
        
    }
}