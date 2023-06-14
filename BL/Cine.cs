using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cine
    {
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}',  '{cine.Direccion}', {cine.Zona.IdZona} , '{cine.Ventas}'");

                    if (query > 0)
                    {
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

        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine}, '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Ventas}");

                    if (query > 0)
                    {
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

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Cines.FromSqlRaw($"CineGetAll").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        int? count = 0;
                        string srt;
                        foreach (var row in query)
                        {
                            count += row.Ventas;
                            result.sum = count;
                        }
                        foreach (var row in query)
                        {
                            ML.Cine cine = new ML.Cine();

                            cine.IdCine = row.IdCine;
                            cine.Nombre = row.Nombre;
                            cine.Direccion = row.Direccion;
                            cine.Ventas = row.Ventas;

                            cine.Zona = new ML.Zona();
                            cine.Zona.IdZona = row.IdZona;
                            cine.Zona.Nombre = row.NombreZona;
                            srt = count.ToString().Remove(2);
                            cine.Ventas = row.Ventas / Convert.ToInt32(srt);

                            result.Objects.Add(cine);
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
        public static ML.Result GetById(int idCine)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Cines.FromSqlRaw($"CineById {idCine}").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.IdCine = query.IdCine;
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Ventas = query.Ventas;

                        cine.Zona = new ML.Zona();
                        cine.Zona.IdZona = query.IdZona;
                        cine.Zona.Nombre = query.NombreZona;

                        result.Object = cine;

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

        public static ML.Result SumByDirrecion()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Cines.FromSqlRaw($"CineGetAll").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        int? count = 0;
                        int? countN = 0;
                        int? countS = 0;
                        int? countE = 0;
                        int? counto = 0;

                        string srt;
                        foreach (var row in query)
                        {
                            count += row.Ventas;
                            result.sum = count;
                        }

                        foreach (var row in query)
                        {
                            if (row.IdZona == 1)
                            {
                                countN += row.Ventas;
                                result.N = countN;
                            }
                            if (row.IdZona == 2)
                            {
                                countS += row.Ventas;
                                result.S = countS;
                            }
                            if (row.IdZona == 3)
                            {
                                countE += row.Ventas;
                                result.E = countE;
                            }
                            if (row.IdZona == 4)
                            {
                                counto += row.Ventas;
                                result.O = counto;
                            }
                        }


                        srt = count.ToString().Remove(2);

                        ML.Cine cineN = new ML.Cine();
                        cineN.Zona = new ML.Zona();
                        cineN.Ventas = result.N / Convert.ToInt32(srt);
                        cineN.Zona.Nombre = "Norte";
                        result.Objects.Add(cineN);

                        ML.Cine cineS = new ML.Cine();
                        cineS.Zona = new ML.Zona();
                        cineS.Ventas = result.S / Convert.ToInt32(srt);
                        cineS.Zona.Nombre = "Sur";
                        result.Objects.Add(cineS);

                        ML.Cine cineE = new ML.Cine();
                        cineE.Zona = new ML.Zona();
                        cineE.Ventas = result.E / Convert.ToInt32(srt);
                        cineE.Zona.Nombre = "Este";
                        result.Objects.Add(cineE);

                        ML.Cine cineO = new ML.Cine();
                        cineO.Zona = new ML.Zona();
                        cineO.Ventas = result.O / Convert.ToInt32(srt);
                        cineO.Zona.Nombre = "Oeste";
                        result.Objects.Add(cineO);

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
