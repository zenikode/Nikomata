namespace Nikomata.Runtime
{
    public abstract class AState<TSubject>
    {
        private Automata<TSubject> _automata;
        protected TSubject Subject { get; private set; }
        
        internal void Init(Automata<TSubject> automata, TSubject subject)
        {
            _automata = automata;
            Subject = subject;
        }

        protected void ChangeState(AState<TSubject> nextState) => _automata.ChangeState(nextState);
        
        protected void ChangeState<TNewState>() where TNewState : AState<TSubject>, new() => _automata.ChangeState<TNewState>();

        public virtual void Enter() { }

        public virtual void Exit() { }
    }
}