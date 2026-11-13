#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public partial class StartWindow
{
    private async System.Threading.Tasks.Task VerifyCompact244()
    {
        string audio=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"media244-audio.mp3");
        var peaks=await System.Threading.Tasks.Task.Run(()=>AudioWave244.Decode(audio,4,CancellationToken.None));
        if(peaks.Length==0||!peaks.Any(p=>p>.01))throw new Exception("Gerçek MP3 ses dalgası alınamadı.");
        cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;
        BeginResidentSession(false,false);ShowResidentMenu();
        panelTimer.Stop();weatherTimer220.Stop();EnsureMusic241();
        AddMusicList241("Çalma listesi · Türkçe ğüşİ",new[]{audio});
        BuildSystemPanel();
        var listButton=FindButtons(dashboardCards["music"]).First(b=>System.Windows.Automation.AutomationProperties.GetName(b)=="Çalma listesi");
        ShowPlaylist244(listButton);
        await System.Threading.Tasks.Task.Delay(100);
        var popup=(FrameworkElement)musicPlaylistPopup244.Child;
        popup.UpdateLayout();
        if(musicLists241.FontFamily.Source!="Segoe UI"||musicQueue241.FontFamily.Source!="Segoe UI"||
            FindButtons(popup).Any(b=>b.FontFamily.Source!="Segoe UI"))
            throw new Exception("Liste simge yazı tipini devraldı.");
        var bitmap=new System.Windows.Media.Imaging.RenderTargetBitmap((int)Math.Ceiling(popup.ActualWidth),(int)Math.Ceiling(popup.ActualHeight),96,96,PixelFormats.Pbgra32);
        bitmap.Render(popup);
        var encoder=new System.Windows.Media.Imaging.PngBitmapEncoder();
        encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));
        using(var output=File.Create(OwnPath15("Medya-liste-v2.52.png")))encoder.Save(output);
        musicPlaylistPopup244.IsOpen=false;
        PlayMusic241(0);
        for(int i=0;i<100&&!musicOpened241&&musicPlayer241!=null;i++)await System.Threading.Tasks.Task.Delay(50);
        if(!musicOpened241)throw new Exception("MP3 açılamadı.");
        ToggleMusic241();
        musicPeaks244=peaks;UpdateMusic241();
        ShowMusic241();
        if(musicWindow241.Owner!=null||!musicWindow241.ShowInTaskbar)throw new Exception("Medya bağımsız pencere değil.");
        var floating=musicWindow241;
        HideMenu();
        if(!floating.IsVisible)throw new Exception("Başlat gizlenince medya kapandı.");
        ShowResidentMenu();
        if(!floating.IsVisible||musicWindow241!=floating)throw new Exception("Başlat yeniden açılınca medya değişti.");
        await System.Threading.Tasks.Task.Delay(100);
        musicWindow241.UpdateLayout();
        Capture(musicWindow241,OwnPath15("Medya-yuzen-v2.52.png"));
        dashboardCards["music"].BringIntoView();UpdateLayout();
        Capture(this,OwnPath15("Medya-yan-v2.52.png"));
        ShowVolume244(musicMute244,false);
        if(musicVolume241.Orientation!=Orientation.Vertical)throw new Exception("Dikey ses ayarı yok.");
        music241.Muted244=true;ApplyVolume244();
        if(musicPlayer241.Volume!=0)throw new Exception("Sessize alınmadı.");
        musicVolume241.Value=.3;
        if(music241.Muted244||Math.Abs(musicPlayer241.Volume-.3)>.01)throw new Exception("Ses ayarı uygulanmadı.");
        CloseVolume244();
        ToggleMusicFullscreen242();
        floating.Activate();
        await System.Threading.Tasks.Task.Delay(2800);
        if(musicControls242.Visibility!=Visibility.Collapsed)throw new Exception("Tam ekranda kontroller gizlenmedi.");
        RevealMusic242();
        var escape=new KeyEventArgs(Keyboard.PrimaryDevice,PresentationSource.FromVisual(floating),0,Key.Escape){RoutedEvent=Keyboard.PreviewKeyDownEvent};
        floating.RaiseEvent(escape);
        if(!escape.Handled||musicWindow241!=null||!musicOpened241)throw new Exception("Esc yalnız yüzen görünümü kapatmadı.");
        StopMusic241();
        Console.WriteLine("PASS: MP3 waveform, compact card, independent window, vertical volume/mute, fullscreen and focused Esc.");
        Console.WriteLine("COMPACT_QA_ROOT="+Root);
    }
}
#endif

