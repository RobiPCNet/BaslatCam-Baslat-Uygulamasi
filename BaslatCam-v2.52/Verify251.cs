#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
public partial class StartWindow {
 private GlassReflection249 CardReflection251(Border card){return ((Grid)card.Child).Children.OfType<GlassReflection249>().Single();}
 private byte[] CardPixels251(Border card){int w=(int)Math.Ceiling(card.ActualWidth),h=(int)Math.Ceiling(card.ActualHeight);var bitmap=new RenderTargetBitmap(w,h,96,96,PixelFormats.Pbgra32);bitmap.Render(card);var pixels=new byte[w*h*4];bitmap.CopyPixels(pixels,w*4,0);return pixels;}
 private async Task VerifyPanelReflection251(){
  string report=OwnPath15("Panel-yansima-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.GlassReflection249=true;cfg.ThemeGlow247=1;cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();
  foreach(int width in new[]{360,500}){
   cfg.RightPanelWidth=width;BuildSystemPanel();UpdateLayout();await Task.Delay(200);var card=dashboardCards["metrics"];var effect=CardReflection251(card);
   check(ReferenceEquals(VisualTreeHelper.GetParent(effect),card.Child)&&!effect.IsHitTestVisible,"Yansıma kartın içinde; tıklamayı engellemez · "+width);
   check(Math.Abs(effect.ActualWidth-card.ActualWidth)<2&&effect.ActualHeight>0,"Efekt kart ölçüsünde düzenlenir · "+width);
   check(effect.Progress>0,"Kart yansıması görünürken ilerler · "+width);
   effect.Stop();effect.Progress=.06;await Task.Delay(50);var a=CardPixels251(card);effect.Progress=.32;await Task.Delay(50);var b=CardPixels251(card);int changed=0;int w=(int)Math.Ceiling(card.ActualWidth),h=(int)Math.Ceiling(card.ActualHeight);
   for(int y=0;y<h;y++)for(int x=0;x<w;x++)if(x<7||x>w-8||y<7||y>h-8){int i=(y*w+x)*4;if(Math.Abs(a[i]-b[i])+Math.Abs(a[i+1]-b[i+1])+Math.Abs(a[i+2]-b[i+2])>30)changed++;}
   check(changed>20,"Gerçek kart kenarı piksellerinde hareketli ışık görünür · "+width);effect.Sync();CaptureElement(card,OwnPath15("Kart-yansima-"+width+"-v2.52.png"));
  }
  cfg.PanelOpen=false;BuildSystemPanel();UpdateLayout();await Task.Delay(60);check(glassCards.All(c=>CardReflection251(c).Progress==0),"Panel kapalıyken kart yansımaları çalışmaz");cfg.PanelOpen=true;BuildSystemPanel();UpdateLayout();await Task.Delay(160);check(glassCards.Any(c=>CardReflection251(c).Progress>0),"Panel tekrar açılınca yansımalar başlar");
  string original=json.Serialize(cfg);settingsProbe=view=>{FindVisual<Slider>(view.Window).First(s=>Convert.ToString(s.Tag)=="reflection-speed250").Value=2;view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;if(probeError!=null)throw probeError;await Task.Delay(120);check(json.Serialize(cfg)==original&&glassCards.All(c=>ReferenceEquals(CardReflection251(c).Theme,cfg)),"Ayarlar İptal sonrası kartlar güncel ayara bağlı kalır");
  startupWriteProbe226=(enabled,command)=>{};settingsProbe=view=>{FindVisual<Slider>(view.Window).First(s=>Convert.ToString(s.Tag)=="reflection-speed250").Value=1.75;view.Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;startupWriteProbe226=null;if(probeError!=null)throw probeError;await Task.Delay(160);check(glassCards.All(c=>Math.Abs(CardReflection251(c).CycleSeconds250-8)<.01)&&glassCards.Any(c=>CardReflection251(c).Progress>0),"Kaydet ve panel yeniden kurma sonrası hız ve hareket korunur");
  cfg.GlassReflection249=false;ApplyTheme247();check(glassCards.All(c=>CardReflection251(c).Progress==0),"Yansıma ayarı kapatılınca bütün kartlar durur");cfg.GlassReflection249=true;ApplyTheme247();HideMenu();await Task.Delay(60);check(glassCards.All(c=>CardReflection251(c).Progress==0),"Başlat gizlenince kart animasyonları durur");ShowResidentMenu();await Task.Delay(180);check(glassCards.Any(c=>CardReflection251(c).Progress>0),"Başlat yeniden açılınca kart animasyonları geri gelir");Capture(this,OwnPath15("Yan-panel-v2.52.png"));
 }
}
#endif
