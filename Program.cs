using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

class Program
{

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    private string? activeWindowTitle;
    private string? GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }


    public static void Main(string[] args) 
    {
        Console.CancelKeyPress += new Program().Console_CancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += new Program().AppDomain_ProcessExit;
        Console.Title = "BdCrashRestart";
        new Program().Run(args); // So this method isn't static
    }

    private void AppDomain_ProcessExit(object? sender, EventArgs e)
    {
        log.logWrite("Exiting!");
        Process.GetCurrentProcess().Kill();
    }

    private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        log.logWrite("Exiting!");
        Process.GetCurrentProcess().Kill();
    }

    Logging log = new Logging();

    public string? ActiveWindowTitle { get => activeWindowTitle; set => activeWindowTitle = value; }

    public void Run(string[] args)
    {
        string DiscordPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Discord\\Update.exe"; // Change as needed
        string DiscordProcessName = "discord"; // Change as needed (don't append .exe to it)
        string DiscordArgs = $"--processStart {DiscordProcessName}.exe";
        log.logWrite(log.escColor("0", "150", "255") + "Initialized successfully.");
        while (true)
        {
            string? wndTitle = GetActiveWindowTitle();
            if(wndTitle != null)
            {
                if (wndTitle.ToLower().Contains("betterdiscord crashed")) // checks if the foreground window title contains "betterdiscord crashed" (This isn't a good way of doing this)
                {
                    log.logWrite(log.escColor("255", "0", "0") + "BetterDiscord has crashed!\n" + log.escColor("255", "145", "0") + "Restarting discord");
                    Process[] procs = Process.GetProcessesByName(DiscordProcessName);
                    log.logWrite("Killing processes");
                    foreach (Process proc in procs)
                    {
                        log.logWrite($"Killing process id {proc.Id}, SessionId {proc.SessionId}");
                        proc.Kill();
                    }
                    log.logWrite("Starting Discord");
                    Process.Start(DiscordPath, DiscordArgs);
                    log.logWrite("Done");
                }
            }
            Thread.Sleep(400);
        }
    }
}