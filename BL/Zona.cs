using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Zona
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Zonas.FromSqlRaw($"ZonaGetAll").ToList();

                    result.Objects = new List<object>();

                    if (query.Count > 0)
                    {
                        foreach (var row in query)
                        {
                            ML.Zona zona = new ML.Zona();

                            zona.IdZona = row.IdZona;
                            zona.Nombre = row.Nombre;

                            result.Objects.Add(zona);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }
    }
}
