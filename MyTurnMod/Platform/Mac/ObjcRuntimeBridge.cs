using System.Runtime.InteropServices;

namespace MyTurnMod.Platform.Mac;

/// <summary>
/// Calls into the macOS Objective-C runtime to control NSApplication.
/// Only instantiate on macOS.
/// </summary>
internal class ObjcRuntimeBridge : INativeAppBridge
{
    private const string ObjcLib = "/usr/lib/libobjc.dylib";

    // NSRequestUserAttentionType constants
    private const nint NSCriticalRequest = 0;       // bounces until focused
    private const nint NSInformationalRequest = 10; // bounces once

    [DllImport(ObjcLib, EntryPoint = "objc_getClass")]
    private static extern IntPtr ObjcGetClass(string name);

    [DllImport(ObjcLib, EntryPoint = "sel_registerName")]
    private static extern IntPtr SelRegisterName(string name);

    [DllImport(ObjcLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr ObjcMsgSend(IntPtr receiver, IntPtr selector);

    [DllImport(ObjcLib, EntryPoint = "objc_msgSend")]
    private static extern void ObjcMsgSend_Bool(IntPtr receiver, IntPtr selector, bool arg);

    [DllImport(ObjcLib, EntryPoint = "objc_msgSend")]
    private static extern nint ObjcMsgSend_NInt(IntPtr receiver, IntPtr selector, nint arg);

    private IntPtr GetSharedApplication()
    {
        var cls = ObjcGetClass("NSApplication");
        return ObjcMsgSend(cls, SelRegisterName("sharedApplication"));
    }

    public void RequestAttention(bool critical)
    {
        var app = GetSharedApplication();
        ObjcMsgSend_NInt(app, SelRegisterName("requestUserAttention:"),
            critical ? NSCriticalRequest : NSInformationalRequest);
    }

    public void ActivateIgnoringOtherApps()
    {
        var app = GetSharedApplication();
        ObjcMsgSend_Bool(app, SelRegisterName("activateIgnoringOtherApps:"), true);
    }
}
