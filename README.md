# 🧩 TaskFlow - Modern Backend Mimarilerine Yönelik Temel DDD Uygulaması

**TaskFlow**, modern yazılım geliştirme süreçlerinde kullanılan temiz, katmanlı ve domain odaklı mimari yaklaşımların temelini dökümante etmek amacıyla geliştirilmiş bir örnek projedir.

Bu proje, karmaşık yapılara girmeden aşağıdaki konuları **en yalın haliyle** göstermeyi hedefler:


- ✅ Domain-Driven Design (DDD) yaklaşımının temel prensipleri
- ✅ Katmanlı mimari (Clean Architecture) yapısında sorumluluk ayrımı
- ✅ Entity, Value Object, Aggregate Root gibi kavramların somut örneklerle kullanımı
- ✅ Test-first yaklaşımıyla domain davranışlarının birer **iş kuralı belgesi** olarak testlerle ifade edilmesi
- ✅ Dış bağımlılıkları en aza indirerek “gerçek domain modeli”nin yalın ve sade şekilde kurulması

---

## 🎯 Amaç


TaskFlow, .NET ekosisteminde çalışan backend geliştiricilere;

- Anlaşılabilir bir DDD tabanı sunmak,
- Gerçek bir iş alanına dayanan, test edilebilir bir domain modeli örneği sağlamak

bir şablon sunma amacıyla oluşturulmuştur.

## 🚫 Ne Değildir?

- Geniş kapsamlı bir proje değildir
- Çoklu aggregate, event sourcing, advanced CQRS gibi ileri seviye konular **bilerek** kapsam dışı bırakılmıştır
- Amacı, **az şeyle çok şey göstermek**tir

---
## ⚙️ Teknolojiler ve Prensipler
- .NET 9
- MSTest ile Unit Testing
- Katmanlı yapı (Domain / Application / API)
- In-Memory kullanım, dış bağımlılık minimizasyonu
- SRP, Encapsulation, Explicit Behavior tanımı

## 📘 Ek Dokümantasyon

- [🧠 Domain Modeli Tasarımı](/docs/domain-design.md)
- [📐 Application Katmanı Tasarımı](/docs/application-design.md)
- [🌐 Infrastructure ve API Katmanı Tasarımı](/docs/infrastructure-api-design.md)