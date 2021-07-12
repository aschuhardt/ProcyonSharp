using System;
using System.Runtime.InteropServices;
using ProcyonSharp.Bindings.Drawing;

// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedParameter.Global

namespace ProcyonSharp.Bindings
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct KeyInfo
    {
        private readonly IntPtr Name;
        public readonly int Value;
    }

    /// <summary>
    ///     Represents the state of modifier keys held during a key press/release input event
    /// </summary>
    public class KeyMod
    {
        public KeyMod(bool shift, bool control, bool alt)
        {
            Shift = shift;
            Control = control;
            Alt = alt;
        }

        /// <summary>
        ///     Whether the Shift key was pressed
        /// </summary>
        public bool Shift { get; }

        /// <summary>
        ///     Whether the Ctrl key was pressed
        /// </summary>
        public bool Control { get; }

        /// <summary>
        ///     Whether the Alt key was pressed
        /// </summary>
        public bool Alt { get; }
    }

    /// <summary>
    ///     The base type representing a game state object which defines behavior for handling events
    /// </summary>
    public abstract class NativeEventHandler : NativeObject
    {
        private DrawContext _drawContext;
        private Window _window;

        protected NativeEventHandler()
        {
            _nativeLoadCallback = OnNativeLoad;
            _nativeUnloadCallback = OnNativeUnload;
            _nativeDrawCallback = OnNativeDraw;
            _nativeResizedCallback = OnNativeResized;
            _nativeKeyPressedCallback = OnNativeKeyPressed;
            _nativeKeyReleasedCallback = OnNativeKeyReleased;
            _nativeCharEnteredCallback = OnNativeCharacterEntered;
            Pointer = CreateCallbackState(_nativeLoadCallback, _nativeUnloadCallback, _nativeDrawCallback,
                _nativeResizedCallback, _nativeKeyPressedCallback, _nativeKeyReleasedCallback,
                _nativeCharEnteredCallback);
        }

        public Window Window
        {
            get => _window ?? throw new Exception("State object was never assigned a Window instance!");
            protected set
            {
                _window = value;
                _drawContext = new DrawContext(_window);
            }
        }

        protected override void Cleanup()
        {
            DestroyCallbackState(Pointer);
        }

        [DllImport("procyon", EntryPoint = "procy_create_callback_state")]
        private static extern IntPtr CreateCallbackState(OnNativeLoadCallback onLoadCallback,
            OnNativeUnloadCallback onUnloadCallback, OnNativeDrawCallback onDrawCallback,
            OnNativeResizedCallback onResizedCallback, OnNativeKeyPressedCallback onKeyPressedCallback,
            OnNativeKeyReleasedCallback onKeyReleasedCallback,
            OnNativeCharEnteredCallback onCharEnteredCallback);

        [DllImport("procyon", EntryPoint = "procy_destroy_state")]
        private static extern void DestroyCallbackState(IntPtr statePtr);

        /// <summary>
        ///     Called at the beginning of execution; should be used to load resources and perform any setup routines
        /// </summary>
        protected virtual void OnLoad()
        {
        }

        /// <summary>
        ///     Called at the end of execution; clean up resources and perform teardown routines here
        /// </summary>
        protected virtual void OnUnload()
        {
        }

        /// <summary>
        ///     Called each time the window is re-drawn
        /// </summary>
        /// <param name="ctx">Provides drawing functionality</param>
        /// <param name="time">The duration, in seconds, since the last time <see cref="OnDraw" /> was called</param>
        protected virtual void OnDraw(DrawContext ctx, double time)
        {
        }

        /// <summary>
        ///     Called when the window is resized
        /// </summary>
        /// <param name="width">The new width of the usable window space, in pixels</param>
        /// <param name="height">The new height of the usable window space, in pixels</param>
        protected virtual void OnResized(int width, int height)
        {
        }

        /// <summary>
        ///     Called when a keyboard key is pressed
        /// </summary>
        /// <param name="key">The key that was pressed</param>
        /// <param name="mod">The modifiers associated with the key press</param>
        protected virtual void OnKeyPressed(Key key, KeyMod mod)
        {
        }

        /// <summary>
        ///     Called when a keyboard key is released
        /// </summary>
        /// <param name="key">The key that was released</param>
        /// <param name="mod">The modifiers associated with the key release</param>
        protected virtual void OnKeyReleased(Key key, KeyMod mod)
        {
        }

        /// <summary>
        ///     Called when the user inputs a text character
        /// </summary>
        /// <param name="c">The ASCII character entered</param>
        protected virtual void OnCharacterEntered(char c)
        {
        }

        private void OnNativeLoad(IntPtr nativeState)
        {
            OnLoad();
        }

        private void OnNativeUnload(IntPtr _)
        {
            OnUnload();
        }

        private void OnNativeDraw(IntPtr _, double time)
        {
            if (_drawContext != null)
                OnDraw(_drawContext, time);
        }

        private void OnNativeResized(IntPtr _, int width, int height)
        {
            OnResized(width, height);
        }

        private void OnNativeKeyPressed(IntPtr _, KeyInfo key, bool shift, bool ctrl, bool alt)
        {
            OnKeyPressed((Key) key.Value, new KeyMod(shift, ctrl, alt));
        }

        private void OnNativeKeyReleased(IntPtr _, KeyInfo key, bool shift, bool ctrl, bool alt)
        {
            OnKeyReleased((Key) key.Value, new KeyMod(shift, ctrl, alt));
        }

        private void OnNativeCharacterEntered(IntPtr _, uint c)
        {
            OnCharacterEntered((char) (0x0000ff & c));
        }

        private delegate void OnNativeLoadCallback(IntPtr _);

        private delegate void OnNativeUnloadCallback(IntPtr _);

        private delegate void OnNativeDrawCallback(IntPtr _, double time);

        private delegate void OnNativeResizedCallback(IntPtr _, int width, int height);

        private delegate void OnNativeKeyPressedCallback(IntPtr _, KeyInfo key, bool shift, bool ctrl, bool alt);

        private delegate void OnNativeKeyReleasedCallback(IntPtr _, KeyInfo key, bool shift, bool ctrl, bool alt);

        private delegate void OnNativeCharEnteredCallback(IntPtr _, uint codepoint);


        // Maintain references to callback delegates in order to prevent them from being GC'd
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        private readonly OnNativeLoadCallback _nativeLoadCallback;
        private readonly OnNativeUnloadCallback _nativeUnloadCallback;
        private readonly OnNativeDrawCallback _nativeDrawCallback;
        private readonly OnNativeResizedCallback _nativeResizedCallback;
        private readonly OnNativeKeyPressedCallback _nativeKeyPressedCallback;
        private readonly OnNativeKeyReleasedCallback _nativeKeyReleasedCallback;

        private readonly OnNativeCharEnteredCallback _nativeCharEnteredCallback;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
    }
}