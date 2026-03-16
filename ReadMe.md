Chạy migration và cập nhật DB

```

dotnet ef database update --project Infrastructure --startup-project MyWebApplication



```
Add migration
```

dotnet ef migrations add [context] --project Infrastructure --startup-project MyWebApplication

```