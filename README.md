# Bogoda API

Добро пожаловать в **Bogoda API**! Этот API предназначен для управления заведениями, категориями и тегами. Он предоставляет конечные точки для создания, чтения, обновления и удаления сущностей.

## Содержание

- [Функции](#функции)
- [Начало работы](#начало-работы)
- [Конечные точки API](#конечные-точки-api)
  - [Заведения](#заведения)
  - [Теги](#теги)
  - [Категории](#категории)
- [Модели данных](#модели-данных)
- [Технологии](#технологии)
- [Участие](#участие)
- [Лицензия](#лицензия)

## Функции

- **Управление заведениями**: Создание, обновление, удаление и поиск заведений по имени, категории или тегу.
- **Управление категориями**: Создание, обновление, удаление и поиск категорий по имени.
- **Управление тегами**: Создание, обновление, удаление и поиск тегов по имени.

## Начало работы

Чтобы начать работу с Bogoda API, выполните следующие шаги:

### Требования

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [PostgreSQL](https://www.postgresql.org/download/) или совместимый провайдер базы данных

### Установка

1. Клонируйте репозиторий:

    ```bash
    git clone https://github.com/Skalersaas/Bogoda.git
    ```

2. Перейдите в директорию проекта:

    ```bash
    cd bogoda-api
    ```

3. Настройте строку подключения в `appsettings.json`. Замените `DefaultConnection` на строку подключения к вашей базе данных PostgreSQL:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=your_host;Database=your_database;Username=your_username;Password=your_password;"
      }
    }
    ```

4. Восстановите зависимости:

    ```bash
    dotnet restore
    ```

5. Выполните миграции для создания базы данных:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

6. Запустите приложение:

    ```bash
    dotnet run
    ```

7. Теперь API должно работать на `http://localhost:5000`.

## Конечные точки API

### Заведения

- **Создание заведения**
  - `POST /venue/create`
  - Параметры: `name`, `address`, `categoryId`, `description`
  - Возвращает: `VenueDTO` или ошибку `NotFound`, если категория не найдена.

- **Получить все заведения**
  - `GET /venue/all`
  - Возвращает: Список всех заведений.

- **Получить заведение по имени**
  - `GET /venue/name/{name}`
  - Параметры: `name` - имя заведения.
  - Возвращает: Список заведений, содержащих в имени указанный текст.

- **Получить заведение по ID**
  - `GET /venue/id/{Id}`
  - Параметры: `Id` - ID заведения.
  - Возвращает: Заведение с указанным ID.

- **Получить заведения по тегу**
  - `GET /venue/tag/{tagId}`
  - Параметры: `tagId` - ID тега.
  - Возвращает: Список заведений, связанных с указанным тегом.

- **Получить заведения по категории**
  - `GET /venue/category/{categoryId}`
  - Параметры: `categoryId` - ID категории.
  - Возвращает: Список заведений, относящихся к указанной категории.

- **Обновить заведение**
  - `PUT /venue/update`
  - Параметры: `Id`, `name`, `address`, `categoryId`, `description`
  - Возвращает: Обновленное заведение или ошибку `NotFound`, если заведение не найдено.

- **Добавить тег к заведению**
  - `PATCH /venue/addtag`
  - Параметры: `venueId`, `tagId`
  - Возвращает: Обновленное заведение или ошибку `NotFound`, если заведение или тег не найдены.

- **Удалить тег из заведения**
  - `PATCH /venue/removetag`
  - Параметры: `venueId`, `tagId`
  - Возвращает: Обновленное заведение или ошибку `NotFound`, если заведение или тег не найдены.

- **Удалить заведение**
  - `DELETE /venue/delete`
  - Параметры: `Id`
  - Возвращает: Сообщение о том, что заведение было удалено, или ошибку `NotFound`, если заведение не найдено.

### Теги

- **Создание тега**
  - `POST /tag/create`
  - Параметры: `name`, `description`
  - Возвращает: `TagDTO`

- **Получить все теги**
  - `GET /tag/all`
  - Возвращает: Список всех тегов.

- **Получить тег по имени**
  - `GET /tag/name/{name}`
  - Параметры: `name` - имя тега.
  - Возвращает: Список тегов, содержащих указанный текст в имени.

- **Получить тег по ID**
  - `GET /tag/id/{Id}`
  - Параметры: `Id` - ID тега.
  - Возвращает: Тег с указанным ID.

- **Обновить тег**
  - `PUT /tag/update`
  - Параметры: `Id`, `name`, `description`
  - Возвращает: Обновленный тег или ошибку `NotFound`, если тег не найден.

- **Удалить тег**
  - `DELETE /tag/delete`
  - Параметры: `Id`
  - Возвращает: Сообщение о том, что тег был удален, или ошибку `NotFound`, если тег не найден.

### Категории

- **Создание категории**
  - `POST /category/create`
  - Параметры: `name`, `description`
  - Возвращает: `CategoryDTO`

- **Получить все категории**
  - `GET /category/all`
  - Возвращает: Список всех категорий.

- **Получить категорию по имени**
  - `GET /category/name/{name}`
  - Параметры: `name` - имя категории.
  - Возвращает: Список категорий, содержащих указанный текст в имени.

- **Получить категорию по ID**
  - `GET /category/id/{Id}`
  - Параметры: `Id` - ID категории.
  - Возвращает: Категорию с указанным ID.

- **Обновить категорию**
  - `PUT /category/update`
  - Параметры: `Id`, `name`, `description`
  - Возвращает: Обновленную категорию или ошибку `NotFound`, если категория не найдена.

- **Удалить категорию**
  - `DELETE /category/delete`
  - Параметры: `Id`
  - Возвращает: Сообщение о том, что категория была удалена, или ошибку `NotFound`, если категория не найдена.

## Модели данных

### Venue (Заведение)

- **Id**: int
- **Name**: string
- **Address**: string
- **CategoryId**: int
- **Description**: string (необязательно)
- **Category**: Category
- **Tags**: List<Tag>

### Tag (Тег)

- **Id**: int
- **Name**: string
- **Description**: string (необязательно)
- **Venues**: List<Venue>

### Category (Категория)

- **Id**: int
- **Name**: string
- **Description**: string (необязательно)
- **Venues**: List<Venue>

## Технологии

- **.NET 6**: Фреймворк, используемый для создания API.
- **Entity Framework Core**: ORM для взаимодействия с базой данных.
- **PostgreSQL**: Сервер базы данных.

## Участие

Приглашаем к участию! Пожалуйста, отправляйте пулл-реквесты с вашими изменениями. Для вопросов или запросов на новые функции откройте issue в репозитории.

## Лицензия

Этот проект лицензируется под лицензией MIT - см. файл [LICENSE](LICENSE) для подробностей.
