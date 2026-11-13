# Başlat Cam v2.52 kaynak projesi

Geliştirici: Mustafa ARSLAN. Kodlama/görsel desteği: OpenAI Codex / ImageGen.

Bu paket yayımlanan v2.52 C# kaynağını değiştirmeden içerir. Arayüz BaslatCam.cs içinde oluşturulur. Hava hareketleri WeatherMotion.cs içindedir. 31 hava görseli EmbeddedResource olarak eklenir. Visual Studio proje dosyası paketleme için sonradan oluşturulmuştur; önceki sürüm doğrudan csc.exe ile derlenmiştir.

Windows üzerinde Visual Studio ve .NET Framework 4.8 Developer Pack ile BaslatCam.csproj dosyasını açıp derleyin. Alternatif: PowerShell içinde ./build.ps1; bu yol Windows .NET Framework derleyicisini kullanır, Python veya NuGet gerektirmez. Çıktı bin/Portable/BaslatCam-v2.52.exe olur. Derleme betiği uygulamayı çalıştırmaz.

Harici/özel DLL yoktur; başvurular Windows .NET Framework kitaplıklarıdır. Kişisel duzen.json, uygulama önbelleği, konum ve kullanıcı verileri pakete eklenmemiştir. Çalışırken kullanıcı verileri %LOCALAPPDATA%/başlat/V2.52 altında oluşur. Hava durumu gerçek veri için dış servis kullanır. Avast tespiti incelenmektedir; bu paket bir güvenlik düzeltmesi içermez.

DIAGNOSTICS ve FAST221 derleme sembolleri kontrollü test kodlarını etkinleştirir; normal projede kapalıdır. docs klasöründe önceki sürümün kullanım notları ve görsel numaraları bulunur. Derleme sonrası farklı proje/derleyici seçenekleri nedeniyle EXE hash'inin yayımlanan dosyayla aynı olması beklenmez; kaynak hash'i SHA256SUMS.txt ile doğrulanabilir.

## Hava hareketleri

Ayarlar → Hava Durumu → Katmanlar bölümünde 2. katmanı açın. Otomatik efekt açık gündüzde güneşi, açık gecede ayı, yağışta yağmur veya karı gösterir. Dört efekti 2. katmanda elle de seçebilirsiniz. Yalnız 2. katman açıksa hareket mevcut arka planın üzerinde gösterilir. İlgili hava manzarası 1. katmanda açıksa güneş ve ay dağın arkasından yükselir.

Süreli görünümde yükseliş seçilen süreye uyarlanır. Sürekli görünümde yükseliş 12 saniyede tamamlanır ve son konumda kalır. Başlat gizlenince animasyon durur.



## v2.52 kullanımı

Yan paneldeki Müzikçalar kartında Klasör seç ile seçilen klasördeki ses ve video dosyalarından bir liste oluşturulur. Listeler düğmesinden boş liste oluşturabilir, isim verebilir ve birden fazla dosya ekleyebilirsiniz. Listeler bu sürümün muzik.json dosyasında saklanır. Oynat düğmesine basılınca çalar. Görüntü düğmesi büyük oynatıcıyı öne getirir. Videonun görüntüsü yalnız bu pencere açıkken çizilir. Kullanılabilir dosya biçimleri Windows medya desteğine bağlıdır. Klasör seçimi doğrudan o klasördeki dosyaları okur.

Uygulama kutucuklarında ve Tüm uygulamalar listesindeki satırlarda Konumunu aç eylemi sağ tık menüsündedir. Masaüstü uygulamasının dosyası veya kısayolu Explorer içinde seçilir. Store uygulamalarında Windows Uygulamalar klasörü açılır.

Windows açılışında başlat seçeneği işaretliyken kullanıcı oturumu gizli hazırlanır. Windows tuşuna veya Başlat düğmesine basınca tam ekran açılır. Başlangıç entegrasyonu hata verirse en fazla 10 kez üç saniyelik aralıkla yeniden denenir. baslangic-durum.txt son denemenin sonucunu içerir. Gerçek yeniden başlatma kullanıcı oturumunda doğrulanmalıdır.

Sessiz WAV ile oynatma, duraklatma, devam etme, liste kaydı, eksik dosya, modeless görüntü kapatma, Explorer seçim komutu ve gizli başlangıç akışı doğrulandı.


## v2.52 kullanımı

GPU göstergesi Windows GPU Engine performans sayaçlarından ölçülür. Her fiziksel GPU motoru için süreç kullanımları toplanır ve en yoğun motorun yüzdesi gösterilir. İlk örnek hazırlanırken beklenir. Sistem veya ekran sürücüsü sayaç sağlamazsa durum açıklaması gösterilir.

Ayarlar → Yan panel altında Kart sırası listesinden bir kart seçip Yukarı / Aşağı ile taşıyın. Panel canlı değişir. Kaydet sıralamayı saklar, İptal önceki sırayı geri getirir.

Müzikçalar → Görüntü yeni medya ekranını açar. Video ve mevcut arka plan saydam yüzeyin arkasındadır. Sağ üstte Klasör seç, ortada sanatçı ve şarkı adı ile oynatma kontrolleri, altında geçen süre/ilerleme/toplam süre ve ses/karışık/tekrar/liste/tam ekran seçenekleri vardır. Sanatçı ve başlık dosyanın yerel Windows medya etiketlerinden okunur. Etiket yoksa dosya adı ve Yerel medya gösterilir.

Tekrar düğmesi Kapalı → Liste → Tek parça şeklinde değişir. Karışık çalmada mevcut parça hemen yeniden seçilmez. Tekrar kapalıyken liste bitince durur. Tam ekranda 2,5 saniye hareketsizlik sonunda yönetim yüzeyi ve imleç gizlenir. Fare hareketiyle geri gelir. Liste açıkken veya kaydırıcı tutulurken kontroller görünür kalır. F11 tam ekranı değiştirir. Esc tam ekrandan pencereye döner. Boşluk çalar/duraklatır.

GPU yaklaşımı için Microsoft açıklaması
https://devblogs.microsoft.com/directx/gpus-in-the-task-manager/
PDH sayaç API
https://learn.microsoft.com/en-us/windows/win32/api/pdh/nf-pdh-pdhaddenglishcounterw


## v2.52 görünüm düzeltmesi
Yan bar müzikçaları büyük oynatıcıyla uyumlu hale getirildi. Sanatçı/başlık ortalı, çalma listesi koyu ve yuvarlak, oynatma düğmesi turkuaz ve yuvarlak, ilerleme ve ses çubukları yumuşak uçludur. Konumunu aç yalnız sağ tık menüsünde görünür.


## v2.52 sade medya tasarımı

Yan bar kartı226 DIP yüksekliğe indirildi. Üstte parça adı ve klasör simgesi, kırmızı ses dalgası, ilerleme, ses/önceki/çal-duraklat/sonraki/liste simgeleri ve altta karıştır/tekrar/görüntü simgeleri bulunur. Liste düğmesi yanda açılır. Parçaya çift tıklayarak çalabilirsiniz. Klasör ve liste düzenleme bu yan alandan da erişilebilir.

Ses ikonuna gelince dikey ses çubuğu açılır. İkona tıklamak uygulamanın sesini kapatıp açar. Çubuğu değiştirmek sesi açar.

Görüntü artık Başlat'tan bağımsız yüzen bir penceredir. Başlat gizlendiğinde açık kalır, Başlat yeniden açıldığında yeni bir görüntü penceresi oluşmaz. Görüntü penceresi odaktayken Esc veya kendi çarpısı yalnız görüntüyü kapatır. Uygulamadan tamamen çıkmak oynatmayı da sonlandırır. F11 ve ekran simgesi tam ekran geçişini sağlar. Tam ekranda2,5 saniye sonra başlık ve kontroller gizlenir, fareyle geri gelir.

Ses dosyasında yüzen pencere330 DIP yüksekliğinde tutulur. Video için görüntü alanı büyür. Cam saydamlığı sürer.

Kırmızı dalga, yalnız seçilen yerel dosyanın sesinden Windows Media Foundation ile üretilir. Yeni parçada önceki analiz iptal edilir. Analiz arka planda çalışır, en fazla5 saniyelik çalışma bütçesi vardır. Büyük dosyalarda dalganın bir bölümü eksik kalabilir. Ses bulunmayan/okunamayan dosyada düz çizgi gösterilir.

Microsoft Source Reader açıklaması
https://learn.microsoft.com/en-us/windows/win32/medfound/processing-media-data-with-the-source-reader


## v2.52 liste adları ve kompakt kontroller
Çalma listesi açılır penceresinde yazı tipinin simge düğmesinden miras alınması düzeltildi. Liste adları, dosya adları ve metin düğmeleri Segoe UI ile gösterilir. Seçili liste adı koyu zeminde beyazdır.

Karıştır, tekrar ve görüntü/tam ekran simgeleri küçültülerek ileri düğmesinin soluna, aynı kontrol satırına taşındı. Yan kart198 DIP, ses için yüzen pencere300 DIP yüksekliğinde tutulur.


## 11 Kasım 2026 — v2.52
- Temada içerik ve kullanıcı yerleşimi korunarak saydam mavi grup yüzeyleri, turkuaz kenarlar ve cam arama alanı eklendi.
- Ayarlar > Kutucuklar > Sık kullanılanlar: varsayılan kapalı. Yalnız Başlat Cam’dan başarıyla açılan öğeler sayılır; tüm bilgisayar etkinliği izlenmez.
- Kara Liste ve Beyaz Liste yönetim sekmeleridir. İzinli/İzinsiz birbirini tamamlar. Sağ tık > Listeden çıkar öğeyi İzinsiz tutar; Sabitle bölgede tutar. Ekle ile dosya, klasör, Store hedefi ve özel simge eklenebilir.
- Medya klasörü seçiminde tüm alt klasörler varsayılan dahil. İşaret kaldırılırsa yalnız seçilen klasör taranır. Ses ve video birlikte listelenir; erişilemeyen alt klasörler bildirilerek atlanır, bağlantı/junction dizinlerine girilmez. Tarama arka planda ve iptal edilebilir.
- Yeni özellikler için 15 doğrulama geçti. Önceki kompakt medya / Türkçe liste / yüzen pencere kontrolleri de geçti.


## 11 Kasım 2026 — v2.52 tema ve kutucuk biçimleri
- Görünüm > Tema ve kutucuk biçimi: Saydam Cam, Parlak Mavi, Hibrit. Sert köşeli, yuvarlatılmış, içbükey ve düşünce bulutu şekilleri bağımsız seçilir.
- Kavis/içbükeylik, mavi ışık, dalga çizgisi ve isteğe bağlı yavaş hareket; normal/üzerine gelme/basılı örnekleri ve ana ekranda canlı önizleme. Kaydet kalıcı, İptal/Esc/× önceki temaya döner.
- Yeni vektör biçimler resimleri de kırpar; dış kutucuk boyutu ve grup düzeni korunur. Güneş/hava varlıkları ve içerikler değiştirilmedi. Saydamlık mevcut kutucuk opaklığından ayarlanır.
- Hareket varsayılan kapalı. Açılırsa 15 kare/saniye hedefli vektör çizimi; görünmezken ve yüksek karşıtlıkta durur. Yeni bitmap/DLL eklenmedi.
- Dört şeklin geometrisi ve ölçüsü, canlı değişim, kaydet/iptal ve animasyon aç/kapat doğrulamaları geçti. Sık kullanılanlar/alt klasör ve kompakt medya regresyonları da geçti.


## 11 Kasım 2026 — v2.52 üç düzeltme
- Görünüm > Tema ve kutucuk biçimi > Kutucuk gruplarının arka yüzeyini ve çerçevesini göster: bölümlerin arka kutusu açılıp kapatılabilir. Başlıklar ve grup düzeni korunur. Canlı uygulanır; Kaydet kalıcı, İptal geri alır.
- Şekle göre verilen genel içerik boşluğu kaldırıldı. İkonlar ve resimler normal ölçülerini korur; biçimin dışında kalan bölüm kırpılır. Okunabilirlik için yalnız yazı/isim bandı içeri alınır.
- Yan oynatıcı: önceki, oynat/duraklat, sonraki, karıştır, tekrar, görüntü. Yüzen oynatıcıda son seçenek tam ekran. Ses ve liste ayrı sütunlarda tutularak kontrollerin çakışması önlendi.
- İkon ve resim ölçüleri, kırpma, grup kutusu canlı değişimi/kaydet/iptal, iki oynatıcı sırası ve çakışma kontrolleri geçti. MP3, Türkçe liste, bağımsız pencere, dikey ses/mute, tam ekran/Esc regresyonları geçti.


## 11 Kasım 2026 — v2.52 hareketli cam yansıması ve bulut
- Görünüm > Cam kenarlarında hareketli ışık yansıması: kutucuk biçiminde, sistem kartlarında, arama ve hava başlığında kenar boyunca kayan ışık. 14 saniyelik döngü, 15 kare/sn hedefi. Mavi ışık belirginliği mevcut ayardan gelir; dalga hareketinden bağımsızdır. İptal geri alır, Kaydet kalıcıdır.
- Gündüz/gece kapalı ve parçalı bulutlu hava ikinci katmanda hareketlidir. Paket içindeki bulut PNG’si farklı ölçek/hız/yönlerde yeniden kullanılır. Gece koyu mavi-gri, parçalı bulutlu daha seyrektir. Yeni görsel veya DLL yoktur.
- Hava Durumu > Katman 2 etkin > Otomatik efekt seçimi. Birinci ve üçüncü katman kapalıyken çalışır. Katman belirginliği ve mevcut gösterim süresi/daima göster tercihi korunur. Deneme ekranında bulut PNG’sinin Başlat üzerinde denemesi de hareketlidir.
- Başlat gizlenince veya küçültülünce yeni animasyonlar durur. Gizleme, kapatma, bağımsız katman, gün/gece seçimi, yansıma hareketi ve kaydet/iptal kontrolleri geçti. Mevcut yağmur/kar/güneş/ay ve v2.48 ikon/oynatıcı kontrolleri geçti.


## 11 Kasım 2026 — v2.52 hız, canlı konum araması ve ses dalgası
- Görünüm > Cam yansıması hızı: 0,25×–3×. Kaydırıcı ve elle yazılabilen sayı alanı (nokta/virgül destekli). Varsayılan 1× = 14 saniyelik tur. Hız değişirken yansıma başa sıçramaz. Bütün kenar yansımalarına canlı uygulanır, Kaydet kalıcı/İptal geri alır.
- Konum alanında iki harften sonra 500 ms beklemeli otomatik şehir araması. Enter hemen aratır, Şehir ara düğmesi de korunur. Aynı pencerede tek istek yürütülür; yeni sorgu geldiğinde eski yanıt atılır. Pencere kapatılırsa sonuç işlenmez. Ana Hava Durumu sekmesi ve eski konum penceresi aynı davranışı kullanır.
- Yan kart ve yüzen medya penceresinin ortak ses dalgası mavi-turkuaz gradyana çevrildi; oynatılan bölüm daha açık turkuaz.
- Hız ve kaydet/iptal, otomatik arama/Enter/düğme, eski yanıt sırası ve kapanış, dalga renk kontrolleri geçti. Konum arama akışı kontrollü sağlayıcı yanıtlarıyla sınandı; mevcut Open-Meteo isteği korunur. Bulut/yansıma ve önceki ikon/oynatıcı kontrolleri de geçti.


## 11 Kasım 2026 — v2.52 yan panel yansıması
- Yan sistem kartlarının yansıması ortak dış Adorner katmanından çıkarılıp her kartın mevcut Grid yüzeyine üst çizim olarak alındı. Böylece yükleme/yeniden kurma, kaydırma ve kapat/aç sırasında kartın kendi yaşam döngüsünü izler.
- Efekt tıklamaları engellemez; kart ölçüsüne uyar. Görünümdeki yansıma anahtarı, parlaklık ve hız ayarı korunur.
- 360/500 DIP panelde gerçek kenar piksel değişimi ve hareket; panel kapat/aç, ayar Kaydet/İptal, gizle/yeniden aç ve hız koruma kontrolleri geçti. v2.50 hız, şehir arama ve dalga kontrolleri de geçti.


## 14 Kasım 2026 — v2.52 yoğun yağmur, şimşek, rüzgâr ve kar
- Yağmurda kayan damlalar 8’den 20’ye, camdaki sabit damlacık üst sınırı 320’den 900’e çıktı. Boyut ve beyaz yansıma artırıldı; mevcut kenar su akışları korundu.
- Gök gürültülü yağmur için yağmurla birlikte kısa çift şimşek çakması eklendi. Paketteki gerçekçi şimşek PNG’si kullanılır; bu sürümde gök gürültüsü sesi yoktur.
- Rüzgâr için farklı ölçek, renk, hız ve dönüşle 48 savrulan yaprak. Mevcut arka planı değiştirmez; Katman 2’den Uçuşan yapraklar seçilebilir.
- Kar tanesi sayısı 100’den 280’e çıktı. Küçük uzak ve iri yakın taneler, farklı hızlar ve bazı iri tanelerde kristal kolları bulunur.
- Efektler Katman 2’de ana manzaradan bağımsızdır. Otomatik seçim ve Başlat üzerinde deneme hareketi desteklenir. Katman belirginliği/gösterim süresi tercihleri korunur; gizlenince durur.
- Yeni efekt seçimi, gündüz/gece, ikinci katman, önizleme ve gizleme kontrolleri geçti. Önceki yağmur/kar/güneş/ay ve yan panel yansıması kontrolleri geçti. Yeni görsel/DLL eklenmedi.
