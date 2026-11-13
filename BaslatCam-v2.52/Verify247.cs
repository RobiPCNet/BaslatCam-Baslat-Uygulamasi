#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
public partial class StartWindow {
 private async System.Threading.Tasks.Task VerifyTheme247(){
  string report=OwnPath15("Tema-dogrulama-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();
  var tile=cfg.Tiles.First(t=>t.Children==null);Refresh();UpdateLayout();var button=TileButtons().First(b=>((Tile)b.DataContext).Id==tile.Id);double width=button.Width,height=button.Height;
  foreach(string shape in new[]{"square","round","concave","cloud"}){
   cfg.TileShape247=shape;ApplyTheme247();UpdateLayout();check(button.Clip.FillContains(new Point(width*.5,height*.5)),shape+" merkez içeriği görünür");check(button.Width==width&&button.Height==height,shape+" dış ölçü ve yerleşimi değiştirmez");
   if(shape=="square")check(button.Clip.FillContains(new Point(1,1)),"Sert köşenin alanı korunur");else check(!button.Clip.FillContains(shape=="round"?new Point(1,1):new Point(width*.5,1)),shape+" geometri kırpması uygulanır"+ (shape=="round"?" (kontrol altı)":""));
  }
  var gallery=Dialog("Tema karşılaştırması · Aynı içerik / farklı yüzeyler",1020,690);gallery.SetGlassChrome230();var rows=new StackPanel {Margin=new Thickness(18)};var original=cfg;
  foreach(string mode in new[]{"glass","blue","hybrid"}){
   rows.Children.Add(new TextBlock {Text=mode=="glass"?"Saydam Cam":mode=="blue"?"Parlak Mavi":"Hibrit",FontSize=20,Margin=new Thickness(6,8,6,5)});var line=new WrapPanel();rows.Children.Add(line);
   foreach(string shape in new[]{"square","round","concave","cloud"}){
    cfg=json.Deserialize<Config>(json.Serialize(original));cfg.ThemeMode247=mode;cfg.TileShape247=shape;cfg.ThemeMotion247=false;
    var sample=new Button {Width=218,Height=140,Margin=new Thickness(7),Foreground=Brushes.White,Background=new SolidColorBrush(Color.FromArgb(105,20,90,125)),Content=new StackPanel {Children={new Image {Source=ResolveIcon(tile),Width=44,Height=44,HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,12,0,8)},new TextBlock {Text=shape=="square"?"Sert köşeli":shape=="round"?"Yuvarlatılmış":shape=="concave"?"İçbükey":"Düşünce bulutu",TextAlignment=TextAlignment.Center}}}};ApplyTileTheme247(sample);line.Children.Add(sample);
   }
  }
  cfg=original;gallery.Content=rows;gallery.Show();await System.Threading.Tasks.Task.Delay(200);Capture(gallery,OwnPath15("Tema-karsilastirma-v2.52.png"));gallery.Close();
  cfg.TileShape247="round";cfg.ThemeMode247="glass";ApplyTheme247();string before=json.Serialize(cfg);
  settingsProbe=view=>{var mode=FindVisual<ComboBox>(view.Window).First(c=>Convert.ToString(c.Tag)=="theme-mode247");var shape=FindVisual<ComboBox>(view.Window).First(c=>Convert.ToString(c.Tag)=="tile-shape247");mode.SelectedIndex=1;shape.SelectedIndex=3;check(cfg.ThemeMode247=="blue"&&cfg.TileShape247=="cloud","Tema ve bulut seçimi ana ekrana canlı uygulanır");view.Window.UpdateLayout();Capture(view.Window,OwnPath15("Tema-ayarlari-v2.52.png"));view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;if(probeError!=null)throw probeError;check(json.Serialize(cfg)==before,"İptal önceki temayı eksiksiz geri getirir");
  startupWriteProbe226=(enabled,command)=>{};settingsProbe=view=>{FindVisual<ComboBox>(view.Window).First(c=>Convert.ToString(c.Tag)=="theme-mode247").SelectedIndex=2;FindVisual<ComboBox>(view.Window).First(c=>Convert.ToString(c.Tag)=="tile-shape247").SelectedIndex=2;view.Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;startupWriteProbe226=null;if(probeError!=null)throw probeError;
  var saved=json.Deserialize<Config>(File.ReadAllText(FilePath));check(saved.ThemeMode247=="hybrid"&&saved.TileShape247=="concave","Kaydet tema ve biçimi kalıcı saklar");
  cfg.ThemeMode247="blue";cfg.ThemeMotion247=true;cfg.ThemeWaves247=true;ApplyTheme247();await System.Threading.Tasks.Task.Delay(250);button=TileButtons().First(b=>((Tile)b.DataContext).Id==tile.Id);var chrome=(TileChrome247)button.Template.FindName("chrome247",button);check(chrome.Phase>0,"Dalga hareketi açılınca vektör dalga ilerler");cfg.ThemeMotion247=false;ApplyTheme247();check(chrome.Phase==0,"Hareket kapatılınca animasyon durur");cfg.TileShape247="cloud";ApplyTheme247();await System.Threading.Tasks.Task.Delay(100);Capture(this,OwnPath15("Baslat-bulut-v2.52.png"));
 }
}
#endif
