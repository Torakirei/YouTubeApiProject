using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeApiProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;

        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"];
        }

        public async Task<(List<YouTubeVideoModel>, string, string)> SearchVideosAsync(string query, string pageToken = "")
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.Q = query;
                searchRequest.MaxResults = 10;
                searchRequest.PageToken = pageToken;

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id.VideoId // Get the VideoId for the clickable link
                }).ToList();

                return (videos, searchResponse.NextPageToken, searchResponse.PrevPageToken);
            }
            catch (Exception ex)
            {
                // Log the exception (if needed)
                return (new List<YouTubeVideoModel>(), null, null);
            }
        }

        public async Task<List<YouTubeVideoModel>> GetTrendingVideosAsync()
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var trendingRequest = youtubeService.Videos.List("snippet");
                trendingRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
                trendingRequest.MaxResults = 10; // You can adjust this number as needed

                var trendingResponse = await trendingRequest.ExecuteAsync();

                var trendingVideos = trendingResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id // Directly access the video ID from the video object
                }).ToList();

                return trendingVideos;
            }
            catch (Exception ex)
            {
                // Log the exception (if needed)
                return new List<YouTubeVideoModel>(); // Return an empty list in case of an error
            }
        }
    }
}
