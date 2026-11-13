using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
public partial class StartWindow {
 private Func<string,Task<List<WeatherPlace220>>> placeSearchProbe250;
 private Action WirePlaceSearch250(Window window,TextBox query,ListBox list,TextBlock status,Button button){
  int generation=0;bool closed=false,busy=false,pending=false;string running="";
  var timer=new DispatcherTimer {Interval=TimeSpan.FromMilliseconds(500)};
  Action invalidate=()=>{generation++;pending=false;timer.Stop();list.ItemsSource=null;list.Visibility=Visibility.Collapsed;};
  Func<Task> run=null;
  run=async()=>{
   timer.Stop();if(closed||!window.IsVisible)return;string input=query.Text.Trim();
   if(input.Length<2){invalidate();status.Text="En az iki harf yazın.";return;}
   if(busy){if(input!=running)pending=true;return;}
   int request=generation;busy=true;running=input;status.Text="Aranıyor…";
   try {
    var places=placeSearchProbe250==null?await Task.Run(()=>SearchPlaces220(input)):await placeSearchProbe250(input);
    if(closed||!window.IsVisible||request!=generation||query.Text.Trim()!=input)return;
    list.ItemsSource=places;list.Visibility=Visibility.Visible;status.Text=places.Count==0?"Şehir bulunamadı.":"Listeden şehri seçin; Kaydet ile uygulanır.";
   }catch(Exception error){if(!closed&&window.IsVisible&&request==generation){WeatherLog220(error);status.Text="Şehir listesi alınamadı. Enter ile yeniden deneyebilirsiniz.";}}
   finally{busy=false;running="";if(pending&&!closed){pending=false;window.Dispatcher.BeginInvoke(new Action(async()=>await run()));}}
  };
  timer.Tick+=async(s,e)=>await run();
  query.TextChanged+=(s,e)=>{invalidate();if(query.Text.Trim().Length>=2){status.Text="Aramak için yazın veya Enter’a basın…";timer.Start();}else status.Text="En az iki harf yazın.";};
  query.PreviewKeyDown+=async(s,e)=>{if(e.Key==Key.Enter){e.Handled=true;await run();}};
  button.Click+=async(s,e)=>await run();
  window.Closed+=(s,e)=>{closed=true;invalidate();};
  query.ToolTip="Şehir yazın: otomatik arama · Enter: hemen ara";
  return invalidate;
 }
 private void AddReflectionSpeed250(StackPanel panel,Action preview){
  double speed=GlassReflection249.SafeSpeed250(cfg.GlassSpeed250);
  var label=new TextBlock {Text="Cam yansıması hızı",Margin=new Thickness(4,10,4,4)};panel.Children.Add(label);
  var row=new DockPanel {Margin=new Thickness(4)};
  var number=new TextBox {Tag="reflection-speed-number250",Width=75,Padding=new Thickness(7),Text=speed.ToString("0.##",CultureInfo.CurrentCulture)};DockPanel.SetDock(number,Dock.Right);row.Children.Add(number);
  var slider=new Slider {Tag="reflection-speed250",Minimum=.25,Maximum=3,SmallChange=.05,LargeChange=.25,Value=speed,Margin=new Thickness(0,0,12,0)};row.Children.Add(slider);panel.Children.Add(row);
  bool updating=false;Action caption=()=>label.Text="Cam yansıması hızı: "+slider.Value.ToString("0.##",CultureInfo.CurrentCulture)+"× · bir tur "+(14/slider.Value).ToString("0.#",CultureInfo.CurrentCulture)+" saniye";caption();
  slider.ValueChanged+=(s,e)=>{cfg.GlassSpeed250=slider.Value;caption();if(!updating){updating=true;number.Text=slider.Value.ToString("0.##",CultureInfo.CurrentCulture);updating=false;}ApplyTheme247();preview();};
  number.TextChanged+=(s,e)=>{if(updating)return;double value;if(double.TryParse(number.Text.Replace(',','.'),NumberStyles.Float,CultureInfo.InvariantCulture,out value)&&!double.IsNaN(value)&&!double.IsInfinity(value)&&value>=.25&&value<=3){number.BorderBrush=new SolidColorBrush(Color.FromRgb(65,180,220));updating=true;slider.Value=value;updating=false;}else number.BorderBrush=Brushes.OrangeRed;};
 }
}
