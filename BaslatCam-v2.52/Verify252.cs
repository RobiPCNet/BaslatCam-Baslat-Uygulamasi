#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
public partial class StartWindow {
 private async Task VerifyWeather252(){
  string report=OwnPath15("Hava-dogrulama-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;cfg.WeatherChosen=true;cfg.WeatherBackdropAlways=true;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();NormalizeLayers312();for(int i=0;i<3;i++)cfg.WeatherLayers[i].Enabled=i==1;cfg.WeatherLayers[1].Source="auto-effect";cfg.WeatherLayers[1].Opacity=1;
  check(RainMotion240.Count==20&&SnowMotion240.Count==280,"Kayan damla 20; irili ufaklı kar tanesi 280");
  foreach(string kind in new[]{"rain","storm","wind","snow"}){
   weatherData220=new WeatherData220 {Code=kind=="storm"?95:kind=="snow"?73:61,Day=1,FetchedUtc=DateTime.UtcNow};
   cfg.WeatherLayers[1].Source=kind=="wind"?"efektler/ucusan-yapraklar":"auto-effect";PlayLayers312(WeatherSceneKey311(weatherData220));await WaitWeather311();UpdateLayout();
   check(activeMotion240!=null&&motionTimer240.IsEnabled,"Hareket ve zamanlayıcı etkin · "+kind);
   check(layerGrids312[0].Visibility==Visibility.Collapsed&&layerGrids312[2].Visibility==Visibility.Collapsed,"Efekt yalnız ikinci katmanda çalışır · "+kind);
   if(kind=="rain")check(activeMotion240 is RainMotion240,"Normal yağmur güçlendirilmiş damla modelini seçer");
   if(kind=="storm"){var storm=activeMotion240 as StormMotion252;check(storm!=null,"Gök gürültülü hava şimşekli yağmuru seçer");double before=storm.Elapsed;storm.Advance(.8-before);check(storm.Flash>.08,"Kısa şimşek çakması görünür");}
   if(kind=="wind"){var wind=activeMotion240 as WindMotion252;check(wind!=null,"Rüzgâr seçimi savrulan yaprakları başlatır");double before=wind.Elapsed;wind.Advance(2);check(wind.Elapsed>before,"Yaprak konumu ve dönüşü zamanla ilerler");}
   if(kind=="snow")check(activeMotion240 is SnowMotion240,"Karlı hava yoğun kar modelini seçer");
   Capture(this,OwnPath15("Efekt-"+kind+"-v2.52.png"));
  }
  cfg.WeatherLayers[1].Source="auto-effect";check(AutoEffect312("gece/ruzgarli")=="efektler/ucusan-yapraklar","Gece rüzgârında da yaprak seçimi");
  foreach(string key in new[]{"efektler/simsek","efektler/ucusan-yapraklar"}){galleryOverride333=key;var plan=PlanLayers312(null).Single();check(plan.Slot==1&&plan.Motion==(key.EndsWith("simsek")?"storm":"wind"),"Önizleme Başlat üzerinde hareketli · "+key);}galleryOverride333=null;
  for(int i=0;i<3;i++)cfg.WeatherLayers[i].Enabled=i==0;PlayLayers312("gunduz/yagmurlu");await WaitWeather311();check(activeMotion240==null,"İkinci katman kapatılınca hareket yok");cfg.WeatherLayers[0].Enabled=false;cfg.WeatherLayers[1].Enabled=true;weatherData220=new WeatherData220 {Code=95,Day=0,FetchedUtc=DateTime.UtcNow};PlayLayers312("gece/yagmurlu");await WaitWeather311();check(activeMotion240 is StormMotion252,"Gece gök gürültülü yağmur modeli");HideMenu();await Task.Delay(70);check(activeMotion240==null&&motionTimer240==null,"Gizlenince yeni hava efektleri durur");
 }
}
#endif
