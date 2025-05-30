using System;
using UnityEngine;

namespace Nikomata.Runtime
{
    public abstract class Automata
    {
        public abstract string GetCurrentStateName();
    }
    
    [Serializable]
    public sealed class Automata<TSubject>: Automata
    {
        private TSubject _subject;
        [SerializeReference] private AState<TSubject> _state;

        public void Init(TSubject subject, AState<TSubject> initState)
        {
            if (_subject != null) throw new Exception("Automata already initialized");
            _subject = subject;
            EnterState(initState);
        }

        private void EnterState(AState<TSubject> newState)
        {
            _state = newState;
            _state.Init(this, _subject);
            _state?.Enter();
        }

        private AState<TSubject> State
        {
            get
            {
                if (_state == default)
                    throw new Exception("FSM not initialized");
                return _state;
            }
            set
            {
                _state?.Exit();
                EnterState(value);
            }
        }

        public void ChangeState<TNewState>() where TNewState : AState<TSubject>, new() => State = new TNewState();

        public void ChangeState(AState<TSubject> nextState) => State = nextState;
        

        public bool Signal<TPayload>(TPayload payload) where TPayload: struct, ISignalPayload 
        {
            if (State is ISignalListener<TPayload> listener)
            {
                listener.Signal(payload);
                return true;
            }

            return false;
        }

        public bool Signal<TPayload>() where TPayload: struct, ISignalPayload => Signal(new TPayload());

        public bool Request<TPayload, TResult>(TPayload payload, out TResult result) where TPayload: struct, IRequestPayload<TResult>
        {
            if (State is IRequestListener<TPayload, TResult> listener)
            {
                result = listener.Request(payload);
                return true;
            }
            result = default;
            return false;
        }

        public override string GetCurrentStateName()
        {
            if (_state != default)
                return _state.GetType().Name;
            return "Not initialized";
        }
    }
}