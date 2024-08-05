using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using D3D11 = SharpDX.Direct3D11;
using CefSharp.OffScreen;
using CefSharp.Enums;
using CefSharp.Structs;
using CefSharp;
using System.IO;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using CursorType = CefSharp.Enums.CursorType;
using Cursors = System.Windows.Forms.Cursors;
using System.Diagnostics;
using System.Threading;
using SharpDX.Mathematics.Interop;

namespace DirectX
{
    public partial class DXForm : Form
    {
        const bool FULLSCREEN = true;
        const bool EXTERNALTRIGGER = true;
        const bool VSYNC = true;

        D3D11.Device device;
        DeviceMultithread deviceMultithread;
        SwapChain swapChain;
        RenderTargetView renderTarget;
        DeviceContext deferredContext;
        Texture2D[] texture = new Texture2D[2];
        ShaderResourceView[] srv = new ShaderResourceView[2];
        Query query;
        object texLock = new object();
        int curTex = 0;
        Adapter adapter;
        Factory factory;
        static VertexDX11[] Vertices;
        VertexBufferBinding binding;
        D3D11.Buffer vb;
        VertexShader vs;
        PixelShader ps;
        InputLayout layout;
        Screen screen;
        static string ShaderSrc =
"Texture2D tex;   \n" +
"   \n" +
"SamplerState texSampler   \n" +
"{   \n" +
"   Texture = <tex>;   \n" +
"   Filter = MIN_MAG_MIP_POINT; \n" +
"};   \n" +
"   \n" +
"struct PS_IN   \n" +
"{  \n" +
"   float4 pos : SV_POSITION; \n" +
"   float2 Tex : TEXCOORD;  \n" +
"};   \n" +
"   \n" +
"PS_IN vertex( PS_IN input )    \n" +
"{  \n" +
"   return input;   \n" +
"}  \n" +
"   \n" +
"float4 pixel( PS_IN input ) : SV_Target  \n" +
"{  \n" +
"   return tex.Sample( texSampler, input.Tex );   \n" +
"}  \n" +
"   \n";


        ChromiumWebBrowser browser;

        [StructLayout(LayoutKind.Sequential)]
        struct VertexDX11
        {
            public Vector4 Position;
            public Vector2 TexCoord0;

            public static InputElement[] GetInputElements()
            {
                InputElement[] inputElements =
                      {
                new InputElement("SV_POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
                      };
                return inputElements;
            }
        }

        public DXForm()
        {
            InitializeComponent();
            KeyPreview = true;
            screen = Screen.FromControl(this);
        }

        static DXForm()
        {
            Vertices = new VertexDX11[]{
                new VertexDX11()
                {
                    Position = new Vector4(-1, 1, 0, 1),
                    TexCoord0 = new Vector2(0, 0)
                },
                new VertexDX11()
                {
                    Position = new Vector4(3, 1, 0, 1),
                    TexCoord0 = new Vector2(2, 0)
                },
                new VertexDX11()
                {
                    Position = new Vector4(-1, -3, 0, 1),
                    TexCoord0 = new Vector2(0, 2)
                }
            };
        }

        void CreateShaders()
        {
            using (ShaderBytecode byteCode = ShaderBytecode.Compile(ShaderSrc, "vertex", "vs_4_0_level_9_1"))
            {
                vs = new VertexShader(device, byteCode);
                ShaderSignature signature = ShaderSignature.GetInputSignature(byteCode);
                layout = new InputLayout(device, signature, VertexDX11.GetInputElements());
            }

            using (ShaderBytecode byteCode = ShaderBytecode.Compile(ShaderSrc, "pixel", "ps_4_0_level_9_1"))
            {
                ps = new PixelShader(device, byteCode);
            }
        }

        void CreateDevice(bool fullscreen)
        {
            DestroyDevice();

            device = new D3D11.Device(DriverType.Hardware, DeviceCreationFlags.None);

            deviceMultithread = device.QueryInterfaceOrNull<DeviceMultithread>();
            if (deviceMultithread != null)
                deviceMultithread.SetMultithreadProtected(true);

            UpgradeDevice();

            using (SharpDX.DXGI.Device dxgiDev = device.QueryInterface<SharpDX.DXGI.Device>())
                adapter = dxgiDev.Adapter;

            UpgradeAdapter();

            factory = adapter.GetParent<Factory>();

            UpgradeFactory();

            CreateSwapChain(fullscreen);

            UpgradeSwapChain();

            CreateShaders();

            using (DataStream data = DataStream.Create(Vertices, false, false))
            {
                BufferDescription bufferDesc = new BufferDescription()
                {
                    BindFlags = BindFlags.VertexBuffer,
                    SizeInBytes = Vertices.Length * Marshal.SizeOf(typeof(VertexDX11)),
                };
                vb = new D3D11.Buffer(device, data, bufferDesc);

                binding = new VertexBufferBinding()
                {
                    Buffer = vb,
                    Offset = 0,
                    Stride = Marshal.SizeOf(typeof(VertexDX11))
                };
            }

            // Use the rendertarget from the swap chain
            Viewport viewPort;

            using (Texture2D resource = D3D11.Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                //setup view port
                viewPort = new Viewport
                {
                    X = 0,
                    Y = 0,
                    Width = resource.Description.Width,
                    Height = resource.Description.Height,
                    MinDepth = 0.0f,
                    MaxDepth = 1.0f
                };
                renderTarget = new RenderTargetView(device, resource);
            }

            device.ImmediateContext.Rasterizer.SetViewport(viewPort);

            device.ImmediateContext.ClearRenderTargetView(renderTarget, Color4.Black);

            device.ImmediateContext.OutputMerger.SetRenderTargets(renderTarget);
            device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            device.ImmediateContext.InputAssembler.InputLayout = layout;
            device.ImmediateContext.InputAssembler.SetVertexBuffers(0, binding);
            device.ImmediateContext.VertexShader.Set(vs);
            device.ImmediateContext.PixelShader.Set(ps);
        }

        void CreateSwapChain(bool fullscreen)
        {
            // device mode
            Format format = Format.Unknown;
            Format[] formats = { Format.B8G8R8A8_UNorm, Format.R8G8B8A8_UNorm };
            foreach (Format fmt in formats)
            {
                if (device.CheckFormatSupport(fmt).HasFlag(FormatSupport.Display))
                {
                    format = fmt;
                    break;
                }
            }

            int width = screen.Bounds.Width;
            int height = screen.Bounds.Height;

            ModeDescription modeDescription = new ModeDescription()
            {
                Format = format,
                Scaling = DisplayModeScaling.Unspecified,
                ScanlineOrdering = DisplayModeScanlineOrder.Unspecified,
                Width = width,
                Height = height
            };

            //multi-sampling
            SampleDescription sampleDescription = new SampleDescription()
            {
                Count = 1,
                Quality = 0
            };

            SwapChainFlags flags = SwapChainFlags.None;
            if (fullscreen)
                flags |= SwapChainFlags.AllowModeSwitch;

            Factory2 factory2 = factory as Factory2;
            if (factory2 != null)
            {
                SwapChainDescription1 swapChainDescription = new SwapChainDescription1()
                {
                    Width = modeDescription.Width,
                    Height = modeDescription.Height,
                    Format = modeDescription.Format,
                    SampleDescription = sampleDescription,
                    BufferCount = 1,
                    Scaling = Scaling.Stretch,
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput,
                    Flags = flags
                };

                SwapChainFullScreenDescription? fullScreenDescription = null;

                if (fullscreen)
                {
                    fullScreenDescription = new SwapChainFullScreenDescription()
                    {
                        ScanlineOrdering = DisplayModeScanlineOrder.Progressive,
                        Scaling = DisplayModeScaling.Unspecified,
                        Windowed = false
                    };
                }

                CreateSwapChain(factory2, this.Handle, swapChainDescription, fullScreenDescription);
            }
            else
            {
                SwapChainDescription swapChainDescription = new SwapChainDescription()
                {
                    ModeDescription = modeDescription,
                    SampleDescription = sampleDescription,
                    BufferCount = 1,
                    Flags = flags,
                    IsWindowed = !fullscreen,
                    OutputHandle = this.Handle,
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                CreateSwapChain(factory, swapChainDescription);
            }
        }

        private void CreateSwapChain(Factory factory, SwapChainDescription swapChainDescription)
        {
            swapChain = new SwapChain(factory, device, swapChainDescription);
        }

        private void CreateSwapChain(Factory2 factory, IntPtr hWnd, SwapChainDescription1 swapChainDescription, SwapChainFullScreenDescription? fullScreenDescription = null)
        {
            swapChain = new SwapChain1(factory, device, hWnd, ref swapChainDescription, fullScreenDescription);
        }

        private void UpgradeDevice()
        {
            D3D11.Device5 device5 = device.QueryInterfaceOrNull<D3D11.Device5>();
            if (device5 != null)
            {
                device.Dispose();
                device = device5;
                return;
            }
            D3D11.Device4 device4 = device.QueryInterfaceOrNull<D3D11.Device4>();
            if (device4 != null)
            {
                device.Dispose();
                device = device4;
                return;
            }
            D3D11.Device3 device3 = device.QueryInterfaceOrNull<D3D11.Device3>();
            if (device3 != null)
            {
                device = device3;
                return;
            }
            D3D11.Device2 device2 = device.QueryInterfaceOrNull<D3D11.Device2>();
            if (device2 != null)
            {
                device = device2;
                return;
            }
            D3D11.Device1 device1 = device.QueryInterfaceOrNull<D3D11.Device1>();
            if (device1 != null)
            {
                device.Dispose();
                device = device1;
            }
        }

        private void UpgradeAdapter()
        {
            Adapter4 adapter4 = adapter.QueryInterfaceOrNull<Adapter4>();
            if (adapter4 != null)
            {
                adapter.Dispose();
                adapter = adapter4;
                return;
            }
            Adapter3 adapter3 = adapter.QueryInterfaceOrNull<Adapter3>();
            if (adapter3 != null)
            {
                adapter.Dispose();
                adapter = adapter3;
                return;
            }
            Adapter2 adapter2 = adapter.QueryInterfaceOrNull<Adapter2>();
            if (adapter2 != null)
            {
                adapter.Dispose();
                adapter = adapter2;
                return;
            }
            Adapter1 adapter1 = adapter.QueryInterfaceOrNull<Adapter1>();
            if (adapter1 != null)
            {
                adapter.Dispose();
                adapter = adapter1;
            }
        }

        private void UpgradeSwapChain()
        {
            SwapChain4 swapChain4 = swapChain.QueryInterfaceOrNull<SwapChain4>();
            if (swapChain4 != null)
            {
                swapChain.Dispose();
                swapChain = swapChain4;
                return;
            }
            SwapChain3 swapChain3 = swapChain.QueryInterfaceOrNull<SwapChain3>();
            if (swapChain3 != null)
            {
                swapChain.Dispose();
                swapChain = swapChain3;
                return;
            }
            SwapChain2 swapChain2 = swapChain.QueryInterfaceOrNull<SwapChain2>();
            if (swapChain2 != null)
            {
                swapChain.Dispose();
                swapChain = swapChain2;
                return;
            }
            SwapChain1 swapChain1 = swapChain.QueryInterfaceOrNull<SwapChain1>();
            if (swapChain1 != null)
            {
                swapChain.Dispose();
                swapChain = swapChain1;
            }
        }

        private void UpgradeFactory()
        {
            Factory5 factory5 = factory.QueryInterfaceOrNull<Factory5>();
            if (factory5 != null)
            {
                factory.Dispose();
                factory = factory5;
                return;
            }
            Factory4 factory4 = factory.QueryInterfaceOrNull<Factory4>();
            if (factory4 != null)
            {
                factory.Dispose();
                factory = factory4;
                return;
            }
            Factory3 factory3 = factory.QueryInterfaceOrNull<Factory3>();
            if (factory3 != null)
            {
                factory.Dispose();
                factory = factory3;
                return;
            }
            Factory2 factory2 = factory.QueryInterfaceOrNull<Factory2>();
            if (factory2 != null)
            {
                factory.Dispose();
                factory = factory2;
                return;
            }
            Factory1 factory1 = factory.QueryInterfaceOrNull<Factory1>();
            if (factory1 != null)
            {
                factory.Dispose();
                factory = factory1;
            }
        }

        void DestroyDevice()
        {
            if (vs != null)
            {
                vs.Dispose();
                vs = null;
            }

            if (ps != null)
            {
                ps.Dispose();
                ps = null;
            }

            if (vb != null)
            {
                vb.Dispose();
                vb = null;
            }

            if (layout != null)
            {
                layout.Dispose();
                layout = null;
            }

            if (renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }

            if (swapChain != null)
            {
                swapChain.Dispose();
                swapChain = null;
            }

            if (query != null)
            {
                query.Dispose();
                query = null;
            }

            if (deferredContext != null)
            {
                deferredContext.Dispose();
                deferredContext = null;
            }

            foreach (ShaderResourceView srv in srv)
            {
                if (srv != null)
                {
                    srv.Dispose();
                }
            }

            foreach (Texture2D tex in texture)
            {
                if (tex != null)
                {
                    tex.Dispose();
                }
            }

            if (device != null)
            {
                device.Dispose();
                device = null;
            }

            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }

            if (factory != null)
            {
                factory.Dispose();
                factory = null;
            }
        }

        private new void Close()
        {
            DestroyDevice();
            base.Close();
        }

        private void DestroyBrowser()
        {
            if (browser != null)
            {
                browser.Dispose();
                browser = null;
            }
        }

        private void CreateBrowser()
        {
            DestroyBrowser();

            browser = new D3DChromiumWebBrowser("https://www.youtube.com/watch?v=7oAjnqu_wxE", EXTERNALTRIGGER);

            browser.RenderHandler = new D3DRenderHandler(this);
            browser.AudioHandler = new D3DAudioHandler(this);
        }

        private void DXForm_Shown(object sender, EventArgs e)
        {
            CreateDevice(FULLSCREEN);

            CreateBrowser();

            (new Thread(RenderThread)).Start();
        }

        AutoResetEvent waitCopy = new AutoResetEvent(false);
        volatile CommandList commandList;

        private void RenderThread()
        {
            while (true)
            {
                if (EXTERNALTRIGGER)
                {
                    browser?.GetBrowserHost()?.SendExternalBeginFrame();
                }

                if (commandList != null)
                {
                    device.ImmediateContext.ExecuteCommandList(commandList, true);

                    RawBool q = device.ImmediateContext.GetData<RawBool>(query);
                    while (!q)
                    {
                        Thread.Yield();
                        q = device.ImmediateContext.GetData<RawBool>(query);
                    }

                    waitCopy.Set();
                    commandList.Dispose();
                    commandList = null;

                    lock (texLock)
                    {
                        // Swap textures
                        curTex ^= 1;
                    }
                }

                lock (texLock)
                {
                    if (srv[curTex] != null)
                    {
                        device.ImmediateContext.PixelShader.SetShaderResources(0, new[] { srv[curTex] });
                        device.ImmediateContext.Draw(Vertices.Length, 0);
                    }
                }

                swapChain.Present(VSYNC ? 1 : 0, PresentFlags.None);
            }
        }

        class D3DChromiumWebBrowser : ChromiumWebBrowser
        {
            public D3DChromiumWebBrowser(string url, bool externalFrameTrigger) : base(url, null,
                new RequestContext(new RequestContextSettings() { CachePath = Path.GetFullPath("cache") }), false)
            {
                WindowInfo windowInfo = new WindowInfo();
                windowInfo.SetAsWindowless(IntPtr.Zero);
                windowInfo.WindowlessRenderingEnabled = true;
                windowInfo.ExternalBeginFrameEnabled = externalFrameTrigger;
                windowInfo.SharedTextureEnabled = true;

                CreateBrowser(windowInfo, new BrowserSettings()
                {
                    WindowlessFrameRate = 60
                });
            }
        }

        /* User input */
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseButtonType button = MouseButtonType.Left;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    button = MouseButtonType.Left;
                    break;
                case MouseButtons.Middle:
                    button = MouseButtonType.Middle;
                    break;
                case MouseButtons.Right:
                    button = MouseButtonType.Right;
                    break;
                default:
                    return;
            }

            browser.GetBrowserHost()?.SendMouseClickEvent(e.X, e.Y, button, false, 1, CefEventFlags.None);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseButtonType button = MouseButtonType.Left;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    button = MouseButtonType.Left;
                    break;
                case MouseButtons.Middle:
                    button = MouseButtonType.Middle;
                    break;
                case MouseButtons.Right:
                    button = MouseButtonType.Right;
                    break;
                default:
                    return;
            }

            browser.GetBrowserHost()?.SendMouseClickEvent(e.X, e.Y, button, true, 1, CefEventFlags.None);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            browser.GetBrowserHost()?.SendMouseMoveEvent(e.X, e.Y, false, CefEventFlags.None);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            browser.GetBrowserHost()?.SendKeyEvent(new KeyEvent()
            {
                Type = KeyEventType.KeyDown,
                WindowsKeyCode = e.KeyChar
            });
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Handle fullscreen switch on ALT+Enter
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                swapChain.SetFullscreenState(!swapChain.IsFullScreen, null);
                return;
            }

            char keyValue = (char)e.KeyValue;
            if (!(e.Shift ^ IsKeyLocked(Keys.CapsLock)))
                keyValue = char.ToLowerInvariant(keyValue);

            if (keyValue == '\r')
                keyValue = (char)Key.Return;

            browser.GetBrowserHost()?.SendKeyEvent(new KeyEvent()
            {
                Type = KeyEventType.Char,
                WindowsKeyCode = keyValue
            });
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            char keyValue = (char)e.KeyValue;
            if (!(e.Shift ^ IsKeyLocked(Keys.CapsLock)))
                keyValue = char.ToLowerInvariant(keyValue);

            if (keyValue == '\r')
                keyValue = (char)Key.Return;

            browser.GetBrowserHost()?.SendKeyEvent(new KeyEvent()
            {
                Type = KeyEventType.KeyUp,
                WindowsKeyCode = keyValue,
            });
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            MouseEvent mouseEvent = new MouseEvent(e.X, e.Y, CefEventFlags.None);

            int deltaX = 0;
            int deltaY = e.Delta;

            browser.GetBrowserHost()?.SendMouseWheelEvent(mouseEvent, deltaX, deltaY);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (browser != null)
                browser.ResizeAsync(ClientSize.Width, ClientSize.Height);

            base.OnSizeChanged(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateBrowser();
        }


        /* IRenderHandler implementation */
        public class D3DRenderHandler : IRenderHandler
        {
            DXForm form;

            public D3DRenderHandler(DXForm form)
            {
                this.form = form;
            }

            public void Dispose()
            {
                form = null;
            }

            public ScreenInfo? GetScreenInfo()
            {
                var screenInfo = new ScreenInfo { DeviceScaleFactor = 1.0F };

                return screenInfo;
            }

            public bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
            {
                screenX = viewX;
                screenY = viewY;

                return false;
            }

            public Rect GetViewRect()
            {
                return new Rect(0, 0, form.ClientSize.Width, form.ClientSize.Height);
            }

            public void OnAcceleratedPaint(PaintElementType type, Rect _dirtyRect, AcceleratedPaintInfo acceleratedPaintInfo)
            {
                D3D11.Device1 device1 = form.device as D3D11.Device1;

                using (Texture2D cefTex11 = device1.OpenSharedResource1<Texture2D>(acceleratedPaintInfo.SharedTextureHandle))
                {
                    int nextTex = form.curTex ^ 1;

                    bool createTex = false;

                    // Do we need to initialize or resize textures?
                    if (form.texture[nextTex] == null || 
                        (form.texture[nextTex].Description.Width != cefTex11.Description.Width ||
                            form.texture[nextTex].Description.Height != cefTex11.Description.Height))
                    {
                        if (form.texture[nextTex] != null)
                            form.texture[nextTex].Dispose();

                        form.texture[nextTex] = new Texture2D(form.device, cefTex11.Description);

                        if (form.srv[nextTex] != null)
                            form.srv[nextTex].Dispose();

                        form.srv[nextTex] = new ShaderResourceView(form.device, form.texture[nextTex]);
                    }

                    if (form.query == null)
                    {
                        QueryDescription desc = new QueryDescription()
                        {
                            Type = QueryType.Event,
                            Flags = QueryFlags.None
                        };
                        form.query = new Query(form.device, desc);
                    }

                    if (form.deferredContext == null)
                    {
                        form.deferredContext = new DeviceContext(form.device);
                    }

                    form.deferredContext.CopyResource(cefTex11, form.texture[nextTex]);
                    form.deferredContext.End(form.query);

                    // Wait until any pending commandlist is consumed
                    while (form.commandList != null)
                        Thread.Yield();

                    form.commandList = form.deferredContext.FinishCommandList(false);

                    // Wait until the deferred context is consumed by the main thread
                    form.waitCopy.WaitOne();
                }
            }

            [DllImport("user32.dll", EntryPoint = "ShowCursor"), System.Security.SuppressUnmanagedCodeSecurity]
            internal static extern int ShowCursor(bool bShow);
            bool cursorHidden = false;

            public void OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
            {
                switch (type)
                {
                    case CursorType.None:
                        form.Invoke(() =>
                        {
                            if (!cursorHidden)
                            {
                                ShowCursor(false);
                                cursorHidden = true;
                            }
                            form.Cursor = null;
                            Debug.WriteLine("HideCursor!");
                        });
                        break;
                    default:
                    case CursorType.Pointer:
                        form.Invoke(() =>
                        {
                            if (cursorHidden)
                            {
                                ShowCursor(true);
                                cursorHidden = false;
                            }
                            form.Cursor = Cursors.Default;
                        });
                        break;
                    case CursorType.Hand:
                        form.Invoke(() =>
                        {
                            if (cursorHidden)
                            {
                                ShowCursor(true);
                                cursorHidden = false;
                            }
                            form.Cursor = Cursors.Hand;
                        });
                        break;
                }
            }

            public void OnImeCompositionRangeChanged(CefSharp.Structs.Range selectedRange, Rect[] characterBounds)
            {
            }

            public void OnPaint(PaintElementType type, Rect _dirtyRect, IntPtr buffer, int width, int height)
            {
            }

            private Rect popupRect;

            public void OnPopupShow(bool show)
            {
            }

            public void OnPopupSize(Rect rect)
            {
                popupRect = rect;
            }

            public void OnVirtualKeyboardRequested(IBrowser browser, TextInputMode inputMode)
            {
            }

            public bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
            {
                return false;
            }

            public void UpdateDragCursor(DragOperationsMask operation)
            {
            }
        }

        /* IAudioHandler implementation */
        public class D3DAudioHandler : IAudioHandler
        {
            DXForm form;

            public D3DAudioHandler(DXForm form)
            {
                this.form = form;
            }
            public void Dispose()
            {
                form = null;
            }

            public bool GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref AudioParameters parameters)
            {
                return true;
            }

            public void OnAudioStreamError(IWebBrowser chromiumWebBrowser, IBrowser browser, string errorMessage)
            {
            }

            public void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, nint data, int noOfFrames, long pts)
            {
            }

            public void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, AudioParameters parameters, int channels)
            {
            }

            public void OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
            }
        }
    }
}
