# DormitoryManagementSystem

Десктопний застосунок для управління гуртожитком університету — облік мешканців, заселення/виселення, пошук резидентів, електронна черга в душ, оголошення адміністрації та звернення мешканців.

Проєкт побудований на **WPF (.NET 9)** за архітектурним патерном **MVVM**, з доступом до бази даних через **Entity Framework Core** (SQL Server).

## Можливості

- **Автентифікація** — вхід за email/паролем з розділенням ролей *адміністратор* / *резидент* (`LoginView`, `LoginViewModel`).
- **Заселення мешканців** — форма заселення нового студента: вибір факультету, кімнати та вільного місця, збереження особистих даних (`SettlementView`).
- **Виселення мешканців** — процес виселення з зазначенням причини, збереження історії виселень (`EvictionReasonDialog`, `EvictionService`, `EvictionHistory`, `EvictionNotification`).
- **Пошук резидентів** — пошук та перегляд мешканців гуртожитку адміністрацією (`ResidentSearchView`).
- **Особистий кабінет резидента** — перегляд власної кімнати та інформації про сусідів по кімнаті (`MyRoomView`, `RoomService`).
- **Інформація про резидента** — картка з деталями студента: група, курс, факультет, форма навчання, пільги тощо (`ResidentInfoView`).
- **Електронна черга в душ** — генерація часових слотів на день, бронювання/скасування місця в черзі з урахуванням статі мешканця, автоматичне очищення застарілих слотів (`ShowerReservationView`, `ShowerSlotGeneratorService`, `ShowerReservationService`, `ShowerCleanupService`).
- **Черга в душ (адмін)** — перегляд та керування чергами адміністрацією (`AdminShowerQueueView`).
- **Оголошення адміністрації** — створення й перегляд активних оголошень для мешканців (`AdminMessageView`, `AdminMessageService`).
- **Звернення мешканців** — можливість залишити звернення/скаргу (анонімно або від свого імені) та перегляд їх адміністрацією (`ContactView`, `AdminContactsView`, `ContactService`).
- **Перевірка доступності серверів** — сервіс пінгування зовнішніх API (`PingService`, `IApiService`, `ZtuApiService`, `MyApiService`) — закладена інтеграція із зовнішнім студентським API університету та власним синхронізаційним API.

## Технологічний стек

| Компонент | Технологія |
|---|---|
| UI-фреймворк | WPF (.NET 9, `net9.0-windows`) |
| Архітектура | MVVM (`CommunityToolkit.Mvvm` 8.4.0) |
| Доступ до даних | Entity Framework Core 9.0.3 (`Microsoft.EntityFrameworkCore.SqlServer`) |
| СУБД | Microsoft SQL Server (LocalDB / SQLEXPRESS) |
| Мова | C# (Nullable enabled) |

## Структура проєкту

```
DormitoryManagementSystem/
├── DormitoryManagementSystem.sln
└── DormitoryManagementSystem/
    ├── Converters/       # IValueConverter'и для XAML-байндингів
    ├── DTO/               # Об'єкти передачі даних (кімнати, слоти душу, черга)
    ├── Migrations/        # Міграції Entity Framework Core
    ├── Models/            # Сутності бази даних (User, Room, ShowerSlot, EvictionHistory тощо)
    ├── Services/          # Бізнес-логіка та доступ до даних (DatabaseContext, RoomService, EvictionService...)
    ├── Styles/             # Спільні стилі XAML (кнопки тощо)
    ├── ViewModels/         # ViewModel'і для кожного екрана (MVVM)
    ├── Views/              # XAML-екрани застосунку
    ├── App.xaml            # Точка входу та глобальні ресурси
    └── MainWindow.xaml     # Головне вікно з навігацією між екранами
```

## Модель даних

Основні сутності: `UserInfo` (резиденти й адміністратори), `Faculty`, `Dormitory`, `Room`, `RoomPlace` (1:1 зв'язок із мешканцем), `AdminMessage`, `ContactMessage`, `ShowerSlot`/`ShowerReservation`, `EvictionNotification`/`EvictionHistory`.

При першому запуску (`DatabaseContext.OnModelCreating`) у базу засіваються тестові дані: 6 факультетів, гуртожиток №1, кілька кімнат із місцями, тестовий адміністратор і кілька тестових резидентів.

## Запуск проєкту

### Вимоги

- Windows
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server (наприклад, SQL Server Express, `localhost\SQLEXPRESS`) або LocalDB

### Кроки

1. Клонуйте репозиторій:
   ```bash
   git clone https://github.com/coutAndrii16/DormitoryManagementSystem.git
   cd DormitoryManagementSystem
   ```
2. За потреби змініть рядок підключення до бази даних у `Services/DatabaseContext.cs` (`OnConfiguring`), вказавши власний сервер SQL Server.
3. Застосуйте міграції для створення бази даних:
   ```bash
   dotnet ef database update --project DormitoryManagementSystem
   ```
4. Зберіть та запустіть застосунок:
   ```bash
   dotnet run --project DormitoryManagementSystem
   ```
---