ѕриложение представл€ет из себ€ веб сервис который обрабатывает GET/POST запросы дл€ получени€ и обновлени€ списка чисел. ¬ качестве хранилища разрешено использовать файл на диске вместо базы данных дл€ упрощени€ логики. ¬ приложении должен быть отдельный модуль дл€ расчетов статистики по набору чисел отдельно от главного проекта сервиса. —ервис содержит простую авторизацию. ѕриложение должно корректно обрабатывать параллельные запросы.

ѕримечание: (ѕароль тестового пользовател€ "q": 1)



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