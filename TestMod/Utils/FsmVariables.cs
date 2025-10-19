using HutongGames.PlayMaker;

namespace KIS.Utils
{
    /// <summary>
    /// Handles getting and setting variables in the FSM
    /// </summary>
    public static class FsmVariables
    {
        /// <summary>
        /// Get NamedVariables of Type T
        /// </summary>
        /// <typeparam name="T">Type of NamedVariable</typeparam>
        /// <param name="fsm"></param>
        /// <returns></returns>
        public static NamedVariable[] GetVariables<T>(this PlayMakerFSM fsm) where T : NamedVariable, new()
        {
            var variables = fsm.FsmVariables;
            var typ = typeof(T);
            if (typ == typeof(FsmFloat))
            {
                return variables.FloatVariables;
            }
            if (typ == typeof(FsmInt))
            {
                return variables.IntVariables;
            }
            if (typ == typeof(FsmBool))
            {
                return variables.BoolVariables;
            }
            if (typ == typeof(FsmString))
            {
                return variables.StringVariables;
            }
            if (typ == typeof(FsmVector2))
            {
                return variables.Vector2Variables;
            }
            if (typ == typeof(FsmVector3))
            {
                return variables.Vector3Variables;
            }
            if (typ == typeof(FsmColor))
            {
                return variables.ColorVariables;
            }
            if (typ == typeof(FsmRect))
            {
                return variables.RectVariables;
            }
            if (typ == typeof(FsmQuaternion))
            {
                return variables.QuaternionVariables;
            }
            if (typ == typeof(FsmGameObject))
            {
                return variables.GameObjectVariables;
            }
            return null;
        }


        public static NamedVariable[] GetVariables<T>(this Fsm fsm) where T : NamedVariable, new()
        {
            var variables = fsm.Variables;
            var typ = typeof(T);
            if (typ == typeof(FsmFloat))
            {
                return variables.FloatVariables;
            }
            if (typ == typeof(FsmInt))
            {
                return variables.IntVariables;
            }
            if (typ == typeof(FsmBool))
            {
                return variables.BoolVariables;
            }
            if (typ == typeof(FsmString))
            {
                return variables.StringVariables;
            }
            if (typ == typeof(FsmVector2))
            {
                return variables.Vector2Variables;
            }
            if (typ == typeof(FsmVector3))
            {
                return variables.Vector3Variables;
            }
            if (typ == typeof(FsmColor))
            {
                return variables.ColorVariables;
            }
            if (typ == typeof(FsmRect))
            {
                return variables.RectVariables;
            }
            if (typ == typeof(FsmQuaternion))
            {
                return variables.QuaternionVariables;
            }
            if (typ == typeof(FsmGameObject))
            {
                return variables.GameObjectVariables;
            }
            return null;
        }
        /// <summary>
        /// Set NamedVariables of Type T
        /// </summary>
        /// <typeparam name="T">Type of NamedVariable</typeparam>
        /// <param name="fsm"></param>
        /// <param name="namedVariables"></param>
        public static void SetVariables<T>(this PlayMakerFSM fsm, NamedVariable[] namedVariables) where T : NamedVariable, new()
        {
            var variables = fsm.FsmVariables;
            var typ = typeof(T);
            if (typ == typeof(FsmFloat))
            {
                variables.FloatVariables = (FsmFloat[])namedVariables;
            }
            if (typ == typeof(FsmInt))
            {
                variables.IntVariables = (FsmInt[])namedVariables;
            }
            if (typ == typeof(FsmBool))
            {
                variables.BoolVariables = (FsmBool[])namedVariables;
            }
            if (typ == typeof(FsmString))
            {
                variables.StringVariables = (FsmString[])namedVariables;
            }
            if (typ == typeof(FsmVector2))
            {
                variables.Vector2Variables = (FsmVector2[])namedVariables;
            }
            if (typ == typeof(FsmVector3))
            {
                variables.Vector3Variables = (FsmVector3[])namedVariables;
            }
            if (typ == typeof(FsmColor))
            {
                variables.ColorVariables = (FsmColor[])namedVariables;
            }
            if (typ == typeof(FsmRect))
            {
                variables.RectVariables = (FsmRect[])namedVariables;
            }
            if (typ == typeof(FsmQuaternion))
            {
                variables.QuaternionVariables = (FsmQuaternion[])namedVariables;
            }
            if (typ == typeof(FsmGameObject))
            {
                variables.GameObjectVariables = (FsmGameObject[])namedVariables;
            }
        }

        public static void SetVariables<T>(this Fsm fsm, NamedVariable[] namedVariables) where T : NamedVariable, new()
        {
            var variables = fsm.Variables;
            var typ = typeof(T);
            if (typ == typeof(FsmFloat))
            {
                variables.FloatVariables = (FsmFloat[])namedVariables;
            }
            if (typ == typeof(FsmInt))
            {
                variables.IntVariables = (FsmInt[])namedVariables;
            }
            if (typ == typeof(FsmBool))
            {
                variables.BoolVariables = (FsmBool[])namedVariables;
            }
            if (typ == typeof(FsmString))
            {
                variables.StringVariables = (FsmString[])namedVariables;
            }
            if (typ == typeof(FsmVector2))
            {
                variables.Vector2Variables = (FsmVector2[])namedVariables;
            }
            if (typ == typeof(FsmVector3))
            {
                variables.Vector3Variables = (FsmVector3[])namedVariables;
            }
            if (typ == typeof(FsmColor))
            {
                variables.ColorVariables = (FsmColor[])namedVariables;
            }
            if (typ == typeof(FsmRect))
            {
                variables.RectVariables = (FsmRect[])namedVariables;
            }
            if (typ == typeof(FsmQuaternion))
            {
                variables.QuaternionVariables = (FsmQuaternion[])namedVariables;
            }
            if (typ == typeof(FsmGameObject))
            {
                variables.GameObjectVariables = (FsmGameObject[])namedVariables;
            }
        }
        /// <summary>
        /// Set NamedVariables of Type T 
        /// </summary>
        /// <typeparam name="T">Type of NamedVariable</typeparam>
        /// <param name="fsm"></param>
        /// <param name="name"></param>
        public static void AddVariable<T>(this PlayMakerFSM fsm, string name) where T : NamedVariable, new()
        {
            GetAddVariable<T>(fsm, name);
        }

        /// <summary>
        /// Get or Add NamedVariable of type T
        /// </summary>
        /// <typeparam name="T">Type of NamedVariable</typeparam>
        /// <param name="fsm"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetAddVariable<T>(this PlayMakerFSM fsm, string name, object value = null) where T : NamedVariable, new()
        {
            T[] VariableArray = (T[])fsm.GetVariables<T>();
            if (VariableArray == null) { return null; }
            T var = fsm.GetVariable<T>(name);
            if (var != null) return var;
            List<T> tempList = new List<T>(VariableArray);
            T newVar = new T();
            newVar.SetName(name);
            if (value != null)
            {
                newVar.RawValue = value;
            }
            tempList.Add(newVar);
            fsm.SetVariables<T>(tempList.ToArray());
            return newVar;
        }


        public static T GetAddVariable<T>(this Fsm fsm, string name, object value = null) where T : NamedVariable, new()
        {

            T[] VariableArray = (T[])fsm.GetVariables<T>();
            if (VariableArray == null) { return null; }
            T var = fsm.GetVariable<T>(name);
            if (var != null) return var;
            List<T> tempList = new List<T>(VariableArray);
            T newVar = new T();
            newVar.SetName(name);
            if (value != null)
            {
                newVar.RawValue = value;
            }
            tempList.Add(newVar);
            fsm.SetVariables<T>(tempList.ToArray());
            return newVar;
        }
        /// <summary>
        /// Get NamedVariable of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fsm"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetVariable<T>(this PlayMakerFSM fsm, string name) where T : NamedVariable, new()
        {
            T[] VariableArray = (T[])fsm.GetVariables<T>();
            if (VariableArray == null) { return null; }
            return Array.Find(VariableArray, x => x.Name == name);
        }
        public static T GetVariable<T>(this Fsm fsm, string name) where T : NamedVariable, new()
        {
            T[] VariableArray = (T[])fsm.GetVariables<T>();
            if (VariableArray == null) { return null; }
            return Array.Find(VariableArray, x => x.Name == name);
        }

    }
}