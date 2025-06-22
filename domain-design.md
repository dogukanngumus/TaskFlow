# Domain Modeli Tasarımı: TaskItem

## 🎯 Stratejik Tasarım Kararları (Big Picture DDD)

Bu projede **Domain-Driven Design** (DDD) prensiplerini stratejik düzeyde uygularken, aşağıdaki temel kararlara vardık:


### 1. `TaskItem` bir **Aggregate Root** olarak tanımlandı.
- Çünkü iş davranışı, yaşam döngüsü, validasyon ve business rule'lar doğrudan görevler üzerinde dönüyor.
- Kullanıcı (`User`) gibi yapılar henüz bu proje kapsamında business yönünden anlam taşımadığı için, Aggregate kapsamı dışında bırakıldı.
- Böylece **transaction boundary** ve **invariant enforcement** tekil bir domain modeli üzerinde daha net uygulanabildi.

### 2. Amacımız bir “**Rich Domain Model**” inşa etmekti.

- Görev sadece bir veri yapısı (anemic model) değil, kendi kurallarını ve geçişlerini kapsayan bir **canlı domain objesi** olarak modellenmiştir.
- Bu sayede hem dışarıdan gelen hatalı davranışlara karşı dayanıklıdır, hem de yüksek test edilebilirlik sunar.

---

## 🧩 Taktiksel Tasarım Kararları (Entity, VO, Exception, etc.)

### ✅ `Title` neden bir **Value Object**?

```csharp
public class Title
{
    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.");
        if (value.Length > 100)
            throw new ArgumentException("Title is too long.");
        Value = value;
    }

    public string Value { get; }
}
```
* Kimliği yoktur, sadece içeriği önemlidir.
* Domain’e dair kuralları içerir (boş olamaz, çok uzun olamaz).
* İleride zenginleştirme (örneğin HTML support, slug üretimi, localize title vs.) ihtimali yüksektir.
* Bu sebeple string olarak değil, özel bir **Value Object** olarak tanımlanmıştır.

Bu yaklaşım sayesinde, her başlık bir `new Title(...)` ile geçerli olmak zorundadır.
Invalid veri sistemin derinlerine giremeden durdurulur.


### ❌ `Description` neden **Value Object** değil ?

* Şu an için business açısından anlamlı bir kurala sahip değil (boş olabilir, uzun olabilir, içerik özgürdür)
* Eğer ileride minimum/maximum uzunluk, özel formatlar (Markdown, Rich Text) gibi ihtiyaçlar doğarsa, bir Value Object'e dönüştürülebilir.

### ⚠️ `DueDate` neden şimdilik **Value Object** değil ?

* Şu anda yalnızca “geçmişte olamaz” kuralı uygulanıyor.
* Eğer ileride aşağıdaki gibi ihtiyaçlar ortaya çıkarsa, bir `DueDate` **Value Object** olarak tasarlanabilir:
    * Zaman dilimi farklılıkları
    * İş günü/hafta sonu kontrolleri
    * Çalışma saatleri içi kısıtlamalar
    * Takvim entegrasyonları

Basit validasyon ihtiyacı nedeniyle şimdilik DateTime olarak bırakıldı.


### 🧱  Neden özel Domain Exception sınıfları kullanıldı ?
Her domain kuralının taşıdığı bir anlam vardır.

Bu anlamları basit `throw new Exception("...")` mesajlarıyla ifade etmek yetersiz olurdu.

Bunun yerine:

```csharp
throw new InvalidDueDateException("Due date cannot be in the past.");
```

şeklinde özel **DomainException** sınıfları ile:

* Domain kuralları daha anlamlı ve okunabilir hale geldi,
* Testlerde beklenen hata türü doğrudan kontrol edilebilir oldu,
* Hatalar context-aware hale geldi (global exception handler'lar için faydalı).

### 🔁  Statü değişimi neden `Complete()` , `MoveToInProgress()` gibi metotlarla çağırılıyor? 

`task.Status = TaskStatus.Done;` gibi doğrudan atamalar yerine şu tarz davranışlar tanımlandı:

```csharp
task.MoveToInProgress();
task.Complete();
```
Bu sayede:

* İzin verilmeyen statü geçişleri engellendi,
* Davranışlar açık şekilde adlandırıldı, kodun niyeti netleşti,
* Domain kuralları davranışlara gömüldü (örneğin `Done → InProgress` geçişi yasak olabilir).

### 🧪  Test Odaklı Domain Tasarımı
Domain modeli, test-first yaklaşımıyla geliştirildi.

Her davranış bir testle belgelenmiş, kurallar test üzerinden somutlaştırılmıştır.

| 🧪 Test Adı                                                              | 🎯 Açıklama                                                                 |
|--------------------------------------------------------------------------|-----------------------------------------------------------------------------|
| `Create_ShouldInitializeWithToDoStatus`                                  | Yeni bir görev oluşturulduğunda varsayılan statü `ToDo` olmalıdır.         |
| `Create_ShouldThrow_WhenDueDateIsInPast`                                 | Geçmiş bir tarih verildiğinde `InvalidDueDateException` fırlatılmalıdır.   |
| `Create_ShouldThrow_WhenTitleIsEmpty`                                    | Boş bir başlık verildiğinde `ArgumentException` fırlatılmalıdır.           |
| `MoveToInProgress_ShouldChangeStatus`                                    | `ToDo` durumundaki görev `InProgress` statüsüne geçebilmelidir.            |
| `Complete_ShouldChangeStatusToDone`                                      | `ToDo` veya `InProgress` durumundaki görev tamamlandığında `Done` olmalıdır.|
| `MoveToInProgress_ShouldThrow_IfAlreadyDone`                             | `Done` durumundaki görev tekrar `InProgress` durumuna geçirilememelidir.   |
| `Update_ShouldModifyTitleAndDescriptionAndDueDate`                       | Görevin başlığı, açıklaması ve son teslim tarihi güncellenebilmelidir.     |


### 📌 Sonuç:
Bu domain tasarımı sayesinde:

* Domain kuralları açık, test edilebilir ve korunaklı hale getirildi.
* Uygulama katmanları için sağlam bir temel oluşturuldu.
* Gereksiz bağımlılıklar engellendi, yalın ama genişletilebilir bir model kuruldu.
* Projede modern .NET uygulamaları için örnek teşkil edecek bir domain katmanı inşa edildi.
