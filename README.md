# simple-blog-back
[simple-blog-front repo](https://github.com/RodionChachura/simple-blog-front)

>

![all text](https://cdn-images-1.medium.com/max/800/1*MDXR5eddScIqHYop-IL9sg.png)

## Requirements:
 - .NET Core 2+
 - PostgreSQL(connection string specified in Blog.API/appsettings.json)

## Example of how to create a database user
```bash
sudo -i -u postgres
psql
create user blogadmin;
alter user blogadmin with password 'blogadmin';
alter user blogadmin createdb;
```

## Run locally
```bash
git clone https://github.com/RodionChachura/simple-blog-back
cd simple-blog-back
cd Blog.API
dotnet ef database update
# if you want to populate the database with mock data
# start
cd ..
cd Blog.Mocker
dotnet run
cd ..
cd Blog.API
# end
dotnet run 
```

## [Story on Medium](https://medium.com/@geekrodion/blog-with-asp-net-core-and-react-redux-c80857b93cb6)

## License

MIT © [RodionChachura](https://geekrodion.com)
