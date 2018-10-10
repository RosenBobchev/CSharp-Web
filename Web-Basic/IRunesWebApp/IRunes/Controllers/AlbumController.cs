using IRunes.Extensions;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Linq;
using System.Text;

namespace IRunes.Controllers
{
    public class AlbumController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            return this.View("/Album/Create");
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            string name = request.FormData["name"].ToString();
            string cover = request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(cover))
            {
                return this.View("/album/create");
            }

            var user = context.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return this.View("User/Login");
            }

            user.Albums.Add(new Album { Name = name, Cover = cover });
            this.context.SaveChanges();

            var response = All(request);
            return response;
        }

        public IHttpResponse All(IHttpRequest request)
        {
            this.ViewBag["albums"] = "There are currently no albums.";

            string albumsParameters = null;

            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            var user = this.context.Users.Include(x => x.Albums).FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return this.View("User/Login");
            }

            var albums = user.Albums.ToArray();

            foreach (var album in albums)
            {
                albumsParameters += $"<a href=\"/album/details?id={album.Id}\">{album.Name}</a></li><br/>";
            }

            if (albumsParameters != null)
            {
                this.ViewBag["albums"] = albumsParameters;
            }
            

            return this.View("/Album/All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            var albumId = request.QueryData["id"].ToString();

            var album = this.context.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);
            string albumCover = album.Cover.UrlDecode();

            var trackPrice = album.Tracks.Sum(x => x.Price);
            var totalPrice = trackPrice - (trackPrice * 13 / 100);

            var albumData = new StringBuilder();

            albumData.Append($"<img src=\"{albumCover}\" width=\"250\" height=\"250\"><br/>");
            albumData.Append($"<b>Name: {album.Name}</b><br/>");
            albumData.Append($"<b>Price: ${totalPrice}</b><br/>");

            var albumTracks = album.Tracks.ToArray();

            var tracks = new StringBuilder();

            this.ViewBag["tracks"] = string.Empty;

            if (albumTracks.Length > 0)
            {
                foreach (var track in albumTracks)
                {
                    tracks.Append($"<a href=\"/track/details?id={track.Id}&albumId={albumId}\">{track.Name}</a></li><br/>");
                }

                this.ViewBag["tracks"] = tracks.ToString();
            }

            this.ViewBag["albumId"] = album.Id;
            this.ViewBag["album"] = albumData.ToString();

            return this.View("Album/Details");
        }
    }
}