cd Blog.API
rm -rf ./Migrations
dotnet ef migrations add InitialMigration
dotnet ef database update

cd ..
