#if DIAGNOSTICS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
public partial class StartWindow {
 private async Task VerifyControls250(){
  string report=OwnPath15("Kontroller-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();ApplyTheme247();await Task.Delay(200);
  var button=TileButtons().First();var reflection=(GlassReflection249)button.Template.FindName("reflection249",button);double phase=reflection.Progress;
  cfg.GlassSpeed250=2;ApplyTheme247();check(Math.Abs(reflection.CycleSeconds250-7)<.01,"2× hız: yansıma turu 7 saniye");check(Math.Abs(reflection.Progress-phase)<.06,"Hız değişimi yansıma konumunu başa sıçratmaz");check(reflections249.Values.All(a=>Math.Abs(a.Surface.CycleSeconds250-7)<.01),"Hız bütün panel ve başlık yansımalarına uygulanır");
  cfg.GlassSpeed250=.25;ApplyTheme247();check(Math.Abs(reflection.CycleSeconds250-56)<.01,"0,25× hız: yansıma turu 56 saniye");
  string original=json.Serialize(cfg);settingsProbe=view=>{FindVisual<TextBox>(view.Window).First(t=>Convert.ToString(t.Tag)=="reflection-speed-number250").Text="1,75";check(Math.Abs(cfg.GlassSpeed250-1.75)<.01,"Elle virgüllü hız değeri kabul edilir");Capture(view.Window,OwnPath15("Yansima-hizi-v2.52.png"));view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;if(probeError!=null)throw probeError;check(json.Serialize(cfg)==original,"İptal önceki hızı geri alır");
  startupWriteProbe226=(enabled,command)=>{};settingsProbe=view=>{FindVisual<Slider>(view.Window).First(t=>Convert.ToString(t.Tag)=="reflection-speed250").Value=1.5;view.Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;startupWriteProbe226=null;if(probeError!=null)throw probeError;check(json.Deserialize<Config>(File.ReadAllText(FilePath)).GlassSpeed250==1.5,"Yansıma hızı kalıcı kaydedilir");
  var window=Dialog("Canlı konum araması",600,400);var body=new StackPanel {Margin=new Thickness(20)};var query=new TextBox();var list=new ListBox();var status=new TextBlock();var searchButton=Btn("Şehir ara",()=>{});body.Children.Add(query);body.Children.Add(searchButton);body.Children.Add(list);body.Children.Add(status);window.Content=body;
  var calls=new List<string>();var tasks=new Dictionary<string,TaskCompletionSource<List<WeatherPlace220>>>();placeSearchProbe250=q=>{calls.Add(q);var task=new TaskCompletionSource<List<WeatherPlace220>>();tasks[q]=task;return task.Task;};WirePlaceSearch250(window,query,list,status,searchButton);window.Show();await Task.Delay(70);
  query.Text="B";await Task.Delay(600);check(calls.Count==0,"Tek harf sunucuya gönderilmez");query.Text="Bu";await Task.Delay(650);check(calls.SequenceEqual(new[]{"Bu"}),"Yazarken 500 ms sonra otomatik arama");query.Text="Bursa";await Task.Delay(650);check(calls.Count==1,"Önceki istek sürerken ikinci ağ isteği açılmaz");tasks["Bu"].SetResult(new List<WeatherPlace220>{new WeatherPlace220{Name="Eski"}});await Task.Delay(120);check(calls.Last()=="Bursa"&&list.Items.Count==0,"Geç gelen eski sonuç atılır; en yeni sorgu başlatılır");tasks["Bursa"].SetResult(new List<WeatherPlace220>{new WeatherPlace220{Name="Bursa",Lat=40.18,Lon=29.06}});await Task.Delay(100);check(((WeatherPlace220)list.Items[0]).Name=="Bursa","Güncel şehir sonuçları görüntülenir");
  query.Text="Bolu";var enter=new KeyEventArgs(Keyboard.PrimaryDevice,PresentationSource.FromVisual(query),0,Key.Enter){RoutedEvent=Keyboard.PreviewKeyDownEvent};query.RaiseEvent(enter);await Task.Delay(30);check(enter.Handled&&calls.Last()=="Bolu","Enter beklemeden aratır");tasks["Bolu"].SetResult(new List<WeatherPlace220>{new WeatherPlace220{Name="Bolu"}});await Task.Delay(650);check(calls.Count==3,"Enter sonrasında bekleyen otomatik arama tekrar çalışmaz");
  query.Text="Ankara";searchButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));await Task.Delay(30);check(calls.Last()=="Ankara","Şehir ara düğmesi de çalışmaya devam eder");window.Close();tasks["Ankara"].SetResult(new List<WeatherPlace220>{new WeatherPlace220{Name="Ankara"}});await Task.Delay(80);check(list.Items.Count==0,"Pencere kapandıktan sonra sonuç arayüze yazılmaz");placeSearchProbe250=null;
  var waveform=new MediaWaveform244 {Peaks=new float[]{.2f,.8f,.4f,1,.6f,.3f},Progress=.5,Width=300,Height=80};waveform.Measure(new Size(300,80));waveform.Arrange(new Rect(0,0,300,80));var image=new RenderTargetBitmap(300,80,96,96,PixelFormats.Pbgra32);image.Render(waveform);var pixels=new byte[300*80*4];image.CopyPixels(pixels,1200,0);int blue=0,red=0;for(int i=0;i<pixels.Length;i+=4)if(pixels[i+3]>30){if(pixels[i]>pixels[i+2]+20)blue++;if(pixels[i+2]>pixels[i]+20)red++;}check(blue>50&&red==0,"Ses dalgası kırmızı yerine mavi-turkuaz çizilir");CaptureElement(waveform,OwnPath15("Ses-dalgasi-v2.52.png"));
 }
}
#endif
