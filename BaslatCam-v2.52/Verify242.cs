#if DIAGNOSTICS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

public partial class StartWindow
{
    private async System.Threading.Tasks.Task VerifyDashboardMedia242()
    {
        Func<string, double, KeyValuePair<string,double>> value = (name, number) => new KeyValuePair<string,double>(name, number);
        double? combined = GpuUsage242.Aggregate(new[] {
            value("pid_1_luid_a_phys_0_eng_0_engtype_3D",20),
            value("pid_2_luid_a_phys_0_eng_0_engtype_3D",30),
            value("pid_3_luid_a_phys_0_eng_1_engtype_Copy",12),
            value("pid_4_luid_b_phys_0_eng_0_engtype_3D",70) });
        if (combined != 70 || GpuUsage242.Aggregate(new KeyValuePair<string,double>[0]).HasValue)
            throw new Exception("GPU motorları yanlış birleştirildi.");
        using (var live = new GpuUsage242())
        {
            live.Read();
            await System.Threading.Tasks.Task.Delay(1100);
            double? load = live.Read();
            Console.WriteLine("GPU live " + (load.HasValue ? load.Value.ToString("0.0") + "%" : live.Status));
        }
        cfg = Defaults();
        cfg.PanelPinned = true;
        cfg.AutoCollapseOverlap = false;
        ShowResidentMenu();
        panelTimer.Stop();
        weatherTimer220.Stop();
        for (int i=0;i<10;i++) MovePanelCard242("music",-1);
        if (panelBody.Children[0] != dashboardCards["music"]) throw new Exception("Kart üst sıraya taşınmadı.");
        settingsProbe = view =>
        {
            cfg.PanelOrder242 = "network,music,metrics,disks,system,tools";
            BuildSystemPanel();
            view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        };
        Settings();
        settingsProbe = null;
        if (PanelOrder242()[0] != "music" || panelBody.Children[0] != dashboardCards["music"])
            throw new Exception("İptal kart sırasını geri almadı.");
        if (!Save() || json.Deserialize<Config>(File.ReadAllText(FilePath)).PanelOrder242 != cfg.PanelOrder242)
            throw new Exception("Kart sırası saklanmadı.");

        EnsureMusic241();
        string video = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"media242-test.wmv");
        AddMusicList241("Örnek liste", new[] { video });
        PlayMusic241(0);
        for(int i=0;i<120&&!musicOpened241&&musicPlayer241!=null;i++)
            await System.Threading.Tasks.Task.Delay(50);
        if(!musicOpened241||!musicPlayer241.HasVideo)throw new Exception("Video açılamadı.");
        ToggleMusic241();
        musicTrackTitle242 = "Örnek şarkı";
        musicTrackArtist242 = "Örnek sanatçı";
        ShowMusic241();
        UpdateLayout();
        musicWindow241.UpdateLayout();
        if(!(musicScreen241.Fill is System.Windows.Media.DrawingBrush))throw new Exception("Video arka planda çizilmedi.");
        musicSeek241.Value = 2;
        if(Math.Abs(musicPlayer241.Position.TotalSeconds-2)>.5)throw new Exception("İlerleme çubuğu çalışmadı.");
        Capture(musicWindow241,OwnPath15("Medya-v2.52.png"));
        ToggleMusicFullscreen242();
        if (!musicFullscreen242 || !musicWindow241.MediaFull242) throw new Exception("Tam ekran açılmadı.");
        await System.Threading.Tasks.Task.Delay(2800);
        if (musicControls242.Visibility != Visibility.Collapsed) throw new Exception("Tam ekran kontrolleri gizlenmedi.");
        RevealMusic242();
        if (musicControls242.Visibility != Visibility.Visible) throw new Exception("Kontroller geri gelmedi.");
        ToggleMusicFullscreen242();
        if (musicFullscreen242 || musicWindow241.MediaFull242) throw new Exception("Pencereye dönülemedi.");
        music241.Shuffle242 = true;
        musicIndex241 = 0;
        musicHistory242.Clear();
        int next = NextMusic242(1,3);
        if (next == 0 || NextMusic242(-1,3) != 0) throw new Exception("Karışık önceki parça çalışmadı.");
        music241.Shuffle242 = false;
        music241.Repeat242 = "one";
        EndMusic242();
        if (musicIndex241 != 0 || musicPlayer241 == null) throw new Exception("Tek parça tekrarı çalışmadı.");
        music241.Repeat242 = "list";
        EndMusic242();
        if (musicIndex241 != 0 || musicPlayer241 == null) throw new Exception("Liste tekrarı çalışmadı.");
        music241.Repeat242 = "off";
        EndMusic242();
        if (musicIndex241 != -1 || musicPlayer241 != null) throw new Exception("Liste sonunda durmadı.");
        musicWindow241.Close();
        StopMusic241();
        Console.WriteLine("PASS: GPU aggregation, panel reorder/cancel/save, media fullscreen hide/reveal and shuffle history.");
        Console.WriteLine("UI_QA_ROOT=" + Root);
    }
}
#endif



