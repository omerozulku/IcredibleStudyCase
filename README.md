# Icredible Storage Case Study

Bu proje, büyük dosyaların parçalara ayrılarak (chunk),
farklı depolama sağlayıcılarına dağıtılması ve
gerektiğinde tekrar birleştirilerek dosya bütünlüğünün
korunmasını amaçlayan bir .NET Console Application'dır.

## Amaç
Bu çalışmanın amacı, gerçek hayatta dosya yedekleme ve
dağıtık depolama sistemlerinde kullanılan temel yapı taşlarını
soyutlanmış, genişletilebilir ve test edilebilir bir mimari ile
modellemektir.

Sonuçtan çok mimari yaklaşım ve tasarım kararları
ön planda tutulmuştur.

---

## Mimari Genel Bakış

Proje aşağıdaki katmanlardan oluşmaktadır:

- **Application**
  - Use-case odaklı servisler (Upload / Download)
- **Domain**
  - Core entity’ler ve domain exception’ları
- **Infrastructure**
  - Chunking, hashing, storage provider’lar
- **Persistence**
  - SQLite + EF Core ile metadata persistence

Tüm bağımlılıklar interface tabanlı olup
IoC container üzerinden enjekte edilmektedir.

---

## Chunking Yaklaşımı

Dosyalar, boyutlarına göre dinamik olarak belirlenen
chunk size ile parçalara ayrılır.

Bu amaçla `IChunkSizeStrategy` kullanılmıştır.
Böylece chunk hesaplama algoritması değiştirilebilir hale getirilmiştir.

---

## Storage Provider Abstraction

Tüm depolama işlemleri `IStorageProvider` üzerinden yapılmaktadır.

Örnek implementasyonlar:
- FileSystemStorageProvider
- DatabaseStorageProvider

Yeni bir storage eklemek için core kodda
herhangi bir değişiklik yapılmasına gerek yoktur.

---

## Metadata Yönetimi

Her dosya ve chunk bilgisi aşağıdaki metadata ile saklanır:
- Dosya adı
- FileId
- Chunk sırası
- Storage provider bilgisi
- SHA256 checksum

Metadata kalıcı olarak SQLite veritabanında tutulur.

---

## Dosya Bütünlüğü

Dosya birleştirme sonrası SHA256 checksum hesaplanır
ve upload sırasında alınan checksum ile karşılaştırılır.

Uyuşmazlık durumunda `IntegrityException` fırlatılır.

---

## Logging

Tüm işlemler Serilog ile loglanmaktadır:
- Console output
- Günlük dönen dosya logları

Log seviyeleri `appsettings.json` üzerinden yönetilmektedir.

---

## Çalıştırma

dotnet run