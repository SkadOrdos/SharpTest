Тестовий HTTP сервіс для збереження та отримання набору чисел. Може містити помилки =)

Опис
Застосунок являє собою веб-сервіс, який обробляє HTTP GET/POST запити для отримання та оновлення списку чисел.

Як сховище даних допускається використання файлу на диску (без використання бази даних) для спрощення реалізації.
У застосунку має бути окремий модуль (бібліотека) для обчислення статистики за набором чисел. Модуль статистики повинен бути відокремлений від основного проєкту веб-сервісу.
Сервіс повинен підтримувати просту авторизацію (наприклад, Basic Authentication).

Примітка
Тестові облікові дані:
Логін: q
Пароль: 1


Test HTTP service for storing and receiving numbers. May contains a few bugs =)

Description
The application is a web service that processes GET and POST requests for retrieving and updating a list of numbers.

A file-based storage may be used instead of a database in order to simplify the implementation.
The application shall include a separate module responsible for calculating statistics on the dataset. This module must be isolated from the main service project.
The service shall implement basic authentication.
The application must correctly handle concurrent requests.

Note
Test user credentials:
Username: q
Password: 1
