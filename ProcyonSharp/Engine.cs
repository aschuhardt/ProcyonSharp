using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using ProcyonSharp.Bindings.Drawing;
using ProcyonSharp.Input;
using ProcyonSharp.Input.Serialization;
using ProcyonSharp.TextEntry;

namespace ProcyonSharp;

public class Engine<T> : NativeEventHandler where T : struct, Enum
{
    private readonly IDictionary<T, GameStateInputFunctionKeyMap> _inputFunctions;
    private readonly Stack<IGameState<T>> _stateStack;
    private T _currentState;
    private TextEntryBuffer _textEntryBuffer;

    internal Engine()
    {
        _stateStack = new Stack<IGameState<T>>();
        _textEntryBuffer = null;
        _inputFunctions = InputFileHelper.Load<T>();
    }

    public bool TextEntryActive => _textEntryBuffer != null;

    protected IGameState<T> CurrentState => _stateStack.Any() ? _stateStack.Peek() : null;

    public void Start(int width, int height, string title)
    {
        using var window = new Window(width, height, title, this);
        Window = window;

        window.Run();
    }

    protected override void OnLoad()
    {
        // initial state needs to have its .Load() call delayed until this point
        // so that by the time it's called by the native Window implementation,
        // it can make changes to the Window that won't be overridden at the start
        // of the draw loop
        CurrentState?.Load();
    }

    internal void InitialState<U>() where U : IGameState<T>, new()
    {
        // push a new state onto the stack without calling Load(), because at this point the Window hasn't been assigned yet so Load() must be called after Start()
        _currentState = GetStateValueFromType(typeof(U));
        _stateStack.Push(new U { Engine = this });
    }

    private static T GetStateValueFromType(MemberInfo stateImplType)
    {
        var stateAttr = stateImplType.GetCustomAttribute<StateAttribute>();
        if (stateAttr?.StateEnumValue is not T stateValue)
            throw new Exception(
                $"Cannot push state of type {stateImplType.Name} as it lacks a valid {nameof(StateAttribute)}");

        return stateValue;
    }

    public void PushState<U, V>(V parameter) where U : IParameterizedGameState<T, V>, new()
    {
        _currentState = GetStateValueFromType(typeof(U));
        var newState = new U
        {
            Engine = this,
            Parameter = parameter
        };
        newState.Load();
        _stateStack.Push(newState);
    }

    public void ReplaceState<U, V>(V parameter) where U : IParameterizedGameState<T, V>, new()
    {
        _stateStack.Pop().Unload();
        PushState<U, V>(parameter);
    }

    public void PushState<U>() where U : IGameState<T>, new()
    {
        _currentState = GetStateValueFromType(typeof(U));
        var newState = new U { Engine = this };
        newState.Load();
        _stateStack.Push(newState);
    }

    public void ReplaceState<U>() where U : IGameState<T>, new()
    {
        _stateStack.Pop().Unload();
        PushState<U>();
    }

    public IGameState<T> PopState()
    {
        var popped = _stateStack.Pop();
        popped.Unload();
        if (!_stateStack.Any())
            Window.Close();
        else
            _currentState = GetStateValueFromType(_stateStack.Peek().GetType());
        return popped;
    }

    /// <summary>
    ///     Begin processing text input events, storing new character entries in the provided buffer
    /// </summary>
    public void BeginTextEntry(StringBuilder buffer, TextEntryOptions options)
    {
        _textEntryBuffer = new TextEntryBuffer(buffer, options);
    }

    /// <summary>
    ///     Stop processing text input
    /// </summary>
    public void EndTextEntry()
    {
        _textEntryBuffer = null;
    }

    public string BuildInputDescription(string functionName)
    {
        var inputFuncs = _inputFunctions[_currentState].Functions.SelectMany(f => f.Value)
            .Where(f => f.FunctionName?.Equals(functionName) ?? false).ToArray();

        if (!inputFuncs.Any())
            return "(none)";

        var description = new StringBuilder();
        for (var i = 0; i < inputFuncs.Length; i++)
        {
            var inputFunc = inputFuncs[i];
            var desc = new StringBuilder();

            if (inputFunc.Alt)
                desc.Append("Alt + ");
            if (inputFunc.Ctrl)
                desc.Append("Ctrl + ");
            if (inputFunc.Shift)
                desc.Append("Shift + ");

            desc.Append(inputFunc.Key.ToString().ToUpperInvariant());

            if (i > 0)
                description.Append(" | ");

            description.Append(desc);
        }

        return description.ToString();
    }

    protected override void OnCharacterEntered(char c)
    {
        _textEntryBuffer?.HandleTextInput(c);
    }

    protected override void OnDraw(DrawContext ctx, double _)
    {
        CurrentState?.Draw(ctx);
    }

    protected override void OnResized(int width, int height)
    {
        CurrentState?.Resized(width, height);
    }

    protected override void OnKeyReleased(Key key, KeyMod mod)
    {
        if (!TextEntryActive)
            CurrentState?.KeyReleased(key, mod);
    }

    protected override void OnKeyPressed(Key key, KeyMod mod)
    {
        if (!_stateStack.Any())
            return;

        // if text is being entered, then we need to handle key inputs differently
        if (TextEntryActive)
        {
            _textEntryBuffer!.HandleKeyPressed(key, mod);

            if (_textEntryBuffer.Finished)
                EndTextEntry();

            return;
        }

        CurrentState?.KeyPressed(key, mod);

        if (!_inputFunctions.ContainsKey(_currentState))
            return; // current state has no input functions

        var modeInputFunctions = _inputFunctions[_currentState].Functions;
        if (!modeInputFunctions.ContainsKey(key))
            return; // current state doesn't care about this key

        foreach (var inputFunction in modeInputFunctions[key])
            if (inputFunction.Alt == mod.Alt
                && inputFunction.Ctrl == mod.Control
                && inputFunction.Shift == mod.Shift)
                inputFunction.FunctionCall.DynamicInvoke(_stateStack.Peek());
    }

    protected override void OnMouseMoved(double x, double y)
    {
        CurrentState?.MouseMoved(x, y);
    }

    protected override void OnMousePressed(MouseButton button, KeyMod mod)
    {
        CurrentState?.MousePressed(button, mod);
    }

    protected override void OnMouseReleased(MouseButton button, KeyMod mod)
    {
        CurrentState?.MouseReleased(button, mod);
    }
}