#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
public partial class StartWindow {
 private async System.Threading.Tasks.Task VerifyAtmosphere249(){
  string report=OwnPath15("Dogrulama-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;cfg.GlassReflection249=true;cfg.ThemeGlow247=.9;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();ApplyTheme247();await System.Threading.Tasks.Task.Delay(400);UpdateLayout();
  var button=TileButtons().First();var reflection=(GlassReflection249)button.Template.FindName("reflection249",button);check(reflection.Progress>0&&!reflection.IsHitTestVisible,"Kutucuk yansıması hareketli ve tıklamayı engellemez");check(reflections249.Count>0&&reflections249.Values.Any(a=>a.Surface.Progress>0),"Yan kartlarda ve başlıkta hareketli cam yansıması");double progress=reflection.Progress;await System.Threading.Tasks.Task.Delay(200);check(reflection.Progress!=progress,"Parlama noktası sabit kalmaz");
  cfg.GlassReflection249=false;ApplyTheme247();check(reflection.Progress==0&&reflections249.Values.All(a=>a.Surface.Progress==0),"Yansıma ayarı bütün animasyonları durdurur");cfg.GlassReflection249=true;ApplyTheme247();
  cfg.WeatherBackdropAlways=true;cfg.WeatherChosen=true;cfg.WeatherCity="Deneme";NormalizeLayers312();for(int i=0;i<3;i++)cfg.WeatherLayers[i].Enabled=i==1;cfg.WeatherLayers[1].Source="auto-effect";cfg.WeatherLayers[1].Opacity=.9;
  foreach(int day in new[]{1,0}){
   weatherData220=new WeatherData220 {Code=3,Day=day,FetchedUtc=DateTime.UtcNow};string scene=WeatherSceneKey311(weatherData220);PlayLayers312(scene);await WaitWeather311();await System.Threading.Tasks.Task.Delay(150);var cloud=activeMotion240 as CloudMotion249;
   check(cloud!=null&&cloud.Night==(day==0),day==1?"Gündüz bulutlu hava hareketi seçilir":"Gece bulutları koyu tonla seçilir");check(layerImages312[1].Source==null&&layerGrids312[0].Visibility==Visibility.Collapsed&&layerGrids312[2].Visibility==Visibility.Collapsed,"Bulut hareketi tek başına ikinci katmanda çalışır");double before=cloud.Elapsed;cloud.Advance(2);check(cloud.Elapsed>before,"Bulut akışı zamanla ilerler");Capture(this,OwnPath15(day==1?"Bulut-gunduz-v2.52.png":"Bulut-gece-v2.52.png"));
  }
  weatherData220=new WeatherData220 {Code=2,Day=1,FetchedUtc=DateTime.UtcNow};PlayLayers312(WeatherSceneKey311(weatherData220));await WaitWeather311();check(activeMotion240 is CloudMotion249&&((CloudMotion249)activeMotion240).Partly,"Parçalı bulutlu havada daha seyrek bulut hareketi");
  for(int i=0;i<3;i++)cfg.WeatherLayers[i].Enabled=i==0;PlayLayers312("gunduz/kapali-bulutlu");await WaitWeather311();check(activeMotion240==null,"İkinci katman kapalıysa bulut hareketi oluşturulmaz");
  cfg.WeatherLayers[0].Enabled=false;cfg.WeatherLayers[1].Enabled=true;PlayLayers312("gece/kapali-bulutlu");await WaitWeather311();HideMenu();await System.Threading.Tasks.Task.Delay(100);check(activeMotion240==null&&motionTimer240==null,"Başlat gizlenince bulut zamanlayıcısı ve katman bırakılır");check(reflection.Progress==0&&reflections249.Values.All(a=>a.Surface.Progress==0),"Başlat gizlenince cam yansımaları durur");
  ShowResidentMenu();StopWeatherBackdrop311();string original=json.Serialize(cfg);settingsProbe=view=>{FindVisual<CheckBox>(view.Window).First(c=>Convert.ToString(c.Content)=="Cam kenarlarında hareketli ışık yansıması").IsChecked=false;view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;if(probeError!=null)throw probeError;check(json.Serialize(cfg)==original,"İptal yansıma tercihini geri alır");
  startupWriteProbe226=(enabled,command)=>{};settingsProbe=view=>{FindVisual<CheckBox>(view.Window).First(c=>Convert.ToString(c.Content)=="Cam kenarlarında hareketli ışık yansıması").IsChecked=false;view.Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;startupWriteProbe226=null;if(probeError!=null)throw probeError;check(!json.Deserialize<Config>(File.ReadAllText(FilePath)).GlassReflection249,"Yansıma tercihi kalıcı kaydedilir");
 }
}
#endif
