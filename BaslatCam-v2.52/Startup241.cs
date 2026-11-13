using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

public partial class StartWindow
{
    private DispatcherTimer startupRetry241;
    private int startupAttempts241;

    private void StartStartupRetry241()
    {
        StopStartupRetry241();
        startupAttempts241 = 0;
        startupRetry241 = new DispatcherTimer(DispatcherPriority.Background)
        { Interval = TimeSpan.FromSeconds(3) };
        startupRetry241.Tick += RetryStartup241;
        startupRetry241.Start();
        RetryStartup241(null, EventArgs.Empty);
    }

    private void RetryStartup241(object sender, EventArgs e)
    {
        if (exitRequested) { StopStartupRetry241(); return; }
        startupAttempts241++;
        bool ready;
        string error = "";
        if (startupIntegrationProbe241 != null)
        {
            startupIntegrationProbe241(true);
            ready = true;
        }
        else ready = SetStartIntegration(true, out error);
        WriteResidentStatus();
        try
        {
            File.WriteAllText(OwnPath15("baslangic-durum.txt"),
                "Zaman " + DateTime.Now.ToString("O") +
                "\r\nDeneme " + startupAttempts241 +
                "\r\nHazır " + ready +
                "\r\nGörünür " + IsVisible +
                "\r\nHata " + error);
        }
        catch { }
        if (ready || startupAttempts241 >= 10) StopStartupRetry241();
    }

    private void StopStartupRetry241()
    {
        if (startupRetry241 == null) return;
        startupRetry241.Stop();
        startupRetry241.Tick -= RetryStartup241;
        startupRetry241 = null;
    }

#if DIAGNOSTICS
    private async System.Threading.Tasks.Task VerifyMusicStartup241()
    {
        cfg = Defaults();
        cfg.PanelPinned = true;
        cfg.AutoCollapseOverlap = false;
        ShowResidentMenu();
        panelTimer.Stop();
        weatherTimer220.Stop();
        EnsureMusic241();
        AddMusicList241("Deneme", new[] { @"C:\music\test.mp3", @"C:\music\test.mp3", @"C:\music\readme.txt" });
        var saved = json.Deserialize<MusicState241>(File.ReadAllText(OwnPath15("muzik.json")));
        if (saved.Lists.Count != 1 || saved.Lists[0].Files.Count != 1 || musicPlaying241 || musicWindow241 != null)
            throw new Exception("Liste kalıcı saklanmadı veya kendiliğinden oynatma başladı.");
        BuildSystemPanel();
        if (!dashboardCards.ContainsKey("music")) throw new Exception("Müzik kartı eksik.");
        ShowMusic241();
        if (musicWindow241 == null || !musicWindow241.IsVisible) throw new Exception("Görsel açılmadı.");
        musicWindow241.Close();
        if (musicWindow241 != null || musicScreen241 != null) throw new Exception("Görsel kapatılmadı.");
        PlayMusic241(0);
        if (musicPlayer241 != null || !musicMessage241.StartsWith("Dosya bulunamadı"))
            throw new Exception("Eksik dosya doğru işlenmedi.");
        string audio = OwnPath15("sessiz-oynatma.wav");
        using (var writer = new BinaryWriter(File.Create(audio)))
        {
            int dataLength = 8000 * 2 * 3;
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(36 + dataLength);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)1);
            writer.Write(8000);
            writer.Write(16000);
            writer.Write((short)2);
            writer.Write((short)16);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(dataLength);
            writer.Write(new byte[dataLength]);
        }
        AddMusicList241("Ses denemesi", new[] { audio });
        PlayMusic241(0);
        for (int i = 0; i < 100 && !musicOpened241 && musicPlayer241 != null; i++)
            await System.Threading.Tasks.Task.Delay(50);
        if (!musicOpened241 || !musicPlaying241) throw new Exception("WAV açılamadı.");
        ToggleMusic241();
        if (musicPlaying241) throw new Exception("Duraklatma çalışmadı.");
        ToggleMusic241();
        if (!musicPlaying241) throw new Exception("Oynatmaya devam edilmedi.");
        StopMusic241();
        Console.WriteLine("PASS: silent WAV playback, pause and resume.");
        UpdateLayout();
        dashboardCards["music"].BringIntoView();
        await System.Threading.Tasks.Task.Delay(100);
        Capture(this, OwnPath15("Muzik-karti-v2.52.png"));
        string path = OwnPath15("konum denemesi.exe");
        File.WriteAllText(path, "");
        string arguments = "";
        locationProbe241 = (exe, args) => arguments = args;
        OpenLocation241(new Tile { Target = path });
        if (arguments != "/select,\"" + path + "\"") throw new Exception("Explorer seçim yolu bozuk.");
        locationProbe241 = null;
        bool enabled = false;
        startupIntegrationProbe241 = value => enabled = value;
        BeginStartup292();
        if (!enabled || IsVisible || startupRetry241 != null)
            throw new Exception("Gizli başlangıç etkinleşmedi.");
        ShowResidentMenu();
        if (!IsVisible || !full) throw new Exception("Tıklama sonrası tam ekran açılmadı.");
        startupIntegrationProbe241 = null;
        if (!StartupCommand226(@"C:\bir klasör\Başlat.exe").EndsWith("\" --startup"))
            throw new Exception("Başlangıç komutu bozuk.");
        Console.WriteLine("PASS: persistent playlist, optional video, missing file, Explorer selection and hidden startup.");
        await System.Threading.Tasks.Task.Delay(100);
    }
#endif
}




