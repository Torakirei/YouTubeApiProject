using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTubeApiProject.Controllers
{
    public class TrendingController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public TrendingController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        // GET: Trending
        public async Task<IActionResult> Index()
        {
            // Fetch trending videos from the YouTube API
            var trendingVideos = await _youtubeService.GetTrendingVideosAsync();

            // Pass the list of trending videos to the view
            return View(trendingVideos);
        }
    }
}
