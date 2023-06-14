using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            ML.Result result = BL.Cine.GetAll();

            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta";
            }
            
            return View(cine);
        }

        public JsonResult GetAllJson()
        {
            ML.Cine cine = new ML.Cine();
            ML.Result result = BL.Cine.GetAll();

            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta";
            }

            return Json(cine);
        }

        public JsonResult GetByIdJson(int? idCine)
        {
            ML.Cine cine = new Cine();
            ML.Result resultCine = BL.Cine.GetById(idCine.Value);

            if (resultCine.Correct)
            {
                cine = (ML.Cine)resultCine.Object;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta  " + resultCine.ErrorMessage;
            }
            return Json(cine);
        }

        [HttpGet]
        public IActionResult ShowMap(int? idCine)
        { 
            ML.Cine cine = new ML.Cine();
            
            ML.Result resultZona = BL.Zona.GetAll();
            cine.Zona = new ML.Zona();

            if (resultZona.Correct)
            {
                cine.Zona.Zonas = resultZona.Objects;
            }
            if (idCine == null)
            {
                return View(cine);
            }
            else
            {
                ML.Result resultCine = BL.Cine.GetById(idCine.Value);

                if (resultCine.Correct)
                {
                    cine = (ML.Cine)resultCine.Object;
                    cine.Zona.Zonas = resultZona.Objects;
                    return View(cine);
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al hacer la consulta  " + resultCine.ErrorMessage;
                    return View("Modal");
                }
            }
        }

        [HttpPost]
        public IActionResult ShowMap(ML.Cine cine)
        {
            ML.Result result = new ML.Result();

            if (cine.IdCine == 0)
            {
                result = BL.Cine.Add(cine);

                if (result.Correct)
                {
                    ViewBag.Message = "Registro correctamente insertado";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al insertar el registro";
                }
            }
            else
            {
                result = BL.Cine.Update(cine);
                if (result.Correct)
                {
                    ViewBag.Message = "Registro correctamente actualizado";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al actualizar el registro";
                }
            }
            return View("Modal");
        }

        public IActionResult Graphs()
        {
            ML.Cine cine = new ML.Cine();
            ML.Result result = BL.Cine.SumByDirrecion();

            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta";
            }

            return View(cine);
        }
    }
}
