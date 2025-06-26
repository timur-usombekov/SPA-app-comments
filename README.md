# SPA-app-comments
This project is a web application built using ASP.NET Web API for the backend and React for the frontend.
## Features

- Add Comments: Users can add new comments (entries) by providing their name, email, website (optional), comment text, and by uploading images or text files.

- Captcha: A CAPTCHA is required to submit a comment.

- Sort Comments: Comments can be sorted by author's name, email, or submission date. Both ascending and descending order are supported. The default sorting order is LIFO (Last-In, First-Out).

- Interact with Comments: Users can click the Watch replies button to view responses to a comment, or the Reply button to respond to it.

- Pagination: If the number of comments on a page exceeds 25, the remaining comments will be displayed on subsequent pages.

- Image and File Uploads: Users can upload an image or a text file with their comment. Image previews include visual effects.
  >Images should be no larger than 320x240 pixels. If a larger image is uploaded, it will be proportionally resized to fit these dimensions.

  >Text files must have a .txt extension and be no larger than 100 KB.

## Local Setup

Clone the repository:

```bash
  git clone https://github.com/timur-usombekov/SPA-app-comments
```

Navigate to the project directory:

```bash
  cd путь/к/проекту
```

Ensure Docker is installed and then build the Docker images:

```bash
  docker-compose build
```

Once the build is successful, run the services:

```bash
  docker-compose up -d
```

After successful startup, navigate into the **ui** directory:

```bash
  cd ui
```

Build the UI Docker image and run it:

```bash
  docker build -t имя_образа .
  docker run -d -p 8003:80 имя_образа
```
If everything runs successfully, you should be able to access the application at http://localhost:8003
  >Note: The initial comment shown in the demonstration was added beforehand and will not be present when you first run the application.

![image](https://github.com/user-attachments/assets/34a4e08d-4001-4cb5-bdc8-2637eff02c43)
