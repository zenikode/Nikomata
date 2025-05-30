# Nikomata — Minimalist Finite State Machine (FSM) Library for C#  

A lightweight, type-safe state machine implementation with signal/request handling. Inspired by automata theory and designed for clarity.  

---  
## 📦 Installation  
Add the Nikomata.Runtime namespace to your C# project.  

---  
## 🚀 Core Concepts  
### 1. Automata\<TSubject>  
The main FSM controller. Manages states and transitions for a subject (TSubject).  

#### Key Methods:  
| Method | Description |  
|--------|-------------|  
| Init(TSubject, AState<TSubject>) | Initializes the FSM with a subject and starting state. |  
| ChangeState<TNewState>() | Transitions to a new state (instantiates TNewState). |  
| ChangeState(AState<TSubject>) | Transitions using a pre-allocated state object. |  
| Signal<TPayload>(TPayload) | Sends a signal payload to the current state (if it implements ISignalListener<TPayload>). |  
| Request<TPayload, TResult>(TPayload, out TResult) | Sends a request to the current state (if it implements IRequestListener<TPayload, TResult>). Returns false if unhandled. |  

---  
### 2. AState\<TSubject>  
Base class for all states. Override Enter() and Exit() for lifecycle hooks.  

#### Example State:  

```csharp
public class IdleState : AState<Player>  
{  
    public override void Enter() => Console.WriteLine("Entered Idle");  
    public override void Exit() => Console.WriteLine("Exited Idle");  
}   
```

#### Transition Methods (callable inside states):  
- ChangeState<TNewState>()  
- ChangeState(AState<TSubject>)  

---  
### 3. Signals & Requests  
- Signals (fire-and-forget):  
```csharp
  public struct JumpSignal : ISignalPayload { }  
  public class RunState : AState<Player>, ISignalListener<JumpSignal>  
  {  
      public void Signal(JumpSignal payload) => ChangeState<JumpState>();  
  }  
```
- Requests (with responses):  
```csharp
  public struct HealthRequest : IRequestPayload<int> { }  
  public class AliveState : AState<Player>, IRequestListener<HealthRequest, int>  
  {  
      public int Request(HealthRequest payload) => 100;  
  }  
```   

---  
## 🛠️ Usage Example  
```csharp
var player = new Player();  
var fsm = new Automata<Player>();  
fsm.Init(player, new IdleState());  

// State transition:  
fsm.ChangeState<RunState>();  

// Send a signal (e.g., jump input):  
fsm.Signal(new JumpSignal());  

// Send a request (e.g., query health):  
if (fsm.Request(new HealthRequest(), out int health))  
    Console.WriteLine($"Health: {health}");   
```
---  
## 🎯 Design Goals  
✔ Type-Safe – Compile-time checks for states and payloads.  
✔ Minimalist – No dependencies, ~100 LOC.  
✔ Extensible – Add signals/requests without modifying core FSM.  

---  
## 📜 License  
MIT  

---
