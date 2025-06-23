# Application Katmanı Tasarımı (Application Layer Design)

Bu doküman, **TaskFlow** uygulamasının *Application Katmanı* tasarımını, sorumluluklarını, kullandığı yapıları ve test stratejilerini açıklar.

---

## 🔐 Amaç

Application katmanı, domain kurallarını dış dünya (UI, API, veri tabanı vb.) ile bağlantıya sokmadan şekilde yöneten bir ölçeklenebilir iş mantığı sunar.

* Use case'ler burada tanımlanır (Task oluştur, sil, güncelle, listele vb.)
* CQRS prensibine uygun olarak **Command** ve **Query** sınıflarına ayrılır
* Ölçeklenebilir, test edilebilir ve DI (Dependency Injection) ile yönetilir

---

## ✍️ Temel Yapılar

| Bileşen                                    | Amaç                                                               |
| ------------------------------------------ | ------------------------------------------------------------------ |
| `ICommand<T>`                              | Veri değişikliği yapan istekleri temsil eder                       |
| `ICommandHandler<T, TResult>`              | Komutları işleyen sınıftır                                         |
| `IQuery<TResult>`                          | Veri okuma isteklerini temsil eder                                 |
| `IQueryHandler<TQuery, TResult>`           | Sorguları işleyen sınıftır                                         |
| `ICommandDispatcher` ve `IQueryDispatcher` | Handler'lara erişim için kullanılan interfaceler                    |
| `PipelineBehavior`                         | Cross-cutting concern (Validation, Logging vb.) yapılarını yönetir |

---

## ⚡ CQRS Mimarisi

* **Command**: Yazma işlemleri (Create, Update, Delete)
* **Query**: Okuma işlemleri (Get, List)
* Bu yapılar **Dispatcher** sınıfları ile handler'lara iletilir
* Örnek: `CommandDispatcher.Dispatch(CreateTaskCommand)`

---

## ✏️ Pipeline Behaviors

### ValidationBehavior

* FluentValidation ile bağlı tüm kurallar otomatik olarak çalıştırılır
* Komutlar handler'a iletilmeden önce doğrulanır

### LoggingBehavior

* Komut/sorgu başladığında ve tamamlandığında log yazar
* Hata durumunda log hatayla birlikte tutulur

---

## 📆 Bağlımlılıklar

| Bağlımlılık                                | Amaç                                |
| ------------------------------------------ | ----------------------------------- |
| `FluentValidation`                         | Komutlar için kurallar tanımlanması |
| `Microsoft.Extensions.DependencyInjection` | DI Container kayıtları              |
| `ILogger`                                  | Logging altyapısı                   |
| `Moq`, `MSTest`                            | Unit testler için                   |

---

## ✅ Test Stratejisi

Tüm handler'lar şu şekilde test edilmiştir:

* `CommandHandler` testlerinde repository mock'lanarak başarı ve hata senaryoları test edilmiştir
* `QueryHandler` testlerinde veri var/yok durumları ayrı ayrı kapsanmıştır
* Domain doğrulamaları ayrıca domain testlerinde kapsanır

---

## 📁 Servis Kayıtları (DI)

`ServiceCollectionExtensions.cs` sınıfında tüm yapılar kayıt edilir:

```csharp
services.AddScoped<ICommandDispatcher, CommandDispatcher>();
services.AddScoped<IQueryDispatcher, QueryDispatcher>();
services.AddScoped<ICommandHandler<CreateTaskCommand, Guid>, CreateTaskCommandHandler>();
services.AddScoped<IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>, GetTasksQueryHandler>();
// ...
```

---


