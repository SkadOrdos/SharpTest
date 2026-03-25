Приложение представляет из себя веб сервис который обрабатывает GET/POST запросы для получения и обновления списка чисел. В качестве хранилища разрешено использовать файл на диске вместо базы данных для упрощения логики. В приложении должен быть отдельный модуль для расчетов статистики по набору чисел отдельно от главного проекта сервиса. Сервис содержит простую авторизацию. Приложение должно корректно обрабатывать параллельные запросы.

Примечание: (Пароль тестового пользователя "q": 1)



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
