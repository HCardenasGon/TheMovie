using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PL.Controllers
{
    public class PeliculaController : Controller
    {
        [HttpGet]
        public ActionResult GetPopular(string page = "1")
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Peliculas = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzM1ZWE5ZTA1NzUzNjg1NWRlOGQyMWZkYmI5NWQxMyIsInN1YiI6IjY0NzBlNzNhNzcwNzAwMDEzNjdmMzFiYiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.UoDAmGct2bxmzgxiUlLLUNrQ6FCR-EofpaOPA2_joko");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = client.GetAsync("popular?language=en-US&page=" + page);

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<dynamic>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Pelicula ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Pelicula>(resultItem.ToString());
                        pelicula.Peliculas.Add(ResultItemList);
                    }
                }
            }
            return View(pelicula);
        }

        [HttpGet]
        public ActionResult FavouriteAdd(int id)
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Peliculas = new List<object>();

            using (var client = new HttpClient())
            {
                var data = new Dictionary<string, dynamic>()
                {
                    ["media_type"] = "movie",
                    ["media_id"] = id,
                    ["favorite"] = true,
                };
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19684223/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzM1ZWE5ZTA1NzUzNjg1NWRlOGQyMWZkYmI5NWQxMyIsInN1YiI6IjY0NzBlNzNhNzcwNzAwMDEzNjdmMzFiYiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.UoDAmGct2bxmzgxiUlLLUNrQ6FCR-EofpaOPA2_joko");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postTask = client.PostAsJsonAsync<dynamic>("favorite", data);

                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Pelicula agregada a favoritos";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al agregar";
                    return PartialView("Modal");
                }
            }
        }

        public ActionResult GetFavourite()
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Peliculas = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19684223/favorite/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzM1ZWE5ZTA1NzUzNjg1NWRlOGQyMWZkYmI5NWQxMyIsInN1YiI6IjY0NzBlNzNhNzcwNzAwMDEzNjdmMzFiYiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.UoDAmGct2bxmzgxiUlLLUNrQ6FCR-EofpaOPA2_joko");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = client.GetAsync("movies?language=en-US&page=1&sort_by=created_at.asc");

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<dynamic>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Pelicula ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Pelicula>(resultItem.ToString());
                        pelicula.Peliculas.Add(ResultItemList);
                    }
                }
            }
            return View(pelicula);
        }

        [HttpGet]
        public ActionResult FavouriteDelete(int id)
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Peliculas = new List<object>();

            using (var client = new HttpClient())
            {
                var data = new Dictionary<string, dynamic>()
                {
                    ["media_type"] = "movie",
                    ["media_id"] = id,
                    ["favorite"] = false,
                };

                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19684223/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzM1ZWE5ZTA1NzUzNjg1NWRlOGQyMWZkYmI5NWQxMyIsInN1YiI6IjY0NzBlNzNhNzcwNzAwMDEzNjdmMzFiYiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.UoDAmGct2bxmzgxiUlLLUNrQ6FCR-EofpaOPA2_joko");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postTask = client.PostAsJsonAsync<dynamic>("favorite?session_id=12b95bb6445507a8962392287c5825d109f39133 ", data);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Pelicula eliminda de favoritos";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al eliminar";
                    return PartialView("Modal");
                }
            }
        }
    }
}
