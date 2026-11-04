# Başlat Cam v2.34

- Uygulamanın kendi WPF kaydırma çubuklarında fare üzerine geldiğinde ray ve tutamak camgöbeği belirginleşir. Çubuk genişliği değişmez.
- Ortak düğmelerde vurgu ayrı, tıklamayı engellemeyen katmandadır. Hover sırasında kenarlık/yazı kalınlığı veya yerleşim ölçüsü değiştirilmez. Resmi al düğmesinin metaveri etiketi artık renk olarak değerlendirilmez; saydam arka planın hit test alanını kaybettirmesi önlenir.
- Sağ tık > İkon ve Resim seçimi: Resmin kutucuğa yerleşimi altında Doldur veya Tam sığdır; ortala, sol, sağ, üst, alt hizalama seçilebilir. Tam sığdır resmin tamamını isim bandının üzerinde gösterir. Seçim kendi resimlerine, elle yakalanan ve otomatik alınan resimlere uygulanır. Eski kutucuklar mevcut Doldur davranışını korur.
- Ayarlar > Kutucuklar: İkon gölge genişliği 0–32 px, varsayılan 10. 0 kapatır. Yalnız ikon ve yedek ikon simgesine uygulanır; isim yazısı ve kart ölçüsü etkilenmez. Canlı düzenlenir; Kaydet kalıcı saklar, İptal geri alır.

219 kontrollü doğrulama geçti. Yerleşim matematiği, sabit düğme geometrisi, kaydırma hover tetikleyicisi, resim ayarlarının Kaydet/İptal davranışı ve gölge kapsamı doğrulandı. Geniş resmin tümünü gösteren diyalog önizlemesi görsel olarak incelendi. Fiziksel fare hareketiyle uzun kullanım denemesi kullanıcı oturumunda yapılmalıdır. Gerçek Store oyunundan PrintWindow yakalaması ve Windows yeniden başlatma denemesi halen bekliyor. Avast ayarlarına müdahale edilmedi; tespit sorununun çözüldüğü iddia edilmez.

Önce eski uygulamada Ayarlar > Uygulamadan çık seçin, ardından yeni EXE'yi açın. Veri klasörü %LOCALAPPDATA%\başlat\V2.34; önceki ayarlar aktarılır, eski klasörler korunur.

Geliştirici: Mustafa ARSLAN. Kodlama/görsel desteği: OpenAI Codex / ImageGen.
