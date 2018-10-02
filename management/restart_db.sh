cd Blog.API
dotnet ef database drop -f
rm -rf ./Migrations
dotnet ef migrations add InitialMigration
dotnet ef database update

cd ..
