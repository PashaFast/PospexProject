# PospexProject

Как развернуть приложение на локальной машине: 

1) Скачать архив по ссылке: https://github.com/PashaFast/PospexProject
2) Если на локальной машине отсутствует .NET 5 Windows Hosting Bundle, то необходимо скачать его по ссылке и установить: 
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-5.0.17-windows-hosting-bundle-installer
3) В приложении используется локальный MS SQL Server "(localdb)\mssqllocaldb" для Windows.
Строка подключения к базе находится в appsettings.json => "ConnectionStrings".
При первом запуске приложения база данных создается автоматически.
Также происходит инициализация базы данных ролями и пользователями:
2 роли: "admin", "user".
Для удобства тестирования было создано 2 пользователя: 
1) username  – "admin",  password  – "Admin-1111", роль – "admin".
2) username  – "user", Password  – "User-1111", роль - "user".
