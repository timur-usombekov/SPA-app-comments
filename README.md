# SPA-app-comments
Проект представляет собой веб-приложение, разработанное с использованием ASP.NET(ASP.NET Web API) и React.
## Возможности приложения

- Добавление комментариев: Пользователь может добавить новыё комментарий(запись), указывая свое имя, email, свой веб-сайт, текст комментария, загружая изображения или текстовые файлы.

- Captcha: Для добавления комментария необходимо ввести капчу.

- Сортировка комментариев: Комментарии могут быть отсортированы по имени автора, адресу электронной почты или дате добавления. Возможны сортировки как по возрастанию, так и по убыванию. Сортировка по умолчанию - LIFO. 

- Взаимодействие с комментариями: Пользователи могут нажать на кнопку **Watch replies**, что бы просмотреть ответы, либо на **Reply** что бы ответить на него.

- Пагинация: Если на странице количество комментариев превысит 25, остальные будут отображатся на следующей странице.

- Загрузка изображений и файлов: Пользователи могут загрузить изображение или текстовый файл к своему комментарию.
  >Изображение должно быть не более 320х240 пикселей, при попытке залить изображение большего размера, картинка пропорционально уменьшается до заданных размеров.
  >Текстовый файл должен иметь расширение **.txt** и быть не более 100 Кб.

## Запуск локально

Клонируйте репозиторий

```bash
  git clone https://github.com/timur-usombekov/SPA-app-comments
```

Перейдите в директорию проекта

```bash
  cd путь/к/проекту
```

Убедитесь что у вас установлен **Docker** и постройте образ

```bash
  docker-compose build
```

После успешого завершения, запустите его 

```bash
  docker-compose up -d
```

После упешного запуска перейдите в директорию c **ui**

```bash
  cd ui
```

Выполните команду для создания image и запустите его

```bash
  docker build -t имя_образа .
  docker run -d -p 8003:80 имя_образа
```
В случае если всё прошло успешно перейдите по адресу http://localhost:8003, вы должны увидеть приблизительно следующий результат
  >Комментарий был оставлен заранее для демонстрации, вы его не увидите

![image](https://github.com/user-attachments/assets/34a4e08d-4001-4cb5-bdc8-2637eff02c43)
