#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
public partial class StartWindow {
 private async System.Threading.Tasks.Task VerifyFeatures246() {
  string report=OwnPath15("Dogrulama-v2.52.txt");File.WriteAllText(report,"");
  Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();
  var target=cfg.Tiles.First(t=>t.Children==null);RecordFrequent246(target.Target);check(!FrequentEntries246().Any(),"Varsayılan kapalı: kullanım kaydı üretilmez");
  cfg.FrequentEnabled246=true;RecordFrequent246(target.Target);RecordFrequent246(target.Target);
  var first=FindFrequent246(target.Target);check(first!=null&&first.Count==2,"Başlat kaynaklı kayıt ve tekrar sayımı");
  first.Allowed=false;RecordFrequent246(target.Target);check(first.Count==2&&!VisibleFrequent246("").Any(),"İzinsiz öğe tekrar sayılmaz ve geri gelmez");
  var manual=new FrequentEntry246 {Item=new Tile {Name="Özel sabit klasör",Target=Root},Pinned=true};FrequentEntries246().Add(manual);
  check(VisibleFrequent246("").Single()==manual,"Elle sabitlenen öğe sıfır kullanımda da görünür");
  check(FindFrequent246(target.Target.ToUpperInvariant())==first,"Hedefler büyük/küçük harf farkıyla çoğalmaz");
  check(!VisibleFrequent246("eşleşmeyen arama").Any(),"Arama sık kullanılanları filtreler");
  string before=json.Serialize(cfg);
  featureProbe246=w=>{var tab=FindVisual<TabControl>(w).First();check(tab.Items.Count==2,"Kara Liste ve Beyaz Liste ayrı yönetim sekmeleri");var boxes=FindVisual<CheckBox>(w).ToList();check(boxes.Count==2&&boxes[0].IsChecked==true&&boxes[1].IsChecked==false,"İzinli ve İzinsiz tamamlayıcı görünür");boxes[1].IsChecked=true;boxes[1].RaiseEvent(new RoutedEventArgs(CheckBox.ClickEvent));FindButtons(w).First(b=>Convert.ToString(b.Content)=="İptal").RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};
  ManageFrequent246(true);featureProbe246=null;check(json.Serialize(cfg)==before,"İzin penceresi İptal ile değişikliği geri alır");
  first.Allowed=true;Refresh();await System.Threading.Tasks.Task.Delay(200);Capture(this,OwnPath15("Tema-sik-kullanilan-v2.52.png"));
  featureProbe246=w=>{Capture(w,OwnPath15("Sik-kullanilan-izinler-v2.52.png"));FindButtons(w).First(b=>Convert.ToString(b.Content)=="Tamam").RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};ManageFrequent246(true);featureProbe246=null;
  var saved=json.Deserialize<Config>(File.ReadAllText(FilePath));check(saved.FrequentEnabled246&&saved.Frequent246.Count==2,"Tercih ve izinler JSON kaydında korunur");
  string root=OwnPath15("media-tree");Directory.CreateDirectory(Path.Combine(root,"alt","ikinci"));File.WriteAllText(Path.Combine(root,"bir.mp3"),"");File.WriteAllText(Path.Combine(root,"alt","iki.mp4"),"");File.WriteAllText(Path.Combine(root,"alt","ikinci","uc.wmv"),"");File.WriteAllText(Path.Combine(root,"alt","metin.txt"),"");
  check(ReadMediaFolder246(root,false,CancellationToken.None).Files.Count==1,"Yalnız seçili klasör modu");
  var scan=await System.Threading.Tasks.Task.Run(()=>ReadMediaFolder246(root,true,CancellationToken.None));check(scan.Files.Count==3&&scan.Files.Any(x=>x.EndsWith("uc.wmv")),"İç içe alt klasörlerden ses ve video aynı listeye eklenir; metin dışlanır");
  var cancel=new CancellationTokenSource();cancel.Cancel();bool stopped=false;try{ReadMediaFolder246(root,true,cancel.Token);}catch(OperationCanceledException){stopped=true;}check(stopped,"Eski tarama iptal edilebilir");
  EnsureMusic241();check(music241.IncludeSubfolders246,"Alt klasörler varsayılan dahil");
  featureProbe246=w=>{Capture(w,OwnPath15("Medya-klasor-v2.52.png"));FindButtons(w).First(b=>Convert.ToString(b.Content)=="İptal").RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};check(PickMediaFolder246()==null,"Klasör seçimi iptal edilince liste eklenmez");featureProbe246=null;
 }
}
#endif
