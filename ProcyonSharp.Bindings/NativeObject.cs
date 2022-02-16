using System;

namespace ProcyonSharp.Bindings;

public abstract class NativeObject : IDisposable
{
    public IntPtr Pointer { get; protected set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Cleanup();

    private void ReleaseUnmanagedResources()
    {
        Cleanup();
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
    }

    ~NativeObject()
    {
        Dispose(false);
    }
}