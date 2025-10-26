using System.Linq;
using HutongGames.PlayMaker;
namespace KIS.Utils
{
    /// <summary>
    /// utilities to work with FSM
    /// </summary>
    public static partial class FsmUtil
    {

        /// <summary>
        /// AddState
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static FsmState AddState(this PlayMakerFSM fsm, FsmState state)
        {
            var currStates = fsm.Fsm.States;
            var states = new FsmState[currStates.Length + 1];
            var i = 0;
            for (; i < currStates.Length; i++)
            {
                states[i] = currStates[i];
            }
            states[i] = state;
            fsm.Fsm.States = states;
            return states[i];
        }


        /// <summary>
        /// Give all states semi-sensible positions for serialisation
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="startX"></param>
        /// <param name="Y"></param>
        public static void GiveStatesPosition(this PlayMakerFSM fsm, float startX = 500, float Y = 1100)
        {
            var currStates = fsm.Fsm.States;
            var currentX = startX;
            for (var i = 0; i < currStates.Length; i++)
            {
                if (currStates[i].Position.height == 0)
                {
                    var st = currStates[i];
                    st.GiveStatePosition(
                        currentX,
                        Y,
                        10 * st.Name.Length + 10,
                        15 * (st.Transitions.Length + 1)
                    );
                    currentX += st.Position.width + 50;
                }
            }
        }
        /// <summary>
        /// Give a state position for serialisation
        /// </summary>
        /// <param name="state"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public static void GiveStatePosition(this FsmState state, float x, float y, float w, float h)
        {
            state.Position = new Rect(x, y, w, h);
        }

        /// <summary>
        /// Add an empty state
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public static FsmState AddState(this PlayMakerFSM fsm, string stateName)
        {
            return fsm.AddState(new FsmState(fsm.Fsm) { Name = stateName });
        }

        [Obsolete("Satchel.FsmUtil.GetState is deprecated, please use Satchel.FsmUtil.TryGetState or Satchel.FsmUtil.GetValidState instead.")]
        public static FsmState GetState(this PlayMakerFSM fsm, string stateName)
        {
            return fsm.Fsm.GetState(stateName);
        }

        /// <summary>
        /// Get state as the out parameter if not null
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="state"></param>
        /// <returns>true if state is not null</returns>
        public static bool TryGetState(this PlayMakerFSM fsm, string stateName, out FsmState state)
        {
            state = fsm.Fsm.GetState(stateName);
            return state != null;
        }
        /// <summary>
        /// GetState or throw ArgumentException
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static FsmState GetValidState(this PlayMakerFSM fsm, string stateName)
        {
            if (fsm.TryGetState(stateName, out var state))
            {
                return state;
            }
            throw new ArgumentException($"FSM \"{fsm.FsmName}\" does not have state \"{stateName}\"");
        }

        /// <summary>
        /// Copy state with origStateName as newStateName
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="origStateName"></param>
        /// <param name="newStateName"></param>
        /// <returns>newStateName</returns>
        public static FsmState CopyState(this PlayMakerFSM fsm, string origStateName, string newStateName)
        {
            var newState = new FsmState(fsm.Fsm.GetState(origStateName))
            {
                Name = newStateName
            };
            newState.FixTransitionsFromState();
            fsm.AddState(newState);
            return newState;
        }
        /// <summary>
        /// Fix the transitions from state
        /// </summary>
        /// <param name="state"></param>
        public static void FixTransitionsFromState(this FsmState state)
        {
            foreach (var t in state.Transitions)
            {
                t.ToFsmState = state.Fsm.GetState(t.ToState);
            }
        }
        /// <summary>
        /// Fix the transitions to state
        /// </summary>
        /// <param name="state"></param>
        public static void FixTransitionToState(this FsmState state)
        {
            var Fsm = state.Fsm;
            foreach (var s in Fsm.States)
            {
                foreach (var t in s.Transitions)
                {
                    if (t.ToState == state.Name)
                    {
                        t.ToFsmState = state;
                    }
                }
            }
        }
        /// <summary>
        /// Add a transition from the state to state toStateName on event onEventName
        /// </summary>
        /// <param name="state"></param>
        /// <param name="onEventName"></param>
        /// <param name="toStateName"></param>
        public static void AddTransition(this FsmState state, string onEventName, string toStateName)
        {
            var currTransitions = state.Transitions;
            var transitions = new FsmTransition[currTransitions.Length + 1];

            var newTransiton = new FsmTransition
            {
                ToState = toStateName,
                ToFsmState = state.Fsm.GetState(toStateName),
                FsmEvent = FsmEvent.GetFsmEvent(onEventName)
            };

            var i = 0;
            for (; i < currTransitions.Length; i++)
            {
                transitions[i] = currTransitions[i];
            }
            transitions[i] = newTransiton;
            state.Transitions = transitions;
        }

        /// <summary>
        /// Add transition between states by name
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="fromStateName"></param>
        /// <param name="onEventName"></param>
        /// <param name="toStateName"></param>
        public static void AddTransition(this PlayMakerFSM fsm, string fromStateName, string onEventName, string toStateName)
        {
            var state = fsm.Fsm.GetState(fromStateName);
            state.AddTransition(onEventName, toStateName);
        }

        /// <summary>
        /// Remove transition from state on event
        /// </summary>
        /// <param name="state"></param>
        /// <param name="onEventName"></param>
        public static void RemoveTransition(this FsmState state, string onEventName)
        {
            var currTransitions = state.Transitions;
            var transitions = new FsmTransition[currTransitions.Length - 1];
            for (int i = 0, newPos = 0; i < currTransitions.Length; i++)
            {
                if (currTransitions[i].EventName != onEventName)
                {
                    transitions[newPos] = currTransitions[i];
                    newPos++;
                }
            }
            state.Transitions = transitions;
        }

        /// <summary>
        /// Remove transition from state by name
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="fromStateName"></param>
        /// <param name="onEventName"></param>
        public static void RemoveTransition(this PlayMakerFSM fsm, string fromStateName, string onEventName)
        {
            var state = fsm.GetValidState(fromStateName);
            state.RemoveTransition(onEventName);
        }

        /// <summary>
        /// Get transition from fsm by name
        /// </summary>
        /// <param name="state"></param>
        /// <param name="onEventName"></param>
        /// <returns></returns>
        public static FsmTransition GetTransition(this FsmState state, string onEventName)
        {
            var transitions = state.Transitions;
            for (int i = 0; i < transitions.Length; i++)
            {
                if (transitions[i].EventName == onEventName)
                {
                    return transitions[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Get transition from state by name
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="fromStateName"></param>
        /// <param name="onEventName"></param>
        /// <returns></returns>
        public static FsmTransition GetTransition(this PlayMakerFSM fsm, string fromStateName, string onEventName)
        {
            var state = fsm.Fsm.GetState(fromStateName);
            return state.GetTransition(onEventName);
        }

        /// <summary>
        /// Change transition from state by names
        /// </summary>
        /// <param name="state"></param>
        /// <param name="onEventName"></param>
        /// <param name="toStateName"></param>
        public static void ChangeTransition(this FsmState state, string onEventName, string toStateName)
        {
            var transition = state.GetTransition(onEventName);
            transition.ToState = toStateName;
            transition.ToFsmState = state.Fsm.GetState(toStateName);
        }

        /// <summary>
        /// Change transition from fsm by names
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="fromStateName"></param>
        /// <param name="onEventName"></param>
        /// <param name="toStateName"></param>
        public static void ChangeTransition(this PlayMakerFSM fsm, string fromStateName, string onEventName, string toStateName)
        {
            fsm.GetValidState(fromStateName).ChangeTransition(onEventName, toStateName);
        }

        /// <summary>
        /// Add a global transition by names
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="onGlobalEventName"></param>
        /// <param name="toStateName"></param>
        public static void AddGlobalTransition(this PlayMakerFSM fsm, string onGlobalEventName, string toStateName)
        {
            var currTransitions = fsm.Fsm.GlobalTransitions;
            var transitions = new FsmTransition[currTransitions.Length + 1];

            var newTransiton = new FsmTransition
            {
                ToState = toStateName,
                ToFsmState = fsm.Fsm.GetState(toStateName),
                FsmEvent = FsmEvent.GetFsmEvent(onGlobalEventName)
            };

            var i = 0;
            for (; i < currTransitions.Length; i++)
            {
                transitions[i] = currTransitions[i];
            }
            transitions[i] = newTransiton;
            fsm.Fsm.GlobalTransitions = transitions;
        }


        /// <summary>
        /// Get Action from state by index
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static FsmStateAction GetAction(this FsmState state, int index)
        {
            return state.Actions[index];
        }

        /// <summary>
        /// Get Action from fsm by state name and index
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static FsmStateAction GetAction(this PlayMakerFSM fsm, string stateName, int index)
        {
            return fsm.GetValidState(stateName).GetAction(index);
        }

        /// <summary>
        /// Get Action of type from state by index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetAction<T>(this FsmState state, int index) where T : FsmStateAction
        {
            return state.GetAction(index) as T;
        }

        /// <summary>
        /// Get Action of type from fsm by state name and index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetAction<T>(this PlayMakerFSM fsm, string stateName, int index) where T : FsmStateAction
        {
            return fsm.GetValidState(stateName).GetAction<T>(index) as T;
        }

        /// <summary>
        /// Get All Actions of type from state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static T[] GetActions<T>(this FsmState state) where T : FsmStateAction
        {
            var actions = state.Actions;
            var actionOfTypeT = new List<T>();
            foreach (var a in actions)
            {
                if (a.GetType() == typeof(T))
                {
                    actionOfTypeT.Add((T)a);
                }
            }
            return actionOfTypeT.ToArray();
        }

        /// <summary>
        /// Get All Actions of type from fsm for state name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public static T[] GetActions<T>(this PlayMakerFSM fsm, string stateName) where T : FsmStateAction
        {
            return fsm.GetValidState(stateName).GetActions<T>();
        }

        /// <summary>
        /// Get first action of type Action of type from state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static T GetFirstActionOfType<T>(this FsmState state) where T : FsmStateAction
        {
            return state.GetActions<T>().FirstOrDefault();
        }

        /// <summary>
        /// Get first action of type Action of type from fsm and state name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static T GetFirstActionOfType<T>(this PlayMakerFSM fsm, string state) where T : FsmStateAction
        {
            return fsm.GetActions<T>(state).FirstOrDefault();
        }
        /// <summary>
        /// Insert new action at a position in state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="index"></param>
        public static void InsertAction(this FsmState state, FsmStateAction action, int index)
        {
            var currActions = state.Actions;
            var actions = new FsmStateAction[currActions.Length + 1];

            for (int i = 0, oldPos = 0; i < actions.Length; i++, oldPos++)
            {
                if (i == index)
                {
                    actions[i] = action;
                    i++;
                }
                if (oldPos < currActions.Length)
                {
                    actions[i] = currActions[oldPos];
                }
            }
            state.Actions = actions;
            action.Init(state);
        }

        /// <summary>
        /// Insert new action in fsm for statename and positon
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="action"></param>
        /// <param name="index"></param>
        public static void InsertAction(this PlayMakerFSM fsm, string stateName, FsmStateAction action, int index)
        {
            fsm.GetValidState(stateName).InsertAction(action, index);
        }

        /// <summary>
        /// Add Action at the end
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        public static void AddAction(this FsmState state, FsmStateAction action)
        {
            state.InsertAction(action, state.Actions.Length);
        }

        /// <summary>
        /// Add Action at the end
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="action"></param>
        public static void AddAction(this PlayMakerFSM fsm, string stateName, FsmStateAction action)
        {
            fsm.GetValidState(stateName).AddAction(action);
        }

        /// <summary>
        /// Add Action at the start
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        public static void AddFirstAction(this FsmState state, FsmStateAction action)
        {
            state.InsertAction(action, 0);
        }

        /// <summary>
        /// Add Action at the start
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="action"></param>
        public static void AddFirstAction(this PlayMakerFSM fsm, string stateName, FsmStateAction action)
        {
            fsm.GetValidState(stateName).AddFirstAction(action);
        }

        /// <summary>
        /// Remove Action by index
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        public static void RemoveAction(this FsmState state, int index)
        {
            var currActions = state.Actions;
            var actions = new FsmStateAction[currActions.Length - 1];
            for (int i = 0, newPos = 0; i < currActions.Length; i++)
            {
                if (i != index)
                {
                    actions[newPos] = currActions[i];
                    newPos++;
                }
            }
            state.Actions = actions;
        }
        /// <summary>
        /// Remove Action by stateName and index
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="index"></param>
        public static void RemoveAction(this PlayMakerFSM fsm, string stateName, int index)
        {
            fsm.GetValidState(stateName).RemoveAction(index);
        }

        /// <summary>
        /// Disable Action by index
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        public static void DisableAction(this FsmState state, int index)
        {
            state.GetAction(index).Enabled = false;
        }
        /// <summary>
        /// Disable Action by stateName and index
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="state"></param>
        /// <param name="index"></param>
        public static void DisableAction(this PlayMakerFSM fsm, string state, int index)
        {
            fsm.GetValidState(state).DisableAction(index);
        }

        /// <summary>
        /// Add a custom action
        /// </summary>
        /// <param name="state"></param>
        /// <param name="method"></param>
        public static void AddCustomAction(this FsmState state, Action method)
        {
            state.AddAction(new CustomFsmAction() { method = method });
        }

        /// <summary>
        /// Add a custom action
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="method"></param>
        public static void AddCustomAction(this PlayMakerFSM fsm, string stateName, Action method)
        {
            fsm.GetValidState(stateName).AddAction(new CustomFsmAction() { method = method });
        }

        /// <summary>
        /// Add a custom action
        /// </summary>
        /// <param name="state"></param>
        /// <param name="method"></param>
        public static void AddCustomAction(this FsmState state, Action<FsmState> method)
        {
            state.AddAction(new CustomFsmAction() { method = () => method(state) });
        }

        /// <summary>
        /// Add a custom action
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="method"></param>
        public static void AddCustomAction(this PlayMakerFSM fsm, string stateName, Action<PlayMakerFSM> method)
        {
            fsm.GetValidState(stateName).AddAction(new CustomFsmAction() { method = () => method(fsm) });
        }

        /// <summary>
        /// Add a custom action at index
        /// </summary>
        /// <param name="state"></param>
        /// <param name="method"></param>
        public static void InsertCustomAction(this FsmState state, Action method, int index)
        {
            state.InsertAction(new CustomFsmAction() { method = method }, index);
        }

        /// <summary>
        /// Add a custom action at index
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="method"></param>
        /// <param name="index"></param>
        public static void InsertCustomAction(this PlayMakerFSM fsm, string stateName, Action method, int index)
        {
            fsm.GetValidState(stateName).InsertAction(new CustomFsmAction() { method = method }, index);
        }

        /// <summary>
        /// Add a custom action at index that takes the current state as argument
        /// </summary>
        /// <param name="state"></param>
        /// <param name="method"></param>
        /// <param name="index"></param>
        public static void InsertCustomAction(this FsmState state, Action<FsmState> method, int index)
        {
            state.InsertAction(new CustomFsmAction() { method = () => method(state) }, index);
        }

        /// <summary>
        /// Add a custom action at index that takes the current fsm as argument
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        /// <param name="method"></param>
        /// <param name="index"></param>
        public static void InsertCustomAction(this PlayMakerFSM fsm, string stateName, Action<PlayMakerFSM> method, int index)
        {
            fsm.GetValidState(stateName).InsertAction(new CustomFsmAction() { method = () => method(fsm) }, index);
        }


        /// <summary>
        /// Remove state from fsm
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateName"></param>
        public static void RemoveState(this PlayMakerFSM fsm, string stateName)
        {
            var currStates = fsm.Fsm.States;
            fsm.Fsm.States = [.. currStates.Where(state => state.Name != stateName)];
        }
    }

}