# Datalk


<br>

> Note: Still pre-release (Version 0.1.0)
    There are a lot of updates coming soon!

<br>

## A- How to use
<br>


### 1- Clone the project
```
git clone https://github.com/kevorkkeheian/Datalk.git
```

<br>

### 2- Go to the Server path
```
cd src/Server
```

### 3- Add the migrations
```
dotnet ef migrations add -c DatalkContext Initial
```

<br>

### 4- Update the databse

> Note: You can chagne the Connection String from appsetting.json

```
dotnet ef database update -c DatalkContext
```

### 5- Run

```
dotnet run
```

## B - Publish to Azure Web App

- Publish the web app locally

```
dotnet publish -c release
```

- Go to Published folder in `Server`
- It should be in: [`src\Server\bin\release\netcoreapp3.1\publish`]
- Right Click on the "`publish`" folder anc click **Deploy to Web App**
