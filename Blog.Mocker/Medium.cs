using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Blog.API.Services;
using Blog.Mocker.Abstraction;
using Blog.Model;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Blog.Mocker
{
  public class Medium : IMocksPacker
  {
    const string FEEDS_URL = "https://medium.com/feed/";
    const string BASIC_CONTENT = "{\"object\":\"value\",\"document\":{\"object\":\"document\",\"data\":{},\"nodes\":[{\"object\":\"block\",\"type\":\"heading-two\",\"data\":{},\"nodes\":[{\"object\":\"text\",\"leaves\":[{\"object\":\"leaf\",\"text\": \"TITLE\",\"marks\":[]}]}]},{\"object\":\"block\",\"type\":\"paragraph\",\"data\":{},\"nodes\":[{\"object\":\"text\",\"leaves\":[{\"object\":\"leaf\",\"text\":\"\",\"marks\":[]}]}]}]}}";
    public async Task<Pack> GetPack(List<string> usernames)
    {
      var client = new HttpClient();
      var authService = new AuthService(null, 0);
      var users = usernames.Select(username => new User
        {
          Id = username,
          Username = username,
          Email = username + "@mail.com",
          Password = authService.HashPassword(username + username),
        }
      ).ToList();

      var urls = usernames.Select(username => FEEDS_URL + "@" + username);
      var feedStrings = await Task.WhenAll(
        urls.Select(async url => await client.GetStringAsync(url))
      );
      var storiesList = await Task.WhenAll(
        feedStrings.Select(async (feedString, feedIndex) => {
          using(var xmlReader = XmlReader.Create(new StringReader(feedString)))
          {
            var stories = new List<Story>();
            var feedReader = new RssFeedReader(xmlReader);
            while (await feedReader.Read())
            {
              var type = feedReader.ElementType;
              if (type == SyndicationElementType.Item)
              {
                var item = await feedReader.ReadItem();   
                
                var time = item.Published.ToUnixTimeSeconds(); 
                var story = new Story
                {
                  Id = Guid.NewGuid().ToString(),
                  Title = item.Title,
                  Content = BASIC_CONTENT.Replace("TITLE", item.Title),
                  Tags = item.Categories.Select(c => c.Name).ToList(),
                  CreationTime = time,
                  LastEditTime = time,
                  PublishTime = time,
                  Draft = false,
                  OwnerId = users[feedIndex].Id
                };
                stories.Add(story);
              }
            }
            return stories;
          }
        })
      );
      var pack = new Pack
      {
        Users = users,
        Stories = storiesList.SelectMany(s => s).ToList()
      };

      return pack; 
    }
  }
}