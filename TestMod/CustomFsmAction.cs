using HutongGames.PlayMaker;

namespace KIS
{
    /// <summary>
    /// An FsmStateAction that executes a method when used
    /// </summary>
    public class CustomFsmAction : FsmStateAction
    {
        public Action method;

        public CustomFsmAction(Action method)
        {
            this.method = method;
        }
        public CustomFsmAction() { }

        public override void Reset()
        {
            method = null;
            base.Reset();
        }

        public override void OnEnter()
        {
            method?.Invoke();
            Finish();
        }
    }

    public class CustomFsmActionUpdate : FsmStateAction
    {
        public Action method;

        public CustomFsmActionUpdate(Action method)
        {
            this.method = method;
        }
        public CustomFsmActionUpdate() { }

        public override void Reset()
        {
            method = null;
            base.Reset();
        }

        public override void OnEnter()
        {
            method?.Invoke();
        }

        public override void OnUpdate()
        {
            method?.Invoke();

        }
    }

    public class CustomFsmActionFixedUpdate : FsmStateAction
    {
        public Action method;

        public CustomFsmActionFixedUpdate(Action method)
        {
            this.method = method;
        }
        public CustomFsmActionFixedUpdate() { }

        public override void Reset()
        {
            method = null;
            base.Reset();
        }

        public override void OnEnter()
        {
            method?.Invoke();
        }

        public override void OnFixedUpdate()
        {
            method?.Invoke();

        }
    }

}